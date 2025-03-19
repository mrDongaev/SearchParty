using Application.User.ExternalModels;

namespace Application.Interfaces
{
    public interface IUserInfoClient
    {
        Task<HttpResponseMessage> CreateUserInfoAsync(CreateUserInfoRequest createUserInfoRequest, string accessToken, string refreshToken);
    }
}
