using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Rules
{
    using System.Reflection;
    using System.Text.RegularExpressions;

    using Monads.NET;

    public abstract class BaseReflectionHandler
    {
         public MethodInfo MethodInfo { get; set; }

        public Type ControllerType { get; set; }

        public Regex MatchingCriteria { get; private set; }

        public string Name { get { return MethodInfo.Name; } }

        protected BaseReflectionHandler(MethodInfo methodInfo, Type controllerType)
        {
            MethodInfo = methodInfo;
            ControllerType = controllerType;
            MatchingCriteria = GetMatchingCriteria();
            ScopeType = controllerType.GetProperty("Scope").PropertyType;
        }

        public Type ScopeType { get; private set; }

        public Match Matches(MatchingContext matchingContext)
        {
            return MatchingCriteria.Match(matchingContext.Statement);
        }

        public Regex GetMatchingCriteria()
        {
            var attr = MethodInfo.GetCustomAttributes<MatchesAttribute>().FirstOrDefault();
            return attr.With(x => x.RegularExpression).Recover(() => MethodNameToRegex(MethodInfo.Name));
        }

        internal static Regex MethodNameToRegex(string name)
        {
            return new Regex(NormalizeMethodName(name),RegexOptions.IgnoreCase);
        }

        internal static string NormalizeMethodName(string name)
        {
            var parts = StringExtentions.SplitNameParts(name);
            var spacings = string.Join("\\s+", parts);
            return spacings;
        }

        protected object CreateController(ExecutionContext executionContext)
        {
            object controller;
            try
            {
                controller = Activator.CreateInstance(ControllerType);
            }
            catch
            {
                throw new CannotExecuteRuleException(MethodInfo,
                    string.Format("Cannot build rule {0} because it's controller could not be constructed.",
                        MethodInfo.Name))
                    {
                        Scope = executionContext.Scope,
                        Statement = executionContext.Statement
                    };
            }
            controller.GetType().GetProperty("Scope").SetValue(controller, executionContext.Scope);
            return controller;
        }

        protected object Cast(string obj, ParameterInfo param, ExecutionContext executionContext)
        {
            try
            {
                return Convert.ChangeType(obj, param.ParameterType);
            }
            catch(FormatException e)
            {
                throw new CannotExecuteRuleException(MethodInfo,
                    string.Format("Cannot convert '{0}' into {1}, to match parameter '{2}'",
                    obj, param.ParameterType, param.Name))
                    {
                        Scope = executionContext.Scope,
                        Statement = executionContext.Statement
                    };
            }
        }
    }
}
