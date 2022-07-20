using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Infra.Data.Sql.Contexts
{
    /// <summary>
    /// Interface that represents the concrete class responsible for handling EF *(Entity Framework)* Core contexts.
    /// </summary>
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// Enable or disable the tracking from entities changes.
        /// </summary>
        /// <param name="enable">True if enable auto detect change, otherwise, false.</param>
        void SetAutoDetectChanges(bool enable = true);

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
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Method that sets up the entity to be used in context.
        /// <note type="note">Only entities that implement the *IModel* interface can be configured in the context.</note>
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being operated on by this set.</typeparam>
        /// <returns>Entity used to query and save instances.</returns>
        DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class, IModel;

        /// <summary>
        /// Method for querying with nolock.
        /// </summary>
        /// <returns>A transaction against the database.</returns>
        IDbContextTransaction UseNoLock();

        /// <summary>
        /// Starts a new transaction.
        /// </summary>
        /// <returns>A transaction against the database.</returns>
        IDbContextTransaction BeginTransaction();

        /// <summary>
        /// Asynchronous method that execute SQL query on database and map result value to ValueObject.
        /// </summary>
        /// <typeparam name="TValueObject">The type of the model used in the instance of the value object.</typeparam>
        /// <param name="sql">SQL query to execute on database.</param>
        /// <param name="parameters">Key and value as parameter of the query.</param>
        /// <returns>Collection of the value object according result to query.</returns>
        Task<IEnumerable<TValueObject>> ExecuteRawSqlAsync<TValueObject>(string sql, IDictionary<string, object> parameters = null) where TValueObject : ValueObject<TValueObject>;

        /// <summary>
        /// Executes the specified asynchronous operation.
        /// </summary>
        /// <param name="action">The strategy that will be used for the execution. A function that returns a started task.</param>
        /// <returns>
        /// A task that will run to completion if the original task completes successfully (either the first time or after retrying transient failures). If the task fails with a non-transient error or the retry limit is reached, the returned task will become faulted and the exception must be observed.
        ///</returns>
        Task ExecuteStrategyAsync([NotNull] Action action);

        /// <summary>
        /// Synchronous method that execute SQL query on database and map result value to ValueObject.
        /// </summary>
        /// <typeparam name="TValueObject">The type of the model used in the instance of the value object.</typeparam>
        /// <param name="sql">SQL query to execute on database.</param>
        /// <param name="parameters">Key and value as parameter of the query.</param>
        /// <returns>Collection of the value object according result to query.</returns>
        IEnumerable<TValueObject> ExecuteRawSql<TValueObject>(string sql, IDictionary<string, object> parameters = null) where TValueObject : ValueObject<TValueObject>;

        /// <summary>
        /// Method that sets up the entity to modified in context.
        /// <note type="note">Only entities that implement the *IModel* interface can be configured in the context with with modified status.</note>
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being operated on by this set.</typeparam>
        /// <param name="entity">The modified entity.</param>
        void SetModified<TEntity>(TEntity entity) where TEntity : class, IModel;

        /// <summary>
        /// Method that sets up the entity to deleted in context.
        /// <note type="note">Only entities that implement the *IModel* interface can be configured in the context with with deleted status.</note>
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being operated on by this set.</typeparam>
        /// <param name="entity">The modified entity.</param>
        void SetDeleted<TEntity>(TEntity entity) where TEntity : class, IModel;
    }
}
