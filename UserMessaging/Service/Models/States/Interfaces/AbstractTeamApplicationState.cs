using Service.Dtos.Message;
using Service.Models.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.States.Interfaces
{
    public abstract class AbstractTeamApplicationState(TeamApplication message) : AbstractMessageState<TeamApplication, TeamApplicationDto>(message)
    {
        public new TeamApplication Message { get; set; }
    }
}
