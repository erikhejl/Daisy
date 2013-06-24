namespace Ancestry.Daisy.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Linq;

    using Ancestry.Daisy.Utils;

    public class ReflectionRuleHandler : IRuleHandler
    {
        public MethodInfo MethodInfo { get; set; }

        public Type ControllerType { get; set; }

        public Regex MatchingCriteria { get; private set; }

        public string Name { get { return MethodInfo.Name; } }

        public ReflectionRuleHandler(MethodInfo methodInfo, Type controllerType)
        {
            MethodInfo = methodInfo;
            ControllerType = controllerType;
            MatchingCriteria = GetMatchingCriteria();
            ScopeType = controllerType.GetProperty("Scope").PropertyType;
            TransformsScopeTo = StaticAnalysis.ExtractProceedFunction(methodInfo)
              .With(x => x.ParameterType)
              .With(StaticAnalysis.ExtractProceedFunctionType);
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
            return new Regex("^"+NormalizeMethodName(name)+"$",RegexOptions.IgnoreCase);
        }

        internal static string NormalizeMethodName(string name)
        {
            var parts = StringExtentions.SplitNameParts(name);
            var spacings = string.Join("\\s+", parts);
            return spacings;
        }

        protected object CreateController(InvokationContext invokationContext)
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
                        Scope = invokationContext.Scope,
                        Statement = invokationContext.Statement
                    };
            }
            controller.GetType().GetProperty("Scope").SetValue(controller, invokationContext.Scope);
            controller.GetType().GetProperty("Context").SetValue(controller, invokationContext.Context);
            controller.GetType().GetProperty("Attachments").SetValue(controller, invokationContext.Attachments);
            return controller;
        }

        protected object Cast(string obj, ParameterInfo param, InvokationContext invokationContext)
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
                        Scope = invokationContext.Scope,
                        Statement = invokationContext.Statement
                    };
            }
        }

        public bool Execute(InvokationContext context)
        {
            var parameters = MethodInfo.GetParameters();
            var inst = CreateController(context);
            var methodParams = MapParameters(context, parameters);
            return (bool)MethodInfo.Invoke(inst, methodParams);
        }

        public Type TransformsScopeTo { get; private set; }

        private object[] MapParameters(InvokationContext context, ParameterInfo[] parameters)
        {
            var objs = new List<object>();
            var ptrGroups = 1;
            foreach (var param in parameters)
            {
                var proceedFunctionType = StaticAnalysis.ExtractProceedFunctionType(param.ParameterType);
                if (proceedFunctionType != null)
                {
                    objs.Add(StaticAnalysis.ConvertPredicate(proceedFunctionType, context.Proceed));
                }
                else
                {
                    if(ptrGroups>=context.Match.Groups.Count)
                    {
                        throw new CannotExecuteRuleException(
                            MethodInfo,
                            string.Format("Rule {1} has more parameters than captures available to it." 
                            + "{0} are available.",
                            context.Match.Groups.Count -1, Name));
                    }
                    var obj = context.Match.Groups[ptrGroups++].Captures[0].Value;
                    objs.Add(Cast(obj, param, context));
                }
            }
            if(objs.Count != parameters.Length)
            {
                throw new CannotExecuteRuleException(
                    MethodInfo,
                    string.Format("Rule {2} wants {0} parameters from statement," 
                    + " but only {1} could be matched.",
                    parameters.Length, objs.Count, Name));
            }
            if(ptrGroups < context.Match.Groups.Count)
            {
                throw new CannotExecuteRuleException(
                    MethodInfo,
                    string.Format("Rule {2} did not use all captures available to it." 
                    + "It used {0} but {1} are available.",
                    ptrGroups-1, context.Match.Groups.Count -1, Name));
            }
            return objs.ToArray();
        }
    }
}