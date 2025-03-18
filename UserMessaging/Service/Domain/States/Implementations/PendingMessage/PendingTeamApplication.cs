using FluentResults;
using Library.Models.Enums;
using Library.Results.Errors.Messages;
using Library.Results.Successes.Messages;
using Microsoft.Extensions.DependencyInjection;
using Service.Domain.Message;
using Service.Domain.States.Interfaces;
using Service.Dtos.Message;
using Service.Services.Interfaces.TeamInterfaces;

namespace Service.Domain.States.Implementations.PendingMessage
{
    public class PendingTeamApplication(TeamApplication message) : AbstractTeamApplicationState(message)
    {
        public override async Task<Result<TeamApplicationDto>> Accept()
        {
            Result<TeamApplicationDto> actionResponse = Result.Fail<TeamApplicationDto>(new MessageAcceptFailedError("Could not accept the application to the team"));
            TeamApplicationDto? message = MessageDto;
            try
            {
                if (DateTime.UtcNow >= ExpiresAt)
                {
                    Message.ChangeState(MessageStatus.Expired);
                    actionResponse = Result.Fail<TeamApplicationDto>(new MessageExpiredError("The application has expired"));
                }
                else
                {
                    using var scope = ScopeFactory.CreateScope();
                    var teamService = scope.ServiceProvider.GetRequiredService<ITeamService>();
                    teamService.UserContext = UserContext;
                    var response = await teamService.PushApplyingPlayerToTeam(AcceptingTeamId, ApplyingPlayerId, PositionName, Id, CancellationToken);
                    if (response != null)
                    {
                        Message.ChangeState(MessageStatus.Accepted);
                    }
                }
                if (Status == MessageStatus.Accepted || Status == MessageStatus.Expired)
                {
                    message = await Message.SaveToDatabase();
                    if (message == null)
                    {
                        Message.ChangeState(MessageStatus.Pending);
                        message = MessageDto;
                        actionResponse = Result.Fail<TeamApplicationDto>(new MessageAcceptFailedError("Could not accept the application to the team"));
                    }
                    else
                    {
                        actionResponse = Result.Ok(message).WithSuccess(new MessageAcceptedSuccess("Application to the team has been accepted"));
                    }
                }
            }
            catch
            {
                Message.ChangeState(MessageStatus.Pending);
                actionResponse = Result.Fail<TeamApplicationDto>(new MessageAcceptFailedError("Could not accept the application to the team"));
            }
            if (actionResponse.IsFailed)
            {
                actionResponse.WithValue(message);
            }
            return actionResponse;
        }

        public async override Task<Result<TeamApplicationDto>> Reject()
        {
            Result<TeamApplicationDto> actionResponse = Result.Fail<TeamApplicationDto>(new MessageRejectFailedError("Could not reject the application to the team"));
            TeamApplicationDto? message = MessageDto;
            try
            {
                if (DateTime.UtcNow >= ExpiresAt)
                {
                    Message.ChangeState(MessageStatus.Expired);
                    actionResponse = Result.Fail<TeamApplicationDto>(new MessageExpiredError("The application has expired"));
                }
                else
                {
                    Message.ChangeState(MessageStatus.Rejected);
                }
                if (Status == MessageStatus.Rejected || Status == MessageStatus.Expired)
                {
                    message = await Message.SaveToDatabase();
                    if (message == null)
                    {
                        Message.ChangeState(MessageStatus.Pending);
                        message = MessageDto;
                        actionResponse = Result.Fail<TeamApplicationDto>(new MessageRejectFailedError("Could not reject the application to the team"));
                    }
                    else
                    {
                        actionResponse = Result.Ok(message).WithSuccess(new MessageAcceptedSuccess("Application to the team has been rejected"));
                    }
                }
            }
            catch
            {
                Message.ChangeState(MessageStatus.Pending);
                actionResponse = Result.Fail<TeamApplicationDto>(new MessageRejectFailedError("Could not reject the application to the team"));
            }
            if (actionResponse.IsFailed)
            {
                actionResponse.WithValue(message);
            }
            return actionResponse;
        }

        public async override Task<Result<TeamApplicationDto>> Rescind()
        {
            Result<TeamApplicationDto> actionResponse = Result.Fail<TeamApplicationDto>(new MessageRescindFailedError("Could not rescind the application to the team"));
            TeamApplicationDto? message = MessageDto;
            try
            {
                if (DateTime.UtcNow >= ExpiresAt)
                {
                    Message.ChangeState(MessageStatus.Expired);
                    actionResponse = Result.Fail<TeamApplicationDto>(new MessageExpiredError("The application has expired"));
                }
                else
                {
                    Message.ChangeState(MessageStatus.Rescinded);
                }
                if (Status == MessageStatus.Rescinded || Status == MessageStatus.Expired)
                {
                    message = await Message.SaveToDatabase();
                    if (message == null)
                    {
                        Message.ChangeState(MessageStatus.Pending);
                        message = MessageDto;
                        actionResponse = Result.Fail<TeamApplicationDto>(new MessageRescindFailedError("Could not rescind the application to the team"));
                    }
                    else
                    {
                        actionResponse = Result.Ok(message).WithSuccess(new MessageRescindedSuccess("Application to the team has been rescinded"));
                    }
                }
            }
            catch
            {
                Message.ChangeState(MessageStatus.Pending);
                actionResponse = Result.Fail<TeamApplicationDto>(new MessageRescindFailedError("Could not rescind the application to the team"));
            }
            if (actionResponse.IsFailed)
            {
                actionResponse.WithValue(message);
            }
            return actionResponse;
        }
    }
}
