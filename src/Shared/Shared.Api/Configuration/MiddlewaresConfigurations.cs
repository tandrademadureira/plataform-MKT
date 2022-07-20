using Microsoft.AspNetCore.Builder;

namespace Shared.Api.Configuration
{
    /// <summary>
    /// MiddlewaresConfigurations: Class that contents IApplicationBuilder extensions methods, used to set SwaggerUI.
    /// </summary>
    public static class MiddlewaresConfigurations
    {
        /// <summary>
        /// Extension of IApplicationBuilder that register Swagger usage.
        /// </summary>
        /// <param name="applicationBuilder">Instance of IApplicationBuilder.</param>
        /// <param name="swaggerDocTitle">Title of SwaggerDoc. Sample: Transaction Repository - Microservice Import.</param>
        /// <param name="swaggerDocVersion">Version of SwaggerDoc. Sample: v1.</param>
        /// <returns>Instance of IApplicationBuilder.</returns>
        public static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder applicationBuilder, string swaggerDocTitle, string swaggerDocVersion)
        {
            applicationBuilder.UseSwagger();
            applicationBuilder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", swaggerDocTitle + " " + swaggerDocVersion.ToUpper());
            });

            return applicationBuilder;
        }
    }
}
