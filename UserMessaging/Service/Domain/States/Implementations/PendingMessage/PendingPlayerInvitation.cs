using Library.Models.Enums;
using Microsoft.Extensions.DependencyInjection;
using Service.Domain.Message;
using Service.Domain.States.Interfaces;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Services.Interfaces.TeamInterfaces;

namespace Service.Domain.States.Implementations.PendingMessage
{
    public class PendingPlayerInvitation(PlayerInvitation message) : AbstractPlayerInvitationState(message)
    {
        public override async Task<ActionResponse<PlayerInvitationDto>> Accept()
        {
            var actionResponse = new ActionResponse<PlayerInvitationDto>();
            try
            {
                if (DateTime.UtcNow >= ExpiresAt)
                {
                    Message.ChangeState(MessageStatus.Expired);
                    actionResponse.ActionMessage = "The invitation to the team has expired";
                    actionResponse.Status = ActionResponseStatus.Failure;
                }
                else
                {
                    using var scope = ScopeFactory.CreateScope();
                    var teamService = scope.ServiceProvider.GetRequiredService<ITeamService>();
                    teamService.UserContext = UserContext;
                    var response = await teamService.PushInvitedPlayerToTeam(InvitingTeamId, AcceptingPlayerId, PositionName, Id, CancellationToken);
                    if (response)
                    {
                        Message.ChangeState(MessageStatus.Accepted);
                        actionResponse.ActionMessage = "Invitation to the team has been accepted";
                        actionResponse.Status = ActionResponseStatus.Success;
                    }
                    else
                    {
                        actionResponse.ActionMessage = "Could not accept the invitation to the team";
                        actionResponse.Status = ActionResponseStatus.Failure;
                    }
                }
                PlayerInvitationDto? message = null;
                if (Status == MessageStatus.Accepted || Status == MessageStatus.Expired)
                {
                    message = await Message.SaveToDatabase();
                    if (message == null)
                    {
                        Message.ChangeState(MessageStatus.Pending);
                        actionResponse.ActionMessage = "Could not accept the invitation to the team";
                        actionResponse.Status = ActionResponseStatus.Failure;
                        actionResponse.Message = MessageDto;
                    }
                }
                actionResponse.Message = message ?? MessageDto;
            }
            catch
            {
                actionResponse.ActionMessage = "Could not accept the invitation to the team";
                actionResponse.Status = ActionResponseStatus.Failure;
                actionResponse.Message = MessageDto;
            }
            return actionResponse;
        }

        public async override Task<ActionResponse<PlayerInvitationDto>> Reject()
        {
            var actionResponse = new ActionResponse<PlayerInvitationDto>();
            if (DateTime.UtcNow >= ExpiresAt)
            {
                Message.ChangeState(MessageStatus.Expired);
                actionResponse.ActionMessage = "The invitation to the team has expired";
                actionResponse.Status = ActionResponseStatus.Failure;
            }
            else
            {
                Message.ChangeState(MessageStatus.Rejected);
                actionResponse.ActionMessage = "Invitation to the team has been rejected";
                actionResponse.Status = ActionResponseStatus.Success;
            }
            try
            {
                PlayerInvitationDto? message = null;
                if (Status == MessageStatus.Rejected || Status == MessageStatus.Expired)
                {
                    message = await Message.SaveToDatabase();
                    if (message == null)
                    {
                        Message.ChangeState(MessageStatus.Pending);
                        actionResponse.ActionMessage = "Could not accept the invitation to the team";
                        actionResponse.Status = ActionResponseStatus.Failure;
                    }
                }
                actionResponse.Message = message ?? MessageDto;
            }
            catch
            {
                Message.ChangeState(MessageStatus.Pending);
                actionResponse.ActionMessage = "Could not accept the invitation to the team";
                actionResponse.Status = ActionResponseStatus.Failure;
                actionResponse.Message = MessageDto;
            }
            return actionResponse;
        }

        public async override Task<ActionResponse<PlayerInvitationDto>> Rescind()
        {
            var actionResponse = new ActionResponse<PlayerInvitationDto>();
            if (DateTime.UtcNow >= ExpiresAt)
            {
                Message.ChangeState(MessageStatus.Expired);
                actionResponse.ActionMessage = "The invitation to the team has expired";
                actionResponse.Status = ActionResponseStatus.Failure;
            }
            else
            {
                Message.ChangeState(MessageStatus.Rescinded);
                actionResponse.ActionMessage = "Invitation to the team has been rescinded";
                actionResponse.Status = ActionResponseStatus.Success;
            }
            try
            {
                PlayerInvitationDto? message = null;
                if (Status == MessageStatus.Rescinded || Status == MessageStatus.Expired)
                {
                    message = await Message.SaveToDatabase();
                    if (message == null)
                    {
                        Message.ChangeState(MessageStatus.Pending);
                        actionResponse.ActionMessage = "Could not rescind the invitation to the team";
                        actionResponse.Status = ActionResponseStatus.Failure;
                    }
                }
                actionResponse.Message = message ?? MessageDto;
            }
            catch
            {
                Message.ChangeState(MessageStatus.Pending);
                actionResponse.ActionMessage = "Could not rescind the invitation to the team";
                actionResponse.Status = ActionResponseStatus.Failure;
                actionResponse.Message = MessageDto;
            }
            return actionResponse;
        }
    }
}
