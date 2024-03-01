using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO.Packaging;
using System.Security.Cryptography;

namespace CheckFPS
{
    public enum ErrorC
    {
        NoError = 0,
        FPSRootError = -1,
        DirectoryError = -2,
        NoFiches = -3,
        BDDError = -4
    }
    public enum ErrorFiche
    {
        NoError = 0,
        DateError = -1,
        OpenFile = -2,
        QueryError = -3,
        QueryError2 = -4,
        Exception = -5
    }

    public class ParserFPS
    {
        #region constantes

        //private const String regexFPS = "^FPSd{5}_d{2}$";
        private const String regexFPS = "(^FPS\\d{5}_\\d{2}$)|((^[CKGDBSFLJM]{1}FP\\d{4}$))";
        private const String regexFPSDir = "(FPS\\d{5})|([CKGDBSFLMJ]{1}FP\\d{4})$";
        private const String regexVerIdialog = @"version iDialog : (\d{1,2}.\d{1,2}.\d{1,2})";
        private const String regexDateLastCompil = @"date : (\d{1,2}/\d{1,2}/\d{2,4} \d{1,2}:\d{1,2}:\d{1,2})";

       


        #endregion
        public ErrorC ParseFPS(string DirectoryFPS)
        {
            ErrorC ErrorCode = ErrorC.FPSRootError;
            Fiches f = null; 
            ErrorCode = ParseOneFPS("", DirectoryFPS, ref f);
            return ErrorCode;
        }
        /// <summary>
        /// Parser toutes les FPS du répertoire courant, mettre à jour la base de données
        /// </summary>
        /// 

        public ErrorC ParseOneFPS(String nonfps, string FPSRoot, ref Fiches resultfiche)
        {
            ErrorC ErrorCode = ErrorC.FPSRootError;
            String FileName;
            String FicheName;
            bool serachAll = true;
            bool Fpsfind = false;
            resultfiche = null;
            Regex fps = new Regex(@"\\(?i)((FPS\d{5}_\d{2}.idialog$)|([@JBCDFGLMS]FP\d{4}.idialog$))");
            if (!String.IsNullOrEmpty(nonfps))
            {
                serachAll = false;
            }
            if (FPSRoot != null)
            {
                ErrorCode = ErrorC.DirectoryError;
                if (Directory.Exists(FPSRoot))
                {
                    IEnumerable<String> allDir = Directory.EnumerateDirectories(FPSRoot, "*FP*", SearchOption.TopDirectoryOnly);

                    ErrorCode = ErrorC.NoFiches;
                    if (allDir.Count() > 0)
                    {
                        ErrorCode = ErrorC.NoError;

                        foreach (String dir in allDir)
                        {
                            if (dir.ToUpper().Contains(nonfps.ToUpper()))
                            {
                                IEnumerable<String> allFiche = Directory.EnumerateFiles(dir, "*FP*.iDialog", SearchOption.TopDirectoryOnly);

                                if ((allFiche != null) && (allFiche.Count() != 0))
                                {
                                    FileName = null;
                                    foreach (string name in allFiche)
                                    {
                                        if (fps.IsMatch(name))
                                        {
                                            FileName = name;
                                            if (name.Contains("SFP1347"))
                                            {

                                            }
                                            break;
                                        }
                                    }
                                   // FileName = allFiche.FirstOrDefault();
                                    if ((FileName != null) && ((serachAll == true) || (FileName.ToUpper().Contains(nonfps.ToUpper()))))
                                    {
                                        Fpsfind = true;
                                        FicheName = Path.GetFileNameWithoutExtension(FileName);
                                        Regex verifFicheName = new Regex(regexFPS);

                                        if (verifFicheName.IsMatch(FicheName))
                                        {
                                            Fiches f = DAL.BDD.Instance.GetFiche(FicheName);
                                            resultfiche = f;
                                            if (f == null)
                                            {
                                                // Créer la ligne de donnée contenant les données de la fiche

                                                f = DAL.BDD.Instance.CreateEmptyFiche(FicheName);
                                                if (f == null)
                                                {
                                                    // attention erreur base de donnée!!
                                                    ErrorCode = ErrorC.BDDError;
                                                    break;
                                                }

                                            }
                                            if (this.MAJFicheData(f, FileName))
                                            {
                                                DAL.BDD.Instance.PegaseCheckFPS.SaveChanges();
                                            }
                                            else
                                            {
                                                // attention erreur base de donnée!!
                                                ErrorCode = ErrorC.BDDError;
                                                break;
                                            }
                                        }
                                        if ((Fpsfind == true) && (serachAll == false))
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {

                                    }
                                }
                                else
                                {
                                    //repertoire mais fiche non trouvé
                                    Regex DirName = new Regex(regexFPSDir);
                                    FicheName = DirName.Match(dir).Value;
                                    Fiches f = DAL.BDD.Instance.GetFiche(FicheName + "_00");
                                    if (f != null)
                                    {
                                        f.RefMO = null;
                                        f.RefMT = null;
                                        f.RefSIM = null;
                                        f.VersionFiche = null;

                                        f.AnyBus = null;
                                        f.TypeFiche = null;
                                        f.OptionExploit = null;
                                        f.OptionIR = null;
                                        f.IsCompiled = false;
                                        f.VerIDialog = null;
                                        f.DateLastCompile = null;
                                        DAL.BDD.Instance.PegaseCheckFPS.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return ErrorCode;
        } // endMethod: ParseFPS

        /// <summary>
        /// Mettre à jour les données d'une fiche
        /// </summary>
        private Boolean MAJFicheData(Fiches fiche, String FileName)
        {
            Boolean Result = true;
            Boolean Check = false;
            Boolean MiseAjour = false;
            String compilInfo;
            //fiche.DateMiseADispo
            FileInfo fi = new FileInfo(FileName);
            DateTime? datefi = fi.LastWriteTime;
            DateTime? dateficrete = fi.CreationTime;
            DateTime? datefiche = fiche.DateMiseADispo;



            DateTime tmpdatefi = new DateTime(datefi.Value.Year, datefi.Value.Month, datefi.Value.Day, datefi.Value.Hour, datefi.Value.Minute, datefi.Value.Second);
            DateTime? checkDate = new DateTime();
            if (datefi> dateficrete)
            {
                checkDate = datefi;
            }
            else
            {
                checkDate = dateficrete;
            }
            try
            {
                if (checkDate == null)
                {
                    fiche.ErrorCheck = (int)ErrorFiche.DateError;
                }
                else if ((datefiche != null) && (datefiche.Value.Year.Equals(checkDate.Value.Year) &&
                    datefiche.Value.Month.Equals(checkDate.Value.Month) &&
                    datefiche.Value.Day.Equals(checkDate.Value.Day) &&
                    datefiche.Value.Hour.Equals(checkDate.Value.Hour) &&
                    datefiche.Value.Minute.Equals(checkDate.Value.Minute) &&
                    datefiche.Value.Second.Equals(checkDate.Value.Second)) &&
                    (fiche.ErrorCheck != (-2)))
                {
                    Check = false;
                }
                else
                {
                    Check = true;
                }
            }
            catch
            {
                Check = true;
            }
            if (FileName.Contains("CFP3446"))
            {

               // Check = true;
            }





            if ((Result == true) && (Check == true))
            {

                IEnumerable<PackagePart> query = null;
                DateTime LAstDateMiseAJour = DateTime.MinValue;
                try
                {
                    using (var zip = ZipArchive.ZipArchive.OpenOnFile(FileName))
                    {
                        var query1 = from filezip in zip.Files
                                where (filezip.ToString().ToUpper().Contains(fiche.NumFiche.ToUpper()) == true) || (filezip.ToString().ToUpper().Contains("IDIALOGDATAPART_00_01") == true) || (filezip.ToString().ToUpper().Contains("IDIALOGDATAPART") == true)
                                orderby filezip.ToString() descending
                                select filezip;
                        if (query1!=null && query1.Count() >0)
                        {
                            var file = zip.GetFile(query1.First().ToString());
                            LAstDateMiseAJour = file.LastModFileDateTime.ToUniversalTime();
                        }
                    }
                    Stream packagePath = File.Open(FileName, FileMode.Open);

                    using (Package package = Package.Open(packagePath, FileMode.Open))
                    {
                        try
                        {

                            //package = (ZipPackage)ZipPackage.Open(File.Open(FileName, FileMode.Open));
                            IEnumerable<PackagePart> parts = package.GetParts();

                            query = from part in parts
                                    where (part.Uri.ToString().ToUpper().Contains(fiche.NumFiche.ToUpper()) == true) || (part.Uri.ToString().ToUpper().Contains("IDIALOGDATAPART_00_01") == true) || (part.Uri.ToString().ToUpper().Contains("IDIALOGDATAPART") == true)
                                    orderby part.Uri.ToString() descending
                                    select part;
                        }
                        catch
                        {
                            fiche.ErrorCheck = (int)ErrorFiche.QueryError;
                            query = null;
                        }
                        if ((query != null) && (query.Count() > 0))
                        {
                            // Ouvrir la partie
                            
                            PackagePart file = query.First();
                            
                            XDocument doc = XDocument.Load(query.First().GetStream());
                           
                            fiche.ErrorCheck = (int)ErrorFiche.Exception;
                            fiche.RefMO = this.GetXMLValue("MORef", doc.Root);
                            fiche.RefMT = this.GetXMLValue("MTRef", doc.Root);
                            fiche.RefSIM = this.GetXMLValue("SIMRef", doc.Root);
                            fiche.VersionFiche = this.GetXMLValue("XMLFileVersion", doc.Root);
                            compilInfo = this.GetXMLValue("XMLFileGenerated", doc.Root);

                            Regex FindiDialogVersion = new Regex(regexVerIdialog);
                            MatchCollection match = FindiDialogVersion.Matches(compilInfo);
                            
                            if (match.Count == 1)
                            {
                                fiche.VerIDialog = match[0].Groups[1].Value;
                                fiche.IsCompiled = true;
                            }
                            else
                            {
                                fiche.VerIDialog = "Non définie";
                                fiche.IsCompiled = false;
                            }

                            Regex FindDateLastCompile = new Regex(regexDateLastCompil);
                            match = FindDateLastCompile.Matches(compilInfo);
                            if (match.Count == 1)
                            {
                                fiche.DateLastCompile = DateTime.Parse(match[0].Groups[1].Value); 
                            }
                            else
                            {
                                fiche.DateLastCompile = null;
                            }

                            if (!(((fiche.DateLastCompile <= LAstDateMiseAJour.AddSeconds(5)) && (fiche.DateLastCompile.Value.AddMinutes(5) > LAstDateMiseAJour))  ||
                                  ((fiche.DateLastCompile.Value.AddHours(12) <= LAstDateMiseAJour.AddSeconds(5)) && (fiche.DateLastCompile.Value.AddMinutes(5).AddHours(12) > LAstDateMiseAJour)) )  )
                            {

                                //fiche.IsCompiled = false;
                                //string nulfiche = fiche.NumFiche;
                            }

                            String tmp = this.GetXMLValue("OptionIR", doc.Root);
                            if (!String.IsNullOrEmpty(tmp))
                            {
                                if (tmp.Contains("0x"))
                                {
                                    fiche.OptionIR = Convert.ToInt32(tmp, 16);
                                }
                                else
                                {
                                    fiche.OptionIR = Convert.ToInt32(tmp, 10);
                                }
                            }
                            else
                            {
                                fiche.OptionIR = null;
                            }
                            tmp = this.GetXMLValue("OptionExploit", doc.Root);
                            if (!String.IsNullOrEmpty(tmp))
                            {
                                if (tmp.Contains("0x"))
                                {
                                    fiche.OptionExploit = Convert.ToInt32(tmp, 16);
                                }
                                else
                                {
                                    fiche.OptionExploit = Convert.ToInt32(tmp, 10);
                                }
                            }
                            else
                            {
                                fiche.OptionExploit = null;
                            }
                            tmp = this.GetXMLValue("Mode", doc.Root);
                            if (!String.IsNullOrEmpty(tmp))
                            {
                                if (tmp.Contains("0x"))
                                {
                                    fiche.PresenceIR = Convert.ToInt32(tmp, 16).ToString();
                                }
                                else
                                {
                                    fiche.PresenceIR = Convert.ToInt32(tmp, 10).ToString();
                                }
                            }
                            else
                            {
                                fiche.OptionExploit = null;
                            }
                            tmp = this.GetXMLValue("TypeFiche", doc.Root);
                            if (!String.IsNullOrEmpty(tmp))
                            {
                                if (tmp.Contains("0x"))
                                {
                                    fiche.TypeFiche = Convert.ToInt32(tmp, 16);
                                }
                                else
                                {
                                    fiche.TypeFiche = Convert.ToInt32(tmp, 10);
                                }
                            }
                            else
                            {
                                fiche.TypeFiche = null;
                            }
                            tmp = this.GetXMLValue("CRProfibus", doc.Root);
                            if (!String.IsNullOrEmpty(tmp))
                            {
                                if (tmp.Contains("0x"))
                                {
                                    fiche.AnyBus = Convert.ToSByte(tmp, 16);
                                }
                                else
                                {
                                    fiche.AnyBus = Convert.ToSByte(tmp, 10);
                                }
                                if (fiche.AnyBus < 0)
                                { fiche.AnyBus = 0; }
                            }
                            else
                            {
                                fiche.AnyBus = null;
                            }
                            Result = true;
                            tmp = this.GetXMLValue("ModeCouplage", doc.Root);
                            if (!String.IsNullOrEmpty(tmp))
                            {
                                if (tmp.Contains("0x"))
                                {
                                    fiche.TypeSynchro = Convert.ToInt32(tmp, 16);
                                }
                                else
                                {
                                    fiche.TypeSynchro = Convert.ToInt32(tmp, 10);
                                }
                            }
                            else
                            {
                                fiche.TypeSynchro = null;
                            }
                            fiche.ErrorCheck = (int)ErrorFiche.NoError;
                            fiche.DateCheck = DateTime.Now;

                            tmp = this.GetXMLValue("MT", "DebitMax", doc.Root);
                            if (!String.IsNullOrEmpty(tmp))
                            {
                                if (tmp.Contains("0x"))
                                {
                                    fiche.MTdebit = Convert.ToInt32(tmp, 16);
                                }
                                else
                                {
                                    fiche.MTdebit = Convert.ToInt32(tmp, 10);
                                }
                            }
                            else
                            {
                                fiche.MTdebit = null;
                            }
                            tmp = this.GetXMLValue("MO", "DebitMax", doc.Root);
                            if (!String.IsNullOrEmpty(tmp))
                            {
                                if (tmp.Contains("0x"))
                                {
                                    fiche.MOdebit = Convert.ToInt32(tmp, 16);
                                }
                                else
                                {
                                    fiche.MOdebit = Convert.ToInt32(tmp, 10);
                                }
                            }
                            else
                            {
                                fiche.MOdebit = null;
                            }
                            tmp = this.GetXMLValue("MT", "SubstituRadioRS485", doc.Root);
                            string tmp2 = this.GetXMLValue("MO", "SubstituRadioRS485", doc.Root);
                            if (!String.IsNullOrEmpty(tmp))
                            {
                                if (tmp.Contains("0x"))
                                {
                                    fiche.LiaisonFilaire = Convert.ToInt32(tmp, 16);
                                }
                                else
                                {
                                    fiche.LiaisonFilaire = Convert.ToInt32(tmp, 10);
                                }
                            }
                            if(!String.IsNullOrEmpty(tmp2))
                            {
                                if (fiche.LiaisonFilaire== null ||fiche.LiaisonFilaire == 0)
                                {
                                    if (tmp2.Contains("0x"))
                                    {
                                        fiche.LiaisonFilaire = Convert.ToInt32(tmp2, 16);
                                    }
                                    else
                                    {
                                        fiche.LiaisonFilaire = Convert.ToInt32(tmp2, 10);
                                    }
                                }
                            }
                            else
                            {
                                fiche.LiaisonFilaire = null;
                            }

                            // comptage des organes de commandes
                            XElement Root = doc.Root;
                            foreach (XElement refProduit in Root.Descendants("Bloc"))
                            {
                                string code = refProduit.Attribute("code").Value;
                                if (code.Equals("OrganeCommande"))
                                {
                                    foreach (XElement sosubloc in refProduit.Descendants("SousBloc"))
                                    {
                                        code = sosubloc.Attribute("code").Value;
                                        if (code.Equals("DicoOrganeMO"))
                                        {
                                            fiche.D30110 = null;
                                            fiche.D30120 = null;
                                            fiche.D30130 = null;
                                            fiche.D30140 = null;
                                            fiche.D30150 = null;
                                            fiche.D30330 = null;
                                            fiche.D30480 = null;
                                            fiche.D30400A = null;
                                            fiche.D30430A = null;
                                            fiche.D30440A = null;
                                            fiche.D30450A = null;
                                            fiche.D30460A = null;
                                            fiche.D30470A = null;
                                            fiche.D30840A = null;
                                            fiche.D30490A = null;

                                            foreach (XElement Rubrique in sosubloc.Descendants("Rubrique"))
                                            {
                                                code = Rubrique.Attribute("code").Value;
                                                if (code.Equals("configHardMO"))
                                                {
                                                    foreach (XElement variable in Rubrique.Descendants("Variable"))
                                                    {
                                                        string nomorgane = variable.Attribute("code").Value;
                                                        if (nomorgane.Equals("NomOrganeMo"))
                                                        {
                                                            string organe = variable.Attribute("valeur").Value;
                                                            if (organe.Equals("A1") || organe.Equals("A2") || organe.Equals("A3") || organe.Equals("A4") ||
                                                                organe.Equals("A5") || organe.Equals("A6") || organe.Equals("A7") || organe.Equals("A8") ||
                                                                organe.Equals("A9") || organe.Equals("A10") || organe.Equals("A11") || organe.Equals("A12"))
                                                            {
                                                                string typeorgane = variable.Attribute("description").Value;
                                                                CompteurFiche(fiche, typeorgane);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        else if ((query == null) || (query!=null && query.Count()==0))
                        {
                            fiche.RefMO = null;
                            fiche.RefMT = null;
                            fiche.RefSIM = null;
                            fiche.VersionFiche = null;
                            compilInfo = null;
                            fiche.AnyBus = null;
                            fiche.TypeFiche = null;
                            fiche.OptionExploit = null;
                            fiche.OptionIR = null;
                            fiche.IsCompiled = false;
                            fiche.VerIDialog = null;
                            fiche.DateLastCompile = null;
                            fiche.ErrorCheck = (int)ErrorFiche.QueryError2;
                            fiche.DateCheck = DateTime.Now;
                        }
                        else
                        {
                            MiseAjour = true;
                        }
                    }// end of using
                    packagePath.Close();
                }
                catch
                {

                    fiche.ErrorCheck = (int)ErrorFiche.OpenFile;
                    fiche.DateCheck = DateTime.Now;
                }
                //if (MiseAjour == true)
                // mise a jour des fiches
                if (false)
                {
                    string destFile = FileName + ".old_check";
                    System.IO.File.Copy(FileName, destFile, true);
                    Stream packagePathwrite = File.Open(FileName, FileMode.OpenOrCreate);

                    using (Package package = Package.Open(packagePathwrite, FileMode.OpenOrCreate))
                    {
                        IEnumerable<PackagePart> parts = package.GetParts();
                        query = (from part in parts
                                 where ((part.Uri.ToString().Contains(".xml") == true) || (part.Uri.ToString().Equals("/IDialogDataPart") == true))
                                 && part.Uri.ToString().Contains("[Content_Types].xml") == false
                                 && part.Uri.ToString().Contains("/References/") == false
                                 && part.Uri.ToString().Contains("/pictures/") == false
                                 && part.Uri.ToString().Contains("/Languages/") == false
                                 orderby part.Uri.ToString() descending
                                 select part);

                        if ((query != null) && (query.Count() > 0))
                        {
                            string filetocopy = query.First().Uri.ToString();
                            Regex regexFPS = new Regex("((FPS\\d{5}_\\d{2})|([CKGDBSFLM]{1}\\d{4}))\\.i[dD]{1}ialog$");
                            Match oldfilename = regexFPS.Match(FileName);
                            //
                            if ((oldfilename.Groups[1].Value.ToString() != null) && (oldfilename.Groups[1].Value.ToString() != ""))
                            {
                                // Add the Document part to the Package
                                List<PackagePart> filetodelete = query.ToList();


                                Uri partUriDocument = PackUriHelper.CreatePartUri(new Uri(oldfilename.Groups[1].Value.ToString().ToUpper() + "_01" + ".xml", UriKind.Relative));
                                Uri partUriDocumentcopy = PackUriHelper.CreatePartUri(new Uri(filetocopy, UriKind.Relative));
                                PackagePart packagePartDocument = package.CreatePart(partUriDocument, System.Net.Mime.MediaTypeNames.Text.Xml);
                                PackagePart packagePartDocumentcopy = package.GetPart(partUriDocumentcopy);

                                CopyStream(packagePartDocumentcopy.GetStream(), packagePartDocument.GetStream());
                                //if (packagePartDocumentcopy.GetStream().Equals(packagePartDocument.GetStream()))
                                //{
                                foreach (PackagePart uri in filetodelete)
                                {
                                    String deluristring = uri.Uri.ToString();
                                    Uri deluri = PackUriHelper.CreatePartUri(new Uri(deluristring, UriKind.Relative));
                                    package.DeletePart(deluri);
                                }
                            }
                            //}
                        }
                    }
                    packagePathwrite.Close();
                }


                DateTime? tmpdate = fi.LastWriteTime;
                DateTime date1 = new DateTime(tmpdate.Value.Year, tmpdate.Value.Month, tmpdate.Value.Day, tmpdate.Value.Hour, tmpdate.Value.Minute, tmpdate.Value.Second);
                fiche.DateMiseADispo = date1;
            }

            return true;
        } // endMethod: MAJFicheData

        private void CompteurFiche(Fiches fiche, string typeorgane)
        {
            if (typeorgane.Contains("D30110"))
            {
                if (fiche.D30110 == null)
                {
                    fiche.D30110 = 1;
                }
                else
                {
                    short tmpa = (short)fiche.D30110;
                    tmpa++;
                    fiche.D30110 = tmpa;
                }
            }
            else if (typeorgane.Contains("D30120"))
            {
                if (fiche.D30120 == null)
                {
                    fiche.D30120 = 1;
                }
                else
                {
                    short tmpa = (short)fiche.D30120;
                    tmpa++;
                    fiche.D30120 = tmpa;
                }
            }
            else if (typeorgane.Contains("D30130"))
            {
                if (fiche.D30130 == null)
                {
                    fiche.D30130 = 1;
                }
                else
                {
                    short tmpa = (short)fiche.D30130;
                    tmpa++;
                    fiche.D30130 = tmpa;
                }
            }
            else if (typeorgane.Contains("D30140"))
            {
                if (fiche.D30140 == null)
                {
                    fiche.D30140 = 1;
                }
                else
                {
                    short tmpa = (short)fiche.D30140;
                    tmpa++;
                    fiche.D30140 = tmpa;
                }
            }
            else if (typeorgane.Contains("D30150"))
            {
                if (fiche.D30150 == null)
                {
                    fiche.D30150 = 1;
                }
                else
                {
                    short tmpa = (short)fiche.D30150;
                    tmpa++;
                    fiche.D30150 = tmpa;
                }
            }
            else if (typeorgane.Contains("D30330"))
            {
                if (fiche.D30330 == null)
                {
                    fiche.D30330 = 1;
                }
                else
                {
                    short tmpa = (short)fiche.D30330;
                    tmpa++;
                    fiche.D30330 = tmpa;
                }
            }
            else if (typeorgane.Contains("D30480"))
            {
                if (fiche.D30480 == null)
                {
                    fiche.D30480 = 1;
                }
                else
                {
                    short tmpa = (short)fiche.D30480;
                    tmpa++;
                    fiche.D30480 = tmpa;
                }
            }
            else if (typeorgane.Contains("D30400A"))
            {
                if (fiche.D30400A == null)
                {
                    fiche.D30400A = 1;
                }
                else
                {
                    short tmpa = (short)fiche.D30400A;
                    tmpa++;
                    fiche.D30400A = tmpa;
                }
            }
            else if (typeorgane.Contains("D30430A"))
            {
                if (fiche.D30430A == null)
                {
                    fiche.D30430A = 1;
                }
                else
                {
                    short tmpa = (short)fiche.D30430A;
                    tmpa++;
                    fiche.D30430A = tmpa;
                }
            }
            else if (typeorgane.Contains("D30440A"))
            {
                if (fiche.D30440A == null)
                {
                    fiche.D30440A = 1;
                }
                else
                {
                    short tmpa = (short)fiche.D30440A;
                    tmpa++;
                    fiche.D30440A = tmpa;
                }
            }
            else if (typeorgane.Contains("D30450A"))
            {
                if (fiche.D30450A == null)
                {
                    fiche.D30450A = 1;
                }
                else
                {
                    short tmpa = (short)fiche.D30450A;
                    tmpa++;
                    fiche.D30450A = tmpa;
                }
            }
            else if (typeorgane.Contains("D30460A"))
            {
                if (fiche.D30460A == null)
                {
                    fiche.D30460A = 1;
                }
                else
                {
                    short tmpa = (short)fiche.D30460A;
                    tmpa++;
                    fiche.D30460A = tmpa;
                }
            }
            else if (typeorgane.Contains("D30470A"))
            {
                if (fiche.D30470A == null)
                {
                    fiche.D30470A = 1;
                }
                else
                {
                    short tmpa = (short)fiche.D30470A;
                    tmpa++;
                    fiche.D30470A = tmpa;
                }
            }
            else if (typeorgane.Contains("D30840A"))
            {
                if (fiche.D30840A == null)
                {
                    fiche.D30840A = 1;
                }
                else
                {
                    short tmpa = (short)fiche.D30840A;
                    tmpa++;
                    fiche.D30840A = tmpa;
                }
            }
            else if (typeorgane.Contains("D30490A"))
            {
                if (fiche.D30490A == null)
                {
                    fiche.D30490A = 1;
                }
                else
                {
                    short tmpa = (short)fiche.D30490A;
                    tmpa++;
                    fiche.D30490A = tmpa;
                }
            }
            else if (typeorgane.Contains("D30220A"))
            {
                if (fiche.D30220A == null)
                {
                    fiche.D30220A = 1;
                }
                else
                {
                    short tmpa = (short)fiche.D30220A;
                    tmpa++;
                    fiche.D30220A = tmpa;
                }
            }
        }
        //  --------------------------- CopyStream ---------------------------
        /// <summary>
        ///   Copies data from a source stream to a target stream.</summary>
        /// <param name="source">
        ///   The source stream to copy from.</param>
        /// <param name="target">
        ///   The destination stream to copy to.</param>
        private static void CopyStream(Stream source, Stream target)
        {
            const int bufSize = 0x1000;
            byte[] buf = new byte[bufSize];
            int bytesRead = 0;
            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
                target.Write(buf, 0, bytesRead);
        }// end:CopyStream()

        /// <summary>
        /// Obtenir le valeur
        /// </summary>
        private String GetXMLValue(String variableCode, XElement root)
        {
            String Result = "";

            //try
            //{
            IEnumerable<XElement> query = from variable in root.Descendants("Variable")
                                          where variable.Attribute("code") != null && variable.Attribute("code").Value == variableCode
                                          select variable;

            if (query.Count() > 0)
            {
                Result = query.First().Attribute("valeur").Value;
            }
            //}
            //catch (Exception e)
            //{
            //    Result = "Error!";
            //}

            return Result;
        } // endMethod: GetXMLValue


        /// 
        private String GetXMLValue(String variablesection, String variableCode, XElement root)
        {
            String Result = "";

            //try
            //{
            IEnumerable<XElement> query1 = from variable in root.Descendants("SousBloc")
                                           where variable.Attribute("code") != null && variable.Attribute("code").Value == variablesection
                                           select variable;
            if (query1.Count() > 0)
            {
                XElement inter = query1.First();
            }
            IEnumerable<XElement> query = from variable in root.Descendants("Variable")
                                          where variable.Attribute("code") != null && variable.Attribute("code").Value == variableCode
                                          select variable;

            if (query.Count() > 0)
            {
                Result = query.First().Attribute("valeur").Value;
            }


            return Result;
        }
        public String CalculateHash(XElement element)
        {
            String Result = "";
            String StrElement = element.ToString();
            Byte[] computeHash;

            MD5 hashage = MD5.Create();

            computeHash = hashage.ComputeHash(Encoding.UTF8.GetBytes(StrElement));

            for (int i = 0; i < computeHash.Length; i++)
            {
                Result += computeHash[i].ToString("X2");
            }

            return Result;
        } // endMethod: CalculateHash
    }

}
