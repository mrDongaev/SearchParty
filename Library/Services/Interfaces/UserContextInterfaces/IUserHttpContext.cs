using System.Security.Claims;

namespace Library.Services.Interfaces.UserContextInterfaces
{
    public interface IUserHttpContext
    {
        public Guid UserId { get; set; }

        public List<Claim> Claims { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
