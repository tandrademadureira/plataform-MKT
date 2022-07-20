using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace Shared.Api.Configuration
{
    /// <summary>
    /// SwaggerConfigurations: Class that contents IServiceCollection extensions methods, used to set Swagger configuration.
    /// </summary>
    public static class SwaggerConfigurations
    {
        /// <summary>
        /// Extension of IServiceCollection interface that configures Swagger engine.
        /// </summary>
        /// <param name="services">Intance of IServiceCollection</param>
        /// <param name="swaggerDocTitle">Title of SwaggerDoc. Sample: Transaction Repository - Microservice Import.</param>
        /// <param name="swaggerDocVersion">Version of SwaggerDoc. Sample: v1.</param>
        /// <typeparam name="TStartup">Class that start configurations.</typeparam>
        /// <returns>Intance of IServiceCollection instance</returns>
        public static IServiceCollection ConfigureSwagger<TStartup>(this IServiceCollection services, string swaggerDocTitle, string swaggerDocVersion)
            where TStartup : class
        {
            services.AddSwaggerGen(c =>
            {
                // Bearer token authentication
                OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
                {
                    Name = "Bearer",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description = "Specify the authorization token.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                };
                c.AddSecurityDefinition("jwt_auth", securityDefinition);

                // Make sure swagger UI requires a Bearer token specified
                OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Id = "jwt_auth",
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {securityScheme, new string[] { }},
                    });

                c.SwaggerDoc("v1", new OpenApiInfo { Title = swaggerDocTitle, Version = swaggerDocVersion });

                c.IncludeXmlComments(GetXmlCommentsPath((typeof(TStartup).Assembly.GetName().Name)));
            });

            return services;
        }

        /// <summary>
        /// Extension of IApplicationBuilder that register Swagger usage.
        /// </summary>
        /// <param name="applicationBuilder">Instance of IApplicationBuilder.</param>
        /// <param name="swaggerDocTitle">Title of SwaggerDoc. Sample: Transaction Repository - Microservice Import.</param>
        /// <param name="swaggerDocVersion">Version of SwaggerDoc. Sample: v1.</param>
        /// <returns>Instance of IApplicationBuilder.</returns>
        public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder applicationBuilder, string swaggerDocTitle, string swaggerDocVersion)
        {
            applicationBuilder.UseSwagger();
            applicationBuilder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", swaggerDocTitle + " " + swaggerDocVersion.ToUpper());
            });

            return applicationBuilder;
        }

        private static string GetXmlCommentsPath(string assemblyName) => $"{AppDomain.CurrentDomain.BaseDirectory}{assemblyName}.xml";
    }
}
