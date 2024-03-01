using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAY.PegaseCore
{
    public class DescriptionE
    {
        public String Name
        {
            get; set;
        }
        public String Type
        {
            get; set;
        }
        public String AliasTexte
        {
            get; set;
        }
        public DescriptionE(String name, String type, String AliasTexte)
        {
            this.Name = name;     
            this.Type = type;
            this.AliasTexte = AliasTexte;
        }
    }
}
