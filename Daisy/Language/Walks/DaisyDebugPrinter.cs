namespace Ancestry.Daisy.Language.Walks
{
    using System;
    using System.Text;
    using System.Linq;

    using Ancestry.Daisy.Language.AST;
    using Ancestry.Daisy.Program;

    public class DaisyDebugPrinter : AstTreeWalker
    {
        public ExecutionDebugInfo DebugInfo { get; private set; }

        public DaisyDebugPrinter(ExecutionDebugInfo debugInfo) : base(debugInfo.Ast.Root)
        {
            DebugInfo = debugInfo;
        }

        private StringBuilder sb;
        internal int indent = 0;

        private bool isFirstStatementInGroup;

        public string Print()
        {
            sb = new StringBuilder();
            indent = 0;
            isFirstStatementInGroup = true;
            Walk();
            return sb.ToString();
        }

        protected override void InfixVisit(AndOperator node)
        {
            Pad(sb, indent); sb.Append("AND ");
        }

        protected override void InfixVisit(OrOperator node)
        {
            Pad(sb, indent); sb.Append("OR ");
        }

        protected override bool PreVisit(NotOperator node)
        {
            sb.Append("NOT ");
            return true;
        }

        protected override void Visit(Statement node)
        {
            if(isFirstStatementInGroup)
            {
                Pad(sb, indent);
            }
            sb.Append(node.Command);
            var nodes = DebugInfo.DebugFor(node);
            sb.Append(" [");
            sb.Append(string.Join(",", nodes.Select(x => x.Result)));
            sb.Append("]");
            sb.AppendLine();
            isFirstStatementInGroup = false;
        }

        protected override bool PreVisit(GroupOperator node)
        {
            if(isFirstStatementInGroup)
            {
                Pad(sb, indent);
            }
            sb.Append("GROUP");
            if (!string.IsNullOrEmpty(node.Command))
            {
                sb.Append(" ");
                sb.Append(node.Command);
            }
            var nodes = DebugInfo.DebugFor(node);
            sb.Append(" [");
            sb.Append(string.Join(",", nodes.Select(x => x.Result)));
            sb.Append("]");

            sb.AppendLine();
            indent++;
            isFirstStatementInGroup = true;
            return true;
        }

        protected override void PostVisit(GroupOperator node)
        {
            indent--;
            isFirstStatementInGroup = false;
        }

        private static void Pad(StringBuilder sb, int pads)
        {
            for (int i = 0; i < pads; ++i)
            {
                sb.Append("-");
            }
        }
    }
}
