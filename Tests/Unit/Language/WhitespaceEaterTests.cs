using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Unit.Language
{
    using Ancestry.Daisy.Language;

    using NUnit.Framework;

    [TestFixture,Category("Unit")]
    public class WhitespaceEaterTests
    {
        [TestCase("a",Result = 0)]
        [TestCase("  a",Result = 1)]
        [TestCase("\ta",Result = 1)]
        public int ItCountsIndents(string line)
        {
            var sut = new WhitespaceEater();
            return sut.Eat(line, 1).Indents;
        }

        [TestCase("a","a",Result = 0)]
        [TestCase("  a","    b",Result = 2)]
        [TestCase("\ta","\t\tb",Result = 2)]
        [TestCase("  a","   b",ExpectedException = typeof(InconsistentWhitespaceException))]
        [TestCase(" a","\tb",ExpectedException = typeof(InconsistentWhitespaceException))]
        public int ItCountsIndentsDivibleByLearnedIndentsPerLine(string previous, string line)
        {
            var sut = new WhitespaceEater();
            sut.Eat(previous, 1);
            return sut.Eat(line, 2).Indents;
        }

        [TestCase("  ","a","a",Result = 0)]
        [TestCase("  ","  a","    b",Result = 1)]
        [TestCase("  ","    a","  b",Result = -1)]
        [TestCase("\t","\ta","\t\tb",Result = 1)]
        [TestCase("\t","\t\ta","\tb",Result = -1)]
        [TestCase("\t","\t\t\ta","\tb",Result = -2)]
        public int ItCountsDeltaIndentsDivibleByLearnedIndentsPerLine(string whitespaceTrainer,string previous, string line)
        {
            var sut = new WhitespaceEater();
            sut.Eat(whitespaceTrainer, 0);
            sut.Eat(previous, 1);
            return sut.Eat(line, 2).DeltaIndents;
        }

        [TestCase("\t","\t\tb",Result = 2)]
        [TestCase("  ","  b",Result = 1)]
        public int ItCountsOpenIndents(string whitespaceTrainer,string line)
        {
            var sut = new WhitespaceEater();
            sut.Eat(whitespaceTrainer, 0);
            sut.Eat(line, 1);
            return sut.OpenIndents;
        }

        private TestCaseData[] itTrimsWhitespace =
            {
                new TestCaseData(" a")
                    .Returns(new WhitespaceEater.TrimResult() {
                        Line = "a",
                        Trimmed = 1,
                        WhitespaceType = ' '
                    }),
                new TestCaseData("  a")
                    .Returns(new WhitespaceEater.TrimResult() {
                        Line = "a",
                        Trimmed = 2,
                        WhitespaceType = ' '
                    }),
                new TestCaseData("\ta")
                    .Returns(new WhitespaceEater.TrimResult() {
                        Line = "a",
                        Trimmed = 1,
                        WhitespaceType = '\t'
                    }),
                new TestCaseData("\t\ta")
                    .Returns(new WhitespaceEater.TrimResult() {
                        Line = "a",
                        Trimmed = 2,
                        WhitespaceType = '\t'
                    }),
                new TestCaseData("a")
                    .Returns(new WhitespaceEater.TrimResult() {
                        Line = "a",
                        Trimmed = 0,
                        WhitespaceType = 'a'
                    }),
                new TestCaseData("  ")
                    .Returns(new WhitespaceEater.TrimResult() {
                        Line = "",
                        Trimmed = 2,
                        WhitespaceType = ' '
                    }),
                new TestCaseData("\t a")
                    .Throws(typeof(InconsistentWhitespaceException))
            };

        [TestCaseSource("itTrimsWhitespace")]
        public WhitespaceEater.TrimResult ItTrimsWhitespace(string line)
        {
            var back = WhitespaceEater.TrimLeadingSpaces(line, 1);
            return back;
        }
    }
}
