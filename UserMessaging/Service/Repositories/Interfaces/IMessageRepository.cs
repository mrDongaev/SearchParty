using Library.Models.Enums;
using Service.Dtos.Message;

namespace Service.Repositories.Interfaces
{
    public interface IMessageRepository<TMessageDto> where TMessageDto : MessageDto
    {
        Task ClearMessages(ISet<MessageStatus> messageTypes, CancellationToken cancellationToken);

        Task<TMessageDto?> GetMessage(Guid id, CancellationToken cancellationToken);

        Task<TMessageDto?> SaveMessage(TMessageDto message, CancellationToken cancellationToken);

        Task<ICollection<TMessageDto>> GetUserMessages(Guid userId, MessageStatus messageType, CancellationToken cancellationToken);
    }
}
