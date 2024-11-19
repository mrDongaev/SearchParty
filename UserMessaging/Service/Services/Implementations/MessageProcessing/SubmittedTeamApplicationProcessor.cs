using Library.Models.API.UserMessaging;
using Library.Services.Interfaces.UserContextInterfaces;
using Service.Domain.Message;
using Service.Domain.States.Implementations.PendingMessage;
using Service.Dtos.Message;
using Service.Services.Interfaces.MessageProcessing;

namespace Service.Services.Implementations.MessageProcessing
{
    public class SubmittedTeamApplicationProcessor(IServiceProvider serviceProvider, IUserHttpContext userHttpContext) : AbstractSubmittedMessageProcessor<TeamApplicationDto>
    {
        public override AbstractMessage<TeamApplicationDto> CreateMessage(ProfileMessageSubmitted submittedMessage)
        {
            TeamApplicationDto messageDto = new TeamApplicationDto(submittedMessage);
            TeamApplication message = new TeamApplication(messageDto, serviceProvider, userHttpContext, CancellationToken.None);
            message.ChangeState(new PendingTeamApplication(message));
            return message;
        }
    }
}
