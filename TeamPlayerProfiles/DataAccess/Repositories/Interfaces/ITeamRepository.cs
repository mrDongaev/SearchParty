using DataAccess.Entities;
using Library.Models;
using static Common.Models.ConditionalProfileQuery;

namespace DataAccess.Repositories.Interfaces
{
    public interface ITeamRepository : IProfileRepository<Team>
    {
        Task<Team?> UpdateTeamPlayers(Guid id, ISet<TeamPlayer> players, CancellationToken cancellationToken);

        Task<ICollection<Team>> GetConditionalTeamRange(TeamConditions config, CancellationToken cancellationToken);

        Task<PaginatedResult<Team>> GetPaginatedTeamRange(TeamConditions config, uint page, uint pageSize, CancellationToken cancellationToken);
    }
}
