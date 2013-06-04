using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Rules
{
    using System.Text.RegularExpressions;

    [AttributeUsage(AttributeTargets.Method)]
    public class MatchesAttribute : Attribute
    {
        public MatchesAttribute(string regexp)
        {
            if (!regexp.EndsWith("$")) regexp = regexp + "$";
            if (!regexp.StartsWith("^")) regexp = "^" + regexp;
            RegularExpression = new Regex(regexp,RegexOptions.IgnoreCase);
        }

        public Regex RegularExpression { get; private set; }
    }
}
