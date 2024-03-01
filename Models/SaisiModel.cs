using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace GenerateurDFUSafir.Models
{
    public class SaisiModel
    {
        public int isnoel
        {
            get
            {
                if (DateTime.Now.Month == 12)
                {
                    return DateTime.Now.Day % 3 + 1;
                }
                return 0;
            }
        }
        public long ID { get; set; }
        public string NomPrenom { get; set; }
        public double HeureByWeek { get; set; }
        private string Service {get;set;}
        public List<TEMPS_SEMAINE> SemaineResume { get; set; }
        [Required]
        public int Annee { get; set; }
        public int SemaineEnCours { get; set; }
        public int firstDayYear { get; set; }
        public Dictionary<string, List<TypeJour>> jourComplet = new Dictionary<string, List<TypeJour>>();
        private IEnumerable<TEMPS_SAISI> semaineARemplir;
        public List<SelectListItem> ProjetSuppl { get; set; }
        public List<TEMPS_SAISI> SemaineARemplir
        {
            get;set;
        }
        public SaisiModel()
        {

        }
        public SaisiModel(string service)
        {
            Service = service;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"> id de l'ustilusateur</param>
        public void Init(int id)
        {
            Init(id, -1, DateTime.Now.Year);
        }
        public int ADDNewSSprojet { get; set; }
        public void Init(int id, int semainedemande , int anneedemande)
        {
            DateTime findateprojet = EndDateOfWeekISO8601(anneedemande, semainedemande);
            RetDInformation info = new RetDInformation(Service);
            // recherche de l'operateur 
            OPERATEURS result;
            if (info.RefreshSaisiTemps(id).Count > 0)
            {
                result = info.RefreshSaisiTemps(id).First().OPERATEURS;
            }
            else
            {
                result = info.DataOperateur(id);
            }

            ID = result.ID;
            if (result.HoraireByWeek != null)
            {
                HeureByWeek = (double)result.HoraireByWeek;
            }
            NomPrenom = result.NOM + " " + result.PRENOM;
            if (result.TEMPS_SAISI.Where(a => (a.Annee).Equals((short)anneedemande))!=null)
            {

            }
            var querytempsedelutilisateur = result.TEMPS_SAISI.Where(a => (a.Annee).Equals((short)anneedemande) ).OrderBy(s => s.Semaine);
            var querysemainedelutilisateur = result.TEMPS_SEMAINE.Where(a => (a.Annee).Equals((short)anneedemande)).OrderBy(s => s.Semaine);
            // si demande de semaine specifique  ou alors semaine courante
            Annee = anneedemande;

            if (semainedemande >= 0)
            {
                SemaineEnCours = semainedemande;
            }
            // sinon on cherche la derniere semaine non rempli par l'utilisateur
            else if (querysemainedelutilisateur != null  && querysemainedelutilisateur.Where(p => p.Complete != true).Count() >0)
            {
                SemaineEnCours = (int)querysemainedelutilisateur.Where(p => p.Complete != true && (p.Annee).Equals((short)anneedemande)).OrderBy(s => s.Semaine).First().Semaine;
            }
            else if ((querysemainedelutilisateur != null && querysemainedelutilisateur.Where(p => p.Complete == true).Count() > 0)  &&
                     (querysemainedelutilisateur.Where(p => p.Complete == true && (p.Annee).Equals((short)anneedemande)).OrderBy(s => s.Semaine).Last().Semaine < CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(new DateTime(anneedemande, 12, 31), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)))
            {
                SemaineEnCours = (short)querysemainedelutilisateur.Where(p => p.Complete == true).OrderBy(s => s.Semaine).Last().Semaine + 1;
            }
            else 
            {
                SemaineEnCours = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(new DateTime(anneedemande,01,02), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                if (SemaineEnCours == 53) { SemaineEnCours = 0; }
            }
            semaineARemplir = new List<TEMPS_SAISI>();
            if (querytempsedelutilisateur == null || querytempsedelutilisateur.Where(p => p.Annee.Equals((short)anneedemande)).Count() == 0)
            {
                SemaineEnCours = SemaineEnCours; 
                
            }
            else
            {
                semaineARemplir = semaineARemplir.Concat(result.TEMPS_SAISI.Where(a => (a.Annee).Equals((short)anneedemande) && a.Semaine.Equals((short)SemaineEnCours)));
            }
            // complter la semaine a remplir  avec les projet toujours visible
            List<SOUSPROJET> projetobligatoire = info.ListSousProjetObligatoire();
            foreach(var ssprojet in  projetobligatoire)
            {
                var query = semaineARemplir.Where(i => i.SOUSPROJET.ID == ssprojet.ID);
                if (semaineARemplir.Where(i => i.SOUSPROJET_ID == ssprojet.ID).Count()==0)
                {
                    TEMPS_SAISI addtemps = new TEMPS_SAISI();
                    addtemps.OPERATEURS = result;
                    addtemps.Annee = (short)anneedemande;
                    addtemps.Semaine = (short)SemaineEnCours;
                    addtemps.Days1 = 0;
                    addtemps.Days2 = 0;
                    addtemps.Days3 = 0;
                    addtemps.Days4 = 0;
                    addtemps.Days5 = 0;
                    addtemps.Days6 = 0;
                    addtemps.Days7 = 0;
                    addtemps.SOUSPROJET = ssprojet;
                    addtemps.SOUSPROJET_ID = ssprojet.ID;
                    addtemps.Complete = false;
                    semaineARemplir = semaineARemplir.Concat(new[] { addtemps });
                }
            }
            // completer la liste avec les projet present sur les 4 derniere semaine
            // sauf si il sont cloture
            List<SOUSPROJET> projetdejaAjoute = result.TEMPS_SAISI.Where(p=>p.OPERATEURS_ID == (short)id && p.Semaine+4>= semainedemande &&  p.Semaine < semainedemande && (p.Days1+ p.Days2 + p.Days3 + p.Days4 + p.Days5 + p.Days6 + p.Days7)>0).Select(a =>a.SOUSPROJET).ToList();
            //List<SOUSPROJET> projetdejaAjoute = projetdejaAjoutenonfiltre.Where(p => p.DateFinProjet > findateprojet).ToList();
            // List<SOUSPROJET> projetdejaAjoute = result.TEMPS_SAISI.Where(p => p.OPERATEURS_ID == (short)id).Select(a => a.SOUSPROJET).ToList();
            foreach (var ssprojet in projetdejaAjoute)
            {
                var query = semaineARemplir.Where(i => i.SOUSPROJET.ID == ssprojet.ID);
                if (ssprojet.DateFinProjet> DateTime.Now &&  semaineARemplir.Where(i => i.SOUSPROJET_ID == ssprojet.ID).Count() == 0)
                {
                    TEMPS_SAISI addtemps = new TEMPS_SAISI();
                    addtemps.OPERATEURS = result;
                    addtemps.Annee = (short)anneedemande;
                    addtemps.Semaine = (short)SemaineEnCours;
                    addtemps.Days1 = 0;
                    addtemps.Days2 = 0;
                    addtemps.Days3 = 0;
                    addtemps.Days4 = 0;
                    addtemps.Days5 = 0;
                    addtemps.Days6 = 0;
                    addtemps.Days7 = 0;
                    addtemps.SOUSPROJET = ssprojet;
                    addtemps.SOUSPROJET_ID = ssprojet.ID;
                    addtemps.Complete = false;
                    semaineARemplir = semaineARemplir.Concat(new[] { addtemps });
                }
            }
            // organisation des projets en fonction de l'id
            SemaineARemplir = semaineARemplir.OrderBy(p =>p.SOUSPROJET_ID).ToList();

            // liste des projets rajoutable ProjetSuppl
            // on recherche les projet qui sont rajoutable mais pas rajoute
            List<long> projetdejapresent = new List<long>();
            foreach(var sem in SemaineARemplir)
            {
                projetdejapresent.Add(sem.SOUSPROJET.IDSOUSPROJET / 100);
            }

            var querySSprojet = info.ListSousProjetNonObligatoire().Where(p => p.IDSOUSPROJET%100 == 0  && p.Service.Contains(Service));
            ProjetSuppl = new List<SelectListItem>(); 
            // ajout de la ligne pour information utilisateur.
            ProjetSuppl.Add(new SelectListItem { Text = "Ajouter une ligne projet ", Value = "00" });
            List<SelectListItem> ProjetSuppltmp  = new List<SelectListItem>();
            foreach (var prj in querySSprojet)
            {
                if ((prj.IDSOUSPROJET%100) == 0)
                {
                    long idprojet = prj.IDSOUSPROJET / 100;
                    if (!projetdejapresent.Contains(idprojet) && (prj.DateFinProjet == null || prj.DateFinProjet > DateTime.Now))
                    {
                        ProjetSuppltmp.Add(new SelectListItem { Text = prj.NomSousProjet+prj.TitreSousProjet, Value = idprojet.ToString() });
                    }
                }
            }
            // ajout des projet suivant l'ordre alphabétique
            ProjetSuppl.AddRange(ProjetSuppltmp.OrderBy(p => p.Text));
            int nbSemaineDansLAnne = GetWeeksInYear(anneedemande);
            SemaineResume = new List<TEMPS_SEMAINE>();
            for (int i = 0; i < nbSemaineDansLAnne+1; i++)
            {
                TEMPS_SEMAINE semaine = new TEMPS_SEMAINE();
                var query = querysemainedelutilisateur.Where(s => s.Semaine== (short)i && s.Annee == (short)anneedemande);
                if (query != null && query.Count()>0)
                {
                    semaine = querysemainedelutilisateur.Where(s => s.Semaine == (short)i && s.Annee == (short)anneedemande).First();
                }
                else
                {
                    semaine.Annee = (short)anneedemande;
                    semaine.Semaine = (short)(i);
                    semaine.Complete = false;
                }
                SemaineResume.Add(semaine);
            }
            firstDayYear = FirstdayOfyear(anneedemande);
            jourComplet = CalendierView(anneedemande, SemaineResume);
        }
        private int GetWeeksInYear(int year)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            DateTime date1 = new DateTime(year, 12, 31);
            Calendar cal = dfi.Calendar;
            CalendarWeekRule CalendarWeekRule = CalendarWeekRule.FirstFourDayWeek;

            return cal.GetWeekOfYear(date1, CalendarWeekRule,DayOfWeek.Monday);
        }
        private int FirstdayOfyear(int annnee)
        {
            DayOfWeek janvierfirst = new DateTime(annnee, 1, 1).DayOfWeek;
            return (int)janvierfirst;
        }
        private Dictionary<string, List<TypeJour>> CalendierView(int Annee, List<TEMPS_SEMAINE> SemaineResume)
        {
            //1er semaine est la 1er semaine conteannt un jour
            Dictionary<string, List<TypeJour>> result = new Dictionary<string, List<TypeJour>>();
            int nmrsemaine = 0;
            int jourdeannee = 0;
            if (FirstdayOfyear(Annee) == 0) { nmrsemaine = 0; }

            for (int Month = 1; Month < 13; Month++)
            {
                List<TypeJour> mois = new List<TypeJour>();
                int nbjourmonth = DateTime.DaysInMonth(Annee, Month);
                int cellspan = 0;
                for (int jour = 1; jour < nbjourmonth + 1; jour++)
                {
                    jourdeannee++;
                    TypeJour newjour = new TypeJour();
                    if (new DateTime(Annee, Month, jour).DayOfWeek == DayOfWeek.Monday)
                    {
                        nmrsemaine++;
                        cellspan = 1;
                    }
                    else if (new DateTime(Annee, Month, jour).DayOfWeek == DayOfWeek.Saturday)
                    {
                        cellspan = 1;
                    }
                    else
                    {
                        cellspan++;
                    }
                    newjour.nmrsemaine = nmrsemaine;
                    TEMPS_SEMAINE svalid = SemaineResume.Where(s => s.Annee == Annee && s.Semaine == (short)nmrsemaine).First();
                    newjour.complet = (bool)svalid.Complete;
                    if (new DateTime(Annee, Month, jour).DayOfWeek == DayOfWeek.Saturday || new DateTime(Annee, Month, jour).DayOfWeek == DayOfWeek.Sunday)
                    {
                        newjour.color = TypeJour.ColorWeek.Noir;
                    }
                    else
                    {
                        newjour.color = (TypeJour.ColorWeek)(nmrsemaine % 3);
                    }
                    newjour.nmrjour = jourdeannee;
                    if (nbjourmonth == jour|| new DateTime(Annee, Month, jour).DayOfWeek == DayOfWeek.Friday || new DateTime(Annee, Month, jour).DayOfWeek == DayOfWeek.Sunday)
                    {
                        newjour.cellspan = cellspan;
                        mois.Add(newjour);
                    }
                }
                CultureInfo culture = new CultureInfo("fr-Fr");
                result.Add(culture.DateTimeFormat.GetMonthName(Month), mois);
            }
            return result;
        }
        public int AddNewWeek(SaisiModel newweek)
        {
            TEMPS_SAISI addtemps = new TEMPS_SAISI();
            RetDInformation info = new RetDInformation(Service);
            List<TEMPS_SAISI> Temps_Saisi_byid = info.RefreshSaisiTemps((int)newweek.ID);
            OPERATEURS op = info.DataOperateur((int)newweek.ID);
            var querySSprojet = info.ListSousProjetNonObligatoire().Where(p => p.IDSOUSPROJET / 100 == newweek.ADDNewSSprojet);
            // ajout si demande de nouveau projet
            if (querySSprojet != null && querySSprojet.Count() > 0)
            {
                foreach (var ssprj in querySSprojet)
                {
                    if (Temps_Saisi_byid.Where(p => p.SOUSPROJET_ID == ssprj.ID && p.Annee == (short)newweek.Annee && p.Semaine == (short)newweek.SemaineEnCours).Count() == 0)
                    {
                        addtemps.OPERATEURS = op;
                        addtemps.OPERATEURS_ID = newweek.ID;
                        addtemps.Annee = (short)newweek.Annee;
                        addtemps.Semaine = (short)newweek.SemaineEnCours;
                        addtemps.Days1 = 0;
                        addtemps.Days2 = 0;
                        addtemps.Days3 = 0;
                        addtemps.Days4 = 0;
                        addtemps.Days5 = 0;
                        addtemps.Days6 = 0;
                        addtemps.Days7 = 0;
                        addtemps.SOUSPROJET = ssprj;
                        addtemps.SOUSPROJET_ID = ssprj.ID;
                        addtemps.Complete = false;
                        info.AddNewWeekID(newweek.ID, addtemps, ssprj.ID);
                    }
                }
            }
            
            // on verifie et on met a jour les autre semaine.
            foreach (var sem in newweek.SemaineARemplir)
            {
                info.CreateOrUpdateSemaine(op, sem, sem.SOUSPROJET_ID);
            }
            return 0;
        }
        
        public int CheckTimeByWeek(short id, int semaine, int annee)
        {
            RetDInformation info = new RetDInformation(Service);
            info.CkeckTime(id, semaine,annee);
            return 0;
        }
        public static DateTime EndDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var result = firstThursday.AddDays(weekNum * 7);

            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            // add 3 days to find Sunday
            return result.AddDays(3);
        }
    }
    public class TypeJour
    {
        public enum ColorWeek : ushort
        {
            blue = 0,
            blanc = 1,
            jaune=2,
            Noir=3  // pour samedi et dimanche
        }
        public int nmrjour = 0;
        public bool complet = false;
        public int nmrsemaine = 0;
        public ColorWeek color = 0;
        public string GetColorWeek
        {
            get
            {
                if (complet&& color != ColorWeek.Noir)
                {
                    return "#008000";
                }
                else if (color == ColorWeek.blue)
                {
                    return "#F0F8FF";
                }
                else if (color == ColorWeek.blanc)
                {
                    return "#FFFFFF";
                }
                else if (color == ColorWeek.jaune)
                {
                    return "#FFFFE0";
                }
                else
                {
                    return "#808080";
                }
            }
        }
        public string GetColorWeekT
        {
            get
            {
                if (complet && color != ColorWeek.Noir)
                {
                    return "#FFFFFF";
                }
                else 
                {
                    return "#000000";
                }
            }
        }
        public string getHidden
        {
            get
            {
                if (color.Equals(ColorWeek.Noir))
                {
                    return "Hidden = \"Hidden\"";
                }
                else
                {
                    return "";
                }
            }
        }
        public string getComplet
        {
            get
            {
                if (complet == true)
                {
                    return "S"+nmrsemaine.ToString("00")+ " Ok";
                }
                else
                {
                    return "S" + nmrsemaine.ToString("00");
                }
            }
        }
        public int cellspan
        {
            get;
            set;
        }
    }
}