namespace Ancestry.Daisy.Language.AST
{
    public class OrOperator : IDaisyAstNode
    {
        public IDaisyAstNode Left { get; private set; }

        public IDaisyAstNode Right { get; private set; }

        public OrOperator(IDaisyAstNode left, IDaisyAstNode right)
        {
            Left = left;
            Right = right;
        }
    }
}
