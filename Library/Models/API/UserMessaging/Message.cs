using Library.Models.Enums;

namespace Library.Models.API.UserMessaging
{
    public class Message
    {
        public Guid SenderId { get; set; }

        public Guid SendingUserId { get; set; }

        public Guid AcceptorId { get; set; }

        public Guid AcceptingUserId { get; set; }

        public MessageType MessageType { get; set; }
    }
}
