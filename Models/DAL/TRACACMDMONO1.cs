using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models.DAL
{
    public partial class TRACACMDMONO
    {
        public string ReturnDateClotureForDisplay
        {
            get
            {
                if (this.DateCloture == null)
                {
                    return "Non cloturée";
                }
                else
                {
                    return ((DateTime)DateCloture).ToString("dd/MM/yyyy");
                }
            }
        }

        //public int StatusFps { get; set; }
        public int DefaultSTATUS1 { get; set; }
        public int DelaiMono1 { get; set; }
        public int DefaultDelaiMono1
        {
            get;
            set;
        }
        public string DelaiMonosString
        {
            get;
            set;
        }
        public string STATUSString
        {
            get;
            set;
        }
        public DateTime test
        {
            get
            {
                if (DateEffectiveClient != null)
                {
                    return (DateTime)DateEffectiveClient;
                }
                else if (EXTDLVDAT_0 != null)
                {
                    return (DateTime)EXTDLVDAT_0;
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
        public string DateClotureString
        {
            get
            {

                if (DateCloture != null)
                    return ((DateTime)DateCloture).ToString("dd/MM/yyyy");
                else
                {
                    return "*";
                }
            }
        }
        public string ErreurString { get; set; }
        public string ErreurStringCouleur { get; set; }
        public List<int> ListTempoErreurs { get; set; }
        public int STCEnChargeInt
        {
            get;
            set;
        }
        public int AjoutCommentaire { get; set; }
        private string _listcommande = "";
        public string listcommande
        {
            get
            {
                return _listcommande;
            }
            set
            {
                _listcommande = value;
            }
        }

        public string Listindex
        {
            get; set;
        }
        public string CodeCouleurLigne
        {
            get; set;
        }
        public string ErreurTempsCouleurCase
        {
            get; set;
        }
        public string ErreurCodeCouleur
        {
            get
            {
                if (STATUSFPS != 4)
                {
                    return "#2E86C1";
                }
                else
                {
                    return "#FFFFFF";
                }
            }
        }
        public short? STATUSnullable { get; set; }
        public string Commentairehtml
        {
            get
            {
                string tmp;
                if (Commentaire != null)
                {
                    tmp = "'" + Commentaire + "'";
                }
                else
                {
                    tmp = "''";
                }
                tmp = tmp.Replace("\r", @"\r");
                tmp = tmp.Replace("\n", @"\n");

                return tmp;
            }
        }

    }
}