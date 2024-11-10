using Library.Models.Enums;

namespace Library.Models.API.UserMessaging
{
    public class ProfileMessageSubmitted
    {
        public Guid SenderId { get; set; }

        public Guid SendingUserId { get; set; }

        public Guid AcceptorId { get; set; }

        public Guid AcceptingUserId { get; set; }

        public PositionName PositionName { get; set; }

        public MessageType MessageType { get; set; }
    }
}
