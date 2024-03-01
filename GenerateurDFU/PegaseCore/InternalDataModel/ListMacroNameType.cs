using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAY.PegaseCore
{
    public class ListMacroNameType
    {
        string Macroname { get; set; }
        string Prototype { get; set; }
        public ListMacroNameType(string name, string prototype)
        {
            this.Macroname = name;
            this.Prototype = prototype;
        }
    }
}
