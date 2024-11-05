using Serilog;

namespace WebAPI.Configurations
{
    public static class LoggerConfiguration
    {
        public static IHostBuilder AddSerilog(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                .ReadFrom.Configuration(ctx.Configuration));
            return hostBuilder;
        }
    }
}
