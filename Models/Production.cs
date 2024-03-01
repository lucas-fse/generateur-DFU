using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class Production
    {
        public List<string> ListOperateur { get; set; }
        public int AccidentDuMoisCount { get; set; }
        public int NbJourDuMois
        {
            //get
            //{
            //    return DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            //}
            get;set;
        }
        public Dictionary<int, string> CouleurCaseParJour { get; set; }
        public int NbJourSansAccident { get; set; }
        public int RecordSansAccident { get; set; }
        public int? NbHumeurPositive { get; set; }
        public string NbHumeurPositiveString
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
        private CasesQcrossType[] _casesQcross;
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
        public Dictionary<string, DataGraph> DataFrcParService { get; set; }
        public int Day { get; set; }
        public string PDFAAfficher { get; set; }
        public int NbAmeliorationDernierMois { get; set; }
        public int NbAmeliorationProduction { get; set; }
        public int NbAmeliorationProductionsecurite { get; set; }
        public int OTRHebdomadaire { get; set; }
        public int OTDHebdomadaire { get; set; }


        public Production()
        {
            DateTime now = DateTime.Now;
            NbJourDuMois= DateTime.DaysInMonth(now.Year, now.Month);
            Day = now.Day;
            int Annee = now.Year;
            int num_semaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

             Annee = now.AddDays( -(((int)(now.DayOfWeek + 13)) % 7)).Year;
             num_semaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            DateTime MoinsUnMois = now.AddMonths(-1);
            ListOperateur = new List<string>();
            string path = "C:\\inetpub\\wwwroot\\GenerateurDFUSafir\\operateurs\\MiniOperateurs\\";
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] fileEntries = di.GetFiles("*.png");
            foreach (var fileName in fileEntries)
            {
                ListOperateur.Add(@"/operateurs/MiniOperateurs/" + fileName);
            }

            PEGASE_PROD2Entities2 pEGASE_PROD2Entities2 = new PEGASE_PROD2Entities2();
            AccidentDuMoisCount = pEGASE_PROD2Entities2.ACCIDENT.Where(d => d.Date.Year >= now.Year && d.Date.Month >= now.Month).ToList().Count();
            NbJourSansAccident = ((now - pEGASE_PROD2Entities2.DATA_GENERIQUE.Where(i => i.ID == 2).First().Datetime1)).Value.Days;
            RecordSansAccident = (int)pEGASE_PROD2Entities2.DATA_GENERIQUE.Where(i => i.ID == 2).First().Value1;
            DateTime? datetimevalue2 = pEGASE_PROD2Entities2.DATA_GENERIQUE.Where(i => i.ID == 3).First().Datetime1;
            try
            {
                OTRHebdomadaire = (int)pEGASE_PROD2Entities2.KPI_PROD.Where(i => i.Annee == Annee && i.Semaine == num_semaine).First().OTRByWeek;
                OTDHebdomadaire = (int)pEGASE_PROD2Entities2.KPI_PROD.Where(i => i.Annee == Annee && i.Semaine == num_semaine).First().OTDByWeek;
            }
            catch
            {
                OTRHebdomadaire = -1;
                OTDHebdomadaire = -1;
            }
            if (datetimevalue2 != null && datetimevalue2> new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
            {
                NbHumeurPositive = (int?)pEGASE_PROD2Entities2.DATA_GENERIQUE.Where(i => i.ID == 3).First().Value2;
            }
            else
            {
                NbHumeurPositive = null;
            }
            CouleurCaseParJour = new Dictionary<int, string>();
            for (int i = 1; i < 32; i++)
            {
                List<ACCIDENT> listaccident = pEGASE_PROD2Entities2.ACCIDENT.Where(d => d.Date.Year == now.Year && d.Date.Month == now.Month && d.Date.Day == i).ToList();
                int cpt = 0;
                foreach (var acc in listaccident)
                {
                    cpt = Math.Max(cpt, acc.Type);

                }
                string color = "#22b14c";
                switch (cpt)
                {
                    case 0:
                        color = "#22b14c"; //rien à signale
                        break;
                    case 1:
                    case 2:
                        color = "#87CEEB";
                        break;
                    case 3:
                        color = "#F0C300";//presque accident
                        break;
                    case 4:
                        color = "#FF8C00";//soins benins
                        break;
                    case 5:
                        color = "#CF0A1D";//accident declar�
                        break;
                    case 6:
                        color = "#303030";//accident grave
                        break;
                    default:
                        color = "#32CD32";
                        break;
                }
                if (now.Day < i) { color = ""; }
                CouleurCaseParJour.Add(i, color);
            }
            _casesQcross = new CasesQcrossType[31];
           //for (int i = 0; i < DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
            for (int i = 0; i < 31; i++)
            {
                _casesQcross[i] = new CasesQcrossType();
                _casesQcross[i].Visible = true;
                if (i < now.Day )
                {
                    _casesQcross[i].Couleur = CasesQcrossType.CasesColor.Green;
                }
                else
                {
                    _casesQcross[i].Couleur = CasesQcrossType.CasesColor.Grey;
                }
            }
            for (int i = DateTime.DaysInMonth(now.Year, now.Month); i < 31; i++)
            {
                _casesQcross[i].Visible = false;
            }
            List<FRC> listFRCMonth = pEGASE_PROD2Entities2.FRC.Where(d => d.Datetime.Year == now.Year && d.Datetime.Month == now.Month && d.StatusBDD.Equals(true)).ToList();
            List<FRC> listFRCAll = pEGASE_PROD2Entities2.FRC.Where(p => p.Datetime.Year == now.Year && p.Valide.Equals(true)).ToList();

            int NBMaxFRCPArAn = (int)(pEGASE_PROD2Entities2.DATA_GENERIQUE.Where(i => i.ID == 3).First().Value1);
            foreach (FRC frc in listFRCMonth)
            {
                _casesQcross[frc.Datetime.Day - 1].Couleur = CasesQcrossType.CasesColor.Red;
            }
            DataFrcParService = new Dictionary<string, DataGraph>();
            DataGraph Objectif = new DataGraph();
            Objectif.backgroundColor = Objectif.GetbackgroundColorValue(0);
            Objectif.borderColor = Objectif.GetborderColorValue(0);
            Objectif.Type = "line";
            for (int i = 0; i < 12; i++)
            {
                Objectif.Data[i] = NBMaxFRCPArAn * (i + 1) / 12;
            }
            DataFrcParService.Add("Objectif", Objectif);
            foreach (var frc in listFRCAll)
            {
                int month = frc.Datetime.Month - 1;
                if (DataFrcParService.ContainsKey(frc.Service.Trim()))
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
                    DataFrcParService.Add(frc.Service.Trim(), tmp);
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
            List<AMELIORATION> listAmelioration = pEGASE_PROD2Entities2.AMELIORATION.Where(p => p.Date.Year == Annee).ToList();
            NbAmeliorationDernierMois = listAmelioration.Where(p => p.Date > MoinsUnMois).ToList().Count();
            NbAmeliorationProductionsecurite = listAmelioration.Where(p => p.Type == 7).ToList().Count();
            NbAmeliorationProduction = listAmelioration.Count();

            try
            {
                //var txtFiles = Directory.EnumerateFiles("~/DIFF_GEN", "*.pdf", SearchOption.AllDirectories);
                string dir = "IIS://" + "GenerateurDFUSafir" + "/W3SVC/1/Root/";
                var txtFiles = Directory.EnumerateFiles(@"\\Jaysvrfiles\societe\DIFFUSION_GENERALE", "*.pdf", SearchOption.AllDirectories);
                DateTime currentfileDatetime = new DateTime(200, 01, 01);
                foreach (string currentFile in txtFiles)
                {
                    DateTime filedatetime = File.GetLastWriteTime(currentFile);
                    if (filedatetime> currentfileDatetime)
                    {
                        currentfileDatetime = filedatetime;
                        PDFAAfficher = currentFile;
                    }
                    //string fileName = currentFile.Substring(sourceDirectory.Length + 1);
                    //Directory.Move(currentFile, Path.Combine(archiveDirectory, fileName));
                }
                PDFAAfficher = PDFAAfficher.Replace(@"\\Jaysvrfiles\societe\DIFFUSION_GENERALE", @"http://jaysvriisint:8080/DIFF_GEN");
                //PDFAAfficher = @"http://dell070219:8080/DIFF_GEN/SECURITE/JAY La sécurite 2021 - Novembre.pdf";

                PDFAAfficher = PDFAAfficher.Replace(@"\", @"/");
                PDFAAfficher = PDFAAfficher.Replace(" ", "%20") + "#zoom=78";
             
            }
            catch
            { PDFAAfficher = "erreur"; }
        }
    }
}