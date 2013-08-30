namespace Ancestry.Daisy.Tests.Daisy.Component.Controllers
{
    using System;
    using System.Linq;

    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Daisy.Component.Domain;

    public class UserController : StatementController<User>
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
