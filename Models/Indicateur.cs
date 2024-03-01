using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class Indicateur
    {
        public float OTD { get; set; }
        public float OTR { get; set; }
        public int NbBL { get; set; }
        public int NbArt { get; set; }
        public float NbjourRequestBeforStandard { get; set; }
        public int NbBLRequestBeforStandard { get; set; }
        public float NbjourSendBeforeStandard { get; set; }
        public float NbBLSendBeforeStandard { get; set; }
        public int NbBlByDHL { get; set; }
        public int NbBlByDPD { get; set; }
        public int NbBlByTNT { get; set; }
        public int NbBlByUPS { get; set; }
        public int NbBlByAutre { get; set; }
    }
}