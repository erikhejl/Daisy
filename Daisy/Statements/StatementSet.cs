namespace Ancestry.Daisy.Statements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class StatementSet
    {
        private readonly List<IStatementHandler> statements = new List<IStatementHandler>();

        public StatementSet FromAssembly(Assembly assembly)
        {
            foreach (var t in assembly.GetTypes()
                .Where(x => !x.IsAbstract
                    && StaticAnalysis.IsStatementController(x)))
            {
                FromController(t);
            }
            return this;
        }

        public StatementSet Add(IStatementHandler statementHandler)
        {
            statements.Add(statementHandler);
            return this;
        }

        public StatementSet FromAssemblyOf(Type type)
        {
            return FromAssembly(type.Assembly);
        }

        public StatementSet FromAssemblyOf<T>()
        {
            return FromAssemblyOf(typeof(T));
        }

        public StatementSet FromCurrentAssembly()
        {
            return FromAssembly(Assembly.GetCallingAssembly());
        }

        public StatementSet FromController(Type controllerType)
        {
            var methods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            statements.AddRange(methods
                .Where(StaticAnalysis.IsStatementMethod)
                .Select(m => new ReflectionStatementHandler(m,controllerType)));
            return this;
        }

        public IEnumerable<IStatementHandler> Statements { get { return statements; } }
    }
}
