using Library.Models.Enums;
using Service.Dtos.Message;

namespace Service.Repositories.Interfaces
{
    public interface IMessageRepository<TMessageDto> where TMessageDto : MessageDto
    {
        Task<bool> ClearMessages(ISet<MessageStatus> messageStatuses, CancellationToken cancellationToken);

        Task<TMessageDto?> GetMessage(Guid id, CancellationToken cancellationToken);

        Task<TMessageDto?> SaveMessage(TMessageDto messageDto, CancellationToken cancellationToken);

        Task<ICollection<TMessageDto>> GetUserMessages(Guid userId, ISet<MessageStatus> messageStatuses, CancellationToken cancellationToken);
    }
}
