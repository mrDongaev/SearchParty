using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Models.Message;
using Service.Models.States.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.States.Implementations.ExpiredMessage
{
    public class ExpiredTeamApplication(TeamApplication message) : AbstractTeamApplicationState(message)
    {
        public override Task<ActionResponse<TeamApplicationDto>> Accept()
        {
            throw new NotImplementedException();
        }

        public override Task<ActionResponse<TeamApplicationDto>> Expire()
        {
            throw new NotImplementedException();
        }

        public override Task<ActionResponse<TeamApplicationDto>> Reject()
        {
            throw new NotImplementedException();
        }

        public override Task<ActionResponse<TeamApplicationDto>> Rescind()
        {
            throw new NotImplementedException();
        }

        public override Task<TeamApplicationDto?> SaveToDatabase()
        {
            throw new NotImplementedException();
        }

        public override Task TrySendToUser()
        {
            throw new NotImplementedException();
        }
    }
}
