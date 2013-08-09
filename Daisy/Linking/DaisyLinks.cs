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

        private Dictionary<string,List<DaisyStatementLink>> statementLinks = new Dictionary<string,List<DaisyStatementLink>>();

        public DaisyStatementLink StatementFor(string statement,Type scopeType)
        {
            return statementLinks.With(statement)
                .With(x => x.FirstOrDefault(y => y.ScopeType.IsAssignableFrom(scopeType)));
        }

        public bool HasStatementLink(string statement, Type scopeType)
        {
            return StatementFor(statement, scopeType) != null;
        }
        
        public void AddLink(DaisyStatementLink link)
        {
            if (StatementFor(link.Statement, link.ScopeType) != null) return;
            var list = statementLinks.With(link.Statement);
            if(list == null)
            {
                list = new List<DaisyStatementLink>();
                statementLinks.Add(link.Statement, list);
            }
            list.Add(link);
        }
    }
}