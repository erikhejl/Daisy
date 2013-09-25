namespace Ancestry.Daisy.Tests.Daisy.Unit.Statements
{
    using System;
    using System.Linq;

    using Ancestry.Daisy.Statements;

    using NUnit.Framework;

    [TestFixture,Category("Unit")]
    public class StatementSetTests
    {
        [Test]
        public void ItAddsFromAssembly()
        {
            var load = new StatementSet();
            load.FromAssemblyOf(typeof(StatementSetTests));
            Assert.That(load.Statements.OfType<ReflectionStatementDefinition>().Any(x => x.Name == "R1"));
        }

        [Test]
        public void ItAddsFromType()
        {
            var load = new StatementSet();
            load.FromController(typeof(MyStatement));
            Assert.That(load.Statements.OfType<ReflectionStatementDefinition>().Any(x => x.Name == "R1"));
        }

        public class MyStatement : StatementController<string>
        {
            public bool R1() { return true; }
            public bool R2(Func<int,bool> proceed) { return true; }
        }
    }
}
