using Library.Models.API.UserMessaging;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Service.Domain.Message;
using Service.Dtos.Message;
using Service.Services.Interfaces.MessageProcessing;

namespace Service.Services.Implementations.MessageProcessing
{
    public class SubmittedPlayerInvitationProcessor(IServiceScopeFactory scopeFactory, IUserHttpContext userHttpContext) : AbstractSubmittedMessageProcessor<PlayerInvitationDto>
    {
        public override AbstractMessage<PlayerInvitationDto> CreateMessage(ProfileMessageSubmitted submittedMessage)
        {
            PlayerInvitationDto messageDto = new PlayerInvitationDto(submittedMessage);
            PlayerInvitation message = new PlayerInvitation(messageDto, scopeFactory, userHttpContext, CancellationToken.None);
            return message;

        }
    }
}
