using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Component.Controllers
{
    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Component.Domain;

    public class AccountController : StatementController<Account>
    {
        public bool HasTransaction(Func<Transaction,bool> proceed)
        {
            return Scope.Transactions.Any(proceed);
        }

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
