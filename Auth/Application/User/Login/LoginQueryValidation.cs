using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.User.Login
{
    public class LoginQueryValidation : AbstractValidator<LoginQuery>
    {
        public LoginQueryValidation()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();

            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
