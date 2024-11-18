using Library.Models.Enums;
using Microsoft.Extensions.DependencyInjection;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Models.Message;
using Service.Models.States.Implementations.AcceptedMessage;
using Service.Models.States.Implementations.ExpiredMessage;
using Service.Models.States.Implementations.RejectedMessage;
using Service.Models.States.Implementations.RescindedMessage;
using Service.Models.States.Interfaces;
using Service.Services.Interfaces.TeamInterfaces;

namespace Service.Models.States.Implementations.PendingMessage
{
    public class PendingPlayerInvitation(PlayerInvitation message) : AbstractPlayerInvitationState(message)
    {
        public override async Task<ActionResponse<PlayerInvitationDto>> Accept()
        {
            var actionResponse = new ActionResponse<PlayerInvitationDto>();
            if (DateTime.UtcNow >= ExpiresAt)
            {
                Status = MessageStatus.Expired;
                Message.ChangeState(new ExpiredPlayerInvitation(Message));
                actionResponse.ActionMessage = "The invitation to the team has expired";
                actionResponse.Status = ActionResponseStatus.Failure;
            }
            else
            {
                using var scope = ServiceProvider.CreateScope();
                var teamService = scope.ServiceProvider.GetRequiredService<ITeamService>();
                var response = await teamService.PushInvitedPlayerToTeam(InvitingTeamId, AcceptingPlayerId, PositionName, Id, CancellationToken);
                if (response)
                {
                    Status = MessageStatus.Accepted;
                    Message.ChangeState(new AcceptedPlayerInvitation(Message));
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
                    Status = MessageStatus.Pending;
                    Message.ChangeState(new PendingPlayerInvitation(Message));
                    actionResponse.ActionMessage = "Could not accept the invitation to the team";
                    actionResponse.Status = ActionResponseStatus.Failure;
                }
            }
            actionResponse.Message = message ?? MessageDto;
            return actionResponse;
        }

        public async override Task<ActionResponse<PlayerInvitationDto>> Reject()
        {
            var actionResponse = new ActionResponse<PlayerInvitationDto>();
            if (DateTime.UtcNow >= ExpiresAt)
            {
                Status = MessageStatus.Expired;
                Message.ChangeState(new ExpiredPlayerInvitation(Message));
                actionResponse.ActionMessage = "The invitation to the team has expired";
                actionResponse.Status = ActionResponseStatus.Failure;
            }
            else
            {
                Status = MessageStatus.Rejected;
                Message.ChangeState(new RejectedPlayerInvitation(Message));
                actionResponse.ActionMessage = "Invitation to the team has been rejected";
                actionResponse.Status = ActionResponseStatus.Success;
            }
            PlayerInvitationDto? message = null;
            if (Status == MessageStatus.Rejected || Status == MessageStatus.Expired)
            {
                message = await Message.SaveToDatabase();
                if (message == null)
                {
                    Status = MessageStatus.Pending;
                    Message.ChangeState(new PendingPlayerInvitation(Message));
                    actionResponse.ActionMessage = "Could not accept the invitation to the team";
                    actionResponse.Status = ActionResponseStatus.Failure;
                }
            }
            actionResponse.Message = message ?? MessageDto;
            return actionResponse;
        }

        public async override Task<ActionResponse<PlayerInvitationDto>> Rescind()
        {
            var actionResponse = new ActionResponse<PlayerInvitationDto>();
            if (DateTime.UtcNow >= ExpiresAt)
            {
                Status = MessageStatus.Expired;
                Message.ChangeState(new ExpiredPlayerInvitation(Message));
                actionResponse.ActionMessage = "The invitation to the team has expired";
                actionResponse.Status = ActionResponseStatus.Failure;
            }
            else
            {
                Status = MessageStatus.Rescinded;
                Message.ChangeState(new RescindedPlayerInvitation(Message));
                actionResponse.ActionMessage = "Invitation to the team has been rescinded";
                actionResponse.Status = ActionResponseStatus.Success;
            }
            PlayerInvitationDto? message = null;
            if (Status == MessageStatus.Rescinded || Status == MessageStatus.Expired)
            {
                message = await Message.SaveToDatabase();
                if (message == null)
                {
                    Status = MessageStatus.Pending;
                    Message.ChangeState(new PendingPlayerInvitation(Message));
                    actionResponse.ActionMessage = "Could not rescind the invitation to the team";
                    actionResponse.Status = ActionResponseStatus.Failure;
                }
            }
            actionResponse.Message = message ?? MessageDto;
            return actionResponse;
        }
    }
}
