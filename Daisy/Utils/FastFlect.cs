﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Utils
{
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Reflection.Emit;

    public static class FastFlect
    {
        public delegate object ObjectActivator(params object[] args);

        public static ObjectActivator GetActivator (ConstructorInfo ctor)
        {
            Type type = ctor.DeclaringType;
            ParameterInfo[] paramsInfo = ctor.GetParameters();

            //create a single param of type object[]
            ParameterExpression param =
                Expression.Parameter(typeof(object[]), "args");

            Expression[] argsExp =
                new Expression[paramsInfo.Length];

            //pick each arg from the params array 
            //and create a typed expression of them
            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp =
                    Expression.ArrayIndex(param, index);

                Expression paramCastExp =
                    Expression.Convert(paramAccessorExp, paramType);

                argsExp[i] = paramCastExp;
            }

            //make a NewExpression that calls the
            //ctor with the args we just created
            NewExpression newExp = Expression.New(ctor, argsExp);

            //create a lambda with the New
            //Expression as body and our param object[] as arg
            LambdaExpression lambda =
                Expression.Lambda(typeof(ObjectActivator), newExp, param);

            //compile it
            var compiled = (ObjectActivator)lambda.Compile();
            return compiled;
        }
    }
}
