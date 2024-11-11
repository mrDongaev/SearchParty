using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Refresh
{
    public class RefreshQuery : IRequest<UserData>
    {
        public string? Email { get; set; }
        public string? RefreshToken { get; set; }
    }
}
