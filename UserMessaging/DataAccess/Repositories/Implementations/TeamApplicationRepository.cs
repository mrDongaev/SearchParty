using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Library.Repositories.Implementations;

namespace DataAccess.Repositories.Implementations
{
    public class TeamApplicationRepository(UserMessagingContext context) : Repository<UserMessagingContext, TeamApplication, Guid>(context), ITeamApplicationRepository
    {
    }
}
