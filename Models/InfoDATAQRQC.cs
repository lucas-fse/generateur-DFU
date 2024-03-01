using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GenerateurDFUSafir.Models.DAL;

namespace GenerateurDFUSafir.Models
{
    public class InfoDATAQRQC
    {
        public List<QRQC> ListQRQC { get; set; }

        public InfoDATAQRQC()
        {
            ListQRQC = new List<QRQC>();
            PEGASE_PROD2Entities2 pEGASE_PROD2Entities2 = new PEGASE_PROD2Entities2();
            ListQRQC = pEGASE_PROD2Entities2.QRQC.OrderBy(i => i.DateSuivis).ToList();
        }
    }
}