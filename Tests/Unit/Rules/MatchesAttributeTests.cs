using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Unit.Rules
{
    using Ancestry.Daisy.Rules;

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
