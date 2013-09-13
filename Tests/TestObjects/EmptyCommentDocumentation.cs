using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.TestObjects
{
    using System.Reflection;

    using Ancestry.Daisy.Documentation;

    public class EmptyCommentDocumentation : ICommentDocumentation
    {
        public MethodDocumentation ForMethod(MethodInfo methodInfo)
        {
            return null;
        }
    }
}
