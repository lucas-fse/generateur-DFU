using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL_Compteur_Mono
{
    // gestion du compteur mono
    public  static class CompteurMono
    {
        public static string getCompteur(string NomCompteur)
        {
            string result ="";
            return result;
        }
        
        public static int? GetNewCompteur(string NomCompteur)
        {
            try
            {
                GAMME_UD_UR_UCEntities1 Compteur = new GAMME_UD_UR_UCEntities1();

                var query = from ligne in Compteur.COMPTEUR
                            where ligne.CODE_COMPTEUR == NomCompteur
                            select ligne;
                int nextcompteur = -1;
                int verifcompteur = -2;
                if (query.Count() == 1)
                {
                    nextcompteur = Convert.ToInt32(query.First().NEXT_NUM_CHRONO);
                    nextcompteur++;
                    string cpt = nextcompteur.ToString();
                    query.First().NEXT_NUM_CHRONO = cpt;
                    Compteur.SaveChanges();
                }
                if (query.Count() == 1)
                {
                    verifcompteur = Convert.ToInt32(query.First().NEXT_NUM_CHRONO);
                }
                if (verifcompteur == nextcompteur + 1)
                {
                    TRACAFABUDE newtrace = new TRACAFABUDE();
                    newtrace.DATETIME = DateTime.Now;
                    newtrace.REFERENCE = "CODEID";
                    newtrace.NMRSERIE = nextcompteur.ToString();
                    Compteur.TRACAFABUDE.Add(newtrace);
                    Compteur.SaveChanges();
                    return nextcompteur;
                }
            }
            catch
            {

            }
            return null;
        }
    }
}
