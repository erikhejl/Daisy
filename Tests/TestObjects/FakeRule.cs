﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.TestObjects
{
    using System.Text.RegularExpressions;

    using Ancestry.Daisy.Rules;

    public class FakeRule : IRuleHandler
    {
        private readonly string test;

        private readonly Func<int, bool> predicate;

        public FakeRule(string test, Func<int,bool> predicate)
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

        public bool Execute(ExecutionContext context)
        {
            return predicate((int)context.Scope);
        }

        public string Name { get; private set; }
    }

    public class FakeAggregate<F,T> : IAggregateHandler
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

        public bool Execute(ExecutionContext executionContext, Func<object, bool> proceed)
        {
            return ((IEnumerable<F>)executionContext.Scope).Any(x => proceed(x));
        }
    }
}