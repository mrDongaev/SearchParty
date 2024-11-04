﻿using Library.Utils;
using Service.Services.Implementations;
using Service.Services.Implementations.UserProfileServices;
using Service.Services.Interfaces;
using Service.Services.Interfaces.UserProfilesInterfaces;

namespace WebAPI.Configurations
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddScoped<IUserService, UserService>()
                .AddHttpClient<IUserProfileService, UserProfilesService>(cfg =>
                {
                    Uri baseAddress;
                    if (EnvironmentUtils.TryGetEnvVariable("CONTAINER").Equals("true"))
                    {
                        baseAddress = new Uri(EnvironmentUtils.GetEnvVariable("USER_PROFILES_URL"));
                    }
                    else
                    {
                        HttpClientSettings httpClientSettings = configuration.GetSection("HttpClientSettings").Get<HttpClientSettings>();
                        baseAddress = new Uri(httpClientSettings.UserProfilesUrl);
                    }
                    cfg.BaseAddress = baseAddress;
                    cfg.DefaultRequestHeaders.Add("Accept", "text/plain");
                });
            return services;
        }
    }

    public class HttpClientSettings
    {
        public string UserProfilesUrl { get; set; }
    }
}