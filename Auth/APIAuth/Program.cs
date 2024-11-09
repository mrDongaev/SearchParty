using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using WebSearchPartyApi;
using Domain;
using EFData;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Application.User.Login;
using MediatR;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Application.Interfaces;
using Infrastructure.Security;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace APIAuth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a web application with default settings
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the dependency injection container
            ConfigureServices(builder.Services, builder.Configuration);

            // Build the web application
            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Show developer page in case of an error
            }

            // Use and map middleware
            UseAndMapMiddleware(app);

            // Run migrations and seed database
            MigrationProcessing(app);

            app.Run();
        }

        public static void UseAndMapMiddleware(WebApplication app)
        {
            app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS

            app.UseAuthentication();

            app.UseAuthorization(); // Enable authorization

            app.MapControllers(); // Map controllers

            //app.UseMiddleware<RefreshTokenMiddleware>();
        }

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Configure Identity for user and role management
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false; // Do not require account confirmation upon sign-in

                options.Password.RequireNonAlphanumeric = false; // Disable requirement for non-alphanumeric characters in the password
            })
            .AddEntityFrameworkStores<DataContext>() // Use Entity Framework for Identity data storage
            .AddDefaultTokenProviders(); // Add default token providers

            // Call AddAuthProcess to add authentication related services
            AddAuthProcess(services);
        }

        public static void AddAuthProcess(IServiceCollection services)
        {
            // Add user manager as a transient dependency
            services.AddTransient<UserManager<AppUser>>();

            // Add sign-in manager as a transient dependency
            services.AddTransient<SignInManager<AppUser>>();

            services.AddTransient<IJwtGenerator, JwtGenerator>();

            services.AddTransient<IRefreshGenerator, RefreshGenerator>();

            // Add controllers
            services.AddControllers();

            // Add API Explorer for API documentation generation
            services.AddEndpointsApiExplorer();

            // Register MediatR with the assembly containing handlers
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(LoginHandler).Assembly);
            });

            AddAuthAndBearer(services);
        }

        public static void AddAuthAndBearer(IServiceCollection services)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TOKEN_KEY")
    ?? throw new ArgumentNullException("Token key not found in server environment variables"));

            var _key = new SymmetricSecurityKey(keyByte);

            // Add authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Indicates whether to validate the token's signature.
                    // If true, the token must be signed with the key specified in IssuerSigningKey.
                    ValidateIssuerSigningKey = true,

                    // The key that will be used to validate the token's signature.
                    // This should be an instance of a class that implements the SecurityKey interface.
                    IssuerSigningKey = _key,

                    // Indicates whether to validate the token's issuer.
                    // If true, it will be checked that the token was issued by a trusted issuer.
                    ValidateIssuer = false,

                    // Indicates whether to validate the token's audience.
                    // If true, it will be checked that the token is intended for a specific audience.
                    ValidateAudience = false
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();

                        logger.LogError(context.Exception, "Authentication failed.");

                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        // Skip standard response handling
                        context.HandleResponse();

                        // Create custom response
                        context.Response.StatusCode = 401;

                        context.Response.ContentType = "application/json";

                        var result = JsonSerializer.Serialize(new { message = "You are not authorized. Please provide a valid JWT token." });

                        return context.Response.WriteAsync(result);
                    }
                };
            });
        }

        public static void MigrationProcessing(WebApplication app)
        {
            // Create a scope for running database migrations
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<DataContext>(); // Get the database context

                    var userManager = services.GetRequiredService<UserManager<AppUser>>(); // Get the user manager

                    context.Database.Migrate(); // Apply database migrations

                    DataSeed.SeedDataAsync(context, userManager).Wait(); // Seed the database with initial data
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>(); // Get the logger

                    logger.LogError(ex, "An error occurred during migration"); // Log migration error
                }
            }
        }
    }
}