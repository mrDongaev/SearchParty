using DataAccess.Entities;

namespace DataAccess.Repositories.Interfaces
{
    public interface IPlayerRepository : IProfileRepository<Player>, IRangeGettable<Player, Guid>
    {
    }
}
