using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy
{
    using System.Dynamic;

    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Language.AST;
    using Ancestry.Daisy.Language.Walks;
    using Ancestry.Daisy.Linking;
    using Ancestry.Daisy.Rules;

    public class DaisyProgram<T>
    {
        private readonly DaisyAst ast;
        private readonly DaisyLinks links;

        public dynamic Context { get; private set; }

        public DaisyProgram(DaisyAst ast, DaisyLinks links)
        {
            this.ast = ast;
            this.links = links;
            if(!links.RootScopeType.IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException("The program has been linked against a different type than this program handles.");
            }
        }

        public bool Execute(T scope)
        {
            Context = new ExpandoObject();
            return Execute(scope, ast.Root);
        }

        private bool Execute(object scope, IDaisyAstNode node)
        {
            if(node is AndOperator)
            {
                var and = node as AndOperator;
                return Execute(scope, and.Left) && Execute(scope, and.Right);
            }
            else if(node is OrOperator)
            {
                var or = node as OrOperator;
                return Execute(scope, or.Left) || Execute(scope, or.Right);
            }
            else if(node is NotOperator)
            {
                var not = node as NotOperator;
                return !Execute(scope, not.Inner);
            }
            else if(node is Statement)
            {
                var statement = node as Statement;
                var link = links.RuleFor(statement.Command, scope.GetType());
                if (link == null) throw new DaisyRuntimeException(string.Format("Expected link for '{0}', but none found", statement.Command));
                SanityCheckLink(link, scope, statement.Command);
                var result = link.Handler.Execute(new ExecutionContext() {
                        Match = link.Match,
                        Scope = scope,
                        Statement = statement.Command,
                        Proceed = o => false,  //I don't know. False since there are no children?
                        Context = Context
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
                    var result =  link.Handler.Execute(new ExecutionContext() {
                        Match = link.Match,
                        Scope = scope,
                        Statement = group.Command,
                        Proceed = o => Execute(o,group.Root),
                        Context = Context
                    });
                    return result;
                } else
                {
                    var result = Execute(scope, group.Root);
                    return result;
                }
            }
            else
            {
                throw new Exception("Don't know how to walk nodes of type: " + node.GetType());
            }
            return false;
        }

        internal static void SanityCheckLink(DaisyRuleLink link, object scope, string command)
        {
            if (link.Statement != command) throw new DaisyRuntimeException(string.Format("Bad link statement. Expected '{0}', but got '{1}'.",command,link.Statement));
            if (!link.ScopeType.IsInstanceOfType(scope)) throw new DaisyRuntimeException(string.Format("Bad link scope type. Expected {0}, but got {1}.", scope.GetType(), link.ScopeType));
        }
    }
}
