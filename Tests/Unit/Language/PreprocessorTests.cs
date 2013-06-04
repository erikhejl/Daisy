namespace Ancestry.Daisy.Tests.Unit.Language
{
    using System.Text;
    using System.IO;

    using Ancestry.Daisy.Language;

    using NUnit.Framework;

    [TestFixture,Category("Unit")]
    public class PreprocessorTests
    {
        [TestCase("a", Result = "a\n", TestName = "It doesn't change prgs w/o groups")]
        [TestCase("a\na\n", Result = "a\na\n", TestName = "It doesn't change prgs w/o groups")]
        [TestCase("a\n\na\n", Result = "a\n\na\n", TestName = "It does not drop multiple intermediate lines")]
        [TestCase("a\r\na\n", Result = "a\na\n", TestName = "It normalizes line endings")]
        public string ItPreProccesses(string input)
        {
            var output = Preprocessor.Preprocess(new MemoryStream(Encoding.UTF8.GetBytes(input)));
            var str = new StreamReader(output).ReadToEnd();
            return str;
        }
    }
}
