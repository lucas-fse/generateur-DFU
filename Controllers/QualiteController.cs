using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GenerateurDFUSafir.Models;
using GenerateurDFUSafir.Models.DAL;

namespace GenerateurDFUSafir.Controllers
{
    public class QualiteController : Controller
    {
        public QualiteController()
        {

        }
        // GET: Qualite
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Qualite()
        {

            Qualite qualite = new Qualite();
            
            return View(qualite);
        }

        public ActionResult Activite (string id)
        {
            Qualite qualite = new Qualite();
            Processus processus = null;
            bool trouve = false;
            foreach (var pro in qualite.ProcessusTop)
            {
                if (pro.IDProcessus.Equals(id))
                {
                    processus = pro;
                    trouve = true;
                    break;
                }
            }
            if (!trouve)
            {
                foreach (var pro in qualite.ProcessusMiddle)
                {
                    if (pro.IDProcessus.Equals(id))
                    {
                        processus = pro;
                        trouve = true;
                        break;
                    }
                }
            }
            if (!trouve)
            {
                foreach (var pro in qualite.ProcessusBottom)
                {
                    if (pro.IDProcessus.Equals(id))
                    {
                        processus = pro;
                        trouve = true;
                        break;
                    }
                }
            }
            if (trouve)
            {
                return View(processus);
            }
            else
            {
                return RedirectToAction("Qualite");
            }
        }
        public ActionResult Liens (string id)
        {
            Activite activite = new Activite();
            Qualite qualite = new Qualite();
           
            bool trouve = false;
            foreach (var pro in qualite.ProcessusTop)
            {
                foreach (var act in pro.ListActivite)
                {
                    if (act.IDActivite.Equals(id))
                    {
                        activite = act;
                        trouve = true;
                        break;
                    }
                }
            }
            if (!trouve)
            {
                foreach (var pro in qualite.ProcessusMiddle)
                {
                    foreach (var act in pro.ListActivite)
                    {
                        if (act.IDActivite.Equals(id))
                        {
                            activite = act;
                            trouve = true;
                            break;
                        }
                    }
                }
            }
            if (!trouve)
            {
                foreach (var pro in qualite.ProcessusBottom)
                {
                    foreach (var act in pro.ListActivite)
                    {
                        if (act.IDActivite.Equals(id))
                        {
                            activite = act;
                            trouve = true;
                            break;
                        }
                    }
                }
            }
            if (trouve)
            {
                return View(activite);
            }
            else
            {
                return RedirectToAction("Qualite");
            }
            
        }
        
    }
}