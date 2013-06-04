namespace Ancestry.Daisy.Linking
{
    using System;
    using System.Text.RegularExpressions;

    using Ancestry.Daisy.Rules;

    public class DaisyRuleLink 
    {
        public Match Match { get; set; }

        public string Statement { get; set; }

        public Type ScopeType { get; set; }

        public IRuleHandler Handler { get; set; }

    }
}