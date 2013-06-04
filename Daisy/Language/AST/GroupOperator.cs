namespace Ancestry.Daisy.Language.AST
{
    public class GroupOperator : IDaisyAstNode
    {
        public string Command { get; private set; }

        public IDaisyAstNode Root { get; private set; }

        public bool HasCommand { get { return !string.IsNullOrEmpty(Command); } }

        public GroupOperator(string command, IDaisyAstNode root)
        {
            Command = command;
            Root = root;
        }
    }
}
