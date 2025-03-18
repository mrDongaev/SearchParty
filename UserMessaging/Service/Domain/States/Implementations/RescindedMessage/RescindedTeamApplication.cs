using FluentResults;
using Library.Results.Errors.Messages;
using Service.Domain.Message;
using Service.Domain.States.Interfaces;
using Service.Dtos.Message;

namespace Service.Domain.States.Implementations.RescindedMessage
{
    public class RescindedTeamApplication(TeamApplication message) : AbstractTeamApplicationState(message)
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
            return Result.Fail<TeamApplicationDto>(new MessageAcceptedError("The application has already been rescinded")).WithValue(MessageDto);
        }
    }
}
