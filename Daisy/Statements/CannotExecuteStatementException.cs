namespace Ancestry.Daisy.Statements
{
    using System;
    using System.Reflection;

    public class CannotExecuteStatementException : Exception
    {

        public CannotExecuteStatementException(MethodInfo method,string message) : base(message)
        {
            StatementName = method.Name;
        }

        public object Scope { get; set; }

        public string Statement { get; set; }

        public string StatementName { get; set; }
    }
}