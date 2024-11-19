using Library.Models.API.UserMessaging;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Service.Dtos.Message;
using Service.Models.Message;
using Service.Models.States.Implementations.PendingMessage;
using Service.Repositories.Interfaces;
using Service.Services.Interfaces.MessageProcessing;

namespace Service.Services.Implementations.MessageProcessing
{
    public class SubmittedPlayerInvitationProcessor(IServiceProvider serviceProvider, IUserHttpContext userHttpContext) : SubmittedMessageAbstractProcessor<PlayerInvitationDto>
    {
        protected override AbstractMessage<PlayerInvitationDto> CreateMessage(ProfileMessageSubmitted submittedMessage)
        {
            PlayerInvitationDto messageDto = new PlayerInvitationDto(submittedMessage);
            PlayerInvitation message = new PlayerInvitation(messageDto, serviceProvider, userHttpContext, CancellationToken.None);
            message.ChangeState(new PendingPlayerInvitation(message));
            return message;

        }
    }
}
