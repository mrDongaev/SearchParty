using Microsoft.EntityFrameworkCore;
using Application.User.Login;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Domain;

namespace WebSearchPartyApi.StartingArea
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration) 
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services) 
        {
            services.AddDbContext<DataContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMediatR(typeof(LoginHandler).Assembly);

            var builder = services.AddIdentityCore<AppUser>();

            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);

            identityBuilder.AddEntityFrameworkStores<DataContext>();

            identityBuilder.AddSignInManager<DataContext>();

        }
        public void Configure(IApplicationBuilder app, Microsoft.Extensions.Hosting.IHostEnvironment env) 
        {
            app.UseAuthentication();
        }
    }
}
