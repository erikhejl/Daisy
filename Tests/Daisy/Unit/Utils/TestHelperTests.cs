namespace Ancestry.Daisy.Tests.Daisy.Unit.Utils
{
    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.TestHelpers;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NUnit.Framework;

    [TestFixture]
    public class TestHelperTests
    {
        private class MyStatement1 : StatementController<int>
        {
            [Matches("Good boy")]
            public bool R1()
            {
                Context.didIt = true;
                return Scope == 1;
            }
        }

        [Test]
        public void ItInvokesFunctions()
        {
            TestHelper.Invoke(typeof(MyStatement1), "R1", 1, "Good boy")
                .AssertMatched()
                .AssertResult(true)
                .AssertContextHas("didIt");
        }

        [Test,NUnit.Framework.ExpectedException(typeof(AssertFailedException))]
        public void ItAssertsMatchFailures()
        {
            TestHelper.Invoke(typeof(MyStatement1), "R1", 1, "Bad boy")
                .AssertMatched()
                ;
        }

        [Test,NUnit.Framework.ExpectedException(typeof(AssertFailedException))]
        public void ItAssertsResultWrong()
        {
            TestHelper.Invoke(typeof(MyStatement1), "R1", 2, "Bad boy")
                .AssertResult(true)
                ;
        }
    }
}
