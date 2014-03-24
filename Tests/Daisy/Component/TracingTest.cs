﻿using System;
using System.Linq;
using Ancestry.Daisy.Language.AST;
using Ancestry.Daisy.Language.AST.Trace;
using Ancestry.Daisy.Language.Walks;
using Ancestry.Daisy.Linking;
using Ancestry.Daisy.Tests.Daisy.Component.Controllers;
using Ancestry.Daisy.Tests.Daisy.Component.Domain;
using Moq;

namespace Ancestry.Daisy.Tests.Daisy.Component
{
    using NUnit.Framework;

    using Ancestry.Daisy.Statements;

    [TestFixture]
    public class TracingTest
    {
        [TestCase("Is Active", typeof(StatementTrace))]
        [TestCase("NOT is Active", typeof(NotOperatorTrace))]
        [TestCase("Has Account\r\n  Is Balanced", typeof(GroupOperatorTrace))]
        public void ItTracesToIdentityNodes(string code, Type traceType)
        {
            Assert.IsInstanceOf(traceType,Trace(code));
        }

        [Test]
        public void ItTracesAcrossGroups()
        {
            var code = "Has account\r\n  Is Balanced\r\n  Type is Savings";
            var trace = Trace(code);
            var ast = new DaisyTracePrinter(trace);
            var printed = ast.Print();
            Console.WriteLine(printed);
            Assert.IsInstanceOf<GroupOperatorTrace>(trace);
            var groupTrace = (GroupOperatorTrace) trace;
            Assert.AreEqual(3, groupTrace.Frames.Count);
        }

        [Test]
        public void ItCollectsTracings()
        {
            var code = "Has account\r\n  Is Balanced";
            var trace = Trace(code);
            var ast = new DaisyTracePrinter(trace);
            var printed = ast.Print();
            Console.WriteLine(printed);
            Assert.IsInstanceOf<GroupOperatorTrace>(trace);
            var groupTrace = (GroupOperatorTrace) trace;
            Assert.AreEqual(1,groupTrace.Tracings.Count);
            Assert.AreEqual("Has 3 accounts",groupTrace.Tracings.First());
        }

        public TraceNode Trace(string code)
        {
            var statements = new StatementSet().FromAssemblyOf(typeof(UserController));

            var program = DaisyCompiler.Compile<User>(code, statements);
            var result = program.Execute(TestData.Ben);

            Assert.IsNotNull(result.DebugInfo.Trace);
            return result.DebugInfo.Trace;
        }
    }

}
