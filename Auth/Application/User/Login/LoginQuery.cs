using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.User.Login
{
    public class LoginQuery : IRequest<UserData>
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
