using AutoMapper;


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
            });

            configuration.AssertConfigurationIsValid();
            return configuration;
        }
    }
}
