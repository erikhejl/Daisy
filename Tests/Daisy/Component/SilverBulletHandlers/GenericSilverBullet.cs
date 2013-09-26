using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Daisy.Component.SilverBulletHandlers
{
    using Ancestry.Daisy.Statements;

    public abstract class GenericSilverBullet : IStatementDefinition
    {
        public abstract Type ScopeType { get; }

        public abstract string Name { get; }

        public abstract Type TransformsScopeTo { get; }

        public abstract ILinkedStatement Link(string statement);
    }
}
