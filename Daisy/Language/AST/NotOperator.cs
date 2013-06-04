namespace Ancestry.Daisy.Language.AST
{
    public class NotOperator : IDaisyAstNode
    {
        public IDaisyAstNode Inner { get; private set; }

        public NotOperator(IDaisyAstNode inner)
        {
            Inner = inner;
        }
    }
}
