namespace Ancestry.Daisy.Tests.Daisy.Component.Domain
{
    using System.Collections.Generic;

    public class Account
    {
        public int AccountId { get; set; }

        public decimal Balance { get; set; }

        public AccountType Type { get; set; }

        public IList<Transaction> Transactions { get; set; }
    }
}
