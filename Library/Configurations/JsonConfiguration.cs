using Library.Services.Implementations.UserContextServices;
using Library.Services.Interfaces.UserContextInterfaces;
using Library.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

namespace Library.Configurations
{
    public static class JsonConfiguration
    {
        public static IServiceCollection AddJsonConfiguration(this IServiceCollection services)
        {
            services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
            {
                options.SerializerOptions.PropertyNamingPolicy = null;
                options.SerializerOptions.PropertyNameCaseInsensitive = false;
            });

            services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
            });

            return services;
        }
    }
}
