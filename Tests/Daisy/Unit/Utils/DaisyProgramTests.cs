namespace Ancestry.Daisy.Tests.Daisy.Unit.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Language.AST;
    using Ancestry.Daisy.Linking;
    using Ancestry.Daisy.Program;
    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.TestObjects;

    using NUnit.Framework;

    [TestFixture,Category("Unit")]
    public class DaisyProgramTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ItRequiresLinksToOperateOnSameBaseTypeAsProgram()
        {
            var links = new DaisyLinks(typeof(int));
            var ast = new DaisyAst(new Statement("Hi"));
            var load = new DaisyProgram<string>(ast, links);
        }

        private TestCaseData[] itRunsPrograms =
            {
                new TestCaseData("t")
                .SetName("It executes statements that are true")
                .Returns(true),
                new TestCaseData("f")
                .SetName("It executes statements that are false")
                .Returns(false),
                new TestCaseData("t\r\nt")
                .SetName("It evaluates ands - happy")
                .Returns(true),
                new TestCaseData("t\r\nf")
                .SetName("It evaluates ands - sad")
                .Returns(false),
                new TestCaseData("t\r\nOR f")
                .SetName("It evaluates ors - happy")
                .Returns(true),
                new TestCaseData("f\r\nOR f")
                .SetName("It evaluates ors - sad")
                .Returns(false),
                new TestCaseData("NOT f")
                .SetName("It evaluates nots - happy")
                .Returns(true),
                new TestCaseData("NOT t")
                .SetName("It evaluates nots - sad")
                .Returns(false),
                new TestCaseData("t\nAND\n  f\n  OR t")
                .SetName("It evaluates groups - happy")
                .Returns(true),
                new TestCaseData("t\nAND\n  f\n  OR f")
                .SetName("It evaluates groups - sad")
                .Returns(false),
                new TestCaseData(TestData.Code_f)
                .SetName("It evaluates a complicated program")
                .Returns(false),
            };

        [TestCaseSource("itRunsPrograms")]
        public bool ItRunsPrograms(string code)
        {
            var ast = DaisyParser.Parse(code);
            var links = new DaisyLinks(typeof(int));
            AddLink(links, "t", i => true);
            AddLink(links, "f", i => false);
            var program = new DaisyProgram<int>(ast, links);
            var result = program.Execute(1).Result;
            return result;
        }

        [TestCase("any\n  t", "1,2,3,4,5" ,Result = true)]
        [TestCase("any\n  even", "1,2,3,4,5" ,Result = true)]
        [TestCase("any\n  even", "1,3,5" ,Result = false)]
        public bool ItExecutesAggregates(string code,string values)
        {
            var ast = DaisyParser.Parse(code);
            var links = new DaisyLinks(typeof(IEnumerable<int>));
            AddLink(links, "even", i => i%2 == 0);
            AddLink(links, "t", i => true);
            AddAggregateLink(links, "any");
            var program = new DaisyProgram<IEnumerable<int>>(ast, links);
            var result = program.Execute(values.Split(',').Select(int.Parse)).Result;
            return result;
        }


        [Test]
        public void ItSanityChecksLinksForStatementEquality()
        {
            var ex = Assert.Catch<DaisyRuntimeException>(() =>
                DaisyProgram<int>.SanityCheckLink(new DaisyStatementLink() { Statement = "a" }, null, "b"));
            Assert.That(ex.Message.Contains("statement"));
        }

        [Test]
        public void ItSanityChecksLinksForScopeConsistency()
        {
            var ex = Assert.Catch<DaisyRuntimeException>(() =>
                DaisyProgram<int>.SanityCheckLink(new DaisyStatementLink() {
                        Statement = "a",
                        ScopeType = typeof(int)
                    },
                "bad scope", "a"));
            Assert.That(ex.Message.Contains("scope"));
        }

        public void AddLink(DaisyLinks links, string rawStatement, Func<int,bool> predicate)
        {
            var statement = new FakeStatement(rawStatement, predicate);
            links.AddLink(new DaisyStatementLink() {
                Match = statement.Matches(new MatchingContext(){Statement = rawStatement}),
                Handler = statement,
                ScopeType = typeof(int),
                Statement = rawStatement
            });
        }

        private void AddAggregateLink(DaisyLinks links, string rawStatement)
        {
            var statement = new FakeAggregate<int,int>(rawStatement);
            links.AddLink(new DaisyStatementLink() {
                Match = statement.Matches(new MatchingContext(){Statement = rawStatement}),
                Handler = statement,
                ScopeType = typeof(IEnumerable<int>),
                Statement = rawStatement
            });
        }
    }
}
