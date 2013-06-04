using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Rules
{
    using System.Text.RegularExpressions;

    public interface IHandler
    {
        Type ScopeType { get; }
        Match Matches(MatchingContext matchingContext);
        string Name { get; }
    }

    public interface IRuleHandler : IHandler
    {
        bool Execute(ExecutionContext context);
    }

    public interface IAggregateHandler :IHandler
    {
        Type TransformsScopeTo { get; }
        bool Execute(ExecutionContext executionContext, Func<object,bool> proceed);
    }
}
