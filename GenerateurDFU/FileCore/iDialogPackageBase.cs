using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Specialized;
using System.Configuration;
using Messenger = GalaSoft.MvvmLight.Messaging.Messenger;
using System.Globalization;

namespace JAY.FileCore
{
    /// <summary>
    /// Cette classe de base permet de fabriquer des packages facilement
    /// ces package enferme des données xml par exemple
    /// </summary>
    public class iDialogPackageBase : FilePackage
    {
        #region Constantes

        // constantes
        private String XML_HEADER = String.Format("<?xml version=\"1.0\" encoding=\"{0}\"?>", Constantes.XML_ENCODING);
        public const String XML_ENCODING = Constantes.XML_ENCODING;
        private const String USER_FILENAME = "/UserFileName.txt";
        public const String FILE_INFO = "/FileInfo.xml";
        public const String REF_FILE = "/References/RefFile.xml";
        public const String REF_FILE_DEFAULT = "FichierModeleXmlTechnique";

        #endregion

        // Variables
        #region Variables

        private String _extension;
        private String _path;
        private ObservableCollection<Uri> _xmlPartName;
        private ObservableCollection<Int32> _versions;
        private Int32 _currentVersion;
        private Uri _txtUserFileNamePartUri;
        private Uri _xmlFileInfoPartUri;
        private String _userTxt;
        private ObservableCollection<String> _packageImages;
        private ObservableCollection<String> _packageLangueFile;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le fichier de langue du package
        /// </summary>
        public ObservableCollection<String> PackageLangueFile
        {
            get
            {
                if (this._packageLangueFile == null)
                {
                    this._packageLangueFile = this.GetPackageLangueFiles();
                }
                return this._packageLangueFile;
            }
        } // endProperty: PackageLangueFile

        /// <summary>
        /// La collection d'images embarquées dans le fichier iDialog
        /// </summary>
        public ObservableCollection<String> PackageImages
        {
            get
            {
                if (this._packageImages == null)
                {
                    this._packageImages = this.GetPackageImages();
                }
                return this._packageImages;
            }
        } // endProperty: PackageImages

        /// <summary>
        /// La liste des versions du fichier de configuration
        /// </summary>
        public ObservableCollection<Int32> Versions
        {
            get
            {
                return this._versions;
            }
            private set
            {
                this._versions = value;
            }
        } // endProperty: Versions

        /// <summary>
        /// Obtenir ou définir le nom utilisateur du fichier
        /// </summary>
        public String UserFileName
        {
            get
            {
                String Result = "";
                // Si le package n'est pas ouvert, l'ouvrir
                this.OpenPackage();
                PackagePart ZPP = this.CurrentPackage.GetPart(this.TXTUserFileNamePartUri);
                Stream stream = ZPP.GetStream();
                using (StreamReader SR = new StreamReader(stream, Encoding.GetEncoding(XML_ENCODING)))
                {
                    Result = SR.ReadToEnd();
                }
                // Fermer le package
                this.ClosePackage();

                return Result;
            }
            set
            {
                String Value = value;
                PackagePart ZPP;

                this.OpenPackage();
                try
                {
                    // La partie existe-t-elle déjà?
                    ZPP = this.CurrentPackage.GetPart(this.TXTUserFileNamePartUri);
                }
                catch (Exception)
                {
                    // Créer le fichier
                    ZPP = this.CurrentPackage.CreatePart(this.TXTUserFileNamePartUri, System.Net.Mime.MediaTypeNames.Text.Plain);
                }
                
                Stream stream;
                stream = ZPP.GetStream();

                // enregistrer la valeur
                using (StreamWriter SW = new StreamWriter(stream, Encoding.GetEncoding(XML_ENCODING)))
                {
                    SW.Write(Value);
                    SW.Flush();
                }
                this.ClosePackage();
            }
        } // endProperty: UserFileName

        /// <summary>
        /// Le lien vers la partie 
        /// </summary>
        public Uri TXTUserFileNamePartUri
        {
            get
            {
                return this._txtUserFileNamePartUri;
            }
        } // endProperty: XMLFileNamePart

        /// <summary>
        /// La partie contenant les informations du fichier
        /// </summary>
        public Uri XMLFileInfoPartUri
        {
            get
            {
                return this._xmlFileInfoPartUri;
            }
            set
            {
                this._xmlFileInfoPartUri = value;
            }
        } // endProperty: XMLFileInfoPart

        /// <summary>
        /// La liste des parties XML du ZipPackage
        /// </summary>
        public ObservableCollection<Uri> XMLParts
        {
            get
            {
                return this._xmlPartName;
            }
            private set
            {
                this._xmlPartName = value;
            }
        } // endProperty: XMLParts

        /// <summary>
        /// Le nom du fichier (package) avec l'extension
        /// </summary>
        public new String PackageFileName
        {
            get
            {
                return base.PackageFileName;
            }
            private set
            {
                base.PackageFileName = value;
                this._extension = System.IO.Path.GetExtension( value );
                this._path = System.IO.Path.GetDirectoryName ( value ) + @"\";
            }
        } // endProperty: FileName

        /// <summary>
        /// L'extension du fichier
        /// </summary>
        public String Extension
        {
            get
            {
                return this._extension;
            }
        } // endProperty: Extension

        /// <summary>
        /// Le chemin du fichier
        /// </summary>
        public String Path
        {
            get
            {
                return this._path;
            }
        } // endProperty: Path

        /// <summary>
        /// La version en cours d'utilisation
        /// </summary>
        public Int32 CurrentVersion
        {
            get
            {
                return this._currentVersion;
            }
            set
            {
                this._currentVersion = value;
                // Selectionner le fichier en cours d'utilisation
            }
        } // endProperty: CurrentVersion

        /// <summary>
        /// Le texte de l'utilisateur
        /// </summary>
        public String UserText
        {
            get
            {
                return this._userTxt;
            }
            private set
            {
                this._userTxt = value;
            }
        } // endProperty: UserText

        #endregion

        // Constructeur
        #region Constructeur

        public iDialogPackageBase ( )
        {
            this._txtUserFileNamePartUri = new Uri(iDialogPackageBase.USER_FILENAME, UriKind.Relative);
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Créer un package iDialog
        /// </summary>
        public void CreateIDialogPackage(String iDialogFileName, String iDialogRefFileName, String UserFileName, String FCXml, ObservableCollection<String> langFiles )
        {
            // Ouvrir le package
            Package pack;
            String Doc;

            pack = ZipPackage.Open(iDialogFileName, FileMode.Create);
            if (pack != null)
            {
                this.IsOpen = true;
            }

            this.CurrentPackage = (ZipPackage)pack;
            this.PackageFileName = iDialogFileName;

            // Ajouter la partie FileName
            this.UserFileName = UserFileName;

            // Nettoyer le nom du fichier
            String ficheName = System.IO.Path.GetFileNameWithoutExtension(iDialogFileName);
            ficheName = ficheName.Replace(' ', '_');
            ficheName = ficheName.Replace('.', '_');
            ficheName = ficheName.ToLowerInvariant();
            ficheName = iDialogPackageBase.RemoveDiacritics(ficheName);
            ficheName = "/" + ficheName;

            ficheName = ficheName + "_00.xml";

            // Ajouter la fiche de configuration
            this.AddXMLPart(new Uri(ficheName, UriKind.Relative), FCXml);

            // Ajouter la fiche d'information
            //if (InfoFileName != null)
            //{
            //    if (InfoFileName != "")
            //    {
            //        FileName = "/" + System.IO.Path.GetFileName(InfoFileName);

            //        using (StreamReader SR = new StreamReader(File.Open(InfoFileName, FileMode.Open), Encoding.GetEncoding(XML_ENCODING)))
            //        {
            //            Doc = SR.ReadToEnd();
            //        }
            //        this.AddXMLPart(new Uri(FileName, UriKind.Relative), Doc);
            //    }
            //}

            // Ajouter le fiche de référence
            if (File.Exists(iDialogRefFileName))
            {
                using (StreamReader SR = new StreamReader(File.OpenRead(iDialogRefFileName), Encoding.GetEncoding(XML_ENCODING)))
                {
                    Doc = SR.ReadToEnd();
                }

                this.AddXMLPart(new Uri(REF_FILE, UriKind.Relative), Doc); 
            }

            // Ajouter les fichiers de langues
            if (langFiles != null && langFiles.Count > 0)
            {
                foreach (String FileName in langFiles)
                {
                    using (StreamReader SR = new StreamReader(File.OpenRead(FileName), Encoding.GetEncoding(XML_ENCODING)))
                    {
                        Doc = SR.ReadToEnd();
                    }
                    String uripath = "/Languages/" + System.IO.Path.GetFileName(FileName);
                    this.AddXMLPart(new Uri(uripath, UriKind.Relative), Doc);
                }
            }
        } // endMethod: CreateIDialogPackage

        /// <summary>
        /// Créer un fichier iDialog
        /// </summary>
        public void CreateIDialogPackage ( String iDialogFileName, String FCFileName, String UserFileName, ObservableCollection<String> langFiles )
        {
            // Ajouter la fiche de configuration
            String FileName = "/" + System.IO.Path.GetFileName(FCFileName);
            String Doc;
            using (StreamReader SR = new StreamReader(File.OpenRead(FCFileName), Encoding.GetEncoding(XML_ENCODING)))
            {
                Doc = SR.ReadToEnd();
            }

            NameValueCollection nvc = (NameValueCollection)ConfigurationManager.GetSection("PEGASE");

            if (nvc != null)
            {
                if (!string.IsNullOrEmpty(nvc[REF_FILE_DEFAULT]))
                {
                    String RefFile = nvc[REF_FILE_DEFAULT].ToString();
                    this.CreateIDialogPackage(iDialogFileName, RefFile, UserFileName, Doc, langFiles );
                }
            }
        } // endMethod: CreateIDialogPackage

        /// <summary>
        /// Enlever les caractères accentués d'une chaine de caractères, les espaces sont remplacés par des underscore
        /// </summary>
        /// <param name="stIn"></param>
        /// <returns></returns>
        public static String RemoveDiacritics(string stIn)
        {
            String Result;
            String stFormD = stIn.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            Result = (sb.ToString().Normalize(NormalizationForm.FormC));
            Result = Result.Replace(' ', '_');

            return Result;
        }

        /// <summary>
        /// Réouvrir le package en cours d'utilisation
        /// </summary>
        ///         
        private readonly object balanceLock = new object();
        public Boolean OpenPackage( )
        {
            Boolean Result = false;
            // Boucler tant que la boucle = 1. 
            // Résultat si boucle = 0 -> OK.
            //          si boucle = 2 -> Quitter sans sauvegarder (porte)
            Int32 Boucle = 1;
            lock (balanceLock)
            {
                if (!this.IsOpen)
                {
                    while (Boucle == 1)
                    {
                        if (File.Exists(this.PackageFileName))
                        {
                            try
                            {
                                this.CurrentPackage = (ZipPackage)ZipPackage.Open(this.PackageFileName, FileMode.Open);
                            }
                            catch (Exception et)
                            {
                                int toto = 1;
                            }
                            Boucle = 0;
                        }
                        else
                        {
                            String Message = "Error retrieving file! If you use extractible drive, please reconnect it then choose YES. If you choose NO, all modifications will be erased.";
                            if (System.Windows.MessageBox.Show(Message, "", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Exclamation) != System.Windows.MessageBoxResult.Yes)
                            {
                                Boucle = 2;
                            }
                        }
                    }

                    if (Boucle == 2)
                    {
                        // Envoyer le message quitter sans sauvegarder
                        Messenger.Default.Send<CommandMessage>(new CommandMessage(this, PegaseCore.Commands.CMD_MENU_DECONNECT_WITHOUTSAVE));
                        return Result;
                    }
                    this.IsOpen = true;
                }
            }
            Result = true;
            return Result;
        } // endMethod: OpenPackage

        /// <summary>
        /// Ouvrir le package désigné par FileName
        /// </summary>
        protected void OpenIDialogFile ( String FileName, FileMode FileMode )
        {
            this.PackageFileName = FileName;

            try
            {
                this.CurrentPackage = (ZipPackage)ZipPackage.Open(FileName, FileMode);

                this.IsOpen = true;

                if (FileMode != System.IO.FileMode.Create && FileMode != System.IO.FileMode.CreateNew)
                {
                    // si le fichier est en mode ouverture
                    // enumérer toute les fiches de configuration présente
                    this.EnumerateConfigPart();
                    // mettre à jour la liste des versions
                    this.Versions = new ObservableCollection<Int32>();
                    foreach (var item in this.XMLParts)
                    {
                        String partName = item.OriginalString;
                        Int32 numVersionS;
                        numVersionS = iDialogPackageBase.GetVersion(partName);
                        this.Versions.Add(numVersionS);
                    }

                    // ordonner les versions
                    var OrderVersions = from ver in this.Versions
                                        orderby ver descending
                                        select ver;

                    // sélectionner par défaut la dernière version

                    if (OrderVersions.Count() > 0)
                    {
                        this.CurrentVersion = Convert.ToInt32(OrderVersions.First());
                    }
                }
                this.UserText = this.GetUserFileContent();
                // Fonctionner en mode 'déconnecter'. Le fichier est réouvert chaque fois qu'une transaction doit être effectuée
                // Justification : lors d'un plantage, le fichier xml en cours d'utilisation plante
                this.ClosePackage();
            }
            catch (Exception e)
            {
                this.ClosePackage();
                System.Windows.MessageBox.Show(e.Message);
            }
        } // endMethod: OpenPackage

        /// <summary>
        /// Ouvrir le package désigné par FileName
        /// </summary>
        public static new iDialogPackageBase OpenPackage ( String FileName, FileMode FileMode )
        {
            iDialogPackageBase Result = new iDialogPackageBase();
            Result.OpenIDialogFile(FileName, FileMode);

            return Result;
        } // endMethod: OpenPackage
        
        /// <summary>
        /// Enumérer les parties XML du package
        /// </summary>
        private void EnumerateConfigPart ( )
        {
            ObservableCollection<Uri> parts = new ObservableCollection<Uri>();
            this._xmlPartName = new ObservableCollection<Uri> ( );

            var Packages = this.CurrentPackage.GetParts( );
            foreach ( PackagePart part in Packages)
            {
                if ( part.ContentType == System.Net.Mime.MediaTypeNames.Text.Xml )
                {
                    if (part.Uri.OriginalString != iDialogPackageBase.FILE_INFO )
                    {
                        if (part.Uri.OriginalString != iDialogPackageBase.REF_FILE)
                        {
                            if (part.Uri.OriginalString != iDialogPackageBase.USER_FILENAME)
	                        {
                                if (!part.Uri.OriginalString.Contains("Languages") && !part.Uri.OriginalString.Contains("ZConfig"))
                                {
                                    parts.Add(part.Uri); 
                                } 
	                        } 
                        }
                    }
                }
            }

            // ordonner les parties
            var query = from row in parts
                            //where (row.OriginalString.Length > 16)
                            where (!row.OriginalString.Equals("IDialogDataPart"))
                        orderby row.OriginalString
                        select row;

            if (query.Count() > 0)
            {
                foreach (var item in query)
                {
                    this._xmlPartName.Add(item);
                } 
            }
            else
            {
              if ((parts.Count() > 0) && (parts[0].OriginalString=="/IDialogDataPart") )
              {
                  this._xmlPartName.Add(parts[0]);
              }
            }
        } // endMethod: EnumerateXMLPart
        
        /// <summary>
        /// Ajouter une partie Xml à l'Uri spécifiée
        /// </summary>
        public Boolean AddXMLPart ( Uri FileUri, XmlDocument Doc )
        {
            Boolean Result;

            Result = this.AddXMLPart(FileUri, Doc.InnerXml);

            if (!Result)
            {
                System.Windows.MessageBox.Show("Fichier non sauvegardé, impossible d'ajouter la fiche dans le package");
            }

            return Result;
        } // endMethod: AddXMLPart
        
        /// <summary>
        /// Acquérir la liste des images comprises dans le package
        /// </summary>
        public ObservableCollection<String> GetPackageImages ( )
        {
            ObservableCollection<String> Result = new ObservableCollection<String>();

            //if (this.CurrentPackage != null)
            //{
                if (!this.IsOpen)
                {
                    this.OpenPackage();
                }

                foreach (var part in this.CurrentPackage.GetParts())
                {
                    if (part.Uri.OriginalString.Contains("pictures"))
                    {
                        Result.Add(part.Uri.OriginalString);
                    }
                }

                this.ClosePackage();
            //}

            return Result;
        } // endMethod: GetPackageImages
        
        /// <summary>
        /// Acquérir la liste des images vectoriel .xaml embarquées dans le package
        /// </summary>
        public ObservableCollection<String> GetXamlImages ( )
        {
            ObservableCollection<String> Result = new ObservableCollection<String>();

            if (this.CurrentPackage != null)
            {
                if (!this.IsOpen)
                {
                    this.OpenPackage();
                }

                foreach (var part in this.CurrentPackage.GetParts())
                {
                    if (part.Uri.OriginalString.ToUpper().Contains("XAML"))
                    {
                        Result.Add(part.Uri.OriginalString);
                    }
                }
                
            }

            return Result;
        } // endMethod: GetXamlImages

        /// <summary>
        /// Acquérir la liste des fichiers de langue inclus dans le package
        /// </summary>
        public ObservableCollection<String> GetPackageLangueFiles()
        {
            ObservableCollection<String> Result = new ObservableCollection<String>();
            if (this.CurrentPackage != null)
            {
                if (!this.IsOpen)
                {
                    this.OpenPackage();
                }
                foreach (var part in this.CurrentPackage.GetParts())
                {
                    if (part.Uri.OriginalString.Contains("Language"))
                    {
                        Result.Add(part.Uri.OriginalString);
                    }
                }
                this.ClosePackage();
            }
            return Result;
        } // endMethod: GetPackageLangueFiles

        /// <summary>
        /// Ajouter une partie Xml à l'Uri spécifiée
        /// </summary>
        public Boolean AddXMLPart(Uri FileUri, String Doc)
        {
            Boolean Result;
            ZipPackagePart ZPP;
            Stream stream;

            if (!Doc.Contains("<?xml version"))
            {
                Doc = XML_HEADER + "\r\n" + Doc;
            }
            try
            {
                // ouvrir le package
                if (!this.IsOpen)
                {
                    this.CurrentPackage = (ZipPackage)ZipPackage.Open(this.PackageFileName, FileMode.Open);
                    this.IsOpen = true;
                }
                ZPP = (ZipPackagePart)this.CurrentPackage.CreatePart(FileUri, System.Net.Mime.MediaTypeNames.Text.Xml, CompressionOption.Normal);
                stream = ZPP.GetStream();

                using (StreamWriter ST = new StreamWriter(stream, Encoding.GetEncoding(XML_ENCODING)))
                {
                    ST.Write(Doc);
                    ST.Flush();
                }
                this.CurrentPackage.Flush();

                Result = true;
            }
            catch
            {
                Result = false;
            }
            this.ClosePackage();
            return Result;
        } // endMethod: AddXMLPart

        /// <summary>
        /// Ouvrir le document XML spécifié
        /// </summary>
        private XmlDocument GetConfigFilePart ( String FileName )
        {
            XmlDocument Result = new XmlDocument();
            Uri UriPart = null;
            PackagePart ZPP;
            Stream stream;

            // 1 - rechercher l'uri dans la table
            foreach ( Uri uri in this.XMLParts )
            {
                if ( uri.OriginalString == FileName )
                {
                    UriPart = uri;
                    break;
                }
            }
            
            // 2 - Ouvrir la parti et le document XML
            if ( UriPart != null )
            {
                ZPP = this.CurrentPackage.GetPart ( UriPart );
                stream = ZPP.GetStream ( );
                Result.Load ( stream );
            }

            return Result;
        } // endMethod: GetXMLPart

        /// <summary>
        /// Trouver l'Uri de la partie FileInfo
        /// </summary>
        private void GetFileInfoPart ( )
        {
            var Query = from uri in this.XMLParts
                        where uri.OriginalString == iDialogPackage.FILE_INFO
                        select uri;

            this._xmlFileInfoPartUri = Query.FirstOrDefault();
        } // endMethod: GetFileInfoPart
        
        /// <summary>
        /// Acquérir le flux pour le fichier de référence
        /// </summary>
        public Stream GetRefFileStream ( )
        {
            Stream Result;
            Uri uri;

            // Si l'archive n'est pas ouverte, le faire
            if (!this.IsOpen)
            {
                this.CurrentPackage = (ZipPackage)ZipPackage.Open(this.PackageFileName, FileMode.Open);
                this.IsOpen = true;
            }

            uri = new Uri(REF_FILE, UriKind.Relative);
            PackagePart part = this.CurrentPackage.GetPart(uri);
            Result = part.GetStream();

            return Result;
        } // endMethod: GetRefFileStream
        
        /// <summary>
        /// Acquérir la version du fichier
        /// </summary>
        private static Int32 GetVersion ( String FileName )
        {
            Int32 Result;

            try
            {
                Int32 Pos = FileName.LastIndexOf('_');
                FileName = System.IO.Path.GetFileNameWithoutExtension(FileName);
                String StrVer = FileName.Substring(Pos);
                Result = Convert.ToInt32(StrVer);
            }
            catch
            {
                Result = 0;
            }
            
            return Result;
        } // endMethod: GetVersion

        /// <summary>
        /// Retourner le numero de la dernière version dans le fichier
        /// </summary>
        public Int32 GetLastVersionNumber ( )
        {
            Int32 Result = 0;

            var QueryOrder = from file in this.XMLParts
                             orderby file.OriginalString descending
                             select file;

            if (QueryOrder.Count() > 0)
            {
                String fileName = QueryOrder.First().OriginalString;
                Result = iDialogPackageBase.GetVersion(fileName);
            }

            return Result;
        } // endMethod: GetLastVersionNumber

        /// <summary>
        /// Retourner le nom du fichier en fonction de la version transmise
        /// </summary>
        public Uri GetFileName ( Int32 Version )
        {
            Uri Result = null;

            foreach (var file in XMLParts)
            {
                Int32 fileVer = iDialogPackageBase.GetVersion(file.OriginalString);
                if (fileVer == Version)
                {
                    Result = file;
                    break;
                }
            }
            return Result;
        } // endMethod: GetFileName

        /// <summary>
        /// Acquérir un flux vers la fiche de configuration en cours d'utilisation
        /// </summary>
        public Stream GetCurrentVersionFileStream( )
        {
            Stream Result = null;
            Uri PartUri = null;
            if (!this.IsOpen)
            {
                if (!this.OpenPackage())
                {
                    return Result;
                }
                this.IsOpen = true;
            }
            PartUri = this.GetFileName(this.CurrentVersion);
            if (PartUri != null)
            {
                PackagePart part = this.CurrentPackage.GetPart(PartUri);
                if (part != null)
                {
                    Result = part.GetStream();
                } 
            }
            return Result;
        } // endMethod: GetCurrentVersionFileStream
        
        /// <summary>
        /// Acquérir la version en cours d'utilisation du fichier XML
        /// </summary>
        public String GetCurrentVersionXML()
        {
            String Result = "";

            if (!this.IsOpen)
            {
                if (!this.OpenPackage())
                {
                    return Result;
                }
                this.IsOpen = true;
            }

            XDocument XDoc = XDocument.Load(this.GetCurrentVersionFileStream());
            if (XDoc != null)
            {
                Result = XDoc.ToString();
            }
            // fermer le package
            this.ClosePackage();
            
            return Result;
        } // endMethod: GetCurrentVersionXML
        
        /// <summary>
        /// Acquérir le contenu du fichier de référence utilisateur
        /// </summary>
        public String GetUserFileContent ( )
        {
            String Result = "";

            if (this.CurrentPackage != null)
            {

                if (!this.IsOpen)
                {
                    this.OpenPackage();
                }

                PackagePart part = this.CurrentPackage.GetPart(this.TXTUserFileNamePartUri);
                Stream streamPart = part.GetStream();

                using (StreamReader SR = new StreamReader(streamPart))
                {
                    Result = SR.ReadToEnd();
                }

                this.ClosePackage();
            }
            
            return Result;
        } // endMethod: GetUserFileContent

        /// <summary>
        /// Supprimer la version en cours du pack
        /// </summary>
        public void DeleteCurrentVersion( )
        {
            this.DeletePackagePart(this.GetFileName(this.CurrentVersion));
        } // endMethod: DeleteCurrentVersion
        
        /// <summary>
        /// Supprimer la partie désigner par son nom
        /// </summary>
        public void DeletePackagePart ( Uri UriPart )
        {
            Boolean IsNotOpen = false;

            // ouvrir le package
            if (!this.IsOpen)
            {
                this.CurrentPackage = (ZipPackage)ZipPackage.Open(this.PackageFileName, FileMode.Open);
                this.IsOpen = true;
                IsNotOpen = true;
            }
            // supprimer la version en cours
            try
            {
                this.CurrentPackage.DeletePart(UriPart);
                this.CurrentPackage.Flush();
            }
            catch
            {
            }

            if (IsNotOpen)
            {
                // fermer le package
                base.ClosePackage();
            }
        } // endMethod: DeletePackage
        
        /// <summary>
        /// Surcharge de la partie GetStream de la classe de base
        /// </summary>
        override public Stream GetPartStream ( Uri PartUri )
        {
            Boolean IsNotOpen = false;
            Stream Result;

            // ouvrir le package
            if (!this.IsOpen)
            {
                this.CurrentPackage = (ZipPackage)ZipPackage.Open(this.PackageFileName, FileMode.Open);
                if (CurrentPackage == null)
                {
                }
                this.IsOpen = true;
                IsNotOpen = true;
            }
            if (CurrentPackage == null)
            {
            }
            Result = base.GetPartStream(PartUri);
            
            // Chercher la partie spécifiée
            

            if (IsNotOpen)
            {
                // fermer le package
                base.ClosePackage();
            }

            return Result;
        } // endMethod: GetPartStream

        /// <summary>
        /// Sauvegarder la version 
        /// </summary>
        public Boolean SaveCurrentVersion ( String XML )
        {
            Boolean Result = false;

            // ouvrir le package
            if (!this.IsOpen)
            {
                Result = this.OpenPackage();
                if (!Result)
                {
                    return Result;
                }
            }
            // ouvrir la partie
            ZipPackagePart part = (ZipPackagePart)this.CurrentPackage.GetPart(this.GetFileName(this.CurrentVersion));
            if (part != null)
            {
                this.DeleteCurrentVersion(); 
            }
            this.AddXMLPart(this.GetFileName(this.CurrentVersion), XML);

            // fermer le package
            this.ClosePackage();

            Result = true;
            return Result;
        } // endMethod: SaveCurrentVersion
        
        /// <summary>
        /// Ajouter une nouvelle version
        /// </summary>
        public Boolean SaveNewVersion ( String XML )
        {
            Int32 NumVersion = this.CurrentVersion + 1;
            Uri NewVersion;
            String FileName;
            Int32 Pos;
            Boolean Result = false;

            // ouvrir le package
            if (!this.IsOpen)
            {
                Result = this.OpenPackage();
                if (!Result)
                {
                    return Result;
                }
            }

            // sauvegarder le fichier
            FileName = this.XMLParts[XMLParts.Count-1].OriginalString;
            FileName = System.IO.Path.GetFileNameWithoutExtension(FileName);
            Pos = FileName.LastIndexOf('_');
            if (Pos > 0)
            {
                FileName = FileName.Substring(0, Pos);
            }
            FileName = String.Format("/{0}_{1:00}.xml", FileName, NumVersion);
            NewVersion = new Uri(FileName, UriKind.Relative);

            this.XMLParts.Add(NewVersion);
            this.AddXMLPart(NewVersion, XML);

            // fermer le package
            this.ClosePackage();

            Result = true;
            return Result;
        } // endMethod: AddNewVersion

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: XMLPackage
}
