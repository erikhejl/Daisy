namespace Ancestry.Daisy.Tests.Unit.Statements
{
    using System;

    using Ancestry.Daisy.Statements;

    using NUnit.Framework;

    [TestFixture,Category("Unit")]
    public class StaticAnalysisTests
    {
        public class MyStatement : StatementController<string>
        {
            public bool R1() { return true; }
        }

        public class MyStatement2 : StatementSetTests.MyStatement
        {
            public bool R2() { return true; }
        }

        [TestCase(typeof(StatementController<int>), Result = true)]
        [TestCase(typeof(MyStatement), Result = true)]
        [TestCase(typeof(MyStatement2), Result = true)]
        [TestCase(typeof(string), Result = false)]
        public bool ItDeterminesIfSomethingIsAStatementController(Type type)
        {
            return StaticAnalysis.IsStatementController(type);
        }

        public class MyStatement3 : StatementController<string>
        {
            public bool R1(Func<int, bool> proceed) { return true; }
            public bool R2(Func<int, string> proceed) { return true; }
            public bool R3() { return true; }
            public bool R4(int i) { return true; }
            private bool R5(Func<int, bool> proceed) { return true; }
        }

        [TestCase(typeof(MyStatement3), "R3", Result = false, TestName = "Must have parameters")]
        [TestCase(typeof(MyStatement3), "R4", Result = false, TestName = "Must have function")]
        [TestCase(typeof(MyStatement3), "R2", Result = false, TestName = "Func must return bool")]
        [TestCase(typeof(MyStatement3), "R1", Result = true)]
        public bool ItDeterminesIfSomethingIsAnAggregateStatement(Type type, string methodName)
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
