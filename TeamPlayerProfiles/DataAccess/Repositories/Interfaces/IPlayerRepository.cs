using DataAccess.Entities;
using DataAccess.Repositories.Models;
using static Common.Models.ConditionalQuery;

namespace DataAccess.Repositories.Interfaces
{
    public interface IPlayerRepository : IProfileRepository<Player>, IRangeGettable<Player, Guid>
    {
        Task<Player?> UpdatePlayerHeroes(Guid id, ISet<int> heroIds, CancellationToken cancellationToken);

        Task<ICollection<Player>> GetConditionalPlayerRange(PlayerConditions query, CancellationToken cancellationToken);

        Task<PaginatedResult<Player>> GetPaginatedPlayerRange(PlayerConditions config, int page, int pageSize, CancellationToken cancellationToken);
    }
}
