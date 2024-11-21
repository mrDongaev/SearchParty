using Library.Models.API.UserMessaging;
using Library.Services.Interfaces.UserContextInterfaces;
using Service.Services.Interfaces.MessageInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

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
    }
}
