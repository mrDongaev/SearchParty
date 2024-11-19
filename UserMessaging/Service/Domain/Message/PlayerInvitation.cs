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
    public class PlayerInvitation : AbstractMessage<PlayerInvitationDto>
    {
        public PlayerInvitation(PlayerInvitationDto messageDto, IServiceProvider serviceProvider, IUserHttpContext userContext, CancellationToken cancellationToken)
            : base(messageDto, serviceProvider, userContext, cancellationToken)
        {
            AcceptingPlayerId = messageDto.AcceptingPlayerId;
            InvitingTeamId = messageDto.InvitingTeamId;
        }

        public Guid AcceptingPlayerId { get; protected set; }

        public Guid InvitingTeamId { get; protected set; }

        public override PlayerInvitationDto MessageDto => new PlayerInvitationDto
        {
            Id = Id,
            AcceptingUserId = AcceptingUserId,
            AcceptingPlayerId = AcceptingPlayerId,
            InvitingTeamId = InvitingTeamId,
            SendingUserId = SendingUserId,
            PositionName = PositionName,
            Status = Status,
            IssuedAt = IssuedAt,
            ExpiresAt = ExpiresAt,
            UpdatedAt = UpdatedAt,
        };

        protected override AbstractMessageState<PlayerInvitationDto> CreateNewMessageState(MessageStatus status)
        {
            return status switch
            {
                MessageStatus.Accepted => new AcceptedPlayerInvitation(this),
                MessageStatus.Rejected => new RejectedPlayerInvitation(this),
                MessageStatus.Rescinded => new RescindedPlayerInvitation(this),
                MessageStatus.Expired => new ExpiredPlayerInvitation(this),
                MessageStatus.Pending => new PendingPlayerInvitation(this),
                _ => throw new InvalidEnumMemberException(status.ToString(), nameof(MessageStatus)),
            };
        }

        public async override Task<PlayerInvitationDto?> SaveToDatabase()
        {
            using var scope = ServiceProvider.CreateScope();
            var playerInvitationService = scope.ServiceProvider.GetRequiredService<IPlayerInvitationRepository>();
            var messageDto = await playerInvitationService.SaveMessage(MessageDto, CancellationToken);
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
