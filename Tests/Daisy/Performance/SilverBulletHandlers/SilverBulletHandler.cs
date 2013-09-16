using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Daisy.Performance
{
    using System.Reflection;
    using System.Text.RegularExpressions;

    using Ancestry.Daisy.Statements;

    public abstract class SilverBulletHandler : ReflectionStatementHandler
    {
        protected SilverBulletHandler(Type controllerType, string method) : base(controllerType.GetMethod(method), controllerType) { }

        public abstract bool Execute(InvokationContext context);
    }
}
