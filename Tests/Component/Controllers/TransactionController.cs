using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Component.Controllers
{
    using Ancestry.Daisy.Rules;
    using Ancestry.Daisy.Tests.Component.Domain;

    public class TransactionController : RuleController<Transaction>
    {
        [Matches("Timestamp before (\\d+) years? ago")]
        public bool TimestampBeforeYearsAgo(int yearsAgo)
        {
            return DateTime.Now.AddYears(-yearsAgo) > Scope.Timestamp;
        }
    }
}
