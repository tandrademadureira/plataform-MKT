using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Shared.Util.Extension
{
    /// <summary>
    /// Extension for collections.
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
        /// var foo = new Foo("mkt App");
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
        ///     public PagedList<![CDATA[<Foo>]]> GetFoo() => GetFooRepository(1, 1);
        /// 
        ///     public PagedList<![CDATA[<Foo>]]> GetFooRepository(int page, int itemsPerPage)
        ///     {
        ///         var foo1 = new Foo("mkt App");
        ///         var foo2 = new Foo("mkt");
        ///         var foo3 = new Foo("Foo");
        /// 
        ///         var fooList = new List<![CDATA[<Foo>]]> { foo1, foo2, foo3 };
        ///         var query = fooList.AsQueryable();
        /// 
        ///         return query.ToPagedList(it => it.Name, page, itemsPerPage);
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <exception cref="ArgumentException">If ***page*** is less than 1, an exception is thrown with message <c>'The page can not be less than 1.'</c>.</exception>
        /// <exception cref="ArgumentException">If ***itemsPerPage*** is less than 1, an exception is thrown with message <c>'The items per page can not be less than 1.'</c>.</exception>
        public static PagedList<TSource> ToPagedList<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> order, int page = 1, int itemsPerPage = 100, bool orderedAsc = true)
        {
            if (page == 0)
                throw new ArgumentException("The page can not be less than 1.", "page");

            if (itemsPerPage == 0)
                throw new ArgumentException("The items per page can not be less than 1.", "itemsPerpage");

            var count = source.Count();
            var query = orderedAsc ? source.OrderBy(order) : source.OrderByDescending(order);
            var items = query.Skip(itemsPerPage * (page - 1)).Take(itemsPerPage).ToList();

            return new PagedList<TSource>(items, page, itemsPerPage, count, orderedAsc);
        }

        /// <summary>
        /// Concatenate string list on single string using one delimiter. If list not contains items, the return is string empty.
        /// </summary>
        /// <param name="value">String list with items to concatenate.</param>
        /// <param name="delimiter">String delimiter to separate items on string.</param>
        /// <returns>Single string with concatenating the items or empty.</returns>
        /// <example>
        /// <code>
        /// var stringList = new List<![CDATA[<string>]]> { "mkt App", "Transaction", "Repository" };
        /// var stringConcatenated = stringList.ToInlineConcat(" | ");
        /// </code>
        /// </example>
        public static string ToInlineConcat(this IEnumerable<string> value, string delimiter = "")
        {
            if (!value.Any())
                return string.Empty;

            return value.Aggregate((i, j) => i + delimiter + j);
        }

        /// <summary>
        /// Concatenates a collection of strings into a single string using the given delimiter.
        /// <para>If it is empty or is returning an empty string.</para>
        /// </summary>
        /// <param name="value">Collection with the values ​​that will be concatenated.</param>
        /// <param name="delimiter">Delimiter that will separate the values ​​within the list.</param>
        /// <returns>A single value concatenated with the supplied delimiter.</returns>
        /// <example>
        /// <code>
        /// IEnumerable<![CDATA[<string>]]> value = new List<![CDATA[<string>]]>() { "Framework", "mkt", "2.0" };
        /// var concatenatedValue = value.GetInLineConcat(" | ");
        /// </code>
        /// *The result is **Framework | mkt | 2.0**.*
        /// </example>
        public static string GetInLineConcat(this List<string> value, string delimiter = "")
        {
            if (value == null || !value.Any()) return string.Empty;

            return value.Aggregate((i, j) => i + delimiter + j);
        }
    }

    /// <summary>
    /// Class used to <see cref="CollectionExtension"/>.
    /// </summary>
    /// <typeparam name="T">List type provider to items.</typeparam>
    [Serializable]
    public class PagedList<T>
    {
        /// <summary>
        /// Default constructor to create a PagedList type. Very useful when transforming the object and passing the values ​​from  paged list to another.
        /// </summary>
        /// <param name="items">Items collection.</param>
        /// <param name="currentPage">Current page of the item collection.</param>
        /// <param name="itemsPerPage">Items per page of result.</param>
        /// <param name="totalCount">Total of all items of all pages.</param>
        /// <param name="orderedAsc">Order by use ***true*** to ascending or ***false*** to descending.</param>
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
        /// Bar class used in this example.
        /// <code>
        /// public class Bar
        /// {
        ///     public PagedList<![CDATA[<Foo>]]> TransformToPagedList(IList<![CDATA[<Foo>]]> fooList)
        ///     {
        ///         var currentPage = 1;
        ///         var itemsPerPage = 1;
        ///         var orderedAsc = true;
        ///         
        ///         // Sample using the default contructor.
        ///         return new PagedList<![CDATA[<Foo>]]>(fooList, currentPage, itemsPerPage, fooList.Count, orderedAsc);
        ///     }
        /// }
        /// </code>
        /// </example>
        public PagedList(IEnumerable<T> items, int currentPage, int itemsPerPage, long totalCount, bool orderedAsc)
        {
            Items = items;
            OrderedAsc = orderedAsc;
            CurrentPage = currentPage;
            TotalCount = totalCount;
            TotalPages = (int)(TotalCount == 0 ? 1 : ((TotalCount - 1) / itemsPerPage) + 1);
            ItemsPerPage = itemsPerPage;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PagedList() { }

        /// <summary>
        /// Collection of items paged.
        /// </summary>
        /// <value>The default value is empty collection, not null.</value>
        [JsonPropertyName("items")]
        public IEnumerable<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Total of all items of all pages.
        /// </summary>
        /// <value>The default value is 0.</value>
        [JsonPropertyName("totalCount")]
        public long TotalCount { get; set; }

        /// <summary>
        /// Total of all pages of all items.
        /// </summary>
        /// <value>The default value is 0.</value>
        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        /// <summary>
        /// Representing the current page.
        /// </summary>
        /// <value>The default value is 0.</value>
        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; }

        /// <summary>
        /// Total of items per page.
        /// </summary>
        /// <value>The default value is 0.</value>
        [JsonPropertyName("itemsPerPage")]
        public int ItemsPerPage { get; set; }

        /// <summary>
        /// Order by asending is equals ***true*** or descending is equals ***false***.
        /// </summary>
        /// <value>The default value is false.</value>
        [JsonPropertyName("orderAsc")]
        public bool OrderedAsc { get; set; }
    }
}
