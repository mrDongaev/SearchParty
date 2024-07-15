using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class PlayerRepository(DbContext context) : Repository<Player, Guid>(context), IPlayerRepository
    {

    }
}
