using Common.Models;
using Service.Contracts.Team;
using Service.Services.Interfaces.Common;

namespace Service.Services.Interfaces.TeamInterfaces
{
    public interface ITeamBoardService : IBoardService<TeamDto, ConditionalTeamQuery>
    {
        Task SendTeamApplicationRequest(Guid teamId, Guid requestingPlayerId, CancellationToken cancellationToken);
    }
}
