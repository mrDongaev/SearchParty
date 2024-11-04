using Library.Models.API.UserProfiles.User;

namespace Service.Services.Interfaces.UserProfilesInterfaces
{
    public interface IUserProfileService
    {
        Task<GetUser.Response> Get(Guid id, CancellationToken cancellationToken);

        Task<ICollection<GetUser.Response>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken);

        Task<ICollection<GetUser.Response>> GetAll(CancellationToken cancellationToken);

        Task<GetUser.Response> Create(CreateUser.Request request, CancellationToken cancellationToken);

        Task<GetUser.Response> Update(Guid id, UpdateUser.Request request, CancellationToken cancellationToken);

        Task<bool> Delete(Guid id, CancellationToken cancellationToken);
    }
}
