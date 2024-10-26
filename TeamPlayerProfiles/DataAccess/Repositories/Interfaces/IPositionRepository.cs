using DataAccess.Entities;
using Library.Repositories.Interfaces;

namespace DataAccess.Repositories.Interfaces
{
    public interface IPositionRepository : IRepository<Position, int>, IRangeGettable<Position, int>
    {
    }
}
