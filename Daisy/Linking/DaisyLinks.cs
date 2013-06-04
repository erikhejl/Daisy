namespace Ancestry.Daisy.Linking
{
    using System;
    using System.Collections.Generic;

    using Monads.NET;

    public class DaisyLinks 
    {
        public Type RootScopeType { get; set; }

        public DaisyLinks(Type rootScopeType)
        {
            RootScopeType = rootScopeType;
        }

        private Dictionary<string, DaisyRuleLink> ruleLinks = new Dictionary<string, DaisyRuleLink>();

        public DaisyRuleLink RuleFor(string statement)
        {
            return ruleLinks.With(statement);
        }

        public bool HasRuleLink(string statement)
        {
            return ruleLinks.ContainsKey(statement);
        }
        
        public void AddLink(DaisyRuleLink link)
        {
            ruleLinks.Add(link.Statement, link);
        }
    }
}