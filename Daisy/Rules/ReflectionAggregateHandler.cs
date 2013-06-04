using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Rules
{
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using Monads.NET;

    public class ReflectionAggregateHandler : BaseReflectionHandler, IAggregateHandler
    {
        public ReflectionAggregateHandler(MethodInfo methodInfo, Type controllerType) :
            base(methodInfo, controllerType)
        {
            TransformsScopeTo = StaticAnalysis.ExtractProceedFunction(methodInfo)
                .With(x => x.ParameterType)
                .With(StaticAnalysis.ExtractProceedFunctionType)
                .Recover(() => { throw new ArgumentException(string.Format("Method {0} does not have a proceed function", methodInfo.Name)); })
                ;
        }

        public bool Execute(ExecutionContext context, Func<object,bool> proceed)
        {
            var parameters = MethodInfo.GetParameters();
            if (parameters.Length - 1 != context.Match.Groups.Count - 1) //Ok. Kinda confusing. parameters is minus one since the proceed function doesn't come from the matches. 
                // The matches is minus 1 since the 0 group is gobal and doesn't count.
            {
                throw new CannotExecuteRuleException(MethodInfo,
                    string.Format("Aggregation {2} wants needs {0} parameters from statement," 
                    + " but the statement matched {1}",
                    parameters.Length, context.Match.Groups.Count - 1, Name))
                {
                    Scope = context.Scope,
                    Statement = context.Statement
                };
            }
            var inst = CreateController(context);
            var methodParams = MapParameters(context, parameters, proceed);
            return (bool)MethodInfo.Invoke(inst, methodParams);
        }

        private object[] MapParameters(ExecutionContext context, ParameterInfo[] parameters, Func<object,bool> proceed)
        {
            var objs = new List<object>();
            var ptrGroups = 1;
            foreach (var param in parameters)
            {
                var proceedFunctionType = StaticAnalysis.ExtractProceedFunctionType(param.ParameterType);
                if(proceedFunctionType != null)
                {
                    objs.Add(StaticAnalysis.ConvertPredicate(proceedFunctionType, proceed));
                }
                else
                {
                    var obj = context.Match.Groups[ptrGroups++].Captures[0].Value;
                    objs.Add(Cast(obj, param, context));
                }
            }
            return objs.ToArray();
        }

        private object FuncInvoker(Type neededType, Func<object,bool> cont)
        {
            var scopeParam = Expression.Parameter(neededType, "i");
            var forward = 
                Expression.Invoke(
                Expression.Constant(cont),
                Expression.Convert(scopeParam,typeof(object)));
            var expr = Expression.Lambda(forward, new []{scopeParam});
            return expr.Compile();
        }

        public Type TransformsScopeTo { get; private set; }
    }
}
