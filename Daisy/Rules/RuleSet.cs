using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Rules
{
    using System.Collections;
    using System.Reflection;

    public class RuleSet
    {
        private readonly List<IRuleHandler> rules = new List<IRuleHandler>();
        private readonly List<IAggregateHandler> aggregates = new List<IAggregateHandler>();

        public RuleSet FromAssembly(Assembly assembly)
        {
            foreach (var t in assembly.GetTypes()
                .Where(x => !x.IsAbstract
                    && StaticAnalysis.IsRuleController(x)))
            {
                FromController(t);
            }
            return this;
        }

        public RuleSet Add(IRuleHandler ruleHandler)
        {
            rules.Add(ruleHandler);
            return this;
        }

        public RuleSet Add(IAggregateHandler aggregateHandler)
        {
            aggregates.Add(aggregateHandler);
            return this;
        }

        public RuleSet FromAssemblyOf(Type type)
        {
            return FromAssembly(type.Assembly);
        }

        public RuleSet FromController(Type controllerType)
        {
            var methods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            rules.AddRange(methods
                .Where(StaticAnalysis.IsRuleMethod)
                .Select(m => new ReflectionRuleHandler(m,controllerType)));
            aggregates.AddRange(methods
                .Where(StaticAnalysis.IsAggregateMethod)
                .Select(m => new ReflectionAggregateHandler(m,controllerType)));
            return this;
        }

        public IEnumerable<IRuleHandler> Rules { get { return rules; } }

        public IEnumerable<IAggregateHandler> Aggregates { get { return aggregates; } }

    }
}
