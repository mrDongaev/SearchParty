namespace Service.Services.Interfaces
{
    public interface IMessageService<TMessageDto>
    {
        Task<TMessageDto?> GetMessage(Guid id, CancellationToken cancellationToken);

        Task<ICollection<TMessageDto>> GetPendingUserMessages(Guid userId, CancellationToken cancellationToken);
    }
}
