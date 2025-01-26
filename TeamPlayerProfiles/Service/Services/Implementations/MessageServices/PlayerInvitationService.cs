using FluentResults;
using Library.Models.API.UserMessaging;
using Library.Models.Enums;
using Library.Results.Errors.EntityRequest;
using Library.Services.Interfaces.UserContextInterfaces;
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

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<GetPlayerInvitation.Response>(cancellationToken);
                return Result.Ok(data);
            }

            return Result.Fail<GetPlayerInvitation.Response?>(new EntityNotFoundError("Failed to get messages of the user")).WithValue(null);
        }

        public async Task<Result<ICollection<GetPlayerInvitation.Response>>> GetUserMessages(ISet<MessageStatus> messageStatuses, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.PostAsJsonAsync($"/api/PlayerInvitation/GetUserMessages", messageStatuses, cancellationToken);
            var data = await response.Content.ReadFromJsonAsync<ICollection<GetPlayerInvitation.Response>>(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return Result.Ok(data ?? []);
            }

            return Result.Fail<ICollection<GetPlayerInvitation.Response>>("Failed to get messages of the user").WithValue([]);
        }
    }
}
