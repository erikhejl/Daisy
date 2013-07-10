namespace Ancestry.Daisy.TestHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using Ancestry.Daisy.Rules;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ExecutionContext = System.Threading.ExecutionContext;

    public class TestHelper
    {

        public static bool Matches(Type controller, string methodName, string statement)
        {
            var method = GetMethod(controller, methodName);
            var ruleHandler = new ReflectionRuleHandler(method, controller);
            return ruleHandler.Matches(new MatchingContext() {
                    Statement = statement,
                }).Success;
        }

        private static MethodInfo GetMethod(Type controller, string methodName)
        {
            var method = controller.GetMethod(methodName);
            if(method == null) throw new ArgumentException(string.Format("Cannot find method {0} on {1}", methodName, controller.GetType().Name));
            return method;
        }

        public static InvokationResult Invoke<T>(Type controller, string methodName, T scope,
            string statement, dynamic context = null, Func<object, bool> proceed = null)
        {
            if(proceed == null)
                proceed = o => false;
            context = context ?? new ExpandoObject();
            var method = GetMethod(controller, methodName);
            var ruleHandler = new ReflectionRuleHandler(method, controller);
            var invokationResult = new InvokationResult()
                {
                    Context = context,
                    Statement = statement,
                    MatchingCriteria = ruleHandler.GetMatchingCriteria(),
                    Attachments = new ExpandoObject()
                };
            invokationResult.Match = ruleHandler.Matches(new MatchingContext() {
                    Statement = statement,
                    ScopeType = typeof(T)
                });
            if (!invokationResult.Matched) return invokationResult;
            invokationResult.Result = ruleHandler.Execute(new InvokationContext() {
                Statement = statement,
                Context = invokationResult.Context,
                Attachments = invokationResult.Attachments,
                Scope = scope,
                Match = invokationResult.Match,
                Proceed = proceed
            });
            return invokationResult;
        }

        public class InvokationResult
        {
            public bool Matched { get { return Match != null && Match.Success; } }

            public Match Match { get; set; }

            public dynamic Context { get; set; }

            public dynamic Attachments { get; set; }

            public bool Result { get; set; }

            public string Statement { get; set; }

            public Regex MatchingCriteria { get; set; }

            public InvokationResult AssertMatched()
            {
                Assert.IsTrue(Matched,string.Format("Expected {0} to match {1}.", Statement, MatchingCriteria));
                return this;
            }

            public InvokationResult AssertMatched(bool matched)
            {
                if (matched) return AssertMatched();
                else return AssertNotMatched();
            }

            public InvokationResult AssertNotMatched()
            {
                Assert.IsFalse(Matched,string.Format("Expected {0} to not match {1}, but it did.", Statement, MatchingCriteria));
                return this;
            }

            public InvokationResult AssertResult(bool expected)
            {
                Assert.AreEqual(expected,Result,string.Format("Expected result to be {0}, but was {1}", expected, Result));
                return this;
            }

            public InvokationResult AssertContextHas(string property)
            {
                if (!((IDictionary<String, object>)Context).ContainsKey(property))
                    Assert.Fail("Expected Context.{0} to be set, but was not", property);
                return this;
            }
        }
    }
}
