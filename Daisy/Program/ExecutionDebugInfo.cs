using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ancestry.Daisy.Language.AST.Trace;

namespace Ancestry.Daisy.Program
{
    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Language.AST;
    using Ancestry.Daisy.Language.Walks;
    using Ancestry.Daisy.Utils;

    public class ExecutionDebugInfo
    {
        public DaisyAst Ast { get; private set; }

        public ExecutionDebugInfo(DaisyAst ast)
        {
            Ast = ast;
        }

        public string DebugView
        {
            get
            {
                return new DaisyTracePrinter(Trace).Print();
            }
        }

        public TraceNode Trace { get; set; }
    }
}
