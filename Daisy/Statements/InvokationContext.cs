namespace Ancestry.Daisy.Statements
{
    using System;
    using System.Text.RegularExpressions;

    public class InvokationContext
    {
        public string Statement { get; set; }
        public object Scope { get; set; }
        public Match Match { get; set; }
        public Func<object,bool> Proceed { get; set; }
        public dynamic Context { get; set; }
        public dynamic Attachments { get; set; }
    }
}