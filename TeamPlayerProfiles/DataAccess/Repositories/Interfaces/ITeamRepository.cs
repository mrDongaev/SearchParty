using DataAccess.Entities;
using DataAccess.Repositories.Models;
using static Common.Models.ConditionalQuery;

namespace DataAccess.Repositories.Interfaces
{
    public interface ITeamRepository : IProfileRepository<Team>
    {
        Task<Team?> UpdateTeamPlayers(Guid id, ISet<TeamPlayer> players, CancellationToken cancellationToken);

        Task<ICollection<Team>> GetConditionalTeamRange(TeamConditions config, CancellationToken cancellationToken);

        Task<PaginatedResult<Team>> GetPaginatedTeamRange(TeamConditions config, uint page, uint pageSize, CancellationToken cancellationToken);
    }
}
