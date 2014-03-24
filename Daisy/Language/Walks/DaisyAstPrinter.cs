namespace Ancestry.Daisy.Language.Walks
{
    using System;
    using System.Text;

    using Ancestry.Daisy.Language.AST;

    public class DaisyAstPrinter : AstTreeWalker
    {
        public DaisyAstPrinter(IDaisyAstNode node) : base(node) { }

        private StringBuilder sb = new StringBuilder();
        internal int indent = 0;

        public static String Print(IDaisyAstNode root)
        {
            return new DaisyAstPrinter(root).Print();
        }

        public string Print()
        {
            Walk();
            return sb.ToString();
        }

        protected override bool PreVisit(AndOperator node)
        {
            Pad(sb, indent); sb.Append("AND\r\n");
            indent++;
            return true;
        }

        protected override void PostVisit(AndOperator node)
        {
            indent--;
        }

        protected override bool PreVisit(OrOperator node)
        {
            Pad(sb, indent); sb.Append("OR\r\n");
            indent++;
            return true;
        }

        protected override void PostVisit(OrOperator node)
        {
            indent--;
        }

        protected override bool PreVisit(NotOperator node)
        {
            Pad(sb, indent); sb.Append("NOT\r\n");
            indent++;
            return true;
        }

        protected override void PostVisit(NotOperator node)
        {
            indent--;
        }

        protected override void Visit(Statement node)
        {
            Pad(sb, indent);
            sb.Append(node.Text);
            sb.Append("\r\n");
        }

        protected override bool PreVisit(GroupOperator node)
        {
            Pad(sb, indent);
            sb.Append("GROUP");
            if(!string.IsNullOrEmpty(node.Text))
            {
                sb.Append("@");
                sb.Append(node.Text);
            }
            sb.Append("\r\n");
            indent++;
            return true;
        }

        protected override void PostVisit(GroupOperator node)
        {
            indent--;
        }

        private static void Pad(StringBuilder sb, int pads)
        {
            for(int i=0; i<pads; ++i)
            {
                sb.Append("-");
            }
        }
    }
}
