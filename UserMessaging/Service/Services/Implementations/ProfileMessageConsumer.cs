using Library.Models.API.UserMessaging;
using Library.Models.Enums;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Service.Repositories.Interfaces;
using Service.Services.Interfaces;

namespace Service.Services.Implementations
{
    public class ProfileMessageConsumer(SubmittedMessageAbstractProcessor messageProcessor, IServiceProvider serviceProvider, ILogger logger) : IConsumer<ProfileMessageSubmitted>
    {
        public async Task Consume(ConsumeContext<ProfileMessageSubmitted> context)
        {
            var receivedMessage = context.Message;
            logger.LogInformation($"Received message {receivedMessage.SenderId} {receivedMessage.AcceptorId}");
            SubmittedMessageAbstractProcessor messageProcessor;
            using (var scope = serviceProvider.CreateScope())
            {
                switch (receivedMessage.MessageType)
                {
                    case MessageType.PlayerApplication:
                        {
                            var repo = scope.ServiceProvider.GetRequiredService<IPlayerInvitationRepository>();
                            messageProcessor = new SubmittedPlayerInvitationProcessor(repo);
                            await messageProcessor.ProcessSubmittedMessage(receivedMessage);
                            break;
                        }

                    case MessageType.TeamInvitation:
                        {
                            var repo = scope.ServiceProvider.GetRequiredService<ITeamApplicationRepository>();
                            messageProcessor = new SubmittedTeamApplicationProcessor(repo);
                            await messageProcessor.ProcessSubmittedMessage(receivedMessage);
                            break;
                        }
                    default:
                        break;
                }
            }
        }
    }
}
