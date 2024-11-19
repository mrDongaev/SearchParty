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
    public class PlayerInvitationManager : AbstractMessageManager<PlayerInvitationDto>
    {
        protected async override Task<AbstractMessage<PlayerInvitationDto>?> CreateMessageModel(Guid id, IServiceProvider serviceProvider, IUserHttpContext userContext, CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var playerInvitationRepository = scope.ServiceProvider.GetRequiredService<IPlayerInvitationRepository>();
            var messageDto = await playerInvitationRepository.GetMessage(id, cancellationToken);
            return messageDto == null ? null : new PlayerInvitation(messageDto, serviceProvider, userContext, cancellationToken);
        }
    }
}
