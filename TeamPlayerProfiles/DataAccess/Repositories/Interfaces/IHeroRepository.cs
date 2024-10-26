using DataAccess.Entities;
using Library.Entities.Interfaces;
using Library.Repositories.Interfaces;

namespace DataAccess.Repositories.Interfaces
{
    public interface IHeroRepository : IRepository<Hero, int>, IRangeGettable<Hero, int>
    {
    }
}
