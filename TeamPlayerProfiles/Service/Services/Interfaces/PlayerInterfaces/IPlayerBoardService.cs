using Common.Models;
using Service.Contracts.Player;
using Service.Services.Interfaces.Common;

namespace Service.Services.Interfaces.PlayerInterfaces
{
    public interface IPlayerBoardService : IBoardService<PlayerDto, ConditionalPlayerQuery>
    {
        Task InvitePlayerToTeam(Guid playerId, Guid invitingTeamId, CancellationToken cancellationToken);
    }
}
