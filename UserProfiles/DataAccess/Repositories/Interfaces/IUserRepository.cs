using DataAccess.Entities;

namespace DataAccess.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User, Guid>, IRangeGettable<User, Guid>
    {
    }
}
