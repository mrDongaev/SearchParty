using Library.Models.API.UserMessaging;
using Library.Models.Enums;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Service.Services.Implementations.MessageProcessing
{
    public class ProfileMessageConsumer(IServiceProvider serviceProvider) : IConsumer<ProfileMessageSubmitted>
    {
        public async Task Consume(ConsumeContext<ProfileMessageSubmitted> context)
        {
            var receivedMessage = context.Message;
            using var scope = serviceProvider.CreateScope();
            switch (receivedMessage.MessageType)
            {
                case MessageType.PlayerInvitation:
                    {
                        var messageProcessor = scope.ServiceProvider.GetRequiredService<SubmittedPlayerInvitationProcessor>();
                        await messageProcessor.ProcessSubmittedMessage(receivedMessage);
                        break;
                    }
                case MessageType.TeamApplication:
                    {
                        var messageProcessor = scope.ServiceProvider.GetRequiredService<SubmittedTeamApplicationProcessor>();
                        await messageProcessor.ProcessSubmittedMessage(receivedMessage);
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
