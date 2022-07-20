using Shared.Infra.Data.Sql.UnitOfWork;
using Plataform.MKT.Infra.Data.Contexts;

namespace Plataform.MKT.Infra.UnitOfWork
{
    public class UnitOfWork : UnitOfWorkBase<DbContextCatalog>, IUnitOfWork
    {
        public UnitOfWork(DbContextCatalog context) : base(context) { }
    }
}
