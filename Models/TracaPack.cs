using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class TracaPackOPE
    {
        public long? IDOpe { get; set; }
        public List<TracaPack> tracaPacks { get; set; }


    }
    public class TracaPack
    {
        
        public string TypeGamme { get; set; }
        public long ID { get; set; }
        public string NmrOf { get; set; }
        public string NmrOrder { get; set; }
        public string Caract1 { get; set; }
        public string Caract2 { get; set; }


    }
}