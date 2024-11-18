using Library.Models.Enums;
using Microsoft.Extensions.DependencyInjection;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Models.Message;
using Service.Models.States.Implementations.ExpiredMessage;
using Service.Models.States.Interfaces;
using Service.Services.Interfaces.TeamInterfaces;

namespace Service.Models.States.Implementations.PendingMessage
{
    public class PendingPlayerInvitation(PlayerInvitation message) : AbstractPlayerInvitationState(message)
    {
        public override async Task<ActionResponse<PlayerInvitationDto>> Accept()
        {
            var actionResponse = new ActionResponse<PlayerInvitationDto>();
            if (DateTime.UtcNow >= this.ExpiresAt)
            {
                this.Message.ChangeState(new ExpiredPlayerInvitation(this.Message));
            }
            else
            {
                using (var scope = this.ServiceProvider.CreateScope())
                {
                    var teamService = scope.ServiceProvider.GetRequiredService<ITeamService>();
                    var response = await teamService.PushInvitedPlayerToTeam(this.InvitingTeamId, this.AcceptingPlayerId, this.PositionName, this.Id, this.CancellationToken);
                    if (response)
                    {
                        this.Status = MessageStatus.Accepted;
                        actionResponse.ActionMessage = "Invitation to team accepted";
                        actionResponse.Status = ActionResponseStatus.Success;
                        this.Message.SaveToDatabase();
                        this.Message.TrySendToUser();
                    }
                    else
                    {
                        actionResponse.ActionMessage = "Could not accept the invitation to a team";
                    }
                }
            }
            return actionResponse;
        }

        public override Task<ActionResponse<PlayerInvitationDto>> Reject()
        {
            throw new NotImplementedException();
        }

        public override Task<ActionResponse<PlayerInvitationDto>> Rescind()
        {
            throw new NotImplementedException();
        }
    }
}
