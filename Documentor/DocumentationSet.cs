namespace Ancestry.Daisy.Documentation
{
    using System.Collections.Generic;
    using System.Linq;

    using Ancestry.Daisy.Statements;

    public class DocumentationSet
    {
        private readonly StatementSet statements;
        private readonly CommentDocumentation commentDocumentation;

        public DocumentationSet(StatementSet statements, CommentDocumentation commentDocumentation)
        {
            this.statements = statements;
            this.commentDocumentation = commentDocumentation;
        }

        public IList<StatementDocumentation> Statements 
        {
            get
            {
                return statements.Statements
                    .OfType<ReflectionStatementHandler>()
                    .Select(x => StatementDocumentation.FromReflection(x,commentDocumentation))
                    .ToList()
                    ;
            }
        }
    }
}
