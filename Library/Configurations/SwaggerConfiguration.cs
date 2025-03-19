using Library.Models.HttpResponses;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Library.Configurations
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, string name, OpenApiInfo apiInfo)
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            Dictionary<string, int> counter = new Dictionary<string, int>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(name, apiInfo);

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

                options.SchemaFilter<PascalCaseDictionaryKeysFilter>();

                options.CustomSchemaIds(type =>
                {
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(HttpResponseBody<>))
                    {
                        var genericType = type.GetGenericArguments()[0];
                        var name = genericType.Name;
                        var declaringName = genericType.DeclaringType?.Name ?? string.Empty;
                        if (declaringName != string.Empty) declaringName += ".";
                        var final = $"HttpResponseBody<{declaringName}{name}>";
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
                    }
                    else
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
                    }
                });
            });

            return services;
        }
    }

    public class PascalCaseDictionaryKeysFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.AdditionalProperties != null)
            {
                schema.AdditionalProperties.Properties = schema.AdditionalProperties.Properties
                    .ToDictionary(
                        kvp => char.ToUpper(kvp.Key[0]) + kvp.Key.Substring(1), // PascalCase keys
                        kvp => kvp.Value
                    );
            }
        }
    }
}
