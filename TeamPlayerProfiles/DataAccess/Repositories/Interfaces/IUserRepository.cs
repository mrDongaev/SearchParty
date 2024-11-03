using Common.Models;
using DataAccess.Entities;
using Library.Entities.Interfaces;
using Library.Repositories.Interfaces;

namespace DataAccess.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User, Guid>, IRangeGettable<User, Guid>
    {
    }
}
