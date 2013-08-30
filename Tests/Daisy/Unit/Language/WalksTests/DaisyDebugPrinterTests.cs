namespace Ancestry.Daisy.Tests.Daisy.Unit.Language.WalksTests
{
    using System;

    using Ancestry.Daisy.Language.Walks;
    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Daisy.Component.Controllers;
    using Ancestry.Daisy.Tests.Daisy.Component.Domain;

    using NUnit.Framework;

    [TestFixture]
    public class DaisyDebugPrinterTests
    {
        [Test]
        public void ItPrintsDebugInfo()
        {
            var code = Component.Statements.UserHasNoRecentTransactions;
            var data = Component.TestData.Ben;
            var program = DaisyCompiler.Compile<User>(code, new StatementSet().FromAssemblyOf(typeof(UserController)));
            var execution = program.Execute(data);
            var printer = new DaisyDebugPrinter(execution.DebugInfo);
            Console.WriteLine(printer.Print());
        }
    }
}
