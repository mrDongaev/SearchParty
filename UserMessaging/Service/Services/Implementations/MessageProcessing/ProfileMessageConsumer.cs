using Library.Models.API.UserMessaging;
using Library.Models.Enums;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Service.Dtos;
using Service.Repositories.Interfaces;
using Service.Services.Interfaces;

namespace Service.Services.Implementations.MessageProcessing
{
    public class ProfileMessageConsumer(IServiceProvider serviceProvider) : IConsumer<ProfileMessageSubmitted>
    {
        public int consumeCounter = 0;

        public async Task Consume(ConsumeContext<ProfileMessageSubmitted> context)
        {
            var receivedMessage = context.Message;
            consumeCounter++;
            using (var scope = serviceProvider.CreateScope())
            {
                switch (receivedMessage.MessageType)
                {
                    case MessageType.PlayerApplication:
                        {
                            var repo = scope.ServiceProvider.GetRequiredService<IPlayerInvitationRepository>();
                            var messageProcessor = new SubmittedPlayerInvitationProcessor(repo);
                            await messageProcessor.ProcessSubmittedMessage(receivedMessage);
                            break;
                        }

                    case MessageType.TeamInvitation:
                        {
                            var repo = scope.ServiceProvider.GetRequiredService<ITeamApplicationRepository>();
                            var messageProcessor = new SubmittedTeamApplicationProcessor(repo);
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
