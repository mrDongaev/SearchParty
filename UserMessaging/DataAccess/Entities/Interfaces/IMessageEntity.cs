using Library.Entities.Interfaces;
using Library.Models.Enums;

namespace DataAccess.Entities.Interfaces
{
    public interface IMessageEntity : IEntity<Guid>, IUpdateable
    {
        public Guid SendingUserId { get; set; }

        public Guid AcceptingUserId { get; set; }

        public PositionName PositionName { get; set; }

        public MessageStatus Status { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}
