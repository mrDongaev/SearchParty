﻿using FluentResults;
using Library.Results.Successes.Messages;
using Service.Domain.Message;
using Service.Domain.States.Interfaces;
using Service.Dtos.Message;

namespace Service.Domain.States.Implementations.AcceptedMessage
{
    public class AcceptedTeamApplication(TeamApplication message) : AbstractTeamApplicationState(message)
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
            return Result.Ok(MessageDto).WithSuccess(new MessageAlreadyAcceptedSuccess("The application has already been accepted"));
        }
    }
}
