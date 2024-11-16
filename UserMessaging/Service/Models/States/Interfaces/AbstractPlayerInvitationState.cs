using Service.Dtos.Message;
using Service.Models.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.States.Interfaces
{
    public abstract class AbstractPlayerInvitationState(PlayerInvitation message) : AbstractMessageState<PlayerInvitation, PlayerInvitationDto>(message)
    {
    }
}
