using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Component.Domain
{
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
