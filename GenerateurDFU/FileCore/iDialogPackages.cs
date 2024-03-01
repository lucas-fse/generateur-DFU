using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;

namespace JAY.FileCore
{
    /// <summary>
    /// Détection d'une liste de fichier de type iDialog,
    /// fourniture des services de recherches
    /// </summary>
    public class iDialogPackages
    {
        // Variables
        #region Variables

        private ObservableCollection<iDialogFileInfo> _iDialogFilesInfos;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le liste des informations des fichiers iDialog pré-chargés
        /// </summary>
        public ObservableCollection<iDialogFileInfo> iDialogFilesInfos
        {
            get
            {
                return this._iDialogFilesInfos;
            }
            private set
            {
                this._iDialogFilesInfos = value;
            }
        } // endProperty: iDialogFilesInfos

        #endregion

        // Constructeur
        #region Constructeur

        // Constructeur
        public iDialogPackages(String FilesPath, Boolean ReadFiles)
        {
            // effectuer le liste des fichiers iDialog présent dans le répertoire indiqué
            String[] Files;

            Files = Directory.GetFiles(FilesPath, "*.iDialog", SearchOption.AllDirectories);
            this._iDialogFilesInfos = new ObservableCollection<iDialogFileInfo>();

            foreach (var file in Files)
            {
                iDialogFileInfo FI = new iDialogFileInfo(file, ReadFiles);
                this._iDialogFilesInfos.Add(FI);
            }
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Ordonne les fichiers par nom réel
        /// </summary>
        public void OrderByFileName ( )
        {
            ObservableCollection<iDialogFileInfo> OrderCollection;

            var Query = from file in this._iDialogFilesInfos
                        orderby file.FileName
                        select file;

            OrderCollection = new ObservableCollection<iDialogFileInfo>();
            
            foreach (var item in Query)
            {
                OrderCollection.Add(item);
            }
        } // endMethod: OrderByFileName

        /// <summary>
        /// Ordonner par les noms utilisateur
        /// </summary>
        public void OrderByUserName ( )
        {
            ObservableCollection<iDialogFileInfo> OrderCollection;

            var Query = from file in this._iDialogFilesInfos
                        orderby file.UserName
                        select file;

            OrderCollection = new ObservableCollection<iDialogFileInfo>();

            foreach (var item in Query)
            {
                OrderCollection.Add(item);
            }
        } // endMethod: OrderByUserName

        /// <summary>
        /// Recherche un fichier dans la liste avec le nom utilisateur
        /// </summary>
        public iDialogFileInfo GetPackageByUserName(String UserName)
        {
            iDialogFileInfo Result;

            var Query = from file in this._iDialogFilesInfos
                        where file.UserName == UserName
                        select file;

            Result = Query.FirstOrDefault();

            return Result;
        } // endMethod: GetPackageByUserName
        
        /// <summary>
        /// Recherche un fichier dans la liste avec le nom de fichier
        /// </summary>
        public iDialogFileInfo GetPackageByFileName ( String FileName )
        {
            iDialogFileInfo Result;

            var Query = from file in this._iDialogFilesInfos
                        where file.FileName == FileName
                        select file;

            Result = Query.FirstOrDefault();

            return Result;
        } // endMethod: GetPackageByFileName
        
        /// <summary>
        /// Raffiner la collection de iDialogInfo pour limiter la liste au fiche FPI
        /// </summary>
        public void RefineFileNames ( String FileType, String Pattern )
        {
            ObservableCollection<iDialogFileInfo> files = new ObservableCollection<iDialogFileInfo>();

            foreach (var file in this.iDialogFilesInfos)
            {
                String FileName;
                // Vérifier le format du nom de fichier
                if (FileType == FPFormat.ID_FICHE_FP_STANDARD)
                {
                    FileName = FPFormat.Instance.IsFPS(file.FileName);
                }
                else if(FileType == FPFormat.ID_FICHE_FP_INSTALLEE)
                {
                    FileName = FPFormat.Instance.IsFPI(file.FileName);
                }
                else
                {
                    FileName = file.FileName;
                }

                // Ajouter le nom de fichier s'il existe
                if (FileName != null)
                {
                    // Vérifier la présence du pattern dans le nom du fichier s'il y a lieu
                    if (Pattern != "")
                    {
                        if (!FileName.Contains(Pattern))
                        {
                            FileName = null;
                        }
                    }

                    if (FileName != null)
                    {
                        files.Add(file);
                    }
                }
            }

            this.iDialogFilesInfos = files;
        } // endMethod: GetFPIFileName

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: iDialogPackages
}
