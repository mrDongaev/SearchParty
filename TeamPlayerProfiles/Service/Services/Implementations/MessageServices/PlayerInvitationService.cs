using Library.Models.API.UserMessaging;
using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Service.Services.Interfaces.MessageInterfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Service.Services.Implementations.MessageServices
{
    public class PlayerInvitationService : IPlayerInvitationService
    {
        private readonly HttpClient _httpClient;

        public string AccessToken
        {
            set
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", value);
            }
        }

        public string RefreshToken
        {
            set
            {
                _httpClient.DefaultRequestHeaders.Add("Cookie", $"RefreshToken={value}");
            }
        }

        public PlayerInvitationService(HttpClient httpClient, IUserHttpContext userContext)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userContext.AccessToken);
            _httpClient.DefaultRequestHeaders.Add("Cookie", $"RefreshToken={userContext.RefreshToken}");
        }

        public async Task<GetPlayerInvitation.Response?> Get(Guid id, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.GetAsync($"/api/PlayerInvitation/Get/{id}", cancellationToken);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<GetPlayerInvitation.Response>() : null;
        }

        public async Task<ICollection<GetPlayerInvitation.Response>?> GetUserMessages(ISet<MessageStatus> messageStatuses, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.PostAsJsonAsync($"/api/PlayerInvitation/GetUserMessages", messageStatuses, cancellationToken);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<ICollection<GetPlayerInvitation.Response>>() : null;
        }
    }
}
