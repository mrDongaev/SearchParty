using Service.Dtos.Message;

namespace Service.Models
{
    public class PlayerInvitation : Message<PlayerInvitationDto>
    {
        public Guid AcceptingPlayerId { get; set; }

        public Guid InvitingTeamId { get; set; }

        public override void Accept()
        {
            throw new NotImplementedException();
        }

        public override void Expire()
        {
            throw new NotImplementedException();
        }

        public override void Reject()
        {
            throw new NotImplementedException();
        }

        public override void Rescind()
        {
            throw new NotImplementedException();
        }

        public override Task<PlayerInvitationDto?> SaveToDatabase()
        {
            throw new NotImplementedException();
        }

        public override Task TrySendToUser()
        {
            throw new NotImplementedException();
        }
    }
}
