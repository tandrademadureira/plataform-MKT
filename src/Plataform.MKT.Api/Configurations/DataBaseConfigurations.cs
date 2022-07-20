using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Plataform.MKT.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Plataform.MKT.Api.Configurations
{
    internal static class DataBaseConfigurations
    {
        internal static void ConfigureDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbContextCatalog>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionConfiguration"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(3),
                    errorNumbersToAdd: null);
                }));
        }
    }
}
