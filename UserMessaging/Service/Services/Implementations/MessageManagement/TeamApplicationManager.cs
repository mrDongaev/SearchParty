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
using System.Threading;
using System.Threading.Tasks;

namespace Service.Services.Implementations.MessageManagement
{
    public class TeamApplicationManager(IServiceScopeFactory scopeFactory) : AbstractMessageManager<TeamApplicationDto>(scopeFactory)
    {
        protected async override Task ClearResolvedMessages()
        {
            using var scope = scopeFactory.CreateScope();
            var teamApplicationRepository = scope.ServiceProvider.GetRequiredService<ITeamApplicationRepository>();
            await teamApplicationRepository.ClearMessages(new HashSet<MessageStatus>()
            {
                MessageStatus.Expired,
                MessageStatus.Accepted,
                MessageStatus.Rejected,
                MessageStatus.Rescinded,
            }, CancellationToken.None);
        }

        protected async override Task<AbstractMessage<TeamApplicationDto>?> CreateMessageModel(Guid id, IUserHttpContext userContext, CancellationToken cancellationToken)
        {
            using var scope = scopeFactory.CreateScope();
            var teamApplicationRepository = scope.ServiceProvider.GetRequiredService<ITeamApplicationRepository>();
            var messageDto = await teamApplicationRepository.GetMessage(id, cancellationToken);
            return messageDto == null ? null : new TeamApplication(messageDto, scopeFactory, userContext, cancellationToken);
        }
    }
}
