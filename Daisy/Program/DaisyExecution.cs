using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Program
{
    using System.Dynamic;

    using Ancestry.Daisy.Language;

    public interface IDaisyExecution
    {
        bool Outcome { get; }
        ExecutionDebugInfo DebugInfo { get; }
        dynamic Attachments { get; }
    }

    public class DaisyExecution : IDaisyExecution
    {
        public bool Outcome { get; internal set; }

        public ExecutionDebugInfo DebugInfo { get; private set; }

        public dynamic Attachments { get; private set; }

        internal DaisyExecution(DaisyAst ast)
        {
            Attachments = new ExpandoObject();
            DebugInfo = new ExecutionDebugInfo(ast);
        }
    }
}
