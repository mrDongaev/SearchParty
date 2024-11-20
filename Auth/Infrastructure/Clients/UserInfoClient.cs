using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.User.ExternalModels;
using Azure.Core;

namespace Infrastructure.Clients
{
    public class UserInfoClient : IUserInfoClient
    {
        private readonly HttpClient _httpClient;

        public UserInfoClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> CreateUserInfoAsync(CreateUserInfoRequest createUserInfoRequest, string accessToken)
        {
            // Установка заголовка авторизации
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var json = JsonSerializer.Serialize(createUserInfoRequest);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/user/create", content);

            return response;
        }
    }
}
