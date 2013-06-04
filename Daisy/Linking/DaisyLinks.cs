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
        private Dictionary<string, DaisyAggregateLink> aggregateLinks = new Dictionary<string, DaisyAggregateLink>();

        public DaisyRuleLink RuleFor(string statement)
        {
            return ruleLinks.With(statement);
        }

        public DaisyAggregateLink AggregateFor(string statement)
        {
            return aggregateLinks.With(statement);
        }

        public bool HasRuleLink(string statement)
        {
            return ruleLinks.ContainsKey(statement);
        }
        
        public void AddLink(DaisyRuleLink link)
        {
            ruleLinks.Add(link.Statement, link);
        }

        public bool HasAggregateLink(string statement)
        {
            return aggregateLinks.ContainsKey(statement);
        }
        
        public void AddLink(DaisyAggregateLink link)
        {
            aggregateLinks.Add(link.Statement, link);
        }
    }
}