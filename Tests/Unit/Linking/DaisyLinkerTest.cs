using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Unit.Linking
{
    using System.Text.RegularExpressions;

    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Language.AST;
    using Ancestry.Daisy.Linking;
    using Ancestry.Daisy.Statements;

    using Moq;

    using NUnit.Framework;

    [TestFixture,Category("Unit")]
    public class DaisyLinkerTest
    {
        [Test]
        public void ItLinksStatements()
        {
            var match = new Regex("a").Match("a");
            var statement = new Mock<IStatementHandler>();
            statement.Setup(x => x.Matches(It.IsAny<MatchingContext>())).Returns(match);
            statement.SetupGet(x => x.Name).Returns("Tennant");
            statement.SetupGet(x => x.ScopeType).Returns(typeof(Int32));
            var statementSet = new StatementSet().Add(statement.Object);

            var ast = new DaisyAst(new Statement("Hello gov'nor"));

            var load = new DaisyLinker(ast,statementSet,typeof(int));

            var links = load.Link();
            var linkFor = links.StatementFor("Hello gov'nor", typeof(int));
            Assert.AreEqual("Tennant",linkFor.Handler.Name);
            Assert.AreEqual(match, linkFor.Match);
        }

        [Test]
        public void ItDoesNotLinkAnonymousGroups()
        {
            var match = new Regex("a").Match("a");
            var statement = new Mock<IStatementHandler>();
            statement.Setup(x => x.Matches(It.IsAny<MatchingContext>())).Returns(match);
            statement.SetupGet(x => x.Name).Returns("Tennant");
            statement.SetupGet(x => x.ScopeType).Returns(typeof(Int32));

            var statementSet = new StatementSet().Add(statement.Object);

            var ast = new DaisyAst(new GroupOperator(null,new Statement("a")));

            var load = new DaisyLinker(ast,statementSet,typeof(int));

            var links = load.Link();
            var linkFor = links.StatementFor("a", typeof(int));
            Assert.AreEqual("Tennant",linkFor.Handler.Name);
            Assert.AreEqual(match, linkFor.Match);
        }

        [Test]
        public void ItDiesOnFailureToLink()
        {
            var statementSet = new StatementSet();
            var ast = new DaisyAst(new Statement("Hello gov'nor"));
            var load = new DaisyLinker(ast,statementSet,typeof(int));
            var ex = Assert.Catch<FailedLinkException>(() => load.Link());
            Assert.AreEqual(1, ex.Errors.Count);
            Assert.IsInstanceOf<NoLinksFoundError>(ex.Errors.First());
        }

        [Test]
        public void ItDiesOnMultipleLinksFound()
        {
            var match = new Regex("a").Match("a");
            var statement = new Mock<IStatementHandler>();
            statement.Setup(x => x.Matches(It.IsAny<MatchingContext>())).Returns(match);
            statement.SetupGet(x => x.Name).Returns("David");
            statement.SetupGet(x => x.ScopeType).Returns(typeof(int));

            var statement2 = new Mock<IStatementHandler>();
            statement2.Setup(x => x.Matches(It.IsAny<MatchingContext>())).Returns(match);
            statement2.SetupGet(x => x.Name).Returns("Tennant");
            statement2.SetupGet(x => x.ScopeType).Returns(typeof(int));
            var statementSet = new StatementSet().Add(statement.Object).Add(statement2.Object);

            var ast = new DaisyAst(new Statement("Hello gov'nor"));

            var load = new DaisyLinker(ast,statementSet,typeof(int));
            var ex = Assert.Catch<FailedLinkException>(() => load.Link());
            Assert.AreEqual(1, ex.Errors.Count);
            Assert.IsInstanceOf<MultipleLinksFoundError>(ex.Errors.First());
        }

        private class A {}
        private class B : A {}

        [Test]
        public void ItAllowsLinkingToMoreGeneralStatementHandlers()
        {
            var match = new Regex("a").Match("a");
            var statement = new Mock<IStatementHandler>();
            statement.Setup(x => x.Matches(It.IsAny<MatchingContext>())).Returns(match);
            statement.SetupGet(x => x.ScopeType).Returns(typeof(A));
            var statementSet = new StatementSet().Add(statement.Object);

            var ast = new DaisyAst(new Statement("a"));

            var sut = new DaisyLinker(ast, statementSet, typeof(B));
            var links = sut.Link();
            var matched = links.StatementFor("a", typeof(B));
            Assert.IsNotNull(matched);
        }

    }
}
