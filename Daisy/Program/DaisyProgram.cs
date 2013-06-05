namespace Ancestry.Daisy.Program
{
    using System;
    using System.Dynamic;
    using System.Runtime.Remoting.Contexts;

    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Language.AST;
    using Ancestry.Daisy.Linking;
    using Ancestry.Daisy.Rules;

    public class DaisyProgram<T>
    {
        private readonly DaisyAst ast;
        private readonly DaisyLinks links;

        public DaisyProgram(DaisyAst ast, DaisyLinks links)
        {
            this.ast = ast;
            this.links = links;
            if(!links.RootScopeType.IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException("The program has been linked against a different type than this program handles.");
            }
        }

        public Execution Execute(T scope)
        {
            return Execute(scope, new ExpandoObject());
        }

        public Execution Execute(T scope, dynamic context)
        {
            var execution = new Execution(ast,context);
            execution.Result = Execute(scope, ast.Root, execution);
            return execution;
        }

        private bool Execute(object scope, IDaisyAstNode node, Execution execution)
        {
            if(node is AndOperator)
            {
                var and = node as AndOperator;
                return Execute(scope, and.Left, execution) && Execute(scope, and.Right, execution);
            }
            else if(node is OrOperator)
            {
                var or = node as OrOperator;
                return Execute(scope, or.Left, execution) || Execute(scope, or.Right, execution);
            }
            else if(node is NotOperator)
            {
                var not = node as NotOperator;
                return !Execute(scope, not.Inner, execution);
            }
            else if(node is Statement)
            {
                var statement = node as Statement;
                var link = links.RuleFor(statement.Command, scope.GetType());
                if (link == null) throw new DaisyRuntimeException(string.Format("Expected link for '{0}', but none found", statement.Command));
                SanityCheckLink(link, scope, statement.Command);
                var result = link.Handler.Execute(new InvokationContext() {
                        Match = link.Match,
                        Scope = scope,
                        Statement = statement.Command,
                        Proceed = o => false,  //I don't know. False since there are no children?
                        Context = execution.Context
                    });
                execution.DebugInfo.AttachDebugInfo(statement, new DebugNode() {
                        Scope = scope,
                        Result = result,
                        ScopeType = link.ScopeType
                    });
                return result;
            }
            else if(node is GroupOperator)
            {
                var group = node as GroupOperator;
                if (group.HasCommand)
                {
                    var link = links.RuleFor(group.Command, scope.GetType());
                    if (link == null) throw new DaisyRuntimeException(string.Format("Expected link for '{0}', but none found", group.Command));
                    SanityCheckLink(link, scope, group.Command);
                    var result =  link.Handler.Execute(new InvokationContext() {
                        Match = link.Match,
                        Scope = scope,
                        Statement = group.Command,
                        Proceed = o => Execute(o,@group.Root, execution),
                        Context = execution.Context
                    });
                    execution.DebugInfo.AttachDebugInfo(group, new DebugNode() {
                        Scope = scope,
                        Result = result,
                        ScopeType = link.ScopeType
                    });
                    return result;
                } else
                {
                    var result = Execute(scope, @group.Root, execution);
                    return result;
                }
            }
            throw new Exception("Don't know how to walk nodes of type: " + node.GetType());
        }

        internal static void SanityCheckLink(DaisyRuleLink link, object scope, string command)
        {
            if (link.Statement != command) throw new DaisyRuntimeException(string.Format("Bad link statement. Expected '{0}', but got '{1}'.",command,link.Statement));
            if (!link.ScopeType.IsInstanceOfType(scope)) throw new DaisyRuntimeException(string.Format("Bad link scope type. Expected {0}, but got {1}.", scope.GetType(), link.ScopeType));
        }
    }
}
