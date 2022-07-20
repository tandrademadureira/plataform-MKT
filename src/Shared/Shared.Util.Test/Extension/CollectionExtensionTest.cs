using NUnit.Framework;
using Shared.Util.Extension;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Util.Test.Extension
{
    public class CollectionExtensionTest
    {
        [Test]
        public void TransformToPagedList()
        {
            //Arrange
            var name = "Smarkets App";
            var foo = new Foo(name);
            var fooList = new List<Foo> { foo };

            //Act
            var pagedList = new PagedList<Foo>(fooList, 1, 1, fooList.Count, true);

            //Assert
            Assert.That(pagedList.Items, Is.Not.Null);
            Assert.That(pagedList.Items, Has.Exactly(1).Items);
            Assert.That(pagedList.Items, Is.All.InstanceOf<Foo>());
            Assert.That(pagedList.CurrentPage, Is.EqualTo(1));
            Assert.That(pagedList.ItemsPerPage, Is.EqualTo(fooList.Count));
            Assert.That(pagedList.OrderedAsc, Is.True);
            Assert.That(pagedList.TotalCount, Is.EqualTo(fooList.Count));
            Assert.That(pagedList.TotalPages, Is.EqualTo(1));
        }

        [TestCase(1, 1, true, ExpectedResult = "Foo")]
        [TestCase(2, 1, true, ExpectedResult = "Smarkets")]
        [TestCase(3, 1, true, ExpectedResult = "Smarkets App")]
        [TestCase(1, 1, false, ExpectedResult = "Smarkets App")]
        [TestCase(2, 1, false, ExpectedResult = "Smarkets")]
        [TestCase(3, 1, false, ExpectedResult = "Foo")]
        public string GetPagedListValidateOrder(int page, int itemsPerPage, bool orderAsc)
        {
            //Arrange
            var foo1 = new Foo("Smarkets App");
            var foo2 = new Foo("Smarkets");
            var foo3 = new Foo("Foo");
            var fooList = new List<Foo> { foo1, foo2, foo3 };

            var query = fooList.AsQueryable();

            //Act
            var pagedList = query.ToPagedList(it => it.Name, page, itemsPerPage, orderAsc);

            //Assert
            return pagedList.Items.Select(it => it.Name).FirstOrDefault();
        }

        [TestCase(" - ", "Smarkets", "Framework", "2.0", ExpectedResult = "Smarkets - Framework - 2.0")]
        [TestCase(" | ", "Smarkets", "Framework", "2.0", ExpectedResult = "Smarkets | Framework | 2.0")]
        [TestCase("_", "Smarkets", "Framework", "2.0", ExpectedResult = "Smarkets_Framework_2.0")]
        public string GetInLineConcat(string delimiter, params string[] collection)
        {
            //Arrange

            //Act
            var concatenatedValue = collection.ToList().GetInLineConcat(delimiter);

            //Assert
            return concatenatedValue;
        }

        [TestCase(1, 1, true, ExpectedResult = 1)]
        [TestCase(2, 1, true, ExpectedResult = 2)]
        [TestCase(3, 1, true, ExpectedResult = 3)]
        [TestCase(2, 2, true, ExpectedResult = 2)]
        [TestCase(3, 3, true, ExpectedResult = 3)]
        public int GetPagedListValidateCurrentPage(int page, int itemsPerPage, bool orderAsc)
        {
            //Arrange
            var foo1 = new Foo("Smarkets App");
            var foo2 = new Foo("Smarkets");
            var foo3 = new Foo("Foo");
            var fooList = new List<Foo> { foo1, foo2, foo3 };

            var query = fooList.AsQueryable();

            //Act
            var pagedList = query.ToPagedList(it => it.Name, page, itemsPerPage, orderAsc);

            //Assert
            return pagedList.CurrentPage;
        }

        [TestCase(1, 1, true, ExpectedResult = 1)]
        [TestCase(2, 1, true, ExpectedResult = 1)]
        [TestCase(3, 1, true, ExpectedResult = 1)]
        [TestCase(2, 2, true, ExpectedResult = 2)]
        [TestCase(1, 3, true, ExpectedResult = 3)]
        public int GetPagedListValidateItemsPerPage(int page, int itemsPerPage, bool orderAsc)
        {
            //Arrange
            var foo1 = new Foo("Smarkets App");
            var foo2 = new Foo("Smarkets");
            var foo3 = new Foo("Foo");
            var fooList = new List<Foo> { foo1, foo2, foo3 };

            var query = fooList.AsQueryable();

            //Act
            var pagedList = query.ToPagedList(it => it.Name, page, itemsPerPage, orderAsc);

            //Assert
            return pagedList.ItemsPerPage;
        }

        [TestCase(1, 1, true, ExpectedResult = 3)]
        [TestCase(2, 2, true, ExpectedResult = 2)]
        [TestCase(3, 3, true, ExpectedResult = 1)]
        [TestCase(1, 3, true, ExpectedResult = 1)]
        public int GetPagedListValidateTotalPages(int page, int itemsPerPage, bool orderAsc)
        {
            //Arrange
            var foo1 = new Foo("Smarkets App");
            var foo2 = new Foo("Smarkets");
            var foo3 = new Foo("Foo");
            var fooList = new List<Foo> { foo1, foo2, foo3 };

            var query = fooList.AsQueryable();

            //Act
            var pagedList = query.ToPagedList(it => it.Name, page, itemsPerPage, orderAsc);

            //Assert
            return pagedList.TotalPages;
        }

        [TestCase(1, 1, true, ExpectedResult = 3)]
        [TestCase(2, 2, true, ExpectedResult = 3)]
        [TestCase(3, 3, true, ExpectedResult = 3)]
        [TestCase(1, 3, true, ExpectedResult = 3)]
        public long GetPagedListValidateTotalCount(int page, int itemsPerPage, bool orderAsc)
        {
            //Arrange
            var foo1 = new Foo("Smarkets App");
            var foo2 = new Foo("Smarkets");
            var foo3 = new Foo("Foo");
            var fooList = new List<Foo> { foo1, foo2, foo3 };

            var query = fooList.AsQueryable();

            //Act
            var pagedList = query.ToPagedList(it => it.Name, page, itemsPerPage, orderAsc);

            //Assert
            return pagedList.TotalCount;
        }

        [TestCase(1, 1, true, ExpectedResult = 1)]
        [TestCase(2, 2, true, ExpectedResult = 1)]
        [TestCase(1, 2, true, ExpectedResult = 2)]
        [TestCase(3, 2, true, ExpectedResult = 0)]
        [TestCase(3, 1, true, ExpectedResult = 1)]
        public long GetPagedListValidateTotalItems(int page, int itemsPerPage, bool orderAsc)
        {
            //Arrange
            var foo1 = new Foo("Smarkets App");
            var foo2 = new Foo("Smarkets");
            var foo3 = new Foo("Foo");
            var fooList = new List<Foo> { foo1, foo2, foo3 };

            var query = fooList.AsQueryable();

            //Act
            var pagedList = query.ToPagedList(it => it.Name, page, itemsPerPage, orderAsc);

            //Assert
            return pagedList.Items.Count();
        }

        [TestCase(" | ", "Smarkets App", "Bather", "Summer", ExpectedResult = "Smarkets App | Bather | Summer")]
        [TestCase(";", "Smarkets App", "Bather", "Summer", ExpectedResult = "Smarkets App;Bather;Summer")]
        [TestCase("", "Smarkets App", "Bather", "Summer", ExpectedResult = "Smarkets AppBatherSummer")]
        public string ToInlineConcat(string delimiter, params string[] stringArray)
        {
            //Arrange
            IList<string> stringList = stringArray.ToList();

            //Act
            var stringConcatenated = stringList.ToInlineConcat(delimiter);

            //Assert
            return stringConcatenated;
        }

        [TestCase(" | ", ExpectedResult = "")]
        public string ToInlineConcatEmpty(string delimiter)
        {
            //Arrange
            IList<string> stringList = new List<string>();

            //Act
            var stringConcatenated = stringList.ToInlineConcat(delimiter);

            //Assert
            return stringConcatenated;
        }

        public class Foo
        {
            public Foo(string name) => Name = name;

            public int Id { get; private set; }
            public string Name { get; private set; }
        }
    }
}
