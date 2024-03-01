using GenerateurDFUSafir.DAL;
using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class RetDInformation
    {
        public List<OPERATEURS> DataOperateurs;
        private string Service { get; set; }
        public RetDInformation(string service)
        {
            Service = service;
        }

        public void RefreshDataUser()
        {
            try
            {
                if (true)
                {
                    OfX3 data = new OfX3();
                    DataOperateurs = data.ListOPERATEURs(Service).OrderBy(p => p.NOM).ToList();
                }
            }
            catch (Exception e)
            {

            }
        }
        public List<TEMPS_SAISI>  RefreshSaisiTemps(int Id)
        {
            List<TEMPS_SAISI> result = new List<TEMPS_SAISI>();
            try
            {
                if (true)
                {
                    OfX3 data = new OfX3();
                    result = data.ListTempsSaisiById(Id);
                }
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public OPERATEURS DataOperateur(int Id)
        {
            OPERATEURS result = new OPERATEURS();
            try
            {
                if (true)
                {
                    OfX3 data = new OfX3();
                    result = data.ListOPERATEURs().Where(p => p.ID == Id).First();    
                }
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public List<SOUSPROJET> ListSousProjet()
        {
            List<SOUSPROJET> result = new List<SOUSPROJET>();
            try
            {
                if (true)
                {
                    OfX3 data = new OfX3();
                    result = data.ListSousProjet().Where(s => s.Service.Contains(Service)).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public List<SOUSPROJET> ListSousProjetObligatoire()
        {
            List<SOUSPROJET> result = new List<SOUSPROJET>();
            try
            {
                if (true)
                {
                    OfX3 data = new OfX3();
                    result = data.ListSousProjet().Where(s => s.Affichage == true && s.Service.Contains(Service)).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public List<SOUSPROJET> ListALLSousProjetALLService()
        {
            List<SOUSPROJET> result = new List<SOUSPROJET>();
            try
            {
                if (true)
                {
                    OfX3 data = new OfX3();
                    result = data.ListSousProjet().ToList();
                }
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public List<SOUSPROJET> ListSousProjetNonObligatoire()
        {
            List<SOUSPROJET> result = new List<SOUSPROJET>();
            try
            {
                if (true)
                {
                    OfX3 data = new OfX3();
                    result = data.ListSousProjet().Where(s => s.Affichage == false && s.Service.Trim().Equals(Service)).ToList();
                }
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public int AddNewWeekID(long Id, TEMPS_SAISI week,long idsousprojet)
        {
            List<TEMPS_SAISI> result = new List<TEMPS_SAISI>();
            try
            {
                if (true)
                {
                    OfX3 data = new OfX3();
                    OPERATEURS op = data.ListOPERATEURs().Where(o => o.ID == Id).First(); 
                    
                    
                    data.SaveDbTempsSemainer(op, week);
                    
                }
            }
            catch (Exception e)
            {

            }
            return 0;
        }
        public int CreateOrUpdateSemaine(OPERATEURS op, TEMPS_SAISI week, long idsousprojet)
        {
            List<TEMPS_SAISI> result = new List<TEMPS_SAISI>();
            try
            {
                if (true)
                {
                    OfX3 data = new OfX3();
                    result = data.ListTempsSaisiById(op.ID);
                    data.SaveDbTemsSaisi(op, week);
                }
            }
            catch (Exception e)
            {

            }
            return 0;
        }
        public int CkeckTime(short id,int semaine, int annee)
        {
            List<TEMPS_SAISI> result = new List<TEMPS_SAISI>();
            try
            {
                if (true)
                {
                    OfX3 data = new OfX3();
                    OPERATEURS op = data.ListOPERATEURs().Where(i => i.ID == id).First();
                    double Tempsafaire = 38.5;
                    if (op != null && op.HoraireByWeek!= null)
                    {
                        Tempsafaire = (double)op.HoraireByWeek;
                    }
                    result = data.ListTempsSaisiById(id).Where(p =>p.Semaine == semaine && p.Annee == annee).ToList();
                    double total = 0;
                    foreach(var ligne in result)
                    {
                        total += ligne.Days1 + ligne.Days2 + ligne.Days3 + ligne.Days4 + ligne.Days5 + ligne.Days6 + ligne.Days7;
                    }
                    bool semainefull = false;
                    if (Math.Round(total,1)== Tempsafaire)
                    {
                        semainefull = true;
                    }
                    else
                    {
                        semainefull = false;
                    }
                    data.SaveDbTempsSemaine( id, semaine,annee,  semainefull);
                }
            }
            catch (Exception e)
            {

            }
            return 0;
        }
        public int UpdateSSProjet(SOUSPROJET ssprojet)
        {
            SOUSPROJET result = new SOUSPROJET();
            try
            {
                if (true)
                {
                    OfX3 data = new OfX3();
                    result = data.ListSousProjet().Where(p=> p.IDSOUSPROJET == ssprojet.IDSOUSPROJET).First();
                    result.NomSousProjet = ssprojet.NomSousProjet;
                    result.TitreSousProjet = ssprojet.TitreSousProjet;
                    result.Commentaire = ssprojet.Commentaire;
                    result.Service = ssprojet.Service;
                    result.Affichage = ssprojet.Affichage;
                    result.DateFinProjet = ssprojet.DateFinProjet;
                    data.SaveDbSSProjet(result);
                }
            }
            catch (Exception e)
            {

            }
            return 0;
        }
        public int CreateSSProjet(SOUSPROJET ssprojet)
        {
            try
            {
                if (true)
                {
                    OfX3 data = new OfX3();
                    data.SaveDbAddSSProjet(ssprojet);
                }
            }
            catch (Exception e)
            {

            }
            return 0;
        }
    }
}