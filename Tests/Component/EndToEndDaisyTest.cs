using Ancestry.Daisy.Rules;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Component
{
    [TestFixture]
    public class EndToEndDaisyTest
    {
        [Test]
        public void ItPreservesAttachments()
        {
            var code = @"Set Attachment";

            var program = DaisyCompiler.Compile<int>(code, new RuleSet().FromController(typeof(TestRuleController)));
            var result = program.Execute(4);

            Assert.AreEqual(5, result.Attachments.Test);

        }
        public class TestRuleController : RuleController<int>
        {
            public bool SetAttachment()
            {
                this.Attachments.Test = 5;
                return true;
            }
        }
    }
    
}
