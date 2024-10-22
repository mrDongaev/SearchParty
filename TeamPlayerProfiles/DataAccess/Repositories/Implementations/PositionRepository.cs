using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class PositionRepository : Repository<Position, int>, IPositionRepository
    {
        private readonly DbSet<Position> _positions;

        public PositionRepository(TeamPlayerProfilesContext context) : base(context)
        {
            _positions = _context.Positions;
        }

        public async Task<ICollection<Position>> GetRange(ICollection<int> ids, CancellationToken cancellationToken)
        {
            return await _positions
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
