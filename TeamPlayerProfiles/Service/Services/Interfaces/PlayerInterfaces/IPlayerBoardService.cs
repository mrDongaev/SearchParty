using Service.Contracts.Player;
using Service.Services.Interfaces.Common;
using static Common.Models.ConditionalQuery;

namespace Service.Services.Interfaces.PlayerInterfaces
{
    public interface IPlayerBoardService : IBoardService<PlayerDto, PlayerConditions>
    {
        Task InvitePlayerToTeam(Guid playerId, Guid invitingTeamId, CancellationToken cancellationToken);
    }
}
