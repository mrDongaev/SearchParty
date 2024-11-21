using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;

namespace Service.Services.Interfaces.TeamInterfaces
{
    public interface ITeamService
    {
        public IUserHttpContext? UserContext { get; set; }

        Task<bool> PushInvitedPlayerToTeam(Guid teamId, Guid playerId, PositionName position, Guid messageId, CancellationToken cancellationToken);

        Task<bool> PushApplyingPlayerToTeam(Guid teamId, Guid playerId, PositionName position, Guid messageId, CancellationToken cancellationToken);

        Task<bool> PushPlayerToTeam(Guid teamId, Guid playerId, PositionName position, Guid messageid, MessageType messageType, CancellationToken cancellationToken);
    }
}
