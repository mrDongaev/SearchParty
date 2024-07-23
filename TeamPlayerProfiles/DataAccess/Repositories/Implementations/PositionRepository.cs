using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;

namespace DataAccess.Repositories.Implementations
{
    public class PositionRepository(TeamPlayerProfilesContext context) : Repository<Position, int>(context), IPositionRepository
    {

    }
}
