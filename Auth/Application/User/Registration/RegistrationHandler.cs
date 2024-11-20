using Application.Exceptions;
using Application.Interfaces;
using Application.User.ExternalModels;
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

        private readonly IUserInfoClient _userInfoClient;

        private readonly IMediator _mediator;

        public RegistrationHandler(UserManager<Domain.AppUser>? userManager, IUserInfoClient userInfoClient, IMediator mediator) 
        {
            _userManager = userManager;

            _userInfoClient = userInfoClient;

            _mediator = mediator;
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

                    Email = request.Email,

                    Id = Guid.NewGuid().ToString()
                };

                try
                {
                    var result = await _userManager.CreateAsync(userEF, request.Password.ToString());

                    if (result.Succeeded)
                    {
                        // Логин пользователя для получения токенов
                        var loginQuery = new LoginQuery
                        {
                            Email = request.Email,

                            Password = request.Password
                        };

                        var userData = await _mediator.Send(loginQuery);

                        //Создание нового пользователя через внешний API
                        var createUserInfoRequest = new CreateUserInfoRequest
                        {
                            Name = userEF.DisplayName,

                            Id = Guid.Parse(userEF.Id)
                        };

                        var response = await _userInfoClient.CreateUserInfoAsync(createUserInfoRequest, userData.AccessToken);

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception("Failed to create user in external service during registration.");
                        }
                    }
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
