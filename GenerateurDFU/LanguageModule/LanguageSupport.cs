using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Mvvm = GalaSoft.MvvmLight;
using JAY.XMLCore;
using JAY.FileCore;
using JAY.PegaseCore;
using System.Xml;
using System.Xml.Linq;

namespace JAY
{
    /// <summary>
    /// Support de langue pour les IHM
    /// Permet de récupérer la langue de l'utilisateur (Windows)
    /// La langue actuellement définie...
    /// </summary>
    public class LanguageSupport : Mvvm.ViewModelBase
    {
        // Variables singleton
        private static LanguageSupport _instance;
        static readonly object instanceLock = new object();
        private ObservableCollection<XMLProcessing> _xmlLanguage;
        private String _languagePath;
        private String _languageName;
        private Stream _languageType;

        // Constantes
        #region Constantes
        private const String ATTRIB_TEXT = "text";
        private const String ATTRIB_TOOLTIP = "tooltip";

        #endregion

        // Variables
        #region Variables

        //CultureInfo _cinfo;
        private ObservableCollection<String> _languages;
        private ObservableCollection<Image> _languagesFlags;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le nom de la culture en cours d'utilisation
        /// </summary>
        //public String CurrentLanguage
        //{
        //    get
        //    {
        //        String Result;
        //        String[] L = CultureInfo.CurrentCulture.DisplayName.Split(new Char[] { ' ' });

        //        Result = L[0];
        //        var Query = from l in Languages
        //                    where l == this.LanguageName
        //                    select l;

        //        if (Query.Count > 0)
        //        {
        //            Result = Query.First();
        //        }


        //        return Result;
        //    }
        //} // endProperty: CurrentCulture

        /// <summary>
        /// Le nom des langues disponibles
        /// </summary>
        public ObservableCollection<String> Languages
        {
            get
            {
                return this._languages;
            }
            set
            {
                this._languages = value;
            }
        } // endProperty: Languages

        /// <summary>
        /// La liste des drapeaux liés au languages
        /// </summary>
        public ObservableCollection<Image> LanguagesFlags
        {
            get
            {
                return this._languagesFlags;
            }
            set
            {
                this._languagesFlags = value;
            }
        } // endProperty: LanguagesFlags

        /// <summary>
        /// Le chemin de langue
        /// </summary>
        public String LanguagePath
        {
            get
            {
                return this._languagePath;
            }
            private set
            {
                this._languagePath = value;
            }
        } // endProperty: LanguagePath

        /// <summary>
        /// Le nom de la langue
        /// </summary>
        public String LanguageName
        {
            get
            {
                return this._languageName;
            }
            private set
            {
                this._languageName = value;
            }
        } // endProperty: LanguageName

        public String LanguageType
        {
            get
            {
                if (LanguageName.ToUpper().Equals("FRANCAIS"))
                {
                    return ("fr-FR");
                }
                else if (LanguageName.ToUpper().Equals("ANGLAIS"))
                {
                    return ("en-GB");
                }
                else if (LanguageName.ToUpper().Equals("ALLEMAND"))
                {
                    return ("de-DE");
                }
                else
                {
                    return ("");
                }
            }
        }

        public String LanguageTypeDe(string languagetype)
        {

                if (LanguageName.ToUpper().Equals("FRANCAIS"))
                {
                    return ("fr-FR");
                }
                else if (LanguageName.ToUpper().Equals("ANGLAIS"))
                {
                    return ("en-GB");
                }
                else if (LanguageName.ToUpper().Equals("ALLEMAND"))
                {
                    return ("de-DE");
                }
                else
                {
                    return ("");
                }
            
        }

        #endregion

        #region MainWindow
        /// <summary>
        /// Traduction du champ contact
        /// </summary>
        public String MW_CONTACT
        {
            get;
            private set;
        } // endProperty: MW_CONTACT

        /// <summary>
        /// Traduction du tooltip pour contact
        /// </summary>
        public String MW_CONTACT_TOOLTIP
        {
            get;
            private set;
        } // endProperty: MW_CONTACT_TOOLTIP

        /// <summary>
        /// Traduction du champ WEB
        /// </summary>
        public String MW_WEB
        {
            get;
            private set;
        } // endProperty: MW_WEB

        /// <summary>
        /// La traduction du TOOLTIP Web
        /// </summary>
        public String MW_WEB_TOOLTIP
        {
            get;
            private set;
        } // endProperty: MW_WEB_TOOLTIP

        /// <summary>
        /// La traduction du champ à propos
        /// </summary>
        public String MW_APROPOS
        {
            get;
            private set;
        } // endProperty: MW_APROPOS

        /// <summary>
        /// La traduction du ToolTip A propos
        /// </summary>
        public String MW_APROPOS_TOOLTIP
        {
            get;
            private set;
        } // endProperty: MW_APROPOS_TOOLTIP 

        /// <summary>
        /// La traduction de l'aide
        /// </summary>
        public String MW_HELP
        {
            get;
            private set;
        } // endProperty: MW_HELP

        /// <summary>
        /// La traduction de l'aide (tooltip)
        /// </summary>
        public String MW_HELP_TOOLTIP
        {
            get;
            private set;
        } // endProperty: MW_HELP_TOOLTIP

        #endregion

        #region WPF Connexion
        /// <summary>
        /// La traduction pour le champ profil
        /// </summary>
        public String WPF_CON_PROFIL
        {
            get;
            private set;
        } // endProperty: WPF_CON_PROFIL

        /// <summary>
        /// La traduction du tooltip pour le profil
        /// </summary>
        public String WPF_CON_PROFIL_TOOLTIP
        {
            get;
            private set;
        } // endProperty: WPF_CON_PROFIL_TOOLTIP

        /// <summary>
        /// La traduction pour l'utilisateur
        /// </summary>
        public String WPF_CON_USER
        {
            get;
            private set;
        } // endProperty: WPF_CON_USER

        /// <summary>
        /// La traduction pour le tooltip du champ utilisateur
        /// </summary>
        public String WPF_CON_USER_TOOLTIP
        {
            get;
            private set;
        } // endProperty: WPF_CON_USER_TOOLTIP

        /// <summary>
        /// La traduction du nom de champ Password
        /// </summary>
        public String WPF_CON_PASSWORD
        {
            get;
            private set;
        } // endProperty: WPF_CON_PASSWORD

        /// <summary>
        /// La traduction du tooltip attaché au champ Password
        /// </summary>
        public String WPF_CON_PASSWORD_TOOLTIP
        {
            get;
            private set;
        } // endProperty: WPF_CON_PASSWORD_TOOLTIP

        /// <summary>
        /// La traduction du champ connexion
        /// </summary>
        public String WPF_CON_CONNECT
        {
            get;
            private set;
        } // endProperty: WPF_CON_CONNECT

        /// <summary>
        /// La traduction du tooltip en lien avec la champ connexion
        /// </summary>
        public String WPF_CON_CONNECT_TOOLTIP
        {
            get;
            set;
        } // endProperty: WPF_CON_CONNECT_TOOLTIP 
        #endregion

        #region WPF_ImportMateriel

        /// <summary>
        /// Le libellé correspondant à l'importation matériel
        /// </summary>
        public String WPF_IMPMAT_IMP_LIBELLE
        {
            get;
            set;
        } // endProperty: WPF_IMPMAT_IMP_LIBELLE

        /// <summary>
        /// Le libellé du bouton d'importation
        /// </summary>
        public String WPF_IMPMAT_IMPORT
        {
            get;
            set;
        } // endProperty: WPF_IMPMAT_IMPORT

        /// <summary>
        /// Le libellé du bouton d'importation
        /// </summary>
        public String WPF_IMPMAT_IMPORT_TOOLTIP
        {
            get;
            set;
        } // endProperty: WPF_IMPMAT_IMPORT_TOOLTIP

        /// <summary>
        /// Le libellé lié à l'importation depuis la base de données
        /// </summary>
        public String WPF_IMPMAT_SELECT_LIBELLE
        {
            get;
            set;
        } // endProperty: WPF_IMPMAT_SELECT_LIBELLE

        /// <summary>
        /// La traduction du bouton sélection
        /// </summary>
        public String WPF_IMPMAT_SELECT
        {
            get;
            set;
        } // endProperty: WPF_IMPMAT_SELECT

        /// <summary>
        /// Le tooltip associé à WPF_IMPMAT_SELECT
        /// </summary>
        public String WPF_IMPMAT_SELECT_TOOLTIP
        {
            get;
            set;
        } // endProperty: WPF_IMPMAT_SELECT_TOOLTIP

        /// <summary>
        /// Le tooltip associé au champ WPF_IMPMAT_NUM_SERIE
        /// </summary>
        public String WPF_IMPMAT_NUM_SERIE_TOOLTIP
        {
            get;
            set;
        } // endProperty: WPF_IMPMAT_NUM_SERIE_TOOLTIP

        /// <summary>
        /// Le tooltip associé au champ WPF_IMPMAT_NUM_ID
        /// </summary>
        public String WPF_IMPMAT_NUM_ID_TOOLTIP
        {
            get;
            set;
        } // endProperty: WPF_IMPMAT_NUM_ID_TOOLTIP

        /// <summary>
        /// Le libellé associé au numéro de série
        /// </summary>
        public String WPF_IMPMAT_NUM_SERIE_LIBELLE
        {
            get;
            set;
        } // endProperty: WPF_IMPMAT_NUM_SERIE_LIBELLE

        /// <summary>
        /// Le libellé associé au numéro unique d'identification
        /// </summary>
        public String WPF_IMPMAT_NUM_ID_LIBELLE
        {
            get;
            set;
        } // endProperty: WPF_IMPMAT_NUM_ID_LIBELLE

        #endregion

        #region OPT_LOG

        /// <summary>
        /// La traduction du mode d'activation IR = 0
        /// </summary>
        public String OPT_LOG_MODEIR1
        {
            get;
            private set;
        } // endProperty: OPT_LOG_MODEIR1

        /// <summary>
        /// La traduction du mode d'activation IR = 1
        /// </summary>
        public String OPT_LOG_MODEIR2
        {
            get;
            private set;
        } // endProperty: OPT_LOG_MODEIR2

        /// <summary>
        /// La traduction du mode d'activation IR = 2
        /// </summary>
        public String OPT_LOG_MODEIR3
        {
            get;
            private set;
        } // endProperty: OPT_LOG_MODEIR2

        #endregion

        #region MO_LIBEL

        /// <summary>
        /// Le libellé de désactivation de l'IR
        /// </summary>
        public String LIBEL_MO_IR_ABSENT
        {
            get;
            private set;
        } // endProperty: LIBEL_MO_IR_DESABLE

        /// <summary>
        /// Le libellé de l'activation de l'IR en façade
        /// </summary>
        public String LIBEL_MO_IR_FACE
        {
            get;
            private set;
        } // endProperty: LIBEL_MO_

        /// <summary>
        /// Le libellé de l'activation de l'IR sur la carte additionnel
        /// </summary>
        public String LIBEL_MO_IR_EXTENSION
        {
            get;
            private set;
        } // endProperty: LIBEL_MO_IR_EXTENSION

        #endregion

        #region COMMON_FIELD

        /// <summary>
        /// La traduction pour "On"
        /// </summary>
        public String COMMON_FIELD_ON
        {
            get;
            private set;
        } // endProperty: COMMON_FIELD_ON

        /// <summary>
        /// La traduction pour "Off"
        /// </summary>
        public String COMMON_FIELD_OFF
        {
            get;
            private set;
        } // endProperty: COMMON_FIELD_OFF

        /// <summary>
        /// La traduction de "Inconnu"
        /// </summary>
        public String COMMON_FIELD_UNKNOW
        {
            get;
            private set;
        } // endProperty: COMMON_FIELD_UNKNOW

        #endregion

        #region Error

        /// <summary>
        /// Le texte de l'erreur ERROR_UNDESCRIBED
        /// </summary>
        public String ERROR_UNDESCRIBED
        {
            get;
            private set;
        } // endProperty: ERROR_UNDESCRIBED

        /// <summary>
        /// Le texte de l'erreur ERROR_FILE_NOT_FOUND
        /// </summary>
        public String ERROR_FILE_NOT_FOUND
        {
            get;
            private set;
        } // endProperty: ERROR_FILE_NOT_FOUND
        
        #endregion

        // Constructeur
        #region Constructeur

        private LanguageSupport()
        {
            // établir la liste des langues disponibles
            this._xmlLanguage = new ObservableCollection<XMLProcessing>();
            // initialiser la culture avec la langue par défaut
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Récupérer la valeur de l'attribut texte s'il existe
        /// </summary>
        public String GetText ( String XMLPath )
        {
            String Result = null;

            Result = this.GetText(XMLPath, this.LanguageName);
            if (Result == null || Result == "")
            {
                String[] names = XMLPath.Split(new Char[]{'/'});
                Result = names[names.Length - 1];
            }

            return Result;
        } // endMethod: GetText

        public String GetTextRapport(String XMLPath)
        {
            String Result = null;

            Result = this.GetText(XMLPath);
            Result = Result.Replace("'", "\\'");

            return Result;
        } // endMethod: GetText

        public String GetToolTipRapport(String XMLPath)
        {
            String Result = null;

            Result = this.GetToolTip(XMLPath);
            if (Result != null)
            {
                Result = Result.Replace("'", "\\'");
            }

            return Result;
        } // endMethod: GetText
        public String GetToolTipRapport(String XMLPath, string langue)
        {
            String Result = null;

            Result = this.GetToolTip(XMLPath, langue);
            if (Result != null)
            {
                Result = Result.Replace("'", "\\'");
            }
            return Result;
        } // endMethod: GetText

        /// <summary>
        /// Récupérer la valeur de l'attribut texte s'il existe
        /// </summary>
        public String GetText(String XMLPath, String LanguageName)
        {
            String Result = null;

            if ((this._xmlLanguage != null) && (XMLPath.Length>0))
            {
                if (XMLPath.Substring(0, 1) != "/")
                {
                    XMLPath = "/" + XMLPath;
                }
                // Parcourir tous les xml chargés
                foreach (var xmlL in this._xmlLanguage)
                {
                    Result = xmlL.GetValue(LanguageName + XMLPath, "", "", ATTRIB_TEXT);
                    if (this.LanguageName == "Francais" && Result == null)
                    {
                        Result = xmlL.GetValue("Français" + XMLPath, "", "", ATTRIB_TEXT);
                    }
                    if (Result != null)
                    {
                        break;
                    }
                }
            }

            return Result;
        } // endMethod: GetText
        
        /// <summary>
        /// Récupérer la valeur de l'attribut tooltip s'il existe
        /// </summary>
        public String GetToolTip ( String XMLPath )
        {
            String Result = this.GetToolTip(XMLPath, this.LanguageName);
            
            return Result;
        } // endMethod: GetToolTip

        /// <summary>
        /// Récupérer la valeur de l'attribut tooltip s'il existe
        /// </summary>
        public String GetToolTip ( String XMLPath, String LanguageName )
        {
            String Result = null;

            if (this._xmlLanguage != null)
            {
                if (XMLPath.Substring(0, 1) != "/")
                {
                    XMLPath = "/" + XMLPath;
                }

                // Parcourir tous les xml chargés
                foreach (var xmlL in this._xmlLanguage)
                {
                    Result = xmlL.GetValue(LanguageName + XMLPath, "", "", ATTRIB_TOOLTIP);
                    if (this.LanguageName == "Francais" && Result == null)
                    {
                        Result = xmlL.GetValue("Français" + XMLPath, "", "", ATTRIB_TOOLTIP);
                    }
                    if (Result != null)
                    {
                        break;
                    }
                }
            }

            return Result;
        } // endMethod: GetToolTip

        // Retourne une instance unique de la classe
        public static LanguageSupport Get()
        {
            lock (instanceLock)
            {
                if (_instance == null)
                    _instance = new LanguageSupport();
                return _instance;
            }
        }

        /// <summary>
        /// Initialise le language en cours d'utilisation
        /// </summary>
        public Int32 InitialiseLanguage(String LanguageName, String Path, iDialogPackage package)
        {
            Int32 Result = XML_ERROR.ERROR_UNDESCRIBED;
            XMLProcessing xmlL = new XMLProcessing();

            this._xmlLanguage.Clear();

            if (LanguageName.ToLower() == "français")
            {
                LanguageName = LanguageName.Replace('ç', 'c'); 
            }
            // Lire toute les valeurs et assigner les propriétés correspondantes
            // du fichier de langue principale
            String XMLFileName = Path + LanguageName + ".xml";
            if (xmlL.OpenXML(XMLFileName) != XML_ERROR.NO_ERROR)
            {
                // charger le fichier de langue par défaut
                XMLFileName = Path + this.Languages[0] + ".xml";
                xmlL.OpenXML(XMLFileName);
            }
            this._xmlLanguage.Add(xmlL);
            this.LanguagePath = Path;
            this.LanguageName = LanguageName;

            // MainWindow
            this.InitMainWindow(LanguageName, this._xmlLanguage[0]);

            // WpfConnexion
            this.InitWPFConnexion(LanguageName, this._xmlLanguage[0]);

            // WpfImportMatériel
            this.InitWPFImportMateriel(LanguageName, this._xmlLanguage[0]);

            // Libel MO
            this.InitMOLabel(LanguageName, this._xmlLanguage[0]);

            // Options Logiciels
            this.InitOptionsLogiciels(LanguageName, this._xmlLanguage[0]);

            // Common Field
            this.InitCommonField(LanguageName, this._xmlLanguage[0]);

            // Error
            this.InitError(LanguageName, this._xmlLanguage[0]);

            if (package != null)
            {
                Result = this.AddiDialogLanguageFile(package);
            }

            Result = XML_ERROR.NO_ERROR;
            return Result;
        } // endMethod: InitialiseLanguage 
        
        /// <summary>
        /// Ajouter un fihcier de langue provenant d'iDialog
        /// </summary>
        public Int32 AddiDialogLanguageFile ( iDialogPackage package )
        {
            Int32 Result = XML_ERROR.NO_ERROR;

            // 1 - Ouvrir le package
            package.OpenPackage();

            // 2 - Chercher le fichier langue correspondant à la langue en cours
            String LName = this.LanguageName;
            LName = LName.Replace('ç', 'c');
            String FileName = String.Format("/Languages/{0}.xml", LName);
            Uri FileUri = new Uri(FileName, UriKind.Relative);
            Stream stream = package.GetPartStream(FileUri);

            // 3 - Ajouter le fichier langue à la collection actuelle
            if (stream != null)
            {
                String XML = "";
                using (StreamReader SR = new StreamReader(stream, Encoding.GetEncoding(Constantes.XML_ENCODING)))
                {
                    XML = SR.ReadToEnd();
                }

                XDocument doc = XDocument.Parse(XML);

                XMLProcessing xmlL = new XMLProcessing();
                xmlL.OpenXML(doc.Root);
                this._xmlLanguage.Add(xmlL);
            }

            // 4 - fermer le package
            package.ClosePackage();

            return Result;
        } // endMethod: AddiDialogLanguageFile

        /// <summary>
        /// Initialiser la langue pour la fenetre principale
        /// </summary>
        private Int32 InitMainWindow(String LanguageName, XMLProcessing XMLP)
        {
            Int32 Result = XML_ERROR.NO_ERROR;

            this.MW_CONTACT = XMLP.GetValue(LanguageName + "/MW/MW_CONTACT", "", "", ATTRIB_TEXT);
            this.MW_CONTACT_TOOLTIP = XMLP.GetValue(LanguageName + "/MW/MW_CONTACT", "", "", ATTRIB_TOOLTIP);

            this.MW_WEB = XMLP.GetValue(LanguageName + "/MW/MW_WEB", "", "", ATTRIB_TEXT);
            this.MW_WEB_TOOLTIP = XMLP.GetValue(LanguageName + "/MW/MW_WEB", "", "", ATTRIB_TOOLTIP);

            this.MW_APROPOS = XMLP.GetValue(LanguageName + "/MW/MW_APROPOS", "", "", ATTRIB_TEXT);
            this.MW_APROPOS_TOOLTIP = XMLP.GetValue(LanguageName + "/MW/MW_APROPOS", "", "", ATTRIB_TOOLTIP);

            this.MW_HELP = XMLP.GetValue(LanguageName + "/MW/MW_HELP", "", "", ATTRIB_TEXT);
            this.MW_HELP_TOOLTIP = XMLP.GetValue(LanguageName + "/MW/MW_HELP", "", "", ATTRIB_TOOLTIP);

            return Result;
        } // endMethod: InitMainWindow
        
        /// <summary>
        /// Initialiser la langue pour WPFConnection
        /// </summary>
        private Int32 InitWPFConnexion ( String LanguageName, XMLProcessing XMLP )
        {
            Int32 Result = XML_ERROR.NO_ERROR;

            this.WPF_CON_PROFIL = XMLP.GetValue(LanguageName + "/WPF_CON/WPF_CON_PROFIL", "", "", ATTRIB_TEXT);
            this.WPF_CON_PROFIL_TOOLTIP = XMLP.GetValue(LanguageName + "/WPF_CON/WPF_CON_PROFIL", "", "", ATTRIB_TOOLTIP);

            this.WPF_CON_USER = XMLP.GetValue(LanguageName + "/WPF_CON/WPF_CON_USER", "", "", ATTRIB_TEXT);
            this.WPF_CON_USER_TOOLTIP = XMLP.GetValue(LanguageName + "/WPF_CON/WPF_CON_USER", "", "", ATTRIB_TOOLTIP);

            this.WPF_CON_PASSWORD = XMLP.GetValue(LanguageName + "/WPF_CON/WPF_CON_PASSWORD", "", "", ATTRIB_TEXT);
            this.WPF_CON_PASSWORD_TOOLTIP = XMLP.GetValue(LanguageName + "/WPF_CON/WPF_CON_PASSWORD", "", "", ATTRIB_TOOLTIP);

            this.WPF_CON_CONNECT = XMLP.GetValue(LanguageName + "/WPF_CON/WPF_CON_CONNECT", "", "", ATTRIB_TEXT);
            this.WPF_CON_CONNECT_TOOLTIP = XMLP.GetValue(LanguageName + "/WPF_CON/WPF_CON_CONNECT", "", "", ATTRIB_TOOLTIP);

            return Result;
        } // endMethod: IntiWPFConnexion

        /// <summary>
        /// Initialiser la langue pour l'importation de données
        /// </summary>
        private Int32 InitWPFImportMateriel ( String LanguageName, XMLProcessing XMLP )
        {
            Int32 Result = XML_ERROR.NO_ERROR;

            this.WPF_IMPMAT_IMP_LIBELLE = XMLP.GetValue(LanguageName + "/WPF_IMPMAT/WPF_IMPMAT_IMP_LIBELLE", "", "", ATTRIB_TEXT);
            this.WPF_IMPMAT_IMPORT = XMLP.GetValue(LanguageName + "/WPF_IMPMAT/WPF_IMPMAT_IMPORT", "", "", ATTRIB_TEXT);
            this.WPF_IMPMAT_IMPORT_TOOLTIP = XMLP.GetValue(LanguageName + "/WPF_IMPMAT/WPF_IMPMAT_IMPORT", "", "", ATTRIB_TOOLTIP);
            this.WPF_IMPMAT_NUM_ID_LIBELLE = XMLP.GetValue(LanguageName + "/WPF_IMPMAT/WPF_IMPMAT_NUM_ID_LIBELLE", "", "", ATTRIB_TEXT);
            this.WPF_IMPMAT_NUM_ID_TOOLTIP = XMLP.GetValue(LanguageName + "/WPF_IMPMAT/WPF_IMPMAT_NUM_ID", "", "", ATTRIB_TOOLTIP);
            this.WPF_IMPMAT_NUM_SERIE_LIBELLE = XMLP.GetValue(LanguageName + "/WPF_IMPMAT/WPF_IMPMAT__NUM_SERIE_LIBELLE", "", "", ATTRIB_TEXT);
            this.WPF_IMPMAT_NUM_SERIE_TOOLTIP = XMLP.GetValue(LanguageName + "/WPF_IMPMAT/WPF_IMPMAT_NUM_SERIE", "", "", ATTRIB_TOOLTIP);
            this.WPF_IMPMAT_SELECT = XMLP.GetValue(LanguageName + "/WPF_IMPMAT/WPF_IMPMAT_SELECT", "", "", ATTRIB_TEXT);
            this.WPF_IMPMAT_SELECT_LIBELLE = XMLP.GetValue(LanguageName + "/WPF_IMPMAT/WPF_IMPMAT_SELECT_LIBELLE", "", "", ATTRIB_TEXT);
            this.WPF_IMPMAT_SELECT_TOOLTIP = XMLP.GetValue(LanguageName + "/WPF_IMPMAT/WPF_IMPMAT_SELECT", "", "", ATTRIB_TOOLTIP);

            return Result;
        } // endMethod: InitWPFImportMateriel
        
        /// <summary>
        /// Initialiser la langue pour l'initialisation des labels
        /// </summary>
        public Int32 InitMOLabel ( String LanguageName, XMLProcessing XMLP )
        {
            Int32 Result = XML_ERROR.NO_ERROR;

            this.LIBEL_MO_IR_ABSENT = XMLP.GetValue(LanguageName + "/LIBEL_MO/LIBEL_MO_IR_ABSENT", "", "", ATTRIB_TEXT);
            this.LIBEL_MO_IR_EXTENSION = XMLP.GetValue(LanguageName + "/LIBEL_MO/LIBEL_MO_IR_EXTENSION", "", "", ATTRIB_TEXT);
            this.LIBEL_MO_IR_FACE = XMLP.GetValue(LanguageName + "/LIBEL_MO/LIBEL_MO_IR_FACE", "", "", ATTRIB_TEXT);

            return Result;
        } // endMethod: InitMOLabel
        
        /// <summary>
        /// Initialiser les options logiciels
        /// </summary>
        public Int32 InitOptionsLogiciels ( String LanguageName, XMLProcessing XMLP )
        {
            Int32 Result = XML_ERROR.NO_ERROR;

            this.OPT_LOG_MODEIR1 = XMLP.GetValue(LanguageName + "/OPT_LOG/OPT_LOG_MODEIR1", "", "", ATTRIB_TEXT);
            this.OPT_LOG_MODEIR2 = XMLP.GetValue(LanguageName + "/OPT_LOG/OPT_LOG_MODEIR2", "", "", ATTRIB_TEXT);
            this.OPT_LOG_MODEIR3 = XMLP.GetValue(LanguageName + "/OPT_LOG/OPT_LOG_MODEIR3", "", "", ATTRIB_TEXT);
            return Result;
        } // endMethod: InitOptionsLogiciels

        /// <summary>
        /// Initialiser les champs communs
        /// </summary>
        public Int32 InitCommonField ( String LanguageName, XMLProcessing XMLP )
        {
            Int32 Result = XML_ERROR.NO_ERROR;

            this.COMMON_FIELD_ON = XMLP.GetValue(LanguageName + "/COMMON_FIELD/COMMON_FIELD_ON", "", "", ATTRIB_TEXT);
            this.COMMON_FIELD_OFF = XMLP.GetValue(LanguageName + "/COMMON_FIELD/COMMON_FIELD_OFF", "", "", ATTRIB_TEXT);
            this.COMMON_FIELD_UNKNOW = XMLP.GetValue(LanguageName + "/COMMON_FIELD/COMMON_FIELD_UNKNOW", "", "", ATTRIB_TEXT);
            return Result;
        } // endMethod: InitCommonField

        /// <summary>
        /// Initialiser la langue pour les messages d'erreurs
        /// </summary>
        private Int32 InitError ( String LanguageName, XMLProcessing XMLP )
        {
            Int32 Result = XML_ERROR.NO_ERROR;

            this.ERROR_UNDESCRIBED = XMLP.GetValue(LanguageName + "/ERR_MSG/ERROR_UNDESCRIBED", "", "", ATTRIB_TEXT);
            this.ERROR_FILE_NOT_FOUND = XMLP.GetValue(LanguageName + "/ERR_MSG/ERROR_FILE_NOT_FOUND", "", "", ATTRIB_TEXT);

            return Result;
        } // endMethod: InitError

        /// <summary>
        /// Construire la liste des languages disponibles
        /// </summary>
        public Int32 BuildLanguagesList ( String Path )
        {
            Int32 Result = XML_ERROR.ERROR_UNDESCRIBED;
            String[] Files;

            Files = Directory.GetFiles(Path, "*.xml");
            if (Files.Length == 0)
            {
                Result = XML_ERROR.ERROR_FILE_NOT_FOUND;
                return Result;
            }

            this._languages = new ObservableCollection<String>();
            this._languagesFlags = new ObservableCollection<Image>();

            for (int i = 0; i < Files.Length; i++)
            {
                String LanguageName = System.IO.Path.GetFileNameWithoutExtension(Files[i]);
                if (!LanguageName.Equals("Italien"))
                {
                    this._languages.Add(LanguageName);

                    // Rechercher l'icon du drapeau qui va avec
                    String ImageFile = Path + LanguageName + ".bmp";
                    Image Img = new Image();
                    Img.Stretch = Stretch.Uniform;
                    Img.Height = 20;
                    Img.Margin = new Thickness(0, 0, 5, 0);
                    Img.Source = ImageUtilities.LoadImage(ImageFile);
                    this._languagesFlags.Add(Img);
                }
            }

            Result = XML_ERROR.NO_ERROR;
            return Result;
        } // endMethod: BuildLanguagesList

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: LanguageSupport
}
