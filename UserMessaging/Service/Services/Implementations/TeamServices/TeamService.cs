using Library.Models.API.TeamPlayerProfiles.Team;
using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.AspNetCore.Http;
using Service.Services.Interfaces.TeamInterfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Service.Services.Implementations.TeamServices
{
    public class TeamService : ITeamService
    {
        private readonly HttpClient _httpClient;

        public TeamService(HttpClient httpClient, IUserHttpContext userContext)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userContext.AccessToken);
            _httpClient.DefaultRequestHeaders.Add("Cookie", $"RefreshToken={userContext.RefreshToken}");
        }

        public async Task<bool> PushApplyingPlayerToTeam(Guid teamId, Guid playerId, PositionName position, Guid messageId, CancellationToken cancellationToken)
        {
            return await PushPlayerToTeam(teamId, playerId, position, messageId, MessageType.TeamApplication, cancellationToken);
        }

        public async Task<bool> PushInvitedPlayerToTeam(Guid teamId, Guid playerId, PositionName position, Guid messageId, CancellationToken cancellationToken)
        {
            return await PushPlayerToTeam(teamId, playerId, position, messageId, MessageType.PlayerInvitation, cancellationToken);
        }

        public async Task<bool> PushPlayerToTeam(Guid teamId, Guid playerId, PositionName position, Guid messageid, MessageType messageType, CancellationToken cancellationToken)
        {
            PushPlayer.Request request = new PushPlayer.Request()
            {
                PlayerId = playerId,
                TeamId = teamId,
                MessageId = messageid,
                MessageType = messageType,
                Position = position,
            };
            using var response = await _httpClient.PostAsJsonAsync("/api/Team/PushPlayerToTeam", request, cancellationToken);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<bool>() : false;
        }
    }
}
