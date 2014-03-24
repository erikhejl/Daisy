using System.Diagnostics;

namespace Ancestry.Daisy.Tests.Daisy.Component.Controllers
{
    using System;
    using System.Linq;

    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Daisy.Component.Domain;

    public class AccountController : StatementController<Account>
    {
        public bool HasTransaction(Func<Transaction,bool> proceed)
        {
            return Scope.Transactions.Any(proceed);
        }

        /// <summary>
        /// True when the Account has a <strong>non-negative</strong> balance
        /// </summary>
        /// <returns></returns>
        public bool IsBalanced()
        {
            return Scope.Balance >= 0;
        }

        /// <summary>
        /// True when the Account has a balance between two values.<br/>
        /// Bounds are exclusive.
        /// </summary>
        /// <param name="lowerEnd">The lowest value a balance may have</param>
        /// <param name="higherEnd">The highest value a balance may have</param>
        [Matches(@"Balance is between (\d+) and (\d+)")]
        public bool BalanceBetween(int lowerEnd, int higherEnd)
        {
            return Scope.Balance > lowerEnd && Scope.Balance < higherEnd;
        }

        /// <summary>
        /// True when the Account is of the given type.
        /// </summary>
        /// <param name="accountType"></param>
        /// <returns></returns>
        [Matches("Type is (.*)")]
        public bool Type(string accountType)
        {
            var type = (AccountType)Enum.Parse(typeof(AccountType), accountType, true);
            return Scope.Type == type;
        }

        [Matches(@"Balance is less than (\d+\.?\d*)")]
        public bool BalanceIsLessThan(int value)
        {
            return Scope.Balance < value;
        }
    }
}
