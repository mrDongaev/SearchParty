using DataAccess.Entities;

namespace DataAccess.Repositories.Interfaces
{
    public interface ITeamRepository : IProfileRepository<Team>
    {
        Task<Team?> UpdateTeamPlayers(Guid id, ISet<TeamPlayer> players, CancellationToken cancellationToken);
    }
}
