using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Statements
{
    public class StatementParameter
    {
        public Type Type { get; set; }

        public string Name { get; set; }
    }

    public class ProceedParameter : StatementParameter
    {
        public Type TransformsTo { get; set; }
    }
}
