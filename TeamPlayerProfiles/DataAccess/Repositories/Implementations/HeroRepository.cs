using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class HeroRepository(TeamPlayerProfilesContext context) : Repository<Hero, int>(context), IHeroRepository
    {
        private readonly DbSet<Hero> _heroSet = context.Heroes;

        public async Task<ICollection<Hero>> GetRange(ICollection<int> ids, CancellationToken cancellationToken)
        {
            return await _heroSet
                .AsNoTracking()
                .Where(h => ids.Contains(h.Id))
                .ToListAsync(cancellationToken);
        }
    }
}
