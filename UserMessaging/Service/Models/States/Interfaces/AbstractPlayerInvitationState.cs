using Service.Dtos.Message;
using Service.Models.Message;

namespace Service.Models.States.Interfaces
{
    public abstract class AbstractPlayerInvitationState : AbstractMessageState<PlayerInvitationDto>
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
    }
}
