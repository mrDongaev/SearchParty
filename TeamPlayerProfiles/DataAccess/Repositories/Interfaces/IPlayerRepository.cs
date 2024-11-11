using Common.Models;
using DataAccess.Entities;
using Library.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IPlayerRepository : IProfileRepository<Player>
    {
        Task<Player?> Update(Player player, ISet<int>? heroIds, CancellationToken cancellationToken);

        Task<ICollection<Player>> GetConditionalPlayerRange(ConditionalPlayerQuery query, CancellationToken cancellationToken);

        Task<PaginatedResult<Player>> GetPaginatedPlayerRange(ConditionalPlayerQuery config, uint page, uint pageSize, CancellationToken cancellationToken);
    }
}
