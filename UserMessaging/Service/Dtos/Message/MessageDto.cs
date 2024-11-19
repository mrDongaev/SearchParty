using Library.Constants;
using Library.Models.API.UserMessaging;
using Library.Models.Enums;

namespace Service.Dtos.Message
{
    public abstract class MessageDto
    {
        public MessageDto()
        {
        }

        public MessageDto(ProfileMessageSubmitted message)
        {
            SendingUserId = message.SendingUserId;
            AcceptingUserId = message.AcceptingUserId;
            PositionName = message.PositionName;
            Status = MessageStatus.Pending;
            IssuedAt = DateTime.UtcNow;
            ExpiresAt = DateTime.UtcNow + SearchPartyConstants.MessageExpirationTime;
        }

        public Guid Id { get; set; }

        public Guid SendingUserId { get; set; }

        public Guid AcceptingUserId { get; set; }

        public PositionName PositionName { get; set; }

        public MessageStatus Status { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
