namespace Ancestry.Daisy.Tests.Unit.Statements
{
    using Ancestry.Daisy.Statements;

    using NUnit.Framework;

    [TestFixture,Category("Unit")]
    public class MatchesAttributeTests
    {
        [TestCase("a", Result = "^a$")]
        [TestCase("^a", Result = "^a$")]
        [TestCase("a$", Result = "^a$")]
        [TestCase("^a$", Result = "^a$")]
        public string ItAddesImplicitStartsAndEnds(string @in)
        {
            return new MatchesAttribute(@in).RegularExpression.ToString();
        }
    }
}
