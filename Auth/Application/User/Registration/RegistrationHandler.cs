﻿using Application.Interfaces;
using Application.User.ExternalModels;
using Application.User.Login;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User.Registration
{
    public class RegistrationHandler : IRequestHandler<RegistrationQuery, UserData>
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

        public async Task<UserData> Handle(RegistrationQuery request, CancellationToken cancellationToken)
        {
            var userEF = await _userManager.FindByNameAsync(request.Email);
            UserData userData = new();

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

                        userData = await _mediator.Send(loginQuery);

                        //Создание нового пользователя через внешний API
                        var createUserInfoRequest = new CreateUserInfoRequest
                        {
                            Name = userEF.DisplayName,

                            Id = Guid.Parse(userEF.Id)
                        };

                        var response = await _userInfoClient.CreateUserInfoAsync(createUserInfoRequest, userData.AccessToken, userData.RefreshToken);

                        if (!response.IsSuccessStatusCode)
                        {
                            await _userManager.DeleteAsync(userEF);
                            throw new Exception("Failed to create user in external service during registration.");
                        }
                    }
                    else
                    {
                        throw new Exception("Failed to create user in internal service during registration.");
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return userData;
        }
    }
}
