using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Unit.Rules
{
    using System.Reflection;

    using Ancestry.Daisy.Rules;

    using Monads.NET;

    using NUnit.Framework;

    [TestFixture,Category("Unit")]
    public class ReflectionRuleHandlerTests
    {
        [TestCase("IsResidence",Result = "Is\\s+Residence")]
        public string ItNormalizesMethodNames(string name)
        {
            return ReflectionRuleHandler.NormalizeMethodName(name);
        }

        private class TestRules : RuleController<int>
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
        }

        [TestCase("R1","R1",Result = true)]
        [TestCase("R1","R2",Result = false)]
        public bool ItMatchesRules(string rule, string statement)
        {
            var m = GetMethod(rule);
            var load = new Daisy.Rules.ReflectionRuleHandler(m, typeof(TestRules));
            var match = load.Matches(new MatchingContext() {
                    Statement = statement
                });
            return match.Let(x => x.Success,false);
        }

        [TestCase("R1","R1",1,Result = true, TestName = "It returns result of rule")]
        [TestCase("R2","R2",9,Result = true, TestName = "It sets scope")]
        [TestCase("R2","R2",8,Result = false, TestName = "It sets scope. Inverse")]
        [TestCase("R3","I haz 2 cheeseburgers",2,Result = true, TestName = "It sets parameter values")]
        [TestCase("R4","I haz 2 cheeseburgers",2,ExpectedException = typeof(CannotExecuteRuleException), TestName = "It errors when cannot make parameter")]
        [TestCase("R5","I haz 2 cheeseburgers",2,ExpectedException = typeof(CannotExecuteRuleException), TestName = "It errors when cannot make parameter")]
        [TestCase("R6","R6",9,Result = true, TestName = "It sets context")]
        public bool ItExecutesRules(string rule, string statement, int scope)
        {
            var load = new ReflectionRuleHandler(GetMethod(rule), typeof(TestRules));
            var match = load.Matches(new MatchingContext() {
                    ScopeType = scope.GetType(),
                    Statement = statement

                });
            Assert.True(match.Success);
            return load.Execute(new ExecutionContext() {
                    Statement = statement,
                    Scope = scope,
                    Match = match,
                    Context = new object(),
                });
        }

        private MethodInfo GetMethod(string ruleName)
        {
            return typeof(TestRules).GetMethod(ruleName);
        }

        [TestCase("R1", "R1", "1,2,3,4", 2, Result = true, TestName = "It returns result of rule")]
        [TestCase("R1", "R1", "1,3,5", 3, Result = false, TestName = "It sets scope. Inverse")]
        [TestCase("R2", "I haz 10 cheeseburgers", "1,2,3",2 , Result = true, TestName = "It sets parameter values")]
        [TestCase("R3", "I haz 10 cheeseburgers", "1", 1, ExpectedException = typeof(CannotExecuteRuleException), TestName = "It errors when cannot make parameter")] //too few
        [TestCase("R4", "I haz 10 cheeseburgers", "1", 1, ExpectedException = typeof(CannotExecuteRuleException), TestName = "It errors when cannot make parameter")] //too many
        public bool ItExecutesAggregates(string rule, string statement, string strScope, int expectedCalls)
        {
            var scope = strScope.Split(',').Select(int.Parse);
            var load = new ReflectionRuleHandler(GetAggregateMethod(rule), typeof(TestAggregates));
            var match = load.Matches(new MatchingContext() {
                ScopeType = scope.GetType(),
                Statement = statement
            });
            Assert.True(match.Success);
            var calls = 0;
            var result = load.Execute(new ExecutionContext() {
                Statement = statement,
                Scope = scope,
                Match = match,
                Proceed =  o =>
                {
                    calls++;
                    return ((int)o) % 2 == 0;
                }
            });
            Assert.AreEqual(expectedCalls,calls);
            return result;
        }

        private MethodInfo GetAggregateMethod(string ruleName)
        {
            return typeof(TestAggregates).GetMethod(ruleName);
        }

        private class TestAggregates : RuleController<IEnumerable<int>>
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
    }
}
