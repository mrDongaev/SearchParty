using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Library.Repositories.Implementations;

namespace DataAccess.Repositories.Implementations
{
    public class PositionRepository(TeamPlayerProfilesContext context) : Repository<TeamPlayerProfilesContext, Position, int>(context), IPositionRepository
    {
    }
}
