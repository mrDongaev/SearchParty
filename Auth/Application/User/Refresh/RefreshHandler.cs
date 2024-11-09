using Application.Exceptions;
using Application.Interfaces;
using Application.User.Login;
using Application.User.Settings;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Refresh
{
    public class RefreshHandler : IRequestHandler<RefreshQuery, UserData>
    {
        private readonly UserManager<Domain.AppUser>? _userManager;

        private readonly IRefreshGenerator _refreshGenerator;

        public RefreshHandler(UserManager<Domain.AppUser> userManager, IRefreshGenerator refreshGenerator)
        {
            _userManager = userManager;

            _refreshGenerator = refreshGenerator;
        }

        public async Task<UserData> Handle(RefreshQuery request, CancellationToken cancellationToken)
        {
            //var tokenHandler = new JwtSecurityTokenHandler();

            var userEF = await _userManager.FindByEmailAsync(request.RefreshToken);

            if (userEF == null)
            {
                throw new RestException(HttpStatusCode.Unauthorized);
            }
            else 
            {
                var userData = new UserData
                {
                    DisplayName = userEF.DisplayName,

                    Email = userEF.Email,

                    Id = userEF.Id,

                    RefreshToken = request.RefreshToken
                };

                userData = _refreshGenerator.RefreshToken(userEF, userData);

                return userData;
            }
        }
    }
}
