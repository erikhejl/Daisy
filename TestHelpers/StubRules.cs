using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.TestHelpers
{
    using Ancestry.Daisy.Rules;

    public class StubRules<T> : RuleController<T>
    {
        public bool t()
        {
            return true;
        }

        public bool f()
        {
            return true;
        }
    }

    public class StubRules : RuleController<int>
    {
        public bool odd()
        {
            return Scope % 2 == 1;
        }

        public bool even()
        {
            return !odd();
        }
    }
}
