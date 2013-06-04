namespace Ancestry.Daisy.Rules
{
    using System;
    using System.Text.RegularExpressions;

    public class ExecutionContext
    {
        public string Statement { get; set; }
        public object Scope { get; set; }
        public Match Match { get; set; }
    }
}