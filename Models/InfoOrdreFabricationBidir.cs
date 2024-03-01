using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class InfoOrdreFabricationBidir
    {
        private List<OrdreFabricationBiDir> _OrdreFabrications;
        public List<OrdreFabricationBiDir> OrdreFabrications
        {
            get
            {
                return _OrdreFabrications;
            }
            set
            {
                _OrdreFabrications = value;
                MiseAJourData();
            }
        }
        public List<OrdreFabricationBiDir> OfEnCours { get; set; }
        public List<OrdreFabricationBiDir> OfEdit { get; set; }
        public List<OrdreFabricationBiDir> OfEnAttente { get; set; }
        public List<OrdreFabricationBiDir> OFSemaineCourante { get; set; }        
        public List<OrdreFabricationBiDir> OFSemainePlus1 { get; set; }
        public List<OrdreFabricationBiDir> OFSemainePlus2 { get; set; }
        public List<OrdreFabricationBiDir> OFSemainePlus3 { get; set; }
        public string SemaineCourante { get; set; }
        public string SemainePlus1 { get; set; }
        public string SemainePlus2 { get; set; }
        public string SemainePlus3 { get; set; }
        public Dictionary<string, int> OfsParPays { get; set; }


        private void MiseAJourData()
        {
            OfEnCours = _OrdreFabrications.Where(p => p.StatusOf == 4).ToList();
            OfEdit = _OrdreFabrications.Where(p => p.StatusOf == 3).ToList();
            OfEnAttente = _OrdreFabrications.Where(p => p.StatusOf == 1).ToList();

            // Gets the Calendar instance associated with a CultureInfo.
            CultureInfo myCI = new CultureInfo("fr-FR");
            Calendar myCal = myCI.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            int NmrWeek = myCal.GetWeekOfYear(DateTime.Now, myCWR, myFirstDOW);
            int num_semaine = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);


            OFSemaineCourante = _OrdreFabrications.Where(p => myCal.GetWeekOfYear(p.DateLivraison, myCWR, myFirstDOW) == NmrWeek).ToList();
            SemaineCourante = "Semaine " +  NmrWeek.ToString();
            
            NmrWeek = myCal.GetWeekOfYear(DateTime.Now.AddDays(7), myCWR, myFirstDOW); ;
            OFSemainePlus1 = _OrdreFabrications.Where(p => myCal.GetWeekOfYear(p.DateLivraison, myCWR, myFirstDOW) == (NmrWeek)).ToList();
            SemainePlus1 = "Semaine " + NmrWeek.ToString();
            
            NmrWeek = myCal.GetWeekOfYear(DateTime.Now.AddDays(14), myCWR, myFirstDOW);
            OFSemainePlus2 = _OrdreFabrications.Where(p => myCal.GetWeekOfYear(p.DateLivraison, myCWR, myFirstDOW) == (NmrWeek)).ToList();
            SemainePlus2 = "Semaine " + NmrWeek.ToString();
            
            NmrWeek = myCal.GetWeekOfYear(DateTime.Now.AddDays(21), myCWR, myFirstDOW);
            OFSemainePlus3 = _OrdreFabrications.Where(p => myCal.GetWeekOfYear(p.DateLivraison, myCWR, myFirstDOW) == (NmrWeek)).ToList();
            SemainePlus3 = "Semaine " + NmrWeek.ToString();

            OfsParPays = new Dictionary<string, int>();
            foreach (var of in _OrdreFabrications)
            {
                string pays = of.Pays_client;
                if (OfsParPays.ContainsKey(pays))
                {
                    int cpt = 0;
                    OfsParPays.TryGetValue(pays, out cpt);
                    OfsParPays[pays] = cpt + 1;
                }
                else
                {
                    OfsParPays.Add(pays, 1);
                }
            }
        }
    }
}