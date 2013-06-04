namespace Ancestry.Daisy.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Linq;

    using Monads.NET;

    public class ReflectionRuleHandler : BaseReflectionHandler, IRuleHandler
    {
        public ReflectionRuleHandler(MethodInfo methodInfo, Type controllerType) : base(methodInfo, controllerType) { }

        public bool Execute(ExecutionContext context)
        {
            var parameters = MethodInfo.GetParameters();
            if(parameters.Length != context.Match.Groups.Count-1)
            {
                throw new CannotExecuteRuleException(MethodInfo,
                    string.Format("Rule {2} wants {0} parameters, but the statement matched {1}",
                    parameters.Length, context.Match.Groups.Count-1, Name))
                    {
                        Scope = context.Scope,
                        Statement = context.Statement
                    };
            }
            var inst = CreateController(context);
            var methodParams = MapParameters(context, parameters);
            return (bool)MethodInfo.Invoke(inst, methodParams);
        }
        
        private object[] MapParameters(ExecutionContext context, ParameterInfo[] parameters)
        {
            var objs = new List<object>();
            for (int i = 1; i < context.Match.Groups.Count; ++i )
            {
                var obj = context.Match.Groups[i].Captures[0].Value;
                var param = parameters[i - 1];
                objs.Add(Cast(obj, param,context));
            }
            return objs.ToArray();
        }
    }
}