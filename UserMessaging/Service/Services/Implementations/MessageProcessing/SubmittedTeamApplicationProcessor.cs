using Library.Models.API.UserMessaging;
using Library.Services.Implementations.UserContextServices;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Service.Dtos.Message;
using Service.Models.Message;
using Service.Models.States.Implementations.PendingMessage;
using Service.Repositories.Interfaces;
using Service.Services.Interfaces.MessageProcessing;

namespace Service.Services.Implementations.MessageProcessing
{
    public class SubmittedTeamApplicationProcessor(IServiceProvider serviceProvider, IUserHttpContext userHttpContext) : SubmittedMessageAbstractProcessor<TeamApplicationDto>
    {
        protected override AbstractMessage<TeamApplicationDto> CreateMessage(ProfileMessageSubmitted submittedMessage)
        {
            TeamApplicationDto messageDto = new TeamApplicationDto(submittedMessage);
            TeamApplication message = new TeamApplication(messageDto, serviceProvider, userHttpContext, CancellationToken.None);
            message.ChangeState(new PendingTeamApplication(message));
            return message;
        }
    }
}
