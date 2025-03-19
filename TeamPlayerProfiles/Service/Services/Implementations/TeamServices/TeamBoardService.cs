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
using Library.Results.Successes.Messages;
using Library.Services.Interfaces.UserContextInterfaces;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Service.Contracts.Team;
using Service.Services.Interfaces.MessageInterfaces;
using Service.Services.Interfaces.TeamInterfaces;
using Service.Services.Utils;

namespace Service.Services.Implementations.TeamServices
{
    public class TeamBoardService(IMapper mapper, ITeamRepository teamRepo, IUserHttpContext userContext, IServiceProvider provider) : ITeamBoardService
    {
        public async Task<Result<TeamDto?>> SetDisplayed(Guid id, bool displayed, CancellationToken cancellationToken = default)
        {
            var team = new Team() { Id = id, Displayed = displayed };
            var updatedTeam = await teamRepo.Update(team, null, cancellationToken);

            if (updatedTeam == null)
            {
                return Result.Fail<TeamDto?>(new EntityNotFoundError("Team with the given ID has not been found"));
            }

            return Result.Ok(mapper.Map<TeamDto?>(updatedTeam));
        }

        public async Task<Result> SendTeamApplicationRequest(Guid playerId, Guid teamId, int positionId, CancellationToken cancellationToken = default)
        {
            if (!Enum.IsDefined(typeof(PositionName), positionId))
            {
                throw new InvalidEnumMemberException(positionId.ToString(), typeof(PositionName).Name);
            }
            using var scope = provider.CreateScope();
            var teamApplicationService = scope.ServiceProvider.GetRequiredService<ITeamApplicationService>();
            teamApplicationService.AccessToken = userContext.AccessToken;
            teamApplicationService.RefreshToken = userContext.RefreshToken;
            var playerRepo = scope.ServiceProvider.GetRequiredService<IPlayerRepository>();

            var playerUserId = await playerRepo.GetProfileUserId(playerId, cancellationToken);
            if (playerUserId == null || playerUserId != userContext.UserId)
            {
                return Result.Fail(new UnauthorizedError());

            }
            var teamUserId = await teamRepo.GetProfileUserId(teamId, cancellationToken);
            if (teamUserId == null)
            {
                return Result.Fail(new EntityNotFoundError("User associated with the given team profile has not been found"));
            }


            var message = new ProfileMessageSubmitted()
            {
                SenderId = playerId,
                SendingUserId = userContext.UserId,
                AcceptorId = teamId,
                AcceptingUserId = (Guid)teamUserId,
                PositionName = (PositionName)positionId,
                MessageType = MessageType.TeamApplication,
            };
            var messageResult = await teamApplicationService.GetUserMessages(new HashSet<MessageStatus> { MessageStatus.Pending }, cancellationToken);
            if (messageResult.IsFailed) return messageResult.ToResult();
            var teamPlayers = await teamRepo.GetTeamPlayers(teamId, cancellationToken);
            var validationResult = MessageValidation.ValidateApplication(teamUserId.Value, messageResult.Value, teamPlayers, message);
            if (validationResult.IsSuccess)
            {
                var sender = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                await sender.Publish(message, cancellationToken);
                validationResult.WithSuccess(new MessageSentSuccess("Team application has been sent successfully"));
            }
            return validationResult;
        }

        public async Task<Result<ICollection<TeamDto>>> GetFiltered(ConditionalTeamQuery query, CancellationToken cancellationToken = default)
        {
            var teams = await teamRepo.GetConditionalTeamRange(query, cancellationToken);

            if (teams.Count == 0)
            {
                return Result.Fail<ICollection<TeamDto>>(new EntitiesForQueryNotFoundError("Teams matching given filtering query have not been found"));
            }

            return Result.Ok(mapper.Map<ICollection<TeamDto>>(teams));
        }

        public async Task<Result<PaginatedResult<TeamDto>>> GetPaginated(ConditionalTeamQuery query, uint page, uint pageSize, CancellationToken cancellationToken = default)
        {
            var teams = await teamRepo.GetPaginatedTeamRange(query, page, pageSize, cancellationToken);
            var result = mapper.Map<PaginatedResult<TeamDto>>(teams);

            if (teams.Total == 0)
            {
                return Result.Fail<PaginatedResult<TeamDto>>(new EntitiesForQueryNotFoundError("Teams matching given filtering query have not been found"));
            }

            return Result.Ok(result);
        }
    }
}
