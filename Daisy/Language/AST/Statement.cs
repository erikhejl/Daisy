namespace Ancestry.Daisy.Language.AST
{
    using Ancestry.Daisy.Statements;

    public class Statement : IDaisyAstNode
    {
        public string Text { get; private set; }

        public ILinkedStatement LinkedStatement { get; set; }

        public Statement(string statement)
        {
            Text = statement;
        }
    }
}
