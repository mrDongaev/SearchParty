using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.User.Refresh
{
    public class RefreshQuery : IRequest<UserData>
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
