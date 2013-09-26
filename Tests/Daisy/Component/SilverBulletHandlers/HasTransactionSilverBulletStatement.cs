using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Daisy.Component.SilverBulletHandlers
{
    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Daisy.Component.Controllers;
    using Ancestry.Daisy.Tests.Daisy.Component.Domain;

    public class HasTransactionSilverBulletStatement : IStatementDefinition
    {
        public Type ScopeType { get { return typeof(Account); } }

        public string Name { get { return "HasTransaction"; } }

        public Type TransformsScopeTo { get { return typeof(Transaction); } }

        public ILinkedStatement Link(string statement)
        {
            if (statement != "Has Transaction") return null;
            return new Linked(this);
        }

        private class Linked : GenericLink
        {
            public Linked(IStatementDefinition def) : base(def) { }

            public override bool Execute(InvokationContext context)
            {
                var controller = new AccountController();
                Initializer(controller, context);
                return controller.HasTransaction(context.Proceed);
            }
        }
    }
}
