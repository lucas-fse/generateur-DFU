using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Xml.Linq;


namespace JAY
{
    /// <summary>
    /// Le chemin de fichiers par défaut et les chemins par défaut
    /// </summary>
    public class DefaultValues
    {
        // Constantes
        public const String USER = "user";
        public const String EXPERT = "expert";
        public const string EXPERT_EQ = "Expert_EQ";
        public const String ERREUR_EMBARQUE_FILE = "/GestionErreurEmbarque.xml";
        public const String APP_DATA_X86 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Jay.fr\Idialog\";
        public const String APP_DATA_X64 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Jay.fr\Idialog\";
        public const String USER_DATA_X86 = @"HKEY_CURRENT_USER\Software\Jay.fr\iDialog";
        public const String USER_DATA_x64 = @"HKEY_CURRENT_USER\Software\Wow6432Node\Jay.fr\iDialog";

        public const String KEY_SYNCHRO_BDD_JAY = "SynchroBDDJay";
        public const String KEY_INSTALLREF = "Installref";
        public const String KEY_USERID = "UserID";
        public const String KEY_PASSWORD = "Password";
        public const String KEY_KEEP_CONNECTION_DATA = "KeepConnectionData";

        public const String FOLDER_LANGUAGE = @"Languages\";
        public const String FOLDER_FILE_LANGUAGE = @"Languages\";
        public const String FOLDER_DEFAULT_ICONS = @"Defaulticons\";
        public const String FOLDER_RAPPORT = @"Rapport\";
        public const String FOLDER_JS = @"js\";
        public const String FOLDER_RAPPORT_IMAGES = @"Images\";
        public const String FOLDER_PLASTRON_GRAPHIC = @"PlastronGraphic\";
        public const String FOLDER_LOCALDATA = @"LocalData\";
        public const String FOLDER_IDIALOG_FILE = @"iDialogFiles\";
        public const String FOLDER_FLASH_FILE = @"FlashFiles\";

        public const String FILE_CONFIG = @"Config\iDialog.exe.config";
        public const String FILE_MATERIEL = @"Materiel\Materiels.data";
        public const String FILE_EDITABLEVARIABLE = @"Params\EditableVariables.data";
        public const String FILE_PARAMS = @"Params\Params.data";
        public const String FILE_RECENT = @"LocalData\RecentFile.xml";
        public const String FILE_REFILE = @"Reffiles\RefFile.xml";
        public const String FILE_CONFIGHARD = @"Params\ConfigurationHardware.data";
        public const String FILE_CONFIGMO = @"/ConfigMo.xml";
        
        public const String SUBFILE_ORGANSNAME = "/OrgansName.xml";
        public const String RAPPORT_CABLAGE_ROW = "/Rapport_Cablage_Row.txt";
        public const String URI_WCF_DATA_SERVICE = "URI_BDD_JAY";

        // Variables singleton
        private static DefaultValues _instance;
        static readonly object instanceLock = new object();

        // Variables
        #region Variables

        private String  _dataFileName;
        private String  _variableSimpleFileName;
        private String  _appPath;
        private String  _userMode;
        private string  _userModeEQ;
        private String  _paramData;
        private Int32   _waitFlashTime;
        private Int32   _waitConnectTimeMs;
        private String  _tempFilePath;
        private Boolean _verifyWriteProduct;
        private String  _organsName;
        private String  _refFileName;
        private String  _configHardName;
        //private String  _perlMigrationMateriel;

        private String  _appDataFolder;
        //private String  _configFile;
        private String _iDialogLanguagesFolder;
        private String _defaultIconsFolder;
        private String _fileLanguageFolder;
        private String _rapportFolder;
        private String _rapportFolderImages;
        private String _uriWCFDataService;
        private Boolean _isConnectedForSynchro;
        private Boolean _isLogsUsed;

        private Journal _logs;
        private bool _optimisationProg;
        private uint _optimisationProgOffset;
        private string _recentOpenFile;
        private bool _optimisationProgMo;
        private bool _optimisationProgMt;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Les logs sont-ils utilisés ?
        /// </summary>
        public Boolean IsLogsUsed
        {
            get
            {
                String Value = PegaseCore.Helper.ConfigurationReader.Instance.GetValue("UseLogs");

                switch (Value.ToLower())
                {
                    case "true":
                        this._isLogsUsed = true;
                        break;
                    case "false":
                    default:
                        this._isLogsUsed = false;
                        break;
                }

                return this._isLogsUsed;
            }
        } // endProperty: IsLogsUsed

        /// <summary>
        /// Acquérir le journal en cours d'utilisation
        /// </summary>
        public Journal Logs
        {
            get
            {
                if (this._logs == null)
                {
                    this._logs = Journal.OpenLog(this.AppDataFolder + "Logs\\iDialogLogs.log");
                }

                return this._logs;
            }
        } // endProperty: Logs

        /// <summary>
        /// iDialog est-il autorisé à se connecter pour la synchro?
        /// </summary>
        public Boolean IsConnectedForSynchro
        {
            get
            {
                return this._isConnectedForSynchro;
            }
            set
            {
                this._isConnectedForSynchro = value;
            }
        } // endProperty: IsConnectedForSynchro

        /// <summary>
        /// Le répertoire utilisé pour stocker les fichiers iDialog
        /// </summary>
        public String iDialogFileFolder
        {
            get
            {
                String Result;

                Result = String.Format("{0}{1}", this.AppDataFolder, FOLDER_IDIALOG_FILE);
                if (!Directory.Exists(Result))
                {
                    Directory.CreateDirectory(Result);
                }

                return Result;
            }
        } // endProperty: iDialogFileFolder

        /// <summary>
        /// Retourné le chemin d'accès au fichiers flashés
        /// </summary>
        public String FlashFilesFolder
        {
            get
            {
                String Result;

                Result = String.Format("{0}{1}", this.AppDataFolder, FOLDER_FLASH_FILE);
                if (!Directory.Exists(Result))
                {
                    Directory.CreateDirectory(Result);
                }

                return Result;
            }
        } // endProperty: FlashFilesFolder

        /// <summary>
        /// Le chemin pour la base de données locales
        /// </summary>
        public String LocalDataFolder
        {
            get
            {
                String Result;

                Result = String.Format("{0}{1}", this.AppDataFolder, FOLDER_LOCALDATA);

                return Result;
            }
        } // endProperty: LocalDataFolder

        /// <summary>
        /// Le répertoire des graphismes xaml pour le plastron
        /// </summary>
        public String PlastronGraphicFolder
        {
            get
            {
                String Result;

                Result = this.AppDataFolder + FOLDER_PLASTRON_GRAPHIC;
                if (!Directory.Exists(Result))
	            {
                    Directory.CreateDirectory(Result);
	            }

                return Result;
            }
        } // endProperty: PlastronGraphicFolder

        /// <summary>
        /// Le nom de la partie ConfigMO du package Params.data
        /// </summary>
        public String ConfigMOFileName
        {
            get
            {
                return FILE_CONFIGMO;
            }
        } // endProperty: ConfigMOFileName

        /// <summary>
        /// Une ligne du tableau de cablage
        /// </summary>
        public String RapportCablageRow
        {
            get
            {
                return RAPPORT_CABLAGE_ROW;
            }
        } // endProperty: RapportCablageRow

        /// <summary>
        /// Le dossier contenant les données permettant de créer le rapport
        /// </summary>
        public String RapportFolder
        {
            get
            {
                if (this._rapportFolder == null || this._rapportFolder == "")
                {
                    this._rapportFolder = this.AppDataFolder + FOLDER_RAPPORT;
                }

                return this._rapportFolder;
            }
        } // endProperty: RapportFolder
        /// <summary>
        /// Le dossier contenant les données permettant de créer le rapport
        /// </summary>
        public String RapportJavaScript
        {
            get
            {
                return this.AppDataFolder + FOLDER_RAPPORT + FOLDER_JS;             
            }
        } // endProperty: RapportFolder

        
        /// <summary>
        /// Le dossier contenant les données permettant de créer le rapport
        /// </summary>
        public String RapportFolderImages
        {
            get
            {
                if (this._rapportFolderImages == null || this._rapportFolderImages == "")
                {
                    this._rapportFolderImages = this.AppDataFolder + FOLDER_RAPPORT + FOLDER_RAPPORT_IMAGES;
                }

                return this._rapportFolderImages;
            }
        } // endProperty: RapportFolder
        /// Le dossier contenant les données permettant de créer le rapport
        /// </summary>
        public String RelativeRapportFolderImages
        {
            get
            {
                return FOLDER_RAPPORT_IMAGES;
            }
        } // endProperty: RapportFolder

        /// <summary>
        /// Le rapport en cours au format HTML
        /// </summary>
        public String CurrentRapportHTML
        {
            get
            {
                return this.RapportFolder + "complet.html";
            }
        } // endProperty: CurrentRapportHTML
          /// <summary>
          /// Le rapport en cours au format HTML
          /// </summary>
        public String CurrentRapportJS
        {
            get
            {
                return this.RapportJavaScript + "CurrentRapport.js";
            }
        } // endProperty: CurrentRapportHTML
        
        /// <summary>
        /// Le chemin d'accès des langues à embarquer dans le fichier iDialog
        /// </summary>
        public String FileLanguageFolder
        {
            get
            {
                if (this._fileLanguageFolder == null || this._fileLanguageFolder == "")
                {
                    this._fileLanguageFolder = this.AppDataFolder + FOLDER_FILE_LANGUAGE;
                }

                return this._fileLanguageFolder;
            }
        } // endProperty: FileLanguageFolder

        /// <summary>
        /// Le répertoire contenant les icons par défaut
        /// </summary>
        public String DefaultIconsFolder
        {
            get
            {
                if (this._defaultIconsFolder == null || this._defaultIconsFolder == "")
                {
                    this._defaultIconsFolder = this.AppDataFolder + FOLDER_DEFAULT_ICONS;
                }

                return this._defaultIconsFolder;
            }
        } // endProperty: DefaultIconsFolder

        /// <summary>
        /// Le répertoire du language
        /// </summary>
        public String IDialogLanguageFolder
        {
            get
            {
                if (this._iDialogLanguagesFolder == null || this._iDialogLanguagesFolder == "")
                {
                    this._iDialogLanguagesFolder = this.AppPath + FOLDER_LANGUAGE;
                    //this._languagesFolder = this.AppDataFolder + @"LanguagesI\";
                }

                return this._iDialogLanguagesFolder;
            }
        } // endProperty: LanguageFolder

        /// <summary>
        /// Le fichier de configuration
        /// </summary>
        public String ConfigFile
        {
            get
            {
                return this.AppDataFolder + FILE_CONFIG;
            }
        } // endProperty: ConfigFile

        /// <summary>
        /// Le chemin des données de l'application
        /// </summary>
        public String AppDataFolder
        {
            get
            {
                if (this._appDataFolder == null || this._appDataFolder == "")
                {
                    // Trouver la valeur dans le registre
                    String Key = (String)Registry.GetValue(APP_DATA_X86, KEY_INSTALLREF, null);
                    if (Key == null)
                    {
                        Key = (String)Registry.GetValue(APP_DATA_X64, KEY_INSTALLREF, null);
                    }

                    if (Key == null)
                    {
                        // Message d'erreur
                        MessageBox.Show("Missing Registry Key 'KEY_INSTALLREF', please reinstall your iDialog", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (Key.LastIndexOf('\\') != Key.Length - 1)
                    {
                        Key += @"\";
                    }
                    this._appDataFolder = Key;
                }
                return this._appDataFolder;
            }
        } // endProperty: AppDataFolder

        /// <summary>
        /// La base de données locale doit-elle être synchronisée avec la base de données JAY ?
        /// </summary>
        public Boolean SynchroBDDJay
        {
            get
            {
                Boolean Result = false;

                String Key = (String)Registry.GetValue(USER_DATA_X86, KEY_SYNCHRO_BDD_JAY, null);
                if (Key == null)
                {
                    Key = (String)Registry.GetValue(USER_DATA_x64, KEY_SYNCHRO_BDD_JAY, null);
                }
                if (Key == null)
                {
                    try
                    {
                        Registry.SetValue(USER_DATA_X86, KEY_SYNCHRO_BDD_JAY, "false");
                    }
                    catch
                    {
                        // Message d'erreur
                        MessageBox.Show("Missing Registry Key 'KEY_SYNCHRO_BDD_JAY', please reinstall your iDialog", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } Result = false;
                }
                else
                {
                    if (Key.ToUpper() == "TRUE" )
                    {
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                    }
                }

                return Result;
            }
            set
            {
                // Fabriquer la valeur
                String Value;

                if (value)
                {
                    Value = "True";
                }
                else
                {
                    Value = "False";
                    this.KeepConnectionData = false;
                    this.Password = "";
                    this.UserID = "";
                }

                // Chercher ou inscrire la clé (x86 ou x64)
                String Key = (String)Registry.GetValue(USER_DATA_X86, KEY_SYNCHRO_BDD_JAY, null);
                if (Key == null)
                {
                    Key = (String)Registry.GetValue(USER_DATA_x64, KEY_SYNCHRO_BDD_JAY, null);
                    if (Key != null)
                    {
                        Registry.SetValue(USER_DATA_x64, KEY_SYNCHRO_BDD_JAY, Value);
                    }
                }
                else
                {
                    Registry.SetValue(USER_DATA_X86, KEY_SYNCHRO_BDD_JAY, Value);
                }
                if (Key == null)
                {
                    // Message d'erreur
                    MessageBox.Show("Missing Registry Key 'KEY_SYNCHRO_BDD_JAY', please reinstall your iDialog", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        } // endProperty: SynchroBDDJay

        /// <summary>
        /// La base de données locale doit-elle être synchronisée avec la base de données JAY ?
        /// </summary>
        public Boolean KeepConnectionData
        {
            get
            {
                Boolean Result = false;

                String Key = (String)Registry.GetValue(USER_DATA_X86, KEY_KEEP_CONNECTION_DATA, null);
                if (Key == null)
                {
                    Key = (String)Registry.GetValue(USER_DATA_x64, KEY_KEEP_CONNECTION_DATA, null);
                }
                if (Key == null)
                {
                    Result = false;
                }
                else
                {
                    if (Key.ToUpper() == "TRUE")
                    {
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                    }
                }

                return Result;
            }
            set
            {
                // Fabriquer la valeur
                String Value;

                if (value)
                {
                    Value = "True";
                }
                else
                {
                    Value = "False";
                }

                // Chercher ou inscrire la clé (x86 ou x64)
                String Key = (String)Registry.GetValue(USER_DATA_X86, KEY_SYNCHRO_BDD_JAY, null);
                if (Key == null)
                {
                    Key = (String)Registry.GetValue(USER_DATA_x64, KEY_SYNCHRO_BDD_JAY, null);
                    if (Key != null)
                    {
                        Registry.SetValue(USER_DATA_x64, KEY_KEEP_CONNECTION_DATA, Value);
                    }
                }
                else
                {
                    Registry.SetValue(USER_DATA_X86, KEY_KEEP_CONNECTION_DATA, Value);
                }
                if (Key == null)
                {
                    // Message d'erreur
                    MessageBox.Show("Missing Registry Key 'KEY_KEEP_CONNECTION_DATA', please reinstall your iDialog", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        } // endProperty: SynchroBDDJay

        /// <summary>
        /// L'ID de l'utilisateur
        /// </summary>
        public String UserID
        {
            get
            {
                String Key = (String)Registry.GetValue(USER_DATA_X86, KEY_USERID, null);
                if (Key == null)
                {
                    Key = (String)Registry.GetValue(USER_DATA_x64, KEY_USERID, null);
                }

                return Key;
            }
            set
            {
                String Key = (String)Registry.GetValue(USER_DATA_X86, KEY_SYNCHRO_BDD_JAY, null);
                if (Key != null)
                {
                    Registry.SetValue(USER_DATA_X86, KEY_USERID, value);
                }
                else
                {
                    Registry.SetValue(USER_DATA_x64, KEY_USERID, value);
                }
            }
        } // endProperty: UserID

        /// <summary>
        /// Le mot de passe de l'utilisateur
        /// </summary>
        public String Password
        {
            get
            {
                String Key = (String)Registry.GetValue(USER_DATA_X86, KEY_PASSWORD, null);
                if (Key == null)
                {
                    Key = (String)Registry.GetValue(USER_DATA_x64, KEY_PASSWORD, null);
                }

                return Key;
            }
            set
            {
                String Key = (String)Registry.GetValue(USER_DATA_X86, KEY_SYNCHRO_BDD_JAY, null);
                if (Key != null)
                {
                    Registry.SetValue(USER_DATA_X86, KEY_PASSWORD, value);
                }
                else
                {
                    Registry.SetValue(USER_DATA_x64, KEY_PASSWORD, value);
                }
            }
        } // endProperty: Password

        /// <summary>
        /// Migration materiel
        /// </summary>
        public String PerlMigrationMateriel
        {
            get
            {
                return this.AppPath + "PatcherData\\Migrate\\migration_materiel_1_3.exe";
            }
        } // endProperty: PerlMigrationMateriel

        /// <summary>
        /// Le fichier de référence utilisé
        /// </summary>
        public String RefFileName
        {
            get
            {
                if (this._refFileName == null)
                {
                    this._refFileName = PegaseCore.Helper.ConfigurationReader.Instance.GetValue("FichierModeleXmlTechnique");
                    if (!File.Exists(this._refFileName))
                    {
                        this._refFileName = this.AppDataFolder + FILE_REFILE;
                    }
                }
                return this._refFileName;
            }
        } // endProperty: RefFile

        /// <summary>
        /// L'uri pour accéder au data service pour la synchro de la BDD
        /// </summary>
        public String UriWCFDataService
        {
            get
            {
                if (this._uriWCFDataService == null)
                {
                    this._uriWCFDataService = PegaseCore.Helper.ConfigurationReader.Instance.GetValue(URI_WCF_DATA_SERVICE);
                }

                return this._uriWCFDataService;
            }
        } // endProperty: UriWCFDataService

        /// <summary>
        /// Le fichier de configuradtion hardware utilisé
        /// </summary>
        public String ConfigHardName
        {
            get
            {
                if (this._configHardName == null)
                {
                    this._configHardName = PegaseCore.Helper.ConfigurationReader.Instance.GetValue("FichierXmlConfigHardware");
                    if (!File.Exists(this._configHardName))
                    {
                        this._configHardName = this.AppDataFolder + FILE_CONFIGHARD;
                    }
                }
                return this._configHardName;
            }
        } // endProperty: RefFile

        /// <summary>
        /// Configuration référence industrielle 
        /// </summary>
        public Uri ConfigRefIndusAppl
        {
            get
            {
                return new Uri("/ConfigRefIndusAppl.xml", UriKind.Relative);
            }
        } // endProperty: ConfigRefIndusAppl

        /// <summary>
        /// Le nombre d'alarmes utilisateur
        /// </summary>
        public Int32 NbUserAlarme
        {
            get
            {
                return 7;
            }
        } // endProperty: NbUserAlarme

        /// <summary>
        /// Le chemin vers les fichiers temporaires utilisés lors de l'édition des bitmaps par exemple
        /// </summary>
        public String TempFilePath
        {
            get
            {
                if ( this._tempFilePath == null )
                {
                    this._tempFilePath = this.AppDataFolder;
                }
                return this._tempFilePath;
            }
        } // endProperty: TempFilePath

        /// <summary>
        /// Vérifier s'il est autoriser d'écrire dans le produit
        /// </summary>
        public Boolean VerifyWriteProduct
        {
            get
            {
                // 8 - Evaluer s'il y a besoin de vérifier l'écriture dans les produits
                String VWP = PegaseCore.Helper.ConfigurationReader.Instance.GetValue("VerifyWriteProduct").ToUpper();
                if (VWP == "TRUE")
                {
                    this._verifyWriteProduct = true;
                }
                else
                {
                    this._verifyWriteProduct = false;
                }

                return this._verifyWriteProduct;
            }
        } // endProperty: VerifyWriteProduct

        /// <summary>
        /// Le fichier de paramétrage
        /// </summary>
        public String ParamData
        {
            get
            {
                if (this._paramData == null)
                {
                    this._paramData = this.AppDataFolder + FILE_PARAMS;
                }

                return this._paramData;
            }
        } // endProperty: ParamData


        /// <summary>
        /// Le fichier de paramétrage
        /// </summary>
        public String RecentOpenFile
        {
            get
            {
                if (this._recentOpenFile == null)
                {
                    this._recentOpenFile = this.AppDataFolder + FILE_RECENT;
                }

                return this._recentOpenFile;
            }
        } // endProperty: ParamData

        /// <summary>
        /// Le chemin de l'application
        /// </summary>
        public String AppPath
        {
            get
            {
                return this._appPath;
            }
        } // endProperty: AppPath

        /// <summary>
        /// Le type d'utilisateur utilisant le logiciel
        /// </summary>
        public String UserMode
        {
            get
            {
                if (this._userMode == null)
                {
                    // 3 - UserMode
                    this._userMode = PegaseCore.Helper.ConfigurationReader.Instance.GetValue("UserMode");
                    switch (this._userMode.Trim().ToLower())
                    {
                        case EXPERT:
                            this._userMode = EXPERT;
                            break;
                        default:
                            this._userMode = USER;
                            break;
                    }
                }

                return this._userMode.ToLower();
            }
        } // endProperty: UserMode
        
        public String UserModeEQ
        {
            get
            {
                if (this._userModeEQ == null)
                {
                    // 3 - UserMode
                    this._userModeEQ = PegaseCore.Helper.ConfigurationReader.Instance.GetValue("UserModeEQ");
                    switch (this._userModeEQ.Trim().ToLower())
                    {
                        case EXPERT:
                            this._userModeEQ = EXPERT_EQ;
                            break;
                        default:
                            this._userModeEQ = USER;
                            break;
                    }
                }

                return this._userModeEQ.ToLower();
            }
        } // endProperty: UserMode
        /// <summary>
        /// Le nom des organes
        /// </summary>
        public String OrgansName
        {
            get
            {
                if (this._organsName == null)
                {
                    this._organsName = SUBFILE_ORGANSNAME;
                }

                return this._organsName;
            }
        } // endProperty: OrgansName

        /// <summary>
        /// Le package enfermant toutes les données matérielles
        /// </summary>
        public String MaterielPackage
        {
            get
            {
                this._dataFileName = PegaseCore.Helper.ConfigurationReader.Instance.GetValue("RefFiles");
                if (!File.Exists(this._dataFileName))
                {
                    this._dataFileName = this.AppDataFolder + FILE_MATERIEL;
                }

                return this._dataFileName;
            }
        } // endProperty: MaterielPackage

        /// <summary>
        /// AFaire: Ajouter une description de la propriété
        /// </summary>
        public String VariableEditablePackage
        {
            get
            {
                if (this._variableSimpleFileName == null)
                {
                    // 4 - Variables Editables
                    this._variableSimpleFileName = PegaseCore.Helper.ConfigurationReader.Instance.GetValue("VariableEditables");
                    if (!File.Exists(this._variableSimpleFileName))
                    {
                        // opter pour la copie locale
                        this._variableSimpleFileName = this.AppDataFolder + FILE_EDITABLEVARIABLE;
                    }
                }

                return this._variableSimpleFileName;
            }
        } // endProperty: VariableEditablePackage

        /// <summary>
        /// Le temps d'attente avant de flasher
        /// </summary>
        public Int32 WaitFlashTimeMs
        {
            get
            {
                if (this._waitFlashTime == 0)
                {
                    // 6 - Le temps d'attente avant le flashage
                    try
                    {
                        String Value = PegaseCore.Helper.ConfigurationReader.Instance.GetValue("FlashWaitTime");
                        this._waitFlashTime = Convert.ToInt32(Value);
                    }
                    catch
                    {
                        this._waitFlashTime = 3000;
                    }
                }

                return this._waitFlashTime;
            }
        } // endProperty: WaitFlashTimeMs

        /// <summary>
        /// Le temps d'attente avant de flasher
        /// </summary>
        public Int32 WaitConnectTimeMs
        {
            get
            {
                if (this._waitConnectTimeMs == 0)
                {
                    // 6 - Le temps d'attente avant le flashage
                    try
                    {
                        String Value = PegaseCore.Helper.ConfigurationReader.Instance.GetValue("ConnectTime");
                        this._waitConnectTimeMs = Convert.ToInt32(Value);
                    }
                    catch
                    {
                        this._waitConnectTimeMs = 5000;
                    }
                }

                return this._waitFlashTime;
            }
        } // endProperty: WaitFlashTimeMs


        public bool OptimisationProgMt
        {
            get
            {

                return this._optimisationProgMt;
            }
            set
            {
                try
                {
                    String Value = PegaseCore.Helper.ConfigurationReader.Instance.GetValue("OptimisationProg");
                    if (Convert.ToBoolean(Value))
                    {
                        this._optimisationProgMt = value;
                    }
                    else
                    {
                        this._optimisationProgMt = false;
                    }
                }
                catch
                {
                    this._optimisationProgMt = false;
                }

            }
        }
        public bool OptimisationProgMo
        {
            get
            {
                
                return this._optimisationProgMo;
            }
            set
            {
                try
                {
                    String Value = PegaseCore.Helper.ConfigurationReader.Instance.GetValue("OptimisationProg");
                    if (Convert.ToBoolean(Value))
                    {
                        this._optimisationProgMo = value;
                    }
                    else
                    {
                        this._optimisationProgMo = false;
                    }
                }
                catch
                {
                    this._optimisationProgMo = false;
                }
                
            }
        }
        public UInt32 OptimisationProgOffset
        {
            get
            {
                try
                {
                    String Value = PegaseCore.Helper.ConfigurationReader.Instance.GetValue("OptimisationProg");
                    this._optimisationProgOffset = Convert.ToUInt32(Value);
                }
                catch
                {
                    this._optimisationProgOffset = 0x400;
                }
                return this._optimisationProgOffset;
            }
        }
        #endregion

        // Constructeur
        #region Constructeur

        private DefaultValues()
        {
            // Initialiser tous les chemins

            // 1 - AppPath
            this._appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            this._appPath = this._appPath.Substring(0, this._appPath.LastIndexOf('\\'));
            this._appPath += @"\";

            // 2 - Initialiser les valeurs
            this._isConnectedForSynchro = false;

            // 3 - Ouvrir un fichier LOG

        }

        #endregion

        // Méthodes
        #region Méthodes

        // Retourne une instance unique de la classe
        public static DefaultValues Get()
        {
            lock (instanceLock)
            {
                if (_instance == null)
                    _instance = new DefaultValues();


                return _instance;
            }
        }

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: DefaultPathAndFile
}
