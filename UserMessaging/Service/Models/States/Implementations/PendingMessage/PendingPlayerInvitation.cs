using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Models.Message;
using Service.Models.States.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.States.Implementations.PendingMessage
{
    public class PendingPlayerInvitation(PlayerInvitation message) : AbstractPlayerInvitationState(message)
    {
        public override Task<ActionResponse<PlayerInvitationDto>> Accept(CancellationToken cancellationToken)
        {
            
        }

        public override Task<ActionResponse<PlayerInvitationDto>> Reject(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<ActionResponse<PlayerInvitationDto>> Rescind(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
