using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenerateurDFUSafir.Models
{
    public class MonoEncours
    {
        public int NameSTC { get; set; }
        public int NameTCS { get; set; }
        public int StatusViewMono { get; set; }
        public bool RechargementCourt
        {
            get
            {
                if (TRACACMDsfiltres != null)
                {
                    foreach (var f in TRACACMDsfiltres)
                    {
                        if (f.STATUSFPS.Equals(0))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        public string CommentaireSpe { get; set; }
        TRACACMDMONO _formTRACACMD = new TRACACMDMONO();
        public TRACACMDMONO FormTRACACMD
        {
            get
            {
                return _formTRACACMD;
            }
            set
            {
                _formTRACACMD = value;
            }
        }
        public List<TRACACMDMONO> TRACACMDsfiltres { get; set; }
        public List<TRACACMDMONO> TRACACMDsfiltresupdate()
        {
            List<TRACACMDMONO> _TRACACMDsfiltres = new List<TRACACMDMONO>();
            TRACACMDMONO lastfiches = new TRACACMDMONO();

            foreach (var f in TRACACMDs)
            {

                if (_TRACACMDsfiltres.Count == 0)
                {
                    f.listcommande = f.SOHNUM.Trim();
                    f.Listindex += f.IndexCmd.ToString() + ";";
                    _TRACACMDsfiltres.Add(f);
                    lastfiches = f;
                }
                // si meme fiche de param et pas FBASe && fps non null && meme ensemble synchro (ENSx)

                
                else
                {
                    f.Listindex += f.IndexCmd.ToString() + ";";
                    if (!f.listcommande.Contains(f.SOHNUM.Trim()))
                    {
                        f.listcommande += "\r\n" + f.SOHNUM.Trim();
                    }
                    _TRACACMDsfiltres.Add(f);
                    lastfiches = f;
                }
            }
            if (StatusViewMono == 1)
            {
                // retourne toute les fiches valide
                return _TRACACMDsfiltres.Where(s => (s.STATUSFPS == 4)).ToList();
            }
            else
            {
                return _TRACACMDsfiltres.Where(s => (s.STATUSFPS != 4 && s.STATUSFPS != 5)).ToList();
            }

        }
        public List<TRACACMDMONO> TRACACMDs
        {
            get; set;
        }
        public void TRACAFPsUpdate()
        {
            List<TRACACMDMONO> result = new List<TRACACMDMONO>();
            List<OPERATEURS> Listoper = new List<OPERATEURS>();
            DeclarationCodeErreurs codeerreurs = new DeclarationCodeErreurs();
            PEGASE_PROD2Entities2 _db1 = new PEGASE_PROD2Entities2();
            OPERATEURS stc = null;
            OPERATEURS tcs = null;
            DateTime now = DateTime.Now.AddDays(-1);
            FormTRACACMD = new TRACACMDMONO();
            if (NameSTC != 0 || NameTCS != 0)
            {

                PEGASE_CHECKFPSEntities1 _db = new PEGASE_CHECKFPSEntities1();

                stc = _db1.OPERATEURS.Where(o => o.ID == NameSTC).FirstOrDefault();
                tcs = _db1.OPERATEURS.Where(o => o.ID == NameTCS).FirstOrDefault();
                IQueryable<TRACACMDMONO>  query  = null;
                if (stc != null  && tcs != null)
                {
                     query = _db.TRACACMDMONO.Where(p => !(p.STATUSFPS.Equals(5)) && p.STcEnCharge.Contains(stc.INITIAL.Trim())).OrderBy(d => d.DateEffectiveClient);
                }
                else if (stc != null && tcs == null)
                {
                     query = _db.TRACACMDMONO.Where(p => !(p.STATUSFPS.Equals(5)) && p.STcEnCharge.Contains(stc.INITIAL.Trim())).OrderBy(d => d.DateEffectiveClient);
                }
                else if (stc == null && tcs != null)
                {
                     query = _db.TRACACMDMONO.Where(p => !(p.STATUSFPS.Equals(5)) && p.TCSORIG.Contains(tcs.INITIAL.Trim())).OrderBy(d => d.DateEffectiveClient);
                }
                            
                if (query!= null && query.Count() > 0)
                {
                    result = query.ToList();
                }
                FormTRACACMD.STCEnChargeInt = NameSTC;
            }
            else
            {
                PEGASE_CHECKFPSEntities1 _db = new PEGASE_CHECKFPSEntities1();
                var query = _db.TRACACMDMONO.Where(p => !(p.STATUSFPS.Equals(5))).OrderBy(d => d.DateEffectiveClient);
                if (query.Count() > 0)
                {
                    result = query.ToList();
                }
                FormTRACACMD.STCEnChargeInt = 0;
            }
            Listoper = _db1.OPERATEURS.ToList();
            foreach (var traca in result)
            {
                if (traca.SOHNUM.Contains("C202200722"))
                {

                }
                if (traca.ListTempoErreurs == null) { traca.ListTempoErreurs = new List<int>(); }
                traca.ErreurString = "";
               // traca.DelaiMonoString = traca.DELAIF.Value;

                traca.STATUSString = traca.STATUSFPS1.Value; 
                if (stc != null)
                {
                    traca.STCEnChargeInt = (int)stc.ID;
                }
                else
                {
                    traca.STCEnChargeInt = 0;
                }
                traca.AjoutCommentaire = 0;

                string[] erreurs = traca.ERRFICHE.Trim().Split(';');
                int NiveauErreur = 2;
                foreach (var s in erreurs)
                {
                    if (!string.IsNullOrWhiteSpace(s) && !traca.ListTempoErreurs.Contains(Convert.ToInt32(s)))
                    {
                        if (Convert.ToInt32(s) < 1000)
                        {
                            traca.ErreurStringCouleur = "#000000";
                            NiveauErreur = 0;
                        }
                        else if (NiveauErreur > 1)
                        {
                            traca.ErreurStringCouleur = "#FF0000";
                            NiveauErreur = 1;
                        }
                        CodeGroupe value = null;
                        traca.ListTempoErreurs.Add(Convert.ToInt32(s));
                        codeerreurs.CodeErreurs.TryGetValue(Convert.ToInt32(s), out value);
                        if (value == null) { value = new CodeGroupe(); value.Erreur = "ERR" + s.ToString(); }
                        traca.ErreurString += "-" + value.Erreur + "\r\n";
                    }
                }
                
                if (traca.ErreurString.Equals(""))
                {
                    CodeGroupe value = null;
                    codeerreurs.CodeErreurs.TryGetValue(9999, out value);
                    traca.ErreurString += "-" + value.Erreur + "\r\n";
                }
            }
            TRACACMDs = result;
            int cpt = 0;
            foreach (var f in TRACACMDsfiltresupdate())
            {
                if (cpt % 3 == 0)
                {
                    f.CodeCouleurLigne = "#F4A460";// "rgba(0x00,0xFF,0xFF,0xFF)";
                }
                else if (cpt % 3 == 1)
                {
                    f.CodeCouleurLigne = "#FFE4C4";// "rgba(0xF4,0xA4,0x60,0xff)";
                }
                else
                {
                    f.CodeCouleurLigne = "#00FFFF"; //"rgba(0xFF,0xE4,0xC4,0xff)";
                }
                cpt++;
                //if (f.DelaiEstimation == null || f.DelaiEstimation == 0)
                //{
                //    f.ErreurTempsCouleurCase = "#FF0000";
                //}
            }
            TRACACMDsfiltres = TRACACMDsfiltresupdate();
        }
        public List<OPERATEURS> GetOperateursSTC
        {
            get
            {
                PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
                var query = _db.OPERATEURS.Where(o => o.SERVICE.Contains("STC"));
                if (query.Count() > 0)
                {
                    return query.ToList();
                }
                return new List<OPERATEURS>();
            }
        }
        private List<SelectListItem> _ListViewStatusMono = null;
        public IEnumerable<SelectListItem> ListViewStatusMono
        {
            get
            {
                if (_ListViewStatusMono == null)
                {
                    _ListViewStatusMono = new List<SelectListItem>();
                    _ListViewStatusMono.Add(new SelectListItem { Text = "Non validée", Value = "0" });
                    _ListViewStatusMono.Add(new SelectListItem { Text = "Clos", Value = "1" });
                   
                }
                return _ListViewStatusMono;
            }
        }
        private List<SelectListItem> _ListOperateursSTC = null; 
        private List<SelectListItem> _ListOperateursTCS = null;
        public IEnumerable<SelectListItem> ListOperateursSTC
        {
            get
            {
                if (_ListOperateursSTC == null)
                {
                    _ListOperateursSTC = new List<SelectListItem>();
                    DateTime now = DateTime.Now;
                    PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
                    var query = _db.OPERATEURS.Where(o => o.SERVICE.Contains("STC") && (o.FINCONTRAT > now || o.FINCONTRAT == null));
                    _ListOperateursSTC.Add(new SelectListItem { Text = "tous", Value = "0" });
                    if (query.Count() > 0)
                    {
                        foreach (var t in query.ToList())
                        {
                            _ListOperateursSTC.Add(new SelectListItem { Text = t.INITIAL, Value = ((int)t.ID).ToString() });
                        }
                    }
                }
                return _ListOperateursSTC;
            }
        }
        public IEnumerable<SelectListItem> ListOperateursTCS
        {
            get
            {
                if (_ListOperateursTCS == null)
                {
                    _ListOperateursTCS = new List<SelectListItem>();
                    PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
                    DateTime now = DateTime.Now;
                    var query = _db.OPERATEURS.Where(o => o.SERVICE.Contains("TCS") && (o.FINCONTRAT > now || o.FINCONTRAT == null));
                    _ListOperateursTCS.Add(new SelectListItem { Text = "tous", Value = "0" });
                    if (query.Count() > 0)
                    {
                        foreach (var t in query.ToList())
                        {
                            _ListOperateursTCS.Add(new SelectListItem { Text = t.INITIAL, Value = ((int)t.ID).ToString() });
                        }
                    }
                }
                return _ListOperateursTCS;
            }
        }
        private List<SelectListItem> _ListCommentaireFiche = null;
        private List<SelectListItem> _ListCommentaireFicheDate = null;
        public List<SelectListItem> ListCommentaireFicheDate
        {
            get
            {
                if (_ListCommentaireFicheDate == null)
                {
                    List<SelectListItem> tmp = ListCommentaireFiche;
                }
                return _ListCommentaireFicheDate;
            }
        }
        public List<SelectListItem> ListCommentaireFiche
        {
            get
            {
                if (_ListCommentaireFiche == null)
                {
                    _ListCommentaireFiche = new List<SelectListItem>();
                    _ListCommentaireFicheDate = new List<SelectListItem>();
                    PEGASE_CHECKFPSEntities1 _db = new PEGASE_CHECKFPSEntities1();
                    var query = _db.COMMENTAIRE;
                    if (query.Count() > 0)
                    {
                        foreach (var t in query.ToList())
                        {
                            _ListCommentaireFiche.Add(new SelectListItem { Text = t.Value.Replace("%Date", ""), Value = ((int)t.IndexCom).ToString() });
                            _ListCommentaireFicheDate.Add(new SelectListItem { Text = t.Value, Value = ((int)t.IndexCom).ToString() });
                        }
                    }
                }
                return _ListCommentaireFiche;
            }
        }
        public int DefaultStcId
        {
            get;
            set;
        }
        public int DefaultTCSId
        {
            get;
            set;
        }
        public int StatusDemande
        {
            get;
            internal set;
        }
        private List<SelectListItem> _ListDelaiFPS = null;
        public IEnumerable<SelectListItem> ListDelaiFPS
        {
            get
            {
                if (_ListDelaiFPS == null)
                {
                    _ListDelaiFPS = new List<SelectListItem>();
                    PEGASE_CHECKFPSEntities1 _db = new PEGASE_CHECKFPSEntities1();
                    var query = _db.DELAIFPS;
                    if (query.Count() > 0)
                    {
                        foreach (var t in query.ToList())
                        {
                            _ListDelaiFPS.Add(new SelectListItem { Text = t.Value, Value = t.indexDELAI.ToString() });
                        }
                    }
                }
                return _ListDelaiFPS;
            }
        }
        public IEnumerable<SelectListItem> ListStatusFPS
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>();
                PEGASE_CHECKFPSEntities1 _db = new PEGASE_CHECKFPSEntities1();
                var query = _db.STATUSFPS;

                if (query.Count() > 0)
                {
                    foreach (var t in query.ToList())
                    {
                        // status 5  clos non validé (cmd supprimée ) 6 check periodique
                        if (t.IndexSTATUSFPS != 5 && t.IndexSTATUSFPS != 6)
                        {
                            result.Add(new SelectListItem { Text = t.Value, Value = t.IndexSTATUSFPS.ToString() });
                        }
                    }
                }
                return result;
            }
        }
        public bool disable1
        {
            get
            {
                if (NameSTC == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public bool UpdatetracaFPS(TRACACMDMONO ligne)
        {
            bool result = false;
            try
            {
                PEGASE_CHECKFPSEntities1 _db = new PEGASE_CHECKFPSEntities1();

                var query = _db.TRACACMDMONO.Where(p => p.SOHNUM == ligne.SOHNUM && p.SOPLIN == ligne.SOPLIN);
                if (query.Count() == 1)
                {
                    TRACACMDMONO tmp = query.First();
                   // tmp.DelaiEstimation = ligne.DelaiEstimation;
                    tmp.STATUSFPS = ligne.STATUSFPS;
                    tmp.STATUSFPS = ligne.STATUSFPS;
                    tmp.STcEnCharge = ligne.STcEnCharge;
                    tmp.Commentaire = ligne.Commentaire;
                    tmp.DateEffectiveClient = ligne.test;
                    tmp.DateCloture = ligne.DateCloture;
                    _db.SaveChanges();
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}