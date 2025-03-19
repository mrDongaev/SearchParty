using MediatR;

namespace Application.User.Registration
{
    public class RegistrationQuery : IRequest<UserData>
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
