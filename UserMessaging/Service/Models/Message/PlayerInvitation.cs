using Microsoft.Extensions.DependencyInjection;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Repositories.Interfaces;
using System.Threading;

namespace Service.Models.Message
{
    public class PlayerInvitation : AbstractMessage<PlayerInvitationDto>
    {
        public readonly IServiceProvider serviceProvider;

        public readonly CancellationToken cancellationToken;

        public PlayerInvitation(IServiceProvider serviceProvider, CancellationToken cancellationToken) : base(cancellationToken) 
        {
            this.serviceProvider = serviceProvider;
            this.cancellationToken = cancellationToken;
        }

        public Guid AcceptingPlayerId { get; set; }

        public Guid InvitingTeamId { get; set; }

        public async override Task<PlayerInvitationDto?> SaveToDatabase()
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var playerInvitationService = scope.ServiceProvider.GetRequiredService<IPlayerInvitationRepository>();
                PlayerInvitationDto message = new PlayerInvitationDto
                {
                    Id = this.Id,
                    SendingUserId = this.SendingUserId,
                    AcceptingUserId = this.AcceptingUserId,
                    InvitingTeamId = this.InvitingTeamId,
                    AcceptingPlayerId = this.AcceptingPlayerId,
                    Status = this.Status,
                    IssuedAt = this.IssuedAt,
                    ExpiresAt = this.ExpiresAt,
                    UpdatedAt = DateTime.UtcNow,
                    PositionName = this.PositionName,
                };
                return await playerInvitationService.SaveMessage(message, cancellationToken);
            }
        }

        public override Task TrySendToUser()
        {
            throw new NotImplementedException();
        }
    }
}
