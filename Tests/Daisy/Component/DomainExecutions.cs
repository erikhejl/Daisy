﻿using System;
using Ancestry.Daisy.Language.Walks;

namespace Ancestry.Daisy.Tests.Daisy.Component
{
    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Daisy.Component.Controllers;
    using Ancestry.Daisy.Tests.Daisy.Component.Domain;

    using NUnit.Framework;

    [TestFixture, Category("Component")]
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
            var execution = DaisyCompiler.Compile<User>(code, statements).Execute(data);
            Console.WriteLine(new DaisyTracePrinter(execution.DebugInfo.Trace).Print());
            return execution.Outcome;
        }
    }
}
