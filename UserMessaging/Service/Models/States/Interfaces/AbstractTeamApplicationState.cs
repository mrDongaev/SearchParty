using Service.Dtos.Message;
using Service.Models.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.States.Interfaces
{
    public abstract class AbstractTeamApplicationState : AbstractMessageState
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
