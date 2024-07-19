using DataAccess.Entities.Interfaces;

namespace DataAccess.Repositories.Interfaces
{
    public interface IProfileRepository<TProfile> : IRepository<TProfile, Guid>
        where TProfile : class, IProfile
    {
    }
}
