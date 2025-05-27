using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class PbLivraisonsFournisseur
    {

        public Dictionary<int, PB_LIVRAISONS_FOURNISSEUR> ListPbLivraisonsFournisseur { get; set; }

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

        public List<PB_LIVRAISONS_FOURNISSEUR> DerniersProblemes { get; set; }

        public PbLivraisonsFournisseur()
        {
            ListPbLivraisonsFournisseur = new Dictionary<int, PB_LIVRAISONS_FOURNISSEUR>();
            DateTime now = DateTime.Now;
            PEGASE_PROD2Entities2 pEGASE_PROD2Entities2 = new PEGASE_PROD2Entities2();
            List<PB_LIVRAISONS_FOURNISSEUR> pbParAnnee = pEGASE_PROD2Entities2.PB_LIVRAISONS_FOURNISSEUR.Where(p => p.Date.Year >= now.Year).ToList();

            _casesQcross = new CasesQcrossType[31];
            for (int i = 0; i < 31; i++)
            {
                int currentDay = i + 1; // correction ici

                List<PB_LIVRAISONS_FOURNISSEUR> tmp = pbParAnnee
                    .Where(d => d.Date.Month == now.Month && d.Date.Day == currentDay)
                    .ToList();

                _casesQcross[i] = new CasesQcrossType();
                _casesQcross[i].Visible = true;

                if (currentDay <= now.Day)
                {
                    if (tmp.Count() > 0)
                    {
                        _casesQcross[i].Couleur = CasesQcrossType.CasesColor.Red;
                    }
                    else
                    {
                        _casesQcross[i].Couleur = CasesQcrossType.CasesColor.Green;
                    }
                }
                else
                {
                    _casesQcross[i].Couleur = CasesQcrossType.CasesColor.Grey;
                }
            }

            // cacher les jours en trop
            for (int i = DateTime.DaysInMonth(now.Year, now.Month); i < 31; i++)
            {
                _casesQcross[i].Visible = false;
            }

            DerniersProblemes = new List<PB_LIVRAISONS_FOURNISSEUR>();
            DerniersProblemes = pEGASE_PROD2Entities2.PB_LIVRAISONS_FOURNISSEUR.OrderByDescending(p => p.Date).Take(10).ToList();
        }

        public int AddPbLivraisonsFournisseur(PB_LIVRAISONS_FOURNISSEUR pbLivFournisseur)
        {
            int result = 0;
            try
            {
                PEGASE_PROD2Entities2 _db = new PEGASE_PROD2Entities2();
                PB_LIVRAISONS_FOURNISSEUR lf = new PB_LIVRAISONS_FOURNISSEUR();
                lf.Date = pbLivFournisseur.Date;
                lf.Ncommande = pbLivFournisseur.Ncommande;
                lf.NaturePB = pbLivFournisseur.NaturePB;
                lf.Probleme = pbLivFournisseur.Probleme;
                lf.Resolution = pbLivFournisseur.Resolution;
                _db.PB_LIVRAISONS_FOURNISSEUR.Add(lf);
                _db.SaveChanges();
                result = 1;
            }
            catch (Exception e)
            {
                result = -1;
            }
            return result;
        }
    }
}