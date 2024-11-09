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
    public class RegistrationHandler : IRequestHandler<RegistrationQuery, Unit>
    {
        private readonly UserManager<Domain.AppUser>? _userManager;

        private readonly IRegistration _registration;

        public RegistrationHandler(UserManager<Domain.AppUser>? userManager, IRegistration registration) 
        {
            _userManager = userManager;

            _registration = registration;
        }

        public async Task<Unit> Handle(RegistrationQuery request, CancellationToken cancellationToken)
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
                    DisplayName = "1",

                    UserName = "1",

                    Email = "1"
                };

                await _userManager.CreateAsync(userEF, "12345678");

                //await _registration.SaveChangesToDbAsync();
            }
            return Unit.Value;
        }
    }
}
