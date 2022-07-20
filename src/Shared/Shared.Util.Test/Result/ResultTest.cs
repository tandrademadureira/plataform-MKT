using NUnit.Framework;
using System;

namespace Shared.Util.Test.Result
{
    [TestFixture]
    public class ResultTest
    {
        [Test]
        public void CreateResultOK()
        {
            var result = Shared.Util.Result.Result.Ok();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public void CreateResultOKCorrelationId()
        {
            var correlationId = Guid.NewGuid().ToString();
            var result = Shared.Util.Result.Result.Ok(correlationId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.CorrelationId);
            Assert.IsTrue(correlationId == result.CorrelationId);
        }

        [Test]
        public void CreateResultFail()
        {
            var result = Util.Result.Result.Fail("Error");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsFailure);
            Assert.IsNotEmpty(result.Error);
        }

        [Test]
        public void CreateResultFailError()
        {
            Assert.Throws<ArgumentNullException>(delegate { Util.Result.Result.Fail(null); });
        }

        [Test]
        public void CreateResultOKObject()
        {
            var obj = new { Id = 1, Name = "Smarkets" };
            var result = Shared.Util.Result.Result.Ok<object>(obj);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.True(result.Data.Equals(obj));
        }

        [Test]
        public void CreateResultFailObject()
        {
            var result = Util.Result.Result.Fail<object>("Error");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsFailure);
            Assert.IsNotEmpty(result.Error);
        }

        [Test]
        public void CreateResultFailObjectError()
        {
            Assert.Throws<ArgumentNullException>(delegate { Util.Result.Result.Fail<object>(null); });
        }
    }
}
