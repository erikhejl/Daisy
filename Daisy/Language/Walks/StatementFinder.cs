using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Language.Walks
{
    using Ancestry.Daisy.Language.AST;

    public class StatementFinder : AstTreeWalker
    {
        public StatementFinder(IDaisyAstNode root)
            : base(root)
        {
        }

        private IList<Statement> statements;

        public IList<Statement> GetStatements()
        {
            if(statements == null)
            {
                statements = new List<Statement>();
                Walk();
            }
            return statements;
        }

        protected override void Visit(Statement node)
        {
            statements.Add(node);
        }

    }
}
