using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy
{
    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Linking;
    using Ancestry.Daisy.Rules;

    public class DaisyCompiler
    {
        public static DaisyProgram<T> Compile<T>(string code, RuleSet rules)
        {
            var ast = DaisyParser.Parse(code);
            var linker = new DaisyLinker(ast, rules, typeof(T));
            var links = linker.Link();
            return new DaisyProgram<T>(ast, links);
        }
    }
}
