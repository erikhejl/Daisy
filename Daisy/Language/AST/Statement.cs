namespace Ancestry.Daisy.Language.AST
{
    public class Statement : IDaisyAstNode
    {
        public string Command { get; private set; }

        public Statement(string statement)
        {
            Command = statement;
        }
    }
}
