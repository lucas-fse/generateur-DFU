using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using GenerateurDFUSafir.Models.DAL;
using Newtonsoft.Json;

namespace GenerateurDFUSafir.Models
{
    public class FRC_PROD
    {
        public string DateDuJourMois { get; set; }
        public string DateDuJourJour { get; set; }
        public string DateDuJour2 { get; set; }
        public List<FRC> ListeFrcMoisCourant { get; set; }
        public string listeFrcMoisCourantJSON { get { return JsonConvert.SerializeObject(ListeFrcMoisCourant); } } //liste des Frc du mois courant au format JSON
        public List<string> ListOperateur { get; set; }
        public List<FRC> ListFrcNonTraitees { get; set; }
        public int NombreFrcNonTraitees   {get;set;}
        public int JourSansFRC { get; set; }
        public int RecordSansFRC { get; set; }
        public int CoutRebutAnnuel{get;set;        }
        public int QuantiteAPS
        {
            get
            {
                return 0;
            }
        }
        CasesQcrossType[] _casesQcross;
        public CasesQcrossType[] CasesQcross
        {
            get
            {
                if (_casesQcross == null)
                {
                    _casesQcross = new CasesQcrossType[31];
                }
                return _casesQcross;
            }
        }
        public Dictionary<string,DataGraph> DataFrcParService { get; set; }
        public void FRC_PROD_Update()
        {
            DateDuJourMois = DateTime.Now.ToString("MMMM",CultureInfo.CreateSpecificCulture("fr-FR"));
            DateDuJourJour = DateTime.Now.ToString("dd", CultureInfo.CreateSpecificCulture("fr-FR"));
            DateDuJour2 = DateTime.Now.ToString("dddd", CultureInfo.CreateSpecificCulture("fr-FR"));
            MiseAJourOperateur();
            MiseAJourFRC();
        }
        public void Check(List<int> IDS)
        {
            if (IDS != null && IDS.Count > 0)
            {
                PEGASE_PROD2Entities2 pEGASE_PROD2Entities = new PEGASE_PROD2Entities2();
                foreach (var id in IDS)
                {
                    List<FRC> items = pEGASE_PROD2Entities.FRC.Where(i => i.ID == id).ToList();
                    if (items != null && items.Count == 1)
                    {
                        FRC item = items.First();
                        item.StatusBDD = true;
                    }
                }
                pEGASE_PROD2Entities.SaveChanges();
            }
        }
        /// <summary>
        /// recherche des image ds operateur
        /// </summary>
        private void MiseAJourOperateur()
        {
            ListOperateur = new List<string>();
            string path =  "C:\\inetpub\\wwwroot\\GenerateurDFUSafir\\operateurs\\MiniOperateurs\\";
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] fileEntries = di.GetFiles("*.png");
            foreach (var fileName in fileEntries)
            {
                ListOperateur.Add(@"/operateurs/MiniOperateurs/"+ fileName);
            }
        }
        private void MiseAJourFRC()
        {
            PEGASE_PROD2Entities2 pEGASE_PROD2Entities = new PEGASE_PROD2Entities2();
            DateTime FirstDayOfYear = new DateTime(DateTime.Now.Year, 1, 1);
            List<FRC> listFRC = pEGASE_PROD2Entities.FRC.Where(p => p.Datetime > FirstDayOfYear && p.Valide.Equals(true) && (p.Service.ToUpper().Contains("PRODUCT")|| p.Service.ToUpper().Contains("LOGIS") || p.Service.ToUpper().Contains("SAV"))).ToList();
            List<FRC> listFRCAll = pEGASE_PROD2Entities.FRC.Where(p => p.Datetime > FirstDayOfYear && p.Valide.Equals(true)).ToList();
            NombreFrcNonTraitees = listFRC.Where(s => s.StatusBDD.Equals(false)).Count();
            ListFrcNonTraitees = listFRC.Where(i => i.StatusBDD.Equals(false)).ToList();

            List<FRC> listFRCMonth = listFRC.Where(d => d.Datetime >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01) && d.StatusBDD.Equals(true)).ToList();
            ListeFrcMoisCourant = listFRCMonth;

            
            JourSansFRC = ((TimeSpan)(DateTime.Now - pEGASE_PROD2Entities.DATA_GENERIQUE.Where(i=>i.ID ==1).First().Datetime1)).Days;
            RecordSansFRC = (int)(pEGASE_PROD2Entities.DATA_GENERIQUE.Where(i => i.ID == 1).First().Value1);
            CoutRebutAnnuel= (int)(pEGASE_PROD2Entities.DATA_GENERIQUE.Where(i => i.ID == 1).First().Value2);
            int NBMaxFRCPArAn = (int)(pEGASE_PROD2Entities.DATA_GENERIQUE.Where(i => i.ID == 3).First().Value1);
            _casesQcross = new CasesQcrossType[31];
            //for (int i= 0; i<DateTime.DaysInMonth(DateTime.Now.Year,DateTime.Now.Month);i++)
            for (int i = 0; i < 31; i++)
            {
                _casesQcross[i] = new CasesQcrossType();
                _casesQcross[i].Visible = true;
                if (i < DateTime.Now.Day-1)
                {
                    _casesQcross[i].Couleur = CasesQcrossType.CasesColor.Green;
                }
                else
                {
                    _casesQcross[i].Couleur = CasesQcrossType.CasesColor.Grey;
                }
            }
            for (int i = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i<31;i++)
            {
                _casesQcross[i].Visible = false;
            }
            foreach(FRC frc in listFRCMonth)
            {
                _casesQcross[frc.Datetime.Day-1].Couleur = CasesQcrossType.CasesColor.Red;
            }

            DataFrcParService = new Dictionary<string, DataGraph>();
            DataGraph Objectif = new DataGraph();
            Objectif.backgroundColor = Objectif.GetbackgroundColorValue(0);
            Objectif.borderColor = Objectif.GetborderColorValue(0);
            Objectif.Type = "line";
            for (int i =0;i<12;i++)
            {
                Objectif.Data[i] = NBMaxFRCPArAn * (i + 1) / 12;
            }
            DataFrcParService.Add("Objectif", Objectif);
            foreach (var frc in listFRCAll)
            {
                int month = frc.Datetime.Month-1;
                if ( DataFrcParService.ContainsKey( frc.Service.Trim()))
                {
                    DataGraph tmp = null;
                    DataFrcParService.TryGetValue(frc.Service.Trim(), out tmp);
                    tmp.Data[month] += 1;
                    DataFrcParService[frc.Service.Trim()] = tmp;
                }
                else
                {
                    DataGraph tmp = new DataGraph();
                    int nbvalue = DataFrcParService.Count();
                    tmp.backgroundColor = tmp.GetbackgroundColorValue(nbvalue);
                    tmp.borderColor = tmp.GetborderColorValue(nbvalue);
                    tmp.Data[month] += 1;
                    tmp.Type = "bar";
                    DataFrcParService.Add(frc.Service.Trim(),  tmp);
                }
            }
            foreach (var service in DataFrcParService)
            {
                int somme = 0;
                if (service.Key != "Objectif")
                {
                    for (int i = 0; i < 12; i++)
                    {
                        somme = somme + service.Value.Data[i];
                        service.Value.Data[i] = somme;
                    }
                }
            }
        }
    }

    public class CasesQcrossType
    {
        public enum CasesColor
        {
            Red = 0,
            Grey = 1,
            Green = 2
        }
        public bool Visible { get; set; }
        
        public CasesColor Couleur { get; set; }
        public string Couleurstring 
        { 
            get 
            {  
                if (Couleur.Equals(CasesColor.Green))
                    return "vert";
                else if (Couleur.Equals(CasesColor.Grey))
                    return "gris";
                else if(Couleur.Equals(CasesColor.Red))
                    return "rouge";
                else
                {
                    return "";
                }
            } 
        }
    }

    public class DataGraph
    {
        public string GetbackgroundColorValue(int i)
        {
            return (backgroundColorValue[i % backgroundColorValue.Count()]);
        }
        public string GetborderColorValue(int i)
        {
            return (borderColorValue[i % borderColorValue.Count()]);
        }
        private string[] backgroundColorValue = new string[] {
            "rgba(255, 99, 132, 0.7)",
            "rgba(54, 162, 235, 0.7)",
            "rgba(255, 150, 0, 0.7)",
            "rgba(153, 102, 255, 0.7)",
            "rgba(75, 192, 192, 0.7)",
            "rgba(255, 0, 0, 0.7)",
            "rgba(0,255,255,0.7)",
            "rgba(0, 0, 0, 0.65)",
            "rgba(127, 0, 255, 0.8)",
            "rgba(0, 255, 0, 0.4)",
        };
        private string[] borderColorValue = new string[] {
            "rgba(255, 99, 132, 1)",
            "rgba(54, 162, 235, 1)",
            "rgba(255, 206, 86, 1)",
            "rgba(153, 102, 255, 1)",
            "rgba(75, 192, 192, 1)",
            "rgba(255, 159, 64, 1)",
            "rgba(200, 200, 200, 1)",
            "rgba(0, 0, 0, 1)",
            "rgba(0, 255, 0, 1)",
            "rgba(0, 255, 0, 1)",
        };
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public int[] Data { get; set; } = new int[12];
        public string Type { get; set; }
    }
}