using AutoMapper;
using Common.Models;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using FluentResults;
using Library.Models;
using Library.Models.API.UserMessaging;
using Library.Models.Enums;
using Library.Results.Errors.EntityRequest;
using Library.Results.Successes.Message;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Service.Contracts.Team;
using Service.Services.Interfaces.MessageInterfaces;
using Service.Services.Interfaces.TeamInterfaces;
using Service.Services.Utils;

namespace Service.Services.Implementations.TeamServices
{
    public class TeamBoardService(IMapper mapper, ITeamRepository teamRepo, ITeamApplicationService teamApplicationService, IServiceProvider provider) : ITeamBoardService
    {
        public async Task<Result<TeamDto?>> SetDisplayed(Guid id, bool displayed, CancellationToken cancellationToken = default)
        {
            var team = new Team() { Id = id, Displayed = displayed };
            var updatedTeam = await teamRepo.Update(team, null, cancellationToken);

            if (updatedTeam == null)
            {
                return Result.Fail<TeamDto?>(new EntityNotFoundError("Team with the given ID has not been found")).WithValue(null);
            }

            return Result.Ok(mapper.Map<TeamDto?>(updatedTeam));
        }

        public async Task<Result> SendTeamApplicationRequest(ProfileMessageSubmitted message, CancellationToken cancellationToken = default)
        {
            using var scope = provider.CreateScope();
            var messages = await teamApplicationService.GetUserMessages(new HashSet<MessageStatus> { MessageStatus.Pending }, cancellationToken);
            var teamPlayers = await teamRepo.GetTeamPlayers(message.AcceptorId, cancellationToken);
            var validationResult = MessageValidation.ValidateApplication(messages, teamPlayers, message);
            if (validationResult.IsSuccess)
            {
                var sender = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                await sender.Publish(message, cancellationToken);
                validationResult.WithSuccess(new MessageSentSuccess(message.MessageType));
            }
            return validationResult;
        }

        public async Task<Result<ICollection<TeamDto>>> GetFiltered(ConditionalTeamQuery query, CancellationToken cancellationToken = default)
        {
            var teams = await teamRepo.GetConditionalTeamRange(query, cancellationToken);

            if (teams.Count == 0)
            {
                return Result.Fail<ICollection<TeamDto>>(new EntityFilteredRangeNotFoundError("Teams matching given filtering query have not been found")).WithValue([]);
            }

            return Result.Ok(mapper.Map<ICollection<TeamDto>>(teams));
        }

        public async Task<Result<PaginatedResult<TeamDto>>> GetPaginated(ConditionalTeamQuery query, uint page, uint pageSize, CancellationToken cancellationToken = default)
        {
            var teams = await teamRepo.GetPaginatedTeamRange(query, page, pageSize, cancellationToken);
            var result = mapper.Map<PaginatedResult<TeamDto>>(teams);

            if (teams.Total == 0)
            {
                return Result.Fail<PaginatedResult<TeamDto>>(new EntityFilteredRangeNotFoundError("Teams matching given filtering query have not been found")).WithValue(result);
            }

            return Result.Ok(result);
        }
    }
}
