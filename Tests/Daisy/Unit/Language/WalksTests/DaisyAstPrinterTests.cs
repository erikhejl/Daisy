namespace Ancestry.Daisy.Tests.Daisy.Unit.Language.WalksTests
{
    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Language.Walks;

    using NUnit.Framework;

    [TestFixture,Category("Unit")]
    public class DaisyAstPrinterTests
    {
        [TestCase("a\r\nAND b", Result = "AND\r\n-a\r\n-b\r\n")]
        [TestCase("a\r\nOR b", Result = "OR\r\n-a\r\n-b\r\n")]
        [TestCase("NOT a", Result = "NOT\r\n-a\r\n")]
        public string ItPrintsPrograms(string code)
        {
            var ast = DaisyParser.Parse(code);
            var p = new DaisyAstPrinter(ast.Root);
            var back = p.Print();
            Assert.AreEqual(0, p.indent);
            return back;
        }
    }
}
