namespace Service.Contracts.User
{
    public class CreateUserDto
    {
        public Guid Id { get; set; }

        public uint Mmr { get; set; }
    }
}
