﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ancestry.Daisy.Tests {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class TestData {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal TestData() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Ancestry.Daisy.Tests.TestData", typeof(TestData).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to namespace Ancestry.Daisy.Tests.Daisy.Component.Controllers
        ///{
        ///    using System;
        ///    using System.Linq;
        ///
        ///    using Ancestry.Daisy.Statements;
        ///    using Ancestry.Daisy.Tests.Daisy.Component.Domain;
        ///
        ///    public class AccountController : StatementController&lt;Account&gt;
        ///    {
        ///        public bool HasTransaction(Func&lt;Transaction,bool&gt; proceed)
        ///        {
        ///            return Scope.Transactions.Any(proceed);
        ///        }
        ///
        ///        /// &lt;summary&gt;
        ///        /// True when the Account is of the given type.
        ///        / [rest of string was truncated]&quot;;.
        /// </summary>
        public static string AccountsController {
            get {
                return ResourceManager.GetString("AccountsController", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to t
        ///AND f
        ///OR
        ///  t
        ///  AND
        ///    f
        ///    OR f    .
        /// </summary>
        public static string Code_f {
            get {
                return ResourceManager.GetString("Code_f", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot;?&gt;
        ///&lt;doc&gt;
        ///    &lt;assembly&gt;
        ///        &lt;name&gt;Ancestry.Context&lt;/name&gt;
        ///    &lt;/assembly&gt;
        ///    &lt;members&gt;
        ///        &lt;member name=&quot;M:Ancestry.Context.Episodes.DaisyRules.EventRules.IsBirth&quot;&gt;
        ///            &lt;summary&gt;
        ///            True if the Event is a birth event
        ///            &lt;/summary&gt;
        ///            &lt;returns&gt;&lt;/returns&gt;
        ///        &lt;/member&gt;
        ///        &lt;member name=&quot;T:Ancestry.Context.ContentServices.NewspaperDataRepo&quot;&gt;
        ///            &lt;summary&gt;
        ///            Class that will check for newspaper entries on a spe [rest of string was truncated]&quot;;.
        /// </summary>
        public static string ExampleCommentsDocumentation {
            get {
                return ResourceManager.GetString("ExampleCommentsDocumentation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to t
        ///AND f
        ///OR
        ///  t
        ///  AND
        ///    f
        ///    OR t    .
        /// </summary>
        public static string TestCode1 {
            get {
                return ResourceManager.GetString("TestCode1", resourceCulture);
            }
        }
    }
}
