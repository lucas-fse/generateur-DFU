using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenerateurDFUSafir.Models
{
    public class CmdEnCours
    {
        public int NameSTC { get; set; }
        public int NameTCS { get; set; }
        public int StatusCmd { get; set; }

        public List<TRACAALLCMD> ListCmdAll { get; set; }
        public TRACAALLCMD FormTRACACMD { get; set; }
        private List<SelectListItem> _ListStatusCmd;
        public List<SelectListItem> ListStatusCmd
        {
            get
            {               
                if (_ListStatusCmd == null)
                {
                    _ListStatusCmd = new List<SelectListItem>();
                    _ListStatusCmd.Add(new SelectListItem { Text = "Cmd Verte en attente", Value = "0" });
                    _ListStatusCmd.Add(new SelectListItem { Text = "Cmd Rouge", Value = "1" });
                    _ListStatusCmd.Add(new SelectListItem { Text = "Cmd Verte en prod", Value = "2" });
                    _ListStatusCmd.Add(new SelectListItem { Text = "Toutes cmd", Value = "3" });
                }
                return _ListStatusCmd;
            }
        }
        public int DefaultStatusCmd
        {
            get;
            set;
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
        private List<SelectListItem> _ListOperateursSTC = null;
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
        private List<SelectListItem> _ListOperateursTCS = null;
        public IEnumerable<SelectListItem> ListOperateursTCS
        {
            get
            {
                if (_ListOperateursTCS == null)
                {
                    _ListOperateursTCS = new List<SelectListItem>();
                    DateTime now = DateTime.Now;
                    PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
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
        public int DefaultStcId
        {
            get;
            set;
        }
        public int DefaultTcsId
        {
            get;
            set;
        }
        public bool disable1
        {
            get
            {
                if ((NameSTC == 0) && (NameTCS == 0))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public bool RechargementCourt
        {
            get
            {
                if (ListCmdAll != null)
                {
                    foreach (var f in ListCmdAll)
                    {
                        if (f.STATUS.Equals(0))
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
        public IEnumerable<SelectListItem> ListStatus
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
        public void CmdEncours()
        {
            ListCmdAll = new List<TRACAALLCMD>();
            List<TRACACMD> result = new List<TRACACMD>();
            List<OPERATEURS> Listoper = new List<OPERATEURS>();
            DeclarationCodeErreurs codeerreurs = new DeclarationCodeErreurs();
            PEGASE_PROD2Entities2 _db1 = new PEGASE_PROD2Entities2();
            OPERATEURS stc = null;
            OPERATEURS tcs = null;
            DateTime now = DateTime.Now.AddDays(-1);
            FormTRACACMD = new TRACAALLCMD();
            if (NameSTC != 0)
            {
                PEGASE_CHECKFPSEntities1 _db = new PEGASE_CHECKFPSEntities1();

                stc = _db1.OPERATEURS.Where(o => o.ID == NameSTC).First();

                var query = _db.TRACACMD.Where(p => !(p.StatusFPS.Equals(4) &&  p.StatusFPS.Equals(5)) && p.STcEnCharge.Contains(stc.INITIAL.Trim())).OrderBy(d => d.DateEffectiveClient);
                if (query.Count() > 0)
                {
                    result = query.ToList();
                }
                FormTRACACMD.STCEnChargeInt = NameSTC;
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
            if (NameTCS != 0)
            { 
                tcs = _db1.OPERATEURS.Where(o => o.ID == NameTCS ).First();

                var query = result.Where(p => !( p.StatusFPS.Equals(5)) && p.TCSORIG.Contains(tcs.INITIAL.Trim())).OrderBy(d => d.DateEffectiveClient);
                if (query.Count() > 0)
                {
                    result = query.ToList();
                }
                else
                {
                    result = new List<TRACACMD>();
                }
                FormTRACACMD.TCSEnChargeInt = NameSTC;
            }
            else
            {
                var query = result.Where(p => !(p.StatusFPS.Equals(5))).OrderBy(d => d.DateEffectiveClient);
                if (query.Count() > 0)
                {
                    result = query.ToList();
                }
                FormTRACACMD.TCSEnChargeInt = 0;
            }
            // commande en attente et fps clos ou fps non close
            if (StatusCmd == 0)
            {
                var query = result.Where(p => (!p.STAT.Contains("Normal") && p.StatusFPS == 4) ).OrderBy(d => d.DateEffectiveClient);
                if (query.Count() > 0)
                {
                    result = query.ToList();
                }
                else
                {
                    result = new List<TRACACMD>();
                }
            }
            else if (StatusCmd == 1)
            {
                var query = result.Where(p => ( p.StatusFPS == 1)).OrderBy(d => d.DateEffectiveClient);
                if (query.Count() > 0)
                {
                    result = query.ToList();
                }
                else
                {
                    result = new List<TRACACMD>();
                }
            }
            // cmd ok verte et pa sen attente
            else if (StatusCmd == 2)
            {
                var query = result.Where(p => (p.STAT.Contains("Normal") && p.StatusFPS == 4)  ).OrderBy(d => d.DateEffectiveClient);
                if (query.Count() > 0)
                {
                    result = query.ToList();
                }
                else
                {
                    result = new List<TRACACMD>();
                }
            }
            else
            {
                // tt le reste
            }
            result = result.OrderBy(c => c.SOHNUM).ToList();
            Listoper = _db1.OPERATEURS.ToList();
            int cpt = 1;
            
            foreach (var traca in result)
            {
                TRACAALLCMD tmp = new TRACAALLCMD();
                tmp.IndexCMD = cpt; cpt++;
                tmp.IndexOrifCmd = traca.IndexCMD;
                tmp.SOHNUM = traca.SOHNUM;
                tmp.SOPLIN = new List<string>();
                tmp.SOPLIN.Add(traca.SOPLIN);
                tmp.StatusCmd = traca.STAT;
                tmp.STCORIG = traca.STCORIG;
                tmp.STCEnCharge = traca.STcEnCharge;
                tmp.STATUS = traca.StatusFPS;
                tmp.ERRFICHE = traca.ERRFICHE;
                tmp.STCEnChargeInt = traca.STCEnChargeInt;
                tmp.TCSORIG = traca.TCSORIG;
                tmp.NomClient = traca.NomClient;
                tmp.DateVerif = traca.DateVerif;
                tmp.CREDAT_0 = traca.CREDAT_0;
                tmp.SHIDAT = traca.SHIDAT;
                tmp.EXTDLVDAT = traca.EXTDLVDAT;
                tmp.DEMDLVDAT_0 = traca.DEMDLVDAT_0;
                tmp.ORDDAT_0 = traca.ORDDAT_0;
                tmp.DateEffectiveClient = traca.DateEffectiveClient;
                tmp.STATUSString = traca.STATUSFPS1.Value;
                tmp.TypeData = 0;
                tmp.PRODUIT = new List<string>();
                if (traca.ITEMREF != null)
                {
                    tmp.PRODUIT.Add(  traca.ITEMREF);
                }
                else
                {
                    
                }

                if (tmp.ListTempoErreurs == null) { tmp.ListTempoErreurs = new List<int>(); }
                tmp.ErreurString = "";

                if (stc != null)
                {
                    tmp.STCEnChargeInt = (int)stc.ID;
                }
                else
                {
                    tmp.STCEnChargeInt = 0;
                }
                ListCmdAll.Add(tmp);
                
                
            }

            List<TRACACMDMONO> result1 = new List<TRACACMDMONO>();
            
            codeerreurs = new DeclarationCodeErreurs();
            

            if (NameSTC != 0)
            {
                PEGASE_CHECKFPSEntities1 _db = new PEGASE_CHECKFPSEntities1();

                stc = _db1.OPERATEURS.Where(o => o.ID == NameSTC).First();

                var query = _db.TRACACMDMONO.Where(p => !( p.STATUSFPS.Equals(5)) && p.STcEnCharge.Contains(stc.INITIAL.Trim())).OrderBy(d => d.DateEffectiveClient);
                if (query.Count() > 0)
                {
                    result1 = query.ToList();
                }
                FormTRACACMD.STCEnChargeInt = NameSTC;
            }
            else
            {
                PEGASE_CHECKFPSEntities1 _db = new PEGASE_CHECKFPSEntities1();
                var query = _db.TRACACMDMONO.Where(p => !(p.STATUSFPS.Equals(5))).OrderBy(d => d.DateEffectiveClient);
                if (query.Count() > 0)
                {
                    result1 = query.ToList();
                }
                FormTRACACMD.STCEnChargeInt = 0;
            }
            if (NameTCS != 0)
            {
                tcs = _db1.OPERATEURS.Where(o => o.ID == NameTCS).First();

                var query = result1.Where(p => !(p.STATUSFPS.Equals(4) && p.STATUSFPS.Equals(5)) && p.TCSORIG != null && p.TCSORIG.Contains(tcs.INITIAL.Trim())).OrderBy(d => d.DateEffectiveClient);
                if (query.Count() > 0)
                {
                    result1 = query.ToList();
                }
                else
                {
                    result1 = new List<TRACACMDMONO>();
                }
                FormTRACACMD.TCSEnChargeInt = NameTCS;
            }
            else
            {
                
                var query = result1.Where(p => !(p.STATUSFPS.Equals(5))).OrderBy(d => d.DateEffectiveClient);
                if (query.Count() > 0)
                {
                    result1 = query.ToList();
                }
                else
                {
                    result1 = new List<TRACACMDMONO>();
                }
                FormTRACACMD.TCSEnChargeInt = 0;
            }
            // commande en attente et fps clos ou fps non close
            if (StatusCmd == 0)
            {
                var query = result1.Where(p => (!p.STAT.Contains("Normal") && p.STATUSFPS == 4)).OrderBy(d => d.DateEffectiveClient);
                if (query.Count() > 0)
                {
                    result1 = query.ToList();
                }
                else
                {
                    result1 = new List<TRACACMDMONO>();
                }
            }
            else if (StatusCmd == 1)
            {
                var query = result1.Where(p => (p.STATUSFPS == 1)).OrderBy(d => d.DateEffectiveClient);
                if (query.Count() > 0)
                {
                    result1 = query.ToList();
                }
                else
                {
                    result1 = new List<TRACACMDMONO>();
                }
            }
            // cmd ok verte et pa sen attente
            else if (StatusCmd == 2)
            {
                var query = result1.Where(p => (p.STAT.Contains("Normal") && p.STATUSFPS == 4)).OrderBy(d => d.DateEffectiveClient);
                if (query.Count() > 0)
                {
                    result1 = query.ToList();
                }
                else
                {
                    result1 = new List<TRACACMDMONO>();
                }
            }
            else
            {
                // tt le reste
            }

            foreach (var traca in result1)
            {
                TRACAALLCMD tmp = new TRACAALLCMD();
                tmp.IndexCMD = cpt; cpt++;
                tmp.IndexOrifCmd = traca.IndexCmd;
                tmp.StatusCmd = traca.STAT;
                tmp.SOHNUM = traca.SOHNUM;
                tmp.SOPLIN = new List<string>();
                tmp.SOPLIN.Add(traca.SOPLIN);
                tmp.STCORIG = traca.STCORIG;
                tmp.STCEnCharge = traca.STCORIG;
                tmp.STATUS = traca.STATUSFPS;
                tmp.ERRFICHE = traca.ERRFICHE;
                tmp.STCEnChargeInt = traca.STCEnChargeInt;
                tmp.TCSORIG = traca.TCSORIG;
                tmp.NomClient = traca.NomClient;
                tmp.DateVerif = traca.DateVerif;
                tmp.CREDAT_0 = traca.CREDAT_0;
                tmp.SHIDAT = traca.SHIDAT;
                tmp.EXTDLVDAT = traca.EXTDLVDAT_0;
                tmp.DEMDLVDAT_0 = traca.DEMDLVDAT_0;
                //tmp.ORDDAT_0 = traca.ORDDAT_0;
                tmp.DateEffectiveClient = traca.DateEffectiveClient;
                tmp.STATUSString = traca.STATUSFPS1.Value;
                tmp.TypeData = 1;
                tmp.PRODUIT = new List<string>();
                if (traca.PRODUIT != null)
                {
                    tmp.PRODUIT.Add(traca.PRODUIT);
                    
                }
                else
                {
                    
                }

                if (tmp.ListTempoErreurs == null) { tmp.ListTempoErreurs = new List<int>(); }
                tmp.ErreurString = "";



                if (stc != null)
                {
                    tmp.STCEnChargeInt = (int)stc.ID;
                }
                else
                {
                    tmp.STCEnChargeInt = 0;
                }
                ListCmdAll.Add(tmp);
                
            }
            cpt = 0;


            TRACAALLCMD Lastligne = new TRACAALLCMD();
            
            List<TRACAALLCMD> ListCmdAlltmp = ListCmdAll.OrderBy(c => c.SOHNUM).ToList();
            ListCmdAll = new List<TRACAALLCMD>();
            foreach (var f in ListCmdAlltmp)
            {
                if (Lastligne.SOHNUM == f.SOHNUM)
                {
                    Lastligne.ERRFICHE += ";" + f.ERRFICHE;
                    Lastligne.PRODUIT.AddRange(f.PRODUIT);
                    Lastligne.SOPLIN.AddRange(f.SOPLIN);
                }
                else
                {
                    Lastligne = f;
                    ListCmdAll.Add(f);
                }
                // mise a jour de l'object 
                string[] erreurs = Lastligne.ERRFICHE.Trim().Split(';');
                int NiveauErreur = 2;
                List<int> ListTempoErreurs = new List<int>();
                foreach (var s in erreurs)
                {
                    if (!string.IsNullOrWhiteSpace(s) && !ListTempoErreurs.Contains(Convert.ToInt32(s)))
                    {
                        if (Convert.ToInt32(s) < 1000)
                        {
                            Lastligne.ErreurStringCouleur = "#000000";
                            NiveauErreur = 0;
                        }
                        else if (NiveauErreur > 1)
                        {
                            Lastligne.ErreurStringCouleur = "#FF0000";
                            NiveauErreur = 1;
                        }
                        CodeGroupe value = null;
                        ListTempoErreurs.Add(Convert.ToInt32(s));
                        codeerreurs.CodeErreurs.TryGetValue(Convert.ToInt32(s), out value);
                        Lastligne.ErreurString += "-" + value.Erreur + "\r\n";
                    }
                }
                if (Lastligne.ErreurString.Equals(""))
                {
                    CodeGroupe value = null;
                    codeerreurs.CodeErreurs.TryGetValue(9999, out value);
                    Lastligne.ErreurString += "-" + value.Erreur + "\r\n";
                }




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
        }
    }
}