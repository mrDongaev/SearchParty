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
                cfg.AddProfile<RepoMapping.HeroMappingProfile>();
                cfg.AddProfile<RepoMapping.PositionMappingProfile>();
                cfg.AddProfile<RepoMapping.PlayerMappingProfile>();
                cfg.AddProfile<RepoMapping.TeamMappingProfile>();
                cfg.AddProfile<RepoMapping.UserMappingProfile>();
                cfg.AddProfile<HeroMappingProfile>();
                cfg.AddProfile<PositionMappingProfile>();
                cfg.AddProfile<PlayerMappingProfile>();
                cfg.AddProfile<TeamMappingProfile>();
                cfg.AddProfile<UserMappingProfile>();
            });

            configuration.AssertConfigurationIsValid();
            return configuration;
        }
    }
}
