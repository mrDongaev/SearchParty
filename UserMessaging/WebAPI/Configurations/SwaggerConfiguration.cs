﻿using Microsoft.OpenApi.Models;
using System.Reflection;

namespace WebAPI.Configurations
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            Dictionary<string, int> counter = new Dictionary<string, int>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Description = "SearchParty User Messaging API v1",
                    Title = "Swagger",
                    Version = "1.0.0"
                });
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
    }
}
