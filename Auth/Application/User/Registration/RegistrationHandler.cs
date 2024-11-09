using Application.Exceptions;
using Application.Interfaces;
using Application.User.Login;
using Application.User.Refresh;
using Application.User.Settings;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Registration
{
    public class RegistrationHandler : IRequestHandler<RegistrationQuery, IdentityResult>
    {
        private readonly UserManager<Domain.AppUser>? _userManager;

        public RegistrationHandler(UserManager<Domain.AppUser>? userManager) 
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(RegistrationQuery request, CancellationToken cancellationToken)
        {
            var userEF = await _userManager.FindByNameAsync(request.Email);

            // Проверяем, есть ли пользователь с таким именем в базе данных
            if (userEF != null)
            {
                throw new InvalidOperationException("Пользователь с таким email уже существует");
            }
            else 
            {
                userEF = new AppUser 
                {
                    DisplayName = request.Username,

                    UserName = request.Username,

                    Email = request.Email
                };

                try
                {
                    var result = await _userManager.CreateAsync(userEF, request.Password.ToString());

                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return IdentityResult.Success;
        }
    }
}
