using Microsoft.Extensions.DependencyInjection;
using Service.Dtos.ActionResponse;
using Service.Dtos.Message;
using Service.Repositories.Interfaces;

namespace Service.Models.Message
{
    public class TeamApplication : AbstractMessage<TeamApplicationDto>
    {
        public readonly IServiceProvider serviceProvider;

        public readonly CancellationToken cancellationToken;

        public TeamApplication(IServiceProvider serviceProvider, CancellationToken cancellationToken) : base(cancellationToken)
        {
            this.serviceProvider = serviceProvider;

            this.cancellationToken = cancellationToken;
        }

        public Guid ApplyingPlayerId { get; set; }

        public Guid AcceptingTeamId { get; set; }

        public async override Task<TeamApplicationDto?> SaveToDatabase()
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var teamApplicationService = scope.ServiceProvider.GetRequiredService<ITeamApplicationRepository>();
                TeamApplicationDto message = new TeamApplicationDto
                {
                    Id = this.Id,
                    SendingUserId = this.SendingUserId,
                    AcceptingUserId = this.AcceptingUserId,
                    ApplyingPlayerId = this.ApplyingPlayerId,
                    AcceptingTeamId = this.AcceptingTeamId,
                    Status = this.Status,
                    IssuedAt = this.IssuedAt,
                    ExpiresAt = this.ExpiresAt,
                    UpdatedAt = DateTime.UtcNow,
                    PositionName = this.PositionName,
                };
                return await teamApplicationService.SaveMessage(message, cancellationToken);
            }
        }

        public override Task TrySendToUser()
        {
            throw new NotImplementedException();
        }
    }
}
