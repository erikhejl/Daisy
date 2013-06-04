using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Component
{
    using Ancestry.Daisy.Rules;
    using Ancestry.Daisy.Tests.Component.Controllers;
    using Ancestry.Daisy.Tests.Component.Domain;

    using NUnit.Framework;

    [TestFixture,Category("Component")]
    public class Executions
    {
        private RuleSet rules;

        [SetUp]
        public void Setup()
        {
            rules = new RuleSet().FromAssemblyOf(typeof(UserController));
        }

        private TestCaseData[] itExecutesRules =
            {
                new TestCaseData(Rules.UserHasNoRecentTransactions, TestData.Ben)
                .Returns(false)
                .SetName("Has no recent transaction"),
                new TestCaseData(Rules.UserIsOverdrawnOnChecking, TestData.Ben)
                .Returns(true)
                .SetName("Is overdrawn on checking"),
                new TestCaseData(Rules.UserHasUnusedMoneyMarket, TestData.Ben)
                .Returns(true)
                .SetName("Has unused money market account"),
            };

        [TestCaseSource("itExecutesRules")]
        public bool ItExecutesRules(string code, User data)
        {
            return DaisyCompiler.Compile<User>(code, rules)
                .Execute(data)
                .Result;
        }
    }
}
