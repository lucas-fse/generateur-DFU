using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace GenerateurDFUSafir.Models.DAL
{
    public class GestionTracaProd
    {
        public List<TracaPack> GetInfo(string NmrOf, string  NmrOrder)
        {
            List<TracaPack> result = new List<TracaPack>();
            Regex OfRegex = new Regex("F[0-9]{7}");
            Regex NmrOrderRegex = new Regex("[0-9]{1,2}");
            String NmrOFbase = "";
            String NmrOFordre = "";
            bool validof = false;
            bool validnmr = false;
            if (OfRegex.IsMatch(NmrOf.Trim()))
            {
                NmrOFbase = NmrOf.Substring(1, 7).Trim();
                validof = true;
            }
            if (NmrOrderRegex.IsMatch(NmrOrder.Trim()))
            {
                NmrOFordre = Convert.ToInt32( NmrOrder).ToString("00").Trim();
                validnmr = true;
            }
            if (validnmr && validof)
            {                
                GAMME_UD_UR_UCEntities db1 = new GAMME_UD_UR_UCEntities();
                GAMME_KEPLEREntities db2 = new GAMME_KEPLEREntities();
                string NMRSERIE = NmrOFbase + NmrOFordre;
                var query1 = db1.TRACAFABUDR.Where(p => p.NMRSERIE.StartsWith(NMRSERIE));
                var query2 = db2.MOMT.Where(p => p.NMR_SERIE.StartsWith(NMRSERIE));
                if (query1!= null && query1.Count()>0)
                {
                    foreach(var ligne in query1)
                    {
                        result.Add(new TracaPack {
                            TypeGamme = "UDR",
                            ID = ligne.INDEXREF,
                            NmrOf = NmrOf,
                            NmrOrder = NmrOrder
                            ,Caract1 = "MT:"+ligne.CODEID
                            ,Caract2 = ((DateTime)ligne.DATETIME).ToString("dd/MM/yyy")});
                    }
                    return result;
                }
                else if (query2 != null && query2.Count() > 0)
                {
                    foreach (var ligne in query2)
                    {
                        if (ligne.IsMO!= null && !(bool)ligne.IsMO)
                        {
                            var query5 = db2.ORDRE_FABRICATION.Where(p => p.ID == ligne.ID_ORDRE_FABRICATION);
                            if (query5 != null && query5.Count() == 1 && query2.Count()>1)
                            {
                                ORDRE_FABRICATION_KEPLER tmp = query5.First();

                                result.Add(new TracaPack
                                {
                                    TypeGamme = "KEPLER",
                                    ID = ligne.ID_ORDRE_FABRICATION,
                                    NmrOf = NmrOf,
                                    NmrOrder = NmrOrder,
                                    Caract1 = "Pack:" + tmp.REF_COMMERCIAL_MT,
                                    Caract2 = ((DateTime)ligne.DATE_PROGRAMMATION).ToString("dd/MM/yyy")
                                });
                            }
                        }
                    }
                    return result;
                }
            }
            return result;
        }
        public bool PrintEtiquette(long ID,String Gamme)
        {
            GAMME_UD_UR_UCEntities db1 = new GAMME_UD_UR_UCEntities();
            GAMME_KEPLEREntities db2 = new GAMME_KEPLEREntities();
            
            var query1 = db1.TRACAFABUDR.Where(p => p.INDEXREF == ID);
            var query2 = db2.MOMT.Where(p => p.ID_ORDRE_FABRICATION == ID);
            try
            {
                if (Gamme.Contains("UDR") && query1 != null & query1.Count() == 1)
                {
                    string path = @"\\Jaysvrfiles\donnees_production\DONNEES_INDUSTRIELLES\DOSSIER UD\DialogUDProd\ProgUD\";
                    string NmrOF = "F" + query1.First().NMRSERIE.Substring(0, 7);
                    string nmrserie = query1.First().NMRSERIE.Substring(0, 9);
                    string[] fileEntries = Directory.GetFiles(string.Concat(path, NmrOF), String.Concat("*", nmrserie, "*.cmd"));
                    string pathcopie = Resource1.REP_SCRUTATION;
                    int cpt = 0;
                    foreach (var file in fileEntries)
                    {
                        cpt++;
                        File.Copy(file, string.Concat(pathcopie, @"/UDR_NEW_ETIQUETTE", cpt.ToString("000"),".cmd"));
                    }
                }
                else if (Gamme.Contains("KEPLER") && query2 != null & query2.Count() > 0)
                {
                    string path = @"\\Jaysvrfiles\donnees_production\DONNEES_INDUSTRIELLES\DOSSIER KEPLER\DialogKepler\PackInstalle V1\";
                    string NmrOF = "F" + query2.First().NMR_SERIE.Substring(0, 7);
                    string nmrserie = query2.First().NMR_SERIE.Substring(0, 9);
                    string[] fileEntries = Directory.GetFiles(string.Concat(path, NmrOF), String.Concat("*", nmrserie, "*.cmd"));
                    string pathcopie = Resource1.REP_SCRUTATION2;
                    int cpt = 0;
                    foreach (var file in fileEntries)
                    {
                        cpt++;
                        File.Copy(file, string.Concat(pathcopie, @"\UDR_NEW_ETIQUETTE", cpt.ToString("000"),".cmd"));
                    }
                }
            }
            catch { return false; }
            

            return true;
        }
        public bool DeleteID(string gamme,long? ID,long? IDOPE)
        {
            GAMME_UD_UR_UCEntities db1 = new GAMME_UD_UR_UCEntities();
            GAMME_KEPLEREntities db2 = new GAMME_KEPLEREntities();
            bool result = true;
            long idbase = -1;
            try
            {
                if (ID!= null)
                {
                    idbase = (long)ID;
                }
                else
                {
                    idbase = -1;
                    result = false;
                }
            }
            catch
            {
                idbase = -1;
                result = false;
            }
            if (result == true)
            {
                try
                {
                    var query1 = db1.TRACAFABUDR.Where(p => p.INDEXREF == idbase);
                    var query2 = db2.ORDRE_FABRICATION.Where(p => p.ID == idbase);
                    if ((query1 != null && query1.Count() == 1) && gamme.Contains("UDR"))
                    {
                        db1.TRACAFABUDR.Remove(query1.First());
                        TRACA_ETAPE_FAB tmp = new TRACA_ETAPE_FAB();
                        try
                        {
                            tmp.NMR_ORDRE = Convert.ToInt32(query1.First().NMRSERIE.Substring(8, 2));
                            tmp.NUM_OF = "F" + query1.First().NMRSERIE.Substring(0, 7);
                        }
                        catch
                        {
                            tmp.NMR_ORDRE = -1;
                            tmp.NUM_OF = "";
                        }
                        tmp.PARAMS1 = "SUPPRESSION_PROG_KEPLER";
                        tmp.PARAMS2 = "IDOPE:" + IDOPE.ToString();
                        tmp.PARAMS3 = query1.First().NMRSERIE;
                        db2.TRACA_ETAPE_FAB.Add(tmp);

                        db2.SaveChanges();
                        db1.SaveChanges();
                        return result;
                    }
                    else if ((query2 != null && query2.Count() ==1 ) && gamme.Contains("KEPLER"))
                    {
                        long idordrefabrication = idbase;
                        var query3 = db2.MOMT.Where(p => p.ID_ORDRE_FABRICATION == idordrefabrication);
                        foreach (var ligne in query3)
                        {
                            db2.MOMT.Remove(ligne);
                        }
                        var query4 = db2.ORDRE_FABRICATION.Where(p => p.ID == idordrefabrication);
                        TRACA_ETAPE_FAB tmp = new TRACA_ETAPE_FAB();
                        
                        tmp.NMR_ORDRE = query4.First().NMRORDRE;
                        tmp.NUM_OF = query4.First().NUM_OF;
                        tmp.PARAMS1 = "SUPPRESSION_PROG_KEPLER";
                        tmp.PARAMS2 = "IDOPE:" + IDOPE.ToString();
                        db2.TRACA_ETAPE_FAB.Add(tmp);

                        db2.SaveChanges();


                        db2.ORDRE_FABRICATION.Remove(query4.First());
                        
                        return result;
                    }
                }
                catch
                {
                    result = false;
                }
            }
            return false;
        }
       
        public void EtiquetteTracaProdAdd(TRACA_ETIQUETTES et)
        {
            try
            {
                TRACA_OUTILSEntities db = new TRACA_OUTILSEntities();
                db.TRACA_ETIQUETTES.Add(et);
                db.SaveChanges();
            }
            catch (Exception ex)
            {

            }            
        }
        public void TracaAction(string action,string description)
        {
            try
            {
                TRACA_OUTILSEntities db = new TRACA_OUTILSEntities();
                TRACA_DATA tmp = new TRACA_DATA();
                tmp.ACTION = action;
                tmp.DESCRIPTION = description;
                db.TRACA_DATA.Add(tmp);
                db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
