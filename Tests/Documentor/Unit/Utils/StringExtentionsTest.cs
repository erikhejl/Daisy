using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Documentor.Unit.Utils
{
    using Ancestry.Daisy.Documentation.Utils;

    using NUnit.Framework;

    [TestFixture,Category("Unit")]
    public class StringExtentionsTest
    {
        [TestCase("   A    ",Result = "A")]
        [TestCase("   A    AA   ",Result = "A AA")]
        public string ItConsolidatesWhiteSpace(string input)
        {
            return input.ConsolidateWhitespace();
        }
    }
}
