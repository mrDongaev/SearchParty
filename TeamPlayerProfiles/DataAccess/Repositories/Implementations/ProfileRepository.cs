using DataAccess.Context;
using DataAccess.Entities.Interfaces;
using DataAccess.Repositories.Interfaces;

namespace DataAccess.Repositories.Implementations
{
    public abstract class ProfileRepository<TProfile>(TeamPlayerProfilesContext context) : Repository<TProfile, Guid>(context), IProfileRepository<TProfile> where TProfile : class, IProfile
    {
        public override async Task<TProfile> Add(TProfile profile, CancellationToken cancellationToken)
        {
            profile.Id = Guid.NewGuid();
            profile.Displayed = false;
            profile.UpdatedAt = DateTime.UtcNow;
            return await base.Add(profile, cancellationToken);
        }
    }
}
