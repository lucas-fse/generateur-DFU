using GenerateurDFUSafir.DAL;

using GenerateurDFUSafir.Models;
using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Util;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using System.Data;
using System.Web.WebPages;
using System.Web.Security;

namespace GenerateurDFUSafir.Controllers
{
    public class ProductionController : Controller
    {
        public ActionResult GestionOutilAdmin(long? ID)
        {
            if (ID == null)
            {
                return RedirectToAction("Production", "Production");
            }
            DataOperateurProd ope = GestionOperateursProd.GestionOFOperateur((long)ID, false);

            return View(ope);
        }
        public ActionResult DeclAccident()
        {
            SECU_PROD vue = new SECU_PROD();
            return View(vue);
        }

        public ActionResult Proposition()
        {
            InfoAmelioration vue = new InfoAmelioration();
            return View(vue);
        }

        public ActionResult SuppressionOFBidir(long? id)
        {
            GestionOutilAdmin vue = new GestionOutilAdmin();
            if (id == null)
            {
                return RedirectToAction("Production", "Production");
            }
            else
            {
                vue.ID = (long)id;
            }
            return View(vue);
        }
        [HttpPost, ActionName("SuppressionOFBidir")]
        public ActionResult SuppressionOFBidir(string id)
        {
            uint? ID = null;
            ID = UInt32.Parse(Request.Form["ID"]);
            string ofASupprimer = Request.Form["numOF"];
            GestionOutilAdmin goa = new GestionOutilAdmin();
            bool resultsuppr = goa.ExecuteSuppressionOF(ofASupprimer);
            if (resultsuppr)
            {
                return RedirectToAction("gestionOf", "Production", new { id = ID });
            }
            else
            {
                return RedirectToAction("SuppressionOFBidir", "Production", new { id = ID });
            }
        }
        public ActionResult SuppressionOfKepler(long? id)
        {
            if (id == null)
            {
                return RedirectToAction("Production", "Production");
            }
            else
            {
                ViewBag.resultView = false;
                List<TracaPack> listraca = new List<TracaPack>();
                TracaPackOPE traca = new TracaPackOPE();
                traca.IDOpe = id;
                traca.tracaPacks = listraca;
                return View(traca);
            }
        }

        [HttpPost, ActionName("SuppressionOfKepler")]
        public ActionResult SuppressionOfKeplerSearch(long id, long? IDOPE)
        {
            ViewBag.resultView = true;
            string numof = Request.Form["numof"].ToUpper();
            string numordre = Request.Form["numordre"];
            GestionTracaProd traca = new GestionTracaProd();
            List<TracaPack> listraca = traca.GetInfo(numof, numordre);
            TracaPackOPE tracaV = new TracaPackOPE();
            tracaV.IDOpe = IDOPE;
            tracaV.tracaPacks = listraca;
            ViewBag.NumOF = numof;


            return View(tracaV);
        }
        // Contrôleur pour la suppression des lignes d'un OF kepler
        [ActionName("SupprLigneKepler")]
        public ActionResult SupprKepler(long ID, string Gamme, long? IDOPE)
        {
            GestionTracaProd tracaP = new GestionTracaProd();
            bool result = tracaP.DeleteID(Gamme, ID, IDOPE);
            if (result)
            {
                TempData["Message"] = "Ligne supprimée avec succès";
                TempData["MessageType"] = "success";
                return RedirectToAction("gestionOf", "PRODUCTION", new { id = IDOPE });
            }
            else
            {
                TempData["Message"] = "Erreur lors de la suppression de la ligne.";
                TempData["MessageType"] = "error";
                List<TracaPack> listraca = new List<TracaPack>();
                TracaPackOPE traca = new TracaPackOPE();
                traca.IDOpe = IDOPE;
                traca.tracaPacks = listraca;
                return View(traca);
            }
        }
        // reimpression des etiquette kepler
        [ActionName("ImpressionLigneKepler")]
        public ActionResult ImpressionKepler(long ID, string Gamme, long? IDOPE)
        {
            GestionTracaProd tracaP = new GestionTracaProd();
            bool result = tracaP.PrintEtiquette(ID, Gamme);
            if (result)
            {
                TempData["Message"] = "Etiquette imprimée avec succès.";
                TempData["MessageType"] = "success";
                return RedirectToAction("gestionOf", "PRODUCTION", new { id = IDOPE });
            }
            else
            {
                TempData["Message"] = "Erreur lors de l'impression de l'étiquette";
                TempData["MessageType"] = "error";
                List<TracaPack> listraca = new List<TracaPack>();
                TracaPackOPE traca = new TracaPackOPE();
                traca.IDOpe = IDOPE;
                traca.tracaPacks = listraca;
                return View(traca);
            }
        }
        public ActionResult DeclaNonConform(long? ID)
        {
            if (ID == null)
            {
                return RedirectToAction("Production", "Production");
            }
            DataOperateurProd ope = GestionOperateursProd.GestionOFOperateur((long)ID, false);
            return View(ope);

        }

        [HttpPost, ActionName("DeclaNonConform")]
        public ActionResult SaveDeclaNonConform()
        {
            int ID = Int32.Parse(Request.Form["ID"]);
            string item = Request.Form["refPiece"];
            int quantite = Int32.Parse(Request.Form["nbPiece"]);
            string nom = Request.Form["nomArticle"];
            string numOF = Request.Form["OF"];
            string descriptionNC = Request.Form["description"];

            // TODO : Effectuer les vérifications

            GestionNonConformite gnc = new GestionNonConformite();
            NON_CONFORMITE newnc = new NON_CONFORMITE();
            newnc.OperateursID = ID;
            newnc.Item = item;
            newnc.Qtr = quantite;
            newnc.DescriptionUser = descriptionNC;
            newnc.DescriptionItem = nom;
            newnc.NmrOF = numOF;

            try
            {
                string cptnc = gnc.AddNonConformite(newnc);

                if (!string.IsNullOrWhiteSpace(cptnc))
                {
                    newnc.NmrChronoS = cptnc;
                    ImprimerEtiquetteNC(newnc);



                    Mail mail = new Mail();
                    mail.From = "iisProd.Conductix@laposte.net";
                    mail.To = "franck.moisy@conductix.com,Louis.Jeanningros@conductix.com,wilfrid.martin@conductix.com,iisProd.Conductix@laposte.net";

                    mail.Subject = "Déclaration de non conformité : " + newnc.NmrChronoS;
                    mail.Message = "Nonconformité : " + newnc.NmrChronoS + "\r\n" +
                                    "Article : " + newnc.Item + "\r\n" +
                                    "Nmr OF: " + newnc.NmrOF + "\r\n" +
                                    newnc.DescriptionItem + "\r\n" +
                                   "qtr : " + newnc.Qtr.ToString() + "\r\n" +
                                   "Description : " + newnc.DescriptionUser + "\r\n";

                    mail.btnSendMail();

                    TempData["Message"] = "Déclaration de non-conformité enregistrée avec succès.";
                    TempData["MessageType"] = "success";
                }
                else
                {
                    TempData["Message"] = "Une erreur est survenue lors de l'enregistrement de la non-conformité.";
                    TempData["MessageType"] = "error";
                }
            } catch (Exception ex)
            {
                TempData["Message"] = "Une erreur technique est survenue";
                TempData["MessageType"] = "error";
            }

            if (!numOF.Equals("MANUEL"))
            {
                return RedirectToAction("OfNumeric", "Production", new { ID = ID, nmrof = numOF });
            }

            return RedirectToAction("GestionOf", "Production", new { ID = ID });
        }
        public ActionResult AffectationOperateurs()
        {
            InfoSerenite vue = new InfoSerenite("");

            return View(vue);
        }
        [HttpPost]
        public ActionResult AffectationOperateurs(long? ID, string idPole)
        {
            if (ID == null || string.IsNullOrWhiteSpace(idPole))
            {
                return Json(new { status = "error", message = "Paramètres invalides." }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                using (var db = new PEGASE_PROD2Entities2())
                {
                    var op = db.OPERATEURS.FirstOrDefault(i => i.ID == ID.Value);
                    if (op == null)
                    {
                        return Json(new { status = "error", message = "Opérateur non trouvé." }, JsonRequestBehavior.AllowGet);
                    }

                    op.POLE = Convert.ToInt32(idPole);
                    db.SaveChanges();

                    return Json(new { status = "success", message = "Affectation enregistrée." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // Tu peux loguer l'erreur ici avec un logger
                return Json(new { status = "error", message = "Erreur serveur : " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult ChangeAnimatrice()
        {
            var name2 = Request.Form["p.Animateur"];
            var pole = Request.Form["IDPOLE"];
            try
            {
                int poleint = Convert.ToInt32(pole);
                int Animatriceint = Convert.ToInt32(name2);
                PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();
                var querypole = db.POLES.Where(i => i.ID == poleint);
                if (querypole != null && querypole.Count() > 0)
                {
                    POLES poles = querypole.First();
                    poles.Animateur = Animatriceint;
                    db.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("AffectationOperateurs", "Production", null);
        }
        public ActionResult PlanificationOf(int? Pole, DateTime? date)
        {
            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();
            List<long> pole = db.POLES.Select(i => i.ID).ToList();
            if (Pole == null || !pole.Contains((long)Pole)) { Pole = (int)pole.First(); }
            if (date == null) date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            PlanificationOf vue = new PlanificationOf((int)Pole, (DateTime)date);

            return View(vue);
        }
        [HttpPost, ActionName("SavePlanifOF")]
        public ActionResult SavePlanification(int pole, DateTime? date)
        {

            if (date == null) date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            // Récupération de la requête JSON
            Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();

            // On déserialiser le contenu de la requête
            JavaScriptSerializer js = new JavaScriptSerializer();
            var ofs_planifie = js.Deserialize<List<OfPlan>>(json);

            //var t = ofs_planifie[0][0];

            // On gère la sauvegarde dans la bdd
            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();

            // On commence par récupérer la planification actuelle
            List<PLANIF_OF> planif_act = db.PLANIF_OF.Where(i => i.Pole == pole && i.DatePlanif == date && i.Etat != 3).ToList();

            // On regarde tout les ofs qui ont été déplanifié
            //List<PLANIF_OF> planif_a_supprimer = new List<PLANIF_OF>();
            foreach (PLANIF_OF pla in planif_act)
            {
                List<string> liste_of = ofs_planifie.Select(i => i.numof).ToList();
                if (!liste_of.Contains(pla.NumOF))
                {
                    db.PLANIF_OF.Remove(pla);
                }
            }

            foreach (var of in ofs_planifie)
            {
                PLANIF_OF of_p;
                // On regarde s'il existe déjà
                var query = planif_act.Where(i => i.NumOF.Equals(of.numof));
                // UPDATE
                if (query != null && query.Count() > 0)
                {
                    of_p = query.First();

                    of_p.Rang = of.rang;

                }
                // CREATE
                else
                {
                    of_p = new PLANIF_OF();
                    of_p.Pole = pole;
                    of_p.NumOF = of.numof;
                    of_p.DatePlanif = date;
                    of_p.Rang = of.rang;
                    of_p.Etat = 1;

                    db.PLANIF_OF.Add(of_p);
                }
            }
            db.SaveChanges();

            return Json("ok", JsonRequestBehavior.AllowGet);

        }
        [ActionName("ApiGetNomenclature")]
        public ActionResult getNomenclatureByOf(string numof)
        {
            OfProdModel data = new OfProdModel();
            data.OfProdModelFull(numof);

            return Json(new { list = data.ofProdModelInfo.listeAServirs, desc = data.ofProdModelInfo.OFItmDescrip1 }, JsonRequestBehavior.AllowGet);
        }

        [ActionName("startOf")]
        public ActionResult startOf(long? ID, string of)
        {
            //GestionOperateursProd.GestionOFOperateur((long) ID, false, of);




            DataOperateurProd dataoperateur = GestionOperateursProd.GestionOFOperateur((long)ID, false, of);
            var tmp1 = dataoperateur.OfEncours.Where(p => p.NMROF == of && p.ISALIVE == true && p.STATUSTYPE.Contains("CLOSED"));
            var tmp = dataoperateur.OfNontraiteNontrace.Where(p => p.NMROF.Contains(of));
            if (tmp1 != null && tmp1.Count() == 1)
            {
                OF_PROD_TRAITE of_traite = tmp1.First();
                OfX3 db = new OfX3();
                if (of_traite.STATUSTYPE.Contains("CLOSED"))
                {

                    of_traite.STATUSTYPE = "INPROGRESS";
                    of_traite.ENDTIME = DateTime.Now;
                    db.SaveDbOF_PROD_TRAITE(of_traite);

                }
                db.SaveDbOF_PLANIF_OF(of_traite.NMROF, 2);
            }
            else if (tmp != null && tmp.Count() > 0)
            {
                OF_PROD_TRAITE of_traite = tmp.First();
                of_traite.STATUSTYPE = "INPROGRESS";
                of_traite.STARTTIME = DateTime.Now;
                of_traite.OPERATEUR = ID;
                of_traite.ISALIVE = true;
                OfX3 db = new OfX3();

                db.SaveDbOF_PROD_TRAITE(of_traite);
                foreach (var of2 in dataoperateur.OfEncours.Where(p => p.OPERATEUR == ID && p.STATUSTYPE == "INPROGRESS" && p.ID != of_traite.ID))
                {
                    of2.STATUSTYPE = "ENPAUSE";
                    of2.ENDTIME = DateTime.Now;
                    db.SaveDbOF_PROD_TRAITE(of2);
                }
                db.SaveDbOF_PLANIF_OF(of_traite.NMROF, 2);
            }

            ViewBag.close = true;

            //return RedirectToAction("PropoOf", "Production", new { pole = 2 });
            return RedirectToAction("gestionOF", "Production", new { id = (long)ID });
        }
        public ActionResult AnalyseOF()
        {
            InfoOf vue = new InfoOf(0);
            return View(vue);
        }
        public ActionResult OfNumeric(string nmrof, long? ID)
        {
            OfProdModel vue = new OfProdModel();
            vue.OfProdModelFull(nmrof);
            vue.ofProdModelInfo.ID = ID;

            return View(vue.ofProdModelInfo);
        }



        public ActionResult OFEdite(string Of)
        {
            OfProdModel ofedite = new OfProdModel();
            ofedite.OfProdModelFull(Of);
            return View(ofedite);
        }
        public ActionResult OutilProdLog(long? ID)
        {
            EtiquetteLogistique et = null;
            if (et == null)
            {
                et = new EtiquetteLogistique();
            }
            et.id = ID;
            //System.Diagnostics.EventLog.WriteEntry("GenerateurDFUSource", "Requete Index" + "Message");
            return View(et);
        }

        [HttpPost, ActionName("OutilProdLog")]
        public ActionResult ImpressionEtiquetteLogistique(EtiquetteLogistique et1)
        {
            EtiquetteLogistique et = null;
            bool result = false;
            if (et1 == null || et1.Itmeref == null || et1.Qtr == null)
            {
                TempData["Message"] = "Référence ou quantité manquante.";
                TempData["MessageType"] = "error";
                et = new EtiquetteLogistique();
            }
            else
            {
                try
                {
                    if (et1.Lot == null) { et1.Lot = ""; }
                    int cpt = Convert.ToInt32(et1.Qtr);
                    //using (Impersonation imp = new Impersonation("product", "jayelectronique", "2017Pr01"))
                    //{
                    result = et1.PrintEtiquetteQTR();
                    // }
                    if (!result)
                    {
                        TempData["Message"] = "Echec de l'impression de l'étiquette";
                        TempData["MessageType"] = "error";
                        et1 = new EtiquetteLogistique();
                    }
                    else
                    {
                        TempData["Message"] = "Etiquette imprimée avec succès";
                        TempData["MessageType"] = "success";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Echec lors de l'impression";
                    TempData["MessageType"] = "error";
                    et1 = new EtiquetteLogistique();
                }


            }
            if (result)
            {
                return RedirectToAction("gestionOf", "Production", new { id = et1.id });
            }
            else
            {
                return View(et1);
            }
        }
        public ActionResult OutilProd(long? id)
        {
            EtiquetteLogistique et = null;
            if (et == null)
            {
                et = new EtiquetteLogistique();
                using (PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2())
                {
                    et.OFdeIDList = new List<string>();
                    if (id != null)
                    {
                        IQueryable<OF_PROD_TRAITE> queryoftraite = _db.OF_PROD_TRAITE.Where(p => p.ISALIVE != false && p.STATUSTYPE != "CLOSED" && p.OPERATEUR != null && p.OPERATEUR == id);

                        foreach (var of in queryoftraite.ToList())
                        {
                            if (!et.OFdeIDList.Contains(of.NMROF) && !of.NMROF.Contains("ANIMATION"))
                            {
                                et.OFdeIDList.Add(of.NMROF);
                            }
                        }
                    }
                    else
                    {
                        et.OFdeIDList = new List<string>();
                    }
                }
            }
            //System.Diagnostics.EventLog.WriteEntry("GenerateurDFUSource", "Requete Index" + "Message");
            return View(et);
        }
        [HttpPost, ActionName("OutilProd")]
        public ActionResult ImpressionEtiquette(EtiquetteLogistique et1)
        {
            EtiquetteLogistique et = null;
            var name = "";
            var name2 = Request.Form["numOF"];
            var name1 = Request.Form["submitButton"];
            if (!string.IsNullOrWhiteSpace(name1))
            {
                name = name1;
            }
            else
            {
                name = name2;
            }

            bool result = false;

            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["Message"] = "Aucun numéro OF fourni.";
                TempData["MessageType"] = "error";
                et = new EtiquetteLogistique();
                et.id = et1.id;
            }
            else
            {
                string ofacherche = name.ToUpper();

                OrdreFabrication op = FinfOfAllX3(ofacherche);
                if (op != null && !string.IsNullOrWhiteSpace(op.ITMREF_0))
                {
                    OfX3 ofs = new OfX3();
                    List<string> casdemploi = ofs.ListCasDEmploi("354140A");
                    if (casdemploi.Contains(op.ITMREF_0))
                    {
                        try
                        {
                            ImprimerEtiquette(op);
                            TempData["Message"] = $"Étiquette imprimée pour l'OF {op.MFGNUM_0}.";
                            TempData["MessageType"] = "success";
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            TempData["Message"] = "Erreur lors de l'impression : " + ex.Message;
                            TempData["MessageType"] = "error";
                        }
                    }
                    else
                    {
                        TempData["Message"] = $"La référence {op.ITMREF_0} ne permet pas l'impression de l'étiquette.";
                        TempData["MessageType"] = "error";
                    }
                }
                else
                {
                    TempData["Message"] = "Ordre de fabrication introuvable ou invalide.";
                    TempData["MessageType"] = "error";
                }
            }

            if (result && et1 != null && et1.id != null)
            {
                return RedirectToAction("gestionOf", "Production", new { id = et1.id });
            }
            else
            {
                return RedirectToAction("outilProd", "Production", new { id = et1?.id });
            }
        }
        public ActionResult OutilProdBac(long? IDop, string bac)
        {
            ITEM_LOCALISATION et = null;
            if (et == null)
            {
                List<ITEM_LOCALISATION> list = EtiquetteBac.GetEtiquetteBac(bac);
                if (list != null && list.Count() == 1)
                {
                    et = list.First();
                }
                else
                {
                    et = new ITEM_LOCALISATION();
                }
                et.idop = IDop;
            }
            return View(et);
        }
        [HttpPost, ActionName("OutilProdBac")]
        public ActionResult ImpressionOutilProdBac(string bac)
        {
            var opID = Request.Form["IDOp"];
            var nameID = Request.Form["IdRef"];
            var nameRef = Request.Form["refPiece"];
            var namechar = Request.Form["chariotpos"];
            var nameqtr = Request.Form["nbPiece"];
            ITEM_LOCALISATION et = null;
            try
            {
                et = EtiquetteBac.SetEtiquetteBac(nameID, nameRef, namechar, nameqtr);
                ImprimerEtiquette(et, false);
                    //pour impression en volume de toute les étiquettes
                    List<ITEM_LOCALISATION> tmp = EtiquetteBac.GetAllList();
                int i = 0;
                //List<string> list = new List<string> { "E26270B", "012174", "015324", "D30270C", "D30260C", "D30250C", "015333", "015034", "015312", "E26690TEST", "000252", "000149", "015218", "015262", "015028", "012712", "015266", "D30160B", "350910A", "M15200A", "013432", "351330A" };
                //List<string> list = new List<string> { "353840A", "015325", "351330A" };
                //foreach (var etiquette in tmp)
                //{
                //    //if (list.Contains(etiquette.ITEMREF))
                //    if (etiquette.ID> 1180)
                //    {
                //        i++;
                //        ImprimerEtiquette(etiquette, true);
                //        //if (i == 50)
                //        //{

                //        //}
                //    }
                //}
                if (et != null)
                {
                    TempData["Message"] = "Etiquette bac générée avec succès.";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("outilProdBac", "Production", new { IDop = Convert.ToUInt64(opID), bac = et.ITEMREF });
                }
                else
                {
                    TempData["Message"] = "Echec de la génération de l'étiquette";
                    TempData["MessageType"] = "error";
                    return RedirectToAction("outilProdBac", "Production", new { IDop = Convert.ToUInt64(opID), bac = "" });
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Erreur lors de la génération de l'étiquette";
                TempData["MessageType"] = "error";
                return RedirectToAction("outilProdBac", "Production", new { IDop = Convert.ToUInt64(opID), bac = "" });
            }
        }
        public ActionResult SearchBacControle(string itemref)
        {
            List<ITEM_LOCALISATION> Bacinfo = EtiquetteBac.GetEtiquetteBac(itemref);

            return Json(Bacinfo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Qualite()
        {

            FRC_PROD frc = new FRC_PROD();
            frc.FRC_PROD_Update();
            return View(frc);
        }
        //GET: STCTCS
        public ActionResult Securite()
        {
            SECU_PROD vue = new SECU_PROD();
            return View(vue);
        }
        [HttpPost, ActionName("Securite")]
        public ActionResult AjoutAccident(HttpPostedFileBase ImageAccident)
        {
            SECU_PROD obj = new SECU_PROD();
            if (Request.Form.Count > 0)
            {
                string SaisieNiveau = Request.Form["TypeAccident"];
                int Niveausaisie = Convert.ToInt32(SaisieNiveau);
                string SaisieDate = Request.Form["SaisieDate"];
                string SaisieDescription = Request.Form["SaisieDescription"];
                string SaisieDescription2 = Request.Form["SaisieDescription2"];
                string SaisieVictime = Request.Form["SaisieVictime"];
                string gravitepotentiel = Request.Form["Gravite"];
                string PhotoAccident = Request.Form["srcImg"];
                string ImageDB = null;

                if (String.IsNullOrWhiteSpace(gravitepotentiel)) { gravitepotentiel = "non renseigné"; }

                if (ImageAccident != null && ImageAccident.ContentType.Contains("image/jpeg"))
                {
                    byte[] thePictureAsBytes = new byte[ImageAccident.ContentLength];
                    using (BinaryReader theReader = new BinaryReader(ImageAccident.InputStream))
                    {
                        thePictureAsBytes = theReader.ReadBytes(ImageAccident.ContentLength);
                    }
                    ImageDB = Convert.ToBase64String(thePictureAsBytes);
                }
                else if (!String.IsNullOrWhiteSpace(PhotoAccident))
                {
                    PhotoAccident = PhotoAccident.Remove(0, 23);
                    ImageDB = PhotoAccident;
                }

                string pathimage = "";
                int result = obj.AddAccident(SaisieNiveau, SaisieDate, SaisieDescription, SaisieDescription2, SaisieVictime, "PRODUCTION", gravitepotentiel, ImageDB, ref pathimage);
                if (result > 0)
                {
                    
                    Mail mail = new Mail();
                    mail.From = "iisProd.Conductix@laposte.net";
                    mail.To = "franck.moisy@conductix.com,romain.destaing@conductix.com,christian.fournier@conductix.com,wilfrid.martin@conductix.com,iisProd.Conductix@laposte.net";
                    mail.Subject = "Déclaration Accidents type : " + obj.NiveauAccident(Niveausaisie);
                    mail.Message = "Niveau d'accident : " + "Niveau " + SaisieNiveau + " - " + obj.NiveauAccident(Niveausaisie) + "\r\n" +
                                   "Victime : " + SaisieVictime + "\r\n" +
                                   "Date : " + SaisieDate + "\r\n" +
                                   "Description : " + SaisieDescription + "\r\n" +
                                   "Niveau de risque: " + gravitepotentiel + "\r\n" +
                                   "Description complémentaire :" + SaisieDescription2;
                    mail.AttachementPath = pathimage;
                    mail.btnSendMail();
                    

                    TempData["Message"] = "Accident déclaré avec succès.";
                    TempData["MessageType"] = "success";
                }
                else
                {
                    TempData["Message"] = "Erreur lors de la déclaration de l'accident.";
                    TempData["MessageType"] = "error";
                }
            }
            else
            {
                TempData["Message"] = "Aucune donnée soumise";
                TempData["MessageType"] = "error";
            }

                return RedirectToAction("Securite", "Production");
        }

        [HttpPost]
        public ActionResult SubmitSignalement(HttpPostedFileBase ImageAccident, HttpPostedFileBase ImageProposition, string SignalementType, string SaisieVictime, string SaisieDate, string TypeAccident, string Gravite, string SaisieDescription, string SaisieDescription2, string SaisieEmetteur, string SaisieService, string SaisieSecteur, string srcImg)
        {
            if (string.IsNullOrEmpty(SignalementType))
            {
                Session["Message"] = "Aucun type de signalement sélectionné.";
                Session["MessageType"] = "error";
                return RedirectToAction("GestionOf", "Production");
            }

            if (SignalementType == "accident")
            {
                SECU_PROD obj = new SECU_PROD();
                int Niveausaisie = Convert.ToInt32(TypeAccident);
                string PhotoAccident = srcImg;
                string ImageDB = null;

                if (ImageAccident != null && ImageAccident.ContentType.Contains("image/jpeg"))
                {
                    byte[] thePictureAsBytes = new byte[ImageAccident.ContentLength];
                    using (BinaryReader theReader = new BinaryReader(ImageAccident.InputStream))
                    {
                        thePictureAsBytes = theReader.ReadBytes(ImageAccident.ContentLength);
                    }
                    ImageDB = Convert.ToBase64String(thePictureAsBytes);
                }
                else if (!string.IsNullOrWhiteSpace(PhotoAccident))
                {
                    PhotoAccident = PhotoAccident.Remove(0, 23);
                    ImageDB = PhotoAccident;
                }

                string pathimage = "";
                int result = obj.AddAccident(TypeAccident, SaisieDate, SaisieDescription, SaisieDescription2, SaisieVictime, "PRODUCTION", Gravite ?? "non renseigné", ImageDB, ref pathimage);
                if (result > 0)
                {
                    Mail mail = new Mail
                    {
                        From = "iisProd.Conductix@laposte.net",
                        To = "franck.moisy@conductix.com,romain.destaing@conductix.com,christian.fournier@conductix.com,wilfrid.martin@conductix.com,iisProd.Conductix@laposte.net",
                        Subject = "Déclaration Accidents type : " + obj.NiveauAccident(Niveausaisie),
                        Message = "Niveau d'accident : " + "Niveau " + TypeAccident + " - " + obj.NiveauAccident(Niveausaisie) + "\r\n" +
                                  "Victime : " + SaisieVictime + "\r\n" +
                                  "Date : " + SaisieDate + "\r\n" +
                                  "Description : " + SaisieDescription + "\r\n" +
                                  "Niveau de risque: " + (Gravite ?? "non renseigné") + "\r\n" +
                                  "Description complémentaire :" + SaisieDescription2,
                        AttachementPath = pathimage
                    };
                    mail.btnSendMail();

                    Session["Message"] = "Accident déclaré avec succès.";
                    Session["MessageType"] = "success";
                }
                else
                {
                    Session["Message"] = "Erreur lors de la déclaration de l'accident.";
                    Session["MessageType"] = "error";
                }
            }
            else if (SignalementType == "amelioration")
            {
                InfoAmelioration obj = new InfoAmelioration();
                string PhotoProposition = srcImg;
                string ImageDB = null;

                if (ImageProposition != null && ImageProposition.ContentType.Contains("image/jpeg"))
                {
                    byte[] thePictureAsBytes = new byte[ImageProposition.ContentLength];
                    using (BinaryReader theReader = new BinaryReader(ImageProposition.InputStream))
                    {
                        thePictureAsBytes = theReader.ReadBytes(ImageProposition.ContentLength);
                    }
                    ImageDB = Convert.ToBase64String(thePictureAsBytes);
                }
                else if (!string.IsNullOrWhiteSpace(PhotoProposition))
                {
                    PhotoProposition = PhotoProposition.Remove(0, 23);
                    ImageDB = PhotoProposition;
                }

                string pathimage = "";
                int result = obj.AddAmelioration(SaisieDescription, SaisieDescription2, SaisieEmetteur, SaisieSecteur, SaisieService, SaisieDate, ImageDB, ref pathimage);
                if (result > 0)
                {
                    Mail mail = new Mail
                    {
                        From = "iisProd.Conductix@laposte.net",
                        To = "franck.moisy@conductix.com,romain.destaing@conductix.com,christian.fournier@conductix.com,wilfrid.martin@conductix.com,iisProd.Conductix@laposte.net",
                        Subject = "Proposition d'amélioration",
                        Message = "Service : " + SaisieSecteur + "\r\n" +
                                  "Type d'amélioration : " + SaisieService + "\r\n" +
                                  "Emetteur : " + SaisieEmetteur + "\r\n" +
                                  "Date : " + SaisieDate + "\r\n" +
                                  "Description : " + SaisieDescription + "\r\n" +
                                  "Solution : " + SaisieDescription2,
                        AttachementPath = pathimage
                    };
                    mail.btnSendMail();

                    Session["Message"] = "Demande enregistrée avec succès.";
                    Session["MessageType"] = "success";
                }
                else
                {
                    Session["Message"] = "Erreur lors de l'enregistrement de la demande.";
                    Session["MessageType"] = "error";
                }
            }

            return RedirectToAction("GestionOf", "Production");
        }


        [HttpPost, ActionName("Qualite")]
        public ActionResult traiterFRC()
        {
            FRC_PROD frc = new FRC_PROD();
            List<int> listid = new List<int>();
            foreach (var itemFormulaire in Request.Form) //Pour récupérer les clés et valeurs du formulaire
            {
                string clee = itemFormulaire.ToString(); // index correspondant dans la bdd 
                string valeur = Request.Form[clee];// valeur vrai ou faux
                try
                {
                    if (valeur.Contains("on"))
                    {
                        listid.Add(Convert.ToInt32(clee));
                    }
                }
                catch { }
            }
            frc.Check(listid);
            frc.FRC_PROD_Update();
            //return View(frc);
            return RedirectToAction("Qualite");
        }
        public ActionResult Serenite()
        {
            InfoSerenite vue = new InfoSerenite();
            return View(vue);
        }
        [HttpPost, ActionName("Serenite")]
        public ActionResult AjoutSerenite(string submitButton)
        {
            int? id;
            int? level;
            InfoSerenite obj = new InfoSerenite();
            try
            {
                id = Convert.ToInt32(Request.Form["IdSalarie"]);
                level = Convert.ToInt32(submitButton);
            }
            catch
            {
                id = null;
                level = null;
            }
            obj.Avoter(id, level);
            obj.CalculSerenite();
            return RedirectToAction("Serenite");
        }

        //GET: 
        public ActionResult Production()
        {

            Production production = new Production();
            return View(production);
        }

        public ActionResult ReloadTimer(string id)
        {
            DateTime now = DateTime.Now;
            string rawId = Request.UserHostName;
            string info = Dns.GetHostEntry(Request.UserHostAddress).HostName.ToString();
            string pcProduct = Resource1.PCPRODUCTION;
            if (info.Contains(pcProduct))
            {


                if (now.Hour >= 7 && now.Hour < 9)
                {
                    return RedirectToAction("Serenite");
                }
                else
                {
                    return RedirectToAction("production");
                }
            }
            else
            {
                return RedirectToAction(id);
            }
        }

        public ActionResult Amelioration()
        {

            InfoAmelioration vue = new InfoAmelioration();
            return View(vue);
        }

        [HttpPost, ActionName("Amelioration")]
        public ActionResult AjoutAmelioration(HttpPostedFileBase ImageProposition)
        {
            string Proposition = Request.Form["SaisieDescription"];
            string Solution = Request.Form["SaisieDescription2"];
            string Emetteur = Request.Form["SaisieEmetteur"];
            string Service = Request.Form["SaisieService"];
            string Date = Request.Form["SaisieDate"];
            string PhotoProposition = Request.Form["srcImg"];
            string ImageDB = null;
            string Secteur = Request.Form["SaisieSecteur"];
            InfoAmelioration obj = new InfoAmelioration();


            if (ImageProposition != null && ImageProposition.ContentType.Contains("image/jpeg"))
            {
                byte[] thePictureAsBytes = new byte[ImageProposition.ContentLength];
                using (BinaryReader theReader = new BinaryReader(ImageProposition.InputStream))
                {
                    thePictureAsBytes = theReader.ReadBytes(ImageProposition.ContentLength);
                }
                ImageDB = Convert.ToBase64String(thePictureAsBytes);
            }
            else if (!string.IsNullOrWhiteSpace(PhotoProposition))
            {
                PhotoProposition = PhotoProposition.Remove(0, 23);
                ImageDB = PhotoProposition;
            }
            else { }

            string pathimage = "";
            int result = obj.AddAmelioration(Proposition, Solution, Emetteur, Secteur, Service, Date, ImageDB, ref pathimage);

            if (result > 0)
            {
                
                Mail mail = new Mail();
                mail.From = "iisProd.Conductix@laposte.net";
                mail.To = "franck.moisy@conductix.com,romain.destaing@conductix.com,christian.fournier@conductix.com,wilfrid.martin@conductix.com,iisProd.Conductix@laposte.net";
                mail.Subject = "Proposition d'amélioration ";
                mail.Message = "Service : " + Secteur + "\r\n" +
                               "Type d'amélioration : " + Service + "\r\n" +
                               "Emetteur : " + Emetteur + "\r\n" +
                               "Date : " + Date + "\r\n" +
                               "Description : " + Proposition + "\r\n" +
                               " -" + Solution;
                mail.AttachementPath = pathimage;
                mail.btnSendMail();

                TempData["Message"] = "Demande enregistrée avec succès.";
                TempData["MessageType"] = "success";
            }
            else
            {
                TempData["Message"] = "Erreur lors de l'enregistrement de la demande.";
                TempData["MessageType"] = "error";
            }
                return RedirectToAction("Amelioration", "Production");
        }

        public ActionResult KPIProd()
        {
            InfoKPI kpi = new InfoKPI();
            return View(kpi);
        }

        public ActionResult AIC2(int? id)
        {
            TempData.Keep("Message");
            TempData.Keep("MessageType");
;            if (id == null)
            {
                return RedirectToAction("Production", "Production");
            }
            else
            {
                InfoAIC2 aic2 = new InfoAIC2();
                aic2.PoleDemande = id;
                aic2.UpdateInfoAIC2();
                return View(aic2);
            }
        }
        [HttpPost, ActionName("AIC2")]
        public ActionResult traiterFormulaire()
        {
            int index = 0;
            Dictionary<string, string> listtmp = new Dictionary<string, string>();

            try
            {
                foreach (var itemFormulaire in Request.Form) //Pour récupérer les clés et valeurs du formulaire
                {
                    string clee = itemFormulaire.ToString();
                    string valeur = Request.Form[clee];
                    listtmp.Add(clee, valeur);
                    if (clee.Contains("ID+")) { index = Convert.ToInt32(valeur); }
                }

                InfoAIC2 aic = new InfoAIC2();
                aic.Addligne(listtmp);

                TempData["Message"] = "Ligne enregistrée avec succès";
                TempData["MessageType"] = "success";
            } catch(Exception ex)
            {
                TempData["Message"] = "Erreur lors de l'enregistrement";
                TempData["MessageType"] = "error";

            }
            return RedirectToAction("AIC2", "Production", new {id = index});

        }

        public ActionResult QRQC(int? id)
        {
            InfoQRQC vue = new InfoQRQC();
            vue.UpdateInfoQRQC(id);
            vue.IdQrqc = id;
            return View(vue);
        }
        [HttpPost, ActionName("QRQC")]
        public ActionResult AjoutQRQC()
        {
            try
            {
                string image = Request.Form["image"];
                int leng = image.Length;
                string id = Request.Form["Index"];
                DateTime? DateOuverture;
                try { DateOuverture = DateTime.Parse(Request.Form["DateOuverture"]); } catch { DateOuverture = null; }
                DateTime? DateCloture;
                try { DateCloture = DateTime.Parse(Request.Form["DateCloture"]); } catch { DateCloture = null; }
                DateTime? DateSuivis;
                try { DateSuivis = DateTime.Parse(Request.Form["DateSuivis"]); } catch { DateSuivis = null; }

                string Participants = Request.Form["Participants"];
                string Pilote = Request.Form["Pilote"];
                string Origine = Request.Form["Origine"];
                Dictionary<string, string> SevenQuestion = new Dictionary<string, string>();
                List<string> Questions = new List<string>() { "Q1", "Q2", "Q3", "Q4", "Q5", "Q6", "Q7" };
                foreach (string item in Questions)
                {
                    SevenQuestion.Add(item, Request.Form[item].Trim());
                }
                string DescriptionProcess = Request.Form["DescritpionProcessus"];
                List<ActionImmediate> ActionsImmediat = new List<ActionImmediate>();
                for (int i = 0; i < 6; i++)
                {
                    DateTime? tmpdate = null;
                    String status = null;
                    try { tmpdate = DateTime.Parse(Request.Form["DelaiI_" + i]); } catch { tmpdate = null; }
                    try { status = Request.Form["StatutI_" + i]; } catch { status = null; }

                    ActionsImmediat.Add(new ActionImmediate(Request.Form["ActionImmediat_" + i], Request.Form["PiloteI_" + i], tmpdate, status));
                }
                List<string> Occurrence = new List<string>();
                List<string> NonDetection = new List<string>();
                for (int i = 0; i < 5; i++)
                {
                    Occurrence.Add(Request.Form["PourquoiOccu_" + i].Trim());
                    NonDetection.Add(Request.Form["PourquoiNonD_" + i].Trim());
                }
                List<SolutionDurable> Solutions = new List<SolutionDurable>();
                for (int i = 0; i < 7; i++)
                {
                    DateTime? tmpdate = null;

                    try { tmpdate = DateTime.Parse(Request.Form["Delai_" + i]); } catch { tmpdate = null; }

                    string tmp = Request.Form["Pilote_" + i];
                    string tmp2 = Request.Form["item_" + i];
                    string tmp3 = Request.Form["Action_" + i];
                    string tmp4 = Request.Form["Statut_" + i];

                    Solutions.Add(new SolutionDurable(Request.Form["item_" + i], Request.Form["Action_" + i], Request.Form["Pilote_" + i], tmpdate, Request.Form["Statut_" + i]));
                }
                InfoQRQC obj = new InfoQRQC();
                obj.SaveQRQC(id, DateOuverture, DateCloture, DateSuivis, Participants, Pilote, Origine, SevenQuestion, DescriptionProcess, ActionsImmediat, Occurrence, NonDetection, Solutions, image);
                
                TempData["Message"] = "Demande enregistrée avec succès.";
                TempData["MessageType"] = "success";

            } catch(Exception ex)
            {
                TempData["Message"] = "Erreur lors de l'enregistrement de la demande.";
                TempData["MessageType"] = "error";
            }
                return RedirectToAction("DataQRQC");
        }

        public ActionResult DataQRQC(int? id)
        {
            TempData.Keep("Message");
            TempData.Keep("MessageType");
            InfoDATAQRQC list = new InfoDATAQRQC();
            return View(list);

        }

        public ActionResult IndexOFOperateur()
        {
            var idConnecte = Session["OperateurConnecte"];

            if (idConnecte != null)
            {
                long id = Convert.ToInt64(idConnecte);
                var admin = GestionOperateursProd.GetIsAdmin(id);

                if (!admin)
                {
                    return RedirectToAction("GestionOF", new { id = id });
                }
            }

            GestionTraitementOFs vue = new GestionTraitementOFs();
            return View(vue);
        }

        //Accès à la page Connexion Operateur, situé entre indexOfOperateur et gestionOf
        //Permet de s'authentifier pour avoir accès aux informations de l'opérateur
        
        public ActionResult ConnexionOp(long? id)
        {
            // Si l'identifiant n'est pas précisé, afficher une page ou liste d'opérateurs
            if (id == null)
            {
                GestionTraitementOFs ofs = new GestionTraitementOFs();
                return View("~/Views/Home/Accueil.cshtml", ofs);
            }

            // Si l'utilisateur est déjà connecté et que l'ID correspond
            if (User.Identity.IsAuthenticated && User.Identity.Name == id.ToString())
            {
                long idOp = long.Parse(User.Identity.Name);
                if (GestionOperateursProd.GetIsAdmin(idOp))
                {
                    Session["isAdmin"] = 1;
                }

                return RedirectToAction("GestionOF", new { id = id });
            }

            // Sinon, afficher la page de saisie du mot de passe
            var operateur = GestionOperateursProd.GestionOFOperateur((long)id, false);
            return View("ConnexionOperateur", operateur);
        }


        
        [HttpPost]
        public ActionResult ConnexionOperateur(long id, string motdepasse)
        {
            // Récupère l'opérateur pour affichage (photo, nom, etc.)
            var operateur = GestionOperateursProd.GestionOFOperateur(id, false);

            if (operateur == null)
            {
                GestionTraitementOFs ofs = new GestionTraitementOFs();
                return View("~/Views/Home/Accueil.cshtml", ofs);
            }

            // Récupère le mot de passe haché depuis OPERATEURS_PWD
            var hashStocke = GestionOperateursProd.GetMotDePasseHash(id);

            // Vérifie le mot de passe
            if (!string.IsNullOrEmpty(hashStocke) &&
                BCrypt.Net.BCrypt.Verify(motdepasse, hashStocke))
            {
                Session["OperateurConnecte"] = operateur.ID;
                Session["isAdmin"] = GestionOperateursProd.GetIsAdmin(id) ? 1 : 0;
                return RedirectToAction("GestionOF", new { id = id });
            }

            // Sinon, erreur
            ViewBag.Erreur = "Mot de passe incorrect";
            return View("ConnexionOperateur", operateur);
        }


        //à enlever
        public ActionResult ConnexionAd(long? id)
        {
            if (id == null)
                return RedirectToAction("IndexOFOperateur");

            var operateur = GestionOperateursProd.GestionOFOperateur((long)id, false);

            return View("ConnexionAdmin", operateur); // 
        }

        //à enlever
        [HttpPost]
        public ActionResult ConnexionAdmin(long id, string adminPassword)
        {
            // Mot de passe admin temporaire
            const string motDePasseAdmin = "admin123"; // à remplacer plus tard par une config sécurisée

            if (adminPassword == motDePasseAdmin)
            {
                // on autorise l'accès à l'espace opérateur en tant qu'admin
                Session["OperateurConnecte"] = id;
                Session["EstAdmin"] = true;

                return RedirectToAction("GestionOF", new { id = id });
            }

            // mot de passe incorrect → retour à la page avec message
            var operateur = GestionOperateursProd.GestionOFOperateur((long)id, false);
            ViewBag.ErreurAdmin = "Mot de passe admin incorrect";
            return View("ConnexionAdmin", operateur);
        }

        [HttpGet]
        public ActionResult DefinirMDPParToken(Guid token)
        {
            OfX3 data = new OfX3();
            var pwdToken = data.GetPwdToken(token);

            if (pwdToken == null || (pwdToken.IsUsed ?? false) || pwdToken.ExpirationDate < DateTime.Now)
                return View("LienInvalideOuExpire");

            // Tu peux stocker l’ID en session si besoin
            Session["OperateurConnecte"] = pwdToken.UserID;

            return RedirectToAction("DefinirMDP", new { id = pwdToken.UserID });
        }


        public ActionResult EnvoyerTokensInitialisation(long id)
        {
            var user = GestionOperateursProd.GestionOFOperateur(id, false);

            OfX3 data = new OfX3();

                Guid token = Guid.NewGuid();
                DateTime expiration = DateTime.Now.AddHours(1);

                bool saved = data.SavePwdToken(user.ID, token, expiration);

                string lien = Url.Action("DefinirMDPParToken", "Production", new { token = token }, protocol: Request.Url.Scheme);

                Mail mail = new Mail();
                mail.From = "iisProd.Conductix@laposte.net";
                mail.To = user.Email;
                mail.Subject = "Création de votre mot de passe ZEN";
                mail.Message = $"Bonjour {user.Prenom},\n\nVeuillez cliquer sur ce lien pour créer votre mot de passe :\n{lien}\n\nCe lien est valable 1 heure.";
                mail.btnSendMail();

            return RedirectToAction("ConfirmationEnvoiEmail", new { id = user.ID });
        }


        [HttpPost]
        public ActionResult DemanderReinitialisation(long id)
        {
            var user = GestionOperateursProd.GestionOFOperateur(id, false); 

            if (user != null)
            {
                var token = Guid.NewGuid();
                var expiration = DateTime.Now.AddHours(1);

                OfX3 data = new OfX3();
                data.SavePwdToken(user.ID, token, expiration);

                var resetLink = Url.Action("ReinitialiserMotDePasse", "Production", new { token = token }, protocol: Request.Url.Scheme);

                Mail mail = new Mail();
                mail.From = "iisProd.Conductix@laposte.net";
                mail.To = user.Email;
                mail.Subject = "Réinitialisation de votre mot de passe";
                mail.Message = $"Bonjour {user.Prenom},\n\nVeuillez cliquer sur le lien suivant pour réinitialiser votre mot de passe :\n{resetLink}\n\nCe lien expirera dans 1 heure.";
                mail.btnSendMail();

                return RedirectToAction("ConfirmationEnvoiEmail", new { id = user.ID });
            }
            else
            {
                return View("~/Views/Authentification/EmailNonTrouve.cshtml");
            }
        }


        
        [HttpGet]
        public ActionResult ConfirmationEnvoiEmail(long id)
        {
            var operateur = GestionOperateursProd.GestionOFOperateur(id, false);
            return View("~/Views/Authentification/ConfirmationEnvoiEmail.cshtml", operateur);
        }

        
        [HttpGet]
        public ActionResult ReinitialiserMotDePasse(Guid token)
        {
            OfX3 data = new OfX3();
            var pwdToken = data.GetPwdToken(token);

            if (pwdToken == null || (pwdToken.IsUsed ?? false) || pwdToken.ExpirationDate < DateTime.Now)
            {
                return View("~/Views/Authentification/LienInvalideOuExpire.cshtml");
            }

            // Récupérer l'ID de l'opérateur depuis le token
            var idOperateur = pwdToken.UserID;

            var operateur = GestionOperateursProd.GestionOFOperateur(idOperateur, false);

            ViewBag.Token = token;
            return View("~/Views/Authentification/FormulaireNouveauMDP.cshtml", operateur);
        }

        
        [HttpPost]
        public ActionResult ReinitialiserMotDePasse(Guid token, string nouveauMotDePasse)
        {
            OfX3 data = new OfX3();
            var pwdToken = data.GetPwdToken(token);

            if (pwdToken == null || (pwdToken.IsUsed ?? false) || pwdToken.ExpirationDate < DateTime.Now)
            {
                return View("~/Views/Authentification/LienInvalideOuExpire.cshtml");
            }

            var idOperateur = pwdToken.UserID;

            // Mise à jour du mot de passe
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(nouveauMotDePasse);

            bool success = data.SavePWDOp(idOperateur, hashedPassword);

            if (success)
            {
                data.MarkPwdTokenAsUsed(token);
                return View("~/Views/Authentification/MotDePasseReinitialise.cshtml");
            }

            ViewBag.Erreur = "Erreur lors de la réinitialisation du mot de passe.";

            var operateur = GestionOperateursProd.GestionOFOperateur(idOperateur, false);
            ViewBag.Token = token;
            return View("~/Views/Authentification/FormulaireNouveauMDP.cshtml", operateur);
        }

        
        public ActionResult Deconnexion()
        {
            Session.Clear();
            GestionTraitementOFs ofs = new GestionTraitementOFs();
            return View("~/Views/Home/Accueil.cshtml", ofs);
        }

        
        public ActionResult DefinirMDP(long? id)
        {
            if (id == null)
                return RedirectToAction("IndexOFOperateur");

            var operateur = GestionOperateursProd.GestionOFOperateur((long)id, false);

            return View("DefinirPassword", operateur);
        }


        [HttpPost]
        public ActionResult CreerPassword(long id, string motdepasse, Guid? token = null)
        {
            if (string.IsNullOrEmpty(motdepasse))
            {
                ViewBag.Erreur = "Le mot de passe ne peut pas être vide.";
                var operateur = GestionOperateursProd.GestionOFOperateur(id, false);
                ViewBag.Token = token;
                return View("DefinirPassword", operateur);
            }

            string hashed = BCrypt.Net.BCrypt.HashPassword(motdepasse);
            bool ok = GestionOperateursProd.SavePasswordOperateur(id, hashed);

            if (!ok)
            {
                ViewBag.Erreur = "Erreur lors de l'enregistrement du mot de passe.";
                var operateur = GestionOperateursProd.GestionOFOperateur(id, false);
                ViewBag.Token = token;
                return View("DefinirPassword", operateur);
            }

            if (token.HasValue)
            {
                OfX3 data = new OfX3();
                data.MarkPwdTokenAsUsed(token.Value);
            }

            GestionTraitementOFs ofs = new GestionTraitementOFs();
            return View("~/Views/Home/Accueil.cshtml", ofs);
        }



        [HttpPost]
        public ActionResult ChangerPhoto(HttpPostedFileBase nouvellePhoto)
        {
            if (Session["OperateurConnecte"] == null)
                return RedirectToAction("ConnexionOp");

            var userId = (long)Session["OperateurConnecte"];

            if (nouvellePhoto != null && nouvellePhoto.ContentLength > 0)
            {
                string filename = Path.GetFileName(nouvellePhoto.FileName).Replace(" ", "_");
                string path = Path.Combine(Server.MapPath("~/operateurs/OperateursA"), filename);
                nouvellePhoto.SaveAs(path);

                // Enregistre le chemin dans la base
                OfX3 data = new OfX3();
                data.MettreAJourPhoto(userId, "/OperateursA/" + filename);
            }

            return RedirectToAction("GestionOF", new { id = userId });
        }

        
        public ActionResult gestionOf(long? id, int? viewAction, string ofCherche)
        {
            // Pour éviter de pouvoir accéder à l'espace juste en tapant le bon URL
            // On met en place un id de Session
            var idConnecte = Session["OperateurConnecte"];
            var estAdmin = Session["EstAdmin"] != null && (bool)Session["EstAdmin"];

            // Donc si il n'est pas connecté il ne peut pas y accéder
            if (idConnecte == null || (!estAdmin && Convert.ToInt64(idConnecte) != id))
            {
                return RedirectToAction("ConnexionOp", new { id = id }); // redirige vers la connexion
            }

            bool ChargeOfPlanifie = false;
            if (id == null)
            {
                return RedirectToAction("IndexOFOperateur", "Production");
            }
            else
            {
                if (viewAction == null || viewAction == 0)
                {
                    ViewData["PopUpOf"] = "false";
                    ViewData["OfTrouve"] = "";
                    ChargeOfPlanifie = false;
                }
                else if (viewAction == 1)
                {
                    ViewData["PopUpOf"] = "true";
                    ViewData["OfTrouve"] = "";
                    ChargeOfPlanifie = true;
                }
                else if (viewAction == 2)
                {
                    ViewData["PopUpOf"] = "false";
                    ViewData["OfTrouve"] = ofCherche;
                }
                else if (viewAction == 5)
                {
                    return RedirectToAction("OutilProd");
                }

                    DataOperateurProd ope = new DataOperateurProd();
                if (viewAction == 3)
                {
                    ViewData["PopUpOf"] = "true";
                    ope = GestionOperateursProd.GestionOFOperateur((long)id, false, ofCherche);
                }
                else
                {
                    ope = GestionOperateursProd.GestionOFOperateur((long)id, false);
                }
                PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();
                uint pole = (uint)(db.OPERATEURS.Where(i => i.ID == ope.ID).Select(i => i.POLE).First());

                ope.initOfList((int)pole, ofCherche, ChargeOfPlanifie);

                // Récupérer les données pour le pop-up
                InfoAmelioration info = new InfoAmelioration();
                ViewData["Sujet"] = info.Sujet; // Liste des services
                ViewData["Secteur"] = info.Secteur; // Liste des secteurs

                // Calculer la date anglaise pour le champ de date
                string date = DateTime.Now.ToString("dd-MM-yyyy");
                string dateAnglaise = date.Substring(6, 4) + date.Substring(2, 3) + "-" + date.Substring(0, 2);
                ViewData["DateAnglaise"] = dateAnglaise;

                ViewBag.OperateurID = id;
                ViewBag.EstAdmin = estAdmin;
                return View(ope);
            }
        }

        [HttpPost, ActionName("AjoutSignalement")]
        public ActionResult AjoutSignalement(HttpPostedFileBase ImageSignalement)
        {
            string typeSignalement = Request.Form["TypeSignalement"];
            string nom = Request.Form["SaisieNom"];
            string date = Request.Form["SaisieDate"];
            string description = Request.Form["SaisieDescription"];
            string description2 = Request.Form["SaisieDescription2"];
            string photoSignalement = Request.Form["srcImg"];
            string imageDB = null;

            // Gestion de l'image (commune aux deux types)
            if (ImageSignalement != null && ImageSignalement.ContentType.Contains("image/jpeg"))
            {
                byte[] thePictureAsBytes = new byte[ImageSignalement.ContentLength];
                using (BinaryReader theReader = new BinaryReader(ImageSignalement.InputStream))
                {
                    thePictureAsBytes = theReader.ReadBytes(ImageSignalement.ContentLength);
                }
                imageDB = Convert.ToBase64String(thePictureAsBytes);
            }
            else if (!string.IsNullOrWhiteSpace(photoSignalement))
            {
                photoSignalement = photoSignalement.Remove(0, 23);
                imageDB = photoSignalement;
            }

            string pathimage = "";
            int result = 0;

            if (typeSignalement == "Proposition")
            {
                string service = Request.Form["SaisieService"];
                string secteur = Request.Form["SaisieSecteur"];
                InfoAmelioration obj = new InfoAmelioration();

                result = obj.AddAmelioration(description, description2, nom, secteur, service, date, imageDB, ref pathimage);

                if (result > 0)
                {
                    /*
                    Mail mail = new Mail();
                    mail.From = "iisProd.Conductix@laposte.net";
                    mail.To = "franck.moisy@conductix.com,romain.destaing@conductix.com,christian.fournier@conductix.com,wilfrid.martin@conductix.com,iisProd.Conductix@laposte.net";
                    mail.Subject = "Proposition d'amélioration";
                    mail.Message = "Service : " + secteur + "\r\n" +
                                   "Type d'amélioration : " + service + "\r\n" +
                                   "Emetteur : " + nom + "\r\n" +
                                   "Date : " + date + "\r\n" +
                                   "Description : " + description + "\r\n" +
                                   " -" + description2;
                    mail.AttachementPath = pathimage;
                    mail.btnSendMail();
                    */
                    TempData["Message"] = "Demande enregistrée avec succès.";
                    TempData["MessageType"] = "success";
                }
                else
                {
                    TempData["Message"] = "Erreur lors de l'enregistrement de la demande.";
                    TempData["MessageType"] = "error";
                }
            }
            else if (typeSignalement == "Accident")
            {
                string typeAccident = Request.Form["TypeAccident"];
                int niveauSaisie = Convert.ToInt32(typeAccident);
                string gravite = Request.Form["Gravite"];
                if (string.IsNullOrWhiteSpace(gravite)) { gravite = "non renseigné"; }
                SECU_PROD obj = new SECU_PROD();

                result = obj.AddAccident(typeAccident, date, description, description2, nom, "PRODUCTION", gravite, imageDB, ref pathimage);

                if (result > 0)
                {
                    /*
                    Mail mail = new Mail();
                    mail.From = "iisProd.Conductix@laposte.net";
                    mail.To = "franck.moisy@conductix.com,romain.destaing@conductix.com,christian.fournier@conductix.com,wilfrid.martin@conductix.com,iisProd.Conductix@laposte.net";
                    mail.Subject = "Déclaration Accidents type : " + obj.NiveauAccident(niveauSaisie);
                    mail.Message = "Niveau d'accident : " + "Niveau " + typeAccident + " - " + obj.NiveauAccident(niveauSaisie) + "\r\n" +
                                   "Victime : " + nom + "\r\n" +
                                   "Date : " + date + "\r\n" +
                                   "Description : " + description + "\r\n" +
                                   "Niveau de risque: " + gravite + "\r\n" +
                                   "Description complémentaire :" + description2;
                    mail.AttachementPath = pathimage;
                    mail.btnSendMail();
                    */
                    TempData["Message"] = "Accident déclaré avec succès.";
                    TempData["MessageType"] = "success";
                }
                else
                {
                    TempData["Message"] = "Erreur lors de la déclaration de l'accident.";
                    TempData["MessageType"] = "error";
                }
            }
            else
            {
                TempData["Message"] = "Type de signalement non valide.";
                TempData["MessageType"] = "error";
            }

            return RedirectToAction("GestionOf", "Production", new { id = Session["OperateurConnecte"], viewAction = 0 });
        }
    


[HttpPost, ActionName("gestionOf")]
        public ActionResult gestionOfPopup(DataOperateurProd ope)
        {
            // description des diferent formulaire de la page GestionOf
            // bouton de of non trite par pole  
            //name="cat" value = 2 bidir 3 mono 4 test 1 tous
            //name="ID" value="@Model.ID"

            // barre de recherche
            //name="ID" value="@Model.ID"
            //id="searched-OF" name="searched-OF"
            //name="submitButton"

            int? id_demande = null;
            int viewAction = 0;  // Indicateur pour ouvrir un pop up au rechargement suivant de la page
            string OFcherche = "";
            string OFaajouter = "";
            string IDOF = "";
            string ArretOF = "";
            int Ilotid = 0;
            string deleleAlea = "";
            string ArretAlea = "";
            int posteDemande = 1;

            try
            {
                id_demande = Int32.Parse(Request.Form["ID"]);
                if (Request.Form["cat"] != null)
                {
                    Ilotid = Int32.Parse(Request.Form["cat"]);
                    // gestion outil admin
                    if (Ilotid == 8)
                    {
                        viewAction = 8;
                    }
                    //gestion operateur dudes poles
                    else if (Ilotid == 7)
                    {
                        viewAction = 7;
                    }
                    else if (Ilotid != 5 && Ilotid != 6)
                    {
                        // ilot1 affiched'une popup
                        viewAction = 1;
                    }
                    // planification des of
                    else if (Ilotid == 6)
                    {
                        viewAction = 6;
                    }
                    else
                    {
                        // (Ilotid == 5) print ZEN pour etiquette
                        viewAction = 5;
                    }
                }
                else if (Request.Form["searchedOF"] != null)
                {
                    OFcherche = Request.Form["searchedOF"].ToUpper().Trim().ToUpper();
                }
                else if (Request.Form["newOf"] != null)
                // ajouter un of 
                {
                    OFaajouter = Request.Form["newOf"];
                }
                else if (Request.Form["IDOF"] != null)
                {
                    //ajouter des aleas a un of
                    IDOF = Request.Form["IDOF"];
                }
                else if (Request.Form["ArretOF"] != null)
                {
                    // demande d'arret de l'of
                    ArretOF = Request.Form["ArretOF"];
                }
                else if (Request.Form["deleteAleaId"] != null)
                {
                    deleleAlea = Request.Form["deleteAleaId"];
                }
                else if (Request.Form["arretAleaIdOF"] != null)
                {
                    ArretAlea = Request.Form["arretAleaIdOF"];
                }
            }
            catch
            {

            }
            if (viewAction == 1) // affichage de l'of plannifie
            {
                OfX3 data = new OfX3();
                OPERATEURS Operateur = data.ListOPERATEURs("PROD").Where(p => p.ID == (long)id_demande).First();
                if (!Operateur.ANIMATEUR)
                {
                    // Operateur.POSTE = Ilotid.ToString();
                }
                data.SaveDbOperateur(Operateur);
            }
            else if (viewAction == 6)
            {
                OfX3 data = new OfX3();
                OPERATEURS Operateur = data.ListOPERATEURs("PROD").Where(p => p.ID == (long)id_demande).First();
                if (Operateur != null && Operateur.POLE != null)
                {
                    posteDemande = (int)Operateur.POLE;
                }
            }
            else if (viewAction == 7)
            {

            }
            else if (viewAction == 8)
            {

            }
            else if (!string.IsNullOrWhiteSpace(OFcherche))
            {
                DataOperateurProd dataoperateur = GestionOperateursProd.GestionOFOperateur((long)id_demande, false, OFcherche);
                if (dataoperateur.OfEncours.Any(p => p.NMROF.Contains(OFcherche) && p.ISALIVE == true && !p.STATUSTYPE.Contains("CLOSED")))
                {
                    viewAction = 2;
                }
                else if (dataoperateur.OfNontraiteNontrace.Any(p => p.NMROF == OFcherche))
                {
                    viewAction = 3;
                }
            }
            else if (!string.IsNullOrWhiteSpace(OFaajouter))
            {
                DataOperateurProd dataoperateur = GestionOperateursProd.GestionOFOperateur((long)id_demande, false, OFaajouter);
                var tmp1 = dataoperateur.OfEncours.Where(p => p.NMROF == OFaajouter && p.ISALIVE == true && p.STATUSTYPE.Contains("CLOSED"));
                var tmp = dataoperateur.OfNontraiteNontrace.Where(p => p.NMROF.Contains(OFaajouter));
                if (tmp1 != null && tmp1.Count() == 1)
                {
                    OF_PROD_TRAITE of_traite = tmp1.First();
                    if (of_traite.STATUSTYPE.Contains("CLOSED"))
                    {
                        OfX3 db = new OfX3();
                        of_traite.STATUSTYPE = "INPROGRESS";
                        of_traite.ENDTIME = DateTime.Now;
                        db.SaveDbOF_PROD_TRAITE(of_traite);
                    }
                }
                else if (tmp != null && tmp.Count() > 0)
                {
                    OF_PROD_TRAITE of_traite = tmp.First();
                    of_traite.STATUSTYPE = "INPROGRESS";
                    of_traite.STARTTIME = DateTime.Now;
                    of_traite.OPERATEUR = id_demande;
                    of_traite.ISALIVE = true;
                    OfX3 db = new OfX3();

                    db.SaveDbOF_PROD_TRAITE(of_traite);
                    foreach (var of in dataoperateur.OfEncours.Where(p => p.OPERATEUR == id_demande && p.STATUSTYPE == "INPROGRESS" && p.ID != of_traite.ID))
                    {
                        of.STATUSTYPE = "ENPAUSE";
                        of.ENDTIME = DateTime.Now;
                        db.SaveDbOF_PROD_TRAITE(of);
                    }
                }
            }
            else if (!string.IsNullOrWhiteSpace(IDOF))
            {
                OfX3 db = new OfX3();
                DeclarationAlea aleas = new DeclarationAlea();
                aleas.GestionCodeOF();
                int ID_OF = Convert.ToInt32(IDOF);
                DataOperateurProd dataoperateur = GestionOperateursProd.GestionOFOperateur((long)id_demande, false);
                OF_PROD_TRAITE of_traite = dataoperateur.OfEncours.Where(p => p.ID.Equals(ID_OF)).First();
                OF_PROD_TRAITE of = new OF_PROD_TRAITE(); of.NMROF = of_traite.NMROF; of.OPERATEUR = of_traite.OPERATEUR;
                foreach (var itemFormulaire in Request.Form) //Pour récupérer les clés et valeurs du formulaire
                {
                    string item = itemFormulaire.ToString();
                    if (item != "ID" && item != "IDOF")
                    {
                        var codeGroupe = aleas.CodeAlea.Where(p => p.Code == Int32.Parse(item));
                        ALEAS_OF alea_de_OF = new ALEAS_OF();
                        ALEAS_OF_DETAILS alea_de_OF_details = new ALEAS_OF_DETAILS();
                        alea_de_OF.NMR_ALEA = Int32.Parse(item);
                        alea_de_OF.DateStart = DateTime.Now;
                        alea_de_OF.IsAlwaysOn = true;
                        alea_de_OF_details.DateStart = DateTime.Now;
                        alea_de_OF.ID_OF_PROD_ORIG = of.ID;
                        alea_de_OF.ALEAS_OF_DETAILS.Add(alea_de_OF_details);
                        of.ALEAS_OF.Add(alea_de_OF);
                    }
                }
                db.SaveDbOFAlea(of, null);
            }
            else if (!string.IsNullOrWhiteSpace(ArretOF))
            {
                Dictionary<String, List<String>> listNewAlea = new Dictionary<string, List<string>>();
                listNewAlea.Add("Alea", new List<String>());
                listNewAlea.Add("Temps", new List<String>());
                List<String> listNewPauses = new List<string>();
                List<String> listNewReunions = new List<string>();
                int ID_OF = Convert.ToInt32(ArretOF);

                int quantiteReelle = Int32.Parse(Request.Form["quantite-reelle"]);
                foreach (var itemFormulaire in Request.Form)
                {
                    if (itemFormulaire.ToString().Contains("SelectAlea") && itemFormulaire.ToString().Length > 10)
                    {
                        int nbr = Convert.ToInt32(itemFormulaire.ToString().Substring(10, itemFormulaire.ToString().Length - 10));
                        listNewAlea["Alea"].Add(Request.Form[itemFormulaire.ToString()]);
                        string tt = Request.Form["SelectTimeAlea" + nbr.ToString()];
                        listNewAlea["Temps"].Add(Request.Form["SelectTimeAlea" + nbr.ToString()]);
                    }
                    //else if (itemFormulaire.ToString().Contains("SelectTimeAlea"))
                    //{
                    //    listNewAlea["Temps"].Add(Request.Form[itemFormulaire.ToString()]);
                    //}

                    else if (itemFormulaire.ToString().Contains("SelectTimeReunion"))
                    {
                        listNewReunions.Add(Request.Form[itemFormulaire.ToString()]);
                    }
                    else if (itemFormulaire.ToString().Contains("SelectTimePause"))
                    {
                        listNewPauses.Add(Request.Form[itemFormulaire.ToString()]);
                    }
                }

                OfX3 db = new OfX3();
                DeclarationAlea aleas = new DeclarationAlea();
                aleas.GestionCodeOF();
                DataOperateurProd dataoperateur = GestionOperateursProd.GestionOFOperateur((long)id_demande, false);
                OF_PROD_TRAITE of_traite = dataoperateur.OfEncours.Where(p => p.ID == ID_OF && p.ISALIVE != false).First();

                for (int i = 0; i < listNewAlea["Alea"].Count; i++)
                {
                    int codeerreur = Convert.ToInt32(listNewAlea["Alea"][i]);
                    ALEAS_OF alea_de_OF = new ALEAS_OF();
                    var codeGroupe = aleas.CodeAlea.Where(p => p.Code == codeerreur).First();
                    alea_de_OF.NMR_ALEA = codeGroupe.Code;
                    alea_de_OF.Delai = long.Parse(Regex.Match(listNewAlea["Temps"][i], @"\d+").Value);
                    alea_de_OF.IsAlwaysOn = false;
                    of_traite.ALEAS_OF.Add(alea_de_OF);
                }
                for (int i = 0; i < listNewReunions.Count; i++)
                {
                    ALEAS_OF alea_de_OF = new ALEAS_OF();
                    var codeGroupe = aleas.CodeAlea.Where(p => p.Code == 1032).First();
                    alea_de_OF.NMR_ALEA = codeGroupe.Code;
                    alea_de_OF.IsAlwaysOn = false;
                    alea_de_OF.Delai = long.Parse(Regex.Match(listNewReunions[i], @"\d+").Value);
                    of_traite.ALEAS_OF.Add(alea_de_OF);
                }
                for (int i = 0; i < listNewPauses.Count; i++)
                {
                    ALEAS_OF alea_de_OF = new ALEAS_OF();
                    var codeGroupe = aleas.CodeAlea.Where(p => p.Code == 1031).First();
                    alea_de_OF.NMR_ALEA = codeGroupe.Code;
                    alea_de_OF.IsAlwaysOn = false;
                    alea_de_OF.Delai = long.Parse(Regex.Match(listNewPauses[i], @"\d+").Value);
                    of_traite.ALEAS_OF.Add(alea_de_OF);
                }
                db.SaveDbOFAlea(of_traite, null);


                if (!of_traite.STATUSTYPE.Contains("ENPAUSE"))
                {
                    of_traite.ENDTIME = DateTime.Now;
                }
                else
                {

                }
                of_traite.STATUSTYPE = "CLOSED";
                of_traite.QTRREEL = quantiteReelle;

                if (db.SaveDbOF_PROD_TRAITE(of_traite) > 0)
                {
                    // soldé OF X3
                    OfProdModel OfASolde = new OfProdModel();
                    OfASolde.OfProdModelFull(of_traite.NMROF);
                    of_traite.EmplacementItem = OfASolde.ofProdModelInfo.EmplacementItem;
                    bool solde = false;
                    if (of_traite.QTRREEL == of_traite.QTRTHEORIQUE)
                    {
                        solde = true;
                    }
                    JobERP.CreateFindOF(of_traite, solde);
                }
                else
                {

                }
            }
            else if (!String.IsNullOrWhiteSpace(deleleAlea))
            {
                int idAlea = Int32.Parse(Request.Form["deleteAleaId"].ToString());
                OfX3 db = new OfX3();
                db.SaveDbDeleteAlea(idAlea);
            }
            else if (!String.IsNullOrWhiteSpace(ArretAlea))
            {
                int idOF = Int32.Parse(Request.Form["arretAleaIdOF"].ToString());
                int idAlea = Int32.Parse(Request.Form["arretAleaId"].ToString());
                DataOperateurProd dataoperateur = GestionOperateursProd.GestionOFOperateur((long)id_demande, false);
                OF_PROD_TRAITE of_traite = dataoperateur.OfEncours.Where(p => p.ID == idOF && p.ISALIVE != false).First();
                //OF_PROD_TRAITE of_traite = ope1.OfEncours.Where(p => p.ID == idOF && p.ISALIVE != false).First();
                ALEAS_OF alea = of_traite.ALEAS_OF.Where(p => p.ID == idAlea).First();
                alea.StopTime = DateTime.Now;
                alea.IsAlwaysOn = false;
                if (alea.ALEAS_OF_DETAILS.Where(p => p.StopTime == null).Count() > 0)
                {
                    alea.ALEAS_OF_DETAILS.Where(p => p.StopTime == null).First().StopTime = DateTime.Now;
                }
                OfX3 db = new OfX3();
                db.SaveDbOFAlea(of_traite, idAlea);
            }


            if (viewAction == 1)
            {
                return RedirectToAction("gestionOF", "Production", new { id = (long)ope.ID, viewAction = 1 }); //cas : ouvrir popup ajout OF
            }
            else if (viewAction == 2)
            {
                return RedirectToAction("gestionOF", "Production", new { id = (long)ope.ID, viewAction = 2, ofCherche = OFcherche }); //cas : OF recherche se situe dans OF en cours
            }
            else if (viewAction == 3)
            {
                return RedirectToAction("gestionOF", "Production", new { id = (long)ope.ID, viewAction = 3, ofCherche = OFcherche });
            }
            else if (viewAction == 5)
            {
                return RedirectToAction("OutilProd", "Production", new { id = (long)ope.ID });
            }
            else if (viewAction == 6)
            {
                return RedirectToAction("Planificationof", "Production", new { pole = (int)posteDemande });
            }
            else if (viewAction == 7)
            {
                return RedirectToAction("AffectationOperateurs", "Production");
            }
            else if (viewAction == 8)
            {
                return RedirectToAction("GestionOutilAdmin", "Production", new { id = (long)ope.ID });
            }

            /*

            else if (viewAction == 9)
            {
                return RedirectToAction("Signalement", "Production", new { id = (long)ope.ID });
            }*/
            else
            {
                return RedirectToAction("gestionOF", "Production", new { id = (long)ope.ID }); //tous les autres cas possibles
            }
            //return RedirectToAction("gestionOf","Production",  new {id = id_demande, popUp = OuvrirPopUp.ToString() , ofcherche = OFcherche });
        }

        public ActionResult ChangeStatus(string actionof, long idOF, long? idOperateur)
        {
            OfX3 db = new OfX3();
            DataOperateurProd ope = GestionOperateursProd.GestionOFOperateur((long)idOperateur, false);

            if (ope != null && ope.OfEncours.Where(p => p.ID == idOF && p.OPERATEUR == idOperateur).Count() > 0)
            {
                OF_PROD_TRAITE of_traite = ope.OfEncours.Where(p => p.ID == idOF).First();
                if (of_traite != null)
                {
                    if (actionof.Equals("PAUSE"))
                    {
                        if (of_traite.STATUSTYPE == "INPROGRESS")
                        {
                            of_traite.STATUSTYPE = "ENPAUSE";
                            of_traite.ENDTIME = DateTime.Now;
                            int tmp = (int)((DateTime)of_traite.STARTTIME - DateTime.Now).TotalSeconds;

                        }
                    }
                    else if (actionof.Equals("REPRISE"))
                    {
                        of_traite.STATUSTYPE = "INPROGRESS";
                        of_traite.STARTTIME = DateTime.Now;
                    }
                    db.SaveDbOF_PROD_TRAITE(of_traite);

                    foreach (var of in ope.OfEncours.Where(p => p.OPERATEUR == idOperateur && p.STATUSTYPE == "INPROGRESS" && p.ID != of_traite.ID))
                    {
                        of.STATUSTYPE = "ENPAUSE";
                        of.ENDTIME = DateTime.Now;
                        int tmp = (int)((DateTime)of_traite.STARTTIME - DateTime.Now).TotalSeconds;

                        db.SaveDbOF_PROD_TRAITE(of);
                    }
                }
            }
            return RedirectToAction("gestionOF", "Production", new { id = idOperateur });
        }
        public ActionResult SearchArticles(string query)
        {
            List<ArticleSite> articleSites = GestionArticlePNC.GetArticleSite(query);

            return Json(articleSites, JsonRequestBehavior.AllowGet);
        }



        public OrdreFabrication FinfOfAllX3(string ofdemande)
        {
            OfX3 ofs = new OfX3();
            List<OrdreFabrication> tmp = ofs.ListOfAllProductionX3().Where(p => p.MFGNUM_0.Equals(ofdemande.Trim())).ToList();
            if (tmp.Count > 0)
            {
                return tmp.First();
            }
            else
            {
                return null;
            }
        }
        private void ImprimerEtiquetteZPL(OrdreFabrication op)
        {
            EtiquetteLogistique et = new EtiquetteLogistique();
            int num_semaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);

            int qtr = (int)(Convert.ToDecimal(op.EXTQTY_0, new CultureInfo("fr-FR")));
            for (int i = 0; i < qtr;)
            {
                et.Reference1 = op.ITMREF_0;
                et.Of1 = op.MFGNUM_0;
                if (op.VCRNUMORI_0.Trim().StartsWith("C20"))
                {
                    et.Commande1 = "ar: " + op.VCRNUMORI_0;
                }
                et.Date1 = num_semaine.ToString("00") + "/" + DateTime.Now.Year.ToString("00");
                if (i % 2 != 0)
                {
                    et.Reference2 = op.ITMREF_0;
                    et.Of2 = op.MFGNUM_0;
                    if (op.VCRNUMORI_0.Trim().StartsWith("C20"))
                    {
                        et.Commande2 = "ar: " + op.VCRNUMORI_0;
                    }
                }
                // obtenir les bons droits d'ecriture sur le reseau
                //using (Impersonation imp = new Impersonation(Resource1.LoginAccesReseau, Resource1.DomaineAccesReseau, Resource1.PasswordAccesReseau))
                //{
                et.PrintEtiquetteQTR();
                // }
                i += 2;
            }
        }
        private void ImprimerEtiquette(OrdreFabrication op)
        {
            // etiquette 
            string path = Resource1.REP_SCRUTATION;
            int num_semaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            string freq = "";

            int quantite = (int)(Convert.ToDecimal(op.EXTQTY_0, new CultureInfo("fr-FR")));
            ParametresModel param = new ParametresModel(null);
            string imprimante = param.ParametresModelValue(5);
            string numserie = op.MFGNUM_0.Substring(1, op.MFGNUM_0.Length - 1);


            GestionTracaProd traca = new GestionTracaProd();
            TRACA_ETIQUETTES traca_et = new TRACA_ETIQUETTES();
            traca_et.CMD = op.VCRNUMORI_0;
            traca_et.DATE = DateTime.Now;
            traca_et.ITEM = op.ITMREF_0;
            traca_et.LOT = op.MFGNUM_0;
            traca_et.QTR = quantite.ToString();
            traca_et.NMR_OF = op.MFGNUM_0;
            traca_et.REFERENCE1 = "";
            traca_et.TYPE_ETIQUETTE = "PrintEtiquetteQTR";
            traca_et.ID_OPE = 0;
            traca.EtiquetteTracaProdAdd(traca_et);


            if (imprimante.Contains("TYPE_354140A"))
            {
                DateTime now = DateTime.Now;
                for (int i = 0; i < quantite;)
                {
                    FileStream fileStream = new FileStream(string.Concat(path, "\\ET354140A_", op.MFGNUM_0, (i + 1).ToString("00"), ".cmd"), FileMode.Create);
                    StreamWriter writer = new StreamWriter(fileStream);

                    writer.WriteLine(string.Concat(StructLabel.LABELNAME, "=\"", Resource1.LABELNAME_DATAMATRIX, "\""));
                    //writer.WriteLine(string.Concat(StructLabel.LABELNAME, "=\"", "57X21 DATAMATRIX.Lab", "\""));
                    writer.WriteLine(string.Concat(StructLabel.CODE_PRODUIT1, "=\"", op.ITMREF_0, "\""));
                    writer.WriteLine(string.Concat(StructLabel.OF1, "=\"", op.MFGNUM_0, "\""));
                    if (op.SERNUM == 3)
                    {
                        writer.WriteLine(string.Concat(StructLabel.NMRSERIE1, "=\"", numserie + (i + 1).ToString("00"), "\""));
                    }
                    if (!string.IsNullOrWhiteSpace(op.VCRNUMORI_0) && op.VCRNUMORI_0.StartsWith("C"))
                    {
                        writer.WriteLine(string.Concat(StructLabel.CDE1, "=\"", op.VCRNUMORI_0, "\""));
                    }
                    else
                    {
                        writer.WriteLine(string.Concat(StructLabel.CDE1, "=\"", "\""));
                    }

                    if (i == quantite - 1)
                    {
                        writer.WriteLine(string.Concat(StructLabel.CODE_PRODUIT2, "=\"", "\""));
                        writer.WriteLine(string.Concat(StructLabel.OF2, "=\"", "\""));
                        writer.WriteLine(string.Concat(StructLabel.CDE2, "=\"", "\""));
                        //if (op.SERNUM == 3)
                        //{
                        //    writer.WriteLine(string.Concat(StructLabel.NMRSERIE1 + (i+2).ToString("00"), "=\"", numserie, "\""));
                        //}
                    }
                    else
                    {
                        writer.WriteLine(string.Concat(StructLabel.CODE_PRODUIT2, "=\"", op.ITMREF_0, "\""));
                        writer.WriteLine(string.Concat(StructLabel.OF2, "=\"", op.MFGNUM_0, "\""));
                        if (op.SERNUM == 3)
                        {
                            writer.WriteLine(string.Concat(StructLabel.NMRSERIE2, "=\"", numserie + (i + 2).ToString("00"), "\""));
                        }
                        if (!string.IsNullOrWhiteSpace(op.VCRNUMORI_0) && op.VCRNUMORI_0.StartsWith("C"))
                        {
                            writer.WriteLine(string.Concat(StructLabel.CDE2, "=\"", op.VCRNUMORI_0, "\""));
                        }
                        else
                        {
                            writer.WriteLine(string.Concat(StructLabel.CDE2, "=\"", "\""));
                        }
                    }
                    writer.WriteLine(string.Concat(StructLabel.QUANTITY, "=\"", "1", "\""));
                    writer.Close();
                    fileStream.Close();
                    i += 2;
                }
            }
            else if (imprimante.Contains("TYPE_319321"))
            {
                DateTime now = DateTime.Now;
                for (int i = 0; i < quantite; i++)
                {
                    FileStream fileStream = new FileStream(string.Concat(path, "\\ET319321_", op.MFGNUM_0, i.ToString("00"), ".cmd"), FileMode.Create);
                    StreamWriter writer = new StreamWriter(fileStream);
                    //writer.WriteLine(string.Concat(StructLabel.LABELNAME, "=\"", Resource1.LABELNAME_DATAMATRIX, "\"")); 
                    writer.WriteLine(string.Concat(StructLabel.LABELNAME, "=\"", "57X21 DATAMATRIX.Lab", "\""));
                    writer.WriteLine(string.Concat(StructLabel.CODE_PRODUIT1, "=\"", op.ITMREF_0, "\""));
                    writer.WriteLine(string.Concat(StructLabel.OF1, "=\"", op.MFGNUM_0, "\""));

                    if (!string.IsNullOrWhiteSpace(op.VCRNUMORI_0) && op.VCRNUMORI_0.StartsWith("C"))
                    {
                        writer.WriteLine(string.Concat(StructLabel.CDE1, "=\"", op.VCRNUMORI_0, "\""));
                    }
                    else
                    {
                        writer.WriteLine(string.Concat(StructLabel.CDE1, "=\"", "\""));
                    }
                    if (op.SERNUM == 3)
                    {
                        writer.WriteLine(string.Concat(StructLabel.NMRSERIE1 + (i + 1).ToString("00"), "=\"", numserie, "\""));
                    }

                    writer.WriteLine(string.Concat(StructLabel.QUANTITY, "=\"", "1", "\""));
                    writer.Close();
                    fileStream.Close();
                }
            }
            // doc rapport
        }
        private void ImprimerEtiquetteNC(NON_CONFORMITE op)
        {
            // etiquette 
            string path = Resource1.REP_SCRUTATION;
            int num_semaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);


            ParametresModel param = new ParametresModel(null);
            string imprimante = param.ParametresModelValue(5);
            string tmp = ((int)op.Qtr).ToString("00");

            if (imprimante.Contains("TYPE_354140A"))
            {
                DateTime now = DateTime.Now;

                FileStream fileStream = new FileStream(string.Concat(path, "\\ET354140A_", op.Item, (1).ToString("00"), ".cmd"), FileMode.Create);
                StreamWriter writer = new StreamWriter(fileStream);

                writer.WriteLine(string.Concat(StructLabel.LABELNAME, "=\"", Resource1.LABELNAME_PNC, "\""));
                //writer.WriteLine(string.Concat(StructLabel.LABELNAME, "=\"", "57X21 DATAMATRIX.Lab", "\""));
                writer.WriteLine(string.Concat(StructLabel.CODE_PRODUIT1, "=\"", op.Item, "\""));
                writer.WriteLine(string.Concat(StructLabel.NC, "=\"", op.NmrChronoS, "\""));
                writer.WriteLine(string.Concat(StructLabel.AUTRE, "=\"", "Qtr:", ((int)op.Qtr).ToString("00"), "\""));

                writer.WriteLine(string.Concat(StructLabel.QUANTITY, "=\"", "1", "\""));
                writer.Close();
                fileStream.Close();

            }
            else if (imprimante.Contains("TYPE_319321"))
            {

            }
        }
        private void ImprimerEtiquette(ITEM_LOCALISATION et, bool nbbac)
        {
            // etiquette 
            string path = Resource1.REP_SCRUTATION;
            int num_semaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            //path = @"c:\tmp";

            ParametresModel param = new ParametresModel(null);
            string imprimante = param.ParametresModelValue(5);
            if (!nbbac) { et.NbBacPourImpression = 1; }
            for (int i = 0; i < et.NbBacPourImpression; i++)
            {
                if (imprimante.Contains("TYPE_354140A"))
                {
                    DateTime now = DateTime.Now;

                    FileStream fileStream = new FileStream(string.Concat(path, "\\ET354140A_", et.ITEMREF, et.ID.ToString(), (i).ToString("00"), ".cmd"), FileMode.Create);
                    StreamWriter writer = new StreamWriter(fileStream);

                    writer.WriteLine(string.Concat(StructLabel.LABELNAME, "=\"", Resource1.LABELNAME_BAC, "\""));
                    writer.WriteLine(string.Concat(StructLabel.ITMREF, "=\"", et.ITEMREF, "\""));
                    writer.WriteLine(string.Concat(StructLabel.LOCALISATION, "=\"", et.Chariot, "\""));
                    writer.WriteLine(string.Concat(StructLabel.NUM, "=\"", "", "\""));

                    writer.WriteLine(string.Concat(StructLabel.QUANTITY, "=\"", "1", "\""));
                    writer.Close();
                    fileStream.Close();

                }
                else if (imprimante.Contains("TYPE_319321"))
                {

                }
            }
        }

    }
}