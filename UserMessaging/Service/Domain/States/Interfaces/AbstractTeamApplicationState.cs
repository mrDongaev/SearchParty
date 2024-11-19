using Service.Domain.Message;
using Service.Dtos.Message;

namespace Service.Domain.States.Interfaces
{
    public abstract class AbstractTeamApplicationState : AbstractMessageState<TeamApplicationDto>
    {
        public new TeamApplication Message { get; set; }

        public AbstractTeamApplicationState(TeamApplication message) : base(message)
        {
            Message = message;
        }

        public Guid ApplyingPlayerId
        {
            get => this.Message.ApplyingPlayerId;
        }

        public Guid AcceptingTeamId
        {
            get => this.Message.AcceptingTeamId;
        }
    }
}
