using Library.Exceptions;
using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Service.Domain.States.Implementations.AcceptedMessage;
using Service.Domain.States.Implementations.ExpiredMessage;
using Service.Domain.States.Implementations.PendingMessage;
using Service.Domain.States.Implementations.RejectedMessage;
using Service.Domain.States.Implementations.RescindedMessage;
using Service.Domain.States.Interfaces;
using Service.Dtos.Message;
using Service.Repositories.Interfaces;

namespace Service.Domain.Message
{
    public class TeamApplication : AbstractMessage<TeamApplicationDto>
    {
        public TeamApplication(TeamApplicationDto messageDto, IServiceScopeFactory scopeFactory, IUserHttpContext userContext, CancellationToken cancellationToken)
            : base(messageDto, scopeFactory, userContext, cancellationToken)
        {
            ApplyingPlayerId = messageDto.ApplyingPlayerId;
            AcceptingTeamId = messageDto.AcceptingTeamId;
        }

        public Guid ApplyingPlayerId { get; protected set; }

        public Guid AcceptingTeamId { get; protected set; }

        public override TeamApplicationDto MessageDto => new TeamApplicationDto
        {
            Id = Id,
            AcceptingUserId = AcceptingUserId,
            SendingUserId = SendingUserId,
            AcceptingTeamId = AcceptingTeamId,
            ApplyingPlayerId = ApplyingPlayerId,
            PositionName = PositionName,
            Status = Status,
            IssuedAt = IssuedAt,
            ExpiresAt = ExpiresAt,
            UpdatedAt = UpdatedAt,
        };

        protected override AbstractMessageState<TeamApplicationDto> CreateNewMessageState(MessageStatus status)
        {
            return status switch
            {
                MessageStatus.Accepted => new AcceptedTeamApplication(this),
                MessageStatus.Rejected => new RejectedTeamApplication(this),
                MessageStatus.Rescinded => new RescindedTeamApplication(this),
                MessageStatus.Expired => new ExpiredTeamApplication(this),
                MessageStatus.Pending => new PendingTeamApplication(this),
                _ => throw new InvalidEnumMemberException(status.ToString(), nameof(MessageStatus)),
            };
        }

        public async override Task<TeamApplicationDto?> SaveToDatabase()
        {
            using var scope = ScopeFactory.CreateScope();
            var teamApplicationService = scope.ServiceProvider.GetRequiredService<ITeamApplicationRepository>();
            var messageDto = await teamApplicationService.SaveMessage(MessageDto, CancellationToken);
            if (messageDto != null)
            {
                Status = messageDto.Status;
                UpdatedAt = messageDto.UpdatedAt;
            }
            return messageDto;
        }

        public async override Task TrySendToUser()
        {
            await Task.CompletedTask;
            //todo
        }
    }
}
