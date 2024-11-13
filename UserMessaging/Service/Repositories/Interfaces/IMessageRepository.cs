using Library.Models.Enums;
using Service.Models;

namespace Service.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task ClearMessages(ISet<MessageType> messageTypes, CancellationToken cancellationToken);

        Task<IMessage> GetMessage(Guid id, CancellationToken cancellationToken);

        Task<IMessage> GetUserMessages(Guid userId, CancellationToken cancellationToken);

        Task<IMessage> SaveMessage(IMessage message, CancellationToken cancellationToken);
    }
}
