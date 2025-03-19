using FluentResults;
using Library.Results.Successes.Messages;
using Service.Domain.Message;
using Service.Domain.States.Interfaces;
using Service.Dtos.Message;

namespace Service.Domain.States.Implementations.RescindedMessage
{
    public class RescindedPlayerInvitation(PlayerInvitation message) : AbstractPlayerInvitationState(message)
    {
        public async override Task<Result<PlayerInvitationDto>> Accept()
        {
            return _giveFailureResponse();
        }

        public async override Task<Result<PlayerInvitationDto>> Reject()
        {
            return _giveFailureResponse();
        }

        public async override Task<Result<PlayerInvitationDto>> Rescind()
        {
            return _giveFailureResponse();
        }

        private Result<PlayerInvitationDto> _giveFailureResponse()
        {
            return Result.Ok(MessageDto).WithSuccess(new MessageAlreadyRescindedSuccess("The invitation has already been rescinded"));
        }
    }
}
