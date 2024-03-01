using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenerateurDFUSafir.Models.DAL
{
    public partial class TRACACMD
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
        public int DefaultStatusFps1 { get; set; }
        public int DelaiFps1 { get; set; }
        public int DefaultDelaiFps1 { 
            get; 
            set; }
        public string DelaiFpsString
        {
            get;
            set;
        }
        public string StatusFpsString
        {
            get;
            set;
        }
        public string GestionstatusString
        {
            get;
            set;
        }
        public DateTime test
        {   get 
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
            get;set;
        }
        public string CodeCouleurLigne
        {get;set;
        }
        public string ErreurTempsCouleurCase
        {
            get; set;
        }
        public string ErreurCodeCouleur
        {
            get
            {
                if (StatusFPS!= 4)
                {
                    return "#2E86C1";
                }
                else
                {
                    return "#FFFFFF";
                }
            }
        }
        public string InfoBulle
        {
            get
            {
                return CFGFLDALP2 + "\r\n" +
                        CFGFLDALP3 + "\r\n" +
                        CFGFLDALP4 + "";
            }
        }
        /// <summary>
        /// groupa auquel aprtient la ligne ENS1
        /// </summary>
        public string Ensemble_synchr
        {
            get 
            {
                if (!String.IsNullOrWhiteSpace(SYNCHRO) && SYNCHRO.Length>=4)
                {
                    return SYNCHRO.Substring(0, 4);
                }
                else
                {
                    return "";
                }
            }
        }
        public short? StatusFPSnullable { get; set; }
        public string Commentairehtml
        {
            get
            {
                string tmp;

                //if (Commentaire != null)
                //{
                //    tmp = Commentaire.Replace("'", @"'");
                //    tmp = "'"+ tmp + "'";
                //}
                //else
                //{
                //    tmp = "''";
                //}
                //tmp = tmp.Replace("\r", @"\r");
                //tmp = tmp.Replace("\n", @"\n");
                tmp = JsonConvert.SerializeObject(Commentaire);
                return tmp;
            }
        }
        public IEnumerable<SelectListItem> ListStatus
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>();
                PEGASE_CHECKFPSEntities1 _db = new PEGASE_CHECKFPSEntities1();
                var query = _db.STATUSRELANCE.Where(p=>p.Actif == true);

                if (query.Count() > 0)
                {
                    foreach (var t in query.ToList())
                    {
                           result.Add(new SelectListItem { Text = t.Relance, Value = t.IndexRELANCE.ToString() });                        
                    }
                }
                return result;
            }
        }
        public short? StatusNullable { get; set; }
        public TRACA_CHANGE_GESTIONSTATUS LastTraca
        {
            get
            {
                TRACA_CHANGE_GESTIONSTATUS tmp = null;
                if (TRACA_CHANGE_GESTIONSTATUS != null && TRACA_CHANGE_GESTIONSTATUS.Count > 0)
                {
                    tmp = TRACA_CHANGE_GESTIONSTATUS.OrderBy(t => t.DateTime).Last();
                }
                return tmp;
            }
        }
        public bool ContactTech
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(NomTech) && (!String.IsNullOrWhiteSpace(MailTech) || !String.IsNullOrWhiteSpace(TelTech) || !String.IsNullOrWhiteSpace(MobTech)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}