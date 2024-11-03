using Library.Models.API.UserProfiles.User;
using Service.Services.Interfaces.UserProfilesInterfaces;
using System.Net.Http.Json;

namespace Service.Services.Implementations.UserProfileServices
{
    public class UserProfilesService : IUserProfileService
    {
        private HttpClient _httpClient;

        public UserProfilesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<GetUser.Response> Create(CreateUser.Request request, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.PostAsJsonAsync(FromUser(nameof(Create)), request, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GetUser.Response>(cancellationToken);
        }

        public async Task<bool> Delete(Guid id, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.DeleteAsync(FromUser(nameof(Delete), id.ToString()), cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>(cancellationToken);
        }

        public async Task<GetUser.Response> Get(Guid id, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.GetAsync(FromUser(nameof(Get), id.ToString()), cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GetUser.Response>(cancellationToken);
        }

        public async Task<ICollection<GetUser.Response>> GetAll(CancellationToken cancellationToken)
        {
            using var response = await _httpClient.GetAsync(FromUser(nameof(GetAll)), cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ICollection<GetUser.Response>>(cancellationToken);
        }

        public async Task<ICollection<GetUser.Response>> GetFiltered(GetConditionalUser.Request request, CancellationToken cancellationToken)
        {
            using JsonContent jsonContent = JsonContent.Create(request);
            using var response = await _httpClient.PostAsync(FromUser(nameof(GetFiltered)), jsonContent, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ICollection<GetUser.Response>>();
        }

        public async Task<ICollection<GetUser.Response>> GetRange(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            using JsonContent jsonContent = JsonContent.Create(ids);
            using var response = await _httpClient.PostAsync(FromUser(nameof(GetRange)), jsonContent, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ICollection<GetUser.Response>>();
        }

        public async Task<GetUser.Response> Update(Guid id, UpdateUser.Request request, CancellationToken cancellationToken)
        {
            using JsonContent jsonContent = JsonContent.Create(request);
            using var response = await _httpClient.PostAsync(FromUser(nameof(Update), id.ToString()), jsonContent, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GetUser.Response>(cancellationToken);
        }

        private string FromUser(string method, string requestPath = "")
        {
            if (!string.IsNullOrEmpty(requestPath))
            {
                requestPath = "/" + requestPath;
            }
            return $"/api/User/{method}{requestPath}/";
        }
    }
}
