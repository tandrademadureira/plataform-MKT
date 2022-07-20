using Microsoft.EntityFrameworkCore;
using Shared.Infra.Data.Sql.Contexts;
using Plataform.MKT.Infra.Data.Mappings;
using Plataform.MKT.Domain.AggregateModels.Products;

namespace Plataform.MKT.Infra.Data.Contexts
{
    public class DbContextCatalog : DbContextBase, IDbContext
    {
        public DbContextCatalog(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Product> Catalog { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity(CatalogMapping.ConfigureMap());
        }
    }
}
