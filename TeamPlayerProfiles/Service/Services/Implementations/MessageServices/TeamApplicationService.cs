using Library.Models.API.UserMessaging;
using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;
using Service.Services.Interfaces.MessageInterfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Service.Services.Implementations.MessageServices
{
    public class TeamApplicationService : ITeamApplicationService
    {
        private readonly HttpClient _httpClient;

        public TeamApplicationService(HttpClient httpClient, IUserHttpContext userContext)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userContext.AccessToken);
            _httpClient.DefaultRequestHeaders.Add("Cookie", $"RefreshToken={userContext.RefreshToken}");
        }

        public async Task<GetTeamApplication.Response?> Get(Guid id, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.GetAsync($"/api/TeamApplication/Get/{id}", cancellationToken);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<GetTeamApplication.Response>() : null;
        }

        public async Task<ICollection<GetTeamApplication.Response>?> GetUserMessages(ISet<MessageStatus> messageStatuses, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.PostAsJsonAsync($"/api/TeamApplication/GetUserMessages", messageStatuses, cancellationToken);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<ICollection<GetTeamApplication.Response>>() : null;
        }
    }
}
