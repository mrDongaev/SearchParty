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
            // Создание веб-приложения с настройками по умолчанию
            var builder = WebApplication.CreateBuilder(args);

            // Добавление сервисов в контейнер зависимостей
            ConfigureServices(builder.Services, builder.Configuration);

            // Построение веб-приложения
            var app = builder.Build();

            // Конфигурация конвейера обработки HTTP-запросов
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Показ страницы разработчика в случае ошибки
            }

            app.UseHttpsRedirection(); // Перенаправление HTTP-запросов на HTTPS

            app.UseAuthentication();

            app.UseAuthorization(); // Включение авторизации

            app.MapControllers(); // Маршрутизация контроллеров

            // Создание области видимости для выполнения миграций базы данных
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<DataContext>(); // Получение контекста базы данных

                    var userManager = services.GetRequiredService<UserManager<AppUser>>(); // Получение менеджера пользователей

                    context.Database.Migrate(); // Применение миграций к базе данных

                    DataSeed.SeedDataAsync(context, userManager).Wait(); // Заполнение базы данных начальными данными
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>(); // Получение логгера

                    logger.LogError(ex, "An error occurred during migration"); // Логирование ошибки миграции
                }
            }

            // Простой маршрут для проверки работы приложения
            //app.MapGet("/start", () => "");

            // Запуск приложения
            app.Run();
        }

        // Метод для настройки сервисов
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Настройка контекста базы данных с использованием строки подключения
            services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Настройка Identity для управления пользователями и ролями
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false; // Требование подтверждения аккаунта при входе

                options.Password.RequireNonAlphanumeric = false; // Отключение требования неалфавитных символов в пароле
            })
            .AddEntityFrameworkStores<DataContext>() // Использование Entity Framework для хранения данных Identity

            .AddDefaultTokenProviders(); // Добавление провайдеров токенов по умолчанию

            // Добавление менеджера пользователей как временной зависимости
            services.AddTransient<UserManager<AppUser>>();

            // Добавление менеджера входа как временной зависимости
            services.AddTransient<SignInManager<AppUser>>();

            services.AddTransient<IJwtGenerator, JwtGenerator>();

            // Добавление контроллеров
            services.AddControllers();

            // Добавление API Explorer для создания документации API
            services.AddEndpointsApiExplorer();

            // Регистрация MediatR с указанием сборки, содержащей обработчики
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(LoginHandler).Assembly);
            });
        }
    }
}