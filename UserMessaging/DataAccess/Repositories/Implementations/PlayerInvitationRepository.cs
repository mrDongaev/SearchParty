using DataAccess.Context;
using DataAccess.Entities;
using Library.Models.Enums;
using Library.Repositories.Implementations;
using Service.Models;
using Service.Repositories.Interfaces;

namespace DataAccess.Repositories.Implementations
{
    public class PlayerInvitationRepository(UserMessagingContext context) : Repository<UserMessagingContext, PlayerInvitation, Guid>(context), IPlayerInvitationRepository
    {
        public Task ClearMessages(ISet<MessageType> messageTypes, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IMessage> GetMessage(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IMessage> GetUserMessages(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IMessage> SaveMessage(IMessage message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
