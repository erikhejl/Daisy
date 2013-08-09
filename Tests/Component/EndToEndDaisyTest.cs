using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Component
{
    using Ancestry.Daisy.Statements;

    [TestFixture]
    public class EndToEndDaisyTest
    {
        [Test]
        public void ItPreservesAttachments()
        {
            var code = @"Set Attachment";

            var program = DaisyCompiler.Compile<int>(code, new StatementSet().FromController(typeof(TestStatementController)));
            var result = program.Execute(4);

            Assert.AreEqual(5, result.Attachments.Test);

        }
        public class TestStatementController : StatementController<int>
        {
            public bool SetAttachment()
            {
                this.Attachments.Test = 5;
                return true;
            }
        }
    }
    
}
