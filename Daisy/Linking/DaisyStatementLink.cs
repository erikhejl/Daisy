namespace Ancestry.Daisy.Linking
{
    using System;
    using System.Text.RegularExpressions;

    using Ancestry.Daisy.Statements;

    public class DaisyStatementLink 
    {
        public Match Match { get; set; }

        public string Statement { get; set; }

        public Type ScopeType { get; set; }

        public IStatementHandler Handler { get; set; }

    }
}