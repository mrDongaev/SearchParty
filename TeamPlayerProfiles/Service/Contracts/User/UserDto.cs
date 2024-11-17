namespace Service.Contracts.User
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public uint Mmr { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
