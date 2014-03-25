using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Daisy.Component.SilverBulletHandlers
{
    using Ancestry.Daisy.Statements;

    public abstract class GenericLink : ILinkedStatement
    {
        protected GenericLink(IStatementDefinition def)
        {
            Definition = def;
        }

        public IStatementDefinition Definition { get; private set; }

        public abstract bool Execute(InvokationContext context);

        protected void Initializer<T>(StatementController<T> controller, InvokationContext context)
        {
            controller.Attachments = context.Attachments;
            controller.Context = context.Context;
            controller.Scope = (T)context.Scope;
            controller.Tracer = context.Tracer;
        }
    }
}
