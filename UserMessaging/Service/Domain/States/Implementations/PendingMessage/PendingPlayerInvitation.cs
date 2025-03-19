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
    public class PendingPlayerInvitation(PlayerInvitation message) : AbstractPlayerInvitationState(message)
    {
        public override async Task<Result<PlayerInvitationDto>> Accept()
        {
            Result<PlayerInvitationDto> actionResponse = Result.Fail<PlayerInvitationDto>(new MessageAcceptFailedError("Could not accept the invitation to the team"));
            PlayerInvitationDto? message = MessageDto;
            try
            {
                if (DateTime.UtcNow >= ExpiresAt)
                {
                    Message.ChangeState(MessageStatus.Expired);
                }
                else
                {
                    using var scope = ScopeFactory.CreateScope();
                    var teamService = scope.ServiceProvider.GetRequiredService<ITeamService>();
                    teamService.UserContext = UserContext;
                    var response = await teamService.PushInvitedPlayerToTeam(InvitingTeamId, AcceptingPlayerId, PositionName, Id, CancellationToken);
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
                        actionResponse = Result.Fail<PlayerInvitationDto>(new MessageAcceptFailedError("Could not accept the invitation to the team"));
                    }
                    else
                    {
                        if (Status == MessageStatus.Expired)
                        {
                            actionResponse = Result.Ok(message).WithSuccess(new MessageAlreadyExpiredSuccess("Invitation to the team has already expired"));
                        }
                        else
                        {
                            actionResponse = Result.Ok(message).WithSuccess(new MessageAcceptedSuccess("Invitation to the team has been accepted"));
                        }
                    }
                }
            }
            catch
            {
                Message.ChangeState(MessageStatus.Pending);
                actionResponse = Result.Fail<PlayerInvitationDto>(new MessageAcceptFailedError("Could not accept the invitation to the team"));
            }

            return actionResponse;
        }

        public async override Task<Result<PlayerInvitationDto>> Reject()
        {
            Result<PlayerInvitationDto> actionResponse = Result.Fail<PlayerInvitationDto>(new MessageRejectFailedError("Could not reject the invitation to the team"));
            PlayerInvitationDto? message = MessageDto;
            try
            {
                if (DateTime.UtcNow >= ExpiresAt)
                {
                    Message.ChangeState(MessageStatus.Expired);
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
                        actionResponse = Result.Fail<PlayerInvitationDto>(new MessageRejectFailedError("Could not reject the invitation to the team"));
                    }
                    else
                    {
                        if (Status == MessageStatus.Expired)
                        {
                            actionResponse = Result.Ok(message).WithSuccess(new MessageAlreadyExpiredSuccess("Invitation to the team has already expired"));
                        }
                        else
                        {
                            actionResponse = Result.Ok(message).WithSuccess(new MessageRejectedSuccess("Invitation to the team has been rejected"));
                        }
                    }
                }
            }
            catch
            {
                Message.ChangeState(MessageStatus.Pending);
                actionResponse = Result.Fail<PlayerInvitationDto>(new MessageRejectFailedError("Could not reject the invitation to the team"));
            }

            return actionResponse;
        }

        public async override Task<Result<PlayerInvitationDto>> Rescind()
        {
            Result<PlayerInvitationDto> actionResponse = Result.Fail<PlayerInvitationDto>(new MessageRescindFailedError("Could not rescind the invitation to the team"));
            PlayerInvitationDto? message = MessageDto;
            try
            {
                if (DateTime.UtcNow >= ExpiresAt)
                {
                    Message.ChangeState(MessageStatus.Expired);
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
                        actionResponse = Result.Fail<PlayerInvitationDto>(new MessageRescindFailedError("Could not rescind the invitation to the team"));
                    }
                    else
                    {
                        if (Status == MessageStatus.Expired)
                        {
                            actionResponse = Result.Ok(message).WithSuccess(new MessageAlreadyExpiredSuccess("Invitation to the team has already expired"));
                        }
                        else
                        {
                            actionResponse = Result.Ok(message).WithSuccess(new MessageRescindedSuccess("Invitation to the team has been rescinded"));
                        }
                    }
                }
            }
            catch
            {
                Message.ChangeState(MessageStatus.Pending);
                actionResponse = Result.Fail<PlayerInvitationDto>(new MessageRescindFailedError("Could not rescind the invitation to the team"));
            }

            return actionResponse;
        }
    }
}
