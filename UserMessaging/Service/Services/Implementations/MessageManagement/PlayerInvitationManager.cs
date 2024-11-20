using Library.Models.Enums;
using Library.Services.Interfaces.UserContextInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Service.Domain.Message;
using Service.Dtos.Message;
using Service.Repositories.Interfaces;
using Service.Services.Interfaces.MessageManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Implementations.MessageManagement
{
    public class PlayerInvitationManager(IServiceScopeFactory scopeFactory) : AbstractMessageManager<PlayerInvitationDto>(scopeFactory)
    {
        protected async override Task ClearResolvedMessages()
        {
            using var scope = scopeFactory.CreateScope();
            var playerInvitationRepository = scope.ServiceProvider.GetRequiredService<IPlayerInvitationRepository>();
            await playerInvitationRepository.ClearMessages(new HashSet<MessageStatus>()
            {
                MessageStatus.Expired,
                MessageStatus.Accepted,
                MessageStatus.Rejected,
                MessageStatus.Rescinded,
            }, CancellationToken.None);
        }

        protected async override Task<AbstractMessage<PlayerInvitationDto>?> CreateMessageModel(Guid id, IUserHttpContext userContext, CancellationToken cancellationToken)
        {
            using var scope = scopeFactory.CreateScope();
            var playerInvitationRepository = scope.ServiceProvider.GetRequiredService<IPlayerInvitationRepository>();
            var messageDto = await playerInvitationRepository.GetMessage(id, cancellationToken);
            return messageDto == null ? null : new PlayerInvitation(messageDto, scopeFactory, userContext, cancellationToken);
        }
    }
}
