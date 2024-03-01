using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;

namespace JAY.FileCore
{
    /// <summary>
    /// Les informations des fichiers iDialog.
    /// Elles sont utilisées par le class iDialogPackages lors de recherche
    /// </summary>
    public class iDialogFileInfo
    {
        // Variables
        #region Variables
        private String _userName;
        private String _fileName;
        private String _filePath;
        private String _fullPath;
        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le chemin complet du fichier, y compris le nom de fichier et l'extension
        /// </summary>
        public String FullPath
        {
            get
            {
                return this._fullPath;
            }
            private set
            {
                this._fullPath = value;
            }
        } // endProperty: FullPath

        /// <summary>
        /// Le nom du fichier défini par l'utilisateur (Ref pack commercial)
        /// </summary>
        public String UserName
        {
            get
            {
                return this._userName;
            }
            private set
            {
                this._userName = value;
            }
        } // endProperty: UserName

        /// <summary>
        /// Le nom du fichier géré par le logiciel
        /// </summary>
        public String FileName
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(this._fileName);
            }
            private set
            {
                this._fileName = value;
            }
        } // endProperty: FileName

        /// <summary>
        /// Le chemin d'accès au fichier
        /// </summary>
        public String FilePath
        {
            get
            {
                return this._filePath;
            }
            private set
            {
                this._filePath = value;
            }
        } // endProperty: FilePath

        /// <summary>
        /// La date de création de la fiche
        /// </summary>
        public DateTime LastMAJDateTime
        {
            get;
            private set;
        } // endProperty: CreateDateTime

        /// <summary>
        /// La date de dernière modification convertie en chaine de charactères pour l'affichage
        /// </summary>
        public String DateToString
        {
            get
            {
                String Result;
                Result = String.Format("{0:dd MMMM yyyy}", this.LastMAJDateTime);
                return Result;
            }
        } // endProperty: DateToString

        #endregion

        // Constructeur
        #region Constructeur

        public iDialogFileInfo(String iDialogFileName, Boolean ReadFiles)
        {
            if (ReadFiles)
            {
                iDialogPackage iDP = iDialogPackage.OpenPackage(iDialogFileName, System.IO.FileMode.Open);

                if (iDP != null)
                {

                    FileInfo FI = new FileInfo(iDialogFileName);

                    this.FullPath = iDialogFileName;
                    if (iDP.UserFileName != null)
                    {
                        this.UserName = iDP.UserFileName;
                    }
                    else
                    {
                        this.UserName = "";
                    }
                    this.FileName = Path.GetFileName(iDialogFileName);
                    this.FilePath = Path.GetDirectoryName(iDialogFileName) + @"\";
                    this.LastMAJDateTime = FI.LastWriteTime;

                    iDP.ClosePackage();
                }
                else
                {
                    String Message;
                    String FileName = Path.GetFileName(iDialogFileName);
                    Message = String.Format("La fiche {0} n'a pas pu être pré-chargée.\nElle est probablement déjà ouverte par ailleurs", FileName);
                    MessageBox.Show(Message);
                } 
            }
            else
            {
                if (File.Exists(iDialogFileName))
                {
                    this.FullPath = iDialogFileName;
                    this.FileName = Path.GetFileName(iDialogFileName);
                    this.FilePath = Path.GetDirectoryName(iDialogFileName) + @"\";

                    FileInfo FI = new FileInfo(iDialogFileName);
                    this.LastMAJDateTime = FI.LastWriteTime;
                }
            }
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Copier le fichier spécifié depuis source vers Dest
        /// </summary>
        /// <param name="Destination">
        /// Le fichier décrit par ce FileInfo est copier vers le chemin spécifié.
        /// Le chemin doit comporter le nom du fichier
        /// </param>
        public void Copy ( String Destination )
        {
            File.Copy(this.FullPath, Destination);
        } // endMethod: Copy
        
        /// <summary>
        /// Ouvrir le package désigné par ce iDialogFileInfo
        /// </summary>
        public iDialogPackage Open ( FileMode Mode)
        {
            iDialogPackage Result;

            Result = iDialogPackage.OpenPackage(this.FullPath, Mode);

            return Result;
        } // endMethod: Open

        /// <summary>
        /// Acquérir la totalité du fichier iDialog sous la forme d'un tableau de byte
        /// Cette méthode peut être utilisée pour le stockage en base de données
        /// </summary>
        public Byte[] GetiDialogBinary()
        {
            Byte[] Result;

            FileInfo FI = new FileInfo(this.FullPath);
            Result = new Byte[FI.Length];

            using (BinaryReader BR = new BinaryReader(File.Open(this.FullPath, FileMode.Open)))
            {
                for (int i = 0; i < Result.Length; i++)
                {
                    Result[i] = BR.ReadByte();
                }
            }

            return Result;
        } // endMethod: GetiDialogBinary

        #endregion

        #region Méthodes static

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: iDialogFileInfo
}
