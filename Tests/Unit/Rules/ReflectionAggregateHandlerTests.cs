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
    public class ReflectionAggregateHandlerTests
    {
        [TestCase("R1", "R1", "1,2,3,4", 2, Result = true, TestName = "It returns result of rule")]
        [TestCase("R1", "R1", "1,3,5", 3, Result = false, TestName = "It sets scope. Inverse")]
        [TestCase("R2", "I haz 10 cheeseburgers", "1,2,3",2 , Result = true, TestName = "It sets parameter values")]
        [TestCase("R3", "I haz 10 cheeseburgers", "1", 1, ExpectedException = typeof(CannotExecuteRuleException), TestName = "It errors when cannot make parameter")] //too few
        [TestCase("R4", "I haz 10 cheeseburgers", "1", 1, ExpectedException = typeof(CannotExecuteRuleException), TestName = "It errors when cannot make parameter")] //too many
        [TestCase("R5", "R5", "1", 1, ExpectedException = typeof(ArgumentException), TestName = "It errors when has no proceed function")]
        public bool ItExecutesAggregates(string rule, string statement, string strScope, int expectedCalls)
        {
            var scope = strScope.Split(',').Select(int.Parse);
            var load = new ReflectionAggregateHandler(GetMethod(rule), typeof(TestAggregates));
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
            }, o =>
                {
                    calls++;
                    return ((int)o) % 2 == 0;
                });
            Assert.AreEqual(expectedCalls,calls);
            return result;
        }

        private MethodInfo GetMethod(string ruleName)
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
                Assert.Fail();
                return false;
            }
        }
    }
}
