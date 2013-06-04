using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Unit.Rules
{
    using Ancestry.Daisy.Rules;

    using NUnit.Framework;

    [TestFixture,Category("Unit")]
    public class StaticAnalysisTests
    {
        public class MyRule : RuleController<string>
        {
            public bool R1() { return true; }
        }

        public class MyRule2 : RuleSetTests.MyRule
        {
            public bool R2() { return true; }
        }

        [TestCase(typeof(RuleController<int>), Result = true)]
        [TestCase(typeof(MyRule), Result = true)]
        [TestCase(typeof(MyRule2), Result = true)]
        [TestCase(typeof(string), Result = false)]
        public bool ItDeterminesIfSomethingIsARuleController(Type type)
        {
            return StaticAnalysis.IsRuleController(type);
        }

        public class MyRule3 : RuleController<string>
        {
            public bool R1(Func<int, bool> proceed) { return true; }
            public bool R2(Func<int, string> proceed) { return true; }
            public bool R3() { return true; }
            public bool R4(int i) { return true; }
            private bool R5(Func<int, bool> proceed) { return true; }
        }

        [TestCase(typeof(MyRule3), "R3", Result = false, TestName = "Must have parameters")]
        [TestCase(typeof(MyRule3), "R4", Result = false, TestName = "Must have function")]
        [TestCase(typeof(MyRule3), "R2", Result = false, TestName = "Func must return bool")]
        [TestCase(typeof(MyRule3), "R1", Result = true)]
        public bool ItDeterminesIfSomethingIsAnAggregateRule(Type type, string methodName)
        {
            var method = type.GetMethod(methodName);
            Assert.IsNotNull(method);
            return StaticAnalysis.IsAggregateMethod(method);
        }

        [TestCase(typeof(int), Result = false, TestName = "Must be a func")]
        [TestCase(typeof(Tuple<int, bool>), Result = false, TestName = "Must be a func")]
        [TestCase(typeof(Func<string, string>), Result = false, TestName = "Must return bool")]
        [TestCase(typeof(Func<string, int, bool>), Result = false, TestName = "Must accept only one argument")]
        [TestCase(typeof(Func<string, bool>), Result = true)]
        public bool ItDeterminesIfSomethingIsAProceedFunction(Type type)
        {
            return StaticAnalysis.IsProceedFunction(type);
        }

        private int didIt = 0;
        [Test]
        public void ItConvertsPredicateTypes()
        {
            var method = GetType().GetMethod("DoIt");
            Func<object,bool> cont = j => (int)j % 2 == 0;
            method.Invoke(this, new object[] { StaticAnalysis.ConvertPredicate(typeof(int), cont)});
            Assert.AreEqual(1, didIt);
        }

        public void DoIt(Func<int,bool> isEven)
        {
            Assert.False(isEven(1));
            Assert.True(isEven(2));
            didIt++;
        }
    }
}
