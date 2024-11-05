using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Library.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class UserRepository : Repository<TeamPlayerProfilesContext, User, Guid>, IUserRepository
    {
        private readonly DbSet<User> _users;

        public UserRepository(TeamPlayerProfilesContext context) : base(context)
        {
            _users = context.Users;
        }

        public async Task<ICollection<User>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            return await _users.AsNoTracking()
                .Where(u => ids.Contains(u.Id))
                .ToListAsync(cancellationToken);
        }
    }
}
