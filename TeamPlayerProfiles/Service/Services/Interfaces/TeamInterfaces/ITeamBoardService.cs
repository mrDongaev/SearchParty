using Service.Contracts.Team;
using Service.Services.Interfaces.Common;
using static Common.Models.ConditionalQuery;

namespace Service.Services.Interfaces.TeamInterfaces
{
    public interface ITeamBoardService : IBoardService<TeamDto, TeamConditions>
    {
        Task SendTeamAccessionRequest(Guid teamId, Guid requestingPlayerId, CancellationToken cancellationToken);
    }
}
