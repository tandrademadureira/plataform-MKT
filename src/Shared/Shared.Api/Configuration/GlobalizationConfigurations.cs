using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Shared.Api.Configuration.Options;
using System.Globalization;
using System.Linq;

namespace Shared.Api.Configuration
{
    /// <summary>
    /// AppSettingsGlobalization: Class that contents IApplicationBuilder extensions methods, used to set globalizations.
    /// </summary>
    public static class GlobalizationConfigurations
    {
        private static string AppSettingsGlobalization => "Globalization";

        /// <summary>
        /// Extension of IApplicationBuilder that make application localization.
        /// </summary>
        /// <param name="applicationBuilder">Intance of IApplicationBuilder.</param>
        /// <param name="configuration">Intance of IConfiguration.</param>
        /// <returns>Intance of IApplicationBuilder.</returns>
        public static IApplicationBuilder UseGlobalization(this IApplicationBuilder applicationBuilder, IConfiguration configuration)
        {
            var globalizationOptions = new GlobalizationOptions();
            configuration.GetSection(AppSettingsGlobalization).Bind(globalizationOptions);

            applicationBuilder.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(globalizationOptions?.DefaultRequestCulture, globalizationOptions?.DefaultRequestCulture),
                SupportedCultures = globalizationOptions?.SupportedCultures.Select(it => new CultureInfo(it)).ToList(),
                SupportedUICultures = globalizationOptions?.SupportedUICultures.Select(it => new CultureInfo(it)).ToList(),
            });

            return applicationBuilder;
        }
    }
}