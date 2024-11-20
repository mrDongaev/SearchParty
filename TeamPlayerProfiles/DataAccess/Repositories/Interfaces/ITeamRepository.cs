using Common.Models;
using DataAccess.Entities;
using Library.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface ITeamRepository : IProfileRepository<Team>
    {
        Task<Team?> Update(Team team, ISet<TeamPlayer>? teamPlayers, CancellationToken cancellationToken);

        Task<ICollection<TeamPlayer>?> GetTeamPlayers(Guid teamId,  CancellationToken cancellationToken);

        Task<ICollection<Team>> GetConditionalTeamRange(ConditionalTeamQuery config, CancellationToken cancellationToken);

        Task<PaginatedResult<Team>> GetPaginatedTeamRange(ConditionalTeamQuery config, uint page, uint pageSize, CancellationToken cancellationToken);
    }
}
