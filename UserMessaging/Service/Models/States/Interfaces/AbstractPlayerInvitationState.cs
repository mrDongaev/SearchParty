using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Models.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.States.Interfaces
{
    public abstract class AbstractPlayerInvitationState : AbstractMessageState
    {
        public new PlayerInvitation Message { get; set; }

        public AbstractPlayerInvitationState(PlayerInvitation message) : base(message)
        {
            Message = message;
        }

        public Guid AcceptingPlayerId
        {
            get => this.Message.AcceptingPlayerId;
        }

        public Guid InvitingTeamId
        {
            get => this.Message.InvitingTeamId;
        }

        public new Task<ActionResponse<PlayerInvitationDto>> Accept();

        public new abstract Task<ActionResponse<PlayerInvitationDto>> Reject();

        public new abstract Task<ActionResponse<PlayerInvitationDto>> Rescind();
    }
}
