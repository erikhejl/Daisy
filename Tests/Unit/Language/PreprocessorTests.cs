namespace Ancestry.Daisy.Tests.Unit.Language
{
    using System.Text;
    using System.IO;
    using Ancestry.Daisy.Utils;

    using NUnit.Framework;
    using Ancestry.Daisy.Language.Preprocessing;

    [TestFixture,Category("Unit")]
    public class PreprocessorTests
    {
        [TestCase("a", Result = "a\n", TestName = "It doesn't change prgs w/o groups")]
        [TestCase("a\na\n", Result = "a\na\n", TestName = "It doesn't change prgs w/o groups")]
        [TestCase("a\n\na\n", Result = "a\n\na\n", TestName = "It does not drop multiple intermediate lines")]
        [TestCase("a\r\na\n", Result = "a\na\n", TestName = "It normalizes line endings")]
        public string ItPreProccesses(string input)
        {
            var output = Preprocessor.Preprocess(input.ToStream());
            var str = new StreamReader(output).ReadToEnd();
            return str;
        }

        [TestCase("start\r\n\tone tab\r\n    four spaces\r\n\t\ttwo tabs")]
        public void ItThrowsWhitespaceValidationExceptions(string input)
        {
            Assert.Throws<InconsistentWhitespaceException>(() =>
                Preprocessor.Preprocess(input.ToStream()));
        }
        [TestCase("start\r\n\tone tab\r\n\t\ttwo tabs\r\n\t\ttwo tabs")]
        public void ItDoesntThrowWhitespaceValidationExceptions(string input)
        {
            Assert.DoesNotThrow(() =>
                Preprocessor.Preprocess(input.ToStream()));
        }
        [TestCase("start\r\n\tone tab\r\n\t\ttwo tabs\r\n\t\ttwo tabs\r\n one space")]
        public void ItDoesntthrowOutOfBoundsExceptionOnLastItem(string input)
        {
            Assert.Throws<InconsistentWhitespaceException>(() =>
                Preprocessor.Preprocess(input.ToStream()));
        }
        [TestCase("start\r\n\tone tab\r\n one space")]
        public void ItReturnsTheCorrectLine(string input)
        {
            bool didThrow = false;
            try
            {
                Preprocessor.Preprocess(input.ToStream());
            }catch(InconsistentWhitespaceException e)
            {
                didThrow = true;
                Assert.AreEqual(3, e.LineNumber);
                Assert.AreEqual(" one space", e.Line);
                Assert.AreEqual(' ', e.UnexpectedWhitespace);
            }
            Assert.IsTrue(didThrow);
        }
    }
}
