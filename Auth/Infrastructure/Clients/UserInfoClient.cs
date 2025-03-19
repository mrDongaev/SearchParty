using Application.Interfaces;
using Application.User.ExternalModels;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Clients
{
    public class UserInfoClient : IUserInfoClient
    {
        private readonly HttpClient _httpClient;

        public UserInfoClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> CreateUserInfoAsync(CreateUserInfoRequest createUserInfoRequest, string accessToken, string refreshToken)
        {
            // Установка заголовка авторизации
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Add("Cookie", $"RefreshToken={refreshToken}");

            var json = JsonSerializer.Serialize(createUserInfoRequest);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/user/create", content);

            return response;
        }
    }
}
