using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Daisy.Performance
{
    using System.Diagnostics;

    using Ancestry.Daisy.Statements;

    using NUnit.Framework;

    [TestFixture,Category("Performance")]
    public class ReflectionStatementHandlerTest
    {
        private class MyController : StatementController<string>
        {
            public bool MyStatement() { return true; }
        }

        [Test]
        [Ignore]
        public void ItCreatesControllerQuickly()
        {
            var type = typeof(MyController);
            var method = type.GetMethod("MyStatement");
            var refHandler = new ReflectionStatementHandler(method, type);
            var context = new InvokationContext() {
                Scope = "The Scope",
                Context = "The Context",
                Attachments = "The Attachments",
                Statement = "The Statement"
            };
            const int iterations = 1000000;

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < iterations; ++i)
            {
                var obj = refHandler.CreateController();
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Per invokation: " + ((double)stopwatch.ElapsedMilliseconds)/iterations);
            Assert.Less(stopwatch.ElapsedMilliseconds, 200);
        }

        [Test]
        [Ignore]
        public void ItInitializesControllerQuickly()
        {
            var type = typeof(MyController);
            var method = type.GetMethod("MyStatement");
            var refHandler = new ReflectionStatementHandler(method, type);
            var context = new InvokationContext() {
                Scope = "The Scope",
                Context = "The Context",
                Attachments = "The Attachments",
                Statement = "The Statement"
            };
            var obj = refHandler.CreateController();
            const int iterations = 1000000;

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < iterations; ++i)
            {
                refHandler.InitializeController(obj, context);
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Per initialization: " + ((double)stopwatch.ElapsedMilliseconds)/iterations);
            Assert.Less(stopwatch.ElapsedMilliseconds, 500);
        }

        [Test]
        public void ItInvokedMethodsQuickly()
        {
            var type = typeof(MyController);
            var method = type.GetMethod("MyStatement");
            var refHandler = new ReflectionStatementHandler(method, type);
            var inst = new MyController() {
                Scope = "The Scope",
                Context = "The Context",
                Attachments = "The Attachments"
            };
            const int iterations = 1000000;
            var parameters = new object[0];

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < iterations; ++i)
            {
                refHandler.Execute(inst,parameters);
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Per invokation: " + ((double)stopwatch.ElapsedMilliseconds)/iterations);
            Assert.Less(stopwatch.ElapsedMilliseconds, 200);
        }
    }
}
