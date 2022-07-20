using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shared.Domain.SeedWork;
using Shared.Infra.Data.Sql.Contexts;
using Shared.Util.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smarkets.Framework.Data.Sql.Test.Contexts
{
    [Ignore("A local database is required.")]
    [TestFixture]
    public class DbContextTest
    {
        public IConfigurationRoot Configuration { get; private set; }

        [SetUp]
        public void Init()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("Data.Sql\\appsettings\\appsettings.json", false)
                .Build();
        }


        [TestCase(true, ExpectedResult = true)]
        [TestCase(false, ExpectedResult = false)]
        public bool SetAutoDetectChanges(bool enable)
        {
            //Arrange           
            var services = new ServiceCollection();
            services.AddDbContext<DbContextFoo>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionConfiguration")));
            var provider = services.BuildServiceProvider();
            var context = provider.GetService<DbContextFoo>();

            //Act
            context.SetAutoDetectChanges(enable);

            //Assert
            return context.ChangeTracker.AutoDetectChangesEnabled;
        }

        [TestCase(10, ExpectedResult = 10)]
        [TestCase(18, ExpectedResult = 18)]
        public int SetTimeOut(int timeoutInSecondsenable)
        {
            //Arrange           
            var services = new ServiceCollection();
            services.AddDbContext<DbContextFoo>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionConfiguration")));
            var provider = services.BuildServiceProvider();
            var context = provider.GetService<DbContextFoo>();

            //Act
            context.SetTimeOut(timeoutInSecondsenable);

            //Assert
            return context.Database.GetCommandTimeout().GetValueOrDefault();
        }

        [Test]
        public void ExecuteRawSql()
        {
            //Arrange           
            var services = new ServiceCollection();
            services.AddDbContext<DbContextFoo>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionConfiguration")));
            var provider = services.BuildServiceProvider();
            var context = provider.GetService<DbContextFoo>();

            var resultFoo = Foo.New("Smarkets com br");

            if (resultFoo.IsSuccess)
                context.Foo.Add(resultFoo.Data);

            context.SaveChanges();

            //Act
            var parameters = new Dictionary<string, object>();

            var sql = @"SELECT name FROM Foo WHERE name LIKE @name";

            parameters.Add("@name", "%Smarkets%");

            var result = context.ExecuteRawSql<FooQuery>(sql, parameters);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.GreaterThan(0));
        }

        [Test]
        public void SetDeleted()
        {
            //Arrange           
            var services = new ServiceCollection();
            services.AddDbContext<DbContextFoo>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionConfiguration")));
            var provider = services.BuildServiceProvider();
            var context = provider.GetService<DbContextFoo>();

            var resultFoo = Foo.New("Smarkets com br");

            if (resultFoo.IsSuccess)
                context.Foo.Add(resultFoo.Data);

            context.SaveChanges();

            var newFoo = context.Foo.FirstOrDefault();

            //Act
            context.SetDeleted(newFoo);
            var deletedFoo = context.ChangeTracker.Entries<Foo>().Where(it => it.State == EntityState.Deleted).FirstOrDefault();

            //Assert
            Assert.That(deletedFoo, Is.Not.Null);
        }

        [Test]
        public async Task DeleteAsync()
        {
            //Arrange           
            var services = new ServiceCollection();
            services.AddDbContext<DbContextFoo>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionConfiguration")));
            var provider = services.BuildServiceProvider();
            var context = provider.GetService<DbContextFoo>();
            var key = Guid.NewGuid().ToString("N");

            var resultFoo = Foo.New($"Smarkets com br Deleted - {key}");

            if (resultFoo.IsSuccess)
                context.Foo.Add(resultFoo.Data);

            await context.SaveChangesAsync();

            var newFoo = await context.Foo.FirstOrDefaultAsync(it => it.Name == $"Smarkets com br Deleted - {key}");

            //Act
            context.SetDeleted(newFoo);
            await context.SaveChangesAsync();
            var deletedFoo = await context.Foo.FirstOrDefaultAsync(it => it.Name == $"Smarkets com br Deleted - {key}");

            //Assert
            Assert.That(deletedFoo, Is.Null);
        }

        [Test]
        public void SetModified()
        {
            //Arrange           
            var services = new ServiceCollection();
            services.AddDbContext<DbContextFoo>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionConfiguration")));
            var provider = services.BuildServiceProvider();
            var context = provider.GetService<DbContextFoo>();

            var resultFoo = Foo.New("Smarkets com br");

            if (resultFoo.IsSuccess)
                context.Foo.Add(resultFoo.Data);

            context.SaveChanges();

            var newFoo = context.Foo.FirstOrDefault();

            //Act
            context.SetModified(newFoo);
            var modifiedFoo = context.ChangeTracker.Entries<Foo>().Where(it => it.State == EntityState.Modified).FirstOrDefault();

            //Assert
            Assert.That(modifiedFoo, Is.Not.Null);
        }

        [Test]
        public async Task UpdateAsync()
        {
            //Arrange           
            var services = new ServiceCollection();
            services.AddDbContext<DbContextFoo>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionConfiguration")));
            var provider = services.BuildServiceProvider();
            var context = provider.GetService<DbContextFoo>();
            var key = Guid.NewGuid().ToString("N");
            var newKey = Guid.NewGuid().ToString("N");

            var resultFoo = Foo.New($"Smarkets com br Update - {key}");

            if (resultFoo.IsSuccess)
                context.Foo.Add(resultFoo.Data);

            await context.SaveChangesAsync();

            var newFoo = await context.Foo.FirstOrDefaultAsync(it => it.Name == $"Smarkets com br Update - {key}");

            //Act
            newFoo.Update($"Smarkets com br Update - {newKey}");
            context.SetModified(newFoo);
            await context.SaveChangesAsync();

            var modifiedFoo = await context.Foo.FirstOrDefaultAsync(it => it.Name == $"Smarkets com br Update - {newKey}");
            newFoo = await context.Foo.FirstOrDefaultAsync(it => it.Name == $"Smarkets com br Update - {key}");

            //Assert
            Assert.That(modifiedFoo, Is.Not.Null);
            Assert.That(newFoo, Is.Null);
        }

        public class DbContextFoo : DbContextBase, IDbContextFoo
        {
            public DbContextFoo(DbContextOptions dbContextOptions)
                : base(dbContextOptions)
            {
            }

            public DbSet<Foo> Foo { get; private set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                MapEntity(modelBuilder);
                MapQuery(modelBuilder);
            }

            private void MapEntity(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity(FooMapConfig.ConfigureMap());
            }

            private void MapQuery(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<FooQuery>().HasNoKey();
            }
        }

        public class FooMapConfig
        {
            public static Action<EntityTypeBuilder<Foo>> ConfigureMap()
            {
                return (entity) =>
                {
                    entity.ToTable("Foo");

                    entity.HasKey(it => it.Id);

                    entity.Property(it => it.Name)
                        .HasColumnName("Name")
                        .HasMaxLength(255)
                        .IsRequired();
                };
            }
        }

        public interface IDbContextFoo : IDbContext
        {
            DbSet<Foo> Foo { get; }
        }

        public class Foo : Entity, IAggregateRoot
        {
            public string Name { get; private set; }

            public static Result<Foo> New(string name)
            {
                var foo = new Foo { Name = name };

                return Validate(foo);
            }

            private static Result<Foo> Validate(Foo foo)
            {
                if (string.IsNullOrWhiteSpace(foo.Name))
                    return Result.Fail<Foo>("Name is required!");

                return Result.Ok(foo);
            }

            public void Update(string name) => Name = name;
        }

        public class FooQuery : ValueObject<FooQuery>
        {
            public FooQuery(string name) => Name = name;

            public string Name { get; private set; }
        }
    }
}
