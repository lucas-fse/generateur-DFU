using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace GenerateurDFUSafir.Models
{
    public class GestionSTCOperateur

    {
        public List<DataOperateursSTC> ListDataOperateurs { get; set; }
        public List<PLANNING_OPERATEUR> ListPLANNING_OPERATEUR { get; set; }
        public DateTime Jour1 { get; set; }
        public DateTime Jour5 { get { return Jour1.AddDays(4);} }
       // public List<PLANNING_VALUE> planning_value { get; set; }
       //public List<PLANNING_OPERATEUR> planning_operateur { get; set; }

        DateTime date = DateTime.Now;

        public int nbJours { get; set; }

        public FluxOffreSemaineGraph fluxOffreSemaine { get; set; }
        public EvolutionOffreStcGraph evolutionOffreStc { get; set; }
        public CommandeEnCoursGraph commandeEnCours { get; set; }

        public FluxCommandeSemaineGraph fluxCommandeSemaine { get; set; }

        public EvolutionCommandeGraph evolutionCommandeGraph { get; set; }

        public FluxATraiterEntreeGraph fluxATraiterEntree { get; set; }

        public FluxEnCoursRelance fluxEnCoursRelance { get; set; }

        public GestionSTCOperateur()
        {
            fluxOffreSemaine = new FluxOffreSemaineGraph();
            evolutionOffreStc = new EvolutionOffreStcGraph();
            commandeEnCours = new CommandeEnCoursGraph();
            fluxCommandeSemaine = new FluxCommandeSemaineGraph();
            evolutionCommandeGraph = new EvolutionCommandeGraph();
            fluxATraiterEntree = new FluxATraiterEntreeGraph();
            fluxEnCoursRelance = new FluxEnCoursRelance();
            ListDataOperateurs = new List<DataOperateursSTC>();
        }

        public void  GestionSTCOperateur_update(int nbJours)
        {
            this.nbJours = nbJours;
            date = date.AddDays(nbJours*7);
            DateTime now = date;
            int delta = DayOfWeek.Monday - date.DayOfWeek;
            if (delta > 0) { delta -= 7; }
            date = date.AddDays(delta);
            Jour1 = date;
            int numsemaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            int annee = date.Year;

            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();
            List<OPERATEURS> optmp = db.OPERATEURS.Where(p => p.SERVICE.Contains("STC") && (p.FINCONTRAT  == null || p.FINCONTRAT> Jour1)).ToList();
            ListDataOperateurs = new List<DataOperateursSTC>();
            foreach (var op in optmp)
            {
                DataOperateursSTC opstc = new DataOperateursSTC();
                opstc.DataOperateursSTCinit(op.ID, annee, numsemaine);
                ListDataOperateurs.Add(opstc);
            }
            ListPLANNING_OPERATEUR = new List<PLANNING_OPERATEUR>();
            foreach(var op in ListDataOperateurs)
            {
                ListPLANNING_OPERATEUR.Add(op.planning_op);
            }
        }

        public bool SavePlanning()
        {
            bool result = false;

            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();
            foreach (var item in ListPLANNING_OPERATEUR)
            {
                var query = db.PLANNING_OPERATEUR.Where(x => (x.ID == item.ID) && item.Annee == x.Annee && item.Semaine == x.Semaine);

                PLANNING_OPERATEUR tmp_tosave = item;


                if (query != null && query.Count() == 1)
                {

                    PLANNING_OPERATEUR tmp = query.First();
                    tmp.LundiMatin = tmp_tosave.LundiMatin;
                    tmp.LundiApresmidi = tmp_tosave.LundiApresmidi;
                    tmp.MardiMatin = tmp_tosave.MardiMatin;
                    tmp.MardiApresmidi = tmp_tosave.MardiApresmidi;
                    tmp.MercrediMatin = tmp_tosave.MercrediMatin;
                    tmp.MercrediApresmidi = tmp_tosave.MercrediApresmidi;
                    tmp.JeudiMatin = tmp_tosave.JeudiMatin;
                    tmp.JeudiApresmidi = tmp_tosave.JeudiApresmidi;
                    tmp.VendrediMatin = tmp_tosave.VendrediMatin;
                    tmp.VendrediApresmidi = tmp_tosave.VendrediApresmidi;
                    tmp.SamediMatin = tmp_tosave.SamediMatin;
                    tmp.SamediApresmidi = tmp_tosave.SamediApresmidi;
                    tmp.DimancheMatin = tmp_tosave.DimancheMatin;
                    tmp.DimancheApresmidi = tmp_tosave.DimancheApresmidi;
                    try
                    {
                        db.SaveChanges();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        private List<SelectListItem> _planning_status = null;

        public IEnumerable<SelectListItem> Planning_status 
        {
            get
            {
                if (_planning_status == null)
                {
                    _planning_status = new List<SelectListItem>();
                    PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();
                    var query = db.PLANNING_VALUE;
                    //var query = db.PLANNING_OPERATEUR.Include("PLANNING_VALUE").Where(x => (x.Semaine == numsemaine) && x.Annee == annee);
                    if (query.Count() > 0)
                    {
                        foreach (var t in query.ToList())
                        {
                            _planning_status.Add(new SelectListItem { Text = t.Status, Value = ((int)t.ID).ToString() });
                        }
                    }
                }
                return _planning_status;
            }
        }
    
    
    }

    public class FluxOffreSemaineGraph
    {
        public List<String> ListStc { get; set; }
        public List<double> NbOffres { get; set; }
        public List<double> Ca { get; set; }

        public List<double> Atex { get; set; }

        public FluxOffreSemaineGraph()
        {
            ListStc = new List<String>();
            NbOffres = new List<double>();
            Ca = new List<double>();
            Atex = new List<double>();

            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();

            DateTime now = DateTime.Now;

            int numsemaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);


            var query = db.KPI_STC.Where(i => i.Semaine == numsemaine && i.Annee == now.Year);

            if (query != null || query.Count() > 0)
            {
                //throw new Exception("Aucun KPI cette semaine");


                List<KPI_STC> kpi_stc = query.ToList();

                foreach (var i in kpi_stc)
                {
                    ListStc.Add(i.OPERATEURS.INITIAL);
                    NbOffres.Add((double)i.Value13);
                    Ca.Add((double)i.Value15);
                    Atex.Add((double)i.Value14);
                }
            }
        }

    }

    public class EvolutionOffreStcGraph
    {
        public List<int> NumSemaines { get; set; }
        public Dictionary<String, List<double>> datas { get; set; }

        public EvolutionOffreStcGraph()
        {
            datas = new Dictionary<string, List<double>>();
            NumSemaines = new List<int>();

            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();

            DateTime now = DateTime.Now;

            int numsemaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            int annee = now.Year;

            // On récupère les 6 dernières semaines au fur et à mesure
            for (int i = 0; i < 6; i++)
            {
                var query = db.KPI_STC.Where(j => j.Semaine == numsemaine && j.Annee == annee);

                if (query != null && query.Count() > 0)
                {
                    List<KPI_STC> list_stc = query.ToList();

                    // Pour chaque semaines on récupère la liste des stc (techniciens)
                    foreach (var s in list_stc)
                    {
                        // Si le stc n'est encore pas ajouté, on le créé en initialisant la liste
                        if (!datas.ContainsKey(s.OPERATEURS.INITIAL))
                        {
                            datas.Add(s.OPERATEURS.INITIAL, new List<double>());
                        }

                        // On ajoute le nombre d'offre de la semaine
                        if (s.Value13 == null)
                        {
                            datas[s.OPERATEURS.INITIAL].Insert(0, 0);
                        }
                        else
                        {
                            datas[s.OPERATEURS.INITIAL].Insert(0, (double)s.Value13);
                        }

                    }

                }
                // TODO: Corriger le problème qui fait que toutes les données par STC sont les mêmes (tout le temps 3, tout le temps 4...)

                NumSemaines.Insert(0, numsemaine);

                numsemaine--;
                if (numsemaine < 0)
                {
                    numsemaine = 52;
                    annee--;
                }
            }


        }
    }

    public class CommandeEnCoursGraph
    {
        public List<double> nbCommandes { get; set; }

        public CommandeEnCoursGraph()
        {

            nbCommandes = new List<double>() { 0, 0, 0 };

            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();

            DateTime now = DateTime.Now;
            int numsemaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var query = db.KPI_STC.Where(i => i.Semaine == numsemaine && i.Annee == now.Year);

            if (query != null || query.Count() > 0)
            {
                //throw new Exception("Il n'y a pas de stc");


                List<KPI_STC> list_stc = query.ToList();

                foreach (var s in list_stc)
                {
                    nbCommandes[0] += (double)s.Value1;
                    nbCommandes[1] += (double)s.Value5;
                    nbCommandes[2] += (double)s.Value4;
                }
            }

        }
    }

    public class FluxCommandeSemaineGraph
    {
        public List<double> nb { get; set; }
        public List<double> ca { get; set; }

        public FluxCommandeSemaineGraph()
        {
            nb = new List<double>() { 0, 0, 0 };
            ca = new List<double>() { 0, 0, 0 };

            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();

            DateTime now = DateTime.Now;
            int numsemaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var query = db.KPI_STC.Where(i => i.Semaine == numsemaine && i.Annee == now.Year);

            if (query != null || query.Count() > 0)
            {
                //throw new Exception("Il n'y a pas de stc");


                List<KPI_STC> list_stc = query.ToList();

                foreach (var s in list_stc)
                {
                    nb[0] += (double)s.Value1;
                    nb[1] += (double)(s.Value3 + s.Value4 + s.Value5 + s.Value6);
                    nb[2] += (double)s.Value2;

                    ca[0] += (double)s.Value10;
                    ca[1] += (double)s.Value11;
                    ca[2] += (double)s.Value12;
                }
            }
        }
    }


    public class EvolutionCommandeGraph
    {
        public List<int> NumSemaines { get; set; }
        public Dictionary<String, List<double>> datas { get; set; }

        public EvolutionCommandeGraph()
        {
            datas = new Dictionary<string, List<double>>();
            NumSemaines = new List<int>();

            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();

            DateTime now = DateTime.Now;

            int numsemaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            int annee = now.Year;

            // On récupère les 6 dernières semaines au fur et à mesure
            for (int i = 0; i < 6; i++)
            {
                var query = db.KPI_STC.Where(j => j.Semaine == numsemaine && j.Annee == annee);

                if (query != null && query.Count() > 0)
                {
                    List<KPI_STC> list_stc = query.ToList();

                    // Pour chaque semaines on récupère la liste des stc (techniciens)
                    foreach (var s in list_stc)
                    {
                        // Si le stc n'est encore pas ajouté, on le créé en initialisant la liste
                        if (!datas.ContainsKey(s.OPERATEURS.INITIAL))
                        {
                            datas.Add(s.OPERATEURS.INITIAL, new List<double>());
                        }

                        // On ajoute le nombre d'offre de la semaine
                        if (s.Value13 == null)
                        {
                            datas[s.OPERATEURS.INITIAL].Insert(0, 0);
                        }
                        else
                        {
                            datas[s.OPERATEURS.INITIAL].Insert(0, (double)s.Value1);
                        }

                    }

                }
                // TODO: Corriger le problème qui fait que toutes les données par STC sont les mêmes (tout le temps 3, tout le temps 4...)

                NumSemaines.Insert(0, numsemaine);

                numsemaine--;
                if (numsemaine < 0)
                {
                    numsemaine = 52;
                    annee--;
                }
            }


        }
    }

    public class FluxATraiterEntreeGraph
    {
        public Dictionary<String, double> datas;

        public FluxATraiterEntreeGraph()
        {
            datas = new Dictionary<string, double>();

            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();

            DateTime now = DateTime.Now;
            int numsemaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var query = db.KPI_STC.Where(i => i.Semaine == numsemaine && i.Annee == now.Year);

            if (query != null || query.Count() > 0)
            {
                //throw new Exception("Il n'y a pas de stc");


                List<KPI_STC> list_stc = query.ToList();

                foreach (var s in list_stc)
                {
                    datas.Add(s.OPERATEURS.INITIAL, (double)(s.Value3 + s.Value5 + s.Value6));
                }
            }
        }
    }

    public class FluxEnCoursRelance
    {
        public Dictionary<String, List<double>> datas;

        public FluxEnCoursRelance()
        {
            datas = new Dictionary<string, List<double>>();

            PEGASE_PROD2Entities2 db = new PEGASE_PROD2Entities2();

            DateTime now = DateTime.Now;
            int numsemaine = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var query = db.KPI_STC.Where(i => i.Semaine == numsemaine && i.Annee == now.Year);

            if (query != null || query.Count() > 0)
            {
                //throw new Exception("Il n'y a pas de stc");


                List<KPI_STC> list_stc = query.ToList();

                foreach (var s in list_stc)
                {
                    datas.Add(s.OPERATEURS.INITIAL, new List<double>() { (double)s.Value6, (double)s.Value4 });
                }
            }
        }
    }
}