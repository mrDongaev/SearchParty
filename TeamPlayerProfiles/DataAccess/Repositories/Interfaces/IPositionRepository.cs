using DataAccess.Entities;

namespace DataAccess.Repositories.Interfaces
{
    public interface IPositionRepository : IRepository<Position, int>, IRangeGettable<Position, int>
    {
    }
}
