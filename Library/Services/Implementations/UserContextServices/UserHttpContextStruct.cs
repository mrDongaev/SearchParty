using Library.Services.Interfaces.UserContextInterfaces;

namespace Library.Services.Implementations.UserContextServices
{
    public record struct UserHttpContextStruct : IUserHttpContext
    {
        public Guid UserId { get; set; }

        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public UserHttpContextStruct(Guid userId, string accessToken, string refreshToken)
        {
            UserId = userId;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public IUserHttpContext GetPersistentData()
        {
            return new UserHttpContextStruct(UserId, AccessToken, RefreshToken);
        }
    }
}
