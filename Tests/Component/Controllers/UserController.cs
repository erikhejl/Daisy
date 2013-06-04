using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Component.Controllers
{
    using Ancestry.Daisy.Rules;
    using Ancestry.Daisy.Tests.Component.Domain;

    public class UserController : RuleController<User>
    {
        public bool HasAccount(Func<Account,bool> procced)
        {
            return Scope.Accounts.Any(procced);
        }

        public bool AllAccounts(Func<Account,bool> procced)
        {
            return Scope.Accounts.Any(procced);
        }

        public bool IsActive()
        {
            return Scope.IsActive;
        }
    }
}
