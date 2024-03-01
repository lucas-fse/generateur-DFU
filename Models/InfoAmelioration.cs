using GenerateurDFUSafir.Models.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace GenerateurDFUSafir.Models
{
    public class InfoAmelioration
    {
        public Dictionary<string, List<AMELIORATION>> AmeliorationParSecteur { get; set; }
        public Dictionary<string, List<AMELIORATION>> AmeliorationParSujet { get; set; }
        public Dictionary <string, List<AMELIORATION>> AmeliorationParStatus { get; set; }
        public Dictionary<string, Dictionary<bool, List<AMELIORATION>>> AmeliorationParMois { get; set; }//  dictionnaire par mois  dictionnaire realise non realise
        public int NbAmeliorationDernierMois { get; set; }
        public int NbAmeliorationProduction { get; set; }
        public int NbAmeliorationProductionsecurite { get; set; }

        public string ChaineAccidentParMois { get; set; }
        public string ChaineAmeliorationParStatus { get; set; }
        public string ChaineAmeliorationParSecteur { get; set; }
        public string ChaineAmeliorationParSujet { get; set; }
        public List<string> Sujet
        {
            get
            {
                List<string> _sujet = new List<string>() { "Production","Logistique" ,"SAV","BE","Support Tech","Commercial","Marketing","Administratif","Entreprise" };
                return _sujet;
            }
        }
        public List<string> Secteur
        {
            get
            {
                List<string> _secteur = new List<string>() { "5S", "AMELIORATION", "ENVIRONNEMENT", "ERGONOMIE", "MAINTENANCE","QUALITE", "SECURITE", "Autre" };
                return _secteur;
            }
        }



        public InfoAmelioration()
        {
            AmeliorationParSecteur = new Dictionary<string, List<AMELIORATION>>();
            AmeliorationParSujet = new Dictionary<string, List<AMELIORATION>>();
            AmeliorationParStatus = new Dictionary<string, List<AMELIORATION>>();
            AmeliorationParMois = new Dictionary<string, Dictionary<bool, List<AMELIORATION>>>();
            for (int m = 1; m < 13; m++)
            {

                DateTime Date = new DateTime(2021,m,01);
                string mois = Date.ToString("MMMM", CultureInfo.CreateSpecificCulture("fr-FR"));
                Dictionary<bool, List<AMELIORATION>> list = new Dictionary<bool, List<AMELIORATION>>();
                list.Add(false, new List<AMELIORATION>());
                list.Add(true, new List<AMELIORATION>());
                AmeliorationParMois.Add(mois, list);
            }
            int anneeEnCours = DateTime.Now.Year;
            PEGASE_PROD2Entities2 pEGASE_PROD2Entities2 = new PEGASE_PROD2Entities2();
            List<AMELIORATION> listAmelioration = pEGASE_PROD2Entities2.AMELIORATION.Where(p => p.Date.Year == anneeEnCours).ToList();
            DateTime MoinsUnMois = DateTime.Now.AddMonths(-1);
            NbAmeliorationDernierMois = listAmelioration.Where(p => p.Date > MoinsUnMois).ToList().Count();
            NbAmeliorationProductionsecurite = listAmelioration.Where(p => p.Type == 7).ToList().Count();
            NbAmeliorationProduction = listAmelioration.Count();
            int cpt = 0;
            foreach (var amelioration in listAmelioration)
            {
                cpt++;
                if (cpt>75)
                {
                  //  break;
                }
                amelioration.Description = amelioration.Description.Replace("\""," ");
                amelioration.Description2 = amelioration.Description2.Replace("\"", " ");
                amelioration.Emetteur = amelioration.Emetteur.Replace("\"", " ");
                amelioration.Service = amelioration.Service.Replace("\"", " ");
                /*------------------------------------------------*/
                //amelioration.UrlImage = amelioration.UrlImage.Replace("\\", " ");
               

                if (AmeliorationParSecteur.ContainsKey(amelioration.Service.Trim()))
                {
                    List<AMELIORATION> tmp = null;
                    AmeliorationParSecteur.TryGetValue(amelioration.Service.Trim(), out tmp);
                    tmp.Add(amelioration);
                    AmeliorationParSecteur[amelioration.Service.Trim()] = tmp;
                }
                else
                {
                    List<AMELIORATION> tmp = new List<AMELIORATION>();
                    tmp.Add(amelioration);
                    AmeliorationParSecteur.Add(amelioration.Service.Trim(), tmp);
                }
                if (AmeliorationParSujet.ContainsKey(AMELIORATION.StringSujet(amelioration.Type)))
                {
                    List<AMELIORATION> tmp = null;
                    AmeliorationParSujet.TryGetValue(AMELIORATION.StringSujet(amelioration.Type), out tmp);
                    tmp.Add(amelioration);
                    AmeliorationParSujet[AMELIORATION.StringSujet(amelioration.Type)] = tmp;
                }
                else
                {
                    List<AMELIORATION> tmp = new List<AMELIORATION>();
                    tmp.Add(amelioration);
                    AmeliorationParSujet.Add(AMELIORATION.StringSujet(amelioration.Type), tmp);
                }
        
                if (AmeliorationParStatus.ContainsKey(AMELIORATION.StringStatus( amelioration.Status)))
                {
                    List<AMELIORATION> tmp = null;
                    AmeliorationParStatus.TryGetValue(AMELIORATION.StringStatus(amelioration.Status), out tmp);
                    tmp.Add(amelioration);
                    AmeliorationParStatus[AMELIORATION.StringStatus(amelioration.Status)] = tmp;
                }
                else
                {
                    List<AMELIORATION> tmp = new List<AMELIORATION>();
                    tmp.Add(amelioration);
                    AmeliorationParStatus.Add(AMELIORATION.StringStatus(amelioration.Status), tmp);
                }
                string Mois = amelioration.Date.ToString("MMMM", CultureInfo.CreateSpecificCulture("fr-FR"));
                bool realise = false;
                if (amelioration.Status == 1)
                {
                    realise = true;
                }
                Dictionary<bool, List<AMELIORATION>> list = null;
                List<AMELIORATION> listam = null;
                AmeliorationParMois.TryGetValue(Mois, out list);
                list.TryGetValue(realise, out listam);
                listam.Add(amelioration);
                list[realise] = listam;
                AmeliorationParMois[Mois] = list;
            }
            ChaineAccidentParMois = JsonConvert.SerializeObject(AmeliorationParMois);

            ChaineAmeliorationParStatus = JsonConvert.SerializeObject(AmeliorationParStatus);
            ChaineAmeliorationParSecteur = JsonConvert.SerializeObject(AmeliorationParSecteur);
            ChaineAmeliorationParSujet = JsonConvert.SerializeObject(AmeliorationParSujet);


        }

        public int AddAmelioration(string proposition, string solution, string emetteur, string secteur, string service, string date, string ImageDB, ref string pathimage)
        {
            int result = -1;
            try
            {
                if (secteur == null) { secteur = ""; }
                if (service == null ) { service = ""; }
                PEGASE_PROD2Entities2 pEGASE_PROD2Entities2 = new PEGASE_PROD2Entities2();
                DateTime date1 = DateTime.Parse(date);

                ImageConverter _imageConverter = new ImageConverter();
                string name_img = "";
                try
                {
                    name_img = "AM_" + DateTime.Now.Ticks.ToString() + ".jpg";
                    string name_path = @"C:\inetpub\wwwroot\GenerateurDFUSafir\ImageProposition\" + name_img;


                    byte[] byteImg = Convert.FromBase64String(ImageDB);
                    Bitmap bitmapImg = (Bitmap)_imageConverter.ConvertFrom(byteImg);

                    //name_path = @"C:\Users\cogne\Desktop\tmp\" + name_img;
                    pathimage = name_path;

                    bitmapImg.Save(name_path, ImageFormat.Jpeg);
                }
                catch { }
                pEGASE_PROD2Entities2.AMELIORATION.Add(new AMELIORATION { Date = date1, Description = proposition, Description2 = solution, Emetteur = emetteur, Service = secteur, Nmr = -1, Status = 4, Type = AMELIORATION.SujetbyString(service), Input = "WEB", UrlImage = name_img});
                pEGASE_PROD2Entities2.SaveChanges();
                result = 1;
            }
            catch (Exception e)
            {
                result = -1;
            }
            return result;
        }
    }

   
}
   