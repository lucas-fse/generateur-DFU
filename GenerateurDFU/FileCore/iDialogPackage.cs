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
using System.ComponentModel;

namespace JAY.FileCore
{
    /// <summary>
    /// Cette classe de base permet de fabriquer des packages facilement
    /// ces package enferme des données xml par exemple
    /// </summary>
    public class iDialogPackage : iDialogPackageBase
    {
        #region Constantes

        // constantes

        #endregion

        // Variables
        #region Variables

        private iDialogPackageBase _idPB;

        #endregion

        // Propriétés
        #region Propriétés


        #endregion

        // Constructeur
        #region Constructeur

        public iDialogPackage ( ) : base()
        {
            
        }

        private iDialogPackage(iDialogPackageBase iDPB)
        {
            this._idPB = iDPB;
        }

        #endregion

        // Méthodes
        #region Méthodes

        /// <summary>
        /// Exploser un fichier iDialog ancien format (une archive unique avec toute les versions à l'intérieures)
        /// En n nouveau fichier iDialog nouveau format (un paramétrage unique par fichier)
        /// </summary>
        /// <param name="Filename">
        /// Le nom du fichier initial
        /// </param>
        /// <param name="DestinationFolder">
        /// Le nom du dossier de destination
        /// </param>
        /// <returns>
        /// La collection des fichiers créés
        /// </returns>
        public static new iDialogPackage OpenPackage(String FileName, FileMode FileMode)
        {
            iDialogPackage Result = new iDialogPackage();
            Result.OpenIDialogFile(FileName, FileMode);

            return Result;
        }

        /// <summary>
        /// Exploser un fichier iDialog ancien format (une archive unique avec toute les versions à l'intérieures)
        /// En n nouveau fichier iDialog nouveau format (un paramétrage unique par fichier)
        /// </summary>
        /// <param name="Filename">
        /// Le nom du fichier initial
        /// </param>
        /// <param name="DestinationFolder">
        /// Le nom du dossier de destination
        /// </param>
        /// <returns>
        /// La collection des fichiers créés
        /// </returns>
        public ObservableCollection<String> ExplodeOldIDialogPackage ( String Filename, String DestinationFolder, BackgroundWorker worker )
        {
            ObservableCollection<String> Result;

            Result = new ObservableCollection<String>();

            // 1 - Si destinationFolder n'existe pas, le créer
            if (!Directory.Exists(DestinationFolder))
            {
                Directory.CreateDirectory(DestinationFolder);
            }
            if (DestinationFolder.Substring(DestinationFolder.Length - 1, 1) != "\\")
            {
                DestinationFolder += "\\";
            }

            // 1.1 - Créer un fichier iDialog vide à partir de packageBase
            String EmptyPackageFileName = DestinationFolder + System.IO.Path.GetFileNameWithoutExtension(Filename) + String.Format("_temp") + ".idialog";

            if (File.Exists(EmptyPackageFileName))
            {
                File.Delete(EmptyPackageFileName);
            }

            File.Copy(Filename, EmptyPackageFileName);

            // 1.2 - Vider le package en copiant l'intégralité des xml de paramétrage dans le dossier en tant que fichiers temporaires
            iDialogPackageBase packageBase = iDialogPackageBase.OpenPackage(EmptyPackageFileName, FileMode.Open);

            Int32 i = 0, NbFile;
            NbFile = packageBase.XMLParts.Count;
            String[] xmlPartFileName = new String[NbFile];

            foreach (Uri subFile in packageBase.XMLParts)
            {
                if (worker != null)
                {
                    worker.ReportProgress((i * 50) / NbFile);
                }

                String DestXMLFileName = DestinationFolder + System.IO.Path.GetFileNameWithoutExtension(Filename) + String.Format("_{0:0000}", i) + ".xml";
                xmlPartFileName[i] = DestXMLFileName;

                if (!packageBase.IsOpen)
                {
                    packageBase.OpenPackage();
                }
                // Enregistrer le fichier XML en fichier temporaire
                Stream stream = packageBase.GetPartStream(subFile);

                XDocument doc = XDocument.Load(stream);
                stream.Close();

                using (StreamWriter SW = new StreamWriter(File.Open(DestXMLFileName, FileMode.Create)))
                {
                    SW.Write(doc.ToString());
                    SW.Flush();
                }

                // Supprimer la partie de l'archive
                packageBase.DeletePackagePart(subFile);
                i++;
            }

            packageBase.ClosePackage();

            // 2 - pour tous les xml temporaire, créer un package séparé
            i = 0;
            foreach (String XMLFileName in xmlPartFileName)
            {
                String XMLContent;

                if (worker != null)
                {
                    worker.ReportProgress((i * 50) / NbFile + 50);
                }
                // 3 - dupliquer le fichier et supprimer le fichier temporaire XML
                String DestFileName = DestinationFolder + System.IO.Path.GetFileNameWithoutExtension(Filename) + String.Format("_{0:0000}", i) + ".idialog";
                if (File.Exists(DestFileName))
                {
                    File.Delete(DestFileName);
                }

                File.Copy(EmptyPackageFileName, DestFileName);

                iDialogPackageBase iDPB = iDialogPackageBase.OpenPackage(DestFileName, FileMode.Open);

                using (StreamReader SR = new StreamReader(File.OpenRead(xmlPartFileName[i])))
                {
                    XMLContent = SR.ReadToEnd();
                    iDPB.AddXmlPackage("IDialogDataPart", XMLContent);
                }

                iDPB.ClosePackage();

                File.Delete(xmlPartFileName[i]);

                i++;

                Result.Add(DestFileName);
            }

            // 4 - Supprimer le package temporaire.
            File.Delete(EmptyPackageFileName);

            return Result;
        } // endMethod: ExplodeOldIDialogPackage

        /// <summary>
        /// Obtenir le numero de version du fichier depuis son nom
        /// </summary>
        public static Int32 GetVersionFromFilename ( String Filename )
        {
            Int32 Result = -1;

            String[] parts;
            Filename = System.IO.Path.GetFileNameWithoutExtension(Filename);
            parts = Filename.Split(new Char[] { '_' });

            if (parts.Length > 0)
            {
                String versionStr = parts.Last();
                try
                {
                    Result = Convert.ToInt32(versionStr);
                }
                catch
                {
                    Result = -1;
                }
            }

            return Result;
        } // endMethod: GetVersionFromFilename

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: XMLPackage
}
