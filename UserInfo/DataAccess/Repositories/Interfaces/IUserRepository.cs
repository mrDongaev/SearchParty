using DataAccess.Entities;
using Library.Repositories.Interfaces;

namespace DataAccess.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User, Guid>
    {
    }
}
