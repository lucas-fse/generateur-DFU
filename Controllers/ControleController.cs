using GenerateurDFUSafir.Models;
using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace GenerateurDFUSafir.Controllers
{
    public class ControleController : Controller
    {
        // GET: Controle
        public ActionResult PackControle(long? id)
        {
            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();
            if (id != null)
            {
                OPERATEURS op = db.OPERATEURS.Where(o => o.ID == (long)id).First();
                ViewBag.Operateur = op.PRENOM + " " + op.NOM;
                OldOF oldOf = new OldOF();
                return View(oldOf);
            }
               return  RedirectToAction("production","production",null);
            
        }

        [ActionName("getPack")]
        public ActionResult GetPack(string rawscan)
        {
            //rawscan = "F2311980";
           // rawscan = "RSL;DM2T1S1823A;231330901;";
            ControleFinalScan datas = new ControleFinalScan();
            datas.GetOFSearch(rawscan);
            //OfProdModel nomenclatureOF = new OfProdModel();
            //nomenclatureOF.OfProdModelFull("F2313309");
            //datas.NomenclatureOF = nomenclatureOF;
            //datas.NomenclatureOF = NomenclatureOF;
            //datas.Pack.QTr = 10;
            return Json(new { Pack = datas.Pack, AlreadyScan = datas.ArticleDejaScanné, N = datas.NbPackDejaScanné, Nom = datas.NomenclatureOF }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("SavePack")]
        public ActionResult SaveValidationPack(List<HISTORIQUE_CONTROL> datas)
        {


            DateTime now = DateTime.Now;
            datas.ForEach(i => {
                i.Date = now;
            });

            PEGASE_CONTROLEntities db = new PEGASE_CONTROLEntities();
            PEGASE_PROD2Entities2 db2 = new PEGASE_PROD2Entities2();
            long operateur = -1;
            foreach(var data in datas)
            {
                db.HISTORIQUE_CONTROL.Add(data);
                operateur = (long)data.Quantite;
                if (data.IsPack == true)
                {
                    CONTROLE_QUALITE cq = new CONTROLE_QUALITE();
                    cq.Conforme = 1;
                    cq.Date = DateTime.Now;
                    cq.ID_OPERATEUR = data.Operateur;
                    cq.ITEMREF = data.ItemRef;
                    cq.NMROF = data.NumOF;
                    cq.Description = data.RawScan;
                    db2.CONTROLE_QUALITE.Add(cq);
                    db2.SaveChanges();
                }
            }
            db.SaveChanges();

            if (true)
            {
                
            }

            return Json("OK", JsonRequestBehavior.AllowGet);

        }

        public ActionResult SuppressionControleFinal(long? ID)
        {
            if (ID != null)
            {
                SupprControleFinal cf = new SupprControleFinal();
                cf.ID = ID;
                return View(cf);
            }
            return RedirectToAction("Production", "Production", null);
        }
        [HttpPost, ActionName("RemoveControlPack")]
        public ActionResult RemoveControlPack(long IDOPE , string numof)
        {

            PEGASE_CONTROLEntities db = new PEGASE_CONTROLEntities();

            var query = db.HISTORIQUE_CONTROL.Where(i => i.NumOF.Equals(numof));

            if(query != null && query.Count() > 0) { 
                List<HISTORIQUE_CONTROL> l = query.ToList();

                foreach(var i in l)
                {
                    db.HISTORIQUE_CONTROL.Remove(i);
                }
                db.SaveChanges();
                return RedirectToAction("gestionOf", "Production",new { id = IDOPE } );
            }
            return RedirectToAction("SuppressionControleFinal","Controle", new { ID = IDOPE });
        }

        public ActionResult ControleQualite(long? ID,long? IDCQ)
        {
            if (ID == null)
            {
                return RedirectToAction("Production", "Production");
            }
            ControleQualite vue = new ControleQualite(IDCQ,null);
            vue.ID = ID.Value; // Stockez l'ID dans la propriété "ID" de votre modèle
            if (vue.CONTROLE_QUALITEEnCours.NMROF != null)
            {
                vue.CONTROLE_QUALITEEnCours.NMROF = vue.CONTROLE_QUALITEEnCours.NMROF.Trim();
            }
            return View(vue);
        }
        [ActionName("getOperateurs")]
        public ActionResult getOperateurs(string numof)
        {
            InfoUSer datas = new InfoUSer(numof);
            return Json(datas, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ActionName("ControleQualite")]
        public ActionResult AjoutControleQualite(long? ID, HttpPostedFileBase ImageControle)
        {

            ControleQualite obj = new ControleQualite();
            if (Request.Form.Count > 0)
            {
                long? Idqc = null;
                if (Request.Form["refIDBDD"] != null)
                {
                    try { Idqc = Convert.ToInt64(Request.Form["refIDBDD"]); } catch { Idqc = null; }
                }
                string NumOF = Request.Form["refPiece"];
                string srcImgold = Request.Form["srcImgold"];
                string Item = Request.Form["nomArticle"];
                string ItemDescript = Request.Form["nomDescription"];
                string ConformeType = Request.Form["TypeConforme"];
                string ItemAnomalie = Request.Form["Anomalie"];
                string ItemCause = Request.Form["Cause"];
                int TypeConforme = Convert.ToInt32(ConformeType);
                string SaisieDescription = Request.Form["SaisieDescription"];
                string PhotoControle = Request.Form["srcImg"];
                string ImageDB = null;
                string pathimage = "";
                if (!string.IsNullOrWhiteSpace(srcImgold))
                {
                    pathimage = srcImgold;
                }
                else if (ImageControle != null && ImageControle.ContentType.Contains("image/jpeg"))
                {
                    byte[] thePictureAsBytes = new byte[ImageControle.ContentLength];
                    using (BinaryReader theReader = new BinaryReader(ImageControle.InputStream))
                    {
                        thePictureAsBytes = theReader.ReadBytes(ImageControle.ContentLength);
                    }
                    ImageDB = Convert.ToBase64String(thePictureAsBytes);
                    //ImageDB = ImageDB.Remove(0, 23);
                }
                else if (!String.IsNullOrWhiteSpace(PhotoControle))
                {
                    PhotoControle = PhotoControle.Remove(0, 23);
                    ImageDB = PhotoControle;
                }
                else { }
                
                int result = obj.AddControle(ID, Idqc, NumOF, Item, ItemDescript, TypeConforme, ItemAnomalie, ItemCause, SaisieDescription, ImageDB, ref pathimage);

            }
            ControleQualite vue = new ControleQualite();
            vue.ID = ID.Value; // Stockez l'ID dans la propriété "ID" de votre modèle
            return View(vue);
        }
        public ActionResult AfficheControles(long? ID, int? type)
        {
            ControleQualite vue = new ControleQualite(ID,type);
            ViewBag.ID = ID;
            return View(vue);
        }
        public ActionResult SearchOfControle(string nmrof)
        {
         List<ControleFinal> OfSites = ControleFinalOp.GetOFSearch(nmrof);

            return Json(OfSites, JsonRequestBehavior.AllowGet);
        }

        // 2 Mode de clotures : 1 = OF COMPLET, 2 = LIGNE OF
        public ActionResult SoldOF(string nmrof, int quantite, int modecloture = 1)
        {
            // ajouter le solde des of.

            return Json("OK", JsonRequestBehavior.AllowGet);

        }
    }
}