using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace JAY.DAL
{
    /// <summary>
    /// Compare la base de données locale et distante
    /// </summary>
    public class CompareBDD
    {
        // Variables
        #region Variables

        #endregion

        // Propriétés
        #region Propriétés

        #endregion

        // Constructeur
        #region Constructeur

        public CompareBDD()
        {
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// iDialog est-il connecté à la BDD JAY ?
        /// CAD l'utilisateur est-il autentifié et iDialog a le droit de se connecter (l'utilisateur est d'accord)
        /// </summary>
        public BDDDistante.ConnectResult IsConnected ( )
        {
            BDDDistante.ConnectResult Result = BDDDistante.ConnectResult.AllFalse;

            if (DefaultValues.Get().SynchroBDDJay)
            {
                if (!DefaultValues.Get().IsConnectedForSynchro)
                {
                    if (DefaultValues.Get().KeepConnectionData)
                    {
                        Result = BDDDistante.BDDServeur.Get().VerifyAccessRights(DefaultValues.Get().UserID, DefaultValues.Get().Password);
                    }
                    // 1.2 - Non ? Alors demander un nom d'utilisateur et un mot de passe
                    if (Result != BDDDistante.ConnectResult.ConnectOK)
                    {
                        ConnectWindow ConnectW = new ConnectWindow();
                        if (ConnectW.ShowDialog().Value == true)
                        {
                            Result = BDDDistante.ConnectResult.ConnectOK;
                            DefaultValues.Get().IsConnectedForSynchro = true;
                        }
                    }
                }
                else
                {
                    Result = BDDDistante.ConnectResult.ConnectOK;
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Synchronisation with BDD Jay is disable", "Warning", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }

            return Result;
        } // endMethod: IsConnected

        /// <summary>
        /// Mettre à jour les bases de données locale et distante pour toutes les SIMs connus par iDialog
        /// </summary>
        public Boolean UpdateBDDs ( )
        {
            Boolean Result = true;
            Boolean IsAuthConnected = false;

            // 1 - vérifier si l'utilisateur est autentifié
            // 1.1 - Peut-on se connecter de façon automatique ?
            if (this.IsConnected() == BDDDistante.ConnectResult.ConnectOK)
            {
                IsAuthConnected = true;
            }

            // 2 - mettre à jour les bases de données

            if (IsAuthConnected)
            {
                var IdSIMS = from id in BDDClient.Get().iDialogLocalData.SIMProject
                             orderby id.Date descending
                             group id by id.IdSIM;

                foreach (var sim in IdSIMS)
                {
                    String IdSim = sim.Key;
                    String IdProduct;
                    SIMProject currentSIM = sim.First();
                    if (currentSIM.IdMO != null && currentSIM.IdMO != "")
                    {
                        IdProduct = currentSIM.IdMO;
                    }
                    else
                    {
                        IdProduct = currentSIM.IdMT;
                    }

                    Boolean r = this.UpdateBDDs(IdSim, IdProduct);
                    if (!r)
                    {
                        System.Windows.MessageBox.Show(String.Format("Erreur de la mise à jour des bases de données pour la SIM {0}", sim.Key));
                    }

                    Result = Result && r;
                } 
            }
            
            return Result;
        } // endMethod: UpdateBDDs

        /// <summary>
        /// Mettre à jour les bases de données locale et distante pour la SIM 'IdSIM'
        /// </summary>
        public Boolean UpdateBDDs ( String IdSIM, String IDProduct )
        {
            Boolean Result = false;
            if (this.IsConnected() == BDDDistante.ConnectResult.ConnectOK)
            {
                // Initialiser les collections de projets flashés pour la SIM dans la base locale et la base distante
                ObservableCollection<CommonSimProject> localSimProject;
                ObservableCollection<CommonSimProject> distanteSimProject;

                localSimProject = BDDClient.Get().GetSimProjects(IdSIM, IDProduct);
                distanteSimProject = BDDDistante.BDDServeur.Get().GetSimProject(IdSIM, IDProduct);

                // Si la base distante est accéssible
                if (distanteSimProject != null)
                {
                    // Construire la liste des projets SIM Locaux absents de la base distante
                    ObservableCollection<CommonSimProject> ProjectDistantAbsent = this.BuildListDistantAbsent(localSimProject, distanteSimProject);

                    // Construire la liste des projets SIM Distant absents de la base locale
                    ObservableCollection<CommonSimProject> ProjectLocauxAbsent = this.BuildListLocauxAbsent(localSimProject, distanteSimProject);

                    // Mettre à jour la base distante + le champs 'fichier'
                    this.MAJBDDDistante(ProjectDistantAbsent);

                    // Mettre à jour la base locale + les fichiers
                    this.MAJBDDLocale(ProjectLocauxAbsent);

                    Result = true;
                }
                else
                {
                    Result = false;
                }
            }

            return Result;
        } // endMethod: UpdateBDDs

        /// <summary>
        /// Mettre à jour la base de données distante
        /// </summary>
        private Boolean MAJBDDDistante ( ObservableCollection<CommonSimProject> projectDistantAbsent )
        {
            Boolean Result = false;

            foreach (var SimPrj in projectDistantAbsent)
            {
                // 1 - Construire la ligne à insérer dans la base distante
                ServiceData.SimPROJECT SIMDistante = new ServiceData.SimPROJECT();
                SIMProject SIMLocale = BDDClient.Get().GetSimProject(SimPrj.IDSim, SimPrj.Date);

                if (SIMLocale != null)
                {
                    SIMDistante.Date = SIMLocale.Date;
                    SIMDistante.Evolution = SIMLocale.Evolution;
                    SIMDistante.IdMO = SIMLocale.IdMO;
                    SIMDistante.IdMT = SIMLocale.IdMT;
                    SIMDistante.IdProject = SIMLocale.IdProject;
                    SIMDistante.IdSIM = SIMLocale.IdSIM;
                    SIMDistante.MOType = SIMLocale.MOType;
                    SIMDistante.MTType = SIMLocale.MTType;
                    SIMDistante.NomIDialog = SIMLocale.NomIDialog;
                    SIMDistante.Obsolete = SIMLocale.Obsolete;
                    SIMDistante.Version = SIMLocale.Version;
                    SIMDistante.Hash = SIMLocale.Hash;

                    // 1.5 - Obtenir les données du fichier et les transmettre à la BDD distante
                    String fileName = String.Format("{0}{1}\\{2}.iDialog", DefaultValues.Get().FlashFilesFolder, SIMLocale.IdSIM, SIMLocale.Hash);

                    if (File.Exists(fileName))
                    {
                        BDDDistante.BDDServeur.Get().UploadSimProject(SIMDistante, fileName);
                    }
                }
            }

            return Result;
        } // endMethod: MAJBDDDistante
        
        /// <summary>
        /// Mettre à jour la base de données locale
        /// </summary>
        private Boolean MAJBDDLocale ( ObservableCollection<CommonSimProject> projectLocalAbsent )
        {
            Boolean Result = false;

            foreach (var SimPrjServer in projectLocalAbsent)
            {
                // 1 - Construire la ligne à insérer dans le base locale
                ServiceData.SimPROJECT SIMDistante = BDDDistante.BDDServeur.Get().GetSimProject(SimPrjServer.IDSim, SimPrjServer.Date);

                SIMProject SIMLocale = new SIMProject();
                SIMLocale.Date = SIMDistante.Date.Value;
                SIMLocale.Evolution = SIMDistante.Evolution;
                SIMLocale.IdMO = SIMDistante.IdMO;
                SIMLocale.IdMT = SIMDistante.IdMT;
                if (SIMDistante.IdProject != null)
                {
                    SIMLocale.IdProject = SIMDistante.IdProject.Value;
                }
                else
                {
                    SIMLocale.IdProject = 0;
                }
                SIMLocale.IdSIM = SIMDistante.IdSIM;
                SIMLocale.MOType = SIMDistante.MOType;
                SIMLocale.MTType = SIMDistante.MTType;
                SIMLocale.NomIDialog = SIMDistante.NomIDialog;
                SIMLocale.Obsolete = SIMDistante.Obsolete.Value;
                SIMLocale.Version = SIMDistante.Version.Value;
                SIMLocale.Hash = SIMDistante.Hash;
                
                // 2 - Obtenir le nom de fichier et le transmettre au gestionnaire de BDD Local
                // 2.1 - Si le directory n'existe pas, le créer
                String directoryPath = DefaultValues.Get().FlashFilesFolder + SIMLocale.IdSIM;
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                String DestFileName = BDDClient.Get().GetSimProjectFileName(SIMLocale);
                BDDDistante.BDDServeur.Get().DownLoadSimProject(SIMDistante, DestFileName);

                // 3 - Inseérer l'entrée dans la base locale
                BDDClient.Get().iDialogLocalData.SIMProject.Add(SIMLocale);
                BDDClient.Get().iDialogLocalData.SaveChanges();
            }

            return Result;
        } // endMethod: MAJBDDLocale

        /// <summary>
        /// Construire une collection comportant la liste des SIMProject présents sur la base distante, mais non sur la base locale
        /// </summary>
        private ObservableCollection<CommonSimProject> BuildListLocauxAbsent ( ObservableCollection<CommonSimProject> localSimProject, ObservableCollection<CommonSimProject> distantSimProject )
        {
            ObservableCollection<CommonSimProject> Result = this.BuildListAbsentBDD1(localSimProject, distantSimProject);
            
            return Result;
        } // endMethod: BuildListLocauxAbsent
        
        /// <summary>
        /// Construire une collection comportant la liste des SIMProject présents sur la base locale, mais non sur la base distante
        /// </summary>
        private ObservableCollection<CommonSimProject> BuildListDistantAbsent ( ObservableCollection<CommonSimProject> localSimProject, ObservableCollection<CommonSimProject> distantSimProject )
        {
            ObservableCollection<CommonSimProject> Result = this.BuildListAbsentBDD1(distantSimProject, localSimProject);
            
            return Result;
        } // endMethod: BuildListDistantAbsent

        /// <summary>
        /// Construire une liste d'objet présent dans la BDD2 mais absent dans la BDD1
        /// </summary>
        private ObservableCollection<CommonSimProject> BuildListAbsentBDD1 ( ObservableCollection<CommonSimProject> BDD1, ObservableCollection<CommonSimProject> BDD2 )
        {
            ObservableCollection<CommonSimProject> Result = new ObservableCollection<CommonSimProject>();

            foreach (CommonSimProject item in BDD2)
            {
                var query = from element in BDD1
                            where element.IDSim == item.IDSim && element.Date == item.Date
                            select element;

                if (query.Count() == 0)
                {
                    Result.Add(item);
                }
            }

            return Result;
        } // endMethod: BuildListAbsentBDD1

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: CompareBDD
}
