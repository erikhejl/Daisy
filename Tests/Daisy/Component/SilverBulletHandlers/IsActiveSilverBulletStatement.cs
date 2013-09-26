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

    public class IsActiveSilverBulletStatement : IStatementDefinition
    {
        public Type ScopeType { get { return typeof(User); } }

        public string Name { get { return "IsActive"; } }

        public Type TransformsScopeTo { get { return null; } }

        public ILinkedStatement Link(string statement)
        {
            if (statement != "Is Active") return null;
            return new Linked(this);
        }

        private class Linked : GenericLink
        {
            public Linked(IStatementDefinition def) : base(def) { }

            public override bool Execute(InvokationContext context)
            {
                var controller = new UserController();
                Initializer(controller, context);
                return controller.IsActive();
            }
        }
    }
}
