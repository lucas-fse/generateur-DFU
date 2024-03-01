using GenerateurDFUSafir.Models.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class SECU_PROD
    {
        public Dictionary<int, AccidentParType> AccidentsParMois { get; set; } // int represente numero du mois
        public Dictionary<int, List<ACCIDENT>> ListAccidentDuMois { get; set; } // int represente les numeros de jours 
        public int NbJourSansAccident { get; set; }
        public int RecordSansAccident { get; set; }
        public int? NbHumeurPositive { get; set; }
        public string NbHumeurPositivestring 
        {
            get
            {
                if (NbHumeurPositive != null)
                {
                    return NbHumeurPositive.ToString();
                }
                else
                {
                    return "-";
                }
            }
        }
        public Dictionary<int,int>  TotalAccidentParType { get; set; }
        public Dictionary<int, string> CouleurCaseParJour { get; set; }
        public string DateAnglaise
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        public string CrossColor { get; set; }
        public string listAccidentsParMois { get; set; }
        public int Day
        {
            get
            {
                return DateTime.Now.Day;
            }
        }


        public SECU_PROD()
        {
            AccidentsParMois = new Dictionary<int, AccidentParType>();
            ListAccidentDuMois = new Dictionary<int, List<ACCIDENT>>();
            TotalAccidentParType = new Dictionary<int, int>();
            DateTime now = DateTime.Now;
            PEGASE_PROD2Entities2 pEGASE_PROD2Entities2 = new PEGASE_PROD2Entities2();
            List<ACCIDENT> accidentParAnnee = pEGASE_PROD2Entities2.ACCIDENT.Where(d => d.Date.Year >= now.Year).ToList();
            NbJourSansAccident =  ((now - pEGASE_PROD2Entities2.DATA_GENERIQUE.Where(i => i.ID == 2).First().Datetime1)).Value.Days;
            RecordSansAccident = (int) pEGASE_PROD2Entities2.DATA_GENERIQUE.Where(i => i.ID == 2).First().Value1;
            DateTime? datetimevalue2 = pEGASE_PROD2Entities2.DATA_GENERIQUE.Where(i => i.ID == 3).First().Datetime1;
            if (datetimevalue2 != null && datetimevalue2 > new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
            {
                NbHumeurPositive = (int?)pEGASE_PROD2Entities2.DATA_GENERIQUE.Where(i => i.ID == 3).First().Value2;
            }
            else
            {
                NbHumeurPositive = null;
            }
            CouleurCaseParJour = new Dictionary<int, string>();
            for (int i =1; i<32;i++)
            {
                List<ACCIDENT> tmp = accidentParAnnee.Where(d => d.Date.Month == now.Month && d.Date.Day == i).ToList();
                int cpt = 0;
                int moy = 0;
                foreach(var acc in tmp)
                {
                    cpt = Math.Max(cpt, acc.Type);
                    moy += acc.Type;
                }
                if (tmp.Count() != 0)
                {
                    moy = moy / tmp.Count();
                }
                else { moy = 0; }
                if (tmp != null)
                {
                    ListAccidentDuMois.Add(i, tmp);
                }
                else
                {
                    ListAccidentDuMois.Add(i, new List<ACCIDENT>());
                }
                string color = "#22b14c";
                switch(cpt)
                {
                    case 0:
                        color = "#22b14c"; //rien � signale
                        break;
                    case 1:
                    case 2:
                        color = "#87CEEB";
                        break;
                    case 3: color =  "#F0C300";//presque accident
                        break;
                    case 4: color = "#FF8C00";//soins benins
                        break;
                    case 5: color = "#CF0A1D";//accident declar�
                        break;
                    case 6: color = "#303030";//accident grave
                        break;
                    default:
                        color = "#32CD32";
                        break;
                }
                if (now.Day < i) { color = ""; }
                CouleurCaseParJour.Add(i, color); 
                
                if (moy <=2)
                {
                    CrossColor = "../image/GreenCross.png";
                }
                else if(moy <= 3)
                {
                    CrossColor = "../image/YellowCross.png";
                }
                else if (moy <= 4)
                {
                    CrossColor = "../image/OrangeCross.png";
                }
                else if (moy <= 5)
                {
                    CrossColor = "../image/RedCross.png";
                }
                else 
                {
                    CrossColor = "../image/BlackCross.png";
                }
                
            }
            for (int t = 1; t < 7;t++) { TotalAccidentParType.Add(t, 0); }
            for (int y =1; y<13;y++)
            {
                AccidentParType acctype = new AccidentParType();
                acctype.ListAccident = new Dictionary<int, List<ACCIDENT>>();
                for (int t = 1; t<7; t++)
                {
                    List<ACCIDENT> listaccidentParType = new List<ACCIDENT>();
                    
                        listaccidentParType = accidentParAnnee.Where(a => a.Date.Month == y && a.Type == t).ToList();
                    
                    acctype.ListAccident.Add(t, listaccidentParType);

                    int value;
                    TotalAccidentParType.TryGetValue(t, out value);
                    value += listaccidentParType.Count();
                    TotalAccidentParType[t] = value;
                }
                AccidentsParMois.Add(y, acctype);
            }
            listAccidentsParMois = JsonConvert.SerializeObject(AccidentsParMois);
        }

        public int AddAccident(string str1,string str2,string str3,string str4,string str5,string service,string gravitepotentiel, string ImageDB, ref string pathimage)
        {
            int  result=0;
            DateTime date = DateTime.Now;
            try
            {
                 date = DateTime.Parse(str2);
                if (date == null) { date = DateTime.Now; }
            }
            catch { if (date == null) { date = DateTime.Now; } }
            try
            {
                int type = Convert.ToInt32(str1);
                PEGASE_PROD2Entities2 pEGASE_PROD2Entities2 = new PEGASE_PROD2Entities2();
                short niveaupotentiel = NiveauGravite(gravitepotentiel);
                ImageConverter _imageConverter = new ImageConverter();

                string name_img =  "";
                string name_path = "";
                Bitmap bitmapImg ;
                if (ImageDB != null)
                {
                     name_img = "ACC_" + DateTime.Now.Ticks.ToString() + ".jpg";
                     name_path = @"C:\inetpub\wwwroot\GenerateurDFUSafir\ImageAccident\" + name_img;
                    byte[] byteImg = Convert.FromBase64String(ImageDB);
                    bitmapImg = (Bitmap)_imageConverter.ConvertFrom(byteImg);

                    //name_path = @"C:\Users\cogne\Desktop\tmp\" + name_img;
                    pathimage = name_path;

                    bitmapImg.Save(name_path, ImageFormat.Jpeg);
                }
                pEGASE_PROD2Entities2.ACCIDENT.Add(new ACCIDENT { Type = (short)type ,Date = date, Description = str3.Replace("\"","'"), Description2 = str4.Replace("\"", "'"), Victime = str5,Service = service,Status = 1,NiveauDeRisque = niveaupotentiel,UrlImage = name_img});
                pEGASE_PROD2Entities2.SaveChanges();
                result = 1;
            }
            catch (Exception e)
            {
                result = -1;
            }
            return result;
        }
        public string NiveauAccident(int niv)
        {
            switch(niv)
            {
                case 6:
                    return "Accident Grave";
                    break;
                case 5:
                    return "Accident Declare";
                    break;
                case 4:
                    return "Soins Benins";
                    break;
                case 3:
                    return "Presque Accident";
                    break;
                case 2:
                    return "Situations dangereuses";
                    break;
                case 1:
                    return "Actes Dangeureux";
                    break;
                default:
                    return "Inconnue";
                    break;
            }
         }
        public short NiveauGravite(string niv)
        {
            if (niv == null)
            {
                return 0;
            }
            else if (niv.ToUpper().Contains("SOIN"))
            {
                return 1;
            }
            else if (niv.ToUpper().Contains("HANDICAP"))
            {
                return 2;
            }
            else if (niv.ToUpper().Contains("ACCIDENT"))
            {
                return 3;
            }
            else if (niv.ToUpper().Contains("DECES"))
            {
                return 4;
            }
            else
            {
                return 0;
            }

        }
    }

    public class AccidentParType // Accident par mois
    {
        public Dictionary<int, List<ACCIDENT>> ListAccident { get; set; } // int represente le type d'accident de 1 a 6;

    }
    
}