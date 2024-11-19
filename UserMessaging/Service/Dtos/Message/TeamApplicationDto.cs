using Library.Models.API.UserMessaging;

namespace Service.Dtos.Message
{
    public class TeamApplicationDto : MessageDto
    {
        public TeamApplicationDto() : base()
        {
        }

        public TeamApplicationDto(ProfileMessageSubmitted message) : base(message)
        {
            ApplyingPlayerId = message.SenderId;
            AcceptingTeamId = message.AcceptorId;
        }

        public Guid ApplyingPlayerId { get; set; }

        public Guid AcceptingTeamId { get; set; }
    }
}
