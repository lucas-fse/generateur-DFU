using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class StatSTCTCS
    {
        public List<DataSTC> DatatSTC { get; set; }
        public DataGlobal Data { get; set; }

        public StatSTCTCS()
        {
            StatSTCbyTCS();
            StatSTCTCSGlobal();
        }
        private void  StatSTCTCSGlobal()
        {
                // pour toute les fiches verifeier les l'etat des fiche avant les commandes
               
            PEGASE_CHECKFPSEntities1 _db1 = new PEGASE_CHECKFPSEntities1();
            PEGASE_PROD2Entities2 _db2 = new PEGASE_PROD2Entities2();
            x160Entities _db3 = new x160Entities();
            Data = new DataGlobal();
            var query = _db1.TRACACMD.Where(f => (f.StatusFPS != 4 && f.StatusFPS != 5) || ((f.StatusFPS == 4 || f.StatusFPS == 5) && f.DateEffectiveClient > DateTime.Now));

                
               
            DateTime mois = DateTime.Now.AddMonths(1);
            DateTime lemois = new DateTime(mois.Year, mois.Month, 1);
            DateTime moisprecedent = DateTime.Now.AddMonths(0);
            DateTime lemoisprecedent = new DateTime(moisprecedent.Year, moisprecedent.Month, 1);

            var queryfiche = query.Where(f =>  f.DateEffectiveClient > lemoisprecedent);
            List<TRACACMD> test = new List<TRACACMD>();


            Data.NbFichesDispo = queryfiche.Where(t => t.DateEffectiveClient > DateTime.Now && t.StatusFPS == 4).ToList().Distinct(new DistinctItemComparer()).Count();
            Data.NbFichesEnRetard = queryfiche.Where(t => t.DateEffectiveClient < DateTime.Now && t.StatusFPS != 4).ToList().Distinct(new DistinctItemComparer()).Count();
            Data.NbFichesAFaire = queryfiche.Where(t => t.DateEffectiveClient > DateTime.Now && t.StatusFPS != 4).ToList().Distinct(new DistinctItemComparer()).Count();
            List<string> cmdmontant = new List<string>();
            List<string> cmdmontantsoldé = new List<string>();
            List<string> cmdmontantMoisEncours = new List<string>();
            List<string> cmdmontantsoldéMoisEncours = new List<string>();

            decimal[] montantTotalSem = new decimal[53];
            decimal[] montantSoldéSem = new decimal[53];
            decimal[] MontantEncoursSem = new decimal[53];
            decimal[] MontantEnRetardSem = new decimal[53];
            string[] stringTotalSem = new string[53];
            string[] stringSoldéSem = new string[53];
            string[] stringEncoursSem = new string[53];
            string[] stringEnRetardSem = new string[53];

            bool fichemois = false;
            foreach (var f in queryfiche)
            {
                if (f.DateEffectiveClient < lemois && f.DateEffectiveClient >= lemoisprecedent) { fichemois = true; } else { fichemois = false; }

                if (!cmdmontant.Contains(f.SOHNUM))
                {
                    cmdmontant.Add(f.SOHNUM);
                    int num_semaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear((DateTime)f.DateEffectiveClient, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
                    SORDER sORDER = _db3.SORDER.Where(s => s.SOHNUM_0.Contains(f.SOHNUM.Trim())).FirstOrDefault();
                    if (sORDER != null)
                    {
                        Data.MontantTotal += sORDER.ORDINVATI_0;
                        montantTotalSem[num_semaine-1] += sORDER.ORDINVATI_0;
                        stringTotalSem[num_semaine - 1] += f.SOHNUM.Trim() + Environment.NewLine;
                    }
                }
                if ((f.StatusFPS == 5 || f.StatusFPS == 4) && !cmdmontantsoldé.Contains(f.SOHNUM))
                {
                    cmdmontantsoldé.Add(f.SOHNUM);
                    SORDER sORDER = _db3.SORDER.Where(s => s.SOHNUM_0.Contains(f.SOHNUM.Trim())).FirstOrDefault();
                    int num_semaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear((DateTime)f.DateEffectiveClient, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
                    if (sORDER != null)
                    {
                        Data.MontantTotalSoldé += sORDER.ORDINVATI_0;
                        montantSoldéSem[num_semaine - 1] += sORDER.ORDINVATI_0;
                        stringSoldéSem[num_semaine - 1] += f.SOHNUM.Trim() + Environment.NewLine;
                    }
                }
                if ((f.StatusFPS != 4 && f.StatusFPS != 5) && !cmdmontantMoisEncours.Contains(f.SOHNUM) && f.DateEffectiveClient> DateTime.Now)
                {
                    cmdmontantMoisEncours.Add(f.SOHNUM);
                    SORDER sORDER = _db3.SORDER.Where(s => s.SOHNUM_0.Contains(f.SOHNUM.Trim())).FirstOrDefault();
                    int num_semaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear((DateTime)f.DateEffectiveClient, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
                    if (sORDER != null)
                    {
                        Data.MontantTotalMoisEncours += sORDER.ORDINVATI_0;
                        MontantEncoursSem[num_semaine - 1] += sORDER.ORDINVATI_0;
                        stringEncoursSem[num_semaine - 1] += f.SOHNUM.Trim() + Environment.NewLine;
                    }
                }
                if ((f.StatusFPS != 4 && f.StatusFPS != 5) && !cmdmontantsoldéMoisEncours.Contains(f.SOHNUM) && f.DateEffectiveClient < DateTime.Now)
                {
                    cmdmontantsoldéMoisEncours.Add(f.SOHNUM);
                    int num_semaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear((DateTime)f.DateEffectiveClient, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
                    SORDER sORDER = _db3.SORDER.Where(s => s.SOHNUM_0.Contains(f.SOHNUM.Trim())).FirstOrDefault();
                    if (sORDER != null)
                    {
                        Data.MontantTotalSoldéMoisEncours += sORDER.ORDINVATI_0;
                        MontantEnRetardSem[num_semaine - 1] += sORDER.ORDINVATI_0;
                        stringEnRetardSem[num_semaine - 1] += f.SOHNUM.Trim() + Environment.NewLine;
                    }
                }
            }
            Data.SemaineCourante = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            Data.MontantTotalSem = new decimal[53];
            Data.MontantTotalSoldéSem = new decimal[53];
            Data.MontantTotalEncoursSem = new decimal[53];
            Data.MontantTotalRetardSem = new decimal[53];
            Data.CMDTotalSem = new string[53];
            Data.CMDTotalSoldéSem = new string[53];
            Data.CMDTotalEncoursSem = new string[53];
            Data.CMDTotalRetardSem = new string[53];
            for (int s =0;s<9;s++)
            {
                int index = Data.SemaineCourante + s-1;
                if (index > 52) { index = index - 53; }
                Data.MontantTotalSem[s] = montantTotalSem[index];
                Data.MontantTotalSoldéSem[s] = montantSoldéSem[index];
                Data.MontantTotalEncoursSem[s] = MontantEncoursSem[index];
                Data.MontantTotalRetardSem[s] = MontantEnRetardSem[index];
                Data.CMDTotalSem[s] = stringTotalSem[index];
                Data.CMDTotalSoldéSem[s] = stringSoldéSem[index];
                Data.CMDTotalEncoursSem[s] = stringEncoursSem[index];
                Data.CMDTotalRetardSem[s] = stringEnRetardSem[index];
            }
            
        }
        private void  StatSTCbyTCS()
        {

            // pour toute les fiches verifeier les l'etat des fiche avant les commandes
            DatatSTC = new List<DataSTC>();
            PEGASE_CHECKFPSEntities1 _db1 = new PEGASE_CHECKFPSEntities1();
            PEGASE_PROD2Entities2 _db2 = new PEGASE_PROD2Entities2();
            x160Entities _db3 = new x160Entities();

            var query = _db1.TRACACMD.Where(f => (f.StatusFPS != 4 && f.StatusFPS != 5) || ((f.StatusFPS == 4 || f.StatusFPS == 5) && f.DateEffectiveClient > DateTime.Now));

            List<OPERATEURS> opes = _db2.OPERATEURS.Where(o => o.SERVICE.Contains("STC")).ToList();
            foreach (var ope in opes)
            {
                DateTime mois = DateTime.Now.AddMonths(1);
                DateTime lemois = new DateTime(mois.Year, mois.Month, 1);
                DateTime moisprecedent = DateTime.Now.AddMonths(0);
                DateTime lemoisprecedent = new DateTime(moisprecedent.Year, moisprecedent.Month, 1);

                DataSTC dataope = new DataSTC();
                dataope.MontantTotal = 0;
                dataope.STCName = ope.INITIAL.Trim();
                var queryfiche = query.Where(f => f.STcEnCharge.Contains(ope.INITIAL.Trim()) && f.DateEffectiveClient> lemoisprecedent);
                List<TRACACMD> test = new List<TRACACMD>();

                List<TRACACMD> test1 = queryfiche.Where(t => t.DateEffectiveClient > DateTime.Now && (t.StatusFPS != 4 && t.StatusFPS != 5)).ToList();
                int toto = test1.Distinct(new DistinctItemComparer()).Count();

                dataope.NbFichesDispo = queryfiche.Where(t =>  t.DateEffectiveClient > DateTime.Now && (t.StatusFPS == 4 ||  t.StatusFPS == 5)).ToList().Distinct(new DistinctItemComparer()).Count();
                dataope.NbFichesEnRetard = queryfiche.Where(t => t.DateEffectiveClient < DateTime.Now && (t.StatusFPS != 4&&  t.StatusFPS !=5)).ToList().Distinct(new DistinctItemComparer()).Count();

                var query1  = queryfiche.Where(t => t.DateEffectiveClient < DateTime.Now && (t.StatusFPS != 4 && t.StatusFPS != 5)).ToList().Distinct(new DistinctItemComparer());

                dataope.NbFichesAFaire = queryfiche.Where(t => t.DateEffectiveClient > DateTime.Now && (t.StatusFPS != 4 &&  t.StatusFPS != 5)).ToList().Distinct(new DistinctItemComparer()).Count();
                
                List<string> cmdmontant = new List<string>();
                List<string> cmdmontantsoldé = new List<string>();
                List<string> cmdmontantMoisEncours = new List<string>();
                List<string> cmdmontantsoldéMoisEncours = new List<string>();
               
                bool fichemois = false;
                foreach (var f in queryfiche)
                {
                    if (f.DateEffectiveClient < lemois && f.DateEffectiveClient>= lemoisprecedent){ fichemois = true;} else {fichemois = false;}
                                       
                    if (!cmdmontant.Contains(f.SOHNUM))
                    {
                        cmdmontant.Add(f.SOHNUM);
                    }
                    if (f.StatusFPS == 4 && !cmdmontantsoldé.Contains(f.SOHNUM))
                    {
                        cmdmontantsoldé.Add(f.SOHNUM);                            
                    }
                    if (!cmdmontantMoisEncours.Contains(f.SOHNUM) && fichemois)
                    {
                        cmdmontantMoisEncours.Add(f.SOHNUM);
                    }
                    if (f.StatusFPS == 4 && !cmdmontantsoldéMoisEncours.Contains(f.SOHNUM) && fichemois)
                    {
                        cmdmontantsoldéMoisEncours.Add(f.SOHNUM);
                    }
                }
                foreach (var c in cmdmontant)
                {
                    SORDER sORDER = _db3.SORDER.Where(s => s.SOHNUM_0.Contains(c.Trim())).FirstOrDefault();
                    if (sORDER != null)
                    {
                        dataope.MontantTotal += sORDER.ORDINVATI_0;
                    }
                }
                foreach (var c in cmdmontantsoldé)
                {
                    SORDER sORDER = _db3.SORDER.Where(s => s.SOHNUM_0.Contains(c.Trim())).FirstOrDefault();
                    if (sORDER != null)
                    {
                        dataope.MontantTotalSoldé += sORDER.ORDINVATI_0;
                    }
                }
                foreach (var c in cmdmontantMoisEncours)
                {
                    SORDER sORDER = _db3.SORDER.Where(s => s.SOHNUM_0.Contains(c.Trim())).FirstOrDefault();
                    if (sORDER != null)
                    {
                        dataope.MontantTotalMoisEncours += sORDER.ORDINVATI_0;
                    }
                }
                foreach (var c in cmdmontantsoldéMoisEncours)
                {
                    SORDER sORDER = _db3.SORDER.Where(s => s.SOHNUM_0.Contains(c.Trim())).FirstOrDefault();
                    if (sORDER != null)
                    {
                        dataope.MontantTotalSoldéMoisEncours += sORDER.ORDINVATI_0;
                    }
                }
                DatatSTC.Add(dataope);
                //ordinvati

                List<Planning> tmp = new List<Planning>();

                for (int i =0; i < 28; i++) { tmp.Add(new Planning { DateTime=DateTime.Now.AddDays(i),Charge = 0 }); }
                var queryorderbydate = queryfiche.Where(s => s.StatusFPS!=4).OrderBy(p => p.DateEffectiveClient);
                foreach(var f in queryorderbydate)
                {
                    int delai = 5*60;
                    if (f.DELAIFPS.indexDELAI != 0)
                    {
                        delai =(int) f.DELAIFPS.Temps;
                    }
                    DateTime livrele = new DateTime();
                    if (f.DateEffectiveClient != null) { livrele = (DateTime)f.DateEffectiveClient; }
                    else { livrele = (DateTime)f.DateEffectiveClient; }
                    
                    int restedelai = delai/60;
                    for (int j =0; j< tmp.Count && restedelai != 0; j++)
                    {
                        if (tmp.ElementAt(j).DateTime.DayOfWeek != DayOfWeek.Saturday && tmp.ElementAt(j).DateTime.DayOfWeek != DayOfWeek.Sunday)
                        {
                            if ((7 - tmp.ElementAt(j).Charge) >= restedelai)
                            {
                                tmp.ElementAt(j).Charge += restedelai;
                                restedelai = 0;
                            }
                            else
                            {
                                if (livrele > tmp.ElementAt(j).DateTime)
                                {
                                    if ((7 - tmp.ElementAt(j).Charge) >= 0)
                                    {
                                        restedelai -= (short)(7 - tmp.ElementAt(j).Charge);
                                        tmp.ElementAt(j).Charge = 7;
                                    }
                                }
                                else
                                {
                                    tmp.ElementAt(j).Charge += restedelai;
                                    restedelai = 0;
                                }
                            }
                        }
                    }
                }
                dataope.ChargeJournaliere = tmp;
            }
        }

    }

    public class DataSTC
    {
        public String STCName { get; set; }
        public int NbFichesEnRetard { get; set; }
        public int NbFichesDispo { get; set; }
        public int NbFichesAFaire { get; set; }
        public decimal MontantTotal { get; set; }
        public decimal MontantTotalSoldé { get; set; }
        public decimal MontantTotalMoisEncours { get; set; }
        public decimal MontantTotalSoldéMoisEncours { get; set; }  
        public List<Planning> ChargeJournaliere { get; set; }
    }
    public class Planning
    {
        public DateTime  DateTime { get; set; }
        public int Charge { get; set; }
    }
    public class DataGlobal
    {       
        public int SemaineCourante { get; set; }
        public int NbFichesEnRetard { get; set; }
        public int NbFichesDispo { get; set; }
        public int NbFichesAFaire { get; set; }
        public decimal MontantTotal { get; set; }
        public decimal MontantTotalSoldé { get; set; }
        public decimal MontantTotalMoisEncours { get; set; }
        public decimal MontantTotalSoldéMoisEncours { get; set; }
        public decimal[] MontantTotalSem { get; set; }
        public string[] CMDTotalSem { get; set; }
        public decimal[] MontantTotalSoldéSem { get; set; }
        public string[] CMDTotalSoldéSem { get; set; }
        public decimal[] MontantTotalEncoursSem { get; set; }
        public string[] CMDTotalEncoursSem { get; set; }
        public decimal[] MontantTotalRetardSem { get; set; }
        public string[] CMDTotalRetardSem { get; set; }
    }

    class DistinctItemComparer : IEqualityComparer<TRACACMD>
    {

        public bool Equals(TRACACMD x, TRACACMD y)
        {
            if (String.IsNullOrWhiteSpace(x.ZFPS) || String.IsNullOrWhiteSpace(y.ZFPS))
            {
                return false;
            }
            else
            {
                return x.ZFPS == y.ZFPS;
            }
        }

        public int GetHashCode(TRACACMD obj)
        {
            if (string.IsNullOrWhiteSpace(obj.ZFPS.Trim()))
            {
                return obj.SOHNUM.GetHashCode() ^
                    obj.SOPLIN.GetHashCode();
            }
            else
            {
                return obj.ZFPS.GetHashCode();
            }
            
        }
    }
}