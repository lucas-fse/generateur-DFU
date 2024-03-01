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
using System.Globalization;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Windows;
using System.Threading;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Interop;
using Image = System.Windows.Controls.Image;
using Size = System.Windows.Size;
using System.Text.RegularExpressions;
using System.Printing;
using Color = System.Windows.Media.Color;
using DLL_Compteur_Mono;
using System.DirectoryServices;
using Util;

namespace GenerateurDFUSafir.Controllers
{
    public class GammeUDController : Controller
    {
        OrdreFabricationMono op;
        // GET: GammeUD
        public ActionResult Index(string of)
        {

            if (op == null)
            {
                op = new OrdreFabricationMono("", " ", new DateTime(), 0, "","");
            }
            //System.Diagnostics.EventLog.WriteEntry("GenerateurDFUSource", "Requete Index" + "Message");
            ViewBag.resultView = false;
            ViewBag.input = "";
            return View(op);
        }

        //public ActionResult Imprimer(string actionof, string of)
        //{
        //    OrdreFabricationMono op = FindOF(of);
        //    ImprimerOF(op);
        //    //return View(FindOF(of));
        //    return RedirectToAction("Index", "GammeUD", new { op1 = op });
        //    // return View(op);
        //}
        public ActionResult CodeId(string of)
        {
            if (op == null)
            {
                op = new OrdreFabricationMono("", " ", new DateTime(), 0, "","");
            }
            //System.Diagnostics.EventLog.WriteEntry("GenerateurDFUSource", "Requete Index" + "Message");
            return View(op);
        }
       
        //public ActionResult Imprimer(string of)
        //{
        //    OrdreFabricationMono op = FindOF(of.ToUpper(), "^UDE|^ADE");
        //    //System.Diagnostics.EventLog.WriteEntry("GenerateurDFUSource", "Requete Imprimer" + "Message");
        //   // ImprimerOF(op);
        //    return RedirectToAction("Index", "GammeUD", new { of = of });
        //}

        [HttpPost]
        public ActionResult Index(string actionOF, string TryToOFUD)
        {
            bool imprimer = !string.IsNullOrWhiteSpace(Request.Form["print"]);
            int qtrAimprime = 0;

            if (imprimer && !string.IsNullOrWhiteSpace(Request.Form["quantite"]))
            {
                qtrAimprime = Int32.Parse(Request.Form["quantite"]);
            }
            ViewBag.input = TryToOFUD.ToUpper();
            
            OrdreFabricationMono op = FindOF(TryToOFUD.ToUpper(), "^UDE|^ADE|^UDR");
            if (!String.IsNullOrWhiteSpace(op.Item) )
            {

                ViewBag.resultView = true;

                if(imprimer)
                {
                    ImprimerOF(op, qtrAimprime);
                }
            }
            else
            {
                ViewBag.resultView = false;
                ViewBag.erreurs = "Numéro d'OF inconnu ou qui ne fait pas parti de la gamme UD.";
            }        


            //op = FindOF(TryToOFUD, "^UDE|^ADE");
            //return RedirectToAction("Index", "GammeUd", TryToOFUD);
            return View(FindOF(TryToOFUD, "^UDE|^ADE|^UDR"));
            
        }
        [HttpPost]
        public ActionResult CodeId(string actionOF, string TryToOFUD)
        {
            // generer les codid pour l'of
            OrdreFabricationMono tmp = FindOF(TryToOFUD, "(^UDR)|(^UCR)|(^URR)");
            if (tmp.ISGenerernewCodeButton)
            {
                for (int c =0; c< tmp.Nbnewcode;c++)
                {
                    int? cpt = CompteurMono.GetNewCompteur("CODE_ID_MONO");
                    if (cpt != null)
                    {
                        tmp.ListNewCode.Add((int)cpt);
                    }
                }
            }
            CompteurMono.GetNewCompteur("CODE_ID_MONO");
            
            return View(FindOF(TryToOFUD, "(^UDR)|(^UCR)|(^URR)"));
        }
        private OrdreFabricationMono FindOF(string TryToOFUD,string regex)
        {
            Regex RegexOf = new Regex("^F.{7}");
            //Regex RegexUDE = new Regex("^UDE");
            Regex RegexUDE = new Regex(regex);
            string TryToOFUDup = TryToOFUD.ToUpper();

            if (!String.IsNullOrWhiteSpace(TryToOFUDup) && RegexOf.IsMatch(TryToOFUDup.Trim()))
            {
                OfX3 odb = new OfX3();
                //List<OrdreFabricationMono> ofs = odb.ListOfMono().ToList();
                List<OrdreFabricationMono> ofs = odb.ListOfMono().Where(p => p.NmrOF.Contains(TryToOFUDup.Trim())).ToList();
                if (ofs != null && ofs.Count > 0)
                {
                    op = new OrdreFabricationMono(ofs.First().NmrOF, ofs.First().Item, ofs.First().Date, ofs.First().Qtr, ofs.First().NmrCmd, ofs.First().CodeId);
                    op.MiseAJourDataProduit();
                }
                else
                {
                    op = new OrdreFabricationMono("Invalide", " ", new DateTime(), 0, "","");
                }
            }
            else if (!String.IsNullOrWhiteSpace(TryToOFUDup) && RegexUDE.IsMatch(TryToOFUDup.Trim()))
            {
                op = new OrdreFabricationMono("", TryToOFUDup.Trim(), new DateTime(), 0, "TEST","");
                op.MiseAJourDataProduit();
            }
            else
            {
                op = new OrdreFabricationMono("Invalide", " ", new DateTime(), 0, "","");
            }
            return op;
        }
        private void ImprimerOF(OrdreFabricationMono op,int qtr)
        {
            // etiquette 
           // using (Impersonation imp = new Impersonation("product", "jayelectronique", "2017Pr01"))
            //{
                EtiquetteUDEUDR(op,qtr);
            //}
            


            if (!op.NmrOF.Contains("Invalide") && op.ImpressionRapport && op.Qtr== qtr) 
            {
                NewThread(op);
            }
            // doc rapport
        }
        private void EtiquetteUDEUDR(OrdreFabricationMono op,int qtr)
        {
            int offset = 0;
            if (qtr != op.Qtr)
            {
                offset = 99 - qtr;
            }
            string path = Resource1.REP_SCRUTATION;
            
            
            int num_semaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            string freq = "";
            bool print_FCC = false;
            if (op.Item.Substring(3, 1).Equals("4") || op.Item.Substring(3, 1).Equals("5"))
            {
                freq = Resource1.INFO_433;
            }
            else if (op.Item.Substring(3, 1).Equals("M") || op.Item.Substring(3, 1).Equals("P"))
            {
                freq = Resource1.INFO_869;
            }
            else if (op.Item.Substring(3, 1).Equals("R") || op.Item.Substring(3, 1).Equals("T"))
            {
                freq = Resource1.INFO_911;
                print_FCC = true; ;
            }
            for (int i = 0; i < qtr; i++)
            {
                DateTime now = DateTime.Now;

                FileStream fileStream = new FileStream(string.Concat(path, "\\label_MO_", op.NmrOF, (i + 1+ offset).ToString("00"), ".cmd"), FileMode.Create);
                StreamWriter writer = new StreamWriter(fileStream);
                string nmrserie = op.NmrOF.Substring(1, op.NmrOF.Length - 1) + (i + 1+ offset).ToString("00");
                writer.WriteLine(string.Concat(StructLabel.LABELNAME, "=\"", Resource1.LABELNAME_FABUDE, "\""));
                writer.WriteLine(string.Concat(StructLabel.DATE, "=\"", num_semaine.ToString("00"), "/", DateTime.Now.ToString("yy"), "\""));
                writer.WriteLine(string.Concat(StructLabel.BANDE_FREQ, "=\"", freq, "\""));
                writer.WriteLine(string.Concat(StructLabel.FCC_ID, "=\"", "\""));
                writer.WriteLine(string.Concat(StructLabel.FOURNISSEUR, "=\"", Resource1.FOURNISSEUR, "\""));
                writer.WriteLine(string.Concat(StructLabel.REF_ENSEMBLE, "=\"", "\""));
                writer.WriteLine(string.Concat(StructLabel.REF_RECEPTEUR, "=\"", "\""));
                writer.WriteLine(string.Concat(StructLabel.COMP_REF_RECEPTEUR, "=\"", "\""));
                writer.WriteLine(string.Concat(StructLabel.NUM_SERIE_RECEPTEUR, "=\"", "\""));
                writer.WriteLine(string.Concat(StructLabel.DOSSIER_RECEPTEUR, "=\"", "\""));
                writer.WriteLine(string.Concat(StructLabel.CODEIDENT_RECEPTEUR, "=\"", "\""));
                writer.WriteLine(string.Concat(StructLabel.ARRET_PASSIF, "=\"", "\""));
                writer.WriteLine(string.Concat(StructLabel.ALIMENTATION, "=\"", "\""));
                writer.WriteLine(string.Concat(StructLabel.PONT_COUPLE_TXT, "=\"", "\""));
                writer.WriteLine(string.Concat(StructLabel.CANAL, "=\"", 1.ToString(), "\""));
                writer.WriteLine(string.Concat(StructLabel.REF_EMETTEUR, "=\"", op.Item, "\""));
                writer.WriteLine(string.Concat(StructLabel.COMP_REF_EMETTEUR, "=\"", op.Compl_Ref_Item, "\""));
                writer.WriteLine(string.Concat(StructLabel.NUM_SERIE_EMETTEUR, "=\"", nmrserie, "\""));
                writer.WriteLine(string.Concat(StructLabel.DOSSIER_EMETTEUR, "=\"", "", "\""));
                writer.WriteLine(string.Concat(StructLabel.CODEIDENT_EMETTEUR, "=\"", "", "\""));
                writer.WriteLine(string.Concat(StructLabel.CODEIDENT_EMETTEUR_2, "=\"", "", "\""));
                writer.WriteLine(string.Concat(StructLabel.OPTION_TEXTE, "=\"", "", "\""));
                writer.WriteLine(string.Concat(StructLabel.NUM_CLE, "=\"", "", "\""));
                writer.WriteLine(string.Concat(StructLabel.REF_CLE, "=\"", "", "\""));
                writer.WriteLine(string.Concat(StructLabel.HOMME_MORT, "=\"", "", "\""));
                writer.WriteLine(string.Concat(StructLabel.FICHE_PERSO, "=\"", "", "\""));
                if (!string.IsNullOrWhiteSpace(op.NmrCmd))
                {
                    writer.WriteLine(string.Concat(StructLabel.AR_OF, "=\"", op.NmrCmd, "\""));
                }
                else
                {
                    writer.WriteLine(string.Concat(StructLabel.AR_OF, "=\"", op.NmrOF, "\""));
                }
                writer.WriteLine(string.Concat(StructLabel.PRINTER, "=\"", "", "\""));
                writer.WriteLine(string.Concat(StructLabel.QUANTITY, "=\"", "1", "\""));
                writer.Close();
                fileStream.Close();
                //if (print_FCC)
                //{
                //    fileStream = new FileStream(string.Concat(path, "\\label_MO_FCC", op.NmrOF, (i + 1).ToString("00"), ".cmd"), FileMode.Create);
                //    writer = new StreamWriter(fileStream);
                //    writer.WriteLine(string.Concat(StructLabel.LABELNAME, "=\"", Resource1.LABELNAME_UDE_FCC, "\""));
                //    writer.WriteLine(string.Concat(StructLabel.PRINTER, "=\"", "", "\""));
                //    writer.WriteLine(string.Concat(StructLabel.QUANTITY, "=\"", "1", "\""));
                //    writer.Close();
                //    fileStream.Close();
                //}
            }
        }

        private static void NewThread(OrdreFabricationMono op)
        {
            Thread thread;
            //System.Diagnostics.EventLog.WriteEntry("GenerateurDFUSource", "Thread" + "Message");
            RapportUDE w = new RapportUDE();
            w.op = op;
            thread = new Thread(new ThreadStart(w.RapportUDE1));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
    class RapportUDE
    {
        public OrdreFabricationMono op;
        public void RapportUDE1()
        {
            //System.Diagnostics.EventLog.WriteEntry("GenerateurDFUSource", "Début du Thread" + "Message");
            // generation page A4 
            List<StackPanel> piedPages = new List<StackPanel>();
            PrintDialog pd = new PrintDialog();
            //PrintServer printsvr = new PrintServer("\\\\SRV-CD-2K16.JAYELECTRONIQUE.ORG");
            PrintServer printsvr = new PrintServer("\\\\CXWFRSAI-IMP.JAYELECTRONIQUE.ORG");
            // PrintServer printsvr = new PrintServer("\\\\DELL08172");
            //pd.PrintQueue = System.Printing.LocalPrintServer.GetDefaultPrintQueue();
            //pd.PrintQueue = new PrintQueue(printsvr, "RDC - Canon C5535i");
            //pd.PrintQueue = new PrintQueue(printsvr, "Dell B2360d");
            pd.PrintQueue = new PrintQueue(printsvr, "ATELIER");
            pd.PrintTicket = pd.PrintQueue.DefaultPrintTicket.Clone();
            Size pageSize = new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);
            //this.Measure(pageSize);
            pd.PrintTicket.PageOrientation = System.Printing.PageOrientation.Portrait;


            FixedDocument document = new FixedDocument();
            document.DocumentPaginator.PageSize = new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);
            FixedPage page1 = new FixedPage();
            Int32 hauteurCalculer = 0;
            page1.Width = document.DocumentPaginator.PageSize.Width;
            page1.Height = document.DocumentPaginator.PageSize.Height;

            StackPanel enveloppe = CreateGuideUDE(op);
            enveloppe.Orientation = Orientation.Vertical; enveloppe.MinHeight = 1070; enveloppe.MaxHeight = 1070;
            page1.Children.Add(enveloppe);
            PageContent page1Content = new PageContent();
            ((IAddChild)page1Content).AddChild(page1);
            // on a joute la page 
            document.Pages.Add(page1Content);
            //System.Diagnostics.EventLog.WriteEntry("GenerateurDFUSource", "Thread avant impression" + "Message");
            pd.PrintDocument(document.DocumentPaginator, "GUIDEASSEMBLAGE");
            
            //System.Diagnostics.EventLog.WriteEntry("GenerateurDFUSource", "Fin du Thread" + "Message");
        }
        private static StackPanel CreateGuideUDE(OrdreFabricationMono op)
        {
            StackPanel result = new StackPanel();
            result.Margin = new System.Windows.Thickness(40, 50, 0, 0);
            StackPanel textblock0 = new StackPanel();
            textblock0.HorizontalAlignment = HorizontalAlignment.Left;
            textblock0.VerticalAlignment = VerticalAlignment.Center;
            textblock0.Width = 780;
            TextBlock textBlock1 = new TextBlock();
            textBlock1.Text = "Guide d'assemblage " + op.Item;
            textBlock1.FontSize = 20;
            textBlock1.FontWeight = System.Windows.FontWeights.Bold;
            textBlock1.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock1.Margin = new Thickness(20, 20, 20, 20);
            TextBlock textBlock2 = new TextBlock();
            textBlock2.Text = op.Item;
            textBlock2.FontSize = 20;
            textBlock2.FontWeight = System.Windows.FontWeights.Bold;
            textBlock2.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock2.Margin = new Thickness(20, 20, 0, 0);
            TextBlock textBlock3 = new TextBlock();
            textBlock3.Text = op.NmrOFAff;
            textBlock3.FontSize = 20;
            textBlock3.FontWeight = System.Windows.FontWeights.Normal;
            textBlock3.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock3.Margin = new Thickness(10, 10, 0, 0);
            TextBlock textBlock4 = new TextBlock();
            textBlock4.Text = op.QtrAff;
            textBlock4.FontSize = 20;
            textBlock4.FontWeight = System.Windows.FontWeights.Normal;
            textBlock4.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock4.Margin = new Thickness(10, 10, 0, 0);
            TextBlock textBlock5 = new TextBlock();
            textBlock5.Text = op.NmrCmdAff;
            textBlock5.FontSize = 20;
            textBlock5.FontWeight = System.Windows.FontWeights.Normal;
            textBlock5.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock5.Margin = new Thickness(10, 10, 0, 0);
            TextBlock textBlock6 = new TextBlock();
            textBlock6.Text = "Date le : "+ op.Date.ToString("D", CultureInfo.CreateSpecificCulture("fr-FR"));
            textBlock6.FontSize = 20;
            textBlock6.FontWeight = System.Windows.FontWeights.Normal;
            textBlock6.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock6.Margin = new Thickness(10, 10, 10, 10);


            textblock0.Children.Add(textBlock1);
            textblock0.Children.Add(textBlock2);
            textblock0.Children.Add(textBlock3);
            textblock0.Children.Add(textBlock4);
            textblock0.Children.Add(textBlock5);
            textblock0.Children.Add(textBlock6);

            StackPanel zoneimage = new StackPanel();
            zoneimage.Orientation = Orientation.Horizontal;
            zoneimage.Margin = new Thickness(20, 20, 20, 20);
            zoneimage.Height = 500;
            zoneimage.Width = 700;
            //zoneimage.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0, 0));

            StackPanel zoneimageText = new StackPanel();
            zoneimageText.Orientation = Orientation.Vertical;
            zoneimageText.Margin = new Thickness(0, 20, 40, 20);
            zoneimageText.HorizontalAlignment = HorizontalAlignment.Left;
            foreach (var lib in op.ListDefProduit)
            {
                TextBlock textBlock7 = new TextBlock();
                textBlock7.Text = lib;
                textBlock7.FontSize = 20;
                textBlock7.FontWeight = System.Windows.FontWeights.Bold;
                textBlock7.HorizontalAlignment = HorizontalAlignment.Left;
                textBlock7.Margin = new Thickness(0, 20, 20, 20);
                zoneimageText.Children.Add(textBlock7);
            }

            zoneimage.Children.Add(zoneimageText);


            Image image1 = new Image();
             var bitmap = new BitmapImage();
            using (var stream = new MemoryStream(op.imageBytes))
            {
               
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
            }
            image1.Stretch = Stretch.Uniform;
            image1.Source = bitmap;
            //image1.Height = 500;
            //image1.Width = 278;
            zoneimage.Children.Add(image1);

            textblock0.Children.Add(zoneimage);
            result.Children.Add(textblock0);

            return result;
        }

    }
    
}