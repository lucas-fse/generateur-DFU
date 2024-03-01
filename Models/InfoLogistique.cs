using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using GenerateurDFUSafir.DAL;
using GenerateurDFUSafir.Models.DAL;

namespace GenerateurDFUSafir.Models
{
    public class InfoLogistique
    {
        private PEGASE_STAMPEntities db;
        public Dictionary<string,int> TempsTraitementBLDernier7Jours;
        public Dictionary<string, int> TempsTraitementBLDernier4Sem;
        public Dictionary<string,int> TempsTraitementBLDernier12Mois;
        public Dictionary<string,int> TempsTraitementPackingBLDernier7Jours ;
        public Dictionary<string,int> TempsTraitementPackingBLDernier4Sem;
        public Dictionary<string,int> TempsTraitementPackingBLDernier12Mois;
        public Dictionary<string,int> TempsTraitementPickingBLDernier7Jours;
        public Dictionary<string,int> TempsTraitementPickingBLDernier4Sem;
        public Dictionary<string,int> TempsTraitementPickingBLDernier12Mois;
        public Dictionary<string,int> TempsTraitementArtDernier7Jours;
        public Dictionary<string,int> TempsTraitementArtDernier4Sem;
        public Dictionary<string,int> TempsTraitementArtDernier12Mois;
        public Dictionary<string, int> TempsTraitementPackingArtDernier7Jours;
        public Dictionary<string, int> TempsTraitementPackingArtDernier4Sem;
        public Dictionary<string, int> TempsTraitementPackingArtDernier12Mois;
        public Dictionary<string, int> TempsTraitementPickingArtDernier7Jours;
        public Dictionary<string, int> TempsTraitementPickingArtDernier4Sem;
        public Dictionary<string, int> TempsTraitementPickingArtDernier12Mois;
        public Dictionary<string, int> TempsTraitementArtByBLDernier7Jours;
        public Dictionary<string, int> TempsTraitementArtByBLDernier4Sem;
        public Dictionary<string, int> TempsTraitementArtByBLDernier12Mois;
        public Dictionary<string, int> NbTraitementArtTraiteDernier7Jours ;
        public Dictionary<string, int> NbTraitementArtTraiteDernier4Sem ;
        public Dictionary<string, int> NbTraitementArtTraiteDernier12Mois;
        public Dictionary<string, int> NbTraitementBlTraiteDernier7Jours;
        public Dictionary<string, int> NbTraitementBlTraiteDernier4Sem;
        public Dictionary<string, int> NbTraitementBlTraiteDernier12Mois;
        public Dictionary<string, Indicateur> IndicateurPerWeek;
        public float OTRByWeek = 0;
        public float OTDByWeek = 0;

        public void MiseAJourData()
        {
            TempsTraitementBLDernier7Jours= new Dictionary<string,int>();
            TempsTraitementBLDernier4Sem = new Dictionary<string,int>();
            TempsTraitementBLDernier12Mois = new Dictionary<string,int>();

            TempsTraitementPackingBLDernier7Jours = new Dictionary<string,int>();
            TempsTraitementPackingBLDernier4Sem = new Dictionary<string,int>();
            TempsTraitementPackingBLDernier12Mois = new Dictionary<string,int>();

            TempsTraitementPickingBLDernier7Jours = new Dictionary<string,int>();
            TempsTraitementPickingBLDernier4Sem = new Dictionary<string,int>();
            TempsTraitementPickingBLDernier12Mois = new Dictionary<string,int>();

            TempsTraitementArtDernier7Jours = new Dictionary<string,int>();
            TempsTraitementArtDernier4Sem = new Dictionary<string,int>();
            TempsTraitementArtDernier12Mois = new Dictionary<string,int>();

            TempsTraitementPackingArtDernier7Jours = new Dictionary<string, int>();
            TempsTraitementPackingArtDernier4Sem = new Dictionary<string, int>();
            TempsTraitementPackingArtDernier12Mois = new Dictionary<string, int>();

            TempsTraitementPickingArtDernier7Jours = new Dictionary<string, int>();
            TempsTraitementPickingArtDernier4Sem = new Dictionary<string, int>();
            TempsTraitementPickingArtDernier12Mois = new Dictionary<string, int>();

            TempsTraitementArtByBLDernier7Jours = new Dictionary<string, int>();
            TempsTraitementArtByBLDernier4Sem = new Dictionary<string, int>();
            TempsTraitementArtByBLDernier12Mois = new Dictionary<string, int>();

            NbTraitementArtTraiteDernier7Jours = new Dictionary<string, int>();
            NbTraitementArtTraiteDernier4Sem = new Dictionary<string, int>();
            NbTraitementArtTraiteDernier12Mois = new Dictionary<string, int>();

            NbTraitementBlTraiteDernier7Jours = new Dictionary<string, int>();
            NbTraitementBlTraiteDernier4Sem = new Dictionary<string, int>();
            NbTraitementBlTraiteDernier12Mois = new Dictionary<string, int>();

            IndicateurPerWeek = new Dictionary<string, Indicateur>();

            OTRByWeek = 0;
            OTDByWeek = 0; 
            DateTime Maintenant = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day);
            db = new PEGASE_STAMPEntities();
            var query = db.TRACABILITE_BL.Where(p => p.FirstTimeStart != null && p.TimeStart != null && p.TimeEnd != null) ;



            DateTime plage = Maintenant.AddDays(-10);
            List<TRACABILITE_BL>  ListBL = query.Where(p => p.FirstTimeStart > plage).OrderBy(y => y.FirstTimeStart).ToList();
            CalculMetrique7Jours(ListBL, Maintenant,
                                out TempsTraitementPickingBLDernier7Jours,
                                out TempsTraitementPickingArtDernier7Jours,
                                out TempsTraitementPackingBLDernier7Jours,
                                out TempsTraitementPackingArtDernier7Jours,
                                out TempsTraitementBLDernier7Jours,
                                out TempsTraitementArtDernier7Jours,
                                out TempsTraitementArtByBLDernier7Jours,
                                out NbTraitementArtTraiteDernier7Jours,
                                out NbTraitementBlTraiteDernier7Jours);
            plage = Maintenant.AddDays(-30);
            ListBL = query.Where(p => p.FirstTimeStart > plage).OrderBy(y => y.FirstTimeStart).ToList();

            CalculMetrique4Semaines(ListBL, Maintenant,
                                out TempsTraitementPickingBLDernier4Sem,
                                out TempsTraitementPickingArtDernier4Sem,
                                out TempsTraitementPackingBLDernier4Sem,
                                out TempsTraitementPackingArtDernier4Sem,
                                out TempsTraitementBLDernier4Sem,
                                out TempsTraitementArtDernier4Sem,
                                out TempsTraitementArtByBLDernier4Sem,
                                out NbTraitementArtTraiteDernier4Sem,
                                out NbTraitementBlTraiteDernier4Sem);
            plage = Maintenant.AddDays(-370);
            ListBL = query.Where(p => p.FirstTimeStart > plage).OrderBy(y => y.FirstTimeStart).ToList();
            CalculMetrique12Mois(ListBL, Maintenant,
                                out TempsTraitementPickingBLDernier12Mois,
                                out TempsTraitementPickingArtDernier12Mois,
                                out TempsTraitementPackingBLDernier12Mois,
                                out TempsTraitementPackingArtDernier12Mois,
                                out TempsTraitementBLDernier12Mois,
                                out TempsTraitementArtDernier12Mois,
                                out TempsTraitementArtByBLDernier12Mois,
                                out NbTraitementArtTraiteDernier12Mois,
                                out NbTraitementBlTraiteDernier12Mois);

            CalculMetriquePerWeek(ListBL, Maintenant, out IndicateurPerWeek);
        }

        private void CalculMetriquePerWeek(List<TRACABILITE_BL> ListBLinput, DateTime Maintenant,
            out Dictionary<string, Indicateur> ResultIndicateurByWeek)
        {
            float tmp_otd = 0;
            float tmp_otr = 0;
            int nb_bl = 0;
            ResultIndicateurByWeek = new Dictionary<string, Indicateur>();
            DateTime CeMatin = Maintenant;
            while (CeMatin.DayOfWeek != DayOfWeek.Monday)
            {
                CeMatin = CeMatin.AddDays(-1);
            }
            CeMatin = CeMatin.AddDays(14);
            for (int j = 0; j < 52; j++)
            {
                CeMatin = CeMatin.AddDays(-7);
                DateTime CeMatinMoins1 = CeMatin.AddDays(-7);
                Indicateur tmp = new Indicateur();
                tmp.NbBL = 0;
                List<TRACABILITE_BL> ListBL = ListBLinput.Where(p => (p.FirstTimeStart > CeMatinMoins1) && (p.FirstTimeStart < CeMatin)).OrderBy(y => y.FirstTimeStart).ToList();
                if (ListBL.Count > 0)
                {
                    foreach (var item in ListBL)
                    {
                        if (item.TimeEnd != null && item.TimeSendPlan != null)
                        {
                            tmp.NbBL+= (int)item.NbArtByBl;
                            if (item.NbArtByBl != null)
                            {
                                tmp.NbArt += (int)item.NbArtByBl;
                            }
                            int totalDays = 0;
                            //if (item.TimeEnd != null && item.TimeSendPlan != null)
                            //{
                            NBDaysbetweenTwoDate((DateTime)item.TimeSendPlan, (DateTime)item.TimeEnd, out totalDays);
                            if (totalDays == 0)
                            {
                                tmp.OTD += (float)item.NbArtByBl;
                            }
                            else
                            {

                            }
                            NBDaysbetweenTwoDate((DateTime)item.TimeSendRequested, (DateTime)item.TimeEnd, out totalDays);
                            if (totalDays == 0)
                            {
                                tmp.OTR += (float)item.NbArtByBl;
                            }
                            else
                            {

                            }

                            //if (item.TimeSendRequested != null && item.TimeSendPlan != null && item.TimeEnd!=null)
                            //{
                            DateTime startDate = (DateTime)item.TimeSendRequested;
                            DateTime endDate = ((DateTime)item.TimeEnd).AddDays(-1);
                            totalDays = 0;
                            NBDaysbetweenTwoDate(startDate, endDate, out totalDays);
                            tmp.NbjourRequestBeforStandard += (float)totalDays;
                            if (totalDays > 0) { tmp.NbBLRequestBeforStandard++; }

                            //if (item.TimeEnd != null && item.TimeSendPlan != null && item.TimeEnd != null)
                            //{
                            startDate = (DateTime)item.TimeEnd;
                            endDate = ((DateTime)item.TimeSendPlan).AddDays(-1);
                            totalDays = 0;
                            NBDaysbetweenTwoDate(startDate, endDate, out totalDays);
                            tmp.NbjourSendBeforeStandard += (float)totalDays;
                            if (totalDays > 0) { tmp.NbBLRequestBeforStandard++; }

                            if (item.Transporteur != null)
                            {
                                if (item.Transporteur.Trim().Equals("DPD"))
                                {
                                    tmp.NbBlByDPD++;
                                }
                                else if (item.Transporteur.Trim().Equals("DHL"))
                                {
                                    tmp.NbBlByDHL++;
                                }
                                else if (item.Transporteur.Trim().Equals("TNT"))
                                {
                                    tmp.NbBlByTNT++;
                                }
                                else if (item.Transporteur.Trim().Equals("UPS"))
                                {
                                    tmp.NbBlByUPS++;
                                }
                                else
                                {
                                    tmp.NbBlByAutre++;
                                }
                            }
                        }
                    }
                    tmp_otd += tmp.OTD;
                    tmp_otr += tmp.OTR;
                    nb_bl += tmp.NbBL;
                    if (tmp.NbArt != 0)
                    {
                        tmp.OTD = tmp.OTD / tmp.NbArt * 100;
                        tmp.OTR = tmp.OTR / tmp.NbArt * 100;
                    }
                    else
                    {
                        tmp.OTD = 0;
                        tmp.OTR = 0;
                    }
                    ResultIndicateurByWeek.Add(NmrSemaine(CeMatinMoins1).ToString(), tmp);
                }
            }
            OTDByWeek = tmp_otd/ nb_bl*100;
            OTRByWeek = tmp_otr/ nb_bl*100;
        }
        private void NBDaysbetweenTwoDate(DateTime start,DateTime end, out int NbDays)
        {
            NbDays = 0;
            DateTime tmpEnd = new DateTime(end.Year, end.Month, end.Day);
            while (start < tmpEnd)
            {
                if (start.DayOfWeek == DayOfWeek.Saturday
                    || start.DayOfWeek == DayOfWeek.Sunday)
                {
                    start = start.AddDays(1);

                }
                else
                {

                    start = start.AddDays(1);
                    NbDays++;
                }
            }
        }
        private void CalculMetrique7Jours(List<TRACABILITE_BL> ListBLinput,DateTime Maintenant,
            out Dictionary<string, int> Resut7JPickingByBL, 
            out Dictionary<string, int> Resut7JPickingByArt, 
            out Dictionary<string, int> Resut7JPackingByBL, 
            out Dictionary<string, int> Resut7JPackingByArt, 
            out Dictionary<string, int> Resut7JByBL, 
            out Dictionary<string, int> Resut7JByArt,
            out Dictionary<string,int> Result7JArtByBL,
            out Dictionary<string, int> Result7JArtTraiteBL,
            out Dictionary<string, int> Result7JBlTraiteBL)
        {
            int NbBl = 0;
            int NbPicking = 0;
            int NbArticle = 0;
            int Nbpacking = 0;
            Resut7JPickingByBL = new Dictionary<string, int>();
            Resut7JPickingByArt = new Dictionary<string, int>();
            Resut7JPackingByBL = new Dictionary<string, int>();
            Resut7JPackingByArt = new Dictionary<string, int>();
            Resut7JByBL = new Dictionary<string, int>();
            Resut7JByArt = new Dictionary<string, int>();
            Result7JArtByBL = new Dictionary<string, int>();
            Result7JArtTraiteBL = new Dictionary<string, int>();
            Result7JBlTraiteBL = new Dictionary<string, int>();

            TimeSpan TempsBls = new TimeSpan(0);
            TimeSpan TempsPicking = new TimeSpan(0);
            TimeSpan TempsPacking = new TimeSpan(0);
            DateTime CeMatin = Maintenant;
            int jourOuvre = 0;
            while (jourOuvre<6)
            {
                CeMatin = CeMatin.AddDays(-1);
                if (CeMatin.DayOfWeek!=DayOfWeek.Saturday && CeMatin.DayOfWeek != DayOfWeek.Sunday)
                {
                    jourOuvre++;
                }
            }
            
            for (; CeMatin <= Maintenant; )
            {
                DateTime CeMatinPlus1 = CeMatin.AddDays(1);
                List<TRACABILITE_BL> ListBL = ListBLinput.Where(p => (p.FirstTimeStart > CeMatin) && (p.FirstTimeStart < CeMatinPlus1)).OrderBy(y => y.FirstTimeStart).ToList();
                if (ListBL.Count > 0)
                {
                    DateTime firststart = (DateTime)ListBL.First().FirstTimeStart;
                    DateTime MinTimestart = (DateTime)ListBL.First().TimeStart;
                    TempsBls = new TimeSpan(0);
                    TempsPicking = new TimeSpan(0);
                    TempsPacking = new TimeSpan(0);
                    NbPicking = 0;
                    NbArticle = 0;
                    bool lastbltraite = false;
                    TRACABILITE_BL lastBl = new TRACABILITE_BL();
                    foreach (var bl in ListBL)
                    {
                        NbPicking++;
                        TempsPacking += (DateTime)bl.TimeEnd - (DateTime)bl.TimeStart;

                        if (bl.NbArtByBl != null)
                        { NbArticle += (int)bl.NbArtByBl; }
                        lastbltraite = false;
                        if (firststart.Equals(bl.FirstTimeStart))
                        {
                            if (bl.TimeStart < MinTimestart)
                            { MinTimestart = (DateTime)bl.TimeStart; }
                            lastbltraite = true;
                        }
                        else
                        {
                            TempsPicking += MinTimestart - firststart;
                            firststart = (DateTime)bl.FirstTimeStart;
                            MinTimestart = (DateTime)bl.TimeStart;
                            lastBl = bl;
                        }
                    }
                    if (!lastbltraite)
                    {
                        TempsPicking += (DateTime)lastBl.TimeStart - firststart;
                    }
                    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("fr-FR");
                    Resut7JPickingByBL.Add(String.Format(culture, "{0:dddd}-{1}", CeMatin, CeMatin.Day), (int)(TempsPicking.TotalSeconds / NbPicking));
                    Resut7JPickingByArt.Add(String.Format(culture, "{0:dddd}-{1}", CeMatin, CeMatin.Day), (int)(TempsPicking.TotalSeconds / NbArticle));
                    Resut7JPackingByBL.Add(String.Format(culture, "{0:dddd}-{1}", CeMatin, CeMatin.Day), (int)(TempsPacking.TotalSeconds / NbPicking));
                    Resut7JPackingByArt.Add(String.Format(culture, "{0:dddd}-{1}", CeMatin, CeMatin.Day), (int)(TempsPacking.TotalSeconds / NbArticle));
                    Resut7JByBL.Add(String.Format(culture, "{0:dddd}-{1}", CeMatin, CeMatin.Day), (int)(TempsPacking.TotalSeconds / NbPicking)+ (int)(TempsPicking.TotalSeconds / NbPicking));
                    Resut7JByArt.Add(String.Format(culture, "{0:dddd}-{1}", CeMatin, CeMatin.Day), (int)(TempsPacking.TotalSeconds / NbArticle)+ (int)(TempsPicking.TotalSeconds / NbArticle));
                    Result7JArtTraiteBL.Add(String.Format(culture, "{0:dddd}-{1}", CeMatin, CeMatin.Day), NbArticle);
                    Result7JBlTraiteBL.Add(String.Format(culture, "{0:dddd}-{1}", CeMatin, CeMatin.Day), NbPicking);
                }
                CeMatin = CeMatin.AddDays(1);
            }
        }
        private void CalculMetrique4Semaines(List<TRACABILITE_BL> ListBLinput, DateTime Maintenant,
               out Dictionary<string, int> Resut7JPickingByBL,
               out Dictionary<string, int> Resut7JPickingByArt,
               out Dictionary<string, int> Resut7JPackingByBL,
               out Dictionary<string, int> Resut7JPackingByArt,
               out Dictionary<string, int> Resut7JByBL,
               out Dictionary<string, int> Resut7JByArt,
               out Dictionary<string, int> Result7JArtByBL,
               out Dictionary<string, int> Result7JArtTraiteBL,
               out Dictionary<string, int> Result7JBlTraiteBL)
        {
            int NbBl = 0;
            int NbPicking = 0;
            int NbArticle = 0;
            int Nbpacking = 0;
            Resut7JPickingByBL = new Dictionary<string, int>();
            Resut7JPickingByArt = new Dictionary<string, int>();
            Resut7JPackingByBL = new Dictionary<string, int>();
            Resut7JPackingByArt = new Dictionary<string, int>();
            Resut7JByBL = new Dictionary<string, int>();
            Resut7JByArt = new Dictionary<string, int>();
            Result7JArtByBL = new Dictionary<string, int>();
            Result7JArtTraiteBL = new Dictionary<string, int>();
            Result7JBlTraiteBL = new Dictionary<string, int>();


            TimeSpan TempsBls = new TimeSpan(0);
            TimeSpan TempsPicking = new TimeSpan(0);
            TimeSpan TempsPacking = new TimeSpan(0);
            
            DateTime CeMatin = Maintenant;
            while (CeMatin.DayOfWeek != DayOfWeek.Monday)
            {
                CeMatin = CeMatin.AddDays(-1);
            }
            CeMatin = CeMatin.AddDays(-28);
            for (int j = 0; j < 4; j++)
            {
                DateTime CeMatinPlus1 = CeMatin.AddDays(7);
                List<TRACABILITE_BL> ListBL = ListBLinput.Where(p => (p.FirstTimeStart > CeMatin) && (p.FirstTimeStart < CeMatinPlus1)).OrderBy(y => y.FirstTimeStart).ToList();
                if (ListBL.Count > 0)
                {
                    DateTime firststart = (DateTime)ListBL.First().FirstTimeStart;
                    DateTime MaxTimestart = (DateTime)ListBL.First().TimeStart;
                    TempsBls = new TimeSpan(0);
                    TempsPicking = new TimeSpan(0);
                    TempsPacking = new TimeSpan(0);
                    NbPicking = 0;
                    NbArticle = 0;
                    bool lastbltraite = false;
                    TRACABILITE_BL lastBl = new TRACABILITE_BL();
                    foreach (var bl in ListBL)
                    {
                        NbPicking++;
                        TempsPacking += (DateTime)bl.TimeEnd - (DateTime)bl.TimeStart;

                        if (bl.NbArtByBl != null)
                        { NbArticle += (int)bl.NbArtByBl; }
                        lastbltraite = false;
                        if (firststart.Equals(bl.FirstTimeStart))
                        {
                            if (bl.TimeStart < MaxTimestart)
                            { MaxTimestart = (DateTime)bl.TimeStart; }
                            lastbltraite = true;
                        }
                        else
                        {
                            TempsPicking += MaxTimestart - firststart;
                            firststart = (DateTime)bl.FirstTimeStart;
                            MaxTimestart = (DateTime)bl.TimeStart;
                            lastBl = bl;
                        }
                    }
                    if (!lastbltraite)
                    {
                        TempsPicking += (DateTime)lastBl.TimeStart - firststart;
                    }

                    
                    int NmrWeek = NmrSemaine(CeMatin);

                    int num_semaine = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(CeMatin, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                    Resut7JPickingByBL.Add(String.Format( "Semaine {0}", NmrWeek), (int)(TempsPicking.TotalSeconds / NbPicking));
                    Resut7JPickingByArt.Add(String.Format("Semaine {0}", NmrWeek), (int)(TempsPicking.TotalSeconds / NbArticle));
                    Resut7JPackingByBL.Add(String.Format("Semaine {0}", NmrWeek), (int)(TempsPacking.TotalSeconds / NbPicking));
                    Resut7JPackingByArt.Add(String.Format("Semaine {0}", NmrWeek), (int)(TempsPacking.TotalSeconds / NbArticle));
                    Resut7JByBL.Add(String.Format("Semaine {0}", NmrWeek), (int)(TempsPacking.TotalSeconds / NbPicking) + (int)(TempsPicking.TotalSeconds / NbPicking));
                    Resut7JByArt.Add(String.Format("Semaine {0}", NmrWeek), (int)(TempsPacking.TotalSeconds / NbArticle) + (int)(TempsPicking.TotalSeconds / NbArticle));
                    Result7JArtTraiteBL.Add(String.Format("Semaine {0}", NmrWeek), NbArticle);
                    Result7JBlTraiteBL.Add(String.Format("Semaine {0}", NmrWeek), NbPicking);
                }
                CeMatin = CeMatin.AddDays(7);
            }
        }

        private int NmrSemaine (DateTime time)
        {
            // Gets the Calendar instance associated with a CultureInfo.
            CultureInfo myCI = new CultureInfo("fr-FR");
            Calendar myCal = myCI.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;

            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            int NmrWeek = myCal.GetWeekOfYear(time, myCWR, myFirstDOW);
            return NmrWeek;
        }
        private void CalculMetrique12Mois(List<TRACABILITE_BL> ListBLinput, DateTime Maintenant,
                   out Dictionary<string, int> Resut7JPickingByBL,
                   out Dictionary<string, int> Resut7JPickingByArt,
                   out Dictionary<string, int> Resut7JPackingByBL,
                   out Dictionary<string, int> Resut7JPackingByArt,
                   out Dictionary<string, int> Resut7JByBL,
                   out Dictionary<string, int> Resut7JByArt,
                   out Dictionary<string, int> Result7JArtByBL,
                   out Dictionary<string, int> Result7JArtTraiteBL,
                   out Dictionary<string, int> Result7JBlTraiteBL)
        {
            int NbBl = 0;
            int NbPicking = 0;
            int NbArticle = 0;
            int Nbpacking = 0;
            Resut7JPickingByBL = new Dictionary<string, int>();
            Resut7JPickingByArt = new Dictionary<string, int>();
            Resut7JPackingByBL = new Dictionary<string, int>();
            Resut7JPackingByArt = new Dictionary<string, int>();
            Resut7JByBL = new Dictionary<string, int>();
            Resut7JByArt = new Dictionary<string, int>();
            Result7JArtByBL = new Dictionary<string, int>();
            Result7JArtTraiteBL = new Dictionary<string, int>();
            Result7JBlTraiteBL = new Dictionary<string, int>();

            TimeSpan TempsBls = new TimeSpan(0);
            TimeSpan TempsPicking = new TimeSpan(0);
            TimeSpan TempsPacking = new TimeSpan(0);

            DateTime CeMatin = Maintenant;
            while (CeMatin.Day != 1)
            {
                CeMatin = CeMatin.AddDays(-1);
            }
            CeMatin = CeMatin.AddMonths(-11);
            for (int j = 0; j < 12; j++)
            {
                DateTime CeMatinPlus1 = CeMatin.AddMonths(1);
                List<TRACABILITE_BL> ListBL = ListBLinput.Where(p => (p.FirstTimeStart > CeMatin) && (p.FirstTimeStart < CeMatinPlus1)).OrderBy(y => y.FirstTimeStart).ToList();
                if (ListBL.Count > 0)
                {
                    DateTime firststart = (DateTime)ListBL.First().FirstTimeStart;
                    DateTime MaxTimestart = (DateTime)ListBL.First().TimeStart;
                    TempsBls = new TimeSpan(0);
                    TempsPicking = new TimeSpan(0);
                    TempsPacking = new TimeSpan(0);
                    NbPicking = 0;
                    NbArticle = 0;
                    bool lastbltraite = false;
                    TRACABILITE_BL lastBl = new TRACABILITE_BL();
                    foreach (var bl in ListBL)
                    {
                        NbPicking++;
                        TempsPacking += (DateTime)bl.TimeEnd - (DateTime)bl.TimeStart;

                        if (bl.NbArtByBl != null)
                        { NbArticle += (int)bl.NbArtByBl; }
                        lastbltraite = false;
                        if (firststart.Equals(bl.FirstTimeStart))
                        {
                            if (bl.TimeStart < MaxTimestart)
                            { MaxTimestart = (DateTime)bl.TimeStart; }
                            lastbltraite = true;
                        }
                        else
                        {
                            TempsPicking += MaxTimestart - firststart;
                            firststart = (DateTime)bl.FirstTimeStart;
                            MaxTimestart = (DateTime)bl.TimeStart;
                            lastBl = bl;
                        }
                    }
                    if (!lastbltraite)
                    {
                        TempsPicking += (DateTime)lastBl.TimeStart - firststart;
                    }

                    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("fr-FR");
                    Resut7JPickingByBL.Add(String.Format(culture.DateTimeFormat.GetMonthName(CeMatin.Month)), (int)(TempsPicking.TotalSeconds / NbPicking));
                    Resut7JPickingByArt.Add(String.Format(culture.DateTimeFormat.GetMonthName(CeMatin.Month)), (int)(TempsPicking.TotalSeconds / NbArticle));
                    Resut7JPackingByBL.Add(String.Format(culture.DateTimeFormat.GetMonthName(CeMatin.Month)), (int)(TempsPacking.TotalSeconds / NbPicking));
                    Resut7JPackingByArt.Add(String.Format(culture.DateTimeFormat.GetMonthName(CeMatin.Month)), (int)(TempsPacking.TotalSeconds / NbArticle));
                    Resut7JByBL.Add(String.Format(culture.DateTimeFormat.GetMonthName(CeMatin.Month)), (int)(TempsPacking.TotalSeconds / NbPicking) + (int)(TempsPicking.TotalSeconds / NbPicking));
                    Resut7JByArt.Add(String.Format(culture.DateTimeFormat.GetMonthName(CeMatin.Month)), (int)(TempsPacking.TotalSeconds / NbArticle) + (int)(TempsPicking.TotalSeconds / NbArticle));
                    Result7JArtTraiteBL.Add(String.Format(culture.DateTimeFormat.GetMonthName(CeMatin.Month)), NbArticle);
                    Result7JBlTraiteBL.Add(String.Format(culture.DateTimeFormat.GetMonthName(CeMatin.Month)), NbPicking);
                }
                CeMatin = CeMatin.AddMonths(1);
            }
        }
    }
    
}