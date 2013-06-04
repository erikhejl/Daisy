namespace Ancestry.Daisy.Tests.TestObjects
{
    using System;
    using System.Collections.Generic;

    public class ParentObject
    {
        public LowerObject LowerObject { get; set; }

        public int Propety1 { get; set; }

        public string Property2 { get; set; }

        public IEnumerable<object> Property3 { get; set; }
    }

    public class LowerObject
    {
        public DateTime DateTime { get; set; }
    }
}
