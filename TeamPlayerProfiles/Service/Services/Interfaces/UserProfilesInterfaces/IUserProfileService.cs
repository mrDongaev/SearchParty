using Library.Models.API.UserProfiles.User;

namespace Service.Services.Interfaces.UserProfilesInterfaces
{
    public interface IUserProfileService
    {
        Task Get(Guid id, CancellationToken cancellationToken);

        Task GetRange(ICollection<Guid> ids, CancellationToken cancellationToken);

        Task GetAll(CancellationToken cancellationToken);

        Task Create(CreateUser.Request request, CancellationToken cancellationToken);

        Task Update(Guid id, UpdateUser.Request request, CancellationToken cancellationToken);

        Task Delete(Guid id, CancellationToken cancellationToken);

        Task GetFiltered(GetConditionalUser.Request request, CancellationToken cancellationToken);
    }
}
