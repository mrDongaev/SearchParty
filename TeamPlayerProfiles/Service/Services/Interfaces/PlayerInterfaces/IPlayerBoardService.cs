using Common.Models;
using Library.Models.API.UserMessaging;
using Service.Contracts.Player;
using Service.Services.Interfaces.Common;

namespace Service.Services.Interfaces.PlayerInterfaces
{
    public interface IPlayerBoardService : IBoardService<PlayerDto, ConditionalPlayerQuery>
    {
        Task InvitePlayerToTeam(ProfileMessageSubmitted message, CancellationToken cancellationToken);
    }
}
