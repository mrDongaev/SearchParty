using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class HeroRepository(DbContext context) : Repository<Hero, int>(context), IHeroRepository
    {

    }
}
