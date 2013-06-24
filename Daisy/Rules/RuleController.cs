namespace Ancestry.Daisy.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class RuleController<T> 
    {
        public T Scope { get; set; }

        public dynamic Context { get; set; }
        public dynamic Attachments { get; set; }
    }
}
