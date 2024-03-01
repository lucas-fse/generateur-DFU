using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class TRACAALLCMD
    {
        public long IndexCMD { get; set; }
        public long IndexOrifCmd { get; set; }
        public int TypeData { get; set; }
        public string SOHNUM { get; set; }
        public List<string> SOPLIN { get; set; }
        public string StatusCmd { get; set; }
        public int STATUS { get; set; }
        public string STATUSString
        {
            get;
            set;
        }
        public string ERRFICHE { get; set; }
        public string STCORIG { get; set; }
        public string STCEnCharge { get; set; }
        public int STCEnChargeInt { get; set; }
        public string TCSEnCharge { get; set; }
        public int TCSEnChargeInt { get; set; }

        public string TCSORIG { get; set; }
        public string NomClient { get; set; }
        public Nullable<System.DateTime> DateVerif { get; set; }
        public Nullable<System.DateTime> CREDAT_0 { get; set; }
        public Nullable<System.DateTime> SHIDAT { get; set; }
        public Nullable<System.DateTime> EXTDLVDAT { get; set; }
        public Nullable<System.DateTime> DEMDLVDAT_0 { get; set; }
        public Nullable<System.DateTime> ORDDAT_0 { get; set; }
        public Nullable<System.DateTime> DateEffectiveClient { get; set; }
        public Nullable<short> QTY { get; set; }

        public string ErreurString { get; set; }
        public string ErreurStringCouleur { get; set; }
        public List<int> ListTempoErreurs { get; set; }
        public string CodeCouleurLigne { get; set; }
        public DateTime test
        {
            get
            {
                if (DateEffectiveClient != null)
                {
                    return (DateTime)DateEffectiveClient;
                }
                else if (EXTDLVDAT != null)
                {
                    return (DateTime)EXTDLVDAT;
                }
                else
                {
                    return (DateTime)DateTime.Now;
                }
            }
            set
            {
                DateEffectiveClient = value;
            }
        }
        public List<string> PRODUIT { get; set; }
        public short? Statusnullable { get; set; }
    }
}
        