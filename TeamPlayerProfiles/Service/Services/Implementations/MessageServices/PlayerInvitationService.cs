using FluentResults;
using Library.Models.API.UserMessaging;
using Library.Models.Enums;
using Library.Models.HttpResponses;
using Library.Results.Errors.EntityRequest;
using Library.Results.Errors.Http;
using Library.Services.Interfaces.UserContextInterfaces;
using Library.Utils;
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

        public async Task<Result<GetPlayerInvitation.Response?>> Get(Guid id, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.GetAsync($"/api/PlayerInvitation/Get/{id}", cancellationToken);

            return await response.ReadResultFromJsonResponse<GetPlayerInvitation.Response?>("HTTP request to get player invitation failed", cancellationToken);
        }

        public async Task<Result<ICollection<GetPlayerInvitation.Response>?>> GetUserMessages(ISet<MessageStatus> messageStatuses, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.PostAsJsonAsync($"/api/PlayerInvitation/GetUserMessages", messageStatuses, cancellationToken);

            return await response.ReadResultFromJsonResponse<ICollection<GetPlayerInvitation.Response>?>("HTTP request to get player invitations of the user failed", cancellationToken);
        }
    }
}
