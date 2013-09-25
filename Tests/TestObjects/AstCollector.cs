using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.TestObjects
{
    using System.Collections;

    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Language.AST;

    public class AstCollector : IEnumerable<IDaisyAstNode>
    {
        private readonly DaisyAst ast;

        public AstCollector(DaisyAst ast)
        {
            this.ast = ast;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<IDaisyAstNode> GetEnumerator()
        {
            return Collect().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<IDaisyAstNode> Collect()
        {
            var queue = new Stack<IDaisyAstNode>();
            queue.Push(ast.Root);
            while (queue.Count != 0)
            {
                var node = queue.Pop();
                yield return node;
                if (node is AndOperator)
                {
                    var and = node as AndOperator;
                    queue.Push(and.Right);
                    queue.Push(and.Left);
                }
                else if (node is OrOperator)
                {
                    var and = node as OrOperator;
                    queue.Push(and.Right);
                    queue.Push(and.Left);
                }
                else if (node is NotOperator)
                {
                    var and = node as NotOperator;
                    queue.Push(and.Inner);
                }
                else if (node is GroupOperator)
                {
                    var and = node as GroupOperator;
                    queue.Push(and.Root);
                }
            }
        }
    }
}
