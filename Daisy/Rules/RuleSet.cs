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

        public RuleSet FromAssemblyOf(Type type)
        {
            return FromAssembly(type.Assembly);
        }

        public RuleSet FromAssemblyOf<T>()
        {
            return FromAssemblyOf(typeof(T));
        }

        public RuleSet FromController(Type controllerType)
        {
            var methods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            rules.AddRange(methods
                .Where(StaticAnalysis.IsRuleMethod)
                .Select(m => new ReflectionRuleHandler(m,controllerType)));
            return this;
        }

        public IEnumerable<IRuleHandler> Rules { get { return rules; } }
    }
}
