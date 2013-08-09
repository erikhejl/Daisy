using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Unit.Language.WalksTests
{
    using Ancestry.Daisy.Language.Walks;
    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Component.Controllers;
    using Ancestry.Daisy.Tests.Component.Domain;

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
