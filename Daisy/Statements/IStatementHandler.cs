namespace Ancestry.Daisy.Statements
{
    using System;
    using System.Text.RegularExpressions;

    public interface IStatementHandler
    {
        bool Execute(InvokationContext context);
        Type ScopeType { get; }
        Match Matches(MatchingContext matchingContext);
        string Name { get; }
        Type TransformsScopeTo { get; }
    }
}
