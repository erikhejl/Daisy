using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Unit.Utils
{
    using Ancestry.Daisy.Rules;
    using Ancestry.Daisy.TestHelpers;
    using Ancestry.Daisy.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NUnit.Framework;

    [TestFixture]
    public class TestHelperTests
    {
        private class MyRule1 : RuleController<int>
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
            TestHelper.Invoke(typeof(MyRule1), "R1", 1, "Good boy")
                .AssertMatched()
                .AssertResult(true)
                .AssertContextHas("didIt");
        }

        [Test,NUnit.Framework.ExpectedException(typeof(AssertFailedException))]
        public void ItAssertsMatchFailures()
        {
            TestHelper.Invoke(typeof(MyRule1), "R1", 1, "Bad boy")
                .AssertMatched()
                ;
        }

        [Test,NUnit.Framework.ExpectedException(typeof(AssertFailedException))]
        public void ItAssertsResultWrong()
        {
            TestHelper.Invoke(typeof(MyRule1), "R1", 2, "Bad boy")
                .AssertResult(true)
                ;
        }
    }
}
