
using GenerateurDFUSafir.Models.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GenerateurDFUSafir.Models
{
    public static class JobERP
    {
        public static bool CreateFindOF(OF_PROD_TRAITE ofasolde, bool solde)
        {
            string path = string.Concat(Resource1.REP_JOB, ofasolde.NMROF, "\\");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            DateTime now = DateTime.Now;
            string filename = "";
            if (CreateFileOf(ofasolde, path,now, solde,ref filename))
            {
                if (CreateFileJob(filename, filename, "ZMESMKI", path, now))
                {
                    //return (CopyFileToJob(path, now));
                    return true;
                }
            }
            return false;
        }
        private static bool CopyFileToJob(string path, DateTime now)
        {
            bool result = true;
            string pathx3 = Resource1.REP_SCRIPT_X3;
            IEnumerable<String> allFiche = Directory.EnumerateFiles(path, "*"+ now.Ticks.ToString()+".txt", SearchOption.TopDirectoryOnly);
            try
            {
                foreach (var file in allFiche)
                {
                    // Remove path from the file name.
                    string fName = file.Substring(path.Length);
                    File.Copy(file, Path.Combine(pathx3, fName), true);
                }
            }
            catch
            { result = false; }
            return result;
        }
        private static bool CreateFileJob(string filename,string filenametxt,string TYPEIMPORT,  string path,DateTime now)
        {
            bool result = true;
            try
            {
                
                
                string date = now.Year.ToString("D4") + now.Month.ToString("D2") + now.Day.ToString("D2");
                string heure = now.Hour.ToString("D2") + now.Minute.ToString("D2");
                FileStream fileStream = new FileStream(string.Concat(path, "\\JOB_X3_", filename, now.Ticks.ToString(), ".cmd"), FileMode.Create);
                StreamWriter writer = new StreamWriter(fileStream);
                writer.WriteLine("DOSSIER=" + Resource1.DOSSIER_IMPORT);
                writer.WriteLine("UTIL=" + Resource1.UTIL);
                writer.WriteLine("PASSE=" + Resource1.PASS);
                writer.WriteLine("GRP=");
                writer.WriteLine("TACHE=ZIMPORT1");
                writer.WriteLine("DATE=" + date);
                writer.WriteLine("HEURE=" + heure);
                writer.WriteLine("MODIMP=" + TYPEIMPORT); // "ZMESMKI",....);
                string tmp1 = string.Concat(Resource1.NOMJAY, filenametxt, ".txt");
                writer.WriteLine("NOMIMP=" + tmp1);
                writer.WriteLine("TYPEXP=2");
                writer.WriteLine("ZMODIMP=import suivant le modele P&P");
                writer.WriteLine("");
                writer.Close();
                fileStream.Close();
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        private static bool CreateFileOf(OF_PROD_TRAITE ofasolde, string path, DateTime now,bool solde, ref string filename)
        {
            bool result = true;
            string asolde="2";
            if (solde) { asolde = "1"; }
            try
            {    
                string date = now.Year.ToString("D4") + now.Month.ToString("D2") + now.Day.ToString("D2");
                string heure = now.Hour.ToString("D2") + now.Minute.ToString("D2");
                filename = string.Concat( "\\OF_", ofasolde.NMROF, "_", ofasolde.OPERATEUR.ToString(), "_", now.Ticks.ToString(), ".txt");
                FileStream fileStream = new FileStream(string.Concat(path, filename), FileMode.Create); ;
                StreamWriter writer = new StreamWriter(fileStream);
                string ligne1 = "M;001;" + ofasolde.NMROF + ";" + ofasolde.ITEMREF + ";" + ofasolde.QTRREEL.ToString() + ";UN;" + date + ";"+ asolde;
                string statut = "Q3";
                List<string> ProduitFinis = new List<string> { "PF01", "PF02", "PF03", "PDR01", "PFST1" };
                if (!ProduitFinis.Contains(ofasolde.TCLCOD_0.Trim()))
                {
                    statut = "A";
                }
                string ligne2 ="S;"+ofasolde.EmplacementItem + ";"+ statut+"; ;;1";
                writer.WriteLine(ligne1);
                writer.WriteLine(ligne2);
                writer.WriteLine("");
                writer.Close();
                fileStream.Close();
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
    
        public static bool CreateEditeOF(OF_PROD_TRAITE nmrof)
        {
            string path = string.Concat(Resource1.REP_JOB, nmrof.NMROF, "\\");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            DateTime now = DateTime.Now;
            string filename="";
            if (CreateFileOfAedite(nmrof, path, now, ref filename))
            {
                if (CreateFileJob(filename, filename, "ZMFGEDT", path, now))
                {
                    //return (CopyFileToJob(path, now));
                    return true;
                }
            }
            return false;
        }
        private static bool CreateFileOfAedite(OF_PROD_TRAITE ofasolde, string path, DateTime now,ref string finename)
        {
            bool result = true;
            //ZMFGEDT 
            // 1	A	En attente
            //2   B A l'étude
            //3   C Edité
            //4   D En cours
            //5   E Soldé
            //6   F Prix de revient calculé
            try
            {
                string date = now.Year.ToString("D4") + now.Month.ToString("D2") + now.Day.ToString("D2");
                string heure = now.Hour.ToString("D2") + now.Minute.ToString("D2");
                finename = string.Concat( "OF_", ofasolde.NMROF, "_", ofasolde.OPERATEUR.ToString(), "_", now.Ticks.ToString(), ".txt");
                FileStream fileStream = new FileStream(string.Concat(path, finename), FileMode.Create); ;
                StreamWriter writer = new StreamWriter(fileStream);
                string ligne1 = ofasolde.NMROF + ";3";
                writer.WriteLine(ligne1);
                writer.WriteLine("");
                writer.Close();
                fileStream.Close();
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static bool CreatMouvementArticle(ControlFinalScanPack articleDe, ControlFinalScanPack articleVers)
        {
            string path = string.Concat(Resource1.REP_JOB, "ofasolde.NMROF", "\\");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            DateTime now = DateTime.Now;
            string filename = "";
            if (CreateFileMouvement(articleDe,articleVers, path,ref filename))
            {
                if (CreateFileJob(filename, filename,"ZSCS", path, now))
                {
                    //return (CopyFileToJob(path, now));
                    return true;
                }
            }
            return false;
        }
        private static bool CreateFileMouvement(ControlFinalScanPack articleDe, ControlFinalScanPack articleVers, string path,ref string filename)
        {
            bool result = false;
            try
            {
                DateTime now = DateTime.Now;
                string date =   now.Day.ToString("D2")+ now.Month.ToString("D2") + now.Year.ToString("D4");
                string heure = now.Hour.ToString("D2") + now.Minute.ToString("D2");
                filename = string.Concat( "\\OF_", articleDe.ItemRef, "_", now.Ticks.ToString(), ".txt");
                FileStream fileStream = new FileStream(string.Concat(path, filename), FileMode.Create); ;
                StreamWriter writer = new StreamWriter(fileStream);
                string ligne1 = "E;"+@""""+"001;" + date + ";;" ;
                string ligne2 = "L;1000;" + articleDe.ItemRef + ";UN;1;" + articleDe.QTr.ToString() + ";" + articleDe.TCLCOD_0 + ";" + articleDe.Emplacement + @";"";"";""";
                string ligne3 = "S;UN;-" + articleDe.QTr + ";-" + articleDe.QTr + ";"+ articleDe.Emplacement+";"+articleDe.TCLCOD_0 +";";
                string ligne4 = "S;UN;" + articleVers.QTr + ";" + articleVers.QTr + ";" + articleVers.Emplacement + ";" + articleVers.TCLCOD_0 + ";";
                writer.WriteLine(ligne1);
                writer.WriteLine(ligne2);
                writer.WriteLine("");
                writer.Close();
                fileStream.Close();
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}