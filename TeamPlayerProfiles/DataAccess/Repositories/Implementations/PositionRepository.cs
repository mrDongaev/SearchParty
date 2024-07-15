using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class PositionRepository(DbContext context) : Repository<Position, int>(context), IPositionRepository
    {

    }
}
