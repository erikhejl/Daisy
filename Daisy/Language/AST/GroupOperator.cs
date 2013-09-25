namespace Ancestry.Daisy.Language.AST
{
    public class GroupOperator : Statement
    {
        public IDaisyAstNode Root { get; private set; }

        public bool HasCommand { get { return !string.IsNullOrEmpty(Text); } }

        public GroupOperator(string text, IDaisyAstNode root) : base(text)
        {
            Root = root;
        }
    }
}
