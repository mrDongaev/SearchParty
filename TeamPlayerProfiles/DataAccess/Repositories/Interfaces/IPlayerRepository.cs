using Common.Models;
using DataAccess.Entities;
using Library.Entities.Interfaces;
using Library.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IPlayerRepository : IProfileRepository<Player>, IRangeGettable<Player, Guid>
    {
        Task<Player?> UpdatePlayerHeroes(Guid id, ISet<int> heroIds, CancellationToken cancellationToken);

        Task<ICollection<Player>> GetConditionalPlayerRange(ConditionalPlayerQuery query, CancellationToken cancellationToken);

        Task<PaginatedResult<Player>> GetPaginatedPlayerRange(ConditionalPlayerQuery config, uint page, uint pageSize, CancellationToken cancellationToken);
    }
}
