using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;


namespace GenerateurDFUSafir.Models
{
    public class InfoSerenite
    {
        
        Salarie _tRACASalarie = new Salarie();
        public Salarie TRACASalarie
        {
            get
            {
                return _tRACASalarie;
            }
            set
            {
                _tRACASalarie = value;
            }
        }

        public IEnumerable<SelectListItem> ListOperateursSTC
        {
            get
            {
                List<SelectListItem> _ListPoleProd = new List<SelectListItem>();
                _ListPoleProd.Add(new SelectListItem { Text = "Autre", Value = "0" });
                _ListPoleProd.Add(new SelectListItem { Text = "Pole Bidirectionnel", Value = "2" });
                _ListPoleProd.Add(new SelectListItem { Text = "Pôle MonoDirectionnel", Value = "3" });

                return _ListPoleProd;
            }
        }
							
        //Liste de tous les poles en BDD
        public List<POLES> listPoles { get; set; }
        List<SelectListItem> _ListAnimateurProd = null;
        public IEnumerable<SelectListItem> ListAnimateurProd
        {
            get
            {
                if (_ListAnimateurProd == null)
                {
                    _ListAnimateurProd = new List<SelectListItem>();
                    PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
                    var query = _db.OPERATEURS.Where(o => o.SERVICE.Contains("PROD") && o.ANIMATEUR == true);
                    //_ListAnimateurProd.Add(new SelectListItem { Text = "tous", Value = "0" });
                    if (query.Count() > 0)
                    {
                        foreach (var t in query.ToList())
                        {
                            _ListAnimateurProd.Add(new SelectListItem { Text = t.PRENOM+"."+t.NOM.Substring(0,1), Value = ((int)t.ID).ToString() });
                        }
                    }
                }
                return _ListAnimateurProd;
            }
        }
        public int DefaultStcId
        {
            get;
            set;
        }
        public List<Salarie> listSalarie { get; set; }

        // Liste de tout les postes du pole actuel
        public List<POSTES> ListPostes { get; set; }

        

        public InfoSerenite(string type = "")
        {
            using (PEGASE_PROD2Entities2 pEGASE_PROD2Entities = new PEGASE_PROD2Entities2())
            {
                DateTime now = DateTime.Now;
                DateTime cejour = new DateTime(now.Year, now.Month, now.Day);
                List<OPERATEURS> operProd = pEGASE_PROD2Entities.OPERATEURS.Include("SERENITE").Where(o => o.SERVICE.Contains("PROD") && (o.FINCONTRAT == null || o.FINCONTRAT > now)).OrderBy(n => n.PRENOM).ToList();
                listSalarie = new List<Salarie>();
                listPoles = pEGASE_PROD2Entities.POLES.ToList();
                List<long> polestring = new List<long>(); 
                foreach (var p in listPoles)
                {
                    polestring.Add(p.ID);
                }
                if (type.Contains("PRODONLY"))
                {
                    operProd = operProd.Where(p => p.POLE != null).ToList();
                }
                foreach (var op in operProd)
                {
                    if (op.POLE == null || !polestring.Contains((int)op.POLE))
                    {
                        op.POLE = 0;
                    }
                    if (op.SERENITE.Where(t => t.Datetime > cejour).Count() == 0)
                    {
                        listSalarie.Add(new Salarie(op.ID, op.PRENOM,op.NOM, op.POLE, "../operateurs" + op.PATHA, null, null, op.ANIMATEUR));
                    }
                    else
                    {
                        int niv_serenite = op.SERENITE.Where(t => t.Datetime > cejour).First().NiveauSerenite;
                        int? niv_serenite2 = op.SERENITE.Where(t => t.Datetime > cejour).First().NiveauSerenite2;
                        listSalarie.Add(new Salarie(op.ID, op.PRENOM, op.NOM, op.POLE, "../operateurs" + op.PATHA, niv_serenite, niv_serenite2, op.ANIMATEUR));
                    }
                }
            }
        }
        public void Avoter(int? id,int? level)
        {
            if (id != null)
            {
                PEGASE_PROD2Entities2 pEGASE_PROD2Entities = new PEGASE_PROD2Entities2();
                DateTime now = DateTime.Now;
                DateTime cejour = new DateTime(now.Year, now.Month, now.Day);

                List<SERENITE> ListoperProd = pEGASE_PROD2Entities.SERENITE.Where(o => o.ID == id && o.Datetime > cejour).ToList();
                SERENITE operProd = null;
                bool newVote = false;
                if (ListoperProd != null && ListoperProd.Count > 0)
                {
                    operProd = ListoperProd.First();
                    
                }
                else
                {
                    operProd = new SERENITE();
                    operProd.ID = (int)id;
                    pEGASE_PROD2Entities.SERENITE.Add(operProd);
                    newVote = true;
                }
                if (DateTime.Now.Hour > 12)
                {
                    if (level != null)
                    {
                        operProd.NiveauSerenite2 = (int)level;
                        if (newVote) { operProd.NiveauSerenite = (int)level; }
                        operProd.Datetime = now;
                    }
                    else
                    {
                        operProd.NiveauSerenite2 = 0;
                        operProd.Datetime = now;
                    }
                }
                else
                {
                    if (level != null)
                    {
                        operProd.NiveauSerenite = (int)level;
                        operProd.Datetime = now;
                    }
                    else
                    {
                        operProd.NiveauSerenite = 0;
                        operProd.Datetime = now;
                    }
                }
                pEGASE_PROD2Entities.SaveChanges();
            }
        }
        public void CalculSerenite()
        {
            PEGASE_PROD2Entities2 pEGASE_PROD2Entities = new PEGASE_PROD2Entities2();
            DateTime now = DateTime.Now;
            DateTime cejour = new DateTime(now.Year, now.Month, now.Day);

            List<SERENITE> ListoperProd = pEGASE_PROD2Entities.SERENITE.Where(o =>  o.Datetime > cejour).ToList();
            int sommeSerenite = 0;
            int NBVotant = 0;
            foreach(var op in ListoperProd)
            {
                if (op.NiveauSerenite > 0)
                {
                    sommeSerenite += op.NiveauSerenite-1;
                    NBVotant++;
                }
            }
            int? SerenitePourCent = null;
            if (NBVotant >= 3)
            {
                 SerenitePourCent = sommeSerenite * 25 / NBVotant;
            }
            else
            {

            }
            DATA_GENERIQUE dtatgen = pEGASE_PROD2Entities.DATA_GENERIQUE.Where(i => i.ID == 3).First();
            dtatgen.Value2 = SerenitePourCent;
            dtatgen.Datetime1 = DateTime.Now;
            pEGASE_PROD2Entities.SaveChanges();
        }
        public void Save()
        {
            using (PEGASE_PROD2Entities2 pEGASE_PROD2Entities = new PEGASE_PROD2Entities2())
            {
                var query = pEGASE_PROD2Entities.OPERATEURS.Where(p => p.ID == TRACASalarie.Id);
                if (query!= null && query.Count()==1)
                {
                    OPERATEURS op = query.First();
                    if (TRACASalarie.Pole != null)
                    {
                        op.POLE = TRACASalarie.Pole;
                    }
                }
                pEGASE_PROD2Entities.SaveChanges();
            }
        }
    }

    public class Salarie
    {
        public string Name { get; set; }
        public string Nom { get; set; }
        public string Image { get; set; }
        public int? humeurDuJour { get; set; }
        public int? humeurDuJour2 { get; set; } // declaration de variable
        public int? Pole { get; set; }   
        public long Id { get; set; }
        public bool Animateur { get; set; }

        public Salarie()
        {
            
        }
        public Salarie(long _id,string _Name, string _Nom, int? _pole, string _Image, int? _humeurDuJour, int? _humeurDuJour2, bool _Animateur)
        {
            Id = _id;
            Nom = _Nom;
            Name = _Name;
            Image = _Image;
            if (_pole != null)
            {
                Pole = Convert.ToInt32(_pole);
            }
            else
            {
                Pole = 0;
            }           
            humeurDuJour = _humeurDuJour;
            humeurDuJour2 = _humeurDuJour2;
            Animateur = _Animateur;
        }

        public string getCouleur()
        {
            if (humeurDuJour == 1)//"PasContentDuTout")
            {
                return "#FF0000";
            }
            else if ( humeurDuJour == 2)//"PasContent")
            {
                return "#F4661B";
            }
            else if (humeurDuJour == 3)//"Moyen")
            {
                return "#FFD700";
            }
            else if (humeurDuJour == 4)//"Content")
            {
                return "#C2F732";
            }
            else if (humeurDuJour == 5)//"TresContent")
            {
                return "#2D8E22";
            }
            else
            {
                return "none";
            }
        }
        public string getCouleur2() // nouvelle methode de la class Salarie
        {
            
            if (humeurDuJour2 == 1)
            {
                return "#FF0000";
            }
            else if (humeurDuJour2 == 2)
            {
                return "#F4661B";
            }

            else if (humeurDuJour2 == 3)
            {
                return "#FFD700";
            }
            else if (humeurDuJour2 == 4)
            {
                return "#C2F732";
            }
            else if (humeurDuJour2 == 5)
            {
                return "#32CD32";
            }
            else
            {
                return "none";
            }
        }

    }
}
   