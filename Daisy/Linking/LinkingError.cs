using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Linking
{
    using Ancestry.Daisy.Rules;

    public abstract class LinkingError
    {
        public string Statement { get; set; }

        public Type ScopeType { get; set; }
    }

    public class NoLinksFoundError : LinkingError
    {
        public NoLinksFoundError(string statement, Type scopeType)
        {
            Statement = statement;
            ScopeType = scopeType;
        }

        public override string ToString()
        {
            return string.Format("No rules linked '{0}'", Statement);
        }
    }

    public class MultipleLinksFoundError : LinkingError
    {
        public IList<IHandler> MatchedRules { get; set; }

        public MultipleLinksFoundError(string statement, Type scopeType, IList<IHandler> matchedRules)
        {
            MatchedRules = matchedRules;
            Statement = statement;
            ScopeType = scopeType;
        }

        public override string ToString()
        {
            return string.Format("Multiple rules linked to '{0}'. They are: {1}", Statement,
                string.Join(",",MatchedRules.Select(x => x.Name)));
        }
    }
}
