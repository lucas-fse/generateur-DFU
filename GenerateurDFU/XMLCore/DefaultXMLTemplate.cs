using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;
using JAY.PegaseCore;
using JAY.FileCore;

namespace JAY.XMLCore
{
    public class DefaultXMLTemplate
    {
        // Variables singleton
        private static DefaultXMLTemplate _instance;
        static readonly object instanceLock = new object();

        // Variables
        #region Variables

        private String _templateFormuleHorsFormule;
        private String _templateConfigCmdPLD;
        private String _templateTableEquationHorsMode;
        private String _templateMnemologique;
        private String _templateEquation;
        private String _templateFormuleMode;
        private String _templateMaskMode;
        private String _templateMode;
        private String _templateLibelSelecteur;
        private String _templateSelecteur;
        private String _templateLibelRI;
        private String _templateRI;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le template des retours d'info
        /// </summary>
        public String TemplateRI
        {
            get
            {
                return this._templateRI;
            }
            private set
            {
                this._templateRI = value;
            }
        } // endProperty: TemplateRI

        /// <summary>
        /// Le template des libellés RI
        /// </summary>
        public String TemplateLibelRI
        {
            get
            {
                return this._templateLibelRI;
            }
            private set
            {
                this._templateLibelRI = value;
            }
        } // endProperty: TemplateLibelRI
                /// <summary>
        /// Le template des libellés RI
        /// </summary>
        public String TemplateAlarme
        {
            get;
            private set;
        } // endProperty: TemplateLibelRI
        
        /// <summary>
        /// Le template des sélecteurs
        /// </summary>
        public String TemplateSelecteur
        {
            get
            {
                return this._templateSelecteur;
            }
            private set
            {
                this._templateSelecteur = value;
            }
        } // endProperty: TemplateSelecteur

        /// <summary>
        /// Le template des libellés des sélecteurs
        /// </summary>
        public String TemplateLibelSelecteur
        {
            get
            {
                return this._templateLibelSelecteur;
            }
            private set
            {
                this._templateLibelSelecteur = value;
            }
        } // endProperty: TemplateLibelSelecteur

        /// <summary>
        /// Le template du mode
        /// </summary>
        public String TemplateMode
        {
            get
            {
                return this._templateMode;
            }
            private set
            {
                this._templateMode = value;
            }
        } // endProperty: TemplateMode

        /// <summary>
        /// Le contenu 
        /// </summary>
        public String TemplateMaskMode
        {
            get
            {
                return this._templateMaskMode;
            }
            private set
            {
                this._templateMaskMode = value;
            }
        } // endProperty: TemplateMaskMode

        /// <summary>
        /// Contenu d'une formule pour un mode
        /// </summary>
        public String TemplateFormuleMode
        {
            get
            {
                return this._templateFormuleMode;
            }
            private set
            {
                this._templateFormuleMode = value;
            }
        } // endProperty: TemplateFormuleMode

        /// <summary>
        /// Le template de l'information Mnemologique pour une équation
        /// </summary>
        public String TemplateMnemologique
        {
            get
            {
                return this._templateMnemologique;
            }
            private set
            {
                this._templateMnemologique = value;
            }
        } // endProperty: TemplateMnemologique

        /// <summary>
        /// Le template d'une équation
        /// </summary>
        public String TemplateEquation
        {
            get
            {
                return this._templateEquation;
            }
            private set
            {
                this._templateEquation = value;
            }
        } // endProperty: TemplateEquation

        /// <summary>
        /// Le template de base pour une formule
        /// </summary>
        public String TemplateFormuleModeUniversel
        {
            get
            {
                return this._templateFormuleHorsFormule;
            }
            private set
            {
                this._templateFormuleHorsFormule = value;
            }
        } // endProperty: TemplateFormuleModeUniversel

        /// <summary>
        /// Le template commande PLD
        /// </summary>
        public String TemplateConfigCmdPLD
        {
            get
            {
                return this._templateConfigCmdPLD;
            }
            private set
            {
                this._templateConfigCmdPLD = value;
            }
        } // endProperty: TemplateFormuleModeUniversel

        /// <summary>
        /// Le template de base pour une équation
        /// </summary>
        public String TemplateTableEquationModeUniversel
        {
            get
            {
                return this._templateTableEquationHorsMode;
            }
            set
            {
                this._templateTableEquationHorsMode = value;
            }
        } // endProperty: TemplateTableEquationModeUniversel

        /// <summary>
        /// L'instance unique de la classe
        /// </summary>
        public static DefaultXMLTemplate Instance
        {
            get
            {
                return DefaultXMLTemplate.Get();
            }
        } // endProperty: Instance

        #endregion

        // Constructeur
        #region Constructeur

        private DefaultXMLTemplate()
        {
            // Charger les différents templates de création du XML
            FilePackage FP = FilePackage.GetMaterielData();

            // 1 - le template des formules
            this.TemplateFormuleModeUniversel = this.GetTemplateFile(RefFiles.FORMULE_HEADER, FP);
            this.TemplateConfigCmdPLD = this.GetTemplateFile(RefFiles.CMD_PLD, FP);
            // 2 - le template des équations
            this.TemplateTableEquationModeUniversel = this.GetTemplateFile(RefFiles.TABLE_EQUATION, FP);

            // 3 - Les en-têtes équations et les équations
            Uri UriEquation = new Uri(RefFiles.EQUATION, UriKind.Relative);
            Stream StreamEquation = FP.GetPartStream(UriEquation);
            
            using (StreamReader SR = new StreamReader(StreamEquation))
            {
                this.TemplateMnemologique = SR.ReadLine();
                this.TemplateEquation = SR.ReadToEnd();
            }

            // 4 - le template des masques de modes
            this.TemplateMaskMode = this.GetTemplateFile(RefFiles.MASK_MODE, FP);

            // 5 - Formule du mode
            this.TemplateFormuleMode = this.GetTemplateFile(RefFiles.FORMULE, FP);

            // 6 - template du mode
            this.TemplateMode = this.GetTemplateFile(RefFiles.MODE, FP);

            // 7 - Selecteur
            this.TemplateSelecteur = this.GetTemplateFile(RefFiles.SELECTEUR, FP);

            // 8 - Libellé selecteur
            this.TemplateLibelSelecteur = this.GetTemplateFile(RefFiles.LIBELSELECTEUR, FP);

            // 9 - Retour d'information
            this.TemplateRI = this.GetTemplateFile(RefFiles.RETOURINFO, FP);

            // 10 - Libellé retour d'information
            this.TemplateLibelRI = this.GetTemplateFile(RefFiles.LIBELRETOURINFO, FP);

            // 11 - Alrmes
            this.TemplateAlarme = this.GetTemplateFile(RefFiles.ALARME, FP);

            // 11 - Fermer le fichier
            FP.ClosePackage();
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Charger un fichier template
        /// </summary>
        public String GetTemplateFile ( String partName, FilePackage FP )
        {
            String Result = "";
            Uri UriPart = new Uri(partName, UriKind.Relative);
            Stream StreamPart = FP.GetPartStream(UriPart);
            if (StreamPart != null)
            {
                if (Path.GetExtension(partName).ToLower() == ".txt")
                {
                    using (StreamReader SR = new StreamReader(StreamPart))
                    {
                        Result = SR.ReadToEnd();
                    }
                }
                else if(Path.GetExtension(partName).ToLower() == ".xml")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(StreamPart);
                    Result = doc.OuterXml;
                }
            }

            return Result;
        } // endMethod: GetTemplateFile

        // Retourne une instance unique de la classe
        private static DefaultXMLTemplate Get()
        {
            lock (instanceLock)
            {
                if (_instance == null)
                    _instance = new DefaultXMLTemplate();

                return _instance;
            }
        }

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: DefaultXMLTemplate
}
