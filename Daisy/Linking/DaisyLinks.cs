namespace Ancestry.Daisy.Linking
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Ancestry.Daisy.Utils;

    public class DaisyLinks 
    {
        public Type RootScopeType { get; set; }

        public DaisyLinks(Type rootScopeType)
        {
            RootScopeType = rootScopeType;
        }

        private Dictionary<string,List<DaisyRuleLink>> ruleLinks = new Dictionary<string,List<DaisyRuleLink>>();

        public DaisyRuleLink RuleFor(string statement,Type scopeType)
        {
            return ruleLinks.With(statement)
                .With(x => x.FirstOrDefault(y => y.ScopeType.IsAssignableFrom(scopeType)));
        }

        public bool HasRuleLink(string statement, Type scopeType)
        {
            return RuleFor(statement, scopeType) != null;
        }
        
        public void AddLink(DaisyRuleLink link)
        {
            if (RuleFor(link.Statement, link.ScopeType) != null) return;
            var list = ruleLinks.With(link.Statement);
            if(list == null)
            {
                list = new List<DaisyRuleLink>();
                ruleLinks.Add(link.Statement, list);
            }
            list.Add(link);
        }
    }
}