using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy
{
    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Linking;
    using Ancestry.Daisy.Program;
    using Ancestry.Daisy.Statements;

    public class DaisyCompiler
    {
        public static DaisyProgram<T> Compile<T>(string code, StatementSet statements)
        {
            var ast = DaisyParser.Parse(code);
            var linker = new DaisyLinker(ast, statements, typeof(T));
            var links = linker.Link();
            return new DaisyProgram<T>(ast, links);
        }
    }
}
