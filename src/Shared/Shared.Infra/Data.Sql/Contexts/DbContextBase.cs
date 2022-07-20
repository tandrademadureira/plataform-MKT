using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Shared.Infra.Data.Sql.Contexts
{
    /// <summary>
    /// Base class that implements the interface with the methods responsible for handling EF *(Entity Framework)* Core contexts.
    /// </summary>
    public abstract class DbContextBase : DbContext, IDbContext
    {
        /// <summary>
        /// Default constructor that set the ***DbContextOptions***.
        /// <note type="warning">This instance is initialized with <see cref="SetAutoDetectChanges(bool)"/> equal false and <see cref="SetTimeOut(int)"/> equal 3 minutes.</note>
        /// </summary>
        /// <param name="dbContextOptions"></param>
        /// <example>
        /// <note type="note">This is an initial configuration that should be used as a basis for the following examples.</note>
        /// First let's create the model that will represent the entity within the context.
        /// <code>
        /// public class Foo : IAggregateRoot
        /// {
        ///     public int IdFoo { get; private set; }
        /// 
        ///     public string Name { get; private set; }
        /// 
        ///     public static IModelResult<![CDATA[<Foo>]]> New(string name)
        ///     {
        ///         var foo = new Foo { Name = name };
        /// 
        ///         return Validate(foo);
        ///     }
        /// 
        ///     private static IModelResult<![CDATA[<Foo>]]> Validate(Foo foo)
        ///     {
        ///         var result = new ModelResult<![CDATA[<Foo>]]>();
        /// 
        ///         if (string.IsNullOrWhiteSpace(foo.Name))
        ///             result.AddValidation("Name", "The field is required!");
        /// 
        ///         if (result.IsModelResultValid())
        ///             result.SetModel(foo);
        /// 
        ///         return result;
        ///     }
        /// }
        /// </code>
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
        ///     .AddDbContext<![CDATA[<DbContextFoo>]]>(options => 
        ///         options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionConfiguration")));
        /// </code>
        /// </example>
        protected DbContextBase(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
            SetAutoDetectChanges(true);
            SetTimeOut();
        }

        /// <summary>
        /// Set timeout to execute the commands into context instance.
        /// </summary>
        /// <param name="timeoutInSeconds">Value that represent in seconds the  timeout.</param>
        /// <example>
        /// Bar class used in this example.
        /// <code>
        /// public class Bar
        /// {
        ///     private IDbContextFoo Context { get; }
        /// 
        ///     public Bar(IDbContextFoo context) => Context = context;
        /// 
        ///     public void SetTimeOut()
        ///     {
        ///         var timeoutInSeconds = 10;
        /// 
        ///         Context.SetTimeOut(timeoutInSeconds);
        ///     }
        /// }
        /// </code>
        /// </example>
        public void SetTimeOut(int timeoutInSeconds = 180) => Database.SetCommandTimeout(timeoutInSeconds);

        /// <summary>
        /// Synchronous method that execute SQL query on database and map result value to ValueObject.
        /// </summary>
        /// <typeparam name="TValueObject">The type of the model used in the instance of the value object.</typeparam>
        /// <param name="sql">SQL query to execute on database.</param>
        /// <param name="parameters">Key and value as parameter of the query.</param>
        /// <returns>Collection of the value object according result to query.</returns>
        /// <example>
        /// In this example, use FooQuery class as result of the query.
        /// <code>
        /// public class FooQuery : ValueObject<![CDATA[<FooQuery>]]>
        /// {
        ///     public FooQuery(string name) => Name = name;
        /// 
        ///     public string Name { get; private set; }
        /// }
        /// </code>
        /// Set on the DbContextFoo into method *OnModelCreating(ModelBuilder modelBuilder)*.
        /// <code>
        /// modelBuilder.Query<![CDATA[<FooQuery>]]>();
        /// </code>
        /// Bar class used in this example.
        /// <code>
        /// public class Bar
        /// {
        ///     private IDbContextFoo Context { get; }
        /// 
        ///     public Bar(IDbContextFoo context) => Context = context;
        /// 
        ///     public IEnumerable<![CDATA[<FooQuery>]]> GetFooQuery(string name)
        ///     {
        ///         var parameters = new Dictionary<![CDATA[<string, object>]]>();
        /// 
        ///         var sql = @"SELECT name FROM Foo WHERE name like '@name'";
        /// 
        ///         parameters.Add("@name", name);
        /// 
        ///         return Context.ExecuteRawSql<![CDATA[<FooQuery>]]>(sql, parameters);        
        ///     }
        /// }
        /// </code>
        /// </example>
        public virtual IEnumerable<TValueObject> ExecuteRawSql<TValueObject>(string sql, IDictionary<string, object> parameters = null)
            where TValueObject : ValueObject<TValueObject> => ExecuteRawSqlAsync<TValueObject>(sql, parameters).Result;


        /// <summary>
        /// Asynchronous method that execute SQL query on database and map result value to ValueObject.
        /// </summary>
        /// <typeparam name="TValueObject">The type of the model used in the instance of the value object.</typeparam>
        /// <param name="sql">SQL query to execute on database.</param>
        /// <param name="parameters">Key and value as parameter of the query.</param>
        /// <returns>Collection of the value object according result to query.</returns>
        /// <example>
        /// In this example, use FooQuery class as result of the query.
        /// <code>
        /// public class FooQuery : ValueObject<![CDATA[<FooQuery>]]>
        /// {
        ///     public FooQuery(string name) => Name = name;
        /// 
        ///     public string Name { get; private set; }
        /// }
        /// </code>
        /// Bar class used in this example.
        /// <code>
        /// public class Bar
        /// {
        ///     private IDbContextFoo Context { get; }
        /// 
        ///     public Bar(IDbContextFoo context) => Context = context;
        /// 
        ///     public async Task<![CDATA[<IEnumerable<FooQuery>>]]> GetFooQuery(string name)
        ///     {
        ///         var parameters = new Dictionary<![CDATA[<string, object>]]>();
        /// 
        ///         var sql = @"SELECT name FROM Foo WHERE name like '@name'";
        /// 
        ///         parameters.Add("@name", name);
        /// 
        ///         return await Context.ExecuteRawSqlAsync<![CDATA[<FooQuery>]]>(sql, parameters);        
        ///     }
        /// }
        /// </code>
        /// </example>
        public virtual async Task<IEnumerable<TValueObject>> ExecuteRawSqlAsync<TValueObject>(string sql, IDictionary<string, object> parameters = null)
            where TValueObject : ValueObject<TValueObject>
        {
            var valueObject = Set<TValueObject>();

            if (parameters != null)
                return await valueObject.FromSqlRaw(sql, GetAllParameters(parameters)).ToListAsync();

            return await valueObject.FromSqlRaw(sql).ToListAsync();
        }

        /// <summary>
        /// Executes the specified asynchronous operation.
        /// </summary>
        /// <param name="action">The strategy that will be used for the execution. A function that returns a started task.</param>
        /// <returns>
        /// A task that will run to completion if the original task completes successfully (either the first time or after retrying transient failures). If the task fails with a non-transient error or the retry limit is reached, the returned task will become faulted and the exception must be observed.
        /// </returns>
        /// <example>
        /// DbContextFoo used in this example.
        /// <code>
        /// public class DbContextFoo : DbContextBase, IDbContext
        /// {
        ///     public DbContextFoo(DbContextOptions dbContextOptions)
        ///         : base(dbContextOptions)
        ///     {
        ///     }
        /// }
        /// </code>
        /// Foo and Bar classes used in this example.
        /// <code>
        /// public class Foo
        /// {
        ///     public int IdFoo { get; set; }
        /// 
        ///     public string Name { get; set; }
        /// 
        ///     public IEnumerable<![CDATA[<Bar>]]> BarList { get; set; }
        /// }
        /// 
        /// public class Bar
        /// {
        ///     public int IdBar { get; set; }
        /// 
        ///     public string Name { get; set; }
        /// 
        ///     public int IdFoo { get; set; }
        /// 
        ///     public Foo Foo { get; set; }
        /// }
        /// </code>
        /// FooRepository sample.
        /// <code>
        /// public class FooRepository
        /// {
        ///     private DbContextFoo Context { get; }
        /// 
        ///     public FooRepository(DbContextFoo context) => Context = context;
        /// 
        ///     public async Task New(IEnumerable<![CDATA[<Foo>]]> modelList)
        ///     {
        ///         await Context.ExecuteStrategyAsync(async () =>
        ///         {
        ///             var bulkConfig = new BulkConfig { UseTempDB = true, SetOutputIdentity = true };
        /// 
        ///             var fooList = modelList.ToList();
        /// 
        ///             if (modelList.Count() > 2000)
        ///                 bulkConfig.BatchSize = modelList.Count();
        /// 
        ///             await Context.BulkInsertAsync(fooList, bulkConfig);
        /// 
        ///             bulkConfig.SetOutputIdentity = false;
        /// 
        ///             foreach (var foo in fooList)
        ///             {
        ///                 var model = modelList.FirstOrDefault(it => it.Name == foo.Name);
        /// 
        ///                 if (model == null)
        ///                     continue;
        /// 
        ///                 var barList = model.BarList.ToList();
        /// 
        ///                 foreach (var bar in barList)
        ///                     bar.IdBar = foo.IdFoo;
        /// 
        ///                 await Context.BulkInsertAsync(barList, bulkConfig);
        ///             }
        ///         });
        ///     }
        /// }
        /// </code>
        /// </example>
        public virtual async Task ExecuteStrategyAsync([NotNull] Action action)
        {
            var strategy = Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using var scope = Database.BeginTransaction();
                try
                {
                    await Task.Run(() => action());
                    scope.Commit();
                }
                catch
                {
                    scope.Rollback();
                    scope.Dispose();
                    throw;
                }
            });
        }

        /// <summary>
        /// Enable or disable the tracking from entities changes.
        /// </summary>
        /// <param name="enable">True if enable auto detect change, otherwise, false.</param>
        /// <example>
        /// Bar class used in this example.
        /// <code>
        /// public class Bar
        /// {
        ///     private IDbContextFoo Context { get; }
        /// 
        ///     public Bar(IDbContextFoo context) => Context = context;
        /// 
        ///     public void SetAutoDetectChangesFalse() => Context.SetAutoDetectChanges(false);
        ///     //Or
        ///     public void SetAutoDetectChangesTrue() => Context.SetAutoDetectChanges();
        /// }
        /// </code>
        /// </example>
        public void SetAutoDetectChanges(bool enable = true) => ChangeTracker.AutoDetectChangesEnabled = enable;

        /// <summary>
        /// Method for querying with nolock.
        /// </summary>
        /// <returns>A transaction against the database.</returns>
        /// <example>
        /// Bar class used in this example.
        /// <code>
        /// public class Bar
        /// {
        ///     private IDbContextFoo Context { get; }
        /// 
        ///     public Bar(IDbContextFoo context) => Context = context;
        /// 
        ///     public async Task<![CDATA[<List<Foo>>]]> SampleUseNoLock()
        ///     {
        ///         using (var transaction = Context.UseNoLock())
        ///             return await Context.Foo.ToListAsync();
        ///     }
        /// }
        /// </code>
        /// </example>
        public IDbContextTransaction UseNoLock() => Database.BeginTransaction(IsolationLevel.ReadUncommitted);

        /// <summary>
        /// Starts a new transaction.
        /// </summary>
        /// <returns>A transaction against the database.</returns>
        /// <example>
        /// Bar class used in this example.
        /// <code>
        /// public class Bar
        /// {
        ///     private IDbContextFoo Context { get; }
        /// 
        ///     public Bar(IDbContextFoo context) => Context = context;
        /// 
        ///     public async Task SampleBeginTransaction()
        ///     {
        ///         using (var transaction = Context.BeginTransaction())
        ///         {
        ///             var result = Foo.New("Smarkets com br");
        /// 
        ///             if (result.IsModelResultValid())
        ///                 await Context.Foo.AddAsync(result.Model);
        /// 
        ///             await Context.SaveChangesAsync();
        /// 
        ///             transaction.Commit();
        ///         }
        /// 
        ///     }
        /// }
        /// </code>
        /// </example>
        public IDbContextTransaction BeginTransaction() => Database.BeginTransaction();

        /// <summary>
        /// Method that sets up the entity to be used in context.
        /// <note type="note">Only entities that implement the *IModel* interface can be configured in the context.</note>
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being operated on by this set.</typeparam>
        /// <returns>Entity used to query and save instances.</returns>
        /// <example>
        /// Settings on the interface IDbContextFoo.
        /// <code>
        /// public interface IDbContextFoo : IDbContext
        /// {
        ///     DbSet<![CDATA[<Foo>]]> Foo { get; }
        /// }
        /// </code>
        /// Settings on the class DbContextFoo.
        /// <code>
        /// public class DbContextFoo : DbContextBase, IDbContextFoo
        /// {
        ///     public DbContextFoo(DbContextOptions dbContextOptions)
        ///         : base(dbContextOptions)
        ///     {
        ///     }
        /// 
        ///     public DbSet<![CDATA[<Foo>]]> Foo => GetDbSet<![CDATA[<Foo>]]>();
        /// }
        /// </code>
        /// </example>
        public virtual DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class, IModel => Set<TEntity>();

        /// <summary>
        /// Method that sets up the entity to deleted in context.
        /// <note type="note">Only entities that implement the *IModel* interface can be configured in the context with with deleted status.</note>
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being operated on by this set.</typeparam>
        /// <param name="entity">The modified entity.</param>
        /// <example>
        /// Bar class used in this example.
        /// <code>
        /// public class Bar
        /// {
        ///     private IDbContextFoo Context { get; }
        /// 
        ///     public Bar(IDbContextFoo context) => Context = context;
        /// 
        ///     public void Delete(Foo foo) => Context.SetDeleted(foo);
        /// }
        /// </code>
        /// </example>
        public virtual void SetDeleted<TEntity>(TEntity entity) where TEntity : class, IModel => Entry(entity).State = EntityState.Deleted;


        /// <summary>
        /// Method that sets up the entity to modified in context.
        /// <note type="note">Only entities that implement the *IModel* interface can be configured in the context with with modified status.</note>
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being operated on by this set.</typeparam>
        /// <param name="entity">The modified entity.</param>
        /// <example>
        /// Bar class used in this example.
        /// <code>
        /// public class Bar
        /// {
        ///     private IDbContextFoo Context { get; }
        /// 
        ///     public Bar(IDbContextFoo context) => Context = context;
        /// 
        ///     public void Update(Foo foo) => Context.SetModified(foo);
        /// }
        /// </code>
        /// </example>
        public virtual void SetModified<TEntity>(TEntity entity) where TEntity : class, IModel => Entry(entity).State = EntityState.Modified;

        private SqlParameter[] GetAllParameters(IDictionary<string, object> parameters)
        {
            var sqlParameters = new List<SqlParameter>();

            foreach (var item in parameters)
            {
                if (item.Value == null)
                    sqlParameters.Add(new SqlParameter(item.Key, DBNull.Value));
                else
                    sqlParameters.Add(new SqlParameter(item.Key, item.Value));
            }

            return sqlParameters.ToArray();
        }
    }
}
