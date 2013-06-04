using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Unit.Rules
{
    using System.Reflection;

    using Ancestry.Daisy.Rules;

    using NUnit.Framework;

    [TestFixture,Category("Unit")]
    public class RuleSetTests
    {
        [Test]
        public void ItAddsFromAssembly()
        {
            var load = new RuleSet();
            load.FromAssemblyOf(typeof(RuleSetTests));
            Assert.That(load.Rules.OfType<Daisy.Rules.ReflectionRuleHandler>().Any(x => x.Name == "R1"));
        }

        [Test]
        public void ItAddsFromType()
        {
            var load = new RuleSet();
            load.FromController(typeof(MyRule));
            Assert.That(load.Rules.OfType<Daisy.Rules.ReflectionRuleHandler>().Any(x => x.Name == "R1"));
        }

        public class MyRule : RuleController<string>
        {
            public bool R1() { return true; }
            public bool R2(Func<int,bool> proceed) { return true; }
        }
    }
}
