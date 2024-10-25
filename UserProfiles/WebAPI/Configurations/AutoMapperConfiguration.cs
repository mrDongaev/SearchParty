using AutoMapper;
using WebAPI.Mapping;
using RepoMapping = Service.Mapping;


namespace WebAPI.Configurations
{
    public static class AutoMapperConfiguration
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddSingleton<IMapper>(new Mapper(Configuration()));
            return services;
        }

        private static MapperConfiguration Configuration()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RepoMapping.UserMappingProfile>();
                cfg.AddProfile<UserMappingProfile>();
            });

            configuration.AssertConfigurationIsValid();
            return configuration;
        }
    }
}
