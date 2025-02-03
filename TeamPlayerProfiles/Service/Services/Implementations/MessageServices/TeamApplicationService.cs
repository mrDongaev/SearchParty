using FluentResults;
using Library.Models.API.UserMessaging;
using Library.Models.Enums;
using Library.Models.HttpResponses;
using Library.Results.Errors.EntityRequest;
using Library.Results.Errors.Http;
using Library.Services.Interfaces.UserContextInterfaces;
using Service.Services.Interfaces.MessageInterfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Service.Services.Implementations.MessageServices
{
    public class TeamApplicationService : ITeamApplicationService
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

        public TeamApplicationService(HttpClient httpClient, IUserHttpContext userContext)
        {
            _httpClient = httpClient;
            if (!string.IsNullOrEmpty(userContext.AccessToken)) _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userContext.AccessToken);
            if (!string.IsNullOrEmpty(userContext.RefreshToken)) _httpClient.DefaultRequestHeaders.Add("Cookie", $"RefreshToken={userContext.RefreshToken}");
        }

        public async Task<Result<GetTeamApplication.Response?>> Get(Guid id, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.GetAsync($"/api/TeamApplication/Get/{id}", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadFromJsonAsync<HttpResponseBody<GetTeamApplication.Response?>>(cancellationToken);
                if (body != null) return body.MapToResult();
            }

            return Result.Fail<GetTeamApplication.Response?>(new HttpRequestFailedError("Failed to get messages of the user")).WithValue(null);
        }

        public async Task<Result<ICollection<GetTeamApplication.Response>?>> GetUserMessages(ISet<MessageStatus> messageStatuses, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.PostAsJsonAsync($"/api/TeamApplication/GetUserMessages", messageStatuses, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadFromJsonAsync<HttpResponseBody<ICollection<GetTeamApplication.Response>?>>(cancellationToken);
                if (body != null) return body.MapToResult();
            }

            return Result.Fail<ICollection<GetTeamApplication.Response>?>(new HttpRequestFailedError("Failed to get messages of the user")).WithValue([]);
        }
    }
}
