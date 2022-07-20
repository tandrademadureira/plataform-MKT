using Microsoft.EntityFrameworkCore.Storage;
using Shared.Infra.Data.Sql.Contexts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Infra.Data.Sql.UnitOfWork
{
    /// <summary>
    /// Base class that implements the interface with the methods responsible for operations outside of the DbContext.
    /// <para>Unit of Work is a design pattern to resolution of concurrency problems and coordinates the writing out of changes.</para>
    /// </summary>
    /// <typeparam name="TContext">The type of context being operated on by this unit of work.</typeparam>
    public abstract class UnitOfWorkBase<TContext> : IUnitOfWork<TContext>
       where TContext : IDbContext
    {
        private TContext Context { get; }

        private bool Disposed { get; set; } = false;

        /// <summary>
        /// Default constructor that set the context operated on by this unit of work.
        /// </summary>
        /// <param name="context">Instance of the context operated on by this unit of work.</param>
        /// <example>
        /// <note type="note">This is an initial configuration that should be used as a basis for the following examples.</note>
        /// Here we have the interface for the Foo context.
        /// <code>
        /// public interface IDbContextFoo : IDbContext
        /// {
        /// }
        /// </code>
        /// Class that defines the Foo context by implementing the ***IDbContextFoo*** interface that was previously created.
        /// <code>
        /// public class DbContextFoo : DbContextBase, IDbContextFoo
        /// {
        ///     public DbContextFoo(DbContextOptions dbContextOptions)
        ///         : base(dbContextOptions)
        ///     {
        ///     }
        ///     
        ///     protected override void OnModelCreating(ModelBuilder modelBuilder)
        ///     {
        ///     }
        /// }
        /// </code>
        /// Here we have the interface for the UnitOfWork.
        /// <code>
        /// public interface IUnitOfWork : IUnitOfWork<![CDATA[<IDbContextFoo>]]>
        /// {
        /// }
        /// </code>
        /// Class that defines the UnitOfWork by implementing the ***IUnitOfWork*** interface that was previously created.
        /// <code>
        /// public class UnitOfWork : UnitOfWorkBase<![CDATA[<IDbContextFoo>]]>, IUnitOfWork
        /// {
        ///     public UnitOfWork(DbContextFoo context)
        ///         : base(context)
        ///     {
        ///     }
        /// }
        /// </code>
        /// For a working example, you need to create the entire dependency injection and configuration part using the ***Options*** design pattern.
        /// <note type="note">This part of the example is not required to use the Framework.</note>
        /// Configuration file ***appsettings.json*** with the connection string.
        /// <code>
        /// {
        ///   "ConnectionStrings": {
        ///     "DefaultConnectionConfiguration": "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;persistsecurityinfo=True;"
        ///   }
        /// }
        /// </code>
        /// Loading the information from the configuration file above.
        /// <code>
        /// var configuration = new ConfigurationBuilder()
        ///     .SetBasePath(Directory.GetCurrentDirectory())
        ///     .AddJsonFile("appsettings.json", false)
        ///     .Build();
        /// </code>
        /// Loading information for dependency injection.
        /// <code>
        /// var services = new ServiceCollection();
        /// 
        /// services
        ///     .AddScoped<![CDATA[<IDbContextFoo, DbContextFoo>]]>()
        ///     .AddScoped<![CDATA[<IUnitOfWork, UnitOfWork>]]>()
        ///     .AddDbContext<![CDATA[<DbContextFoo>]]>(options => 
        ///         options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionConfiguration")));
        /// </code>
        /// </example>
        protected UnitOfWorkBase(TContext context) => Context = context;

        /// <summary>
        /// Synchronous method that confirms the modifications made to entities within the context.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public virtual int SaveChanges() => Context.SaveChanges();

        /// <summary>
        /// Asynchronous method that confirms the modifications made to entities within the context.
        /// </summary>
        /// <param name="cancellationToken">Spreads the notification that transactions should be canceled.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) => await Context.SaveChangesAsync();

        /// <summary>
        /// Starts a new transaction.
        /// </summary>
        /// <returns>A transaction against the database.</returns>
        /// <example>
        /// Bar class used in this example.
        /// <code>
        /// public class Bar
        /// {
        ///     private IUnitOfWork UnitOfWork { get; }
        /// 
        ///     public Bar(IUnitOfWork unitOfWork) => UnitOfWork = unitOfWork;
        /// 
        ///     public async Task SampleBeginTransaction()
        ///     {
        ///         using (var transaction = UnitOfWork.BeginTransaction())
        ///         {
        ///             transaction.Commit();
        ///         }
        /// 
        ///     }
        /// }
        /// </code>
        /// </example>
        public IDbContextTransaction BeginTransaction() => Context.BeginTransaction();

        /// <summary>
        /// Set timeout to execute the commands into context instance.
        /// </summary>
        /// <param name="timeoutInSeconds">Value that represent in seconds the  timeout.</param>
        /// <example>
        /// Bar class used in this example.
        /// <code>
        /// public class Bar
        /// {
        ///     private IUnitOfWork UnitOfWork { get; }
        /// 
        ///     public Bar(IUnitOfWork unitOfWork) => UnitOfWork = unitOfWork;
        /// 
        ///     public void SetTimeOut()
        ///     {
        ///         var timeoutInSeconds = 10;
        /// 
        ///         UnitOfWork.SetTimeOut(timeoutInSeconds);
        ///     }
        /// }
        /// </code>
        /// </example>
        public void SetTimeOut(int timeoutInSeconds = 180) => Context.SetTimeOut(timeoutInSeconds);

        /// <summary>
        /// Dispose the context.
        /// </summary>
        /// <param name="disposing">True to dispose the context, otherwise, false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
                Context.Dispose();

            Disposed = true;
        }

        /// <summary>
        /// Dispose the context.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
