
using Shared.Infra.Data.Sql.Extensions;
using Shared.Infra.Data.Sql.Repository;
using Shared.Util.Extension;
using Plataform.MKT.Infra.Data.Contexts;
using System.Linq;
using System.Threading.Tasks;
using Plataform.MKT.Domain.AggregateModels.Products;

namespace Plataform.MKT.Infra.Data.Repositories
{
    public class ProductRepository : RepositoryBase<DbContextCatalog, Product>, IProductRepository
    {
        public ProductRepository(DbContextCatalog context) : base(context) { }

        public async Task<PagedList<Product>> GetAllByFilterAsync(int page, int itemsPerPage, bool orderedAsc)
        {
            var query = _context.Catalog.Where(it => !it.DeletedAt.HasValue);
            return await query.ToPagedListAsync(it => it.Description, page, itemsPerPage, orderedAsc);
        }

        public async Task SetAutoChanges(bool value)
        {
             _context.SetAutoDetectChanges(value);
            await Task.CompletedTask;
        }

        async Task IProductRepository.UpdateCatalog(Product catalog)
        {
            _context.SetModified(catalog);
            await Task.CompletedTask;
        }
    }
}
