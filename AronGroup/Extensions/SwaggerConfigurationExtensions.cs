using AronGroup.Models.Common;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AronGroup.Extensions;

public static class SwaggerConfigurationExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration, string sectionName)
    {
        var swaggerOption = configuration.GetSection(sectionName).Get<SwaggerOption>();
        var apiAuthenticationOptions = new ApiAuthenticationOptions();

        if (swaggerOption != null && swaggerOption.SwaggerDoc != null && swaggerOption.Enabled == true)
        {
            services.AddSwaggerGen(o =>
            {

                o.SwaggerDoc(swaggerOption.SwaggerDoc.Name, new OpenApiInfo
                {
                    Title = swaggerOption.SwaggerDoc.Title,
                    Version = swaggerOption.SwaggerDoc.Version
                });
                var xmlDocPath = Path.Combine(AppContext.BaseDirectory, swaggerOption.SwaggerDoc.DocumentationFile);
                o.IncludeXmlComments(xmlDocPath, true);

                o.AddSecurityDefinition(apiAuthenticationOptions.TokenHeaderName, new OpenApiSecurityScheme
                {
                    Description = $"Api Authorization header using the {ApiAuthenticationOptions.DefaultScheme} scheme. Example: \"{apiAuthenticationOptions.TokenHeaderName}: {ApiAuthenticationOptions.DefaultScheme} {{token}}\"",
                    Name = apiAuthenticationOptions.TokenHeaderName,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = ApiAuthenticationOptions.DefaultScheme,

                });
                o.OperationFilter<AuthenticationRequirementsOperationFilter>();
            });
        }
        return services;
    }

    public class AuthenticationRequirementsOperationFilter : IOperationFilter
    {

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Security == null)
                operation.Security = new List<OpenApiSecurityRequirement>();

            var apiAuthenticationOptions = new ApiAuthenticationOptions();

            var scheme =  new OpenApiSecurityScheme
                                         {
                                             Reference = new OpenApiReference
                                             {
                                                 Type = ReferenceType.SecurityScheme,
                                                 Id = apiAuthenticationOptions.TokenHeaderName
                                             },
                                             Scheme = ApiAuthenticationOptions.DefaultScheme,
                                             Name = apiAuthenticationOptions.TokenHeaderName,
                                             In = ParameterLocation.Header,
                                         };

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [scheme] = new List<string>()
            });
        }
    }

    public static void UseSwaggerAndUI(this WebApplication app, string sectionName)
    {
        var swaggerOption = app.Configuration.GetSection(sectionName).Get<SwaggerOption>();

        if (swaggerOption != null && swaggerOption.SwaggerDoc != null && swaggerOption.Enabled == true)
        {
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerOption.SwaggerDoc.URL, swaggerOption.SwaggerDoc.Title);
            });
        }
    }
}
