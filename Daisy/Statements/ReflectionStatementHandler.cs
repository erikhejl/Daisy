namespace Ancestry.Daisy.Statements
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Linq;

    using Ancestry.Daisy.Utils;

    using Fasterflect;

    public class ReflectionStatementHandler : IStatementHandler
    {
        private FastFlect.ObjectActivator activator;

        private MemberSetter delegateForAttachmentsSetter;

        private MemberSetter delegateForContextSetter;

        private MemberSetter delegateForScopeSetter;

        private MethodInvoker delegateForInvokation;

        private object controllerInstance;

        private StaticAnalysis.TransformPredicate scopeConverter;

        public MethodInfo MethodInfo { get; private set; }

        public Type ControllerType { get; private set; }

        public Regex MatchingCriteria { get; private set; }

        public string Name { get { return MethodInfo.Name; } }

        public ReflectionStatementHandler(MethodInfo methodInfo, Type controllerType)
        {
            MethodInfo = methodInfo;
            ControllerType = controllerType;
            MatchingCriteria = GetMatchingCriteria();
            ScopeType = controllerType.GetProperty("Scope").PropertyType;
            IsolateParameters(methodInfo);
            PrecacheReflections(controllerType);
        }

        private void PrecacheReflections(Type controllerType)
        {
            var defaultConstructor = controllerType.GetConstructors().FirstOrDefault(x => x.GetParameters().Length == 0);
            if (defaultConstructor == null) throw new ArgumentException("StatementHandler " + controllerType.Name + " does not have a default constructor");
            activator = FastFlect.GetActivator(defaultConstructor);
            delegateForScopeSetter = controllerType.DelegateForSetPropertyValue("Scope");
            delegateForContextSetter = controllerType.DelegateForSetPropertyValue("Context");
            delegateForAttachmentsSetter = controllerType.DelegateForSetPropertyValue("Attachments");
            delegateForInvokation = MethodInfo.DelegateForCallMethod();
            controllerInstance = CreateController();
            if (TransformsScopeTo != null) scopeConverter = StaticAnalysis.CreateConverter(TransformsScopeTo);
        }

        public IList<StatementParameter> Parameters { get; private set; }

        public Type ScopeType { get; private set; }

        public Match Matches(MatchingContext matchingContext)
        {
            return MatchingCriteria.Match(matchingContext.Statement);
        }

        public Regex GetMatchingCriteria()
        {
            var attr = MethodInfo
                .GetCustomAttributes<MatchesAttribute>()
                .FirstOrDefault();
            return attr.With(x => x.RegularExpression).Recover(() => MethodNameToRegex(MethodInfo.Name));
        }

        internal static Regex MethodNameToRegex(string name)
        {
            return new Regex("^"+NormalizeMethodName(name)+"$",RegexOptions.IgnoreCase);
        }

        internal static string NormalizeMethodName(string name)
        {
            var parts = StringExtentions.SplitNameParts(name);
            var spacings = string.Join("\\s+", parts);
            return spacings;
        }

        internal protected object CreateController()
        {
            object controller;
            try
            {
                controller = activator();
            }
            catch
            {
                throw new CannotExecuteStatementException(MethodInfo,
                    string.Format("Cannot build statement {0} because it's controller could not be constructed.",
                        MethodInfo.Name));
            }
            return controller;
        }

        internal protected void InitializeController(object controller ,InvokationContext invokationContext)
        {
            delegateForScopeSetter(controller, invokationContext.Scope);
            delegateForContextSetter(controller, invokationContext.Context);
            delegateForAttachmentsSetter(controller, invokationContext.Attachments);

            /*
            ControllerType.GetProperty("Scope").SetValue(controller,invokationContext.Scope);
            ControllerType.GetProperty("Context").SetValue(controller,invokationContext.Context);
            ControllerType.GetProperty("Attachments").SetValue(controller,invokationContext.Attachments);
            */
        }

        protected object Cast(string obj, StatementParameter param, InvokationContext invokationContext)
        {
            try
            {
                return Convert.ChangeType(obj, param.Type);
            }
            catch(FormatException e)
            {
                throw new CannotExecuteStatementException(MethodInfo,
                    string.Format("Cannot convert '{0}' into {1}, to match parameter '{2}'",
                    obj, param.Type, param.Name))
                    {
                        Scope = invokationContext.Scope,
                        Statement = invokationContext.Statement
                    };
            }
        }

        public bool Execute(InvokationContext context)
        {
            var parameters = MethodInfo.GetParameters();
            var inst = controllerInstance; // CreateController(context);
            InitializeController(inst,context);
            var methodParams = MapParameters(context, parameters);
            return Execute(inst, methodParams);
        }

        internal bool Execute(object inst, object[] methodParams)
        {
            //return (bool)MethodInfo.Invoke(inst, methodParams);
            return (bool)delegateForInvokation(inst, methodParams);
        }

        public Type TransformsScopeTo
        {
            get
            {
                return Parameters.OfType<ProceedParameter>().Select(x => x.TransformsTo).FirstOrDefault();
            }
        }

        private void IsolateParameters(MethodInfo methodInfo)
        {
            Parameters = methodInfo.GetParameters()
                .Select(p => {
                    var proceedFunctionType = StaticAnalysis.ExtractProceedFunctionType(p.ParameterType);
                    if (proceedFunctionType != null)
                    {
                        return new ProceedParameter() {
                            Name = p.Name,
                            Type = p.ParameterType,
                            TransformsTo = proceedFunctionType
                        };
                    }
                    else
                    {
                        return new StatementParameter() { Name = p.Name, Type = p.ParameterType };
                    }
                })
                .ToList();
        }

        private object[] MapParameters(InvokationContext context, ParameterInfo[] parameters)
        {
            var objs = new List<object>();
            var ptrGroups = 1;
            foreach (var param in Parameters)
            {
                if (param is ProceedParameter)
                {
                    objs.Add(scopeConverter(context.Proceed));  //StaticAnalysis.CreateConverter(((ProceedParameter)param).TransformsTo, context.Proceed));
                }
                else
                {
                    if(ptrGroups>=context.Match.Groups.Count)
                    {
                        throw new CannotExecuteStatementException(
                            MethodInfo,
                            string.Format("Statement {1} has more parameters than captures available to it." 
                            + "{0} are available.",
                            context.Match.Groups.Count -1, Name));
                    }
                    var obj = context.Match
                        .Groups[ptrGroups++]
                        .Captures
                        .If(x => x.Count > 0)
                        .With(x => x[0].Value);
                    objs.Add(Cast(obj, param, context));
                }
            }
            if(objs.Count != parameters.Length)
            {
                throw new CannotExecuteStatementException(
                    MethodInfo,
                    string.Format("Statement {2} wants {0} parameters from statement," 
                    + " but only {1} could be matched.",
                    parameters.Length, objs.Count, Name));
            }
            if(ptrGroups < context.Match.Groups.Count)
            {
                throw new CannotExecuteStatementException(
                    MethodInfo,
                    string.Format("Statement {2} did not use all captures available to it." 
                    + "It used {0} but {1} are available.",
                    ptrGroups-1, context.Match.Groups.Count -1, Name));
            }
            return objs.ToArray();
        }
    }
}