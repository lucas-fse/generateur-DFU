using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using JAY.XMLCore;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Les options logiciels
    /// </summary>
    public class OptionsLogiciels
	{
        // Constantes
        #region Constantes

        public const Int32 MAX_LIBEL_SELECTEUR = 96;
        public const Int32 MAX_SELECTEUR = 128;
        public const Int32 MAX_LIBEL_INFO = 48;
        public const Int32 MAX_INFO = 16;
        public const Int32 MAX_EQUATIONS = 256;

        #endregion

        // Variables
        #region Variables

        // Variables singleton
        private static OptionsLogiciels _instance;
        static readonly object instanceLock = new object();

        //private XElement _RootOL;
        //private XMLProcessing _xmlTechnique;
        //private XMLProcessing _xmlMetier;
        private ObservableCollection<ConfigGainOffsetESAna> _configGainOffsetSortieAnas;
        private ObservableCollection<ConfigGainOffsetESAna> _gainsOffsetsEntrees;
        private ObservableCollection<CollecConfigRetour> _collecConfigRetours;
        private ObservableCollection<SelecteurLabel> _libelSelecteurs;
        private ObservableCollection<SelecteurLabel> _libelWHC;
        private ObservableCollection<InformationLabel> _libelRI;
        private ObservableCollection<ModeExploitation> _modeExploitation;
        private ObservableCollection<Selecteur> _selecteurs;
        private ObservableCollection<Information> _informations;
        private Random _rnd;

        private ModeExploitation _modeSecurite;

        #endregion
        
        // Propriétés
        #region Propriétés

        /// <summary>
        /// La collection des libellés pour le WHC
        /// </summary>
        public ObservableCollection<SelecteurLabel> LibelWHC
        {
            get
            {
                return this._libelWHC;
            }
            set
            {
                this._libelWHC = value;
            }
        } // endProperty: LibelWHC

        /// <summary>
        /// Le mode sécurité lié à la configuration
        /// </summary>
        public ModeExploitation ModeSecurite
        {
            get
            {
                if (this._modeSecurite == null)
                {
                    this._modeSecurite = new ModeExploitation(33);
                }
                return this._modeSecurite;
            }
            set
            {
                this._modeSecurite = value;
            }
        } // endProperty: ModeSecurite

        /// <summary>
        /// L'instance unique de la classe
        /// </summary>
        public static OptionsLogiciels Instance
        {
            get
            {
                return Get();
            }
        } // endProperty: Instance

        /// <summary>
        /// Le gain et offset des entrées
        /// </summary>
        public ObservableCollection<ConfigGainOffsetESAna> GainsOffsetsEntrees
        {
            get
            {
                return this._gainsOffsetsEntrees;
            }
            private set
            {
                this._gainsOffsetsEntrees = value;
            }
        } // endProperty: GainsOffsetsEntrees

        /// <summary>
        /// Le nombre de modes
        /// </summary>
        public Int32 NbModes
        {
            get
            {
                ObservableCollection<XElement> nbModes = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/ConfigModeExploit/ConfigNbModes/NbModesReels");
                if (nbModes == null)
                {
                    // La ligne NbModesReels n'existe pas, l'ajouter
                    nbModes = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/ConfigModeExploit/ConfigNbModes/NbModes");
                    if (nbModes != null)
                    {
                        XElement nbM = nbModes.First();
                        XElement nbModeReel = new XElement(nbM);
                        nbModeReel.Attribute(XML_ATTRIBUTE.CODE).Value = "NbModesReels";
                        nbM.AddAfterSelf(nbModeReel);
                    }
                }
                String nbMode;
                nbMode = nbModes.First().Attribute(XML_ATTRIBUTE.VALUE).Value;
                Int32 NbMode = 0;
                try
                {
                    NbMode = Convert.ToInt32(nbMode);
                }
                catch
                {
                    NbMode = 0;
                }
                return NbMode;
            }
            set
            {
                ObservableCollection<XElement> nbModes = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/ConfigModeExploit/ConfigNbModes/NbModesReels");
                XElement XnbMode;
                XnbMode = nbModes.First();
                if (XnbMode != null)
                {
                    XnbMode.Attribute(XML_ATTRIBUTE.VALUE).Value = value.ToString();
                }
            }
        } // endProperty: NbModes

        /// <summary>
        /// La liste des libellés des retours et alarmes
        /// </summary>
        public ObservableCollection<InformationLabel> LibelRI
        {
            get
            {
                if (this._libelRI == null)
                {
                    this.InitDicoRI();
                }
                return this._libelRI;
            }
        } // endProperty: LibelAlarmes

        /// <summary>
        /// Le dictionnaire de libellé des sélecteurs
        /// </summary>
        public ObservableCollection<SelecteurLabel> LibelSelecteurs
        {
            get
            {
                if (this._libelSelecteurs == null)
                {
                    this._libelSelecteurs = new ObservableCollection<SelecteurLabel>();
                    this.InitDicoLabel(); 
                }

                return this._libelSelecteurs;
            }
        } // endProperty: LibelSelecteurs

        /// <summary>
        /// La gestion de l'IR par les MO
        /// </summary>
        public Int32 GestionIR
        {
            get
            {
                Int32 V = 0;

                String SV = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresModifiables/GestionInfraRouge/Mode", "", "", XML_ATTRIBUTE.VALUE);
                String List = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresModifiables/GestionInfraRouge/Mode", "", "", XML_ATTRIBUTE.PLAGE_VALEUR);
                V = Tools.GetCorrespondantIntValue(List, SV);

                return V;
            }
            set
            {
                String List = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresModifiables/GestionInfraRouge/Mode", "", "", XML_ATTRIBUTE.PLAGE_VALEUR);
                String SV = Tools.GetCorrespondantStringValue(List, value);
                PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresModifiables/GestionInfraRouge/Mode", "", "", XML_ATTRIBUTE.VALUE, SV);
            }
        } // endProperty: GestionIR

        /// <summary>
        /// La table des gains et des offsets des sorties
        /// </summary>
        public ObservableCollection<ConfigGainOffsetESAna> GainsOffsetsSorties
        {
            get
            {
                return this._configGainOffsetSortieAnas;
            }
        } // endProperty: GainsSorties

        /// <summary>
        /// La table des configurations de retour
        /// </summary>
        public ObservableCollection<CollecConfigRetour> CollecConfigRetours
        {
            get
            {
                return this._collecConfigRetours;
            }
        } // endProperty: CollecConfigRetours

        /// <summary>
        /// La liste des modes d'exploitation
        /// </summary>
        public ObservableCollection<ModeExploitation> ModesExploitation
        {
            get
            {
                return this._modeExploitation;
            }
        } // endProperty: ModesExploitation

        /// <summary>
        /// La liste des sélecteurs
        /// </summary>
        public ObservableCollection<Selecteur> Selecteurs
        {
            get
            {
                return this._selecteurs;
            }
        } // endProperty: Selecteurs


        /// <summary>
        /// La liste des retours d'informations
        /// </summary>
        public ObservableCollection<Information> Informations
        {
            get
            {
                return this._informations;
            }
        } // endProperty: Informations

        #endregion
        
        // Constructeur
        #region Constructeur

        private OptionsLogiciels()
        {
            //this._xmlTechnique = new XMLProcessing();
            //this._xmlMetier = new XMLProcessing();
            // réinitialiser les collections
            this._rnd = new Random(DateTime.Now.Millisecond);
        }

        public void ConstructOptionsLogiciels(XElement TechOL, XElement RootParamAppFixe)
        {
            //this._RootOL = TechOL;

            //this._xmlTechnique.OpenXML(this._RootOL);
            //this._xmlMetier.OpenXML(RootParamAppFixe);

            this.InitCollecConfigRetour();
            this.InitSelecteur();
            this.InitRetourInfos();
            this.EvalLibelWHC();
        }
        
        #endregion
        
        // Méthodes
        #region Méthodes

        /// <summary>
        /// reconstruire la partie dico des sélecteurs
        /// </summary>
        private void SerialiseSelecteurs( )
        {
            // 1 - Effacer le contenu du dico des sélecteurs
            if (PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/DicosExploitation/DicoSelecteurs/Selecteur") != null)
            {
                var Query = from selecteur in PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/DicosExploitation/DicoSelecteurs/Selecteur")
                            select selecteur;
                List<XElement> Elements = Query.ToList();

                for (Int32 i = Elements.Count - 1; i >= 0; i--)
                {
                    Elements[i].Remove();
                }
            }
            // 2 - Construire tout le contenu du dictionnaire des sélecteurs
            foreach (var selecteur in this.Selecteurs)
            {
                XElement XSelecteur = this.AddXMLObject(PegaseCore.RefFiles.SELECTEUR).First();
                selecteur.SerialiseSelecteur(XSelecteur);
            }
        } // endMethod: SerialiseSelecteurs
        
        /// <summary>
        /// reconstruire la partie dico des libellés selecteurs
        /// </summary>
        private void SerialiseSelecteursLibellé ( )
        {
            // 1 - Effacer le contenu du dico des libellés des sélecteurs
            if (PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/DicosExploitation/DicoLibelleSelecteurs/LibellesSelecteur") != null)
            {
                var Query = from lselecteur in PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/DicosExploitation/DicoLibelleSelecteurs/LibellesSelecteur")
                            select lselecteur; 
            
                List<XElement> Elements = Query.ToList();

                for (int i = Query.Count() - 1; i >= 0 ; i--)
                {
                    Elements[i].Remove();
                }
            }
            // 2 - Construire tout le contenu du dictionnaire des libellés des sélecteurs
            foreach (var libelselecteur in this.LibelSelecteurs)
            {
                XElement XLSelecteur = this.AddXMLObject(PegaseCore.RefFiles.LIBELSELECTEUR).First();
                libelselecteur.SerialiseLibelSelecteur(XLSelecteur);
            }
        } // endMethod: SerialiseSelecteursLibellé

        /// <summary>
        /// reconstruire la partie dico des retours d'informations
        /// </summary>
        private void SerialiseRInformations ( )
        {
            // 1 - Effacer le contenu du dico des retours d'informations
            if (PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/DicosExploitation/DicoRetourInfo/RetourInfo") != null)
            {
                var Query = from ri in PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/DicosExploitation/DicoRetourInfo/RetourInfo")
                            select ri;

                List<XElement> Elements = Query.ToList();

                for (int i = Query.Count() - 1; i >= 0; i--)
                {
                    Elements[i].Remove();
                }
            }

            // 2 - Construire tout le dictionnaire des retours d'information
            foreach (var ri in this.Informations)
            {
                XElement XRI = this.AddXMLObject(PegaseCore.RefFiles.RETOURINFO).First();
                ri.SaveRI(XRI);
            }
        } // endMethod: SerialiseRInformations

        /// <summary>
        /// Les libellés des retours d'informations
        /// </summary>
        private void SerialiseRInformationLibelles ( )
        {
            // 1 - Effacer le contenu du dico des libellés des retours d'informations
            if (PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/DicosExploitation/DicoLibelleInformation/LibellesInformation") != null)
            {
                var Query = from lri in PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/DicosExploitation/DicoLibelleInformation/LibellesInformation")
                            select lri;

                List<XElement> Elements = Query.ToList();

                for (int i = Elements.Count - 1; i>=0; i--)
                {
                    Elements[i].Remove();
                }
            }
            // 2 - Construire tout le dictionnaire des libellés des retours d'informations
            foreach (var lri in this.LibelRI)
            {
                XElement XLRI = this.AddXMLObject(PegaseCore.RefFiles.LIBELRETOURINFO).First();
                lri.SerialiseXML(XLRI);
            }
        } // endMethod: SerialiseRInformationLibelles
        
        /// <summary>
        /// Reconstruire la partie XML des modes
        /// </summary>
        private void SerialiseModes()
        {
            //System.Windows.MessageBox.Show("Enregistrement des modes désactivés...");
            //return;
            // 1 - Effacer tous les modes et les reconstruires
            if (PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/ModesExploitation/ModeExploitation") != null)
            {
                var QueryMode = from mode in PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/ModesExploitation/ModeExploitation")
                                select mode;

                var QueryDescriptInterverrouillage = from Di in PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/ModesExploitation/DescriptInterverrouillage")
                                                     select Di;

                var QueryConfigSelecteursMode = from Csm in PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/ModesExploitation/ConfigSelecteursMode")
                                                select Csm;

                var QueryCollecConfigRetour = from CCR in PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/ModesExploitation/CollecConfigRetour")
                                              select CCR;

                var QueryCollecFormules = from CF in PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/ModesExploitation/CollecFormules")
                                          select CF;

                List<XElement> Mode2Delete = QueryMode.ToList();
                List<XElement> DI2Delete = QueryDescriptInterverrouillage.ToList();
                List<XElement> Csm2Delete = QueryConfigSelecteursMode.ToList();
                List<XElement> CCR2Delete = QueryCollecConfigRetour.ToList();
                List<XElement> CF2Delete = QueryCollecFormules.ToList();

                for (int i = Mode2Delete.Count() - 1; i >= 0; i--)
                {
                    CF2Delete[i].Remove();
                    CCR2Delete[i].Remove();
                    Csm2Delete[i].Remove();
                    DI2Delete[i].Remove();
                    Mode2Delete[i].Remove();
                }
                
                List<XElement> test = CF2Delete[0].Elements().ToList();
                for(int i = test.Count() - 1; i == 0; i--)
                {
                    test[i].Remove();

                }
            }

            // 
            List<AnalyseEquation.EquationBymode> list = new List<AnalyseEquation.EquationBymode>();
            if (PegaseData.Instance.ParamHorsMode.EquationTxt != null)
            {
                JAY.PegaseCore.AnalyseEquation analyse = new AnalyseEquation();
                string result;
                analyse.RechercheCommentaire(PegaseData.Instance.ParamHorsMode.EquationTxt, out result);
                list = analyse.AnalyseEquationByModeByFonction(ref result);
            }
            // 2 - reconstruire tous les modes

            foreach (var mode in this.ModesExploitation)
            {
                List<XElement> modeNodes = this.AddXMLObject(PegaseCore.RefFiles.MODE).ToList();
                XElement XMode = modeNodes[0];
                XElement Interverouillage = modeNodes[1];
                XElement Selecteurs = modeNodes[2];
                XElement RetoursInfo = modeNodes[3];
                XElement Formules = modeNodes[4];
                mode.SerialiseMode(XMode, Interverouillage, Selecteurs, RetoursInfo, Formules, list);
            }

            // 3 - Ajouter le mode en sécurité s'il existe
            if (this._modeSecurite != null)
            {
                List<XElement> modeNodes = this.AddXMLObject(PegaseCore.RefFiles.MODE).ToList();
                XElement XMode = modeNodes[0];
                XElement Interverouillage = modeNodes[1];
                XElement Selecteurs = modeNodes[2];
                XElement RetoursInfo = modeNodes[3];
                XElement Formules = modeNodes[4];
                this.ModeSecurite.SerialiseMode(XMode, Interverouillage, Selecteurs, RetoursInfo, Formules, list);
            }

            // 4 - Supprimer les masques utilisateurs / modes
            ObservableCollection<XElement> XMaskUserMode;
            XElement RootMaskUserMode;

            XMaskUserMode = PegaseData.Instance.XMLFile.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsMasques/Modes");
            RootMaskUserMode = PegaseData.Instance.XMLFile.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsMasques").First();

            if (XMaskUserMode != null)
            {
                foreach (XElement mMode in XMaskUserMode)
                {
                    mMode.Remove();
                }
            }

            // 5 - Reconstruire les masques utilisateurs / modes

            if (DefaultXMLTemplate.Instance.TemplateMaskMode != "")
            {
                for (int i = 0; i < this.NbModes; i++)
                {
                    XElement element = this.SerializeXMLMaskUserMode(DefaultXMLTemplate.Instance.TemplateMaskMode, i);
                    RootMaskUserMode.Add(element);
                }
            }
        } // endMethod: SerialiseModes
        
        /// <summary>
        /// Sérialiser les données d'un mode
        /// </summary>
        public XElement SerializeXMLMaskUserMode ( String XML, Int32 NumMode )
        {
            XElement Result;
            Int32 OffsetAbsolu, OffsetRelatif, Taille, NumMask;
            List<XElement> CategoriesXML, VariablesXML;

            Result = XElement.Parse(XML);

            if (Result != null)
            {
                // 1 - initialiser les valeurs initiales des offsets de la structure complète
                String XOffAbsolu = Result.Attribute(XML_ATTRIBUTE.OFFSET_ABSOLU).Value;
                OffsetAbsolu = Convert.ToInt32(XOffAbsolu);
                String XOffRelatif = Result.Attribute(XML_ATTRIBUTE.OFFSET_RELATIF).Value;
                OffsetRelatif = Convert.ToInt32(XOffRelatif);
                String XTaille = Result.Attribute(XML_ATTRIBUTE.TAILLE).Value;
                Taille = Convert.ToInt32(XTaille);

                OffsetAbsolu = OffsetAbsolu + NumMode * Taille;
                OffsetRelatif = OffsetRelatif + NumMode * Taille;

                Result.Attribute(XML_ATTRIBUTE.OFFSET_ABSOLU).Value = OffsetAbsolu.ToString();
                Result.Attribute(XML_ATTRIBUTE.OFFSET_RELATIF).Value = OffsetRelatif.ToString();

                // 2 - initialiser les valeurs initiales des offsets des catégories 
                CategoriesXML = Result.Descendants("Categorie").ToList<XElement>();
                NumMask = 0;
                foreach (XElement categorie in CategoriesXML)
                {
                    categorie.Attribute(XML_ATTRIBUTE.OFFSET_ABSOLU).Value = OffsetAbsolu.ToString();
                    categorie.Attribute(XML_ATTRIBUTE.OFFSET_RELATIF).Value = OffsetRelatif.ToString();

                    // 3 - Mettre à jour les variables
                    VariablesXML = categorie.Descendants("Variable").ToList<XElement>();

                    foreach (XElement variableXML in VariablesXML)
                    {
                        String Value;
                        variableXML.Attribute(XML_ATTRIBUTE.OFFSET_ABSOLU).Value = OffsetAbsolu.ToString();
                        variableXML.Attribute(XML_ATTRIBUTE.OFFSET_RELATIF).Value = OffsetRelatif.ToString();
                        switch (variableXML.Attribute(XML_ATTRIBUTE.CODE).Value)
                        {
                            case "MasqueBoutonsToggle":
                                Value = this.ModesExploitation[NumMode].UserModeMask[NumMask].Mask32.ToString();
                                //Value = XMLCore.Tools.UInt32ToBitfield32(this.ModesExploitation[NumMode].UserModeMask[NumMask].Mask32);
                                variableXML.Attribute(XML_ATTRIBUTE.VALUE).Value = Value;
                                break;
                            case "MasqueCom12AxesPot":
                                Value = this.ModesExploitation[NumMode].UserModeMask[NumMask].Mask16.ToString();
                                //Value = XMLCore.Tools.UInt16ToBitfield16(this.ModesExploitation[NumMode].UserModeMask[NumMask].Mask16);
                                variableXML.Attribute(XML_ATTRIBUTE.VALUE).Value = Value;
                                break;
                            default:
                                break;
                        }
                        XTaille = variableXML.Attribute(XML_ATTRIBUTE.TAILLE).Value;
                        Taille = Convert.ToInt32(XTaille);
                        OffsetAbsolu += Taille;
                        OffsetRelatif += Taille;
                    }
                    NumMask++;
                }
            }

            return Result;
        } // endMethod: SerializeXMLMaskUserMode

        /// <summary>
        /// Collecter la liste de tous les libellés WHC
        /// </summary>
        public void EvalLibelWHC()
        {
            ObservableCollection<SelecteurLabel> WHCLabel = new ObservableCollection<SelecteurLabel>();
            Int32 i;

            if (this.LibelSelecteurs != null && this.LibelSelecteurs.Count > 0)
            {
                //var Query = from libel in this.LibelSelecteurs
                //            where libel.IdentLibelSelecteur.Contains("FIXE_WHC")
                //            select libel;

                //if (Query.Count() > 0)
                i = 0;
                while (i < this.LibelSelecteurs.Count && this.LibelSelecteurs[i].IdentLibelSelecteur.Contains("FIXE_WHC"))
                {
                    this.LibelSelecteurs[i].NumLibelSelecteur = i;
                    this.LibelSelecteurs[i].IdentLibelSelecteur = String.Format("{0}{1:0000}", Constantes.PREFIX_LIBEL_WHC, i);
                    WHCLabel.Add(this.LibelSelecteurs[i]);
                    i++;
                }
            }

            this.LibelWHC = WHCLabel;
        } // endMethod: EvalLibelWHC

        /// <summary>
        /// Reconstruire le XML en fonction des données des options logiciels
        /// </summary>
        public void SaveOptionsLogiciels()
        {
            // 1 - Modes + Mode de sécurité
            this.SerialiseModes();

            // 2 - Dico des sélecteurs
            this.SerialiseSelecteurs();

            // 3 - Dico des libellés sélecteurs
            this.SerialiseSelecteursLibellé();

            // 4 - Dico des retours d'informations
            this.SerialiseRInformations();

            // 5 - Dico des libellés des retours d'informations
            this.SerialiseRInformationLibelles();

            // 6 - Enregistrer le mask par mode de l'homme mort
            UInt32 MaskHM = this.CalculMaskHM();
            PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionHommeMort/Reglage/MasqueModes", "", "", XML_ATTRIBUTE.VALUE, MaskHM.ToString());
        } // endMethod: SaveOptionsLogiciels

        // Retourne une instance unique de la classe
        private static OptionsLogiciels Get()
        {
            lock (instanceLock)
            {
                if (_instance == null)
                    _instance = new OptionsLogiciels();
                return _instance;
            }
        }

        /// <summary>
        /// Ajouter l'objet XML provenant du fichier Materiel et nommés par objectType
        /// </summary>
        private ObservableCollection<XElement> AddXMLObject ( String objectType )
        {
            ObservableCollection<XElement> Result = null;
            String XMLNodeString = "";
            XMLCore.XMLCreation Create;

            // Charger le fichier
            switch (objectType)
            {
                case PegaseCore.RefFiles.MODE:
                    XMLNodeString = DefaultXMLTemplate.Instance.TemplateMode;
                    break;
                case PegaseCore.RefFiles.EQUATION:
                    XMLNodeString = DefaultXMLTemplate.Instance.TemplateEquation;
                    break;
                case PegaseCore.RefFiles.FORMULE:
                    XMLNodeString = DefaultXMLTemplate.Instance.TemplateFormuleMode;
                    break;
                case PegaseCore.RefFiles.FORMULE_HEADER:
                    XMLNodeString = DefaultXMLTemplate.Instance.TemplateFormuleModeUniversel;
                    break;
                case PegaseCore.RefFiles.LIBELRETOURINFO:
                    XMLNodeString = DefaultXMLTemplate.Instance.TemplateLibelRI;
                    break;
                case PegaseCore.RefFiles.LIBELSELECTEUR:
                    XMLNodeString = DefaultXMLTemplate.Instance.TemplateLibelSelecteur;
                    break;
                case PegaseCore.RefFiles.MASK_MODE:
                    XMLNodeString = DefaultXMLTemplate.Instance.TemplateMaskMode;
                    break;
                case PegaseCore.RefFiles.RETOURINFO:
                    XMLNodeString = DefaultXMLTemplate.Instance.TemplateRI;
                    break;
                case PegaseCore.RefFiles.SELECTEUR:
                    XMLNodeString = DefaultXMLTemplate.Instance.TemplateSelecteur;
                    break;
                case PegaseCore.RefFiles.TABLE_EQUATION:
                    XMLNodeString = DefaultXMLTemplate.Instance.TemplateTableEquationModeUniversel;
                    break;
                case PegaseCore.RefFiles.ALARME:
                    XMLNodeString = DefaultXMLTemplate.Instance.TemplateAlarme;
                    break;
                default:
                    return Result;
            }

            Create = new XMLCore.XMLCreation(PegaseData.Instance.Document);
            
            XDocument xmlNode = XDocument.Parse(XMLNodeString);

            // Ajouter le fichier
            Result = Create.AddFirstElement(xmlNode);

            // Récupérer le document XML
            PegaseData.Instance.Document = Create.DocumentXml;

            return Result;
        } // endMethod: AddObject

        /// <summary>
        /// Supprimer le mode dans le fichier XML
        /// </summary>
        public void DeleteMode(ModeExploitation mode)
        {
            if (PegaseData.Instance.OLogiciels.NbModes > 1)
            {
                // Supprimer les commentaires du mode
                PegaseData.Instance.Commentaire.RemoveMode(mode);

                // Supprimer le mode
                this.ModesExploitation.Remove(mode);
                PegaseData.Instance.OLogiciels.NbModes -= 1;
                //PegaseData.Instance.InitOptionsLogiciels(true);

                // Renuméroter tous les modes de 1 à n en continu
                for (int i = 0; i < this.ModesExploitation.Count; i++)
                {
                    // Si le mode n'est pas un mode spécial, le renuméroter
                    if (this.ModesExploitation[i].Position < 32)
                    {
                        this.ModesExploitation[i].Position = i;
                        // Renumeroter toutes les formules pour les maintenir associées au mode
                        foreach (var formule in this.ModesExploitation[i].Formules)
                        {
                            formule.NumModeExploit = i;
                        } 
                    }
                }
            }
        } // endMethod: DeleteMode

        /// <summary>
        /// Ajouter un mode à la suite du dernier mode
        /// </summary>
        public void AddMode()
        {
            if (PegaseData.Instance.OLogiciels.NbModes < 32)
            {
                ModeExploitation mode = new ModeExploitation(PegaseData.Instance.OLogiciels.NbModes);
                mode.ModeLabel = PegaseData.Instance.OLogiciels.AddLibelSelecteur();
                this.ModesExploitation.Add(mode);
                PegaseData.Instance.OLogiciels.NbModes += 1;
                // Ajouter les commentaires de la section commentaire
                PegaseData.Instance.Commentaire.AddMode(mode);
            }
        } // endMethod: AddMode

        /// <summary>
        /// Ajouter un sélecteur
        /// </summary>
        public Selecteur AddSelecteur( )
        {
            String ID;
            Selecteur Result;

            ID = this.GetSelecteurID();
            Result = new Selecteur(ID);
            this.Selecteurs.Add(Result);

            // 3 : Retourner le sélecteur avec le bon ID
            return Result;
        } // endMethod: AddSelecteur

        /// <summary>
        /// Supprimer un sélecteur
        /// </summary>
        public void DeleteSelecteur ( Selecteur selecteur )
        {
            this.Selecteurs.Remove(selecteur);
        } // endMethod: DeleteSelecteur
        
        /// <summary>
        /// Ajouter un libellé WHC
        /// </summary>
        private String AddWHCLibel ( String FileName )
        {
            String ID;
            Int32 num;
            SelecteurLabel SL;

            num = this.LibelWHC.Count;
            ID = String.Format("{0}{1:0000}", Constantes.PREFIX_LIBEL_WHC, num);

            SL = new SelecteurLabel(ID);
            // Transformer le label en direct to bmp
            SL.LibelSelecteur = Constantes.DIRECT_TO_BMP;
            SL.NomFichierBitmapSelecteur = FileName;

            this.LibelSelecteurs.Insert(num, SL);
            this.EvalLibelWHC();
            return ID;
        } // endMethod: AddWHCLibelSelecteur

        /// <summary>
        /// Ajouter des libellés WHC par lot
        /// </summary>
        public void AddWHCLibels ( List<String> FileNames )
        {
            // 1 - créer tous les libellés
            foreach (String file in FileNames)
            {
                this.AddWHCLibel(file);
            }
            // 2 - importer les bitmaps
            XMLTools.ImportNewBitmaps();
        } // endMethod: AddWHCLibels

        /// <summary>
        /// Supprimer le libellé WHC et l'image du fichier iDialog
        /// Pour la suppression par lot, utiliser DeleteWHCLibels (plus performant)
        /// </summary>
        public void DeleteWHCLibel(SelecteurLabel Libel)
        {
            if (Libel.NomFichierBitmapSelecteur != "")
            {
                PegaseData.Instance.CurrentPackage.DeletePackagePart(new Uri(Libel.NomFichierBitmapSelecteur, UriKind.Relative));
            }
            this.LibelSelecteurs.Remove(Libel);
            this.EvalLibelWHC();
        } // endMethod: DeleteWHCLibel

        /// <summary>
        /// Supprimer les libellés par lot
        /// </summary>
        public void DeleteWHCLibels(List<SelecteurLabel> Libels)
        {
            foreach (SelecteurLabel libel in Libels)
            {
                if (libel.NomFichierBitmapSelecteur != "")
                {
                    PegaseData.Instance.CurrentPackage.DeletePackagePart(new Uri(libel.NomFichierBitmapSelecteur, UriKind.Relative));
                }
                this.LibelSelecteurs.Remove(libel);
            }

            this.EvalLibelWHC();
        } // endMethod: DeleteWHCLibles

        /// <summary>
        /// Ajouter un libellé sélecteur
        /// </summary>
        public SelecteurLabel AddLibelSelecteur ( )
        {
            String ID;

            ID = this.GetLibelSelecteurID();
            SelecteurLabel label = new SelecteurLabel(ID);
            this.LibelSelecteurs.Add(label);

            return label;
        } // endMethod: AddLibelSelecteur

        /// <summary>
        /// Supprimer le libellé d'un sélecteur
        /// </summary>
        public void DeleteLibelSelecteur ( SelecteurLabel LibelSelecteur )
        {
            this.LibelSelecteurs.Remove(LibelSelecteur);
        } // endMethod: DeleteLibelSelecteur

        /// <summary>
        /// Ajouter un retour d'information
        /// </summary>
        public Information AddRetourInfo ( )
        {
            Information Result;
            String ID;
            // 1 : Ajouter le retour d'information

            ID = this.GetRetourInfoID();
            Result = new Information(ID);
            this.Informations.Add(Result);

            // 2 : Retourner le sélecteur avec le bon ID
            return Result;
        } // endMethod: AddRetourInfo
        
        /// <summary>
        /// Supprimer un retour d'information
        /// </summary>
        public void DeleteRetourInfo ( Information RetourInfo )
        {
            this.Informations.Remove(RetourInfo);
        } // endMethod: DeleteRetourInfo

        /// <summary>
        /// Ajout d'un libellé de retour d'info
        /// </summary>
        public InformationLabel AddLibelRetourInfo ( )
        {
            InformationLabel Result;
            String ID;

            // 1 : Définir l'Id du Libellé du retour d'information
            ID = this.GetLibelRetourInfoID();
            Result = new InformationLabel(ID);
            this.LibelRI.Add(Result);

            // 2 : Retourner le retour d'information
            return Result;
        } // endMethod: AddLibelRetourInfo
        
        /// <summary>
        /// Supprimer un libellé de retour d'info
        /// </summary>
        public void DeleteLibelRetourInfo ( InformationLabel LabelRetourInfo )
        {
            this.LibelRI.Remove(LabelRetourInfo);
        } // endMethod: DeleteLibelRetourInfo
        
        /// <summary>
        /// Parcourir les masques des modes et vérifier s'il en existe au moins un utilisé (!= 0xFFFFFFFF && != 0xFFFF)
        /// </summary>
        public Boolean ThereIsMaskInMode ( )
        {
            Boolean Result = false;

            foreach (var mode in this.ModesExploitation)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (mode.UserModeMask[i].Mask16 != 0xFFFF || mode.UserModeMask[i].Mask32 != 0xFFFFFFFF)
                    {
                        Result = true;
                    }
                }
            }

            return Result;
        } // endMethod: ThereIsMaskInMode

        /// <summary>
        /// Supprimer tous les masks de tous les modes
        /// </summary>
        public void ClearAllModeMask ( )
        {
            for (int i = 0; i < 4; i++)
            {
                this.ClearAllModeMask((TypePin)i);
            }
        } // endMethod: ClearAllModeMask

        /// <summary>
        /// Supprimer le mask de tous les modes du type défini
        /// </summary>
        public void ClearAllModeMask ( TypePin typePin )
        {
            for (int i = 0; i < this.NbModes; i++)
            {
                this.ClearModeMask(typePin, i);
            }
        } // endMethod: ClearMask
        
        /// <summary>
        /// Supprimer le masque du mode et du code pin spécifié
        /// </summary>
        public void ClearModeMask ( TypePin typePin, Int32 numMode )
        {
            this.ModesExploitation[numMode].UserModeMask[(Int32)typePin].Mask16 = 0xFFFF;
            this.ModesExploitation[numMode].UserModeMask[(Int32)typePin].Mask32 = 0xFFFFFFFF;
        } // endMethod: ClearModeMask

        /// <summary>
        /// Fabriquer un ID unique pour un libellé de sélecteur
        /// </summary>
        /// <returns></returns>
        public String GetLibelSelecteurID()
        {
            String Result;
            while (true)
            {
                Result = this.GetID("AutoLibel");

                var Query = from row in this.LibelSelecteurs
                            where row.IdentLibelSelecteur == Result
                            select row;

                if (Query.Count() == 0)
                {
                    break;
                }
            }
            return Result;
        }

        /// <summary>
        /// Fabriquer un ID unique pour un libellé de retour d'infos
        /// </summary>
        public String GetLibelRetourInfoID ( )
        {
            String Result;
            while (true)
            {
                Result = this.GetID("AutoLibelRI");

                var Query = from row in this.LibelRI
                            where row.IdentLibelInformation == Result
                            select row;

                if (Query.Count() == 0)
                {
                    break;
                }
            }
            return Result;
        } // endMethod: GetLibelRetourInfo
        
        /// <summary>
        /// ID du sélecteur
        /// </summary>
        public String GetSelecteurID ( )
        {
            String Result;
            while (true)
            {
                Result = this.GetID("AutoSelect");

                var Query = from row in this.Selecteurs
                            where row.IdentSelecteur == Result
                            select row;

                if (Query.Count() == 0)
                {
                    break;
                }
            }
            return Result;
        } // endMethod: GetSelecteurID
        
        /// <summary>
        /// ID unique pour un Retour d'info 
        /// </summary>
        public String GetRetourInfoID ( )
        {
            String Result;
            while (true)
            {
                Result = this.GetID("AutoRInfo");

                var Query = from row in this.Informations
                            where row.IdentRetour == Result
                            select row;

                if (Query.Count() == 0)
                {
                    break;
                }
            }
            return Result;
        } // endMethod: GetRetourInfoID

        /// <summary>
        /// Fabriquer un ID résultant de la concaténation d'une chaine de base et d'un nombre tiré aléatoirement
        /// </summary>
        private String GetID ( String Base )
        {
            String Result;
            Int32 num = this._rnd.Next(5000);

            Result = String.Format("{0}_{1:0000}", Base, num);

            return Result;
        } // endMethod: GetID
        
        /// <summary>
        /// Obtenir le libellé sélecteur désigné par cet ID
        /// </summary>
        public SelecteurLabel GetSelecteurLabelByID ( String IdSelecteurLabel )
        {
            SelecteurLabel Result;

            var Query = from ls in this.LibelSelecteurs
                        where ls.IdentLibelSelecteur == IdSelecteurLabel
                        select ls;

            Result = Query.FirstOrDefault();

            return Result;
        } // endMethod: GetSelecteurLabelByID

        /// <summary>
        /// Clos les données proprement
        /// </summary>
        public void Dispose ( )
        {
            //this._xmlMetier.Close();
            //this._xmlTechnique.Close();
            this._modeExploitation = null;
            this._selecteurs = null;
            this._libelSelecteurs = null;
            this._libelRI = null;
            this._informations = null;
            this._collecConfigRetours = null;
            this._configGainOffsetSortieAnas = null;
            this._gainsOffsetsEntrees = null;
            this._modeSecurite = null;
        } // endMethod: Dispose

        /// <summary>
        /// Initialiser le dictionnaire des labels
        /// Ils sont initialisé à partir du XML Métier
        /// </summary>
        public Int32 InitDicoLabel ( )
        {
            Int32 Result = XML_ERROR.NO_ERROR;

            ObservableCollection<XElement> Dico = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/DicosExploitation/DicoLibelleSelecteurs/LibellesSelecteur");
            this._libelSelecteurs = new ObservableCollection<SelecteurLabel>();

            if (Dico != null)
            {
                foreach (XElement item in Dico)
                {
                    SelecteurLabel LS = new SelecteurLabel(item);
                    LS.NumLibelSelecteur = LS.IdSelecteur;
                    this._libelSelecteurs.Add(LS);
                }
            }

            return Result;
        } // endMethod: InitDicoLabel
        
        /// <summary>
        /// Initialiser le dictionnaire des libellés des retours d'informations
        /// </summary>
        public Int32 InitDicoRI ( )
        {
            Int32 Result = XML_ERROR.NO_ERROR;

            ObservableCollection<XElement> Dico = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/DicosExploitation/DicoLibelleInformation/LibellesInformation");
            this._libelRI = new ObservableCollection<InformationLabel>();

            if (Dico != null)
            {
                foreach (XElement item in Dico)
                {
                    InformationLabel IL = new InformationLabel(item);
                    IL.NumLibelInformation = IL.IdSelecteur;
                    if (IL.IdentLibelInformation != null)
                    {
                        this._libelRI.Add(IL);
                    }
                } 
            }

            return Result;
        } // endMethod: InitDicoAlarme
        
        /// <summary>
        /// Vérifier si l'ensemble des libellés sélecteurs sont utilisés
        /// Supprimer les libellés non-utilisés
        /// </summary>
        public void OptimiseLibelSelecteur ( )
        {
            // 1 - construire la collection de tous les libellés sélecteurs utilisés dans les différents modes
            ObservableCollection<SelecteurLabel> labelUsed = new ObservableCollection<SelecteurLabel>();

            foreach (var mode in this.ModesExploitation)
            {
                if (mode.ModeLabel != null)
                {
                    labelUsed.Add(mode.ModeLabel);
                }
                foreach (var labels in mode.LabelsSelecteurByPos)
                {
                    foreach (var label in labels)
                    {
                        if (label != null)
                        {
                            labelUsed.Add(label);
                        }
                    }
                }
            }

            // 2 - construire la liste de tous les libellés sélecteurs non utilisés
            ObservableCollection<SelecteurLabel> labelNotUsed = new ObservableCollection<SelecteurLabel>();
            foreach (var label in this.LibelSelecteurs)
            {
                var Query = from l in labelUsed
                            where label.IdentLibelSelecteur == l.IdentLibelSelecteur
                            select l;

                if (Query.Count() == 0)
                {
                    labelNotUsed.Add(label);
                }
            }

            // 3 - supprimer les libellés sélecteurs non utilisés
            for (int i = 0; i < labelNotUsed.Count; i++)
            {
                if (labelNotUsed[i].IdentLibelSelecteur.Length >= 3)
                {
                    if (labelNotUsed[i].IdentLibelSelecteur.Substring(0, 3).ToUpper() != "FIX")
                    {
                        this.DeleteLibelSelecteur(labelNotUsed[i]);
                    }
                }
                else
                {
                    this.DeleteLibelSelecteur(labelNotUsed[i]);
                }
            }
        } // endMethod: OptimiseLibelSelecteur
        
        /// <summary>
        /// Supprimer tous les sélecteurs sans références
        /// </summary>
        public void OptimiseSelecteur ( )
        {
            // Construire une collection de tous les sélecteurs utilisés dans les différents modes
            ObservableCollection<Selecteur> selecteurUsed = new ObservableCollection<Selecteur>();

            foreach (var mode in this.ModesExploitation)
            {
                foreach (var selecteur in mode.Selecteurs)
                {
                    if (selecteur != null)
                    {
                        selecteurUsed.Add(selecteur);
                    }
                }
            }

            // Vérifier avec le dico des sélecteurs et sélectionner tous les sélecteurs non utilisés
            ObservableCollection<Selecteur> selecteurNotUsed = new ObservableCollection<Selecteur>();

            foreach (var selecteur in this.Selecteurs)
            {
                var query = from s in selecteurUsed
                            where selecteur.IdentSelecteur == s.IdentSelecteur
                            select s;

                if (query.Count() == 0)
                {
                    selecteurNotUsed.Add(selecteur);
                }
            }

            // -> supprimer tous les sélecteurs non utilisés
            for (int i = 0; i < selecteurNotUsed.Count; i++)
            {
                this.DeleteSelecteur(selecteurNotUsed[i]);
            }
        } // endMethod: OptimiseSelecteur
        
        /// <summary>
        /// Supprimer tous les retours d'infos sans références
        /// </summary>
        public void OptimiseRetourInfo ( )
        {
            // 1 - faire la liste de tous les retours d'informations utilisés
            ObservableCollection<Information> RInfoUsed = new ObservableCollection<Information>();
            foreach (var mode in this.ModesExploitation)
            {
                foreach (var ri in mode.Informations)
                {
                    if (ri != null)
                    {
                        RInfoUsed.Add(ri);
                    }
                }
            }

            // 2 - faire la liste de tous les retours d'informations non utilisés
            ObservableCollection<Information> RInfoNotUsed = new ObservableCollection<Information>();
            foreach (var ri in this.Informations)
            {
                var query = from rinfo in RInfoUsed
                            where ri.IdentRetour == rinfo.IdentRetour
                            select rinfo;

                if (query.Count() == 0)
                {
                    RInfoNotUsed.Add(ri);
                }
            }

            // 3 - supprimer les retours d'informations non utilisés
            for (int i = 0; i < RInfoNotUsed.Count; i++)
            {
                this.DeleteRetourInfo(RInfoNotUsed[i]);
            }
        } // endMethod: OptimiseRetourInfo
        
        /// <summary>
        /// Supprimer tous les retours d'information sans références
        /// </summary>
        public void OptimiseLibelRetourInfo ( )
        {
            // 1 - faire la liste de tous les libellés de retours d'informations utilisés dans les modes
            ObservableCollection<InformationLabel> ILabelUsed = new ObservableCollection<InformationLabel>();
            foreach (var mode in this.ModesExploitation)
            {
                foreach (var label in mode.LabelsInfoByPos)
                {
                    if (label != null)
                    {
                        if (label.InfoLibelMax != null)
                        {
                            ILabelUsed.Add(label.InfoLibelMax);
                        }
                        if (label.InfoLibelMin != null)
                        {
                            ILabelUsed.Add(label.InfoLibelMin);
                        }
                        if (label.InfoLibelRetour != null)
                        {
                            ILabelUsed.Add(label.InfoLibelRetour);
                        }
                        if (label.InfoLibelUnit != null)
                        {
                            ILabelUsed.Add(label.InfoLibelUnit);
                        }
                        foreach (InformationLabel libelVarNum in label.LibelsVarNumerique)
                        {
                            if (libelVarNum != null)
                            {
                                ILabelUsed.Add(libelVarNum);
                            }
                        }
                    }
                }
            }
            // 1.1 - faire la liste de tous les libellés de retours d'information utilisés dans les alarmes
            if (PegaseData.Instance.ParamHorsMode != null)
            {
                foreach (var alarme in PegaseData.Instance.ParamHorsMode.Alarmes)
                {
                    if (alarme.IdentLibelInformation != "")
                    {
                        var Query = from libel in this.LibelRI
                                    where libel.IdentLibelInformation == alarme.IdentLibelInformation
                                    select libel;

                        InformationLabel IL = Query.FirstOrDefault();

                        if (IL != null)
                        {
                            ILabelUsed.Add(IL);
                        }
                    }
                }
            }
            // 1.2 - faire la liste de tous les libellés de retours d'information utilisés dans les alarmes
            if (PegaseData.Instance.ParamHorsMode != null)
            {
                foreach (var alarme in PegaseData.Instance.ParamHorsMode.Alarmes)
                {
                    if (alarme.IdentLibelTitre != "")
                    {
                        var Query = from libel in this.LibelRI
                                    where libel.IdentLibelInformation == alarme.IdentLibelTitre
                                    select libel;

                        InformationLabel IL = Query.FirstOrDefault();

                        if (IL != null)
                        {
                            ILabelUsed.Add(IL);
                        }
                    }
                }
            }
            // 2 - faire la liste de tous les libellés de retours d'informations non utilisés
            ObservableCollection<InformationLabel> ILabelNotUsed = new ObservableCollection<InformationLabel>();
            foreach (var label in this.LibelRI)
            {
                var Query = from l in ILabelUsed
                            where label.IdentLibelInformation == l.IdentLibelInformation
                            select l;

                if (Query.Count() == 0)
                {
                    ILabelNotUsed.Add(label);
                }
            }

            // 3 - supprimer les libellés de retour d'information non utilisés
            for (int i = 0; i < ILabelNotUsed.Count; i++)
            {
                if (ILabelNotUsed[i].IdentLibelInformation.Length >= 3)
                {
                    if (ILabelNotUsed[i].IdentLibelInformation.Substring(0, 3).ToUpper() != "FIX")
                    {
                        this.DeleteLibelRetourInfo(ILabelNotUsed[i]);
                    } 
                }
                else
                {
                    this.DeleteLibelRetourInfo(ILabelNotUsed[i]);
                }
            }
        } // endMethod: OptimiseLibelRetourInfo

        /// <summary>
        /// Initialiser la liste des modes
        /// </summary>
        public Int32 InitModes( )
        {
            Int32 Result = XML_ERROR.NO_ERROR;
            Int32 NbMode = 0;
            ObservableCollection<ModeExploitation> ListModeExploitation;
            ObservableCollection<XElement> Modes;
            ObservableCollection<XElement> Interverouillages;
            ObservableCollection<XElement> SelecteursMode;
            ObservableCollection<XElement> ConfigRetour;
            ObservableCollection<XElement> CollecFormules;
            ObservableCollection<XElement> CollecMask;
            UInt32 MaskModeHM = 0xFFFFFFFF;
            String Value;

            ListModeExploitation = new ObservableCollection<ModeExploitation>();
            Modes = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/ModesExploitation/ModeExploitation");
            Interverouillages = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/ModesExploitation/DescriptInterverrouillage");
            SelecteursMode = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/ModesExploitation/ConfigSelecteursMode");
            ConfigRetour = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/ModesExploitation/CollecConfigRetour");
            CollecFormules = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/ModesExploitation/CollecFormules");
            CollecMask = PegaseData.Instance.XMLFile.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsMasques/Modes");

            // récupérer le nombre de modes défini dans le métier
            ObservableCollection<XElement> nbModes = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/ConfigModeExploit/ConfigNbModes/NbModesReels");
            if (nbModes == null)
            {
                nbModes = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/ConfigModeExploit/ConfigNbModes/NbModes");
            }
            String nbMode;
            nbMode = nbModes.First().Attribute(XML_ATTRIBUTE.VALUE).Value;
            try
            {
                NbMode = Convert.ToInt32(nbMode);
            }
            catch
            {
                NbMode = 0;
            }

            // vérifier que le nombre d'enregistrements est le même de partout
            ModeExploitation ME = null;
            if (Modes.Count == Interverouillages.Count && 
                Interverouillages.Count == SelecteursMode.Count &&
                SelecteursMode.Count == ConfigRetour.Count &&
                ConfigRetour.Count == CollecFormules.Count)
            {
                Int32 i;
                
                for (i = 0; i < NbMode && i < Modes.Count; i++)
                {
                    if (CollecMask != null)
                    {
                        if (i >= CollecMask.Count)
                        {
                            ME = new ModeExploitation(Modes[i], Interverouillages[i], SelecteursMode[i], ConfigRetour[i], CollecFormules[i], null);
                        }
                        else
                        {
                            ME = new ModeExploitation(Modes[i], Interverouillages[i], SelecteursMode[i], ConfigRetour[i], CollecFormules[i], CollecMask[i]);
                        }
                    }
                    
                    ListModeExploitation.Add(ME);
                }
                this._modeExploitation = ListModeExploitation;

                // Mode en sécurité ?
                if (NbMode < Modes.Count)
                {
                    for (; i < Modes.Count; i++)
                    {
                        XMLProcessing XProcess = new XMLProcessing();
                        XProcess.OpenXML(Modes[i]);
                        XElement XNumMode = XProcess.GetNodeByPath("Position").FirstOrDefault();

                        if (XNumMode != null)
                        {
                            String ValueStr = XNumMode.Attribute(XML_ATTRIBUTE.VALUE).Value.Trim();
                            if (ValueStr == "33")
                            {
                                // Mode en sécurité à initialiser
                                ME = new ModeExploitation(Modes[i], Interverouillages[i], SelecteursMode[i], ConfigRetour[i], CollecFormules[i], null);
                                this.ModeSecurite = ME;
                            }
                        }
                    }
                }
            }
            else
            {
                Result = XML_ERROR.ERROR_XML_INTEGRITY;
            }

            Value = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionHommeMort/Reglage/MasqueModes", "", "", XML_ATTRIBUTE.VALUE);

            try
            {
                if (Value.Contains("0x"))
                {
                    MaskModeHM = Convert.ToUInt32(Value, 16);
                }
                else
                {
                    MaskModeHM = Convert.ToUInt32(Value);
                }
            }
            catch
            {
                MaskModeHM = 0xFFFFFFFF;
            }

            this.CalculMaskHMByMode(MaskModeHM);

            return Result;
        } // endMethod: InitModes
        
        /// <summary>
        /// Initialiser le masque par mode pour l'homme mort
        /// </summary>
        public void CalculMaskHMByMode ( UInt32 Mask )
        {
            // Evaluer la valeur de chaque bit
            Boolean[] DMask = new Boolean[32];
            for (int i = 0; i < 32; i++)
            {
                UInt32 M = 1;
                M = M << i;

                if ((M | Mask) == Mask)
                {
                    DMask[i] = true;
                }
                else
                {
                    DMask[i] = false;
                }
            }

            // Assigner la valeur de chaque bit au mode correspondant
            for (int i = 0; i < this.ModesExploitation.Count; i++)
            {
                if (DMask[i])
                {
                    this.ModesExploitation[i].MaskHMDispo = true;
                }
                else
                {
                    this.ModesExploitation[i].MaskHMDispo = false;
                }
            } 
        } // endMethod: InitMaskModeHM
        
        /// <summary>
        /// Calcul du mask Homme-Mort
        /// </summary>
        public UInt32 CalculMaskHM ( )
        {
            UInt32 Result = 0;
            // Evaluer la valeur de chaque bit
            Boolean[] DMask = new Boolean[32];

            for (int i = 0; i < this.ModesExploitation.Count; i++)
            {
                UInt32 M;
                if (this.ModesExploitation[i].MaskHMDispo)
                {
                    M = 1;
                }
                else
                {
                    M = 0;
                }
                M = M << i;
                Result += M;
            }

            for (int i = this.ModesExploitation.Count; i < 32; i++)
            {
                UInt32 M;
                M = 1;
                M = M << i;
                Result += M;
            }

            return Result;
        } // endMethod: CalculMaskHM

        /// <summary>
        /// Initialiser la configuration des retours d'informations
        /// </summary>
        public Int32 InitCollecConfigRetour ( )
        {
            Int32 Result = XML_ERROR.NO_ERROR;
            ObservableCollection<XElement> ListConfig = new ObservableCollection<XElement>();
            //this._collecConfigRetours = new ObservableCollection<CollecConfigRetour>();

            //ListConfig = this._xmlTechnique.GetNodeByPath("ParametresApplicatifsFixesMO/DicosExploitation/DicoRetourInfo/Retours");

            //if (ListConfig.Count == 0)
            //{
            //    Cmd = XML_ERROR.ERROR_XML_INTEGRITY;
            //}

            //foreach (XElement item in ListConfig)
            //{
            //    CollecConfigRetour CCR = new CollecConfigRetour(item);
            //    this._collecConfigRetours.Add(CCR);
            //}

            return Result;
        } // endMethod: InitCollecConfigRetour

        /// <summary>
        /// Initialiser la liste des sélecteurs
        /// </summary>
        public Int32 InitSelecteur ( )
        {
            Int32 Result = XML_ERROR.NO_ERROR;

            ObservableCollection<XElement> XSelecteurs = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/DicosExploitation/DicoSelecteurs/Selecteur");
            this._selecteurs = new ObservableCollection<Selecteur>();

            if (XSelecteurs != null)
            {
                foreach (var item in XSelecteurs)
                {
                    this._selecteurs.Add(new Selecteur(item));
                }
            }

            return Result;
        } // endMethod: InitSelecteur
        
        /// <summary>
        /// Initialiser la liste des retours d'infos
        /// </summary>
        public Int32 InitRetourInfos ( )
        {
            Int32 Result = XML_ERROR.NO_ERROR;

            ObservableCollection<XElement> XRetourInfo = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/ParametresApplicatifsFixesMO/DicosExploitation/DicoRetourInfo/RetourInfo");
            this._informations = new ObservableCollection<Information>();

            if (XRetourInfo != null)
            {
                foreach (var item in XRetourInfo)
                {
                    this._informations.Add(new Information(item));
                }
            }

            return Result;
        } // endMethod: InitRetourInfos

        #endregion
        
        // Messages
        #region Messages
        
        #endregion
        
    } // endClass: OptionsLogiciels
}
