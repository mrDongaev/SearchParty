using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Library.Repositories.Implementations;

namespace DataAccess.Repositories.Implementations
{
    public class UserRepository(TeamPlayerProfilesContext context) : Repository<TeamPlayerProfilesContext, User, Guid>(context), IUserRepository
    {
    }
}
