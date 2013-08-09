using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Component
{
    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Component.Controllers;
    using Ancestry.Daisy.Tests.Component.Domain;

    using NUnit.Framework;

    [TestFixture,Category("Component")]
    public class DomainExecutions
    {
        private StatementSet statements;

        [SetUp]
        public void Setup()
        {
            statements = new StatementSet().FromAssemblyOf(typeof(UserController));
        }

        private TestCaseData[] itExecutesStatments =
            {
                new TestCaseData(Statements.UserHasNoRecentTransactions, TestData.Ben)
                .Returns(false)
                .SetName("Has no recent transaction"),
                new TestCaseData(Statements.UserIsOverdrawnOnChecking, TestData.Ben)
                .Returns(true)
                .SetName("Is overdrawn on checking"),
                new TestCaseData(Statements.UserHasUnusedMoneyMarket, TestData.Ben)
                .Returns(true)
                .SetName("Has unused money market account"),
                new TestCaseData(Statements.UserHasNonCheckingWithABalance, TestData.Ben)
                .Returns(true)
                .SetName("Has non checking account with a balance"),
            };

        [TestCaseSource("itExecutesStatments")]
        public bool ItExecutesStatements(string code, User data)
        {
            return DaisyCompiler.Compile<User>(code, statements)
                .Execute(data)
                .Result;
        }
    }
}
