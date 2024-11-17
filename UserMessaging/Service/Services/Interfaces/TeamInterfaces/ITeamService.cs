using Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces.TeamInterfaces
{
    public interface ITeamService
    {
        Task<bool> PushInvitedPlayerToTeam(Guid teamId, Guid playerId, PositionName position, Guid messageId, CancellationToken cancellationToken);

        Task<bool> PushApplyingPlayerToTeam(Guid teamId, Guid playerId, PositionName position, Guid messageId, CancellationToken cancellationToken);

        Task<bool> PushPlayerToTeam(Guid teamId, Guid playerId, PositionName position, Guid messageid, MessageType messageType, CancellationToken cancellationToken);
    }
}
