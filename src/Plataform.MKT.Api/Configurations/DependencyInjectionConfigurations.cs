using Microsoft.Extensions.DependencyInjection;
using Plataform.MKT.Domain.AggregateModels.Products;
using Plataform.MKT.Infra.Data.Repositories;
using Plataform.MKT.Infra.UnitOfWork;

namespace Plataform.MKT.Api.Configurations
{
    /// <summary>
    /// Class that contents IServiceCollection extensions methods.
    /// </summary>
    public static class DependencyInjectionConfigurations
    {
        /// <summary>
        /// Configure DI.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureDI(this IServiceCollection services)
        {
            #region Repository

            services.AddScoped<IProductRepository, ProductRepository>();

            #endregion

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
