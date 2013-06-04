using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Program
{
    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Language.AST;
    using Ancestry.Daisy.Language.Walks;
    using Ancestry.Daisy.Utils;

    public class ExecutionDebugInfo
    {
        public DaisyAst Ast { get; private set; }

        private MultiMap<Statement, DebugNode> statements = new MultiMap<Statement,DebugNode>();
        private MultiMap<GroupOperator, DebugNode> groups = new MultiMap<GroupOperator,DebugNode>();

        public ExecutionDebugInfo(DaisyAst ast)
        {
            Ast = ast;
        }

        public void AttachDebugInfo(Statement statement, DebugNode node)
        {
            statements.Add(statement, node);
        }

        public void AttachDebugInfo(GroupOperator group, DebugNode node)
        {
            groups.Add(group, node);
        }

        public IEnumerable<DebugNode> DebugFor(Statement statement)
        {
            return statements[statement];
        }

        public IEnumerable<DebugNode> DebugFor(GroupOperator group)
        {
            return groups[group];
        }

        public string DebugView
        {
            get
            {
                return new DaisyDebugPrinter(this).Print();
            }
        }
    }

    public class DebugNode
    {
        public bool Result { get; set; }

        public object Scope { get; set; }

        public Type ScopeType { get; set; }
    }
}
