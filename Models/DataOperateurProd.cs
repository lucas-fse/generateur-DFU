using GenerateurDFUSafir.DAL;
using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace GenerateurDFUSafir.Models
{
    public class DataOperateurProd
    {
        public long ID { get; set; }
        public String Nom { get; set; }
        public bool Popup { get; set; }

        public String Prenom { get; set; }
        public String Initial { get; set; }

        public String Password { get; set; }

        public bool isValidPasswd { get; set; }

        public string Email { get; set; }

        public List<OFView> ofAfaire { get; set; }
        public bool Animateur { get; set; }
        public DateTime? Anniversaire { get; set; }
        [Required]
        public List<OF_PROD_TRAITE> OfEncours
        {
            get;
            set;
        } /* OF en haut */
        public List<OF_PROD_TRAITE> OfNontraiteNontrace
        {
            get;
            set;
        }
        /* tous les OF pour un operateur */
        public List<OF_PROD_TRAITE> OfNontraiteNontraceAff
        { /* tous les OF selon ilot */
            get
            {
                DateTime now = DateTime.Now;
                DateTime nowfiltre = new DateTime(now.Year, now.Month, now.Day);
                //return OfNontraiteNontrace.Where(p => p.ENDTIMETHEORIQUE >= nowfiltre).OrderBy(t  => t.NMROF).ToList();
                return OfNontraiteNontrace.ToList();
            }
        }
        //public string Photo 
        //{
        //    get
        //    {
        //        return "/operateurs/OperateursA/"+ Initial + ".bmp";
        //    }
        //}

        public string Photo { get; set; }
        public string OFFindToStart { get; set; }
        public IEnumerable<SelectListItem> ListIlotdispo
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem {Text = "Tous", Value="1"},
                    new SelectListItem {Text = "Bidir", Value="2"},
                    new SelectListItem {Text = "Mono", Value="3"},
                    new SelectListItem {Text = "Test", Value="4"}
                };
            }
        }
        public int DefaultIlotid { get; internal set; }
        public int Ilotid { get; set; }
        public string TryToAddOF { get; set; }
        public DeclarationAlea DeclarationAlea { get; set; } /* sert pour la déclaration d'aléas */
        public string JsonOfEnCours { get; set; }
        //OfX3 data;
        public DataOperateurProd() /* constructeur temporaire */
        {
            this.DeclarationAlea = new DeclarationAlea();
            DeclarationAlea.GestionCodeOF();
            //  data = new OfX3();
            // OfEncours = data.ListOFsTraite();

        }

        public void initOfList(int pole, string ofCherche, bool ChargeOfPlanifie)
        {
            ofAfaire = getOfsAFaire(pole, ofCherche, ChargeOfPlanifie);
            //ofAfaire = new List<OFView>();
        }

        public void ToJson()
        {
            JsonOfEnCours = JsonConvert.SerializeObject(OfEncours, Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            //JsonOfEnCours = JsonConvert.SerializeObject(OfEncours);
        }

        private List<OFView> getOfsAFaire(long pole, string ofCherche, bool ChargeOfPlanifie)
        {
            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();
            DataTable rawResult = new DataTable();
            ofProdIieCmd Ofs = new ofProdIieCmd();
            List<PLANIF_OF> ofs = new List<PLANIF_OF>();
            if (!string.IsNullOrWhiteSpace(ofCherche))
            {
                PLANIF_OF of = new PLANIF_OF();
                of.NumOF = ofCherche.Trim();
                of.Etat = 1;
                ofs.Add(of);
                Ofs.RequeteOFBis(ref rawResult, ofCherche, "EDITE");
            }
            else
            {
                if (ChargeOfPlanifie)
                {
                    ofs = db.PLANIF_OF.Where(i => i.Pole == pole).OrderBy(i => i.DatePlanif)
                       .ThenBy(i => i.Rang).ToList();
                    Ofs.RequeteOFBis(ref rawResult, "", "EDITE");
                }
            }
            // On récupère les vrais ofs de X3 correspondant

            //Ofs.RequeteOF(ref rawResult,"EDITE");


            List<OFView> liste_of = new List<OFView>();
            List<string> poste_occupe = new List<string>();

            List<POSTES> ListPostes = db.POSTES.ToList();
            List<OF_PROD_TRAITE> oF_PROD_TRAITEs = db.OF_PROD_TRAITE.ToList();
            foreach (PLANIF_OF of in ofs)
            {

                OFView of_cherche = null;

                // On cherche l'OF avec le bon numéro
                foreach (DataRow row in rawResult.Rows)
                {

                    if (row["MFGNUM_0"].ToString().Trim().Equals(of.NumOF))
                    {
                        of_cherche = new OFView();
                        of_cherche.numOF = row["MFGNUM_0"].ToString();
                        of_cherche.refIndu = row["ITMREF_0"].ToString();
                        of_cherche.rupture = row["ALLSTA_0"].ToString() != "3";
                        if (!String.IsNullOrWhiteSpace(row["STRDAT_0"].ToString()))
                        {
                            of_cherche.dateDebut = Convert.ToDateTime(row["STRDAT_0"].ToString());
                        }
                        if (!String.IsNullOrWhiteSpace(row["SHIDAT_0"].ToString()))
                        {
                            of_cherche.dateExpe = Convert.ToDateTime(row["SHIDAT_0"].ToString());
                        }
                        else
                        {
                            of_cherche.dateExpe = of_cherche.dateDebut;
                        }

                        of_cherche.poste = row["EXTWST_0"].ToString();
                        of_cherche.quantite = (int)Convert.ToDouble(row["EXTQTY_0"].ToString());
                        of_cherche.numCommande = row["VCRNUMORI_0"].ToString();
                        of_cherche.Description = row["MFGDES_0"].ToString();
                        double duree_heures = Convert.ToDouble(row["EXTOPETIM_0"].ToString());
                        of_cherche.duree = duree_heures * 60;

                        var q = ListPostes.Where(i => i.libelle.Trim().Equals(of_cherche.poste.Trim()));

                        if (q.Count() > 0)
                        {
                            of_cherche.couleur = q.First().couleur;
                        }

                        break;
                    }
                }

                if (of_cherche != null)
                {
                    if (of.Etat == 1) // OF disponible et prêt
                    {

                        int nb_op = oF_PROD_TRAITEs.Where(p => p.STATUSTYPE.Equals("INPROGRESS") && p.ILOT != null && p.ILOT.Equals(of_cherche.poste)).Count();

                        if ((nb_op == 0) || !string.IsNullOrWhiteSpace(ofCherche))
                        {
                            liste_of.Add(of_cherche);
                        }
                    }
                }

            }

            //var liste_of_a_faire = liste_of.Where(i => i.rupture == false).Take(3);
            var liste_of_a_faire = liste_of.Take(3);

            return liste_of_a_faire.ToList();
        }
    }
}