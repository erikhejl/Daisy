namespace Ancestry.Daisy.Language.Walks
{
    using System;

    using Ancestry.Daisy.Language.AST;

    public abstract class AstTreeWalker
    {
        public IDaisyAstNode Root { get; private set; }

        protected AstTreeWalker(IDaisyAstNode root)
        {
            Root = root;
        }

        protected void Walk()
        {
            Walk(Root);
        }

        private void Walk(IDaisyAstNode node)
        {
            if (node is AndOperator)
            {
                var and = node as AndOperator;
                if(PreVisit(and))
                {
                    Walk(and.Left);
                    Walk(and.Right);
                    PostVisit(and);
                }
            }
            else if (node is OrOperator)
            {
                var or = node as OrOperator;
                if(PreVisit(or))
                {
                    Walk(or.Left);
                    Walk(or.Right);
                    PostVisit(or);
                }
            }
            else if (node is NotOperator)
            {
                var not = node as NotOperator;
                if(PreVisit(not))
                {
                    Walk(not.Inner);
                    PostVisit(not);
                }
            }
            else if (node is Statement)
            {
                var statement = node as Statement;
                Visit(statement);
            }
            else if (node is GroupOperator)
            {
                var group = node as GroupOperator;
                if(PreVisit(group))
                {
                    Walk(group.Root);
                    PostVisit(group);
                }
            }
            else
            {
                throw new Exception("Don't know how to walk " + node);
            }
        } 

        protected virtual bool PreVisit(AndOperator node) { return true; }
        protected virtual bool PreVisit(OrOperator node) { return true; }
        protected virtual bool PreVisit(NotOperator node) { return true; }
        protected virtual bool PreVisit(GroupOperator node) { return true; }
        protected virtual bool PreVisit(DaisyAst node) { return true; }

        protected virtual void PostVisit(AndOperator node) {}
        protected virtual void PostVisit(OrOperator node) {}
        protected virtual void PostVisit(NotOperator node) {}
        protected virtual void PostVisit(GroupOperator node) {}
        protected virtual void PostVisit(DaisyAst node) {}

        protected virtual void Visit(Statement node) {}
    }
}
