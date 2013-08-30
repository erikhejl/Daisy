using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Documentor.Unit.Utils
{
    using Ancestry.Daisy.Documentation.Utils;
    using Ancestry.Daisy.Tests.Daisy.Component.Controllers;

    using NUnit.Framework;

    [TestFixture,Category("Unit")]
    public class DocSignatureExtentionsTests
    {
        [TestCase(typeof(AccountController),"Type", Result = "Ancestry.Daisy.Tests.Daisy.Component.Controllers.AccountController.Type(System.String)")]
        [TestCase(typeof(AccountController), "IsBalanced", Result = "Ancestry.Daisy.Tests.Daisy.Component.Controllers.AccountController.IsBalanced")]
        [TestCase(typeof(AccountController), "BalanceBetween", Result = "Ancestry.Daisy.Tests.Daisy.Component.Controllers.AccountController.BalanceBetween(System.Int32,System.Int32)")]
        [TestCase(typeof(AccountController), "HasTransaction", Result = "Ancestry.Daisy.Tests.Daisy.Component.Controllers.AccountController.HasTransaction(System.Func{Ancestry.Daisy.Tests.Daisy.Component.Domain.Transaction,System.Boolean})")]
        public string ItGetsSignatures(Type clas,string methodName)
        {
            return clas.GetMethod(methodName).GetDocStyleSignature();
        }

        [TestCase(typeof(AccountController), Result = "Ancestry.Daisy.Tests.Daisy.Component.Controllers.AccountController")]
        [TestCase(typeof(bool), Result = "System.Boolean")]
        [TestCase(typeof(Func<bool>), Result = "System.Func{System.Boolean}")]
        public string ItGetsTypeSignatures(Type type)
        {
            return type.GetDocStyleSignature();
        }
    }
}
