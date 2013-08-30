namespace Ancestry.Daisy.Tests.Daisy.Component
{
    using System.Collections.Generic;

    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Daisy.Component.Controllers;
    using Ancestry.Daisy.Tests.Daisy.Component.Domain;

    using NUnit.Framework;

    [TestFixture, Category("Component")]
    public class TestExecutions
    {
        private StatementSet statements;

        [SetUp]
        public void Setup()
        {
            statements = new StatementSet().FromAssemblyOf(typeof(UserController));
        }

        private TestCaseData[] itExecutesStatements =
            {
                new TestCaseData(Statements.DoubleGroupEndings, new User()
                    {
                        Accounts = new List<Account>()
                            {
                                new Account()
                                    {
                                        Transactions = new List<Transaction>()
                                            {
                                                new Transaction()
                                                    {
                                                        Amount = -1
                                                    }
                                            }
                                    },
                            },
                            IsActive = true
                    } )
                .Returns(true)
                .SetName("It works with or groups"),
            };

        [TestCaseSource("itExecutesStatements")]
        public bool ItExecutesStatements(string code, User data)
        {
            var exec = DaisyCompiler.Compile<User>(code, statements).Execute(data);
            return exec.Result;
        }
    }
}
