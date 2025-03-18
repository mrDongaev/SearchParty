﻿using FluentResults;
using Library.Results.Errors.Messages;
using Service.Domain.Message;
using Service.Domain.States.Interfaces;
using Service.Dtos.Message;

namespace Service.Domain.States.Implementations.ExpiredMessage
{
    public class ExpiredPlayerInvitation(PlayerInvitation message) : AbstractPlayerInvitationState(message)
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
            return Result.Fail<PlayerInvitationDto>(new MessageAcceptedError("The invitation has expired")).WithValue(MessageDto);
        }
    }
}
