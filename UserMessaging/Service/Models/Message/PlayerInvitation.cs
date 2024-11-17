using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Models.States.Interfaces;
using Service.Repositories.Interfaces;
using System.Threading;

namespace Service.Models.Message
{
    public class PlayerInvitation : AbstractMessage
    {
        public PlayerInvitation(IServiceProvider serviceProvider, IUserHttpContext userContext, AbstractMessageState startingState, CancellationToken cancellationToken) 
            : base(serviceProvider, userContext, startingState, cancellationToken) 
        {
        }

        public Guid AcceptingPlayerId { get; set; }

        public Guid InvitingTeamId { get; set; }

        public override PlayerInvitationDto MessageDto => new PlayerInvitationDto
        {
            Id = this.Id,
            AcceptingUserId = this.AcceptingUserId,
            AcceptingPlayerId = this.AcceptingPlayerId,
            InvitingTeamId = this.InvitingTeamId,
            SendingUserId = this.SendingUserId,
            PositionName = this.PositionName,
            Status = this.Status,
            IssuedAt = this.IssuedAt,
            ExpiresAt = this.ExpiresAt,
            UpdatedAt = DateTime.UtcNow,
        };

        public async override Task<PlayerInvitationDto?> SaveToDatabase()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var playerInvitationService = scope.ServiceProvider.GetRequiredService<IPlayerInvitationRepository>();
                return await playerInvitationService.SaveMessage(MessageDto, CancellationToken);
            }
        }

        public override Task TrySendToUser()
        {
            return Task.CompletedTask;
            //throw new NotImplementedException();
        }
    }
}
