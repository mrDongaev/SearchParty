using Library.Services.Interfaces.UserContextInterfaces;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Service.Dtos.Message;
using Service.Models.States.Interfaces;
using Service.Repositories.Interfaces;

namespace Service.Models.Message
{
    public class PlayerInvitation : AbstractMessage<PlayerInvitationDto>
    {
        public PlayerInvitation(PlayerInvitationDto messageDto, IServiceProvider serviceProvider, IUserHttpContext userContext, AbstractMessageState<PlayerInvitationDto> startingState, CancellationToken cancellationToken)
            : base(messageDto, serviceProvider, userContext, startingState, cancellationToken)
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

        public async override Task<PlayerInvitationDto?> SaveToDatabase()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var playerInvitationService = scope.ServiceProvider.GetRequiredService<IPlayerInvitationRepository>();
                var messageDto = await playerInvitationService.SaveMessage(MessageDto, CancellationToken);
                if (messageDto != null)
                {
                    Status = messageDto.Status;
                    UpdatedAt = messageDto.UpdatedAt;
                }
                return messageDto;
            }
        }
    }
}
