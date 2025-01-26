using Common.Models;
using FluentResults;
using Library.Models.API.UserMessaging;
using Service.Contracts.Team;
using Service.Services.Interfaces.Common;

namespace Service.Services.Interfaces.TeamInterfaces
{
    public interface ITeamBoardService : IBoardService<TeamDto, ConditionalTeamQuery>
    {
        Task<Result> SendTeamApplicationRequest(Guid playerId, Guid teamId, int positionId, CancellationToken cancellationToken);
    }
}
