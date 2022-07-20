using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace Shared.Api.HealthCheck.Extensions
{
    /// <summary>
    /// HealthCheckExtensions: Class that contents IApplicationBuilder extensions methods, used to set health check extensions.
    /// </summary>
    public static class HealthCheckExtensions
    {
        private static void HandleInfoBranch(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var response = JsonConvert.SerializeObject(new
                {
                    hostname = Environment.GetEnvironmentVariable("HOSTNAME"),
                    commit_id = Environment.GetEnvironmentVariable("COMMIT_ID"),
                    pod_ip = Environment.GetEnvironmentVariable("POD_IP"),
                    aspnetcore_environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                });

                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "application/json";
                context.Response.ContentLength = response.Length;

                await context.Response.WriteAsync(response);
            });
        }

        /// <summary>
        /// Extension of IApplicationBuilder interface that configures health checks engine.
        /// </summary>
        /// <example>
        /// Using the root startup class.
        /// <code>
        /// public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        /// {
        ///     app.UseHealthChecks();
        /// }
        /// </code>
        /// </example>        
        /// <param name="app">Instance of IApplicationBuilder.</param>
        /// <returns>Intance of IServiceCollection instance</returns>
        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/hc", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            app.Map("/__info", HandleInfoBranch);

            return app;
        }
    }
}
