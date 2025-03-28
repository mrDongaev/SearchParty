﻿using Application.Exceptions;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Application.User.Login
{
    public class LoginHandler : IRequestHandler<LoginQuery, UserData>
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

        public async Task<UserData> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new RestException(HttpStatusCode.Unauthorized);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded)
            {
                var tokens = _jwtGenerator?.CreateJwtToken(user);

                return new UserData
                {
                    DisplayName = user.DisplayName,

                    AccessToken = tokens?.AccessToken,

                    RefreshToken = tokens?.RefreshToken,

                    Email = user.Email,

                    Id = user.Id
                };
            }
            throw new RestException(HttpStatusCode.Unauthorized);
        }
    }
}
