using GenerateurDFUSafir.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DALPEGASE;
using GenerateurDFUSafir.DAL;
using System.Data.Entity.Infrastructure;
using System.Net;
//using GenerateurDFUSafir.data;
using GenerateurDFUSafir.Models.DAL;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Permissions;
using System.ComponentModel;
using OF_PROD_TRAITE = GenerateurDFUSafir.Models.DAL.OF_PROD_TRAITE;
using OPERATEURS = GenerateurDFUSafir.Models.DAL.OPERATEURS;

namespace GenerateurDFUSafir.Controllers
{
    public class OFOperateursController : Controller
    {
        GestionTraitementOFs traitement;
        // GET: Operateurs
        public ActionResult Index()
        {
            traitement = new GestionTraitementOFs();
            return View(traitement);
        }
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "OFOperateurs");
            }
            else
            {
                DataOperateurProd ope = GestionOperateursProd.GestionOFOperateur((long)id,false);
                return View(ope);
            }
        }
        public ActionResult ChangeStatus(string actionof,string of, long? id)
        {
            DataOperateurProd ope = GestionOperateursProd.GestionOFOperateur((long)id, false);
            if (actionof== null)
            { }
            else if (actionof.Equals("DEBUT"))
            {
                OF_PROD_TRAITE of_traite = ope.OfNontraiteNontrace.Where(p => p.NMROF.Contains(of)).First();
                of_traite.STATUSTYPE = "INPROGRESS";
                of_traite.STARTTIME = DateTime.Now;
                of_traite.OPERATEUR = id;
                of_traite.ISALIVE = true;
                OfX3 db = new OfX3();
                //CreateEtiquette(of_traite.NMROF,ope.Nom);
                db.SaveDbOF_PROD_TRAITE(of_traite);
            }
            else if (ope.OfEncours.Where(p => p.NMROF == of && p.ISALIVE != false).Count() > 0)
            {
                OF_PROD_TRAITE of_traite = ope.OfEncours.Where(p => p.NMROF == of && p.ISALIVE != false).First();
                if (of_traite != null)
                {
                    if (actionof.Equals("PAUSE"))
                    {
                        if (of_traite.STATUSTYPE == "INPROGRESS")
                        {
                            of_traite.STATUSTYPE = "ENPAUSE";
                            of_traite.ENDTIME = DateTime.Now;
                        }
                        else
                        {
                            of_traite.STATUSTYPE = "INPROGRESS";
                            //of_traite.STARTTIME = DateTime.Now;
                        }
                    }
                    else if (actionof.Equals("ARRET"))
                    {
                        if (!of_traite.STATUSTYPE.Contains("CLOSED") && of_traite.QTRREEL!=0)
                        {
                            of_traite.STATUSTYPE = "CLOSED";
                            of_traite.ENDTIME = DateTime.Now;
                        }
                    }
                    OfX3 db = new OfX3();
                    db.SaveDbOF_PROD_TRAITE(of_traite);
                }
            }
            return RedirectToAction("Edit", "OFOperateurs", new {id = id });

        }
        public void CreateEtiquette(string of,string nom)
        {
            string path = "\\\\Jaysvrfiles\\donnees_production\\DONNEES_INDUSTRIELLES\\ETIQUETTES\\Etiquettes_scrutation";

            // YVA protection de code

            WrapperImpersonationContext context = new WrapperImpersonationContext("jayelectronique.org", "Product", "2017Pr01");
            context.Enter();
            FileStream fileStream = new FileStream(string.Concat(path, "\\label_OF", of + nom, ".cmd"), FileMode.Create);
                StreamWriter writer = new StreamWriter(fileStream);

                writer.WriteLine(string.Concat("LABELNAME", "=\"", "\\\\Jaysvrfiles\\donnees_production\\DONNEES_INDUSTRIELLES\\ETIQUETTES\\Etiquettes_Models\\OF+NOM.Lab", "\""));
                writer.WriteLine(string.Concat("NOM", "=\"", nom, "\""));
                writer.WriteLine(string.Concat("OF", "=\"", of, "\""));
                writer.WriteLine(string.Concat("PRINTER", "=\"", "BRADY BP-PR 300 plus", "\""));
                writer.WriteLine(string.Concat("LABELQUANTITY", "=\"", "1", "\""));


                writer.Close();
                fileStream.Close();
            context.Leave();
        }
        public ActionResult Alea(string action, string of, long id)
        {
            DataOperateurProd ope = GestionOperateursProd.GestionOFOperateur(id, false);
            GestionTraitementOFs traitement = new GestionTraitementOFs();
            DeclarationAlea aleas = new DeclarationAlea();
            aleas.GestionCodeOF();
            aleas.NmrOF = of;
            //// code OF
            ///
            OF_PROD_TRAITE of_traite = ope.OfEncours.Where(p => p.NMROF == of).First();
            List<string> aleas_code;
            if (of_traite.Alea != null)
            {
                aleas_code = of_traite.Alea.Split(';').ToList();
            }
            else
            {
                aleas_code = new List<string>();
            }
            for(int i= 0;i<aleas.CodeAlea.Count();i++)
            {
                if (aleas_code.Contains(aleas.CodeAlea[i].Code.ToString()))
                {
                    aleas.CodeAlea[i].ischecked = true;
                }
            }
            return View(aleas);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(DataOperateurProd ope)
        {
            DataOperateurProd ope1 = GestionOperateursProd.GestionOFOperateur((long)ope.ID, true);
            string addofmanuel = "";
            if (ope.TryToAddOF!= null)
            {
                addofmanuel = ope.TryToAddOF.ToUpper();
            }
            if (!string.IsNullOrWhiteSpace(addofmanuel) && addofmanuel.Length >= 7)
            {
                List<OF_PROD_TRAITE> listof_traite = ope1.OfNontraiteNontrace.Where(p => p.NMROF.Contains(addofmanuel)).ToList();
                if (listof_traite != null && listof_traite.Count == 1)
                {
                    OF_PROD_TRAITE of_traite = listof_traite.First();
                    of_traite.STATUSTYPE = "INPROGRESS";
                    of_traite.STARTTIME = DateTime.Now;
                    of_traite.OPERATEUR = (long)ope1.ID;
                    of_traite.ISALIVE = true;
                    OfX3 db = new OfX3();
                    db.SaveDbOF_PROD_TRAITE(of_traite);
                }
            }
            if (!String.IsNullOrWhiteSpace(ope.Ilotid.ToString()))
            {
                OfX3 data = new OfX3();
                OPERATEURS Operateur = data.ListOPERATEURs("PROD").Where(p => p.ID == (long)ope.ID).First();
                if (Operateur.POLE == null || !Operateur.POLE.Equals(ope.Ilotid))
                {
                    if (!Operateur.ANIMATEUR)
                    {
                        Operateur.POLE = ope.Ilotid;
                        data.SaveDbOperateur(Operateur);
                    }
                }
            }
            if (ope.OfEncours!=null && ope.OfEncours.Count>0)
            {
                foreach(var of in ope.OfEncours)
                {
                    string nmrof = of.NMROF;
                    int tempssuppl = of.TempsId;
                    int qtrreee = (int)of.QTRREEL;
                    List<OF_PROD_TRAITE> listof_traite = ope1.OfEncours.Where(p => p.NMROF.Contains(nmrof)&& p.ISALIVE == true).ToList();
                    if (listof_traite != null && listof_traite.Count == 1)
                    {
                        OF_PROD_TRAITE of_traite = listof_traite.First();
                        of_traite.TEMPSSUPPL = GestionOperateursProd.returnTimeCas(tempssuppl);
                        of_traite.QTRREEL = qtrreee;
                        OfX3 db = new OfX3();
                        db.SaveDbOF_PROD_TRAITE(of_traite);
                    }
                }
            }
            return RedirectToAction("Edit", "OFOperateurs", new { id = (long)ope.ID });
        }
        [HttpPost, ActionName("Alea")]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAlea(string id,string of, DeclarationAlea aleas)
        {
            DeclarationAlea aleas1 = new DeclarationAlea();
            aleas1.GestionCodeOF();
            DataOperateurProd ope = GestionOperateursProd.GestionOFOperateur(Convert.ToInt32(id), false);
            //// code OF
            OF_PROD_TRAITE of_traite = ope.OfEncours.Where(p => p.NMROF == of).First();
            String result = "";
            for(int i=0;i<aleas.CodeAlea.Count;i++)
            {
                if (aleas.CodeAlea[i].ischecked)
                {
                    result += aleas1.CodeAlea[i].Code.ToString() + ";";
                }
            }
            if (result.Length > 200)
            {
                result = result.Substring(0, 200);
            }
            of_traite.Alea = result;
            OfX3 db = new OfX3();
            db.SaveDbOF_PROD_TRAITE(of_traite);
            return  RedirectToAction("Edit","OFOperateurs", ope);
        }
    }
    public class WrapperImpersonationContext
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain,
        String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        private const int LOGON32_PROVIDER_DEFAULT = 0;
        private const int LOGON32_LOGON_INTERACTIVE = 2;

        private string m_Domain;
        private string m_Password;
        private string m_Username;
        private IntPtr m_Token;

        private WindowsImpersonationContext m_Context = null;


        protected bool IsInContext
        {
            get { return m_Context != null; }
        }

        public WrapperImpersonationContext(string domain, string username, string password)
        {
            m_Domain = domain;
            m_Username = username;
            m_Password = password;
        }

        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public void Enter()
        {
            if (this.IsInContext) return;
            m_Token = new IntPtr(0);
            try
            {
                m_Token = IntPtr.Zero;
                bool logonSuccessfull = LogonUser(
                   m_Username,
                   m_Domain,
                   m_Password,
                   LOGON32_LOGON_INTERACTIVE,
                   LOGON32_PROVIDER_DEFAULT,
                   ref m_Token);
                if (logonSuccessfull == false)
                {
                    int error = Marshal.GetLastWin32Error();
                    throw new Win32Exception(error);
                }
                WindowsIdentity identity = new WindowsIdentity(m_Token);
                m_Context = identity.Impersonate();
            }
            catch (Exception exception)
            {
                // Catch exceptions here
            }
        }


        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public void Leave()
        {
            if (this.IsInContext == false) return;
            m_Context.Undo();

            if (m_Token != IntPtr.Zero) CloseHandle(m_Token);
            m_Context = null;
        }
    }
}