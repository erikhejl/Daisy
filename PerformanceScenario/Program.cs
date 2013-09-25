namespace Ancestry.Daisy.PerformanceScenario
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;

    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Daisy.Component.Controllers;
    using Ancestry.Daisy.Tests.Daisy.Component.Domain;

    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var code = Tests.Daisy.Component.Statements.UserHasNoRecentTransactions;
            var statements = new StatementSet().FromAssemblyOf<UserController>();
            var pgrm = DaisyCompiler.Compile<User>(code, statements);
            stopwatch.Stop();
            Console.WriteLine("Setup: " + stopwatch.ElapsedMilliseconds);

            stopwatch = new Stopwatch();
            var iterations = 500000;
            Console.WriteLine("Running Daisy...");
            stopwatch.Start();

            for(int i=0; i<iterations; ++i)
            {
                pgrm.Execute(Tests.Daisy.Component.TestData.Ben);
            }

            stopwatch.Stop();
            Console.WriteLine("Elapsed: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Per execution: " + ((double)stopwatch.ElapsedMilliseconds)/iterations);
            Console.ReadKey();
        }
    }
}
