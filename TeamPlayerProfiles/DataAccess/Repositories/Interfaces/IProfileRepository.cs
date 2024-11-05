using DataAccess.Entities.Interfaces;
using Library.Repositories.Interfaces;

namespace DataAccess.Repositories.Interfaces
{
    public interface IProfileRepository<TProfile> : IRepository<TProfile, Guid>
        where TProfile : class, IProfile
    {
    }
}
