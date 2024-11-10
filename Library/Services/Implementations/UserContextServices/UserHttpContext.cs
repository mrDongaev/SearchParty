using Library.Services.Interfaces.UserContextInterfaces;
using System.Security.Claims;

namespace Library.Services.Implementations.UserContextServices
{
    public class UserHttpContext : IUserHttpContext
    {
        public Guid UserId { get; set; }

        public List<Claim> Claims { get; set; } = [];

        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;
    }
}
