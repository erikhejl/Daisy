namespace Ancestry.Daisy.Language
{
    using System.Collections.Generic;

    using Ancestry.Daisy.Language.AST;
    using Ancestry.Daisy.Language.Walks;

    public class DaisyAst
    {
        public DaisyAst(IDaisyAstNode root)
        {
            Root = root;
        }

        public IDaisyAstNode Root { get; private set; }

    }
}
