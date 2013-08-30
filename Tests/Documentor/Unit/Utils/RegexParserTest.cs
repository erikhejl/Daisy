namespace Ancestry.Daisy.Tests.Documentor.Unit.Utils
{
    using Ancestry.Daisy.Documentation.Utils;

    using NUnit.Framework;

    [TestFixture,Category("Unit")]
    public class RegexParserTest
    {
        [TestCase("aaa",false,0,0)]
        [TestCase("a(a)a",true,1,3)]
        [TestCase(@"a\(a\)a",false,0,0)]
        [TestCase(@"a\((abba)\)a",true,3,6)]
        [TestCase(@"a((abba))a",true,1,8)]
        public void ItParsesGroups(string expression, bool found, int offset, int length)
        {
            var span = RegexParser.FirstGroup(expression);
            Assert.AreEqual(found, span != null);
            if (!found) return;
            Assert.AreEqual(offset, span.Value.Offset);
            Assert.AreEqual(length, span.Value.Length);
        }

        [TestCase("aaaa",1,1,"b",Result = "abaa")]
        [TestCase("abba",1,2,"", Result =  "aa")]
        [TestCase("abba",0,2,"cccccc", Result =  "ccccccba")]
        [TestCase("abba",2,2,"dd", Result =  "abdd")]
        public string ItReplaces(string input, int offset, int length, string with)
        {
            return RegexParser.Replace(input, new RegexParser.TextSpan() { Offset = offset, Length = length }, with);
        }
    }
}
