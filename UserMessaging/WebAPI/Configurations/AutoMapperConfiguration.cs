using AutoMapper;
using WebAPI.Mapping;
using DaoMapping = DataAccess.Mapping;


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
                cfg.AddProfile(new DaoMapping.PlayerInvitationMappingProfile());
                cfg.AddProfile(new DaoMapping.TeamApplicationMappingProfile());
                cfg.AddProfile(new PlayerInvitationMappingProfile());
                cfg.AddProfile(new TeamApplicationMappingProfile());

            });

            configuration.AssertConfigurationIsValid();
            return configuration;
        }
    }
}
