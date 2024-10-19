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

namespace APIAuth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // �������� ���-���������� � ����������� �� ���������
            var builder = WebApplication.CreateBuilder(args);

            // ���������� �������� � ��������� ������������
            ConfigureServices(builder.Services, builder.Configuration);

            // ���������� ���-����������
            var app = builder.Build();

            // ������������ ��������� ��������� HTTP-��������
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // ����� �������� ������������ � ������ ������
            }

            app.UseHttpsRedirection(); // ��������������� HTTP-�������� �� HTTPS

            app.UseAuthentication();

            app.UseAuthorization(); // ��������� �����������

            app.MapControllers(); // ������������� ������������

            // �������� ������� ��������� ��� ���������� �������� ���� ������
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<DataContext>(); // ��������� ��������� ���� ������

                    var userManager = services.GetRequiredService<UserManager<AppUser>>(); // ��������� ��������� �������������

                    context.Database.Migrate(); // ���������� �������� � ���� ������

                    DataSeed.SeedDataAsync(context, userManager).Wait(); // ���������� ���� ������ ���������� �������
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>(); // ��������� �������

                    logger.LogError(ex, "An error occurred during migration"); // ����������� ������ ��������
                }
            }

            // ������� ������� ��� �������� ������ ����������
            //app.MapGet("/start", () => "");

            // ������ ����������
            app.Run();
        }

        // ����� ��� ��������� ��������
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // ��������� ��������� ���� ������ � �������������� ������ �����������
            services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // ��������� Identity ��� ���������� �������������� � ������
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false; // ���������� ������������� �������� ��� �����

                options.Password.RequireNonAlphanumeric = false; // ���������� ���������� ������������ �������� � ������
            })
            .AddEntityFrameworkStores<DataContext>() // ������������� Entity Framework ��� �������� ������ Identity

            .AddDefaultTokenProviders(); // ���������� ����������� ������� �� ���������

            // ���������� ��������� ������������� ��� ��������� �����������
            services.AddTransient<UserManager<AppUser>>();

            // ���������� ��������� ����� ��� ��������� �����������
            services.AddTransient<SignInManager<AppUser>>();

            services.AddTransient<IJwtGenerator, JwtGenerator>();

            // ���������� ������������
            services.AddControllers();

            // ���������� API Explorer ��� �������� ������������ API
            services.AddEndpointsApiExplorer();

            // ����������� MediatR � ��������� ������, ���������� �����������
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(LoginHandler).Assembly);
            });
        }
    }
}