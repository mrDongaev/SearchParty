using Library.Utils;
using MassTransit;
using Service.Services.Implementations.MessageProcessing;

namespace WebAPI.Configurations
{
    public static class RabbitMQConfiguration
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<ProfileMessageConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    ConfigureRmq(cfg);

                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddOptions<MassTransitHostOptions>()
                .Configure(options =>
                {
                    options.WaitUntilStarted = true;
                    options.StartTimeout = TimeSpan.FromSeconds(10);
                });

            return services;
        }

        /// <summary>
        /// Конфигурирование RMQ.
        /// </summary>
        /// <param name="configurator"> Конфигуратор RMQ. </param>
        /// <param name="configuration"> Конфигурация приложения. </param>
        private static void ConfigureRmq(IRabbitMqBusFactoryConfigurator configurator)
        {
            configurator.Host(
                EnvironmentUtils.GetEnvVariable("RABBITMQ_HOST"),
                EnvironmentUtils.GetEnvVariable("RABBITMQ_VHOST"),
                h =>
                {
                    h.Username(EnvironmentUtils.GetEnvVariable("RABBITMQ_LOGIN"));
                    h.Password(EnvironmentUtils.GetEnvVariable("RABBITMQ_PASSWORD"));
                });
        }
    }
}
