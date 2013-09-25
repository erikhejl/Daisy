namespace Ancestry.Daisy.Program
{
    using System;
    using System.Dynamic;

    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Language.AST;
    using Ancestry.Daisy.Linking;
    using Ancestry.Daisy.Statements;

    public class DaisyProgram<T>
    {
        private readonly DaisyAst ast;

        public DaisyProgram(DaisyAst ast)
        {
            this.ast = ast;
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
            else if(node is GroupOperator)
            {
                var group = node as GroupOperator;
                if (group.HasCommand)
                {
                    var link = group.LinkedStatement;
                    if (link == null) throw new DaisyRuntimeException(string.Format("Group '{0}' was never linked", group.Text));
                    var result =  link.Execute(new InvokationContext() {
                        Scope = scope,
                        Proceed = o => Execute(o, @group.Root, daisyExecution, context),
                        Context = context,
                        Attachments = daisyExecution.Attachments
                    });
                    daisyExecution.DebugInfo.AttachDebugInfo(group, new DebugNode() {
                        Scope = scope,
                        Result = result,
                        ScopeType = link.Definition.ScopeType
                    });
                    return result;
                } else
                {
                    var result = Execute(scope, @group.Root, daisyExecution, context);
                    return result;
                }
            }
            else if(node is Statement)
            {
                var statement = node as Statement;
                var link = statement.LinkedStatement;
                if (link == null) throw new DaisyRuntimeException(string.Format("Statement '{0}' was never linked.", statement.Text));
                var result = link.Execute(new InvokationContext() {
                        Scope = scope,
                        Proceed = o => false,  //I don't know. False since there are no children?
                        Context = context,
                        Attachments = daisyExecution.Attachments
                    });
                daisyExecution.DebugInfo.AttachDebugInfo(statement, new DebugNode() {
                        Scope = scope,
                        Result = result,
                        ScopeType = link.Definition.ScopeType
                    });
                return result;
            }
            throw new Exception("Don't know how to walk nodes of type: " + node.GetType());
        }
    }
}
