using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Component.Domain
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }

        private static int TransactionIdCursor = 0;
        public static Transaction Deposit(decimal amount, DateTime timestamp)
        {
            return new Transaction
                {
                    Amount = amount,
                    Timestamp = timestamp,
                    Type = TransactionType.Deposit,
                    TransactionId = TransactionIdCursor++
                };
        }

        public static Transaction Withdrawl(decimal amount, DateTime timestamp)
        {
            return new Transaction
                {
                    Amount = -amount,
                    Timestamp = timestamp,
                    Type = TransactionType.Withdrawl,
                    TransactionId = TransactionIdCursor++
                };
        }
    }
}
