using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Documentor.Component
{
    using System.IO;

    using Ancestry.Daisy.Documentation;
    using Ancestry.Daisy.Tests.Daisy.Component.Controllers;

    using NUnit.Framework;

    [TestFixture,Category("Component")]
    public class DocumentorTests
    {
        [Test]
        public void ItDocuments()
        {
            var assembly = typeof(AccountController).Assembly;
            var docPath = @"Ancestry.Daisy.Tests.XML";//Path.Combine(assembly.CodeBase,"..",@"\Ancestry.Daisy.Tests.XML");
            DaisyDocumentor.Document(assembly, docPath);
        }
    }
}
