using FluentResults;
using Library.Models.API.UserProfiles.User;

namespace Service.Services.Interfaces.UserProfilesInterfaces
{
    public interface IUserProfileService
    {
        Task<Result<GetUser.Response?>> Get(Guid id, CancellationToken cancellationToken);

        Task<Result<ICollection<GetUser.Response>?>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken);

        Task<Result<ICollection<GetUser.Response>?>> GetAll(CancellationToken cancellationToken);

        Task<Result<GetUser.Response?>> Create(CreateUser.Request request, CancellationToken cancellationToken);

        Task<Result<GetUser.Response?>> Update(Guid id, UpdateUser.Request request, CancellationToken cancellationToken);

        Task<Result<bool?>> Delete(Guid id, CancellationToken cancellationToken);
    }
}
