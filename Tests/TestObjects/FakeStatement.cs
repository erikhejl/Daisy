using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.TestObjects
{
    using System.Text.RegularExpressions;

    using Ancestry.Daisy.Statements;

    public class MoqStatement : IStatementDefinition
    {
        private readonly string test;

        private readonly Func<int, bool> predicate;

        public Type ScopeType { get; set; }

        public Match Matches(MatchingContext matchingContext)
        {
            return new Regex("^" + test + "$", RegexOptions.IgnoreCase).Match(matchingContext.Statement);
        }

        public bool Execute(InvokationContext context)
        {
            return predicate((int)context.Scope);
        }

        public string Name { get; set; }

        public Type TransformsScopeTo { get; set; }

        public ILinkedStatement Link(string statement)
        {
            return null;
        }
    }

    public class FakeStatement : IStatementDefinition
    {
        private readonly string test;

        private readonly Func<int, bool> predicate;

        public FakeStatement(string test, Func<int,bool> predicate)
        {
            this.test = test;
            this.predicate = predicate;
            Name = test;
        }

        public Type ScopeType { get { return typeof(int); } }

        public Match Matches(MatchingContext matchingContext)
        {
            return new Regex("^" + test + "$", RegexOptions.IgnoreCase).Match(matchingContext.Statement);
        }

        public string Name { get; private set; }

        public Type TransformsScopeTo { get; set; }

        public ILinkedStatement Link(string statement)
        {
            return new FakeLinkedStatement(predicate,this);
        }
    }

    public class FakeLinkedStatement : ILinkedStatement
    {
        private readonly Func<int, bool> predicate;

        public FakeLinkedStatement(Func<int, bool> predicate, IStatementDefinition def)
        {
            this.predicate = predicate;
            Definition = def;
        }

        public IStatementDefinition Definition { get; private set; }

        public bool Execute(InvokationContext context)
        {
            return predicate((int)context.Scope);
        }
    }


    public class FakeAggregate<F,T> : IStatementDefinition
    {
        private readonly string test;

        public FakeAggregate(string test)
        {
            this.test = test;
            Name = test;
            TransformsScopeTo = typeof(T);
        }

        public Type ScopeType { get { return typeof(int); } }

        public Match Matches(MatchingContext matchingContext)
        {
            return new Regex("^" + test + "$", RegexOptions.IgnoreCase).Match(matchingContext.Statement);
        }

        public string Name { get; private set; }

        public Type TransformsScopeTo { get; private set; }

        public ILinkedStatement Link(string statement)
        {
            return new FakeLinkedStatement(this);
        }

        private class FakeLinkedStatement : ILinkedStatement
        {
            private FakeAggregate<F,T> parent;

            public FakeLinkedStatement(FakeAggregate<F,T> parent)
            {
                this.parent = parent;
                Definition = parent;
            }

            public IStatementDefinition Definition { get; private set; }

            public bool Execute(InvokationContext context)
            {
                return ((IEnumerable<F>)context.Scope).Any(x => context.Proceed(x));
            }
        }
    }
}
