using Library.Models.API.UserMessaging;
using Library.Services.Interfaces.UserContextInterfaces;
using Service.Domain.Message;
using Service.Domain.States.Implementations.PendingMessage;
using Service.Dtos.Message;
using Service.Services.Interfaces.MessageProcessing;

namespace Service.Services.Implementations.MessageProcessing
{
    public class SubmittedPlayerInvitationProcessor(IServiceProvider serviceProvider, IUserHttpContext userHttpContext) : AbstractSubmittedMessageProcessor<PlayerInvitationDto>
    {
        public override AbstractMessage<PlayerInvitationDto> CreateMessage(ProfileMessageSubmitted submittedMessage)
        {
            PlayerInvitationDto messageDto = new PlayerInvitationDto(submittedMessage);
            PlayerInvitation message = new PlayerInvitation(messageDto, serviceProvider, userHttpContext, CancellationToken.None);
            return message;

        }
    }
}
