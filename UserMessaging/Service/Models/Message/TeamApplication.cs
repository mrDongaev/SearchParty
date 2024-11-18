using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Service.Dtos.Message;
using Service.Models.States.Interfaces;
using Service.Repositories.Interfaces;

namespace Service.Models.Message
{
    public class TeamApplication : AbstractMessage
    {
        public TeamApplication(IServiceProvider serviceProvider, IUserHttpContext userContext, AbstractMessageState startingState, CancellationToken cancellationToken)
            : base(serviceProvider, userContext, startingState, cancellationToken)
        {
        }

        public Guid ApplyingPlayerId { get; set; }

        public Guid AcceptingTeamId { get; set; }

        public override TeamApplicationDto MessageDto => new TeamApplicationDto
        {
            Id = this.Id,
            AcceptingUserId = this.AcceptingUserId,
            SendingUserId = this.SendingUserId,
            AcceptingTeamId = this.AcceptingTeamId,
            ApplyingPlayerId = this.ApplyingPlayerId,
            PositionName = this.PositionName,
            Status = this.Status,
            IssuedAt = this.IssuedAt,
            ExpiresAt = this.ExpiresAt,
            UpdatedAt = DateTime.UtcNow,
        };

        public async override Task<TeamApplicationDto?> SaveToDatabase()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var teamApplicationService = scope.ServiceProvider.GetRequiredService<ITeamApplicationRepository>();
                return await teamApplicationService.SaveMessage(MessageDto, CancellationToken);
            }
        }

        public override Task TrySendToUser()
        {
            return Task.CompletedTask;
            //throw new NotImplementedException();
        }
    }
}
