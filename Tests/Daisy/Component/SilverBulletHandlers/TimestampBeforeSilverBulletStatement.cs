using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Daisy.Component.SilverBulletHandlers
{
    using System.Text.RegularExpressions;

    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Daisy.Component.Controllers;
    using Ancestry.Daisy.Tests.Daisy.Component.Domain;

    public class TimestampBeforeSilverBulletStatement : IStatementDefinition
    {
        public Type ScopeType { get { return typeof(Transaction); } }

        public string Name { get { return "HasTransaction"; } }

        public Type TransformsScopeTo { get { return null; } }

        public ILinkedStatement Link(string statement)
        {
            var match = Regex.Match(statement, @"Timestamp before (\d*) year ago");
            if (!match.Success) return null;
            return new Linked(this, int.Parse(match.Groups[1].Value));
        }

        private class Linked : GenericLink
        {
            public int Years { get; set; }

            public Linked(IStatementDefinition def, int years) : base(def)
            {
                Years = years;
            }

            public override bool Execute(InvokationContext context)
            {
                var controller = new TransactionController();
                Initializer(controller, context);
                return controller.TimestampBeforeYearsAgo(Years);
            }
        }
    }
}
