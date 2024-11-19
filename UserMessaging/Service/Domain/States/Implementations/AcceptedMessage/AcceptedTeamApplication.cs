using Library.Models.Enums;
using Service.Domain.Message;
using Service.Domain.States.Interfaces;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;

namespace Service.Domain.States.Implementations.AcceptedMessage
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
