using DataAccess.Entities;
using Library.Models;
using Library.Repositories.Interfaces;
using static Common.Models.ConditionalProfileQuery;

namespace DataAccess.Repositories.Interfaces
{
    public interface IPlayerRepository : IProfileRepository<Player>, IRangeGettable<Player, Guid>
    {
        Task<Player?> UpdatePlayerHeroes(Guid id, ISet<int> heroIds, CancellationToken cancellationToken);

        Task<ICollection<Player>> GetConditionalPlayerRange(PlayerConditions query, CancellationToken cancellationToken);

        Task<PaginatedResult<Player>> GetPaginatedPlayerRange(PlayerConditions config, uint page, uint pageSize, CancellationToken cancellationToken);
    }
}
