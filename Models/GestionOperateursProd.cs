using GenerateurDFUSafir.DAL;
using GenerateurDFUSafir.Models;
using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Web;


namespace GenerateurDFUSafir.Models
{
    /// <summary>
    /// gestion des operateurs de production uniquement
    /// </summary>
    public static class GestionOperateursProd
    {
        public static OF_PROD_TRAITE AddoF_PROD_TRAITE_FromControl(string NmrOF)
        {
            List<OrdreFabrication> ofproductNonTraite = InfoAllProduction.InfoAllOfProduction().Where(p => p.MFGNUM_0 == NmrOF).ToList();
            if (ofproductNonTraite != null && ofproductNonTraite.Count() > 0)
            {

            }
            return null;
        }
        public static List<DataOperateurProd> GestionTraitementOperateurs(string service)
        {
            List<DataOperateurProd> DataOperateurs = new List<DataOperateurProd>();
            OfX3 data = new OfX3();
            List<OPERATEURS> Operateurs = data.ListOPERATEURs("PROD").Where(p => p.SousService != null && p.SousService.Contains("FAB")).ToList();
            List<OF_PROD_TRAITE> OfTraite = data.ListOFsTraite();
            return InitDataOperateurs(Operateurs, OfTraite);
        }

        public static bool SavePasswordOperateur(long idOperateur, string hashedPassword)
        {
            bool result = false;
            try
            {
                using (PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2())
                {
                    var FirstOP = _db.OPERATEURS
                                     .Include("OPERATEURS_PWD")
                                     .FirstOrDefault(p => p.ID == idOperateur);

                    if (FirstOP != null && FirstOP.OPERATEURS_PWD != null)
                    {
                        FirstOP.OPERATEURS_PWD.Password = hashedPassword;
                        FirstOP.isValidPasswd = true;

                        _db.SaveChanges();
                        result = true;
                    }
                    else
                    {
                        Console.WriteLine("Opérateur ou mot de passe introuvable pour l'ID : " + idOperateur);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.Message);
            }
            return result;
        }

        public static string GetMotDePasseHash(long idOperateur)
        {
            OfX3 data = new OfX3();
            return data.GetPasswordOperateur(idOperateur);
        }





        public static DataOperateurProd GestionOFOperateur(long id, bool timelimit, string numeroOF = null)
        {
            string[] DEFPOSTEBIDIR = new string[] { "A5/01", "C6/00", "P0/01", "P1/01", "P1/02", "P1/03", "P1/99", "P2/01", "P3/01", "A5/01" };
            string[] DEFPOSTEMONO = new string[] { "A5/00", "A1/00", "A2/00", "A3/00", "A4/00", "A6/00", "A5/00", "C1/00", "C5/00", "C3/00", "C3/01", "C3/02", "C3/99", "C4/00" };
            // la baie larysis est attribue au bidir et le banc radio au monodirectionnel
            //string[] DEFPOSTETEST = new string[] { "A5/00", "A5/01" };
            string[] DEFPOSTETEST = new string[] { };

            DataOperateurProd result = new DataOperateurProd();
            OfX3 data = new OfX3();
            OPERATEURS Operateur = data.ListOPERATEURs("PROD").Where(p => p.ID == id).First();
            result.Anniversaire = Operateur.ANNIVERSAIRE;
            result.ID = Operateur.ID;
            result.Nom = Operateur.NOM;
            result.Prenom = Operateur.PRENOM;
            result.Animateur = Operateur.ANIMATEUR;
            result.Initial = Operateur.INITIAL.Trim();
            string photoRelatif = "/operateurs" + Operateur.PATHB;
            string photoPhsyique = HttpContext.Current.Server.MapPath(photoRelatif);
            if (System.IO.File.Exists(photoPhsyique))
            {
                result.Photo = photoRelatif;
            }
            else
            {
                result.Photo = "/operateurs/OperateursA/default.bmp";
            }
            result.Password = Operateur.Password;
            try
            {
                if (Operateur.POLE != null)
                {
                    result.Ilotid = (int)Operateur.POLE;
                }
                else
                {
                    result.Ilotid = 1;
                }
            }
            catch { result.Ilotid = 1; }

            DateTime now = DateTime.Now;
            DateTime nowMoins1 = new DateTime(now.Year, now.Month, now.Day).AddDays(-1);
            // on recupere tt les of non soldé ou soldé dans la journée
            List<OF_PROD_TRAITE> OfTraites = data.ListOFsTraite().Where(p => p.OPERATEUR == id && (!p.STATUSTYPE.Contains("CLOSED") || p.ENDTIME > nowMoins1) && (p.ISALIVE == true)).OrderBy(t => t.NMROF).ToList();
            foreach (var oft in OfTraites)
            {
                oft.DefaultTempsId = returncastemps(oft.TEMPSSUPPL);
                oft.TempsId = returncastemps(oft.TEMPSSUPPL);
            }
            result.OfEncours = OfTraites;

            List<string> of = new List<string>();
            foreach (var of_prod_traite in OfTraites)
            {
                of.Add(of_prod_traite.NMROF);
            }
            result.ToJson();
            // list des OF dispo 
            // liste des of non traite MFGTRKFLG_0
            //1   A En attente
            //2   B A l'étude
            //3   C Edité
            //4   D En cours
            //5   E Soldé
            //6   F Prix de revient calculé
            DateTime nowPlus1 = new DateTime(now.Year, now.Month, now.Day).AddDays(1);
            List<OrdreFabrication> ofproductNonTraite;
            int adddays = 5;
            if (timelimit)
            {
                adddays = 365;
            }
            if (numeroOF != null)
            {
                ofproductNonTraite = InfoAllProduction.InfoAllOfProduction().Where(p => p.MFGNUM_0 == numeroOF).ToList();
            }
            else if (Operateur.POLE != null && Operateur.POLE.Equals(2) && timelimit == false)
            {
                ofproductNonTraite = InfoAllProduction.InfoAllOfProduction().Where(p => (p.MFGTRKFLG_0 == "3" || p.MFGTRKFLG_0 == "4" || p.MFGTRKFLG_0 == "5") && !of.Contains(p.MFGNUM_0) && (p.ENDDAT_0 < nowPlus1.AddDays(adddays)) && PosteProduction.IsPosteIsInPole(p.EXTWST_0, 2)).ToList();
            }
            else if (Operateur.POLE != null && Operateur.POLE.Equals(3) && timelimit == false)
            {
                ofproductNonTraite = InfoAllProduction.InfoAllOfProduction().Where(p => (p.MFGTRKFLG_0 == "3" || p.MFGTRKFLG_0 == "4" || p.MFGTRKFLG_0 == "5") && !of.Contains(p.MFGNUM_0) && (p.ENDDAT_0 < nowPlus1.AddDays(adddays)) && PosteProduction.IsPosteIsInPole(p.EXTWST_0, 3)).ToList();
            }
            else if (Operateur.POLE != null && Operateur.POLE.Equals(4) && timelimit == false)
            {
                //ofproductNonTraite = InfoAllProduction.InfoAllOfProduction().Where(p => (p.MFGTRKFLG_0 == "3" || p.MFGTRKFLG_0 == "4" || p.MFGTRKFLG_0 == "5") && !of.Contains(p.MFGNUM_0) && (p.ENDDAT_0 < nowPlus1.AddDays(adddays)) && DEFPOSTETEST.Contains(p.EXTWST_0)).ToList();
                ofproductNonTraite = InfoAllProduction.InfoAllOfProduction().Where(p => (p.MFGTRKFLG_0 == "3" || p.MFGTRKFLG_0 == "4" || p.MFGTRKFLG_0 == "5") && !of.Contains(p.MFGNUM_0) && (p.ENDDAT_0 < nowPlus1.AddDays(adddays)) && !PosteProduction.IsPosteIsInPole(p.EXTWST_0, 2) && !PosteProduction.IsPosteIsInPole(p.EXTWST_0, 3)).ToList();
            }
            else
            {
                ofproductNonTraite = InfoAllProduction.InfoAllOfProduction().Where(p => (p.MFGTRKFLG_0 == "3" || p.MFGTRKFLG_0 == "4" || p.MFGTRKFLG_0 == "5") && !of.Contains(p.MFGNUM_0)).ToList();//&& (p.ENDDAT_0 < nowPlus1.AddDays(adddays))).ToList();
                //ofproductNonTraite = InfoAllProduction.InfoAllOfProduction().ToList();
            }
            List<OF_PROD_TRAITE> ListOfNontraite = new List<OF_PROD_TRAITE>();

            // trouver les of  appartenant a l'operateur qui ont ete planifié pour le pole
            PlanificationModel planification = new PlanificationModel();

            List<string> ofplanifie = new List<string>();
            if (numeroOF != null)
            {
                ofplanifie.Add(numeroOF.Trim());
            }
            ofplanifie.AddRange(planification.LoadPlanificationOfFromDb(result.ID));
            List<string> ofencoursbyope = new List<string>();
            foreach (var ofnmr in OfTraites)
            {
                ofencoursbyope.Add(ofnmr.NMROF.Trim());
            }

            foreach (var ofclasse in ofplanifie)
            {
                var queryof = ofproductNonTraite.Where(p => p.MFGNUM_0 == ofclasse.Trim());
                if (queryof != null && queryof.Count() > 0)
                {
                    OrdreFabrication ofprod = ofproductNonTraite.Where(p => p.MFGNUM_0 == ofclasse.Trim()).First();
                    //}
                    //foreach (var ofprod in ofproductNonTraite)
                    //{
                    OF_PROD_TRAITE ofprodnew = new OF_PROD_TRAITE();
                    ofprodnew.Alea = "";
                    ofprodnew.ENDTIME = null;
                    //ofprodnew.ID 
                    ofprodnew.ILOT = ofprod.EXTWST_0;
                    ofprodnew.ITEMREF = ofprod.ITMREF_0;
                    ofprodnew.MFGDES = ofprod.MFGDES_0;
                    ofprodnew.NMROF = ofprod.MFGNUM_0;
                    ofprodnew.OPERATEUR = null;
                    ofprodnew.QTRREEL = 0;
                    ofprodnew.QTRTHEORIQUE = (int)(Convert.ToDecimal(ofprod.EXTQTY_0, new CultureInfo("fr-FR")));
                    ofprodnew.STARTTIME = null;
                    ofprodnew.STATUSTYPE = "NONTRAITE";
                    ofprodnew.ENDTIMETHEORIQUE = ofprod.ENDDAT_0;
                    ofprodnew.TEMPSTHEORIQUE = Convert.ToSingle(ofprod.EXTUNTTIM_0);
                    ofprodnew.TCLCOD_0 = ofprod.TCLCOD_0;// le type de produit  produit fini ou semi fini
                    if ((ofplanifie.Contains(ofprodnew.NMROF) && !ofencoursbyope.Contains(ofprodnew.NMROF)) || (numeroOF != null))
                    {
                        ListOfNontraite.Add(ofprodnew);
                    }
                }
            }
            //ofplanifie est ordonne, ListOfNontraite n'est pas ordonee

            result.OfNontraiteNontrace = ListOfNontraite;
            return result;
        }
        private static List<DataOperateurProd> InitDataOperateurs(List<OPERATEURS> Operateurs, List<OF_PROD_TRAITE> OfTraite)
        {
            List<DataOperateurProd> DataOperateurs = new List<DataOperateurProd>();
            foreach (var operateur in Operateurs)
            {
                DataOperateurProd dataOperateur = new DataOperateurProd();
                dataOperateur.ID = operateur.ID;
                dataOperateur.Anniversaire = operateur.ANNIVERSAIRE;
                dataOperateur.Nom = operateur.NOM;
                dataOperateur.Prenom = operateur.PRENOM;
                dataOperateur.isValidPasswd = (bool)operateur.isValidPasswd;
                dataOperateur.Animateur = operateur.ANIMATEUR;
                dataOperateur.Initial = operateur.INITIAL.Trim();
                string photoRelatif = "/operateurs" + operateur.PATHB;
                string photoPhysique = HttpContext.Current.Server.MapPath(photoRelatif);
                if (System.IO.File.Exists(photoPhysique))
                {
                    dataOperateur.Photo = photoRelatif;
                }
                else
                {
                    dataOperateur.Photo = "/operateurs/OperateursA/default.bmp";
                }
                DataOperateurs.Add(dataOperateur);
                dataOperateur.OfEncours = new List<OF_PROD_TRAITE>();
                DateTime nowmoins1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-1);
                foreach (var of in OfTraite)
                {
                    if (of.OPERATEUR == operateur.ID && (!of.STATUSTYPE.Contains("CLOSED") || of.ENDTIME > nowmoins1))
                    {
                        dataOperateur.OfEncours.Add(of);
                    }
                }
            }
            return DataOperateurs;
        }

        public static int returncastemps(double temps)
        {
            //new SelectListItem { Text = "0 mn", Value = "0" },
            //        new SelectListItem { Text = "5 mn", Value = "2" },
            //        new SelectListItem { Text = "10 mn", Value = "3" },
            //        new SelectListItem { Text = "15 mn", Value = "4" },
            //        new SelectListItem { Text = "20 mn", Value = "5" },
            //        new SelectListItem { Text = "25 mn", Value = "6" },
            //        new SelectListItem { Text = "30 mn", Value = "7" },
            //        new SelectListItem { Text = "40 mn", Value = "8" },
            //        new SelectListItem { Text = "50 mn", Value = "9" },
            //        new SelectListItem { Text = "1 h", Value = "10" },
            //        new SelectListItem { Text = "1 h 30 mn", Value = "11" },
            //        new SelectListItem { Text = "2 h", Value = "12" }

            if (temps >= (double)2) { return 11; }
            else if (temps >= (double)1.5) { return 10; }
            else if (temps >= (double)1) { return 9; }
            else if (temps >= ((double)50 / 60)) { return 8; }
            else if (temps >= ((double)40 / 60)) { return 7; }
            else if (temps >= ((double)30 / 60)) { return 6; }
            else if (temps >= ((double)25 / 60)) { return 5; }
            else if (temps >= ((double)20 / 60)) { return 4; }
            else if (temps >= ((double)15 / 60)) { return 3; }
            else if (temps >= ((double)10 / 60)) { return 2; }
            else if (temps >= ((double)5 / 60)) { return 1; }
            else { return 0; }
        }
        public static double returnTimeCas(int cas)
        {
            switch (cas)
            {
                case 0:
                    return 0;
                    break;
                case 1:
                    return (double)5 / 60;
                    break;
                case 2:
                    return (double)10 / 60;
                    break;
                case 3:
                    return (double)15 / 60;
                    break;
                case 4:
                    return (double)20 / 60;
                    break;
                case 5:
                    return (double)25 / 60;
                    break;
                case 6:
                    return (double)30 / 60;
                    break;
                case 7:
                    return (double)40 / 60;
                    break;
                case 8:
                    return (double)50 / 60;
                    break;
                case 9:
                    return (double)1;
                    break;
                case 10:
                    return (double)1.5;
                    break;
                case 11:
                    return (double)2;
                    break;
                default:
                    return (double)0;
                    break;
            }
        }
    }
}