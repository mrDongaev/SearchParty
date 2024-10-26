using Service.Services.Implementations;
using Service.Services.Interfaces;

namespace WebAPI.Configurations
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
