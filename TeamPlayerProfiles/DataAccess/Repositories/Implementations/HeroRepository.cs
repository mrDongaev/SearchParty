using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Library.Repositories.Implementations;

namespace DataAccess.Repositories.Implementations
{
    public class HeroRepository(TeamPlayerProfilesContext context) : Repository<TeamPlayerProfilesContext, Hero, int>(context), IHeroRepository
    {
    }
}
