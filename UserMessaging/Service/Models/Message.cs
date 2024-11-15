using Service.Dtos;

namespace Service.Models
{
    public abstract class Message<TMessageDto> where TMessageDto : MessageDto
    {
        public Guid SendingUserId { get; set; }

        public Guid AcceptingUserId { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public abstract void Accept();

        public abstract void Reject();

        public abstract void Expire();

        public abstract void Rescind();

        public abstract Task TrySendToUser();

        public abstract Task<TMessageDto?> SaveToDatabase();
    }
}
