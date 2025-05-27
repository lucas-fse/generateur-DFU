using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GenerateurDFUSafir.DAL;
using GenerateurDFUSafir.Models;
using GenerateurDFUSafir.Models.DAL;
using System.Net;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Drawing.Imaging;

namespace GenerateurDFUSafir.Controllers
{
    public class GeneriqueController : Controller
    {

        private static readonly ImageConverter _imageConverter = new ImageConverter();

        public ActionResult GestionOp(string service)
        {     
            
            GestionOp vue = null;
            if (string.IsNullOrWhiteSpace(service))
            {
                vue = new GestionOp();
            }
            else
            {
                vue = new GestionOp(service,"",0);
            }
            return View(vue);
        }
        [HttpPost, ActionName("listespe")]
        public ActionResult listespe()
        {
            string FormId = Request.Form["FiltreSaisieService"];
            //GestionOp vue = new GestionOp(FormId);

            return RedirectToAction("GestionOp", new { service = FormId });
        }
        [HttpPost, ActionName("GestionOp")]
        public ActionResult FormulaireInfoOperateur()
        {
            OfX3 data = new OfX3();
            
            string FormId = Request.Form["Id"];

            string FormNOM = Request.Form["Nom"];
            string FormPrenom = Request.Form["Prenom"];

            string FormService = Request.Form["FormService"];
            string FormSousService = Request.Form["FormSousService"];

            string FormAnim = Request.Form["Anim"];

            string FormAnniv = Request.Form["Anniv"];
            string FormFinContrat = Request.Form["FinContrat"];

            string FormSrcImg = Request.Form["srcImg"];

            OPERATEURS opnew = new OPERATEURS();
            List<OPERATEURS> listoperateurs = new List<OPERATEURS>();
            if (string.IsNullOrWhiteSpace(FormId))
            {
                opnew.ID = 0;
                listoperateurs = data.ListOPERATEURs();
                opnew.INITIAL = FormPrenom.Remove(1).ToUpper() + FormNOM.Remove(1).ToUpper();
                int count = listoperateurs.Where(i => i.INITIAL.Contains( opnew.INITIAL)).Count();
                if (count != 0)
                {
                    opnew.INITIAL = FormPrenom.Remove(1).ToUpper() + FormNOM.Remove(2).ToUpper();
                    count = listoperateurs.Where(i => i.INITIAL.Contains(opnew.INITIAL)).Count();
                    if (count != 0)
                    {
                        opnew.INITIAL = FormPrenom.Remove(1).ToUpper() + FormNOM.Remove(1).ToUpper();
                        for (int cpt = 1; cpt < 10; cpt++)
                        {
                            opnew.INITIAL = opnew.INITIAL.Remove(2) + cpt.ToString();
                            count = listoperateurs.Where(i => i.INITIAL.Contains(opnew.INITIAL)).Count();
                            if (count == 0) { break; }
                        }
                    }
                }
            }
            else
            {
                //On utilise Parse pour forcer le type int à la valeur "FormId"
                try
                {
                    opnew.ID = Int32.Parse(FormId);
                    listoperateurs = data.ListOPERATEURs().Where(p=>p.ID == opnew.ID).ToList();
                    if (listoperateurs!= null && listoperateurs.Count()==1)
                    {
                        opnew.INITIAL = listoperateurs.First().INITIAL;
                    }
                    else 
                    {
                        opnew.INITIAL = null;
                    }
                    
                }
                catch { }
            }


            opnew.NOM = FormNOM;
            opnew.PRENOM = FormPrenom;
            opnew.SERVICE = FormService;
            opnew.SousService  = FormSousService;

            //Test pour savoir si l'opérateur est animateur ou non
            if (Int16.Parse(FormAnim) == 1)
            {
                opnew.ANIMATEUR = true;
            }
            else
            {
                opnew.ANIMATEUR = false;
            }

            if (!string.IsNullOrWhiteSpace(FormAnniv ))
            {
                //La chaine est découpé pour pouvoir créer l'objet DateTim
                string[] splitFormAnniv = FormAnniv.Split('-');
                try
                {                    
                        DateTime dateAnniv = new DateTime(Int16.Parse(splitFormAnniv[0]), Int16.Parse(splitFormAnniv[1]), Int16.Parse(splitFormAnniv[2]));
                        opnew.ANNIVERSAIRE = dateAnniv;                    
                }
                catch
                {
                    opnew.ANNIVERSAIRE = null;
                }
            }
            else
            {
                opnew.ANNIVERSAIRE = null;
            }

            if (!string.IsNullOrWhiteSpace(FormFinContrat ))
            {
                try
                {
                    //La chaine est découpé pour pouvoir créer l'objet DateTime
                    string[] splitFormFinContrat = FormFinContrat.Split('-');

                    DateTime dateFinContrat = new DateTime(Int16.Parse(splitFormFinContrat[0]), Int16.Parse(splitFormFinContrat[1]), Int16.Parse(splitFormFinContrat[2]));
                    opnew.FINCONTRAT = dateFinContrat;
                }
                catch
                {
                    opnew.FINCONTRAT = null;
                }
            }
            else
            {
                opnew.FINCONTRAT = null;
            }

            if (!string.IsNullOrWhiteSpace(FormSrcImg))
            {
                try
                {
                    //Suppression de l'en-tête de la chaine pour la convertire en Bitmap "data:image/jpeg;base64,"
                    FormSrcImg = FormSrcImg.Remove(0, 23);

                    byte[] byteImg = Convert.FromBase64String(FormSrcImg);

                    Bitmap bitmapImg = (Bitmap)_imageConverter.ConvertFrom(byteImg);
                    

                    if (bitmapImg != null && (bitmapImg.HorizontalResolution != (int)bitmapImg.HorizontalResolution ||
                                       bitmapImg.VerticalResolution != (int)bitmapImg.VerticalResolution))
                    {
                        // Correct a strange glitch that has been observed in the test program when converting 
                        //  from a PNG file image created by CopyImageToByteArray() - the dpi value "drifts" 
                        //  slightly away from the nominal integer value
                        bitmapImg.SetResolution((int)(bitmapImg.HorizontalResolution + 0.5f),
                                         (int)(bitmapImg.VerticalResolution + 0.5f));
                    }
                    if (bitmapImg.Height > 200 || bitmapImg.Width >= 150)
                    {
                        int hauteur = Math.Min(200, bitmapImg.Height);
                        int largeur = Math.Min(150, bitmapImg.Width);
                        bitmapImg= cut_rectangle(bitmapImg, (bitmapImg.Width - largeur) /2, (bitmapImg.Height - hauteur) / 2, largeur, hauteur);
                    }
                    string path = @"C:\inetpub\wwwroot\GenerateurDFUSafir\operateurs\OperateursB\" + opnew.INITIAL.Trim() + ".jpg";
                    //bitmapImg.Save("C:\\Users\\bourgeaud\\source\\repos\\GenerateurDFUSafirV14\\operateurs\\OperateursB\\" + opnew.INITIAL + ".jpg", ImageFormat.Jpeg);
                    bitmapImg.Save(path, ImageFormat.Jpeg);
                    opnew.PATHA = "/OperateursB/" + opnew.INITIAL.Trim() + ".jpg";
                }
                catch { }
            }

            data.SaveDbOperateurAll(opnew);

            return RedirectToAction("GestionOp");
        }
        public Bitmap cut_rectangle(Bitmap original_image, int x, int y, int nb_pixel_x, int nb_pixel_y)
        {
            // Rectangle -> x,y points and how many pixel for x and y
            Rectangle rect = new Rectangle(x, y, nb_pixel_x, nb_pixel_y);
            // Build new image
            Bitmap timage;
            timage = new Bitmap(nb_pixel_x, nb_pixel_y); // bitmap size
            timage = original_image.Clone(rect, original_image.PixelFormat);

            return timage;
        }
        // GET: Generique
        public ActionResult Parametres()
        {
            //string ConfigImprimmante = ConfigurationManager.AppSettings["TypeEtiquetteEmballage"];

            ParametresModel param = new ParametresModel(true);
            foreach(var para in param.DataGeneriques)
            {
                para.StringValue1.Trim();
            }
            //param.TypeEtiquetteEmballage = ConfigImprimmante;
            return View( param);
        }
        [HttpPost, ActionName("Parametres")]
        [ValidateAntiForgeryToken]
        public ActionResult ParametresS()
        {
            ParametresModel param = new ParametresModel(null);
           //string[] fieldsToBind = new string[] { "RefComPack", "RefComMO", "RefIndusMO", "Fps", "RefComSim", "RefOptionlogSim", "RefComMT", "RefIndusMT", "RefOptionMaterielMT", "RefOptionMaterielMO" };
                string[] fieldsToBind = new string[] { "StringValue1" };
            foreach(var id in Request.Form.AllKeys)
            {
                string var = Request.Form[id.ToString()].ToString();
                try
                {
                    int IdValue = Convert.ToInt32(id.ToString());
                    param.Save(IdValue, var);
                }
                catch { }
            }
            
             
            return RedirectToAction("Parametres");
        }

        [HttpPost]
        public ActionResult CreerOperateur(
        string Nom,
        string Prenom,
        string Contrat,
        string Service,
        string SousService,
        string Animateur,
        string Administrateur,
        HttpPostedFileBase PhotoProfil)
        {
            try
            {
                // Création de l'entité OPERATEURS
                OPERATEURS nouvelOperateur = new OPERATEURS
                {
                    NOM = Nom,
                    PRENOM = Prenom,
                    FINCONTRAT = string.IsNullOrWhiteSpace(Contrat) ? (DateTime?)null : DateTime.Parse(Contrat),
                    SERVICE = Service,
                    SousService = SousService,
                    ANIMATEUR = Animateur == "on",
                    isAdmin = Administrateur == "on"
                };

                // Appel à la méthode de création
                bool success = OfX3.AddOperateur(nouvelOperateur, PhotoProfil);

                if (!success)
                {
                    ViewBag.Erreur = "Erreur lors de la création de l'opérateur.";
                    return View("ErreurModifOp");
                }

                return RedirectToAction("GestionOp", "Generique");
            }
            catch
            {
                ViewBag.Erreur = "Exception lors de la création.";
                return View("ErreurModifOp");
            }
        }



        [HttpPost]
        public ActionResult ModifierOperateur(
            long IdOperateur,
            string Nom,
            string Prenom,
            string Contrat,
            string Service,
            string SousService,
            string Animateur,
            string Administrateur,
            HttpPostedFileBase PhotoProfil)
        {
            // 1. Mettre à jour dans la BDD via OfX3
            bool success = OfX3.UpdateOperateur(new OPERATEURS
            {
                ID = IdOperateur,
                NOM = Nom,
                PRENOM = Prenom,
                FINCONTRAT = string.IsNullOrWhiteSpace(Contrat) ? (DateTime?)null : DateTime.Parse(Contrat),
                SERVICE = Service,
                SousService = SousService,
                ANIMATEUR = !string.IsNullOrEmpty(Animateur) && Animateur == "on",
                isAdmin = !string.IsNullOrEmpty(Administrateur) && Administrateur == "on",

            }, PhotoProfil);

            if (!success)
            {
                ViewBag.Erreur = "Erreur lors de la mise à jour.";
                return RedirectToAction("ErreurModifOp"); // page d'erreur à créer si besoin
            }

            return RedirectToAction("GestionOp", "Generique");
        }

    }
}