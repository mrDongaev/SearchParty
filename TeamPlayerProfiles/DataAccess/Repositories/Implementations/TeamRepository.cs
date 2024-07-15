using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class TeamRepository(DbContext context) : Repository<Team, Guid>(context), ITeamRepository
    {

    }
}
