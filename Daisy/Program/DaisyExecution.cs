using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Program
{
    using Ancestry.Daisy.Language;

    public interface IDaisyExecution
    {
        bool Outcome { get; }
        ExecutionDebugInfo DebugInfo { get; }
        ContextBundle Attachments { get; }
    }

    public class DaisyExecution : IDaisyExecution
    {
        public bool Outcome { get; internal set; }

        public ExecutionDebugInfo DebugInfo { get; private set; }

        public ContextBundle Attachments { get; private set; }

        internal DaisyExecution(DaisyAst ast)
        {
            Attachments = new ContextBundle();
            DebugInfo = new ExecutionDebugInfo(ast);
        }
    }
}
