namespace Ancestry.Daisy.Linking
{
    using System;
    using System.Text.RegularExpressions;

    using Ancestry.Daisy.Rules;

    public abstract class Link
    {
        public Match Match { get; set; }

        public string Statement { get; set; }

        public Type ScopeType { get; set; }
    }

    public class DaisyRuleLink : Link
    {
        public IRuleHandler Handler { get; set; }

    }

    public class DaisyAggregateLink : Link
    {

        public IAggregateHandler Handler { get; set; }

    }
}