using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace GenerateurDFUSafir.Models
{
    public class PlanificationOf
    {

        // Liste de tout les ofs
		public List<OFView> ListOf { get; set; }

        // Liste de tout les postes du pole actuel
        public List<POSTES> ListPostes { get; set; }

        public Dictionary<POSTES, string> listOp { get; set; }

        // Liste de tout les poles
        public List<POLES> ListPoles { get; set; }

        // Liste des ofs planifiés
        public List<OFView> ListOfPlan { get; set; }

        public int nbPersPole { get; set; }



        public PlanificationOf(int Pole, DateTime date)
        {
            string demande = "";
            bool oldof = false;
            if (date < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
            {
                demande = "ENCOURS SOLDE";
                oldof = true;
            }
            else
            {
                demande = "EDITE";
                oldof = false;
            }
            ListOf = new List<OFView>();
            ListOfPlan = new List<OFView>();
            listOp = new Dictionary<POSTES, string>();

            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();

            // On récupère le nombre de personne sur le pole actuellement
            List<OPERATEURS> list_op = db.OPERATEURS.Where(i => i.POLE != null && i.SERVICE.Contains("PROD") && (i.FINCONTRAT == null || i.FINCONTRAT > DateTime.Now)).ToList();
            nbPersPole = list_op.Where(i => i.POLE!= null && i.POLE == Pole).Count();


            // Récupères la liste des pôles
            var query = db.POLES.Where(i => i.ID == Pole);
            ListPoles = db.POLES.ToList();

            if(query.Count() == 0 ) {
                throw new Exception("Pole inconnu");
            }

            POLES pole = query.First();

            ListPostes = pole.POSTES.ToList();

            // on récupère la liste des personnes sur ce poste
            foreach(POSTES po in ListPostes)
            {
                var qry = db.OF_PROD_TRAITE.Where(p => p.STATUSTYPE.Equals("INPROGRESS") && p.ISALIVE && p.ILOT.Equals(po.libelle)).Select(i => i.OPERATEUR);
                if(qry != null && qry.Count() > 0 )
                {
                    long? ops = qry.First();
                    string prenom = db.OPERATEURS.Where(i => i.ID == ops).Select(i => i.PRENOM).First();
                    listOp.Add(po, prenom);
                }
                else
                {
                    listOp.Add(po, "DISPONIBLE");
                }
            }
            // on charge les of "vivant" ou qui correspondate a la date a afficher

            List<PLANIF_OF> planification = db.PLANIF_OF.Where(p => p.Etat != 3 || (p.DatePlanif == date)).ToList();
            

            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            // on passe les of planifié qui sont soldé dans X3 a l'état 3 
            DataTable rawResult = new DataTable();
            ofProdIieCmd Ofs = new ofProdIieCmd();
            // "EDITE" "ENCOURS" "SOLDE"
            string Liste_IN = "";
            foreach(var of in planification)
            {
                Liste_IN += "'"+of.NumOF + "',";
                    
            }
            Liste_IN += "'F9999999'";

            if (oldof)
            {
                Ofs.RequeteOF(ref rawResult, Liste_IN, demande);
            }
            else
            {
                Ofs.RequeteOF(ref rawResult, demande);
            }
            List<string> listofX3 = new List<string>();
            listofX3 = rawResult.Rows.Cast<DataRow>().Select(p => p["MFGNUM_0"].ToString()).ToList();

            // On décale les ofs non réalisé des jours d'avant
            if (!oldof)
            {
                foreach (PLANIF_OF of in planification)
                {
                    if (!listofX3.Contains(of.NumOF))
                    {
                        of.Etat = 3;
                    }
                    else if (of.Etat != 3 && of.DatePlanif < now)
                    {
                        of.DatePlanif = now;
                        of.Rang = 0;
                    }
                }
            }


            // Récupère la liste des OFs
            // deja recupere plus haut
            

            foreach(DataRow row in rawResult.Rows)
            {
                OFView oFATraite = new OFView();

                // On enregistre chaque OF
                oFATraite.numOF = row["MFGNUM_0"].ToString();
                oFATraite.refIndu = row["ITMREF_0"].ToString();
                oFATraite.rupture = row["ALLSTA_0"].ToString() != "3";
                if (!String.IsNullOrWhiteSpace(row["STRDAT_0"].ToString()))
                {
                    oFATraite.dateDebut = Convert.ToDateTime(row["STRDAT_0"].ToString());
                }
                
                if (!String.IsNullOrWhiteSpace(row["SHIDAT_0"].ToString()))
                {
                    oFATraite.dateExpe = Convert.ToDateTime(row["SHIDAT_0"].ToString());
                    oFATraite.stock = false;
                }
                else
                {
                    oFATraite.dateExpe = oFATraite.dateDebut;
                    oFATraite.stock = true;
                }

                oFATraite.poste = row["EXTWST_0"].ToString();
                oFATraite.quantite = (int)Convert.ToDouble(row["EXTQTY_0"].ToString());
                oFATraite.numCommande = row["VCRNUMORI_0"].ToString();
                oFATraite.Description = row["MFGDES_0"].ToString();
                double duree_heures = 0;
                try
                {
                    duree_heures = Convert.ToDouble(row["EXTOPETIM_0"].ToString());
                } catch
                {

                }

                oFATraite.duree = duree_heures * 60;

                // On regarde si c'est Atex ou quelque chose du genre

                if (row["ITMSTD_0"].ToString().ToUpper().Contains("ATEX"))
                {
                    oFATraite.specs = 1;
                }
                if (row["ITMSTD_0"].ToString().ToUpper().Contains("UL"))
                {
                    oFATraite.specs = 5;
                }

                // On vérifie l'apartenance au pole
                var postes_string = ListPostes.Select(i => i.libelle.Trim());
                if (postes_string.Contains(oFATraite.poste))
                {
                    // Si il appartient au pole choisis on l'ajoute à la liste

                    // On récupère la couleur correspondant au poste
                    var q = ListPostes.Where(i => i.libelle.Trim().Equals(oFATraite.poste.Trim()));

                    if (q.Count() > 0) { 
                        oFATraite.couleur = q.First().couleur;
                    }

                    // On regarde si cet OF est planifié
                    var planification_string = planification.Select(p => p.NumOF);
                    if (planification_string.Contains(oFATraite.numOF))
                    {

                        PLANIF_OF of_planif = planification.Where(i => i.NumOF.Equals(oFATraite.numOF)).First();

                        // On regarde si l'OF est planifié pour la date voulu avant de l'ajouter dans la liste à afficher
                        if (of_planif.DatePlanif == date)
                        {
                            oFATraite.rang = of_planif.Rang;
                            oFATraite.etat = of_planif.Etat;
                            of_planif.Pole = Pole;
                            if (date.Year == DateTime.Now.Year && date.Month == DateTime.Now.Month && date.Day == DateTime.Now.Day)
                            {
                                // Si c'est la date du jour, on affiche pas les ofs terminés
                                if (of_planif.Etat != 3)
                                {
                                    ListOfPlan.Add(oFATraite);
                                }
                            }
                            else
                            {
                                ListOfPlan.Add(oFATraite);
                            }

                        }
                    }
                    else
                    {
                        ListOf.Add(oFATraite);
                    }
                }
            }
            //ListOf.Sort((a,b)=> (int)( ((DateTime)a.dateExpe).Ticks - ((DateTime)b.dateExpe).Ticks));
            ListOf = ListOf.OrderBy(aa => aa.dateExpe).ToList();
            ListOfPlan.Sort((a, b) => a.rang - b.rang);
            db.SaveChanges();

        }


    }
}