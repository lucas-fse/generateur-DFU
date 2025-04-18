using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public class ControlFinalScanPack
    {
            public int TypeSuivie { get; set; }
            public string TCLCOD_0 { get; set; }
            public string Emplacement { get; set; }
            public string NumOf { get; set; }
            public string ItemRef { get; set; }
            public string ItemDescript { get; set; }
            public int QTr { get; set; } 
            public List<ControlFinalScan> ListArticle { get; set; }        
    }
    public class ControlFinalScan
    {

        public string NumOf { get; set; }
        public string ItemRef { get; set; }
        public string ItemDescript { get; set; }
        public int QTr { get; set; }
        public int TypeSuivie { get; set; }

    }
    public class ControleFinalScan
    {
        public ControlFinalScanPack Pack { get; set; }
        public List<string> ArticleDejaScanné { get; set; }
        public int? NbPackDejaScanné { get; set; }
        public OfProdModel NomenclatureOF { get; set; }


        public void  GetOFSearch(string LigneDeScan)
        {
            string of = CheckLigneDeScan(LigneDeScan);
            Pack = new ControlFinalScanPack();


            OfProdModel nomenclatureOF = new OfProdModel();
            nomenclatureOF.OfProdModelFull(of);
            NomenclatureOF = nomenclatureOF;

            List <ControleFinal> result = new List<ControleFinal>();

            DataTable rawResult1 = new DataTable();
            DataTable rawResult2 = new DataTable();
            ofProdIieCmd Ofs = new ofProdIieCmd();

            /*Faire une requete ici*/
            ModelOF1.RequeteControleFinalScan(of, ref rawResult1,ref rawResult2);
            // liste des contenu de l'of
            if (rawResult1 != null && rawResult1.Rows != null && rawResult1.Rows.Count == 1)
            {
                DataRow row = rawResult1.Rows[0];
                Pack.NumOf = row["MFGNUM_0"].ToString();
                Pack.ItemRef = row["ITMREF_0"].ToString();
                Pack.ItemDescript = row["MFGDES_0"].ToString();
                string tmp = row["EXTQTY_0"].ToString();
                Pack.QTr = (int)Convert.ToDouble(row["EXTQTY_0"].ToString());
                Pack.ListArticle = new List<ControlFinalScan>();
                Pack.TCLCOD_0 = row["TCLCOD_0"].ToString();
                if (!string.IsNullOrWhiteSpace(row["emplacemntITEMPROD"].ToString()))
                {
                    Pack.Emplacement = row["emplacemntITEMPROD"].ToString();
                }
                else if (!string.IsNullOrWhiteSpace(row["emplacemntITEMMZ"].ToString()))
                {
                    Pack.Emplacement = row["emplacemntITEMMZ"].ToString();
                }
                else if (!string.IsNullOrWhiteSpace(row["emplacemntITEMBZ"].ToString()))
                {
                    Pack.Emplacement = row["emplacemntITEMBZ"].ToString();
                }
                else
                {
                    Pack.Emplacement = "";
                }

                // serialié?
                if (row["SERMGTCOD_0"].ToString() == "2" || row["SERMGTCOD_0"].ToString() == "3")
                {
                    Pack.TypeSuivie = 2;
                }
                else if (row["LOTMGTCOD_0"].ToString() == "3" || row["LOTMGTCOD_0"].ToString() == "4")
                {
                    Pack.TypeSuivie = 1;
                }
                else
                {
                    Pack.TypeSuivie = 0;
                }

                if (rawResult2 != null && rawResult2.Rows != null && rawResult2.Rows.Count > 0)
                {
                    foreach (DataRow rowArt in rawResult2.Rows)
                    {
                        ControlFinalScan art = new ControlFinalScan();
                        art.NumOf = rowArt["MFGNUM_0"].ToString();
                        art.ItemRef = rowArt["ITMREF_0"].ToString();
                        art.ItemDescript = rowArt["ITMDES1_0"].ToString();
                        art.QTr = (int)Convert.ToDouble(rowArt["LIKQTY_0"].ToString());
                        // serialié?
                        if (rowArt["SERMGTCOD_0"].ToString() == "2" || rowArt["SERMGTCOD_0"].ToString() == "3")
                        {
                            art.TypeSuivie = 2;
                        }
                        else if (rowArt["LOTMGTCOD_0"].ToString() == "3" || rowArt["LOTMGTCOD_0"].ToString() == "4")
                        {
                            art.TypeSuivie = 1;
                        }
                        else
                        {
                            art.TypeSuivie = 0;
                        }
                        Pack.ListArticle.Add(art);
                    }
                }

            }
            List<HISTORIQUE_CONTROL> Listnbpack = null;
            List<string> listresult = GetControlPackAlwaysScan(of , ref Listnbpack);
            
            ArticleDejaScanné = ControlAndCompleRSL(listresult );
            // cas des suremballage uniquement un seul article dans le pack on colle juste une étiquette
            if (Pack.ListArticle != null && Pack.ListArticle.Count() ==1)
            {
                Pack.ListArticle = new List<ControlFinalScan>();
            }
            NbPackDejaScanné = GetNbPack(Listnbpack);
            
        }
        public bool RemoveControlPack(string NmrOF)
        {
            

            PEGASE_CONTROLEntities db = new PEGASE_CONTROLEntities();
            var query = db.HISTORIQUE_CONTROL.Where(p => p.NumOF.Contains(NmrOF.Trim()));
            if (query !=null && query.Count()>0)
            {
                List<HISTORIQUE_CONTROL> tmp = query.ToList();
                foreach(var entity in tmp)
                {
                    db.HISTORIQUE_CONTROL.Remove(entity);
                }
                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
                
            }
            return false;
        }
        private string CheckLigneDeScan(string ligneDeScan)
        {
            string result = "";
            if (ligneDeScan != null)
            {
                ligneDeScan = ligneDeScan.Trim();
                Regex RegexNmrOf = new Regex("^F[0-9]{7}$");
                Regex RegexRSL = new Regex(@"^RSL;([A-Z0-9-]*);([A-Z0-9-]*);([A-Z0-9-]*);?([A-Z0-9]*)?");
                if (RegexNmrOf.IsMatch(ligneDeScan))
                {
                    result = ligneDeScan;
                }
                else if (RegexRSL.IsMatch(ligneDeScan))
                {
                    string item = ""; string nmrserie = ""; string lot = ""; string compteur = "";
                    var match = RegexRSL.Match(ligneDeScan);
                    if (match.Groups[1] != null) { item = match.Groups[1].Value; }
                    if (match.Groups[2] != null) { nmrserie = match.Groups[2].Value; }
                    if (match.Groups[3] != null) { lot = match.Groups[3].Value; }
                    if (match.Groups[4] != null) { compteur = match.Groups[4].Value; }
                    string chaine = "RSL;" + item + ";" + nmrserie + ";";
                    if (!string.IsNullOrWhiteSpace(item) && !string.IsNullOrWhiteSpace(nmrserie) && nmrserie.Length > 7)
                    {
                        result = "F" + nmrserie.Substring(0, 7);
                    }
                    else if (!string.IsNullOrWhiteSpace(item) && !string.IsNullOrWhiteSpace(lot))
                    {
                        if (RegexNmrOf.IsMatch(lot.Trim()))
                        {
                            result = lot;
                        }
                    }
                }
            }
            return result;
        }
        private List<string> GetControlPackAlwaysScan(string NmrOF, ref List<HISTORIQUE_CONTROL> Listnbpack)
        {
            PEGASE_CONTROLEntities db = new PEGASE_CONTROLEntities();
            List<string> result = new List<string>();
            Listnbpack = new List<HISTORIQUE_CONTROL>();
            var query = db.HISTORIQUE_CONTROL.Where(p => p.NumOF.Contains(NmrOF.Trim()));

            if (query!= null && query.Count()>0)
            {
                result = query.Select(p => p.RawScan).ToList();
                Listnbpack = query.ToList();
            }
            return result;
        }
        private List<string> ControlAndCompleRSL(List<string> Liste)
        {
            Regex RegexRSL = new Regex(@"^RSL;([A-Z0-9-]*);([A-Z0-9-]*);([A-Z0-9-]*);?([A-Z0-9]*)?");
            List<string> result = new List<string>();
            int compteurArticle = 0;
            
            foreach (var ligne in Liste)
            {
                compteurArticle++;
                if (ligne != null && RegexRSL.IsMatch(ligne))
                {
                    string item = ""; string nmrserie = ""; string lot = ""; string compteur = "";
                    var match = RegexRSL.Match(ligne);
                    if (match.Groups[1] != null) { item = match.Groups[1].Value; }
                    if (match.Groups[2] != null) { nmrserie = match.Groups[2].Value; }
                    if (match.Groups[3] != null) { lot = match.Groups[3].Value; }
                    if (match.Groups[4] != null) { compteur = match.Groups[4].Value; }
                    string chaine = "RSL;" + item + ";" + nmrserie + ";";
                    if (!string.IsNullOrWhiteSpace(nmrserie))
                    {
                        chaine = ligne;
                    }
                    else if (!string.IsNullOrWhiteSpace(lot))
                    {
                        chaine = chaine + lot ;
                        if (!string.IsNullOrWhiteSpace(compteur))
                        {
                            chaine = chaine + compteur;
                        }
                        else
                        {
                            chaine = chaine + ";" +"AAA";
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(lot)&& string.IsNullOrWhiteSpace(nmrserie))
                    {
                        chaine = chaine + "FXXXXX" + ";" + compteurArticle.ToString("000") ;
                    }
                    else
                    {
                        chaine = ligne;
                    }
                    result.Add(chaine);
                }
                else if (ligne != null)
                {
                    string chaine = "RSL;" + ligne + ";;FXXXX;"+ compteurArticle.ToString("000");
                    result.Add(chaine);
                }
            }
            return result;
        }
        private int? GetNbPack( List<HISTORIQUE_CONTROL>  Listnbpack)
        {
            int compteur = 0;
            var query = Listnbpack.Where(p => p.IsPack == true ).OrderBy(p => p.NumPack);
            if (query != null && query.Count() > 0)
            {
                foreach (var index in query.ToList())
                {
                    if (index != null && (index.NumPack+1) == compteur + 1)
                    {
                        if (index.Quantite != null)
                        {
                            compteur = compteur + 1 * (int)index.Quantite;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return compteur;
        }
    }
}