using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Program
{
    public interface ITracer
    {
        void Trace(string pattern, params object[] templateValues);
        List<string> Tracings { get; }
    }

    public class Tracer : ITracer
    {
        public Tracer()
        {
            Tracings = new List<string>();
        }

        public void Trace(string pattern, params object[] templateValues)
        {
            var tracing = string.Format(pattern, templateValues);
            Tracings.Add(tracing);
        }

        public List<string> Tracings { get; private set; }
    }
}
