using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.IO.Packaging;
using System.Xml;
using System.Xml.Linq;
using System.Text;
//using System.Data.EntityClient;
using JAY.FileCore;
using JAY.PegaseCore;
using System.Windows;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Data;
//using Microsoft.Synchronization.Data.Server;
//using Microsoft.Synchronization.Data.SqlServerCe;
using JAY.DAL;
using System.ComponentModel;
using Messenger = GalaSoft.MvvmLight.Messaging.Messenger;
using System.Data.Entity.Core.EntityClient;
using System.Configuration;

namespace JAY.DAL
{
    /// <summary>
    /// Objet unique permettant de gérer les données
    /// </summary>

    public class BDDClient
    {
        // Constantes
        #region constantes

        private const String ROOT = "Root";
        private const String PROJECTS = "Projects";
        private const String DETAIL_PROJECTS = "DetailProjects";
        private const String DETAIL_PROJECT = "DetailProject";
        private const String PROJECT = "Project";
        private const String PROJECTNAME = "ProjectName";
        private const String USER_PROJECTNAME = "UserProjectName";
        private const String MOTYPE = "MOType";
        private const String MTTYPE = "MTType";
        private const String IDPROJECT = "IdProject";
        private const String VERSION = "Version";
        private const String NOMIDIALOG = "NomIDialog";
        private const String OBSOLETE = "Obsolete";
        private const String FOLDER_FILES = "files";
        private const String TYPEMIME_IDIALOG = "JAY/iDialog";
        private const String DATE = "Date";
        private const String EVOLUTION = "Evolution";

        #endregion

        // Variables singleton

        // Variables
        #region Variables

         private iDialogDataEntities _iDialogLocalData;
       // private EntityConnection _iDialogLocalData;
        private static BDDClient _instance;
        static readonly object instanceLock = new object();

        // Variables utilisées pour effectuer l'importation de fichiers iDialog en asynchrone
        private ObservableCollection<String> _files;
        private iDialogPackage _package;
        private String _iDialogFile;
        private Int32 _iDProject;
        private ImportWindow _importWindow;
        private Boolean _autoOpenImportedProject;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Les données local iDialog (base de traçabilité)
        /// </summary>
        public iDialogDataEntities iDialogLocalData
        {
            get
            {
                if (this._iDialogLocalData == null)
                {
                    // entity frmawork 4
                    // EntityConnectionStringBuilder Connection = new EntityConnectionStringBuilder(String.Format(@"metadata=res://*/iDialogData.csdl|res://*/iDialogData.ssdl|res://*/iDialogData.msl;provider=System.Data.SqlServerCe.4.0;provider connection string='Data Source={0}iDialogData.sdf;Password=iDialog;Persist Security Info=True'", DefaultValues.Get().LocalDataFolder));
                    // entity framework 6
                    //String Connection = String.Format("metadata=res://*/iDialogData.csdl|res://*/iDialogData.ssdl|res://*/iDialogData.msl;provider=System.Data.SqlServerCe.4.0;provider connection string='Data Source={0}iDialogData.sdf;Password=iDialog;Persist Security Info=True';", DefaultValues.Get().LocalDataFolder);
                    //Connection = "metadata=res://*/iDialogData.csdl|res://*/iDialogData.ssdl|res://*/iDialogData.msl;provider=System.Data.SqlServerCe.4.0;provider connection string='Data Source=C:\\ProgramData\\Jay Electronique\\iDialog\\LocalData\\iDialogData.sdf;Password=iDialog';";
                    ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;
                    try
                    {
                        // this._iDialogLocalData = new iDialogDataEntities(Connection);
                          this._iDialogLocalData = new iDialogDataEntities();
                        //this._iDialogLocalData.= Connection.ToString();

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }
                return this._iDialogLocalData;
            }
        } // endProperty: iDialogLocalData

        #endregion

        // Constructeur
        #region Constructeur

        private BDDClient()
        {
            this._autoOpenImportedProject = false;
        }

        #endregion

        // Méthodes
        #region Méthodes

        // Retourne une instance unique de la classe
        public static BDDClient Get()
        {
            lock (instanceLock)
            {
                if (_instance == null)
                    _instance = new BDDClient();


                return _instance;
            }
        }
        
        /// <summary>
        /// Synchroniser la BDDClient distante dans le sens client -> serveur
        /// </summary>
        public void SynchroniseUPLOAD ( )
        {
            
        } // endMethod: SynchroniseUPLOAD
        
        /// <summary>
        /// Synchroniser la base de données dans le sens serveur -> client
        /// </summary>
        public void SynchroniseDOWNLOAD ( )
        {

        } // endMethod: SynchroniseDOWNLOAD

        /// <summary>
        /// Importer des fichiers iDialogs
        /// </summary>
        /// <returns>retourne l'IDProject de la base</returns>
        public Int32 ImportiDialogFiles ( String iDialogFile, String MOType, String MTType, Boolean AutoOpen )
        {
            Int32 Result = -1; // retour correspondant à un échec de l'importation
            this._iDialogFile = iDialogFile;
            this._autoOpenImportedProject = AutoOpen;

            // 2 - Créer le projet
            Projects prj = new Projects();
            prj.MOType = MOType;
            prj.MTType = MTType;
            prj.ProjectName = Path.GetFileNameWithoutExtension(_iDialogFile);
            prj.UserProjectName = prj.ProjectName;
            prj.IsActive = true;

            // 2.2 - Vérifier si le projet existe déjà. Si oui, demandé si les données doivent être remplacées
            var QueryProjectExist = from project in BDDClient.Get().iDialogLocalData.Projects
                                    where project.ProjectName == prj.ProjectName
                                    select project;

            if (QueryProjectExist.Count() > 0)
            {
                String Message = String.Format(LanguageSupport.Get().GetToolTip("Plg_ConnexionT/PROJECT_EXIST"), prj.ProjectName);
                if (MessageBox.Show(Message, "", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    // ne rien enregistrer dans la base
                }
                else
                {
                    // ne pas importer le projet
                    return Result;
                }
            }
            else
            {
                this.iDialogLocalData.Projects.Add(prj);
                this.iDialogLocalData.SaveChanges();
            }

            // 3 - Récupérer l'ID de la ligne

            String PName = Path.GetFileNameWithoutExtension(_iDialogFile);
            IEnumerable<Int32> prjs = from project in this.iDialogLocalData.Projects
                                         where project.ProjectName == PName
                                         select project.Id;

            if (prjs.Count() > 0)
            {
                this._iDProject = prjs.First();
                Result = this._iDProject;
            }

            // 4 - Fabriquer les fichiers .idialog correspondant aux différentes versions et remplir la base -> Lancer la tâche asynchrone
            //     Et la fenêtre de suivi de la progression
            BackgroundWorker importWorker;
            this._importWindow = new ImportWindow();
            this._importWindow.Show();

            importWorker = new BackgroundWorker();
            importWorker.DoWork += new DoWorkEventHandler(importWorker_DoWork);
            importWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(importWorker_RunWorkerCompleted);
            importWorker.ProgressChanged += new ProgressChangedEventHandler(importWorker_ProgressChanged);
            importWorker.WorkerReportsProgress = true;
            importWorker.RunWorkerAsync();

            return Result;
        }

        void importWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BDDLocal.ViewModelImportWindow vmIW = this._importWindow.DataContext as BDDLocal.ViewModelImportWindow;
            vmIW.Progress = e.ProgressPercentage;
        }

        void importWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker importWorker = sender as BackgroundWorker;
            Int32 i = 0;

            foreach (String file in this._files)
            {
                String fileWE = System.IO.Path.GetFileNameWithoutExtension(file);
                Projects prj = BDDClient.Get().GetProject(this._iDProject);
                ProjectDetail prjD = new ProjectDetail();

                prjD.IdProject = this._iDProject;
                prjD.NomIDialog = prj.ProjectName;
                prjD.Obsolete = false;
                prjD.Version = i;
                prjD.Evolution = "root";
                prjD.DateCreation = DateTime.Now;
                i++;

                // si le nom de fichier existe déjà pour ce projet, ne pas le ré-inscrire dans la base
                var QueryExistDetail = from detail in this.iDialogLocalData.ProjectDetail
                                       where detail.IdProject == this._iDProject && detail.NomIDialog == fileWE
                                       select detail;

                if (QueryExistDetail.Count() == 0)
                {
                    this.iDialogLocalData.ProjectDetail.Add(prjD);
                }
            }

            this.iDialogLocalData.SaveChanges();
            this._package.ClosePackage();

            // Fermer la fenêtre
            BDDLocal.ViewModelImportWindow vmIW = this._importWindow.DataContext as BDDLocal.ViewModelImportWindow;
            vmIW.Progress = 100;

            this._importWindow.Close();

            // Si la lecture automatique est activée, charger le fichier
            if (this._autoOpenImportedProject)
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<CommandMessage>(new CommandMessage(this, PegaseCore.Commands.CMD_LOAD));
            }

            // Envoyer un message spécifiant que l'import est terminé
            Messenger.Default.Send<CommandMessage>(new CommandMessage(this, Commands.CMD_WAIT_OFF));
        }

        void importWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            String DestFolder;
            BackgroundWorker importWorker = sender as BackgroundWorker;

            DestFolder = String.Format(@"{0}{1}\", DefaultValues.Get().iDialogFileFolder, System.IO.Path.GetFileNameWithoutExtension(this._iDialogFile));
            this._package = iDialogPackage.OpenPackage(this._iDialogFile, FileMode.Open);
            this._files = this._package.ExplodeOldIDialogPackage(this._iDialogFile, DestFolder, importWorker);
        } // endMethod: ImportiDialogFiles

        /// <summary>
        /// Importer une archive iDialog
        /// </summary>
        public UInt32 ImportiDialogArchive ( String iDialogArchives )
        {
            UInt32 Result = ErrorCode.NO_ERROR;

            ZipPackage package = (ZipPackage)ZipPackage.Open(iDialogArchives, FileMode.Open);
            
            // 1 - restaurer les fichiers iDialog
            // si un fichier existe déjà, il sera écrasé impitoyablement
            var parts = package.GetParts();

            foreach (var part in parts)
            {
                if (part.ContentType == TYPEMIME_IDIALOG)
                {
                    this.RestaurePart((ZipPackagePart)part);
                }
            }

            // 2 - insérer les données de la base de données
            ZipPackagePart data = (ZipPackagePart)package.GetPart(new Uri("/Data.xml", UriKind.Relative));
            this.RestaureData(data);

            return Result;
        } // endMethod: ImportiDialogArchive
        
        /// <summary>
        /// Restaurer les données d'une archive dans la base
        /// Si une donnée existe déjà, elle est écrasée
        /// </summary>
        private void RestaureData ( ZipPackagePart dataPart )
        {
            Stream dataStream = dataPart.GetStream();
            XDocument doc = XDocument.Load(dataStream);
            XElement XRoot = doc.Root;
            XAttribute Attrib;

            // 1 - traiter les données projets
            foreach (XElement project in XRoot.Descendants(PROJECTS).First().Descendants(PROJECT))
            {
                Attrib = project.Attribute(PROJECTNAME);
                Projects currentProject;

                // 1.1 - vérifier si le projet existe déjà
                var queryProject = from prj in this.iDialogLocalData.Projects
                                   where prj.ProjectName == Attrib.Value
                                   select prj;

                if (queryProject.Count() > 0)
                {
                    currentProject = queryProject.First();
                }
                else
                {
                    // Inserer la donnée dans la base
                    currentProject = new Projects();
                    currentProject.ProjectName = FileCore.iDialogPackageBase.RemoveDiacritics(Attrib.Value);
                    currentProject.MOType = "";
                    currentProject.MTType = "";
                    currentProject.UserProjectName = Attrib.Value;
                    currentProject.IsActive = true;
                    this.iDialogLocalData.Projects.Add(currentProject);
                    this.iDialogLocalData.SaveChanges();
                    String requiredPrj = FileCore.iDialogPackageBase.RemoveDiacritics(Attrib.Value);

                    var queryCurrentProject = from prj in this.iDialogLocalData.Projects
                                              where prj.ProjectName == requiredPrj
                                              select prj;

                    currentProject = queryCurrentProject.First();
                }

                Attrib = project.Attribute(USER_PROJECTNAME);
                currentProject.UserProjectName = Attrib.Value;

                Attrib = project.Attribute(MOTYPE);
                currentProject.MOType = Attrib.Value;

                Attrib = project.Attribute(MTTYPE);
                currentProject.MTType = Attrib.Value;

                // 2 - traiter les données projet détail
                foreach (XElement detailProject in project.Descendants(DETAIL_PROJECT))
                {
                    Attrib = detailProject.Attribute(NOMIDIALOG);
                    ProjectDetail currentProjectDetail;

                    Int32 Version = Convert.ToInt32(detailProject.Attribute(VERSION).Value);

                    // Vérifier si le détail du projet existe déjà
                    var queryDetailProject = from dProject in this.iDialogLocalData.ProjectDetail
                                             where dProject.NomIDialog == Attrib.Value && dProject.Version == Version
                                             select dProject;

                    if (queryDetailProject.Count() > 0)
                    {
                        currentProjectDetail = queryDetailProject.First();
                    }
                    else
                    {
                        // Inserer la donnée dans la base
                        currentProjectDetail = new ProjectDetail();
                        currentProjectDetail.NomIDialog = FileCore.iDialogPackageBase.RemoveDiacritics(Attrib.Value);
                        this.iDialogLocalData.ProjectDetail.Add(currentProjectDetail);
                    }

                    Attrib = detailProject.Attribute(DATE);
                    currentProjectDetail.DateCreation = DateTime.Parse(Attrib.Value);

                    Attrib = detailProject.Attribute(IDPROJECT);
                    currentProjectDetail.IdProject = currentProject.Id;

                    Attrib = detailProject.Attribute(VERSION);
                    currentProjectDetail.Version = Convert.ToInt32(Attrib.Value);

                    Attrib = detailProject.Attribute(EVOLUTION);
                    currentProjectDetail.Evolution = Attrib.Value;

                    Attrib = detailProject.Attribute(OBSOLETE);
                    currentProjectDetail.Obsolete = Convert.ToBoolean(Attrib.Value);
                }
            }

            this.iDialogLocalData.SaveChanges();
        } // endMethod: RestaureData

        /// <summary>
        /// Restaurer la partie 
        /// </summary>
        private void RestaurePart ( ZipPackagePart part )
        {
            // 1 - vérifier si le fichier existe déjà.si oui, le supprimer (avertir?)
            String DestFileName = part.Uri.OriginalString;
            DestFileName = DestFileName.Remove(0, FOLDER_FILES.Length + 2);
            String ProjectFolder = DefaultValues.Get().iDialogFileFolder + DestFileName.Remove(DestFileName.IndexOf('/'));
            DestFileName = DestFileName.Replace('/', '\\');
            DestFileName = DefaultValues.Get().iDialogFileFolder + DestFileName;

            if (File.Exists(DestFileName))
            {
                File.Delete(DestFileName);
            }

            if (!Directory.Exists(ProjectFolder))
            {
                Directory.CreateDirectory(ProjectFolder);
            }

            // 2 - copier le fichier 'part' dans le répertoire de destination
            Stream sourceStream = part.GetStream();
            Byte[] fileBuffer;

            using (BinaryReader BR = new BinaryReader(sourceStream))
            {
                using (BinaryWriter BW = new BinaryWriter(File.OpenWrite(DestFileName)))
                {
                    Int32 ChunkSize = 1024 * 32;

                    fileBuffer = BR.ReadBytes(ChunkSize);
                    BW.Write(fileBuffer);
                    BW.Flush();

                    while (fileBuffer.Length == ChunkSize)
                    {
                        fileBuffer = BR.ReadBytes(ChunkSize);
                        BW.Write(fileBuffer);
                        BW.Flush();
                    }
                }
            }
        } // endMethod: RestaurePart

        /// <summary>
        /// Exporter une archive iDialog
        /// </summary>
        public UInt32 ExportiDialogArchive ( String iDialogArchives )
        {
            UInt32 Result = ErrorCode.NO_ERROR;
            
            // Créer l'archive d'export
            ZipPackage ZP = (ZipPackage)ZipPackage.Open(iDialogArchives, FileMode.Create);

            // 1 - pour tous les projets sélectionnés -> exporter les données dans l'archive principale
            XDocument doc = new XDocument();

            XElement XRoot = this.BuildXMLExport();
            doc.Add(XRoot);
            
            // 1.3 - Enregistrer la partie XML
            ZipPackagePart zipPart = (ZipPackagePart)ZP.CreatePart(new Uri("/Data.xml", UriKind.Relative), System.Net.Mime.MediaTypeNames.Text.Xml, CompressionOption.Normal);
            Stream streamDataPart = zipPart.GetStream();

            doc.Save(streamDataPart);

            // 2 - exporter les fichiers iDialog
            foreach (var project in BDDClient.Get().iDialogLocalData.Projects)
            {
                if (project.IsSelected)
                {
                    this.ExportSelectedProject(ZP, project);
                }
            }

            ZP.Close();

            return Result;
        } // endMethod: ExportiDialogArchive
        
        /// <summary>
        /// Construire la partie "projects" lors de l'export
        /// </summary>
        private XElement BuildXMLExport( )
        {
            XElement Result = new XElement(ROOT);
            XElement XProjects = new XElement(PROJECTS);

            foreach (var project in this.iDialogLocalData.Projects)
            {
                if (project.IsSelected)
                {
                    XElement Xproject = new XElement(PROJECT);

                    XAttribute Attrib = new XAttribute(PROJECTNAME, project.ProjectName);
                    Xproject.Add(Attrib);

                    Attrib = new XAttribute(USER_PROJECTNAME, project.UserProjectName);
                    Xproject.Add(Attrib);

                    Attrib = new XAttribute(MOTYPE, project.MOType);
                    Xproject.Add(Attrib);

                    Attrib = new XAttribute(MTTYPE, project.MTType);
                    Xproject.Add(Attrib);

                    XProjects.Add(Xproject);

                    var querySubProject = from subProject in this.iDialogLocalData.ProjectDetail
                                          where subProject.IdProject == project.Id
                                          select subProject;

                    if (querySubProject.Count() > 0)
                    {
                        foreach (var subProject in querySubProject)
                        {
                            XElement XSubProject = new XElement(DETAIL_PROJECT);

                            Attrib = new XAttribute(IDPROJECT, subProject.IdProject);
                            XSubProject.Add(Attrib);

                            Attrib = new XAttribute(VERSION, subProject.Version);
                            XSubProject.Add(Attrib);

                            Attrib = new XAttribute(EVOLUTION, subProject.Evolution);
                            XSubProject.Add(Attrib);

                            Attrib = new XAttribute(NOMIDIALOG, subProject.NomIDialog);
                            XSubProject.Add(Attrib);

                            Attrib = new XAttribute(OBSOLETE, subProject.Obsolete);
                            XSubProject.Add(Attrib);

                            Attrib = new XAttribute(DATE, subProject.DateCreation);
                            XSubProject.Add(Attrib);

                            Xproject.Add(XSubProject); 
                        }
                    }
                }
            }
            Result.Add(XProjects);

            return Result;
        } // endMethod: BuildProjectsExport

        /// <summary>
        /// Exporter les fichiers iDialog projet correspondant au projet, dans l'archive package
        /// </summary>
        private void ExportSelectedProject ( ZipPackage package, Projects project )
        {
            // 1 - retourver tous les fichiers iDialog liés au projet
            var querySubProject = from subProject in BDDClient.Get().iDialogLocalData.ProjectDetail
                                  where subProject.IdProject == project.Id
                                  select subProject;

            // 2 - pour tous les fichiers, les enregistrer dans l'archive
            if (querySubProject.Count() > 0)
            {
                foreach (var subProject in querySubProject)
                {
                    Uri destUri = new Uri(String.Format("/{0}/{1}/{2}_{3:0000}.idialog", FOLDER_FILES, iDialogPackageBase.RemoveDiacritics(project.ProjectName), iDialogPackageBase.RemoveDiacritics(subProject.NomIDialog), subProject.Version), UriKind.Relative);
                    try
                    {
                        ZipPackagePart zipPart = (ZipPackagePart)package.CreatePart(destUri, TYPEMIME_IDIALOG, CompressionOption.NotCompressed);
                        Stream stream = zipPart.GetStream();

                        String SourceFileName = String.Format("{0}\\{1}\\{2}_{3:0000}.idialog", DefaultValues.Get().iDialogFileFolder, project.ProjectName, subProject.NomIDialog, subProject.Version);

                        using (BinaryWriter BW = new BinaryWriter(stream))
                        {
                            Byte[] buffer = File.ReadAllBytes(SourceFileName);
                            BW.Write(buffer);
                            BW.Flush();
                        }
                    }
                    catch
                    {

                    }
                }
            }
        } // endMethod: ExportSelectedProject

        /// <summary>
        /// Acquérir le nom du fichier iDialog correspondant au nom du projet et à la version
        /// </summary>
        public String GetIDialogFilename ( Int32 IDProject, Int32 FileVersion )
        {
            String Result = "";

            var QueryProject = from project in BDDClient.Get().iDialogLocalData.Projects
                               where project.Id == IDProject
                               select project;

            if (QueryProject.Count() > 0)
            {
                Projects projet = QueryProject.First();

                var QueryProjectDetail = from projetDetail in projet.ProjectDetails
                                         where projetDetail.Version == FileVersion
                                         select projetDetail;

                if (QueryProjectDetail.Count() > 0)
                {
                    ProjectDetail projectDetail = QueryProjectDetail.First();
                    Result = String.Format("{0}_{1:0000}",projectDetail.NomIDialog, projectDetail.Version);
                }
            }
            
            return Result;
        } // endMethod: GetIDialogFilename
        
        /// <summary>
        /// Acquérir la dernière version de fichier pour le projet 'ProjectName'
        /// </summary>
        public Int32 GetLastVersion ( Int32 IDProject )
        {
            Int32 Result = -1;

            var QueryProject = from project in BDDClient.Get().iDialogLocalData.Projects
                               where project.Id == IDProject
                               select project;

            if (QueryProject.Count() > 0)
            {
                Projects projet = QueryProject.First();
                Result = projet.GetLastVersion();
            }

            return Result;
        } // endMethod: GetLastVersion

        /// <summary>
        /// Définir la visibilité des obsolètes et des lignes de détail A Jour
        /// </summary>
        public void SetDetailVisibility ( Boolean Obsolete, Boolean AJ )
        {
            foreach (ProjectDetail projectDetail in this.iDialogLocalData.ProjectDetail)
            {
                projectDetail.SetVisibility(Obsolete, AJ);
            }
        } // endMethod: SetDetailVisibility
        
        /// <summary>
        /// Copier le projet simProject vers le projet d'origine (à la dernière version)
        /// </summary>
        /// <param name="simProject">Le projet SIM à copier</param>
        /// <returns>Le détail du projet nouvellement créé</returns>
        public ProjectDetail CopyProjectFromSIMProject2Project ( SIMProject simProject )
        {
            ProjectDetail Result = null;

            Projects prj = null;
            // 1 - Trouver le projet d'origine
            String ProjectName = simProject.NomIDialog;
            prj = this.GetProject(ProjectName);

            // 1.5 - Si le projet d'origine a été supprimer, le créer
            if (prj == null)
            {
                // Inserer la donnée dans la base
                prj = new Projects();
                prj.ProjectName = simProject.NomIDialog;
                prj.MOType = simProject.MOType;
                prj.MTType = simProject.MTType;
                prj.UserProjectName = simProject.IdSIM;
                prj.IsActive = true;
                this.iDialogLocalData.Projects.Add(prj);
                this.iDialogLocalData.SaveChanges();

                var queryCurrentProject = from project in this.iDialogLocalData.Projects
                                          where project.ProjectName.Contains(ProjectName)
                                          orderby project.Id descending
                                          select project;

                prj = queryCurrentProject.First();
                simProject.IdProject = prj.Id;
                this.iDialogLocalData.SaveChanges();

                String ProjectPath = String.Format("{0}{1}", DefaultValues.Get().iDialogFileFolder, prj.ProjectName);
                if (!Directory.Exists(ProjectPath))
                {
                    Directory.CreateDirectory(ProjectPath);
                }
            }

            // 2 - duppliquer le fichier
            Int32 nextVersion = prj.GetLastVersion() + 1;
            String SourceFile = this.GetSimProjectFileName(simProject);
            String NomIDialog = prj.ProjectName;
            String DestFile = String.Format("{0}{1}\\{2}_{3:0000}.idialog", DefaultValues.Get().iDialogFileFolder, prj.ProjectName, NomIDialog, nextVersion);

            if (File.Exists(DestFile))
            {
                File.Delete(DestFile);
            }

            File.Copy(SourceFile, DestFile);

            // 3 - Ajouter le détail de projet correspondant
            ProjectDetail prjDetail = new ProjectDetail();
            prjDetail.IdProject = prj.Id;
            prjDetail.Version = nextVersion;
            prjDetail.Evolution = "FromFlash";
            prjDetail.NomIDialog = prj.ProjectName;
            prjDetail.Obsolete = false;
            prjDetail.DateCreation = DateTime.Now;

            this.iDialogLocalData.ProjectDetail.Add(prjDetail);
            this.iDialogLocalData.SaveChanges();

            Result = prjDetail;

            return Result;
        } // endMethod: CopyProjectFromSIMProject2Project
        
        /// <summary>
        /// Obtenir le nom de fichier associé à ce projet SIM
        /// </summary>
        public String GetSimProjectFileName ( SIMProject simProject )
        {
            String Result = String.Format("{0}{1}\\{2}.idialog", DefaultValues.Get().FlashFilesFolder, simProject.IdSIM, simProject.Hash);
            
            return Result;
        } // endMethod: GetSimProjectFileName

        /// <summary>
        /// Duppliquer le projet vers la dernière version
        /// </summary>
        public ProjectDetail DuplicateProjectToLastVersion ( ProjectDetail PrjD )
        {
            ProjectDetail Result = PrjD;

            String SourceFile;
            String DestFile;
            Int32 nextVersion;

            Projects Prj = PrjD.GetParentProject();
            nextVersion = Prj.GetLastVersion() + 1;

            // Si le fichier projet est une version antérieur de la dernière version, le dupliquer et 

            if (PrjD.Version < Prj.GetLastVersion())
            {
                SourceFile = String.Format("{0}{1}\\{2}.idialog", DefaultValues.Get().iDialogFileFolder, PrjD.GetParentProject().ProjectName, PrjD.NomIDialog);
                String NomIDialog = String.Format("{0}_{1:0000}", Prj.ProjectName, nextVersion);
                DestFile = String.Format("{0}{1}\\{2}.idialog", DefaultValues.Get().iDialogFileFolder, Prj.ProjectName, NomIDialog);
                File.Copy(SourceFile, DestFile);

                // Enregistrer les données dans la base
                ProjectDetail prjDetail = new ProjectDetail();
                prjDetail.IdProject = PrjD.IdProject;
                prjDetail.Version = nextVersion;
                prjDetail.Evolution = "FromFlash";
                prjDetail.NomIDialog = NomIDialog;
                prjDetail.Obsolete = false;
                prjDetail.DateCreation = DateTime.Now;

                this.iDialogLocalData.ProjectDetail.Add(prjDetail);
                this.iDialogLocalData.SaveChanges();

                Result = prjDetail;
            }

            return Result;
        } // endMethod: DuplicateProjectToLastVersion

        /// <summary>
        /// Sauver le projet sous une nouvelle version
        /// </summary>
        public ProjectDetail SaveProjectNewVersion ( Int32 IDProject, String Hash, String XML, out String NextFileName, String currentShortFileName, String MOType, String MTType )
        {
            ProjectDetail Result = null;

            // 1 - trouver le numero de version maximum actuel du projet
            Projects prj = this.GetProject(IDProject);
            Int32 currentVersion = this.GetLastVersion(IDProject);
            String CurrentFilename = String.Format("{0}{1}\\{2}.idialog", DefaultValues.Get().iDialogFileFolder, prj.ProjectName, currentShortFileName);

            // 1.5 - trouver le nom du prochain fichier
            String[] parts;
            String nextShortFileName;
            String ShortFileName = "";

            parts = currentShortFileName.Split(new Char[] { '_' });
            if (parts.Length == 0)
            {
                ShortFileName = "Default";
            }
            else if (parts.Length == 1)
            {
                ShortFileName = parts[0];
            }
            else
            {
                ShortFileName = parts[0];

                for (int i = 1; i < parts.Length - 1; i++)
                {
                    ShortFileName += "_" + parts[i];
                }
            }

            currentVersion++;
            nextShortFileName = String.Format("{0}_{1:0000}", ShortFileName, currentVersion);

            // 2 - enregistrer le fichier sous une nouvelle version
            NextFileName = String.Format("{0}{1}\\{2}.idialog", DefaultValues.Get().iDialogFileFolder, prj.ProjectName, nextShortFileName);
            if (File.Exists(NextFileName))
            {
                currentVersion++;
                nextShortFileName = String.Format("{0}_{1:0000}", ShortFileName, currentVersion);
                NextFileName = String.Format("{0}{1}\\{2}.idialog", DefaultValues.Get().iDialogFileFolder, prj.ProjectName, nextShortFileName);
            }
            if (File.Exists(NextFileName))
            {
                File.Delete(NextFileName);
            }
            File.Copy(CurrentFilename, NextFileName);

            iDialogPackage package = iDialogPackage.OpenPackage(NextFileName, FileMode.Open);
            if (package.SaveCurrentVersion(XML))
            {
                // 3 - compléter la base de données
                Projects currentProject = this.GetProject(IDProject);
                Result = new ProjectDetail();
                Result.DateCreation = DateTime.Now;
                Result.IdProject = currentProject.Id;
                Result.NomIDialog = ShortFileName;
                Result.Version = currentVersion;
                Result.Obsolete = false;
                Result.Evolution = "SubVersion";
                Result.Hash = Hash;

                // 4 - mettre à jour le type d'équipements
                currentProject.MOType = MOType;
                currentProject.MTType = MTType;

                currentProject.ProjectDetails.Add(Result);
                this.iDialogLocalData.ProjectDetail.Add(Result);
                this.iDialogLocalData.SaveChanges();
            }
            package.ClosePackage();

            return Result;
        } // endMethod: SaveProjectNewVersion
        
        /// <summary>
        /// Enregistrer dans la base le flashage d'une configuration dans un produit
        /// Lors d'un flashage, une nouvelle trace est toujours enregistrée, même si la configuration est flashée deux fois de suite sans modification
        /// </summary>
        /// <param name="IDProject">L'ID du projet dans la base locale</param>
        /// <param name="IdMO">Le numéro de série du MO connecté, il est fournit pour être stocké</param>
        /// <param name="IdMT">Le numéro de série du MT connecté, il est fournit pour être stocké</param>
        /// <param name="SerialNumSim">Le numéro de série de la SIM utilisé pour la traçabilité, il est fournit pour être stocké</param>
        /// <param name="Version">Le numéro de version du projet flashé. Si la valeur -1 est transmise, la dernière version sera prise en compte</param>
        public void FlashConfig ( Int32 IDProject, String IdMO, String IdMT, String SerialNumSim, Int32 Version, String Hashage )
        {
            SIMProject simProject = new SIMProject();

            if (IdMO == null)
            {
                IdMO = "";
            }
            else if (IdMO.Count() > 10)
            {
                IdMO = IdMO.Substring(0, 10);
            }
            if (IdMT == null)
            {
                IdMT = "";
            }
            else if (IdMT.Length > 10)
            {
                IdMT = IdMT.Substring(0, 10);
            }
            if (SerialNumSim == null)
            {
                SerialNumSim = "";
            }
            else if (SerialNumSim.Length > 10)
            {
                SerialNumSim = SerialNumSim.Substring(0, 10);
            }

            Projects prj = BDDClient.Get().GetProject(IDProject);

            if (Version < 0)
            {
                Version = prj.GetLastVersion();
            }

            Projects project = BDDClient.Get().GetProject(IDProject);

            simProject.IdProject = IDProject;
            simProject.IdMO = IdMO;
            simProject.IdMT = IdMT;
            simProject.IdSIM = SerialNumSim;
            simProject.Version = Version;
            simProject.Date = DateTime.Now;
            simProject.Evolution = "Flash";
            simProject.MOType = project.MOType;
            simProject.MTType = project.MTType;
            simProject.Hash = Hashage;

            // Le nom iDialog
            var queryIDialog = from idialogName in this.iDialogLocalData.ProjectDetail
                               where idialogName.IdProject == IDProject && idialogName.Version == Version
                               select idialogName.NomIDialog;

            if (queryIDialog.Count() > 0)
            {
                simProject.NomIDialog = queryIDialog.First();
            }
            else
            {
                simProject.NomIDialog = "NoFile";
            }

            this.iDialogLocalData.SIMProject.Add(simProject);
            this.iDialogLocalData.SaveChanges();

            // Copier le fichier flasher dans le répertoire dédié de la sim
            // 1 - Créer le répertoire de la SIM s'il n'existe pas
            String SimFolder = String.Format("{0}{1}", DefaultValues.Get().FlashFilesFolder, simProject.IdSIM);

            if (!Directory.Exists(SimFolder))
            {
                Directory.CreateDirectory(SimFolder);
            }

            // 2 - Copier le fichier projet dans la SIM s'il n'existe pas
            String SourceFileName, DestFileName;

            SourceFileName = String.Format("{0}{1}\\{2}_{3:0000}.idialog", DefaultValues.Get().iDialogFileFolder, prj.ProjectName, simProject.NomIDialog, simProject.Version);
            // DestFileName = String.Format("{0}{1}\\{2}.idialog", DefaultValues.Get().FlashFilesFolder, simProject.IdSIM, simProject.Hash);
            DestFileName = this.GetSimProjectFileName(simProject);

            if (File.Exists(DestFileName))
            {
                File.Delete(DestFileName);
            }

            File.Copy(SourceFileName, DestFileName);
        } // endMethod: FlashConfig
        
        /// <summary>
        /// Obtenir le projet SIM correspondant à l'IdSIM et à la date
        /// </summary>
        public SIMProject GetSimProject ( String IdSIM, DateTime date )
        {
            SIMProject Result;

            var Query = from simPrj in this.iDialogLocalData.SIMProject
                        where simPrj.IdSIM == IdSIM && simPrj.Date == date
                        select simPrj;

            if (Query.Count() > 0)
            {
                Result = Query.First();
            }
            else
            {
                Result = null;
            }

            return Result;
        } // endMethod: GetSimProject

        /// <summary>
        /// Obtenir les projets SIM et leurs historiques correspondant à l'IDSim
        /// </summary>
        public ObservableCollection<CommonSimProject> GetSimProjects ( String IdSIM, String IDProduct )
        {
            ObservableCollection<CommonSimProject> Result;
            IEnumerable<CommonSimProject> Query;

            if (IDProduct != "")
            {
                Query = from simPrj in this.iDialogLocalData.SIMProject
                        where simPrj.IdSIM == IdSIM && (simPrj.IdMO == IDProduct || simPrj.IdMT == IDProduct)
                        select new CommonSimProject()
                        {
                            IDSim = simPrj.IdSIM,
                            Date = simPrj.Date
                        }; 
            }
            else
            {
                Query = from simPrj in this.iDialogLocalData.SIMProject
                        where simPrj.IdSIM == IdSIM
                        select new CommonSimProject()
                        {
                             IDSim = simPrj.IdSIM,
                             Date = simPrj.Date
                        }; 
            }

            if (Query.Count() > 0)
            {
                Result = new ObservableCollection<CommonSimProject>(Query);
            }
            else
            {
                Result = new ObservableCollection<CommonSimProject>();
            }
            
            return Result;
        } // endMethod: GetSimProjects

        /// <summary>
        /// Acquérir le projet désigné par ProjectName
        /// </summary>
        public Projects GetProject(Int32 IdProject)
        {
            Projects Result = null;

            var queryProject = from project in this.iDialogLocalData.Projects
                               where project.Id == IdProject
                               select project;

            if (queryProject.Count() > 0)
            {
                Result = queryProject.First();
            }

            return Result;
        } // endMethod: GetProject

        /// <summary>
        /// Acquérir le projet désigné par ProjectName
        /// </summary>
        public Projects GetProject ( String ProjectName )
        {
            Projects Result = null;

            var queryProject = from project in this.iDialogLocalData.Projects
                               where project.ProjectName.Contains(ProjectName)
                               select project;

            if (queryProject.Count() > 0)
            {
                Result = queryProject.First();
            }

            return Result;
        } // endMethod: GetProject
        
        /// <summary>
        /// Acquérir le détail de projet désigné par l'ID du projet et la version
        /// </summary>
        public ProjectDetail GetProjectDetail ( Int32 IDProject, Int32 Version )
        {
            ProjectDetail Result = null;
            Projects prj;

            prj = this.GetProject(IDProject);

            var query = from prjDetail in prj.ProjectDetails
                        where prjDetail.Version == Version
                        select prjDetail;

            if (query.Count() > 0)
            {
                Result = query.First();
            }
            
            return Result;
        } // endMethod: GetProjectDetail
        
        /// <summary>
        /// Acquérir la dernière version du détail du projet
        /// </summary>
        public ProjectDetail GetLastVersionProjectDetail ( Int32 IDProject )
        {
            ProjectDetail Result;
            Projects prj = this.GetProject(IDProject);
            Int32 prjLastVer = prj.GetLastVersion();
            Result = this.GetProjectDetail(IDProject, prjLastVer);
            
            return Result;
        } // endMethod: GetLastVersionProjectDetail
        
        /// <summary>
        /// Acquérir la liste des détails d'un projet
        /// </summary>
        public ObservableCollection<ProjectDetail> GetProjectDetails ( Int32 IDProject )
        {
            Projects prj;
            ObservableCollection<ProjectDetail> Result;

            prj = this.GetProject(IDProject);
            Result = prj.ProjectDetails;

            return Result;
        } // endMethod: GetProjectDetails
        
        /// <summary>
        /// Retourner la dernière config flashée dans le produit ayant ce numéro de sim
        /// </summary>
        /// <param name="NumSim">Le numéro unique de la SIM (obligatoire)</param>
        /// <param name="IDProduct">Le code identité du produit (facultatif), si le code identité produit et vide "", 
        /// la recherche est effectuée uniquement avec NumSim</param>
        public SIMProject GetLastFlashConfig ( String NumSim, String IDProduct )
        {
            SIMProject Result = null;

            IEnumerable<SIMProject> queryConfig;

            if (IDProduct != "" && IDProduct != null)
            {
                queryConfig = from config in this.iDialogLocalData.SIMProject
                              where config.IdSIM == NumSim && (config.IdMO == IDProduct || config.IdMT == IDProduct)
                              orderby config.Date descending
                              select config; 
            }
            else
            {
                queryConfig = from config in this.iDialogLocalData.SIMProject
                              where config.IdSIM == NumSim
                              orderby config.Date descending
                              select config; 
            }

            if (queryConfig.Count() > 0)
            {
                Result = queryConfig.First();
            }

            return Result;
        } // endMethod: GetLastFlashConfig

        /// <summary>
        /// Fermeture de la base -> enregistrer tous les changements
        /// </summary>
        public void Close( )
        {
            this.iDialogLocalData.SaveChanges();
        } // endMethod: Close

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: BDDClient
}
