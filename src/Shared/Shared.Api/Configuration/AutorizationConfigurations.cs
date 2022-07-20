using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Shared.Api.Configuration
{
    /// <summary>
    /// AutorizationConfigurations: Class that contains the IServiceCollection extension methods, used to set authorizations.
    /// </summary>
    public static class AutorizationConfigurations
    {
        /// <summary>
        /// Extension of IServiceCollection interface that configures Autorization engine.
        /// </summary>
        /// <param name="services">Intance of IServiceCollection</param>
        /// <param name="configuration">Intance of IConfiguration</param>
        /// <returns>Intance of IServiceCollection instance</returns>
        public static IServiceCollection ConfigureAutorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.Authority = configuration["Jwt:Authority"];
                o.Audience = configuration["Jwt:Audience"];
                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync("An error ocured processing your authentication");
                    }
                };
            });

            return services;
        }
    }
}
