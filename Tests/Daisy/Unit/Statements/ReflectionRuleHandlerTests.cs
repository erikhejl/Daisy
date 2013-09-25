namespace Ancestry.Daisy.Tests.Daisy.Unit.Statements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Utils;

    using NUnit.Framework;

    [TestFixture,Category("Unit")]
    public class ReflectionStatementHandlerTests
    {
        [TestCase("IsResidence",Result = "Is\\s+Residence")]
        public string ItNormalizesMethodNames(string name)
        {
            return ReflectionStatementDefinition.NormalizeMethodName(name);
        }

        private class TestStatements : StatementController<int>
        {
            public bool R1()
            {
                return true;
            }

            public bool R2()
            {
                return Scope == 9;
            }

            [Matches("I haz (\\d+) cheeseburgers")]
            public bool R3(int haz)
            {
                return haz == Scope;
            }

            [Matches("I haz (\\d+) cheeseburgers")]
            public bool R4(DateTime haz)
            {
                return false;
            }

            [Matches("I haz (\\d+) cheeseburgers")]
            public bool R5()
            {
                return false;
            }

            public bool R6()
            {
                return Context != null;
            }

            [Matches(@"I haz (an a+wesome )?cheeseburgers")]
            public bool R7(string value)
            {
                return value == null;
            }
        }

        [TestCase("R1","R1",Result = true)]
        [TestCase("R1","R1AndSomeMore",Result = false)]
        [TestCase("R1","R2",Result = false)]
        public bool ItMatchesStatements(string statment, string statement)
        {
            var m = GetMethod(statment);
            var load = new ReflectionStatementDefinition(m, typeof(TestStatements));
            var match = load.Matches(new MatchingContext() {
                    Statement = statement
                });
            return match.Let(x => x.Success,false);
        }

        [TestCase("R1","R1",1,Result = true, TestName = "It returns result of statment")]
        [TestCase("R2","R2",9,Result = true, TestName = "It sets scope")]
        [TestCase("R2","R2",8,Result = false, TestName = "It sets scope. Inverse")]
        [TestCase("R3","I haz 2 cheeseburgers",2,Result = true, TestName = "It sets parameter values")]
        [TestCase("R4","I haz 2 cheeseburgers",2,ExpectedException = typeof(CannotExecuteStatementException), TestName = "It errors when cannot make parameter")]
        [TestCase("R5","I haz 2 cheeseburgers",2,ExpectedException = typeof(CannotExecuteStatementException), TestName = "It errors when cannot make parameter")]
        [TestCase("R6","R6",9,Result = true, TestName = "It sets context")]
        [TestCase("R7","I haz cheeseburgers",1,Result = true, TestName = "It injects null for non-captured groups")]
        public bool ItExecutesStatements(string statement, string rawStatement, int scope)
        {
            var load = new ReflectionStatementDefinition(GetMethod(statement), typeof(TestStatements));
            var linked = load.Link(rawStatement);
            Assert.IsNotNull(linked);
            return linked.Execute(new InvokationContext() {
                    Scope = scope,
                    Context = new object(),
                });
        }

        [TestCase("R1", "R1", "1,2,3,4", 2, Result = true, TestName = "It returns result of statment")]
        [TestCase("R1", "R1", "1,3,5", 3, Result = false, TestName = "It sets scope. Inverse")]
        [TestCase("R2", "I haz 10 cheeseburgers", "1,2,3",2 , Result = true, TestName = "It sets parameter values")]
        [TestCase("R3", "I haz 10 cheeseburgers", "1", 1, ExpectedException = typeof(CannotExecuteStatementException), TestName = "It errors when cannot make parameter")] //too few
        [TestCase("R4", "I haz 10 cheeseburgers", "1", 1, ExpectedException = typeof(CannotExecuteStatementException), TestName = "It errors when cannot make parameter")] //too many
        public bool ItExecutesAggregates(string statement, string rawStatement, string strScope, int expectedCalls)
        {
            var scope = strScope.Split(',').Select(int.Parse);
            var load = new ReflectionStatementDefinition(GetAggregateMethod(statement), typeof(TestAggregates));
            var link = load.Link(rawStatement);
            Assert.IsNotNull(link);
            var calls = 0;
            var result = link.Execute(new InvokationContext() {
                Scope = scope,
                Proceed =  o =>
                {
                    calls++;
                    return ((int)o) % 2 == 0;
                }
            });
            Assert.AreEqual(expectedCalls,calls);
            return result;
        }

        private MethodInfo GetAggregateMethod(string statementName)
        {
            return typeof(TestAggregates).GetMethod(statementName);
        }

        private class TestAggregates : StatementController<IEnumerable<int>>
        {
            public bool R1(Func<int,bool> proceed)
            {
                return Scope.Any(proceed);
            }

            [Matches("I haz (\\d+) cheeseburgers")]
            public bool R2(int haz,Func<int,bool> proceed)
            {
                Assert.AreEqual(10, haz);
                return Scope.Any(proceed);
            }

            [Matches("I haz (\\d+) cheeseburgers")]
            public bool R3(int haz,int other,Func<int,bool> proceed)
            {
                Assert.Fail();
                return false;
            }

            [Matches("I haz (\\d+) cheeseburgers")]
            public bool R4(Func<int,bool> proceed)
            {
                Assert.Fail();
                return false;
            }

            public bool R5()
            {
                return false;
            }
        }

        private MethodInfo GetMethod(string statementName)
        {
            return typeof(TestStatements).GetMethod(statementName);
        }
    }
}
