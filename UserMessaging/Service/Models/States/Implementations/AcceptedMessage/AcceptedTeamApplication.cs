using Library.Models.Enums;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Models.Message;
using Service.Models.States.Interfaces;

namespace Service.Models.States.Implementations.AcceptedMessage
{
    public class AcceptedTeamApplication(TeamApplication message) : AbstractTeamApplicationState(message)
    {
        public async override Task<ActionResponse<TeamApplicationDto>> Accept()
        {
            return _giveFailureResponse();
        }

        public async override Task<ActionResponse<TeamApplicationDto>> Reject()
        {
            return _giveFailureResponse();
        }

        public async override Task<ActionResponse<TeamApplicationDto>> Rescind()
        {
            return _giveFailureResponse();
        }

        private ActionResponse<TeamApplicationDto> _giveFailureResponse()
        {
            var actionResponse = new ActionResponse<TeamApplicationDto>();
            actionResponse.ActionMessage = "The application has already been accepted";
            actionResponse.Status = ActionResponseStatus.Failure;
            actionResponse.Message = MessageDto;
            return actionResponse;
        }
    }
}
