namespace Ancestry.Daisy.Tests.Daisy.Component.Domain
{
    using System;
    using System.Collections.Generic;

    public class User
    {
        public IList<Account> Accounts { get; set; }

        public int UserId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public DateTime MemberSince { get; set; }

        public bool IsActive { get; set; }
    }
}
