using Plataform.MKT.Domain.AggregateModels.Products;
using Shared.Infra.Data.Sql.Repository;
using Shared.Util.Extension;
using System.Threading.Tasks;

namespace Plataform.MKT.Infra.Data.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task SetAutoChanges(bool value);
        Task UpdateCatalog(Product catalog);
        Task<PagedList<Product>> GetAllByFilterAsync(int page, int itemsPerPage, bool orderedAsc);
    }
}