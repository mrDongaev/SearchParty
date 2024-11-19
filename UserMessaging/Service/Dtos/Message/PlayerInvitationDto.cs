using Library.Models.API.UserMessaging;

namespace Service.Dtos.Message
{
    public class PlayerInvitationDto : MessageDto
    {
        public PlayerInvitationDto() : base()
        {
        }

        public PlayerInvitationDto(ProfileMessageSubmitted message) : base(message)
        {
            InvitingTeamId = message.SenderId;
            AcceptingPlayerId = message.AcceptorId;
        }
        public Guid InvitingTeamId { get; set; }

        public Guid AcceptingPlayerId { get; set; }
    }
}
