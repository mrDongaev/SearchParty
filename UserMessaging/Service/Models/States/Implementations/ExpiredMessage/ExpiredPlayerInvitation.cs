using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Models.Message;
using Service.Models.States.Interfaces;

namespace Service.Models.States.Implementations.ExpiredMessage
{
    public class ExpiredPlayerInvitation(PlayerInvitation message) : AbstractPlayerInvitationState(message)
    {
        public override Task<ActionResponse<PlayerInvitationDto>> Accept()
        {
            throw new NotImplementedException();
        }

        public override Task<ActionResponse<PlayerInvitationDto>> Expire()
        {
            throw new NotImplementedException();
        }

        public override Task<ActionResponse<PlayerInvitationDto>> Reject()
        {
            throw new NotImplementedException();
        }

        public override Task<ActionResponse<PlayerInvitationDto>> Rescind()
        {
            throw new NotImplementedException();
        }

        public override Task<PlayerInvitationDto?> SaveToDatabase()
        {
            throw new NotImplementedException();
        }

        public override Task TrySendToUser()
        {
            throw new NotImplementedException();
        }
    }
}
