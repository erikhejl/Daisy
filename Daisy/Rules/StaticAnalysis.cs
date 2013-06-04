using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Rules
{
    using System.Linq.Expressions;
    using System.Reflection;

    public static class StaticAnalysis
    {
        public class ProceedFunction
        {
            public Type ChildScopeType { get; set; }
        }

        public static bool IsRuleController(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(RuleController<>)) return true;
            return type.BaseType != null && IsRuleController(type.BaseType);
        }

        public static bool IsRuleMethod(MethodInfo m)
        {
            return m.IsPublic
                && m.ReturnType == typeof(bool) && m.DeclaringType != typeof(object);
        }

        public static bool IsAggregateMethod(MethodInfo m)
        {
            return m.IsPublic && ExtractProceedFunction(m) != null;
        }

        public static bool IsProceedFunction(Type func)
        {
            return ExtractProceedFunctionType(func) != null;
        }

        public static Type ExtractProceedFunctionType(Type func)
        {
            if (func.BaseType != typeof(MulticastDelegate)) return null;
            if (!func.IsGenericType) return null;
            var gs = func.GenericTypeArguments;
            if (gs.Length != 2) return null;
            if (gs[1] != typeof(bool)) return null;
            return gs[0];
        }

        public static ParameterInfo ExtractProceedFunction(MethodInfo m)
        {
            return m.GetParameters()
                .LastOrDefault(p => IsProceedFunction(p.ParameterType))
                ;
        }

        public static object ConvertPredicate(Type destType, Func<object,bool> predicate)
        {
            var scopeParam = Expression.Parameter(destType, "i");
            var forward = 
                Expression.Invoke(
                Expression.Constant(predicate),
                Expression.Convert(scopeParam,typeof(object)));
            var expr = Expression.Lambda(forward, new []{scopeParam});
            return expr.Compile();
        }
    }
}
