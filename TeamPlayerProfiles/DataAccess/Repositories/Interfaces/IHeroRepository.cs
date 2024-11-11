using DataAccess.Entities;
using Library.Repositories.Interfaces;

namespace DataAccess.Repositories.Interfaces
{
    public interface IHeroRepository : IRepository<Hero, int>
    {
    }
}
