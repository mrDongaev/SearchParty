using Library.Models.Enums;
using Service.Dtos.Message;

namespace Service.Repositories.Interfaces
{
    public interface IMessageRepository<T> where T : MessageDto
    {
        Task ClearMessages(ISet<MessageStatus> messageTypes, CancellationToken cancellationToken);

        Task<T?> GetMessage(Guid id, CancellationToken cancellationToken);

        Task<T?> SaveMessage(T message, CancellationToken cancellationToken);

        Task<ICollection<T>> GetUserMessages(Guid userId, MessageStatus messageType, CancellationToken cancellationToken);
    }
}
