using FluentResults;
using Library.Models.API.UserProfiles.User;
using Library.Services.Interfaces.UserContextInterfaces;
using Library.Utils;
using Service.Services.Interfaces.UserProfilesInterfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Service.Services.Implementations.UserProfileServices
{
    public class UserProfilesService : IUserProfileService
    {
        private readonly HttpClient _httpClient;

        public UserProfilesService(HttpClient httpClient, IUserHttpContext userContext)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userContext.AccessToken);
            _httpClient.DefaultRequestHeaders.Add("Cookie", $"RefreshToken={userContext.RefreshToken}");
        }
        public async Task<Result<GetUser.Response?>> Create(CreateUser.Request request, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.PostAsJsonAsync($"/api/User/Create/", request, cancellationToken);

            return await response.ReadResultFromJsonResponse<GetUser.Response?>("Failed to create user profile", cancellationToken);
        }

        public async Task<Result<bool?>> Delete(Guid id, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.DeleteAsync($"/api/User/Delete/{id}/", cancellationToken);

            return await response.ReadResultFromJsonResponse<bool?>("Failed to delete user profile", cancellationToken);
        }

        public async Task<Result<GetUser.Response?>> Get(Guid id, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.GetAsync($"/api/User/Get/{id}/", cancellationToken);

            return await response.ReadResultFromJsonResponse<GetUser.Response?>("Failed to get user profile", cancellationToken);
        }

        public async Task<Result<ICollection<GetUser.Response>?>> GetAll(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"/api/User/GetAll/", cancellationToken);

            return await response.ReadResultFromJsonResponse<ICollection<GetUser.Response>?>("Failed to get user profile list", cancellationToken);
        }

        public async Task<Result<ICollection<GetUser.Response>?>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.PostAsJsonAsync($"/api/User/GetRange/", ids, cancellationToken);

            return await response.ReadResultFromJsonResponse<ICollection<GetUser.Response>?>("Failed to get user profile range", cancellationToken);
        }

        public async Task<Result<GetUser.Response?>> Update(Guid id, UpdateUser.Request request, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.PostAsJsonAsync($"/api/User/Update/{id}/", request, cancellationToken);

            return await response.ReadResultFromJsonResponse<GetUser.Response?>("Failed to update user profile", cancellationToken);
        }
    }
}
