using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Documentor.Unit
{
    using Ancestry.Daisy.Documentation;
    using Ancestry.Daisy.Tests.Daisy.Component.Controllers;

    using Monads.NET;

    using NUnit.Framework;

    [TestFixture,Category("Unit")]
    public class CommentDocumentationTest
    {
        [TestCase(typeof(AccountController),"Type","True when the Account is of the given type.")]
        [TestCase(typeof(AccountController), "IsBalanced", "True when the Account has a non-negative balance")]
        [TestCase(typeof(AccountController), "BalanceBetween", "True when the Account has a balance between two values, exclusive.")]
        public void ItParsesSummaries(Type type, string methodName, string summary)
        {
            var docs = new CommentDocumentation("Ancestry.Daisy.Tests.XML");
            var methodDoc = docs.ForMethod(type.GetMethod(methodName));
            Assert.IsNotNull(methodDoc);
            Assert.AreEqual(summary, methodDoc.Summary);
        }

        [TestCase(typeof(AccountController), "BalanceBetween","lowerEnd", "The lowest value a balance may have")]
        [TestCase(typeof(AccountController), "BalanceBetween","higherEnd", "The highest value a balance may have")]
        public void ItParsesParameters(Type type, string methodName, string paramName, string paramDescription)
        {
            var docs = new CommentDocumentation("Ancestry.Daisy.Tests.XML");
            var paramDoc = docs.ForMethod(type.GetMethod(methodName))
                .Parameters.With(paramName);
            Assert.IsNotNull(paramDoc);
            Assert.AreEqual(paramDescription, paramDoc);
        }
    }
}
