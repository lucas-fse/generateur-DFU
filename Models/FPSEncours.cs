using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenerateurDFUSafir.Models
{
    public class FPSEncours
    {
        public int NameSTC { get; set; }
        public int NameTCS { get; set; }
        public int StatusViewFps { get; set; }
        public bool RechargementCourt { get
            {
                if (TRACACMDsfiltres != null)
                {
                    foreach (var f in TRACACMDsfiltres)
                    {
                        if (f.StatusFPS.Equals(0))
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
        TRACACMD _formTRACACMD = new TRACACMD();
        public TRACACMD FormTRACACMD
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
        public List<TRACACMD> TRACACMDsfiltres { get; set; }
        public List<TRACACMD> TRACACMDsfiltresupdate()
        {
            List<TRACACMD> _TRACACMDsfiltres = new List<TRACACMD>();
            TRACACMD lastfiches = new TRACACMD();
            
            foreach (var f in TRACACMDs.OrderBy(p => p.VAL_FPARAM_0).OrderBy(s=>s.GestionStatus))
            {               
                if (_TRACACMDsfiltres.Count == 0)
                {
                    f.listcommande = f.SOHNUM.Trim();
                    f.Listindex += f.IndexCMD.ToString()+";";
                    _TRACACMDsfiltres.Add(f);
                    lastfiches = f;
                }
                // si meme fiche de param et pas FBASe && fps non null && meme ensemble synchro (ENSx)
                
                else if (lastfiches.VAL_FPARAM_0.Trim().Contains(f.VAL_FPARAM_0.Trim()) && !f.VAL_FPARAM_0.Trim().Contains("FBASE") && !String.IsNullOrWhiteSpace(f.VAL_FPARAM_0.Trim())
                        && f.Ensemble_synchr.Equals(lastfiches.Ensemble_synchr) && f.BPAADD.Equals(lastfiches.BPAADD) && f.BPCORD.Equals(lastfiches.BPCORD) && f.GestionStatus.Equals(lastfiches.GestionStatus))
                {
                    // on somme les erreurs
                    if (f.StatusFPS!= 4 && f.StatusFPS!=5)
                    {
                        lastfiches.StatusFPS = f.StatusFPS;
                        lastfiches.ErreurString += f.ErreurString;
                        //lastfiches.ErreurStringCouleur = f.ErreurStringCouleur;
                    }
                    lastfiches.ERRFICHE += f.ERRFICHE;
                    lastfiches.Listindex+=f.IndexCMD.ToString() + ";";
                    if (!lastfiches.listcommande.Contains(f.SOHNUM.Trim()))
                    {
                        lastfiches.listcommande += "\r\n" + f.SOHNUM.Trim();
                    }
                    //cas de 3 lignes de commandes pour un ensemble type atex 
                    if (!String.IsNullOrWhiteSpace(f.CFGFLDALP2) && String.IsNullOrWhiteSpace(lastfiches.CFGFLDALP2))
                    {
                        lastfiches.CFGFLDALP2 = f.CFGFLDALP2;
                    }
                    else if (!String.IsNullOrWhiteSpace(f.CFGFLDALP2) && f.CFGFLDALP2 != lastfiches.CFGFLDALP2)
                    {
                        lastfiches.CFGFLDALP2 = "Erreur";
                    }
                    if (!String.IsNullOrWhiteSpace(f.CFGFLDALP3) && String.IsNullOrWhiteSpace(lastfiches.CFGFLDALP3))
                    {
                        lastfiches.CFGFLDALP3 = f.CFGFLDALP3;
                    }
                    else if (!String.IsNullOrWhiteSpace(f.CFGFLDALP3) && f.CFGFLDALP3 != lastfiches.CFGFLDALP3)
                    {
                        lastfiches.CFGFLDALP3 = "Erreur";
                    }
                    if (!String.IsNullOrWhiteSpace(f.CFGFLDALP4) && String.IsNullOrWhiteSpace(lastfiches.CFGFLDALP4))
                    {
                        lastfiches.CFGFLDALP4 = f.CFGFLDALP4;
                    }
                    else if (!String.IsNullOrWhiteSpace(f.CFGFLDALP4) && f.CFGFLDALP4 != lastfiches.CFGFLDALP4)
                    {
                        lastfiches.CFGFLDALP4 = "Erreur";
                    }
                    if (!String.IsNullOrWhiteSpace(f.CFGFLDALP2) || !String.IsNullOrWhiteSpace(f.CFGFLDALP3) || !String.IsNullOrWhiteSpace(f.CFGFLDALP4))
                    {
                        lastfiches.ITEMREF = "";
                    }
                }
                else
                {
                    f.Listindex += f.IndexCMD.ToString() + ";";
                    if (!f.listcommande.Contains(f.SOHNUM.Trim()))
                    {
                        f.listcommande += "\r\n" + f.SOHNUM.Trim();
                    }
                    // forcage de la couleur par defaut
                    
                    _TRACACMDsfiltres.Add(f);
                    lastfiches = f;
                }
            }
            DeclarationCodeErreurs codeerreurs = new DeclarationCodeErreurs();
            foreach (var f in _TRACACMDsfiltres)
            {
                if ( f.SOHNUM.StartsWith("C202204551"))
                {

                }
                if (f.SOHNUM.Contains("C202204028"))
                {

                }
                if (string.IsNullOrWhiteSpace(f.ERRFICHE))
                {
                    // reference non controlé
                    CodeGroupe value = null;
                    
                    codeerreurs.CodeErreurs.TryGetValue(9999, out value);
                    f.ErreurString += "-" + value.Erreur + "\r\n";
                    f.ErreurStringCouleur = "#34C924";
                }
                else
                {
                    string[] erreurs = f.ERRFICHE.Trim().Split(';');
                    //f.ErreurStringCouleur = "#34C924";
                    int NiveauErreur = 4;
                    foreach (var s in erreurs)
                    {
                        if (!string.IsNullOrWhiteSpace(s) && !f.ListTempoErreurs.Contains(Convert.ToInt32(s)))
                        {
                            CodeGroupe value = null;
                            f.ListTempoErreurs.Add(Convert.ToInt32(s));
                            codeerreurs.CodeErreurs.TryGetValue(Convert.ToInt32(s), out value);
                            f.ErreurString += "-" + value.Erreur + "\r\n";
                            if (Convert.ToInt32(s) < 1000)
                            {
                                f.ErreurStringCouleur = "#000000";
                                NiveauErreur = 0;
                            }
                            else if (value.Erreur.StartsWith("Warning") && NiveauErreur > 1)
                            {
                                f.ErreurStringCouleur = "#FBEF2B";
                                NiveauErreur = 2;
                            }
                            else if (NiveauErreur > 1)
                            {
                                f.ErreurStringCouleur = "#FF0000";
                                NiveauErreur = 1;
                            }
                        }                        
                        else if (NiveauErreur == 4 && f.ListTempoErreurs.Count() == 0)
                        {
                            f.ErreurStringCouleur = "#34C924";
                            NiveauErreur = 3;
                        }
                    }
                }
            }

            if (StatusViewFps ==2)
            {
                // retourne toute les fiches valide
                return _TRACACMDsfiltres.Where(s => (s.StatusFPS == 4 && (s.GestionStatus == 6 || s.GestionStatus == 5)) ).ToList();
            }
            else if (StatusViewFps == 1)
            {
                // retourne toute les fichiers valide avec controle final 
                return _TRACACMDsfiltres.Where(s => (s.StatusFPS == 4 && (s.GestionStatus == 6 || s.GestionStatus == 5)) && !s.ZFPCONTROL_0.ToUpper().Contains("PROD")).ToList();
            }
            else if (StatusViewFps == 3)
            {
                return _TRACACMDsfiltres.ToList();
            }
            else 
            {
                return _TRACACMDsfiltres.Where(s => ( s.StatusFPS != 5) && !(s.StatusFPS == 4 && (s.GestionStatus == 6 || s.GestionStatus == 5))).ToList();
            }             
        }
        public List<TRACACMD> TRACACMDs
        {
            get;set;
        }
        public void TRACAFPsUpdate()
        {
            List<TRACACMD> result = new List<TRACACMD>();
            List<OPERATEURS> Listoper = new List<OPERATEURS>();
            DeclarationCodeErreurs codeerreurs = new DeclarationCodeErreurs();
            PEGASE_PROD2Entities2 _db1 = new PEGASE_PROD2Entities2();
            OPERATEURS stc = null;
            OPERATEURS tcs = null;
            DateTime now = DateTime.Now.AddDays(-1);
            FormTRACACMD = new TRACACMD();
            if (NameSTC > 0 || NameTCS >0)
            {
                PEGASE_CHECKFPSEntities1 _db = new PEGASE_CHECKFPSEntities1();                
                stc = _db1.OPERATEURS.Where(o => o.ID == NameSTC).FirstOrDefault();
                tcs = _db1.OPERATEURS.Where(o => o.ID == NameTCS).FirstOrDefault();
                IQueryable<TRACACMD> query = null;
                if (stc != null && tcs != null)
                {
                    query = _db.TRACACMD.Include("TRACA_CHANGE_GESTIONSTATUS").Where(p => !(p.StatusFPS.Equals(5)) && p.STcEnCharge.Contains(stc.INITIAL.Trim()) && p.TCSORIG.Contains(tcs.INITIAL.Trim())).OrderBy(d => d.DateEffectiveClient);

                }
                else if (stc != null && tcs == null)
                {
                    query = _db.TRACACMD.Include("TRACA_CHANGE_GESTIONSTATUS").Where(p => !(p.StatusFPS.Equals(5)) && p.STcEnCharge.Contains(stc.INITIAL.Trim())).OrderBy(d => d.DateEffectiveClient);

                }
                else if (stc == null && tcs != null)
                {
                    query = _db.TRACACMD.Include("TRACA_CHANGE_GESTIONSTATUS").Where(p => !(p.StatusFPS.Equals(5)) && p.TCSORIG.Contains(tcs.INITIAL.Trim())).OrderBy(d => d.DateEffectiveClient);

                }
                if (query.Count() > 0)
                {
                    //result = query.Where(p => p.SOHNUM.Contains("C202202614")).ToList();
                    result = query.ToList();
                }

                FormTRACACMD.STCEnChargeInt = NameSTC;
            }
            else if (NameSTC ==-1)
            {
                PEGASE_CHECKFPSEntities1 _db = new PEGASE_CHECKFPSEntities1();
                

                var query = _db.TRACACMD.Include("TRACA_CHANGE_GESTIONSTATUS").Where(p=> (p.STCORIG!=null && p.TCSORIG!=null)).OrderBy(d => d.DateEffectiveClient);
                if (query.Count() > 0)
                {
                    //result = query.Where(p => p.SOHNUM.Contains("C202202614")).ToList();
                    result = query.ToList();
                }
                FormTRACACMD.STCEnChargeInt = 0;
            }
            else
            {
                PEGASE_CHECKFPSEntities1 _db = new PEGASE_CHECKFPSEntities1();
                var query = _db.TRACACMD.Where(p => !(p.StatusFPS.Equals(5))).OrderBy(d => d.DateEffectiveClient);
                if (query.Count() > 0)
                {
                    result = query.ToList();
                }
                FormTRACACMD.STCEnChargeInt = 0;
            }

            List<TRACACMD> resulttmp = result.Where(p => p.SOHNUM.Contains("C202302062")).ToList();
            Listoper = _db1.OPERATEURS.ToList();
            foreach (var traca in result)
            {
                if (traca.ListTempoErreurs == null) { traca.ListTempoErreurs = new List<int>(); }
                traca.ErreurString = "";
                traca.DelaiFpsString = traca.DELAIFPS.Value;
                
                traca.StatusFpsString = traca.STATUSFPS1.Value;
                traca.GestionstatusString = traca.STATUSRELANCE.Relance;
                if (stc != null)
                {
                    traca.STCEnChargeInt = (int)stc.ID;
                }
                else 
                {
                    traca.STCEnChargeInt = 0;
                }
                traca.AjoutCommentaire = 0;   

            }
            TRACACMDs = result;
            int cpt = 0;
            //mise a jour des couleurs
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
                if (f.DelaiEstimation == null  || f.DelaiEstimation == 0)
                {
                    f.ErreurTempsCouleurCase = "#FF0000";
                }
            }
            TRACACMDsfiltres = TRACACMDsfiltresupdate().OrderBy(p => p.NomClient).ToList();
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
        private List<SelectListItem> _ListViewStatusFPS = null;
        public IEnumerable<SelectListItem> ListViewStatusFPS
        {
            get
            {
                if (_ListViewStatusFPS == null)
                {
                    _ListViewStatusFPS = new List<SelectListItem>();
                    _ListViewStatusFPS.Add(new SelectListItem { Text = "Non validée", Value = "0" });
                    _ListViewStatusFPS.Add(new SelectListItem { Text = "Clos avec Ctrl final", Value = "1" });
                    _ListViewStatusFPS.Add(new SelectListItem { Text = "Clos avec et sans Ctrl final", Value = "2" });
                }
                return _ListViewStatusFPS;
            }
        }
        private List<SelectListItem>  _ListOperateursSTC = null;
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
                    List<SelectListItem>  tmp = ListCommentaireFiche;
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
        public int DefaultStcId { 
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
                if (NameSTC==0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public bool UpdatetracaFPS(TRACACMD ligne,bool addtraca)
        {
            bool result = false;
            try
            {
                PEGASE_CHECKFPSEntities1 _db = new PEGASE_CHECKFPSEntities1();

                var query = _db.TRACACMD.Where(p => p.SOHNUM == ligne.SOHNUM && p.SOPLIN == ligne.SOPLIN);
                if (query.Count() == 1)
                {
                    TRACACMD tmp = query.First();
                    TRACA_CHANGE_GESTIONSTATUS tracastc = new TRACA_CHANGE_GESTIONSTATUS();
                    tracastc.DateTime = DateTime.Now;
                    tracastc.Status = ligne.GestionStatus;
                    tracastc.STC = ligne.STcEnCharge;
                    tmp.DelaiEstimation = ligne.DelaiEstimation;
                    tmp.StatusFPS = ligne.StatusFPS;
                    tmp.GestionStatus = ligne.GestionStatus;
                    tmp.STcEnCharge = ligne.STcEnCharge;
                    tmp.Commentaire = ligne.Commentaire;
                    tmp.DateEffectiveClient = ligne.test;
                    tmp.DateCloture = ligne.DateCloture;
                    tmp.TRACA_CHANGE_GESTIONSTATUS.Add(tracastc);
                    _db.SaveChanges();
                    result = true;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                result = false;
            }
            return result;
        }
    }
}