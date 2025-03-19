using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.User.Registration
{
    public class RegistrationQuery : IRequest<UserData>
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
