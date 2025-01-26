using AutoMapper;
using Common.Models;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using FluentResults;
using Library.Exceptions;
using Library.Models;
using Library.Models.API.UserMessaging;
using Library.Models.Enums;
using Library.Results.Errors.Authorization;
using Library.Results.Errors.EntityRequest;
using Library.Results.Successes.Message;
using Library.Services.Interfaces.UserContextInterfaces;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Service.Contracts.Player;
using Service.Services.Interfaces.MessageInterfaces;
using Service.Services.Interfaces.PlayerInterfaces;
using Service.Services.Utils;

namespace Service.Services.Implementations.PlayerServices
{
    public class PlayerBoardService(IMapper mapper, IPlayerRepository playerRepo, IUserHttpContext userContext, IServiceProvider provider) : IPlayerBoardService
    {
        public async Task<Result<PlayerDto?>> SetDisplayed(Guid id, bool displayed, CancellationToken cancellationToken = default)
        {
            var userId = await playerRepo.GetProfileUserId(id, cancellationToken);
            if (userId == null || userId != userContext.UserId)
            {
                return Result.Fail<PlayerDto?>(new UnauthorizedError()).WithValue(null);
            }
            var player = new Player() { Id = id, Displayed = displayed };
            var updatedPlayer = await playerRepo.Update(player, null, cancellationToken);
            if (updatedPlayer == null)
            {
                return Result.Fail<PlayerDto?>(new EntityNotFoundError("Player with the given ID has not been found")).WithValue(null);
            }

            return Result.Ok(mapper.Map<PlayerDto?>(updatedPlayer));
        }

        public async Task<Result> InvitePlayerToTeam(Guid playerId, Guid teamId, int positionId, CancellationToken cancellationToken = default)
        {
            if (!Enum.IsDefined(typeof(PositionName), positionId))
            {
                throw new InvalidEnumMemberException(positionId.ToString(), typeof(PositionName).Name);
            }
            using var scope = provider.CreateScope();
            var playerInvitationService = scope.ServiceProvider.GetRequiredService<IPlayerInvitationService>();
            playerInvitationService.AccessToken = userContext.AccessToken;
            playerInvitationService.RefreshToken = userContext.RefreshToken;
            var teamRepo = scope.ServiceProvider.GetRequiredService<ITeamRepository>();

            var teamUserId = await teamRepo.GetProfileUserId(teamId, cancellationToken);
            if (teamUserId == null || teamUserId != userContext.UserId)
            {
                return Result.Fail(new UnauthorizedError());
            }
            var playerUserId = await playerRepo.GetProfileUserId(playerId, cancellationToken);
            if (playerUserId == null)
            {
                return Result.Fail(new EntityNotFoundError("User associated with the given player profile has not been found"));
            }

            var message = new ProfileMessageSubmitted()
            {
                SenderId = teamId,
                SendingUserId = userContext.UserId,
                AcceptorId = playerId,
                AcceptingUserId = (Guid)playerUserId,
                PositionName = (PositionName)positionId,
                MessageType = MessageType.PlayerInvitation,
            };
            var messageResult = await playerInvitationService.GetUserMessages(new HashSet<MessageStatus> { MessageStatus.Pending }, cancellationToken);
            if (messageResult.IsFailed) return messageResult.ToResult();
            var teamPlayers = await teamRepo.GetTeamPlayers(teamId, cancellationToken);
            var validationResult = MessageValidation.ValidateInvitation(teamUserId.Value, messageResult.Value, teamPlayers, message);
            if (validationResult.IsSuccess)
            {
                var sender = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                await sender.Publish(message, cancellationToken);
                validationResult.WithSuccess(new MessageSentSuccess(message.MessageType));
            }
            return validationResult;
        }

        public async Task<Result<ICollection<PlayerDto>>> GetFiltered(ConditionalPlayerQuery query, CancellationToken cancellationToken = default)
        {
            var players = await playerRepo.GetConditionalPlayerRange(query, cancellationToken);

            players = players.Where(p => p.UserId == userContext.UserId || (p.Displayed.HasValue && p.Displayed.Value)).ToList();

            if (players.Count == 0)
            {
                Result.Fail<ICollection<PlayerDto>>(new EntityFilteredRangeNotFoundError("Players matching given filtering query have not been been found")).WithValue([]);
            }

            return Result.Ok(mapper.Map<ICollection<PlayerDto>>(players));
        }

        public async Task<Result<PaginatedResult<PlayerDto>>> GetPaginated(ConditionalPlayerQuery query, uint page, uint pageSize, CancellationToken cancellationToken = default)
        {
            var players = await playerRepo.GetPaginatedPlayerRange(query, page, pageSize, cancellationToken);

            players.List = players.List.Where(p => p.UserId == userContext.UserId || (p.Displayed.HasValue && p.Displayed.Value)).ToList();

            var result = mapper.Map<PaginatedResult<PlayerDto>>(players);

            if (result.Total == 0)
            {
                return Result.Fail<PaginatedResult<PlayerDto>>(new EntityFilteredRangeNotFoundError("Players matching given filtering query have not been been found")).WithValue(result);
            }

            return Result.Ok(result);
        }
    }
}
