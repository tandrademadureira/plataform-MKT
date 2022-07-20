using Shared.Infra.Data.Sql.Contexts;
using Shared.Infra.Data.Sql.UnitOfWork;

namespace Plataform.MKT.Infra.UnitOfWork
{
    public interface IUnitOfWork : IUnitOfWork<IDbContext>
    {
    }
}
