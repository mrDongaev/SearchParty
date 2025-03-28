﻿using FluentResults;
using Library.Models.API.TeamPlayerProfiles.Team;
using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;
using Library.Utils;
using Service.Services.Interfaces.TeamInterfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Service.Services.Implementations.TeamServices
{
    public class TeamService : ITeamService
    {
        private readonly HttpClient _httpClient;

        private IUserHttpContext? _userContext;

        public IUserHttpContext? UserContext
        {
            get => _userContext;
            set
            {
                _userContext = value;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userContext?.AccessToken ?? "");
                _httpClient.DefaultRequestHeaders.Add("Cookie", $"RefreshToken={_userContext?.RefreshToken ?? ""}");
            }
        }

        public TeamService(HttpClient httpClient, IUserHttpContext userContext)
        {
            _httpClient = httpClient;
            UserContext = userContext;
        }

        public async Task<Result<GetTeam.Response?>> PushApplyingPlayerToTeam(Guid teamId, Guid playerId, PositionName position, Guid messageId, CancellationToken cancellationToken)
        {
            return await PushPlayerToTeam(teamId, playerId, position, messageId, MessageType.TeamApplication, cancellationToken);
        }

        public async Task<Result<GetTeam.Response?>> PushInvitedPlayerToTeam(Guid teamId, Guid playerId, PositionName position, Guid messageId, CancellationToken cancellationToken)
        {
            return await PushPlayerToTeam(teamId, playerId, position, messageId, MessageType.PlayerInvitation, cancellationToken);
        }

        public async Task<Result<GetTeam.Response?>> PushPlayerToTeam(Guid teamId, Guid playerId, PositionName position, Guid messageid, MessageType messageType, CancellationToken cancellationToken)
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
            return await response.ReadResultFromJsonResponse<GetTeam.Response?>("Could not push player to the team", cancellationToken);
        }
    }
}
