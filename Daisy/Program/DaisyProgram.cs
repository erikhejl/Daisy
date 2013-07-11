namespace Ancestry.Daisy.Program
{
    using System;
    using System.Dynamic;

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

        public IDaisyExecution Execute(T scope)
        {
            return Execute(scope, new ExpandoObject());
        }

        public IDaisyExecution Execute(T scope, dynamic context)
        {
            var execution = new DaisyExecution(ast);
            execution.Result = Execute(scope, ast.Root, execution, context);
            return execution;
        }

        private bool Execute(object scope, IDaisyAstNode node, DaisyExecution daisyExecution, dynamic context)
        {
            if(node is AndOperator)
            {
                var and = node as AndOperator;
                return Execute(scope, and.Left, daisyExecution, context) && Execute(scope, and.Right, daisyExecution, context);
            }
            else if(node is OrOperator)
            {
                var or = node as OrOperator;
                return Execute(scope, or.Left, daisyExecution, context) || Execute(scope, or.Right, daisyExecution, context);
            }
            else if(node is NotOperator)
            {
                var not = node as NotOperator;
                return !Execute(scope, not.Inner, daisyExecution, context);
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
                        Context = context,
                        Attachments = daisyExecution.Attachments
                    });
                daisyExecution.DebugInfo.AttachDebugInfo(statement, new DebugNode() {
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
                        Proceed = o => Execute(o, @group.Root, daisyExecution, context),
                        Context = context,
                        Attachments = daisyExecution.Attachments
                    });
                    daisyExecution.DebugInfo.AttachDebugInfo(group, new DebugNode() {
                        Scope = scope,
                        Result = result,
                        ScopeType = link.ScopeType
                    });
                    return result;
                } else
                {
                    var result = Execute(scope, @group.Root, daisyExecution, context);
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
