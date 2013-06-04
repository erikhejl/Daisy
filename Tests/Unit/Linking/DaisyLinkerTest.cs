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
    using Ancestry.Daisy.Rules;

    using Moq;

    using NUnit.Framework;

    [TestFixture,Category("Unit")]
    public class DaisyLinkerTest
    {
        [Test]
        public void ItLinksRules()
        {
            var match = new Regex("a").Match("a");
            var rule = new Mock<IRuleHandler>();
            rule.Setup(x => x.Matches(It.IsAny<MatchingContext>())).Returns(match);
            rule.SetupGet(x => x.Name).Returns("Tennant");
            rule.SetupGet(x => x.ScopeType).Returns(typeof(Int32));
            var ruleSet = new RuleSet().Add(rule.Object);

            var ast = new DaisyAst(new Statement("Hello gov'nor"));

            var load = new DaisyLinker(ast,ruleSet,typeof(int));

            var links = load.Link();
            var linkFor = links.RuleFor("Hello gov'nor", typeof(int));
            Assert.AreEqual("Tennant",linkFor.Handler.Name);
            Assert.AreEqual(match, linkFor.Match);
        }

        /*
        [Test]
        public void ItLinksAggregates()
        {
            var match = new Regex("a").Match("a");
            var aggregate = new Mock<IAggregateHandler>();
            aggregate.Setup(x => x.Matches(It.IsAny<MatchingContext>())).Returns(match);
            aggregate.SetupGet(x => x.Name).Returns("Tennant");
            aggregate.SetupGet(x => x.ScopeType).Returns(typeof(Int32));
            aggregate.SetupGet(x => x.TransformsScopeTo).Returns(typeof(string));

            var rule = new Mock<IRuleHandler>();
            rule.Setup(x => x.Matches(It.IsAny<MatchingContext>())).Returns(match);
            rule.SetupGet(x => x.Name).Returns("rule");
            rule.SetupGet(x => x.ScopeType).Returns(typeof(string));
            var ruleSet = new RuleSet().Add(rule.Object).Add(aggregate.Object);

            var ast = new DaisyAst(new GroupOperator("Hello gov'nor",new Statement("Hi there")));

            var load = new DaisyLinker(ast,ruleSet,typeof(int));

            var links = load.Link();
            var linkFor = links.AggregateFor("Hello gov'nor");
            Assert.AreEqual("Tennant",linkFor.Handler.Name);
            Assert.AreEqual(match, linkFor.Match);
        }
        */

        [Test]
        public void ItDiesOnFailureToLink()
        {
            var ruleSet = new RuleSet();
            var ast = new DaisyAst(new Statement("Hello gov'nor"));
            var load = new DaisyLinker(ast,ruleSet,typeof(int));
            var ex = Assert.Catch<FailedLinkException>(() => load.Link());
            Assert.AreEqual(1, ex.Errors.Count);
            Assert.IsInstanceOf<NoLinksFoundError>(ex.Errors.First());
        }

        [Test]
        public void ItDiesOnMultipleLinksFound()
        {
            var match = new Regex("a").Match("a");
            var rule = new Mock<IRuleHandler>();
            rule.Setup(x => x.Matches(It.IsAny<MatchingContext>())).Returns(match);
            rule.SetupGet(x => x.Name).Returns("David");
            rule.SetupGet(x => x.ScopeType).Returns(typeof(int));

            var rule2 = new Mock<IRuleHandler>();
            rule2.Setup(x => x.Matches(It.IsAny<MatchingContext>())).Returns(match);
            rule2.SetupGet(x => x.Name).Returns("Tennant");
            rule2.SetupGet(x => x.ScopeType).Returns(typeof(int));
            var ruleSet = new RuleSet().Add(rule.Object).Add(rule2.Object);

            var ast = new DaisyAst(new Statement("Hello gov'nor"));

            var load = new DaisyLinker(ast,ruleSet,typeof(int));
            var ex = Assert.Catch<FailedLinkException>(() => load.Link());
            Assert.AreEqual(1, ex.Errors.Count);
            Assert.IsInstanceOf<MultipleLinksFoundError>(ex.Errors.First());
        }
    }
}
