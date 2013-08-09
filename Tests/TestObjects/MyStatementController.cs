namespace Ancestry.Daisy.Tests.TestObjects
{
    using System;
    using System.Linq;

    using Ancestry.Daisy.Statements;

    public class MyStatementController : StatementController<ParentObject>
    {
        [Matches("Number is greater than \\d+")]
        public bool NumberIsGreaterThan(int greaterThan)
        {
            return Scope.Propety1 > greaterThan;
        }

        [Matches("Number is less than (\\d+)")]
        public bool NumberIsLessThan(int greaterThan)
        {
            return Scope.Propety1 > greaterThan;
        }

        public bool WhereString(Func<string,bool> applyToChildren)
        {
            return applyToChildren(Scope.Property2);
        }

        public bool HasObjects(Func<object,bool> proceed)
        {
            return Scope.Property3.Any(proceed);
        }

        public bool HasDigit(Func<int,bool> proceed)
        {
            return Scope.Propety1
                .ToString()
                .Any(x => proceed(int.Parse("" + x)));
        }
    }
}
