using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User.Login
{
    public class LoginHandler : IRequestHandler<LoginQuery, User>
    {
        private readonly UserManager<Domain.AppUser>? _userManager;

        private readonly SignInManager<Domain.AppUser>? _signInManager;

        private readonly IJwtGenerator? _jwtGenerator;

        public LoginHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtGenerator jwtGenerator)
        {
            _userManager = userManager;

            _signInManager = signInManager;

            _jwtGenerator = jwtGenerator;
        }

        public async Task<User> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new RestException(HttpStatusCode.Unauthorized);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded)
            {
                return new User
                {
                    UserDisplayName = user.DisplayName,

                    UserToken = _jwtGenerator?.CreateToken(user),

                    UserFullName = user.UserName,

                    UserImage = null
                };
            }

            throw new RestException(HttpStatusCode.Unauthorized);
        }
    }
}
