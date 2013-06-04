using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Unit
{
    using NUnit.Framework;

    [TestFixture,Category("Unit")]
    public class StringExtentionsTests
    {
        [TestCase("","")]
        [TestCase("a","a")]
        [TestCase("A","A")]
        [TestCase("AaBb","Aa|Bb")]
        [TestCase("TheDogIsIn","The|Dog|Is|In")]
        [TestCase("theDogIsIn","the|Dog|Is|In")]
        [TestCase("a_b_c","a|b|c")]
        [TestCase("and_then_ItHappened","and|then|It|Happened")]
        public void ItSplitsBasedOnCasing(string @in, string expected)
        {
            var @out = StringExtentions.SplitNameParts(@in);
            var normed = string.Join("|", @out);
            Assert.AreEqual(expected, normed);
        }
    }
}
