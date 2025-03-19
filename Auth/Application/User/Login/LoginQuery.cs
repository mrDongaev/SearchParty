using MediatR;

namespace Application.User.Login
{
    public class LoginQuery : IRequest<UserData>
    {
        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
