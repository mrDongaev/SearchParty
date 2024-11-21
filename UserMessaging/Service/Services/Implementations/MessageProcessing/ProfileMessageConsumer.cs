using Library.Models.API.UserMessaging;
using Library.Models.Enums;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Service.Repositories.Interfaces;

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
                        var playerInvitationRepo = scope.ServiceProvider.GetRequiredService<IPlayerInvitationRepository>();
                        var existingMessages = await playerInvitationRepo.GetUserMessages(receivedMessage.SendingUserId, new HashSet<MessageStatus> { MessageStatus.Pending }, CancellationToken.None);
                        if (existingMessages.SingleOrDefault(m => m.InvitingTeamId == receivedMessage.SenderId && m.AcceptingPlayerId == receivedMessage.AcceptorId) != null)
                        {
                            break;
                        }
                        var messageProcessor = scope.ServiceProvider.GetRequiredService<SubmittedPlayerInvitationProcessor>();
                        await messageProcessor.ProcessSubmittedMessage(receivedMessage);
                        break;
                    }
                case MessageType.TeamApplication:
                    {
                        var teamApplicationRepo = scope.ServiceProvider.GetRequiredService<ITeamApplicationRepository>();
                        var existingMessages = await teamApplicationRepo.GetUserMessages(receivedMessage.SendingUserId, new HashSet<MessageStatus> { MessageStatus.Pending }, CancellationToken.None);
                        if (existingMessages.SingleOrDefault(m => m.ApplyingPlayerId == receivedMessage.SenderId && m.AcceptingTeamId == receivedMessage.AcceptorId) != null)
                        {
                            break;
                        }
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
