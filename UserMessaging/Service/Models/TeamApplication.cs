using Service.Dtos;

namespace Service.Models
{
    public class TeamApplication : Message<TeamApplicationDto>
    {
        public Guid ApplyingPlayerId { get; set; }

        public Guid AcceptingTeamId { get; set; }

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

        public override Task<TeamApplicationDto?> SaveToDatabase()
        {
            throw new NotImplementedException();
        }

        public override Task TrySendToUser()
        {
            throw new NotImplementedException();
        }
    }
}
