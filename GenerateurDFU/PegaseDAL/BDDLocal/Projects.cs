using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Input;
using Mvvm = GalaSoft.MvvmLight;

namespace JAY.DAL
{
    public partial class Projects
    {
        // Variables
        #region Variables

        private ObservableCollection<ProjectDetail> _projectDetails;
        private Boolean _isSelected;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Libellé pour la poubelle
        /// </summary>
        public String LibelTrashcan
        {
            get
            {
                return "Plg_ConnexionT/TRASHCAN";
            }
        } // endProperty: LibelTrashcan

        /// <summary>
        /// Libellé pour la suppression des fichiers obsolètes
        /// </summary>
        public String LibelDeleteObsolete
        {
            get
            {
                return "Plg_ConnexionT/DELETE_OBSOLETE";
            }
        } // endProperty: LibelDeleteObsolete

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

        /// <summary>
        /// Le projet est-il sélectionné?
        /// </summary>
        public Boolean IsSelected
        {
            get
            {
                return this._isSelected;
            }
            set
            {
                this._isSelected = value;
            }
        } // endProperty: IsSelected

        /// <summary>
        /// La liste des elements 'ProjectDetail' permettant de gérer les différentes versions d'un projet
        /// </summary>
        public ObservableCollection<ProjectDetail> ProjectDetails
        {
            get
            {
                if (this._projectDetails == null)
                {
                    this.InitProjectDetail();
                }

                return this._projectDetails;
            }
        } // endProperty: ProjectDetails

        #endregion

        // Constructeur
        #region Constructeur
        
        public Projects() : base()
        {
            // Définir les commandes
            this.CreateCommandDeleteProject();
            this.CreateCommandOpenLastVersion();
            this.CreateCommandPurgeObsoleteFile();
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Donnée la dernière version de fichier pour ce projet
        /// </summary>
        public Int32 GetLastVersion( )
        {
            Int32 Result = -1;

            var QueryVer = from version in this.ProjectDetails
                           orderby version.Version descending
                           select version.Version;

            if (QueryVer.Count() > 0)
            {
                Result = QueryVer.First();
            }

            return Result;
        } // endMethod: GetLastVersion

        /// <summary>
        /// Initialiser la collection des détails du projet
        /// </summary>
        public void InitProjectDetail( )
        {
            // Remplir la collection
            this._projectDetails = new ObservableCollection<ProjectDetail>();
            if (BDDClient.Get().iDialogLocalData != null)
            {
                var QueryList = from projectDetail in BDDClient.Get().iDialogLocalData.ProjectDetail
                                where projectDetail.IdProject == this.Id
                                orderby projectDetail.DateCreation descending, projectDetail.Version descending
                                select projectDetail;

                if (QueryList.Count() > 0)
                {
                    foreach (var item in QueryList)
                    {
                        this._projectDetails.Add(item);
                    }
                }
            }
        } // endMethod: InitProjectDetail

        #endregion

        // Messages
        #region Messages

        #endregion

        #region Commandes

        #region CommandOpenLastVersion
        /// <summary>
        /// La commande OpenLastVersion
        /// </summary>
        public ICommand CommandOpenLastVersion
        {
            get;
            internal set;
        } // endProperty: CommandOpenLastVersion

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandOpenLastVersion()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandOpenLastVersion = new Mvvm.Command.RelayCommand(ExecuteCommandOpenLastVersion, CanExecuteCommandOpenLastVersion);
        } // endMethod: CreateCommandOpenLastVersion

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandOpenLastVersion()
        {
            // à faire : implémenter la commande
            if (this.ProjectDetails.Count > 0)
            {
                this.ProjectDetails[0].ExecuteCommandOpen();
            }
        } // endMethod: ExecuteCommandOpenLastVersion

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandOpenLastVersion()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandOpenLastVersion
        #endregion


        #region CommandDeleteProject
        /// <summary>
        /// La commande DeleteProject
        /// </summary>
        public ICommand CommandDeleteProject
        {
            get;
            internal set;
        } // endProperty: CommandDeleteProject

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandDeleteProject()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandDeleteProject = new Mvvm.Command.RelayCommand(ExecuteCommandDeleteProject, CanExecuteCommandDeleteProject);
        } // endMethod: CreateCommandDeleteProject

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandDeleteProject()
        {
            String message = String.Format(LanguageSupport.Get().GetToolTip("Plg_ConnexionT/WARNING_DEL_PROJECT"), this.ProjectName);
            if (System.Windows.MessageBox.Show(message, "", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Information) == System.Windows.MessageBoxResult.Yes)
            {
                // 1 - Effacer toutes les lignes de détails correspondant à ce projet
                for (int i = this.ProjectDetails.Count - 1; i >= 0; i--)
                {
                    ProjectDetail detailProject = this.ProjectDetails[i];
                    this.ProjectDetails.Remove(detailProject);
                    BDDClient.Get().iDialogLocalData.ProjectDetail.Remove(detailProject);
                    // 2 - Effacer tous les fichiers correspondant à ce projet
                    if (File.Exists(detailProject.FileName))
                    {
                        File.Delete(detailProject.FileName);
                    }
                }

                // 3 - supprimer le projet des données du projet
                BDDClient.Get().iDialogLocalData.Projects.Remove(this);
                //this.IsActive = false;

                BDDClient.Get().iDialogLocalData.SaveChanges();

                // 4 - Envoyer un message permettant de mettre à jour l'affichage
                Mvvm.Messaging.Messenger.Default.Send<CommandMessage>(new CommandMessage(this, "UpdateView"));
            }
        } // endMethod: ExecuteCommandDeleteProject

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandDeleteProject()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandDeleteProject 
        #endregion

        #region CommandPurgeObsoleteFile
        /// <summary>
        /// La commande PurgeObsoleteFile
        /// </summary>
        public ICommand CommandPurgeObsoleteFile
        {
            get;
            internal set;
        } // endProperty: CommandPurgeObsoleteFile

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandPurgeObsoleteFile()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandPurgeObsoleteFile = new Mvvm.Command.RelayCommand(ExecuteCommandPurgeObsoleteFile, CanExecuteCommandPurgeObsoleteFile);
        } // endMethod: CreateCommandPurgeObsoleteFile

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandPurgeObsoleteFile()
        {
            String message = LanguageSupport.Get().GetToolTip("Plg_ConnexionT/WARNING_DEL_OBSOLETE");
            // 1 - Prévenir l'utilisateur de la commande et de l'impossibilité de revenir en arrière
            if (System.Windows.MessageBox.Show(message, "", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
	        {
                for (int i = this.ProjectDetails.Count - 1; i >= 0; i--)
                {
                    ProjectDetail detailProject = this.ProjectDetails[i];
                    if (detailProject.Obsolete)
                    {
                        // 2 - Supprimer les références dans la base
                        BDDClient.Get().iDialogLocalData.ProjectDetail.Remove(detailProject);
                        // 3 - Supprimer les fichiers sur le disque
                        File.Delete(detailProject.FileName);
                        // 4 - Supprimer la référence dans les détails du project
                        this.ProjectDetails.Remove(detailProject);
                    }
                }

                BDDClient.Get().iDialogLocalData.SaveChanges();
                // 5 - Envoyer un message permettant de mettre à jour l'affichage
                Mvvm.Messaging.Messenger.Default.Send<CommandMessage>(new CommandMessage(this, "UpdateView"));
	        }
        } // endMethod: ExecuteCommandPurgeObsoleteFile

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandPurgeObsoleteFile()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandPurgeObsoleteFile 
        #endregion

        #endregion
    } // endClass: Projects
}
