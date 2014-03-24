namespace Ancestry.Daisy.Linking
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Language.AST;
    using Ancestry.Daisy.Language.Walks;

    using System.Linq;

    using Ancestry.Daisy.Statements;

    public class DaisyLinker
    {
        public Type RootScopeType { get; set; }

        private readonly DaisyAst ast;
        private readonly StatementSet statements;

        private IList<LinkingError> errors;

        public DaisyLinker(DaisyAst ast, StatementSet statements, Type rootType)
        {
            RootScopeType = rootType;
            this.ast = ast;
            this.statements = statements;
        }

        private class Walker : AstTreeWalker
        {
            private readonly DaisyLinker linker;
            private Stack<Type> scopes;

            public Walker(IDaisyAstNode root, DaisyLinker linker) : base(root)
            {
                this.linker = linker;
                scopes = new Stack<Type>();
                scopes.Push(linker.RootScopeType);
            }

            public void DoWalk()
            {
                Walk();
            }

            protected override void Visit(Statement node)
            {
                linker.Link(node, scopes.Peek(),false);
            }

            protected override void PostVisit(GroupOperator node)
            {
                scopes.Pop();
            }

            protected override bool PreVisit(GroupOperator node)
            {
                if(node.HasCommand)
                {
                    var nextScope = linker.Link(node, scopes.Peek(),true);
                    if (nextScope == null) return false;
                    scopes.Push(nextScope);
                }
                else
                {
                    scopes.Push(scopes.Peek());
                }
                return true;
            }
        }

        public void Link()
        {
            errors = new List<LinkingError>();
            var walker = new Walker(ast.Root, this);
            walker.DoWalk();
            if (errors.Count > 0) throw new FailedLinkException(errors);
        }

        private Type Link(Statement statement, Type scopeType, bool isGroup)
        {
            var matches = FindStatementMatches(statement.Text, scopeType).ToList();
            if(matches.Count == 0)
            {
                errors.Add(new NoLinksFoundError(statement.Text, scopeType));
                return null;
            }
            if(matches.Count > 1)
            {
                errors.Add(new MultipleLinksFoundError(statement.Text, scopeType,matches
                    .Select(x => x.Definition)
                    .ToArray()));
                return null;
            }
            var match = matches.First();
            if (isGroup && match.Definition.TransformsScopeTo == null)
            {
                errors.Add(new NoLinksPermittedError(statement.Text, scopeType,match.Definition));
            }
            statement.LinkedStatement = match;
            return isGroup ? match.Definition.TransformsScopeTo : scopeType;
        }

        private IEnumerable<ILinkedStatement> FindStatementMatches(string statement, Type scopeType)
        {
            return statements.Statements
                .Where(x => x.ScopeType.IsAssignableFrom(scopeType))
                .Select(x => x.Link(statement))
                .Where(x => x != null);
        }
    }
}
