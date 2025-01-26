using Common.Models;
using FluentResults;
using Service.Contracts.Player;
using Service.Services.Interfaces.Common;

namespace Service.Services.Interfaces.PlayerInterfaces
{
    public interface IPlayerBoardService : IBoardService<PlayerDto, ConditionalPlayerQuery>
    {
        Task<Result> InvitePlayerToTeam(Guid playerId, Guid teamId, int positionId, CancellationToken cancellationToken);
    }
}
