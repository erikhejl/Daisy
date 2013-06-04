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
            return BaseReflectionHandler.NormalizeMethodName(name);
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
        public bool ItExecutesRules(string rule, string statement, int scope)
        {
            var load = new Daisy.Rules.ReflectionRuleHandler(GetMethod(rule), typeof(TestRules));
            var match = load.Matches(new MatchingContext() {
                    ScopeType = scope.GetType(),
                    Statement = statement

                });
            Assert.True(match.Success);
            return load.Execute(new ExecutionContext() {
                    Statement = statement,
                    Scope = scope,
                    Match = match,
                });
        }

        private MethodInfo GetMethod(string ruleName)
        {
            return typeof(TestRules).GetMethod(ruleName);
        }
    }
}
