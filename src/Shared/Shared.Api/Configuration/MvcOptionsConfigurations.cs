using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Shared.Api.Configuration.Binders;
using Shared.Api.Filters;
using System.Collections.Generic;

namespace Shared.Api.Configuration
{
    /// <summary>
    /// MvcOptionsConfigurations: Class that contents MvcOptions and MvcJsonOptions extensions methods, used to set mvc options confugurations.
    /// </summary>
    public static class MvcOptionsConfigurations
    {
        /// <summary>
        /// Configure Filters of Authorization, Request Messages and Request validation filters and so Model binders of DateTime and decimal.
        /// </summary>
        /// <param name="mvcOptions">Intance of MvcOptions.</param>
        /// /// <param name="headerKeysToExtract">New keys for header.</param>
        /// <returns>Intance of MvcOptions.</returns>
        public static MvcOptions ConfigureMvcOptions(this MvcOptions mvcOptions, IEnumerable<string> headerKeysToExtract = null)
        {
            mvcOptions.ConfigureFilters(headerKeysToExtract);
            mvcOptions.ConfigureModelBinders();

            return mvcOptions;
        }

        private static MvcOptions ConfigureFilters(this MvcOptions options, IEnumerable<string> headerKeysToExtract)
        {
            var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

            options.Filters.Add(new AuthorizeFilter(policy));
            options.Filters.Add(new RequestFilters(headerKeysToExtract));

            return options;
        }

        private static MvcOptions ConfigureModelBinders(this MvcOptions options)
        {
            options.ModelBinderProviders.Insert(0, new CurrentCultureDateTimeBinderProvider());
            options.ModelBinderProviders.Insert(1, new CurrentCultureDecimalBinderProvider());

            return options;
        }
    }
}
