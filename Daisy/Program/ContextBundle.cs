using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Program
{
    public class ContextBundle : Dictionary<string,object>
    {
        public T Get<T>(string key)
        {
            if (!this.ContainsKey(key)) return default(T);
            return (T)this[key];
        }
    }
}
