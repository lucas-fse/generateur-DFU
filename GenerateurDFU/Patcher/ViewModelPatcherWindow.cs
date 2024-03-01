using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.IO;
using Mvvm = GalaSoft.MvvmLight;
using JAY.FileCore;
using System.Globalization;

namespace JAY.Patcher
{
    /// <summary>
    /// Le ViewModel pour le patcher de la fiche
    /// </summary>
    public class ViewModelPatcherWindow : Mvvm.ViewModelBase
    {
        // Variables
        #region Variables

        private String _message;
        private BackgroundWorker _worker;
        private Double _inProgress;
        private Visibility _inProgressVisibility;

        private String _pathPatchFiles;
        private String _pathRefFiles;
        private String _pathWorkDir;
        private String _pathLanguage;
        private String _patchFileName;
        private String _currentDirectory;

        private iDialogPackage _packageToPatch;
        private Boolean _isInPatch;

        private Boolean _endofpatch = true;
        private Boolean _lockmessage = false;

        private Dictionary<Int32, String> _correspondanceFileVersionToPatch;
        private ObservableCollection<String> _listLangFiles;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le nom du fichier package
        /// </summary>
        public String PackageFileName
        {
            get;
            private set;
        } // endProperty: PackageFileName

        /// <summary>
        /// La liste des fichiers de langue
        /// </summary>
        public ObservableCollection<String> ListLangFiles
        {
            get
            {
                if (this._listLangFiles == null)
                {
                    this._listLangFiles = new ObservableCollection<String>();
                    List<String> Files = Directory.EnumerateFiles(PathLanguage, "*.xml").ToList();

                    if (Files != null)
                    {
                        foreach (var file in Files)
                        {
                            this._listLangFiles.Add(file);
                        }
                    }
                }

                return this._listLangFiles;
            }
        } // endProperty: ListLangFiles

        /// <summary>
        /// Correspondance entre la version du fichier et le patch à appliquer
        /// </summary>
        public Dictionary<Int32, String> CorrespondanceFileVersionToPatch
        {
            get
            {
                if (this._correspondanceFileVersionToPatch == null)
	            {
		            this._correspondanceFileVersionToPatch = new Dictionary<Int32, String>();
                    this._correspondanceFileVersionToPatch.Add(2, "patcher_from_2_to_3.exe");
                    this._correspondanceFileVersionToPatch.Add(3, "patcher_from_3_to_4.exe");
                    this._correspondanceFileVersionToPatch.Add(4, "patcher_from_4_to_5.exe");
                    this._correspondanceFileVersionToPatch.Add(5, "patcher_from_5_to_6.exe");
                    this._correspondanceFileVersionToPatch.Add(6, "patcher_from_6_to_13.exe");
                    this._correspondanceFileVersionToPatch.Add(7, "patcher_from_6_to_13.exe");
                    this._correspondanceFileVersionToPatch.Add(8, "patcher_from_6_to_13.exe");
                    this._correspondanceFileVersionToPatch.Add(9, "patcher_from_6_to_13.exe");
                    this._correspondanceFileVersionToPatch.Add(10, "patcher_from_6_to_13.exe");
                    this._correspondanceFileVersionToPatch.Add(11, "patcher_from_6_to_13.exe");
                    this._correspondanceFileVersionToPatch.Add(12, "patcher_from_6_to_13.exe");
                }
                return this._correspondanceFileVersionToPatch;
            }
        } // endProperty: CorrespondanceFileVersionToPatch

        /// <summary>
        /// L'application est en cours de patch
        /// </summary>
        public Boolean IsInPatch
        {
            get
            {
                return this._isInPatch;
            }
            set
            {
                this._isInPatch = value;
                RaisePropertyChanged("IsInPatch");
            }
        } // endProperty: IsInPatch

        /// <summary>
        /// Le package iDialog à patcher
        /// </summary>
        public iDialogPackage PackageToPatch
        {
            get
            {
                return this._packageToPatch;
            }
            set
            {
                this._packageToPatch = value;
                RaisePropertyChanged("PackageToPatch");
            }
        } // endProperty: PackageToPatch
        
        /// <summary>
        /// Valeur en progression
        /// </summary>
        public Double InProgress
        {
            get
            {
                return this._inProgress;
            }
            set
            {
                this._inProgress = value;
                RaisePropertyChanged("InProgress");
            }
        } // endProperty: InProgress

        /// <summary>
        /// La visibilité de l'icon de progression
        /// </summary>
        public Visibility InProgressVisibility
        {
            get
            {
                return this._inProgressVisibility;
            }
            set
            {
                this._inProgressVisibility = value;
                RaisePropertyChanged("InProgressVisibility");
            }
        } // endProperty: InProgressVisibility

        /// <summary>
        /// Le titre de la fenêtre
        /// </summary>
        public String Title
        {
            get
            {
                return "iDialog patcher 1.0";
            }
        } // endProperty: Title

        /// <summary>
        /// Les messages d'information
        /// </summary>
        public String Message
        {
            get
            {
                return this._message;
            }
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        } // endProperty: Message

        /// <summary>
        /// Le worker utilisé sur les tâches asynchrones
        /// </summary>
        public BackgroundWorker Worker
        {
            get
            {
                return this._worker;
            }
            private set
            {
                this._worker = value;
            }
        } // endProperty: Worker

        /// <summary>
        /// Le path pour les fichiers de patch
        /// </summary>
        public String PathPatchFiles
        {
            get
            {
                return this._pathPatchFiles;
            }
            private set
            {
                this._pathPatchFiles = value;
                RaisePropertyChanged("PathPatchFiles");
            }
        } // endProperty: PathPatchFiles

        /// <summary>
        /// Le chemin du fichier de référence
        /// </summary>
        public String PathRefFiles
        {
            get
            {
                return this._pathRefFiles;
            }
            private set
            {
                this._pathRefFiles = value;
                RaisePropertyChanged("PathRefFiles");
            }
        } // endProperty: PathRefFiles

        /// <summary>
        /// Le chemin du répertoire de travail
        /// </summary>
        public String PathWorkDir
        {
            get
            {
                return this._pathWorkDir;
            }
            private set
            {
                this._pathWorkDir = value;
                RaisePropertyChanged("PathWorkDir");
            }
        } // endProperty: PathWorkDir

        /// <summary>
        /// Le chemin pour les fichiers de langues 
        /// </summary>
        public String PathLanguage
        {
            get
            {
                return this._pathLanguage;
            }
            set
            {
                this._pathLanguage = value;
                RaisePropertyChanged("PathLanguage");
            }
        } // endProperty: PathLanguage

        public bool EndOfPatch
        {
            get
            {
                return this._endofpatch;
            }
        }
        #endregion

        // Constructeur
        #region Constructeur

        public ViewModelPatcherWindow()
        {
            // Initialisations
            this.InProgressVisibility = Visibility.Hidden;
            
            // Initialiser les valeurs des chemins
            this.PathLanguage = DefaultValues.Get().FileLanguageFolder;
            this.PathPatchFiles = DefaultValues.Get().AppPath + @"PatcherData\PatchFiles\";
            this.PathRefFiles = DefaultValues.Get().RefFileName;
        }

        void W_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Détruire le worker
            this.Worker.DoWork -= W_DoWork;
            this.Worker.RunWorkerCompleted -= W_RunWorkerCompleted;
            this.Worker = null;

            // Envoyer un message à la fenêtre pour qu'elle se ferme
            Mvvm.Messaging.Messenger.Default.Send<CommandMessage>(new CommandMessage(this, JAY.PegaseCore.Commands.CMD_CLOSE_WINDOW));
        }

        void W_DoWork(object sender, DoWorkEventArgs e)
        {
            // Vérifier la version de la fiche en cours
            DateTime Start = DateTime.Now;
            DateTime StartUpdate = DateTime.Now;
            TimeSpan Time, TimeUpdate;
            Double Vitesse = 2;

            this.Message = String.Format("Update file. Please wait");
            this.InProgressVisibility = Visibility.Visible;

            // Lancer la mise à jour
            BackgroundWorker patcherWorker = new BackgroundWorker();
            patcherWorker.DoWork += new DoWorkEventHandler(patcherWorker_DoWork);
            patcherWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(patcherWorker_RunWorkerCompleted);
            patcherWorker.RunWorkerAsync();
            this.IsInPatch = true;
            // faire tourner l'icon de 'work in progress'
            while (this.IsInPatch)
            {
                // Rotation de l'icon 'in progress' en temps réel
                TimeUpdate = DateTime.Now - StartUpdate;
                
                this.InProgress += Vitesse * TimeUpdate.TotalMilliseconds * Vitesse / 10;
                if (this.InProgress > 360)
                {
                    this.InProgress -= 360;
                }
                StartUpdate = DateTime.Now;
                // Fin rotation
            }
        }

        void patcherWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // nettoyer l'application
            BackgroundWorker patcherWorker = sender as BackgroundWorker;

            patcherWorker.DoWork -= patcherWorker_DoWork;
            patcherWorker.RunWorkerCompleted -= patcherWorker_RunWorkerCompleted;

            this.IsInPatch = false;
        }

        void patcherWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // ouvrir le fichier iDialog et exécuter le scripts
            this.ExecuteCommandMAJ();
        }

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandMAJ()
        {
            this.PackageToPatch = iDialogPackage.OpenPackage(PackageFileName, FileMode.Open);
            // Charger le fichier de référence
            String RefFile;
            String RefFileName = this.PathRefFiles;
            using (StreamReader SR = new StreamReader(File.Open(RefFileName, FileMode.Open), Encoding.GetEncoding(Constantes.XML_ENCODING)))
            {
                RefFile = SR.ReadToEnd();
            }

            // Mettre à jour tous les fichiers
            if(this.PackageToPatch != null)
            {
                // Ouvrir le fichier
                //this.PackageToPatch.OpenPackage();
                String Doc = this.PackageToPatch.GetCurrentVersionXML();

                // Créer le fichier bat dans le répertoire Work
                this._currentDirectory = Directory.GetCurrentDirectory();
                Directory.SetCurrentDirectory(this.PathPatchFiles);

                String bat = String.Format("\"{0}\" \"{1}\" RefFile tempFile", this._patchFileName, this.PathWorkDir.Substring(0, this.PathWorkDir.Length - 1));
                String batchFile = this.PathWorkDir + "Update.bat";
                String tempFile = this.PathWorkDir + "tempFile.xml";
                String refFile = this.PathWorkDir + "RefFile.xml";
                Int32 CurrentCodePage = CultureInfo.CurrentCulture.TextInfo.ANSICodePage;
                using (StreamWriter SW = new StreamWriter(File.Open(batchFile , FileMode.Create), Encoding.GetEncoding(CurrentCodePage)))
                {
                    SW.WriteLine(bat);
                    SW.Flush();
                }

                // Copier les fichiers dans le répertoire Work
                using (StreamWriter SW = new StreamWriter(File.Open(refFile, FileMode.Create)))
                {
                    SW.Write(RefFile);
                    SW.Flush();
                }

                using (StreamWriter SW = new StreamWriter(File.Open(tempFile, FileMode.Create)))
                {
                    SW.Write(Doc);
                    SW.Flush();
                }

                // executer le fichier bat
                JAY.BatchUtilities.ExecuteBat(tempFile, batchFile, System.Diagnostics.ProcessWindowStyle.Hidden);

                // effacer le fichier Temp_output.xml
                File.Delete(tempFile);
                File.Delete(batchFile);
                File.Delete(refFile);

                // recopier le fichier dans l'archive
                tempFile = this.PathWorkDir + @"\tempFile_output.xml";
                if (File.Exists(tempFile))
                {
                    using (StreamReader SR = new StreamReader(File.Open(tempFile, FileMode.Open)))
                    {
                        Doc = SR.ReadToEnd();
                    }
                    JAY.PegaseCore.Helper.TimeHelper.Wait(1000);
                    this.PackageToPatch.SaveNewVersion(Doc);

                    // effacer le fichier Temp_output.xml
                    File.Delete(tempFile);
                }

                // Ajouter les fichiers de langues
                if (this.ListLangFiles.Count > 0)
                {
                    foreach (String FileName in this.ListLangFiles)
                    {
                        using (StreamReader SR = new StreamReader(File.Open(FileName, FileMode.Open), Encoding.GetEncoding(Constantes.XML_ENCODING)))
                        {
                            Doc = SR.ReadToEnd();
                        }
                        String uripath = "/Languages/" + System.IO.Path.GetFileName(FileName);
                        Uri UPath = new Uri(uripath, UriKind.Relative);

                        Stream ExistFile = this.PackageToPatch.GetPartStream(UPath);

                        if (ExistFile != null)
                        {
                            // Effacer le fichier existant
                            this.PackageToPatch.DeletePackagePart(UPath);
                        }

                        this.PackageToPatch.AddXMLPart(UPath, Doc);
                    }
                }
                else
                {
                    
                    string message = LanguageSupport.Get().GetText("DECONNEXION/LANG_ERROR");
                    System.Windows.MessageBox.Show(message);
                }

                // Mettre à jour le fichier Ref de l'archive iDialog
                if (File.Exists(RefFileName))
                {
                    Uri RefFileUri = new Uri("/References/RefFile.xml", UriKind.Relative);

                    Stream ExistFile = this.PackageToPatch.GetPartStream(RefFileUri);

                    if (ExistFile != null)
                    {
                        // Effacer le fichier existant
                        this.PackageToPatch.DeletePackagePart(RefFileUri);
                    }

                    this.PackageToPatch.AddXMLPart(RefFileUri, RefFile);
                }
                this.PackageToPatch.ClosePackage();

                // restaurer le directory en cours
                Directory.SetCurrentDirectory(this._currentDirectory);
            }
        } // endMethod: ExecuteCommandMAJ

        /// <summary>
        /// Mettre à jour le fichier transmis ici, s'il y a lieux
        /// </summary>
        public Boolean UpdateFile ( String PackageFileName, Int32 CurrentVersion )
        {
            Boolean Result = false;
            Boolean resultrequest = false;
            String patchFileName;
            // 1 - Vérifier si la version nécessite un update
            this._endofpatch = this.CorrespondanceFileVersionToPatch.TryGetValue(CurrentVersion, out patchFileName);
            if (this._endofpatch)
            {
                this._patchFileName = DefaultValues.Get().AppPath + "PatcherData\\PatchFiles\\";
                this._patchFileName += patchFileName;
                // 2- Demander si l'update doit être appliqué
                if (this._lockmessage == false)
                {
                    string message = LanguageSupport.Get().GetText("ERR_MSG/OBS_FILE");
                    string titre = LanguageSupport.Get().GetToolTip("ERR_MSG/OBS_FILE");
                    if (MessageBox.Show(message, titre, MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    {
                        resultrequest = true;
                        this._lockmessage = true;
                    }
                    else
                    {
                        resultrequest = false;
                        this._lockmessage = true;
                    }
                }
                else
                {
                    resultrequest = true;
                }

                if (resultrequest == true)
                {
                    BackgroundWorker W = new BackgroundWorker();
                    W.DoWork += new DoWorkEventHandler(W_DoWork);
                    W.RunWorkerCompleted += new RunWorkerCompletedEventHandler(W_RunWorkerCompleted);

                    this.Worker = W;

                    this.PackageFileName = PackageFileName;
                    //this.PathWorkDir = Path.GetDirectoryName(PackageFileName) + "\\";
                    this.PathWorkDir = DefaultValues.Get().AppDataFolder;
                    // 3 - Appliquer l'update
                    this.Worker.RunWorkerAsync();
                    Result = true;
                }
            }

            return Result;
        } // endMethod: UpdateFile

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: ViewModelPatcherWindow
}
