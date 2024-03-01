using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Windows;

namespace JAY.FileCore
{
    /// <summary>
    /// Fournit des méthodes et propriétés permettant de manipuler un package
    /// de base, de stocker des informations...
    /// </summary>
    public class FilePackage
    {
        #region Constantes

        private const Int32 TIME_OUT = 5000;
        private const Int32 WAIT_TIME = 500;
        private const String XML_HEADER = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>";

        #endregion

        // Variables
        #region Variables

        private Boolean _isOpen = false;        // le fichier est-il ouvert?

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le nom du fichier englobant
        /// </summary>
        public String PackageFileName
        {
            get;
            protected set;
        } // endProperty: FileName

        /// <summary>
        /// Le package en cours d'utilisation
        /// </summary>
        private ZipPackage _CurrentPackage;
        public ZipPackage CurrentPackage
        {
            get { return _CurrentPackage; }
            protected set
            {
                _CurrentPackage = value;
                if (_CurrentPackage == null && this.IsOpen == true)
                {

                }
            }
        } // endProperty: CurrentPackage

        /// <summary>
        /// Le package est-il ouvert?
        /// </summary>
        public Boolean IsOpen
        {
            get
            {
                return this._isOpen;
            }
            protected set
            {
                this._isOpen = value;
            }
        } // endProperty: IsOpen

        #endregion

        // Constructeur
        #region Constructeur

        public FilePackage()
        {
        }

        #endregion

        // Méthodes
        #region Méthodes

        /// <summary>
        /// Ouvrir le package désigné par FileName
        /// </summary>
        public static FilePackage OpenPackage(String FileName, FileMode FileMode)
        {
            FilePackage Result = new FilePackage();

            Result.PackageFileName = FileName;

            try
            {
                Result.CurrentPackage = (ZipPackage)ZipPackage.Open(FileName, FileMode);

                Result.IsOpen = true;
            }
            catch (Exception e)
            {
                Result = null;
            }

            return Result;
        } // endMethod: OpenPackage

        /// <summary>
        /// Fermer le package en cours d'utilisation
        /// </summary>
        public void ClosePackage ( )
        {
            if (this.IsOpen)
            {
                this.IsOpen = false;
                if (this.CurrentPackage != null)
                {
                   // this.CurrentPackage.Flush();
                    this.CurrentPackage.Close();
                    this.CurrentPackage = null;
                }
                else
                {
                    this.CurrentPackage.Close();
                }
            }
        } // endMethod: ClosePackage
        
        /// <summary>
        /// Acquérir toutes les parties contenant l'extension spécifiée
        /// </summary>
        public ObservableCollection<Uri> GetParts ( String Extension )
        {
            ObservableCollection<Uri> Result = new ObservableCollection<Uri>();

            foreach (var parts in this.CurrentPackage.GetParts())
            {
                if (parts.Uri.OriginalString.Contains(Extension))
                {
                    Result.Add(parts.Uri);
                }
            }
            return Result;
        } // endMethod: GetParts
        
        /// <summary>
        /// Acquérir  la parti désignée par l'uri
        /// </summary>
        public virtual Stream GetPartStream ( Uri PartUri )
        {
            Stream Result = null;
            if (this.IsOpen)
            {
                try
                {
                    ZipPackagePart part = (ZipPackagePart)this.CurrentPackage.GetPart(PartUri);
                    Result = part.GetStream();
                }
                catch
                {
                    Result = null;
                }
            }

            return Result;
        } // endMethod: GetPart

        /// <summary>
        /// Ajouter un package XML au FilePackage en cours
        /// </summary>
        public ZipPackagePart AddXmlPackage ( String PartName, String XML )
        {
            ZipPackagePart Result = null;
            Uri PackageUri = new Uri("/" + PartName, UriKind.Relative);
            //if (this.CurrentPackage != null)
            //{
                if (!this.IsOpen)
                {
                    this.CurrentPackage = (ZipPackage)OpenPackage(this.PackageFileName, FileMode.Open).CurrentPackage;
                    this.IsOpen = true;
                }
                // Si le package existe déjà, le supprimer
                try
                {
                    this.CurrentPackage.DeletePart(PackageUri);
                }
                catch
                {
                    
                }

                if (!this.IsOpen)
                {
                    this.CurrentPackage = (ZipPackage)OpenPackage(this.PackageFileName, FileMode.Open).CurrentPackage;
                    this.IsOpen = true;
                }

                // Créer la partie
                Result = (ZipPackagePart)this.CurrentPackage.CreatePart(PackageUri, System.Net.Mime.MediaTypeNames.Text.Xml, CompressionOption.Normal);
                if (Result != null)
                {
                    // écrire le fichier Xml
                    using (StreamWriter SW = new StreamWriter(Result.GetStream(), Encoding.GetEncoding(Constantes.XML_ENCODING)))
                    {
                        if (!XML.Contains("<?xml version"))
                        {
                            XML = XML_HEADER + "\r\n" + XML;
                        }

                        SW.Write(XML);
                        SW.Flush();
                    }
                }
          //  }

            return Result;
        } // endMethod: AddXmlPackage
        
        /// <summary>
        /// Ajouter une image dans le package
        /// </summary>
        public ZipPackagePart AddPicturePart ( String FileName, String DestFileName )
        {
            if (!this.IsOpen)
            {
                this.CurrentPackage = (ZipPackage)OpenPackage(this.PackageFileName, FileMode.Open).CurrentPackage;
                this.IsOpen = true;
            }
            ZipPackagePart Result = null;

            // Créer l'Uri de destination

            Uri DestPartUri = new Uri(DestFileName , UriKind.Relative);

            // Créer le type MIME
            String TypeMime = "";
            switch (Path.GetExtension(FileName).ToLower())
            {
                case ".jpg":
                    TypeMime = System.Net.Mime.MediaTypeNames.Image.Jpeg;
                    break;
                case ".bmp":
                    TypeMime = "image/bmp";
                    break;
                case ".png":
                    TypeMime = "image/png";
                    break;
                default:
                    TypeMime = "image/";
                    break;
            }

            // Enregistrer le fichier
            if (this.CurrentPackage != null)
            {
                Result = (ZipPackagePart)this.CurrentPackage.CreatePart(DestPartUri, TypeMime, CompressionOption.Normal);
                if (Result != null)
                {
                    using (BinaryWriter SW = new BinaryWriter(Result.GetStream()))
                    {
                        using (BinaryReader SR = new BinaryReader(File.Open(FileName, FileMode.Open)))
                        {
                            Byte[] img = SR.ReadBytes((int)SR.BaseStream.Length);
                            SW.Write(img);
                            SW.Flush();
                        }
                    }
                }
            }

            return Result;
        } // endMethod: AddPicturePart
        public ZipPackagePart AddPicturePart(MemoryStream streamData ,String DestFileName)
        {
            if (!this.IsOpen)
            {
                this.CurrentPackage = (ZipPackage)OpenPackage(this.PackageFileName, FileMode.Open).CurrentPackage;
                this.IsOpen = true;
            }
            ZipPackagePart Result = null;

            // Créer l'Uri de destination

            Uri DestPartUri = new Uri(DestFileName, UriKind.Relative);

            // Créer le type MIME
            String TypeMime = "";
            switch (Path.GetExtension(DestFileName).ToLower())
            {
                case ".jpg":
                    TypeMime = System.Net.Mime.MediaTypeNames.Image.Jpeg;
                    break;
                case ".bmp":
                    TypeMime = "image/bmp";
                    break;
                case ".png":
                    TypeMime = "image/png";
                    break;
                default:
                    TypeMime = "image/";
                    break;
            }

            // Enregistrer le fichier
            if (this.CurrentPackage != null)
            {
                Result = (ZipPackagePart)this.CurrentPackage.CreatePart(DestPartUri, TypeMime, CompressionOption.Normal);
                if (Result != null)
                {
                    using (BinaryWriter SW = new BinaryWriter(Result.GetStream()))
                    {
                        using (BinaryReader SR = new BinaryReader(streamData))
                        {
                            Byte[] img = SR.ReadBytes((int)SR.BaseStream.Length);
                            SW.Write(img);
                            SW.Flush();
                        }
                    }
                }
            }

            return Result;
        } // endMethod: AddPicturePart
        /// <summary>
        /// Obtenir le package Param.Data
        /// </summary>
        public static FilePackage GetParamData ( )
        {
            FilePackage Result;

            string fileName = DefaultValues.Get().ParamData;
            //MessageBox.Show(fileName, "Erreur optionlog");
            Result = FilePackage.OpenPackage(fileName, FileMode.Open);
            //Result = FilePackage.GetFilePackage(fileName);
            
            return Result;
        } // endMethod: GetParamData

        /// <summary>
        /// Obtenir le package Materiel.Data
        /// </summary>
        public static FilePackage GetMaterielData ( )
        {
            FilePackage Result;

            Result = FilePackage.GetFilePackage(DefaultValues.Get().MaterielPackage);
            
            return Result;
        } // endMethod: GetMaterielData

        /// <summary>
        /// Obtenir le package VariableSimple.Data
        /// </summary>
        public static FilePackage GetVariableSimple ( )
        {
            FilePackage Result;

            Result = FilePackage.GetFilePackage(DefaultValues.Get().VariableEditablePackage);

            return Result;
        } // endMethod: GetVariableSimple

        /// <summary>
        /// Obtenir le fichier décrit par Filename
        /// </summary>
        private static FilePackage GetFilePackage ( String Filename )
        {
            FilePackage Result = null;
            Int32 TimeOut = 0;

            // Attendre que la ressource se libère
            if (File.Exists(Filename))
            {
                while (Result == null)
                {
                    Result = FilePackage.OpenPackage(Filename, FileMode.Open);
                    if (Result == null)
                    {
                        JAY.PegaseCore.Helper.TimeHelper.Wait(WAIT_TIME);
                        TimeOut += WAIT_TIME;
                        if (TimeOut > TIME_OUT)
                        {
                            break;
                        }
                    }
                }
            }

            if (Result == null)
            {
                // Essayer sur le chemin local
                String Chemin, Fichier;
                Chemin = AppDomain.CurrentDomain.BaseDirectory + "GenerateData\\";
                Fichier = Path.GetFileName(Filename);
                Result = FilePackage.OpenPackage(Chemin + Fichier, FileMode.OpenOrCreate);
                MessageBox.Show(String.Format("Ressource réseau indisponible, utilisation de la ressource local : {0}", Chemin + Fichier), "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            return Result;
        } // endMethod: GetFilePackage

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: Package
}
