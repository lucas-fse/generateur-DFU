using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using Mvvm = GalaSoft.MvvmLight;
using System.Windows;
using Form = System.Windows.Forms;
using JAY.FileCore;
using JAY.XMLCore;

namespace JAY.DAL
{
    /// <summary>
    /// Ajout de code sur la classe générée automatiquement par Entity Framework
    /// </summary>
    public partial class ProjectDetail
    {
        // Variables
        #region Variables

        private Visibility _isVisible;
        private String _fileName;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La date et l'heure formatée
        /// </summary>
        public String DateCreationFormat
        {
            get
            {
                String Result;

                if (LanguageSupport.Get().LanguageName == "Francais")
                {
                    Result = String.Format("{0:dd/MM/yyyy HH:mm:ss}", this.DateCreation); 
                }
                else
                {
                    Result = String.Format("{0:MM/dd/yyyy HH:mm:ss}", this.DateCreation);
                }

                return Result;
            }
        } // endProperty: DateCreationFormat

        /// <summary>
        /// Libellé pour la création de FPS
        /// </summary>
        public String LibelCreateFPS
        {
            get
            {
                return "Plg_ConnexionT/TOOLS_CREATEFPS";
            }
        } // endProperty: LibelCreateFPS

        /// <summary>
        /// La visibilité du bouton 'créer une FPS'
        /// </summary>
        public Visibility CreateFPSVisibility
        {
            get
            {
                Visibility Result = Visibility.Collapsed;
                if (DefaultValues.Get().UserMode == DefaultValues.EXPERT)
                {
                    Result = Visibility.Visible;
                }

                return Result;
            }
        } // endProperty: CreateFPSVisibility

        /// <summary>
        /// Le nom du fichier et son emplacement
        /// </summary>
        public String FileName
        {
            get
            {
                if (this._fileName == null)
                {
                    this._fileName = String.Format("{0}{1}\\{2}_{3:0000}.idialog", DefaultValues.Get().iDialogFileFolder, this.GetParentProject().ProjectName, this.NomIDialog, this.Version);
                }

                return this._fileName;
            }
        } // endProperty: FileName

        /// <summary>
        /// Le numéro de version du fichier
        /// </summary>
        public String VersionNumber
        {
            get
            {
                String Result = "";
                Result = String.Format("{0:0000}", this.Version);
                return Result;
            }
        } // endProperty: VersionNumber

        /// <summary>
        /// Le nom de base du fichier paramétrage dans l'archive iDialog
        /// </summary>
        public String BaseFileName
        {
            get
            {
                //String Result = this.NomIDialog;
                String Result = this.GetParentProject().UserProjectName;
                return Result;
            }
        } // endProperty: BaseFileName

        /// <summary>
        /// Le détail est-il visible?
        /// </summary>
        public Visibility IsVisible
        {
            get
            {
                return this._isVisible;
            }
        } // endProperty: IsVisible
        
        /// <summary>
        /// Le sous-projet est actuellement sélectionné
        /// </summary>
        public Boolean IsObsolete
        {
            get
            {
                return this.Obsolete;
            }
            set
            {
                // Si value = false, vérifier qu'au moins un ProjectDetail est actif dans le projet
                if (value == true)
                {
                    var query = from detail in this.GetParentProject().ProjectDetails
                                where detail.Obsolete == false
                                select detail;

                    if (query.Count() > 1)
                    {
                        this.Obsolete = value;
                        BDDClient.Get().iDialogLocalData.SaveChanges();
                    }
                }
                else
                {
                    this.Obsolete = value;
                    BDDClient.Get().iDialogLocalData.SaveChanges();
                }
            }
        } // endProperty: IsSelected

        /// <summary>
        /// Libellé pour le chargement
        /// </summary>
        public String LibelLoad
        {
            get
            {
                return "Plg_ConnexionT/TOOLS_LOAD";
            }
        } // endProperty: LibelLoad

        #endregion

        // Constructeur
        #region Constructeur

        public ProjectDetail()
        {
            // Créer les commandes
            this.CreateCommandOpen();
            this.CreateCommandCreateFPS();

            // Recevoir les messages
            Messenger.Default.Register<CommandMessage>(this, ReceiveMessage);
        }

        #endregion

        // Méthodes
        #region Méthodes

        /// <summary>
        /// Le message reçu
        /// </summary>
        public void ReceiveMessage(CommandMessage message)
        {
            // Message destiné à tous les éléments
            if (message.Sender == null)
            {
                switch (message.Command)
                {
                    case PegaseCore.Commands.CMD_MAJ_LANGUAGE:
                        
                        break;
                    default:
                        break;
                }
            }
        } // endMethod: ReceiveMessage

        /// <summary>
        /// Acquérir le projet parent de ce ProjectDetail
        /// </summary>
        public Projects GetParentProject ( )
        {
            Projects Result = null;

            var QueryProject = from project in BDDClient.Get().iDialogLocalData.Projects
                               where project.Id == this.IdProject
                               select project;

            if (QueryProject.Count() > 0)
            {
                Result = QueryProject.First();
            }

            return Result;
        } // endMethod: GetParentProject

        /// <summary>
        /// Définir la visibilité du détail en fonction de la visibilité des obsoletes et des récents
        /// </summary>
        public void SetVisibility ( Boolean IsObsoleteVisible, Boolean IsNewVisible )
        {
            Visibility Result = Visibility.Collapsed;

            if (this.Obsolete)
            {
                if (IsObsoleteVisible)
                {
                    Result = Visibility.Visible;
                }
            }
            else
            {
                if (IsNewVisible)
                {
                    Result = Visibility.Visible;
                }
            }

            this._isVisible = Result;
        } // endMethod: SetVisibility

        #endregion

        // Messages
        #region Messages

        #endregion

        #region Commandes

        #region CommandOpen
        /// <summary>
        /// La commande Open
        /// </summary>
        public ICommand CommandOpen
        {
            get;
            internal set;
        } // endProperty: CommandOpen

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandOpen()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandOpen = new Mvvm.Command.RelayCommand(ExecuteCommandOpen, CanExecuteCommandOpen);
        } // endMethod: CreateCommandOpen

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandOpen()
        {
            Messenger.Default.Send<CommandMessage>(new CommandMessage(this, JAY.PegaseCore.Commands.CMD_LOAD));
        } // endMethod: ExecuteCommandOpen

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandOpen()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandOpen 
        #endregion

        #region CommandCreateFPS
        /// <summary>
        /// La commande CreateFPS
        /// </summary>
        public ICommand CommandCreateFPS
        {
            get;
            internal set;
        } // endProperty: CommandCreateFPS

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandCreateFPS()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandCreateFPS = new Mvvm.Command.RelayCommand(ExecuteCommandCreateFPS, CanExecuteCommandCreateFPS);
        } // endMethod: CreateCommandCreateFPS

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandCreateFPS()
        {
            Form.SaveFileDialog SFD;

            SFD = new Form.SaveFileDialog();
            SFD.Filter = LanguageSupport.Get().GetText("FILTER/IDIALOG_FILTER");

            if (SFD.ShowDialog() == Form.DialogResult.OK)
            {
                // Exporter le fichier en cours sous forme de FPS
                if (File.Exists(SFD.FileName))
                {
                    File.Delete(SFD.FileName);
                }

                File.Copy(this.FileName, SFD.FileName);

                // renommer le fichier contenant les données xml
                iDialogPackageBase package = iDialogPackageBase.OpenPackage(SFD.FileName, FileMode.Open);
                String doc;

                // 1 - Charger les données
                using (StreamReader sr = new StreamReader(package.GetCurrentVersionFileStream(), Encoding.GetEncoding(iDialogPackageBase.XML_ENCODING)))
                {
                    doc = sr.ReadToEnd();
                }

                // 1.5 - Supprimer les parties du fichier ZIP
                Uri oldPartUri = new Uri("/IDialogDataPart", UriKind.Relative);
                package.DeletePackagePart(oldPartUri);
                if (package.XMLParts.Count > 0)
                {
                    foreach (var item in package.XMLParts)
                    {
                        package.DeletePackagePart(item);
                    } 
                }

                // 2 - Enregistrer la partie correctement nommées pour le logiciel de production
                String baseFileName = Path.GetFileNameWithoutExtension(SFD.FileName);
                Int32 pos = baseFileName.IndexOf('_');
                if (pos > -1)
                {
                    baseFileName = baseFileName.Substring(0, pos);
                }

                baseFileName += "_00_01.xml";
                package.AddXmlPackage(baseFileName, doc);

                // 3 - Fermer le package
                package.ClosePackage();
            }
        } // endMethod: ExecuteCommandCreateFPS

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandCreateFPS()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandCreateFPS

        #endregion

        #endregion

    } // endClass: ProjectDetail
}
