namespace Ancestry.Daisy.PerformanceScenario
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Dynamic;
    using System.Linq;

    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Daisy.Component.Controllers;
    using Ancestry.Daisy.Tests.Daisy.Component.Domain;
    using Ancestry.Daisy.Tests.Daisy.Component.SilverBulletHandlers;

    class Program
    {
        static void Main(string[] args)
        {
            var iterations = 5000000;
            var code = Tests.Daisy.Component.Statements.UserHasNoRecentTransactions;
            var stopwatch = new Stopwatch();

            var statements = new StatementSet()
              .Add(new HasAccountSilverBulletStatement())
              .Add(new HasTransactionSilverBulletStatement())
              .Add(new IsActiveSilverBulletStatement())
              .Add(new TimestampBeforeSilverBulletStatement())
              ;

            var pgrm = DaisyCompiler.Compile<User>(code, statements);
            Console.WriteLine("Setup: " + stopwatch.ElapsedMilliseconds);

            stopwatch = new Stopwatch();
            Console.WriteLine("Running Daisy with silver bullet...");
            stopwatch.Start();

            for(int i=0; i<iterations; ++i)
            {
                pgrm.Execute(Tests.Daisy.Component.TestData.Ben);
            }

            stopwatch.Stop();
            Console.WriteLine("Elapsed: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Per execution: " + ((double)stopwatch.ElapsedMilliseconds)/iterations);

            statements = new StatementSet()
                .FromController(typeof(UserController))
                .FromController(typeof(TransactionController))
                .FromController(typeof(AccountController))
                ;
            pgrm = DaisyCompiler.Compile<User>(code, statements);
            Console.WriteLine("Running Daisy with reflection...");
            GC.Collect();
            GC.WaitForFullGCComplete();

            stopwatch = new Stopwatch();
            stopwatch.Start();

            for(int i=0; i<iterations; ++i)
            {
                pgrm.Execute(Tests.Daisy.Component.TestData.Ben);
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Per execution: " + ((double)stopwatch.ElapsedMilliseconds)/iterations);

            GC.Collect();
            GC.WaitForFullGCComplete();

            Console.WriteLine("Running raw iterations...");
            stopwatch = new Stopwatch();
            stopwatch.Start();

            for(int i=0; i<iterations; ++i)
            {
                var data = Tests.Daisy.Component.TestData.Ben;
                var result = data.IsActive 
                    && !data.Accounts.Any(account => 
                        account.Transactions.Any(transaction => 
                            DateTime.Now.AddYears(-1) > transaction.Timestamp
                        )
                    );
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Per execution: " + ((double)stopwatch.ElapsedMilliseconds)/iterations);

            Console.WriteLine("Running iterations from handlers...");
            stopwatch = new Stopwatch();
            stopwatch.Start();
            dynamic context = new ExpandoObject();
            dynamic attachments = new ExpandoObject();

            for(int i=0; i<iterations; ++i)
            {
                var data = Tests.Daisy.Component.TestData.Ben;
                var result = new UserController() { Scope = data, Context = context, Attachments = attachments}.IsActive()
                    && !new UserController() { Scope = data, Context = context, Attachments = attachments}.HasAccount(account =>
                        new AccountController() { Scope = account, Context = context, Attachments = attachments}.HasTransaction(
                        transaction =>
                            new TransactionController() { Scope = transaction, Context = context, Attachments = attachments}.TimestampBeforeYearsAgo(1)
                        )
                    );
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Per execution: " + ((double)stopwatch.ElapsedMilliseconds)/iterations);
        }
    }
}
