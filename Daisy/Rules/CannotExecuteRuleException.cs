namespace Ancestry.Daisy.Rules
{
    using System;
    using System.Reflection;

    public class CannotExecuteRuleException : Exception
    {

        public CannotExecuteRuleException(MethodInfo method,string message) : base(message)
        {
            RuleName = method.Name;
        }

        public object Scope { get; set; }

        public string Statement { get; set; }

        public string RuleName { get; set; }
    }
}