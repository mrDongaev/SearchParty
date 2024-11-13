using Service.Repositories.Interfaces;

namespace Service.Models
{
    public interface IMessage
    {
        public Guid SenderId { get; set; }

        public Guid SendingUserId { get; set; }

        public Guid AcceptorId { get; set; }

        public Guid AcceptingUserId { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public void Accept();

        public void Reject();

        public void Expire();

        public void Rescind();

        public Task<IMessage?> SaveToDatabase(IMessageRepository messageRepository);

        public Task TrySendToUser();
    }
}
