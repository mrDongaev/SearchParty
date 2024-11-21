using Library.Models.API.UserMessaging;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Service.Domain.Message;
using Service.Dtos.Message;
using Service.Services.Interfaces.MessageProcessing;

namespace Service.Services.Implementations.MessageProcessing
{
    public class SubmittedTeamApplicationProcessor(IServiceScopeFactory scopeFactory, IUserHttpContext userHttpContext) : AbstractSubmittedMessageProcessor<TeamApplicationDto>
    {
        public override AbstractMessage<TeamApplicationDto> CreateMessage(ProfileMessageSubmitted submittedMessage)
        {
            TeamApplicationDto messageDto = new TeamApplicationDto(submittedMessage);
            TeamApplication message = new TeamApplication(messageDto, scopeFactory, userHttpContext, CancellationToken.None);
            return message;
        }
    }
}
