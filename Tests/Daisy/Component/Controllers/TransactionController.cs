namespace Ancestry.Daisy.Tests.Daisy.Component.Controllers
{
    using System;

    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Daisy.Component.Domain;

    public class TransactionController : StatementController<Transaction>
    {
        [Matches("Timestamp before (\\d+) years? ago")]
        public bool TimestampBeforeYearsAgo(int yearsAgo)
        {
            return DateTime.Now.AddYears(-yearsAgo) > Scope.Timestamp;
        }

        [Matches("Amount is greater than (\\d+)")]
        public bool AmountIsGreaterThan(int value)
        {
            return Scope.Amount > value;
        }
    }
}
