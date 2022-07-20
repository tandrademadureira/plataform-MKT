using NUnit.Framework;
using Shared.Util.Extension;
using System;
using System.Collections.Generic;

namespace Shared.Util.Test.Extension
{
    [TestFixture]
    public class ObjectExtensionTest
    {
        [Test]
        public void ToQueryString()
        {
            var date = new DateTime(2019, 12, 26);
            var bar = new Bar { BarId = 2, Value = "Bar", Date = date };
            //Arrange
            var foo = new Foo
            {
                IdFoo = 1,
                Name = "Smarkets App",
                Code = "IBTH",
                Bar = bar,
                Bares = new List<Bar>() { new Bar { BarId = 3, Value = "Bar3", Date = date.AddDays(5) }, { new Bar { BarId = 4, Value = "Bar4", Date = date.AddDays(18), NewBares = new List<Bar>() { new Bar { BarId = 3, Value = "Bar3", Date = date.AddDays(5) }, { new Bar { BarId = 4, Value = "Bar4", Date = date.AddDays(18) } } } } } }
            };

            //Act
            var result = foo.ToQueryString();

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(string.IsNullOrWhiteSpace(result), Is.False);
            Assert.That(result, Is.EqualTo("IdFoo=1&Name=Smarkets+App&Code=IBTH&Bar.BarId=2&Bar.Value=Bar&Bar.Date=2019-12-26T00%3A00%3A00&Bares%5B0%5D.BarId=3&Bares%5B0%5D.Value=Bar3&Bares%5B0%5D.Date=2019-12-31T00%3A00%3A00&Bares%5B1%5D.BarId=4&Bares%5B1%5D.Value=Bar4&Bares%5B1%5D.Date=2020-01-13T00%3A00%3A00&Bares%5B1%5D.NewBares%5B0%5D.BarId=3&Bares%5B1%5D.NewBares%5B0%5D.Value=Bar3&Bares%5B1%5D.NewBares%5B0%5D.Date=2019-12-31T00%3A00%3A00&Bares%5B1%5D.NewBares%5B1%5D.BarId=4&Bares%5B1%5D.NewBares%5B1%5D.Value=Bar4&Bares%5B1%5D.NewBares%5B1%5D.Date=2020-01-13T00%3A00%3A00"));

        }

        [Test]
        public void ToByteArray()
        {
            //Arrange
            var foo = new Foo
            {
                IdFoo = 1,
                Name = "Smarkets App",
                Code = "IBTH",
            };

            //Act
            var result = foo.ToByteArray();

            //Assert
            Assert.That(result, Is.Not.Null);

        }

        public class Foo
        {
            public int IdFoo { get; set; }

            public string Name { get; set; }

            public string Code { get; set; }

            public Bar Bar { get; set; }

            public IEnumerable<Bar> Bares { get; set; }
        }

        public class Bar
        {
            public int BarId { get; set; }

            public string Value { get; set; }

            public DateTime Date { get; set; }

            public IEnumerable<Bar> NewBares { get; set; }
        }
    }
}
