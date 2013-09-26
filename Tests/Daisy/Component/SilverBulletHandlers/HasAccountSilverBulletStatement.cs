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

    public class HasAccountSilverBulletStatement : IStatementDefinition
    {
        public Type ScopeType { get { return typeof(User); } }

        public string Name { get { return "IsActive"; } }

        public Type TransformsScopeTo { get { return typeof(Account); } }

        public ILinkedStatement Link(string statement)
        {
            if (statement != "Has Account") return null;
            return new Linked(this);
        }

        private class Linked : GenericLink
        {
            public Linked(IStatementDefinition def) : base(def) { }

            public override bool Execute(InvokationContext context)
            {
                var controller = new UserController();
                Initializer(controller, context);
                return controller.HasAccount(context.Proceed);
            }
        }
    }
}
