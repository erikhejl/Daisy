using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Component.Domain
{
    public class Account
    {
        public int AccountId { get; set; }

        public decimal Balance { get; set; }

        public AccountType Type { get; set; }

        public IList<Transaction> Transactions { get; set; }
    }
}
