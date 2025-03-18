using FluentResults;
using Library.Models.API.TeamPlayerProfiles.Team;
using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;

namespace Service.Services.Interfaces.TeamInterfaces
{
    public interface ITeamService
    {
        public IUserHttpContext? UserContext { get; set; }

        Task<Result<GetTeam.Response?>> PushInvitedPlayerToTeam(Guid teamId, Guid playerId, PositionName position, Guid messageId, CancellationToken cancellationToken);

        Task<Result<GetTeam.Response?>> PushApplyingPlayerToTeam(Guid teamId, Guid playerId, PositionName position, Guid messageId, CancellationToken cancellationToken);

        Task<Result<GetTeam.Response?>> PushPlayerToTeam(Guid teamId, Guid playerId, PositionName position, Guid messageid, MessageType messageType, CancellationToken cancellationToken);
    }
}
