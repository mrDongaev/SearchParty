using FluentResults;
using Library.Models.Enums;

namespace Library.Results.Successes.Message
{
    public class MessageSentSuccess : Success
    {
        public MessageSentSuccess() : base("Message has been sent") { }

        public MessageSentSuccess(MessageType type) : base($"{(type == MessageType.PlayerInvitation ? "Team invitation" : "Team application")} has been sent") { }
    }
}
