using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Rules
{
    using System.Text.RegularExpressions;

    public interface IRuleHandler
    {
        bool Execute(ExecutionContext context);
        Type ScopeType { get; }
        Match Matches(MatchingContext matchingContext);
        string Name { get; }
        Type TransformsScopeTo { get; }
    }
}
