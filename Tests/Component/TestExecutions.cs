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

    [TestFixture, Category("Component")]
    public class TestExecutions
    {
        private RuleSet rules;

        [SetUp]
        public void Setup()
        {
            rules = new RuleSet().FromAssemblyOf(typeof(UserController));
        }

        private TestCaseData[] itExecutesRules =
            {
                new TestCaseData(Rules.DoubleGroupEndings, new User()
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

        [TestCaseSource("itExecutesRules")]
        public bool ItExecutesRules(string code, User data)
        {
            var exec = DaisyCompiler.Compile<User>(code, rules).Execute(data);
            return exec.Result;
        }
    }
}
