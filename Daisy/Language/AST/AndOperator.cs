namespace Ancestry.Daisy.Language.AST
{
    public class AndOperator : IDaisyAstNode
    {
        public IDaisyAstNode Left { get; private set; }

        public IDaisyAstNode Right { get; private set; }

        public AndOperator(IDaisyAstNode left, IDaisyAstNode right)
        {
            Left = left;
            Right = right;
        }
    }
}
