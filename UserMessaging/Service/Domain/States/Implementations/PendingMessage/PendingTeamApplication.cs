using Library.Models.Enums;
using Microsoft.Extensions.DependencyInjection;
using Service.Domain.Message;
using Service.Domain.States.Implementations.AcceptedMessage;
using Service.Domain.States.Implementations.ExpiredMessage;
using Service.Domain.States.Implementations.RejectedMessage;
using Service.Domain.States.Implementations.RescindedMessage;
using Service.Domain.States.Interfaces;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Services.Interfaces.TeamInterfaces;

namespace Service.Domain.States.Implementations.PendingMessage
{
    public class PendingTeamApplication(TeamApplication message) : AbstractTeamApplicationState(message)
    {
        public override async Task<ActionResponse<TeamApplicationDto>> Accept()
        {
            var actionResponse = new ActionResponse<TeamApplicationDto>();
            if (DateTime.UtcNow >= ExpiresAt)
            {
                Status = MessageStatus.Expired;
                Message.ChangeState(new ExpiredTeamApplication(Message));
                actionResponse.ActionMessage = "The application to the team has expired";
                actionResponse.Status = ActionResponseStatus.Failure;
            }
            else
            {
                using var scope = ServiceProvider.CreateScope();
                var teamService = scope.ServiceProvider.GetRequiredService<ITeamService>();
                var response = await teamService.PushApplyingPlayerToTeam(AcceptingTeamId, ApplyingPlayerId, PositionName, Id, CancellationToken);
                if (response)
                {
                    Status = MessageStatus.Accepted;
                    Message.ChangeState(new AcceptedTeamApplication(Message));
                    actionResponse.ActionMessage = "Application to the team has been accepted";
                    actionResponse.Status = ActionResponseStatus.Success;
                }
                else
                {
                    actionResponse.ActionMessage = "Could not accept the application to the team";
                    actionResponse.Status = ActionResponseStatus.Failure;
                }
            }
            TeamApplicationDto? message = null;
            if (Status == MessageStatus.Accepted || Status == MessageStatus.Expired)
            {
                message = await Message.SaveToDatabase();
                if (message == null)
                {
                    Status = MessageStatus.Pending;
                    Message.ChangeState(new PendingTeamApplication(Message));
                    actionResponse.ActionMessage = "Could not accept the application to the team";
                    actionResponse.Status = ActionResponseStatus.Failure;
                }
            }
            actionResponse.Message = message ?? MessageDto;
            return actionResponse;
        }

        public async override Task<ActionResponse<TeamApplicationDto>> Reject()
        {
            var actionResponse = new ActionResponse<TeamApplicationDto>();
            if (DateTime.UtcNow >= ExpiresAt)
            {
                Status = MessageStatus.Expired;
                Message.ChangeState(new ExpiredTeamApplication(Message));
                actionResponse.ActionMessage = "The application to the team has expired";
                actionResponse.Status = ActionResponseStatus.Failure;
            }
            else
            {
                Status = MessageStatus.Rejected;
                Message.ChangeState(new RejectedTeamApplication(Message));
                actionResponse.ActionMessage = "Application to the team has been rejected";
                actionResponse.Status = ActionResponseStatus.Success;
            }
            TeamApplicationDto? message = null;
            if (Status == MessageStatus.Rejected || Status == MessageStatus.Expired)
            {
                message = await Message.SaveToDatabase();
                if (message == null)
                {
                    Status = MessageStatus.Pending;
                    Message.ChangeState(new PendingTeamApplication(Message));
                    actionResponse.ActionMessage = "Could not accept the application to the team";
                    actionResponse.Status = ActionResponseStatus.Failure;
                }
            }
            actionResponse.Message = message ?? MessageDto;
            return actionResponse;
        }

        public async override Task<ActionResponse<TeamApplicationDto>> Rescind()
        {
            var actionResponse = new ActionResponse<TeamApplicationDto>();
            if (DateTime.UtcNow >= ExpiresAt)
            {
                Status = MessageStatus.Expired;
                Message.ChangeState(new ExpiredTeamApplication(Message));
                actionResponse.ActionMessage = "The application to the team has expired";
                actionResponse.Status = ActionResponseStatus.Failure;
            }
            else
            {
                Status = MessageStatus.Rescinded;
                Message.ChangeState(new RescindedTeamApplication(Message));
                actionResponse.ActionMessage = "Application to the team has been rescinded";
                actionResponse.Status = ActionResponseStatus.Success;
            }
            TeamApplicationDto? message = null;
            if (Status == MessageStatus.Rescinded || Status == MessageStatus.Expired)
            {
                message = await Message.SaveToDatabase();
                if (message == null)
                {
                    Status = MessageStatus.Pending;
                    Message.ChangeState(new PendingTeamApplication(Message));
                    actionResponse.ActionMessage = "Could not rescind the application to the team";
                    actionResponse.Status = ActionResponseStatus.Failure;
                }
            }
            actionResponse.Message = message ?? MessageDto;
            return actionResponse;
        }
    }
}
