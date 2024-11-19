using Library.Models.Enums;
using Service.Domain.Message;
using Service.Domain.States.Interfaces;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;

namespace Service.Domain.States.Implementations.RejectedMessage
{
    public class RejectedPlayerInvitation(PlayerInvitation message) : AbstractPlayerInvitationState(message)
    {
        public async override Task<ActionResponse<PlayerInvitationDto>> Accept()
        {
            return _giveFailureResponse();
        }

        public async override Task<ActionResponse<PlayerInvitationDto>> Reject()
        {
            return _giveFailureResponse();
        }

        public async override Task<ActionResponse<PlayerInvitationDto>> Rescind()
        {
            return _giveFailureResponse();
        }

        private ActionResponse<PlayerInvitationDto> _giveFailureResponse()
        {
            var actionResponse = new ActionResponse<PlayerInvitationDto>();
            actionResponse.ActionMessage = "The invitation has already been rejected";
            actionResponse.Status = ActionResponseStatus.Failure;
            actionResponse.Message = MessageDto;
            return actionResponse;
        }
    }
}
