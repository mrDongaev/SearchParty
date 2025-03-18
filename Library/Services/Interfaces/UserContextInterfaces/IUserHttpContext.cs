namespace Library.Services.Interfaces.UserContextInterfaces
{
    public interface IUserHttpContext
    {
        public Guid UserId { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public IUserHttpContext GetPersistentData();
    }
}
