using Microsoft.EntityFrameworkCore.Storage;
using Shared.Infra.Data.Sql.Contexts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Infra.Data.Sql.UnitOfWork
{
    /// <summary>
    /// Interface that represents the concrete class responsible for operations outside of the DbContext.
    /// <para>Unit of Work is a design pattern to resolution of concurrency problems and coordinates the writing out of changes.</para>
    /// </summary>
    /// <typeparam name="TContext">The type of context being operated on by this unit of work.</typeparam>
    public interface IUnitOfWork<TContext> : IDisposable
        where TContext : IDbContext
    {
        /// <summary>
        /// Set timeout to execute the commands into context instance.
        /// </summary>
        /// <param name="timeoutInSeconds">Value that represent in seconds the  timeout.</param>
        void SetTimeOut(int timeoutInSeconds = 180);

        /// <summary>
        /// Synchronous method that confirms the modifications made to entities within the context.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        int SaveChanges();

        /// <summary>
        /// Asynchronous method that confirms the modifications made to entities within the context.
        /// </summary>
        /// <param name="cancellationToken">Spreads the notification that transactions should be canceled.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Starts a new transaction.
        /// </summary>
        /// <returns>A transaction against the database.</returns>
        IDbContextTransaction BeginTransaction();
    }
}
