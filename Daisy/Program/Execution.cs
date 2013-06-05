using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Program
{
    using System.Dynamic;

    using Ancestry.Daisy.Language;

    public class Execution
    {
        public bool Result { get; internal set; }

        public ExecutionDebugInfo DebugInfo { get; private set; }

        public dynamic Context { get; private set; }

        internal Execution(DaisyAst ast, dynamic context)
        {
            DebugInfo = new ExecutionDebugInfo(ast);
            Context = context;
        }
    }
}
