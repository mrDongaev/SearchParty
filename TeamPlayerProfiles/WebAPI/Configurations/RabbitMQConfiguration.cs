using MassTransit;

namespace WebAPI.Configurations
{
    public static class RabbitMQConfiguration
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    ConfigureRmq(cfg, configuration);
                });
            });

            return services;
        }

        /// <summary>
        /// Конфигурирование RMQ.
        /// </summary>
        /// <param name="configurator"> Конфигуратор RMQ. </param>
        /// <param name="configuration"> Конфигурация приложения. </param>
        private static void ConfigureRmq(IRabbitMqBusFactoryConfigurator configurator, IConfiguration configuration)
        {
            RmqSettings rmqSettings = configuration.GetSection("RmqSettings").Get<RmqSettings>();

            configurator.Host(rmqSettings.Host,
                rmqSettings.VHost,
                h =>
                {
                    h.Username(rmqSettings.Login);
                    h.Password(rmqSettings.Password);
                });
        }

        public class RmqSettings
        {
            public string Host { get; set; }
            public string VHost { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
        }
    }
}
