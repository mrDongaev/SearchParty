using FluentResults;
using Library.Results.Errors.Messages;
using Service.Domain.Message;
using Service.Domain.States.Interfaces;
using Service.Dtos.Message;

namespace Service.Domain.States.Implementations.RejectedMessage
{
    public class RejectedTeamApplication(TeamApplication message) : AbstractTeamApplicationState(message)
    {
        public async override Task<Result<TeamApplicationDto>> Accept()
        {
            return _giveFailureResponse();
        }

        public async override Task<Result<TeamApplicationDto>> Reject()
        {
            return _giveFailureResponse();
        }

        public async override Task<Result<TeamApplicationDto>> Rescind()
        {
            return _giveFailureResponse();
        }

        private Result<TeamApplicationDto> _giveFailureResponse()
        {
            return Result.Fail<TeamApplicationDto>(new MessageAcceptedError("The application has already been rejected")).WithValue(MessageDto);
        }
    }
}
