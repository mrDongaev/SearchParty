using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Application.User;

namespace Application.User.Login
{
    public class LoginQuery : IRequest<User>
    {
        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
