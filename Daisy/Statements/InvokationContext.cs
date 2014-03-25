using Ancestry.Daisy.Program;

namespace Ancestry.Daisy.Statements
{
    using System;
    using System.Text.RegularExpressions;

    public class InvokationContext
    {
        public object Scope { get; set; }
        public Func<object,bool> Proceed { get; set; }
        public ContextBundle Context { get; set; }
        public ContextBundle Attachments { get; set; }
        public ITracer Tracer { get; set; }
    }
}