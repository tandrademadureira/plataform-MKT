using Microsoft.EntityFrameworkCore;
using Shared.Util.Extension;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared.Infra.Data.Sql.Extensions
{
    /// <summary>
    /// Extensions for collections on SQL Server.
    /// </summary>
    public static class CollectionExtension
    {
        /// <summary>
        /// Get object of type <see cref="PagedList{T}"/> from ***IQueryable***.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the data source.</typeparam>
        /// <typeparam name="TKey">The type of the field  in the data source.</typeparam>
        /// <param name="source">The collection to be paginated.</param>
        /// <param name="order">Expression with the field that should be paginated.</param>
        /// <param name="page">Page number of items.</param>
        /// <param name="itemsPerPage">Total of items per page.</param>
        /// <param name="orderedAsc">Indicating the ordination. If value ***true*** the order is ascending else value ***false*** the order is descending.</param>
        /// <returns>Result of paginated collection.</returns>
        /// <example>
        /// Foo class used in this example.
        /// <code>
        /// public class Foo
        /// {
        ///     public int Id { get; private set; }
        ///     public string Name { get; private set; }
        ///     public Foo(string name) => Name = name;
        /// }
        /// </code>
        /// Creating a List of Foo class and converting in IQueryable.
        /// <code>
        /// var foo = new Foo("Smarkets com br");
        /// var fooList = new List<![CDATA[<Foo>]]> { foo };
        /// var query = fooList.AsQueryable();
        /// </code>
        /// Sample of expression of order parameter.
        /// <code>
        /// Expression<![CDATA[<Func<Foo, int>>]]> order = it => it.Id;
        /// </code>
        /// Bar class used in this full example.
        /// <code>
        /// public class Bar
        /// {
        ///     public async Task<![CDATA[<PagedList<Foo>>]]> GetFoo() => await GetFooRepository(1, 1);
        /// 
        ///     public async Task<![CDATA[<PagedList<Foo>>]]> GetFooRepositoryAsync(int page, int itemsPerPage)
        ///     {
        ///         var foo1 = new Foo("Smarkets com br");
        ///         var foo2 = new Foo("Smarkets");
        ///         var foo3 = new Foo("Foo");
        /// 
        ///         var fooList = new List<![CDATA[<Foo>]]> { foo1, foo2, foo3 };
        ///         var query = fooList.AsQueryable();
        /// 
        ///         return await query.ToPagedListAsync(it => it.Name, page, itemsPerPage);
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <exception cref="ArgumentException">If ***page*** is less than 1, an exception is thrown with message <c>'The page can not be less than 1.'</c>.</exception>
        /// <exception cref="ArgumentException">If ***itemsPerPage*** is less than 1, an exception is thrown with message <c>'The items per page can not be less than 1.'</c>.</exception>
        public static async Task<PagedList<TSource>> ToPagedListAsync<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> order, int page = 1, int itemsPerPage = 100, bool orderedAsc = true)
        {
            if (page == 0)
                throw new ArgumentException("The page can not be less than 1.", "page");

            if (itemsPerPage == 0)
                throw new ArgumentException("The items per page can not be less than 1.", "itemsPerpage");

            var count = await source.LongCountAsync();
            var query = orderedAsc ? source.OrderBy(order) : source.OrderByDescending(order);
            var items = await query.Skip(itemsPerPage * (page - 1)).Take(itemsPerPage).ToListAsync();

            return await Task.FromResult(new PagedList<TSource>(items, page, itemsPerPage, count, orderedAsc));
        }
    }
}
