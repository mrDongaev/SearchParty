using FluentResults;
using Library.Models.Enums;
using Service.Dtos.Message;

namespace Service.Services.Interfaces.MessageInteraction
{
    public interface IMessageInteractionService<TMessageDto> where TMessageDto : MessageDto
    {
        Task<Result<TMessageDto?>> GetMessage(Guid id, CancellationToken cancellationToken);

        Task<Result<ICollection<TMessageDto>>> GetUserMessages(ISet<MessageStatus> messageStatuses, CancellationToken cancellationToken);

        Task<Result<TMessageDto?>> Accept(Guid id, CancellationToken cancellationToken);

        Task<Result<TMessageDto?>> Reject(Guid id, CancellationToken cancellationToken);

        Task<Result<TMessageDto?>> Rescind(Guid id, CancellationToken cancellationToken);
    }
}
