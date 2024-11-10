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
using Library.Utils;
using Microsoft.OpenApi.Models;
using System.Reflection;

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
            app.UseSwagger();
            app.UseSwaggerUI();
            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Show developer page in case of an error
            }

            // Use and map middleware
            UseAndMapMiddleware(app);

            // Run migrations and seed database
            if (app.Environment.IsDevelopment())
            {
                MigrationProcessing(app);
            }

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

            AddSwagger(services);
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
            byte[] keyByte = Encoding.UTF8.GetBytes(EnvironmentUtils.GetEnvVariable("TOKEN_KEY"));

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

        public static IServiceCollection AddSwagger(IServiceCollection services)
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            Dictionary<string, int> counter = new Dictionary<string, int>();

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Description = "SearchParty Authentication and authorization API v1",
                    Title = "Swagger",
                    Version = "1.0.0"
                });
                options.CustomSchemaIds(type =>
                {
                    var name = type.Name;
                    var declaringName = type.DeclaringType?.Name ?? string.Empty;
                    if (declaringName != string.Empty) declaringName += ".";
                    var final = declaringName + name;
                    if (counter.ContainsKey(final))
                    {
                        counter[final] += 1;
                        final += $"({counter[final]})";
                    }
                    else
                    {
                        counter.Add(final, 0);
                    }
                    return final;
                });
            });
            return services;
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