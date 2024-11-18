﻿using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Models.Message;
using Service.Models.States.Interfaces;

namespace Service.Models.States.Implementations.RescindedMessage
{
    public class RescindedTeamApplication(TeamApplication message) : AbstractTeamApplicationState(message)
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
