using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Unit.Linking
{
    using Ancestry.Daisy.Linking;

    using NUnit.Framework;

    [TestFixture]
    public class DaisyLinksTests
    {
        [Test]
        public void ItFindsByType()
        {
            var link = new DaisyStatementLink()
                {
                    Statement = "a",
                    ScopeType = typeof(int)
                };
            var load = new DaisyLinks(typeof(int));
            load.AddLink(link);
            Assert.IsNotNull(load.StatementFor("a",typeof(int)));
        }

        private class MyClassA { }

        private class MyClassB : MyClassA {}

        [Test]
        public void ItFindsByDerivableType()
        {
            var link = new DaisyStatementLink()
                {
                    Statement = "a",
                    ScopeType = typeof(MyClassA)
                };
            var load = new DaisyLinks(typeof(int));
            load.AddLink(link);
            Assert.IsNotNull(load.StatementFor("a",typeof(MyClassB)));
        }

        [Test]
        public void ItAllowsForDoubleLinking()
        {
            var link = new DaisyStatementLink()
                {
                    Statement = "a",
                    ScopeType = typeof(int)
                };
            var load = new DaisyLinks(typeof(int));
            load.AddLink(link);
            load.AddLink(link);
            Assert.IsNotNull(load.StatementFor("a",typeof(int)));
        }

    }
}
