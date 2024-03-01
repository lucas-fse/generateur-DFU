using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using JAY.XMLCore;
using System.Windows.Documents;
using System.Xml;
using System.Windows.Markup;
using Pegase.CompilEquation;

namespace JAY.PegaseCore
{
    /// <summary>
    /// La classe contenant les données de paramétrage indépendante du mode
    /// </summary>
    public class HorsMode
    {
        // Variables
        #region Variables

        private SelecteurLabel _libelleEquipement;
        private ObservableCollection<Formule> _formules;
        private ObservableCollection<Alarme> _alarmes;
        //private Boolean _titreEquipementIsBold;
        private Boolean _validerChangement;
        private Boolean _sortieSecu;
        private FlowDocument _equationtxt = null;
        
        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Sortie en sécu si le mode de changement de mode est en liste
        /// </summary>
        public Boolean SortieSecu
        {
            get
            {
                return this._sortieSecu;
            }
            set
            {
                this._sortieSecu = value;
            }
        } // endProperty: SortieSecu

        /// <summary>
        /// Valider le changement de mode (mode menu)
        /// </summary>
        public Boolean ValiderChangement
        {
            get
            {
                return this._validerChangement;
            }
            set
            {
                this._validerChangement = value;
            }
        } // endProperty: ValiderChangement

        /// <summary>
        /// Nom du bouton utiliser pour incrémanter la valeur dans le menu
        /// </summary>
        public String NavigationInc
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlMetier/HorsMode/ConfigModeExploit/Selecteurs/BtIncrementer", "", "", XML_ATTRIBUTE.VALUE);
                return Result;
            }
            set
            {
                PegaseData.Instance.XMLFile.SetValue("XmlMetier/HorsMode/ConfigModeExploit/Selecteurs/BtIncrementer", "", "", XML_ATTRIBUTE.VALUE, value);
            }
        } // endProperty: NavigationInc
        
        /// <summary>
        /// Nom du bouton utilisé pour décrémenter la valeur dans le menu
        /// </summary>
        public String NavigationDec
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlMetier/HorsMode/ConfigModeExploit/Selecteurs/BtDecrementer", "", "", XML_ATTRIBUTE.VALUE);
                return Result;
            }
            set
            {
                PegaseData.Instance.XMLFile.SetValue("XmlMetier/HorsMode/ConfigModeExploit/Selecteurs/BtDecrementer", "", "", XML_ATTRIBUTE.VALUE, value);
            }
        } // endProperty: NavigationDec

        /// <summary>
        /// Nom du bouton utilisé en tant que selecteur de position (Pos_00 = mode1, Pos_01 == mode2, Pos_02 = mode3)
        /// </summary>
        public String NavigationSelecteurPosition
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlMetier/HorsMode/ConfigModeExploit/Selecteurs/OrganeARecopier", "", "", XML_ATTRIBUTE.VALUE);
                return Result;
            }
            set
            {
                PegaseData.Instance.XMLFile.SetValue("XmlMetier/HorsMode/ConfigModeExploit/Selecteurs/OrganeARecopier", "", "", XML_ATTRIBUTE.VALUE, value);
            }
        } // endProperty: NavigationSelecteurPosition

        /// <summary>
        /// Nom du bouton utilisé en tant que selecteur de position (Pos_00 = mode1, Pos_01 == mode2, Pos_02 = mode3)
        /// </summary>
        public String NavigationComportementAuxBornes
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlMetier/HorsMode/ConfigModeExploit/Selecteurs/ComportementAuxBornes", "", "", XML_ATTRIBUTE.VALUE);
                return Result;
            }
            set
            {
                PegaseData.Instance.XMLFile.SetValue("XmlMetier/HorsMode/ConfigModeExploit/Selecteurs/ComportementAuxBornes", "", "", XML_ATTRIBUTE.VALUE, value);
            }
        } // endProperty: NavigationComportementAuxBornes

        /// <summary>
        /// Nom du bouton utilisé en tant que selecteur de position (Pos_00 = mode1, Pos_01 == mode2, Pos_02 = mode3)
        /// </summary>
        public String NavigationPosValMax
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlMetier/HorsMode/ConfigModeExploit/Selecteurs/BtPositionnerAValMax", "", "", XML_ATTRIBUTE.VALUE);
                return Result;
            }
            set
            {
                PegaseData.Instance.XMLFile.SetValue("XmlMetier/HorsMode/ConfigModeExploit/Selecteurs/BtPositionnerAValMax", "", "", XML_ATTRIBUTE.VALUE, value);
            }
        } // endProperty: NavigationPosValMax

        /// <summary>
        /// Nom du bouton utilisé en tant que selecteur de position (Pos_00 = mode1, Pos_01 == mode2, Pos_02 = mode3)
        /// </summary>
        public String NavigationPosValMin
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlMetier/HorsMode/ConfigModeExploit/Selecteurs/BtPositionnerAValMin", "", "", XML_ATTRIBUTE.VALUE);
                return Result;
            }
            set
            {
                PegaseData.Instance.XMLFile.SetValue("XmlMetier/HorsMode/ConfigModeExploit/Selecteurs/BtPositionnerAValMin", "", "", XML_ATTRIBUTE.VALUE, value);
            }
        } // endProperty: NavigationPosValMin

        /// <summary>
        /// type de selecteur de nav
        /// </summary>
        ///             <Variable code="TypeSelecteur" description="Descrip_TypeSelecteur" commentaire="Com_TypeSelecteur" offsetAbsolu="-1" offsetRelatif="-1" taille="20" type="String" count="-1" min="" max="" commentairePlageValeur="" plagevaleur="Plage_TypeSelecteur" valeur="IndicateurPosition" indice="-1" unite="" countmax="-1" Num="-1" />

        public String NavigationType
        {
            get
            {
                String Result = PegaseData.Instance.XMLFile.GetValue("XmlMetier/HorsMode/ConfigModeExploit/Selecteurs/TypeSelecteur", "", "", XML_ATTRIBUTE.VALUE);
                return Result;
            }
            set
            {
                PegaseData.Instance.XMLFile.SetValue("XmlMetier/HorsMode/ConfigModeExploit/Selecteurs/TypeSelecteur", "", "", XML_ATTRIBUTE.VALUE, value);
            }
        } // endProperty: NavigationPosValMin


        /// <summary>
        /// La collection de formule du mode universel
        /// </summary>
        public ObservableCollection<Formule> Formules
        {
            get
            {
                if (this._formules == null)
                {
                    this._formules = new ObservableCollection<Formule>();
                }
                return this._formules;
            }
            private set
            {
                this._formules = null;
            }
        } // endProperty: Formules

        /// <summary>
        /// Le nombre d'équations de la formule
        /// </summary>
        public Int32 NbEquations
        {
            get;
            set;
            
        } // endProperty: NbEquations


        
        public FlowDocument EquationTxt
        {
            get
            {
               
                //TextRange textRange = new TextRange(
                //          // TextPointer to the start of content in the RichTextBox.
                //          _equationtxt.ContentStart,
                //          // TextPointer to the end of content in the RichTextBox.
                //          _equationtxt.ContentEnd);
                //string toto = textRange.ToString();
                return this._equationtxt;
            }
            set
            {
                this._equationtxt = value;
               // String test = XamlWriter.Save(value);
            }
        } 
        
        // endProperty: LibelleEquipement
        /// <summary>
        /// Le libeller de l'équipement s'affichant sur la deuxième ligne de l'écran
        /// </summary>
        public SelecteurLabel LibelleEquipement
        {
            get
            {
                return this._libelleEquipement;
            }
            set
            {
                this._libelleEquipement = value;
            }
        } // endProperty: LibelleEquipement

        /// <summary>
        /// La collection des alarmes
        /// </summary>
        public ObservableCollection<Alarme> Alarmes
        {
            get
            {
                if (this._alarmes == null)
                {
                    this._alarmes = new ObservableCollection<Alarme>();
                }
                return this._alarmes;
            }
            private set
            {
                this._alarmes = null;
            }
        } // endProperty: Alarmes

        public int NbEquations32 { get; internal set; }
        public int TailleEquations32 { get; internal set; }

        #endregion

        // Constructeur
        #region Constructeur

        public HorsMode(XElement RootHorsMode)
        {
            this.InitHorsMode();
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Nettoyer proprement les données
        /// </summary>
        public void Dispose ( )
        {
            this.Alarmes = null;
            this.Formules = null;
            this.LibelleEquipement = null;
            this.EquationTxt = null;
        } // endMethod: Dispose

        private  bool IsFlowDocument( string xamlString)
        {
            if ( String.IsNullOrEmpty(xamlString))
                return false;

            if (xamlString.StartsWith("<") && xamlString.EndsWith(">"))
            {
                XmlDocument xml = new XmlDocument();
                try
                {
                    xml.LoadXml(string.Format("<Root>{0}</Root>", xamlString));
                    return true;
                }
                catch (XmlException)
                {
                    return false;
                }
            }
            return false;
        }

        private  FlowDocument toFlowDocument(string xamlString)
        {
            if (IsFlowDocument(xamlString))
            {
                var stringReader = new StringReader(xamlString);
                var xmlReader = System.Xml.XmlReader.Create(stringReader);

                return XamlReader.Load(xmlReader) as FlowDocument;
            }
            else
            {
                Paragraph myParagraph = new Paragraph();
                myParagraph.Inlines.Add(new Run(xamlString));
                FlowDocument myFlowDocument = new FlowDocument();
                myFlowDocument.Blocks.Add(myParagraph);

                return myFlowDocument;
            }
        }
        /// <summary>
        /// Initialiser les données indépendantes du mode
        /// </summary>
        public void InitHorsMode ( )
        {
            // 1 - Le libellé de l'équipement
            XElement X = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/LibelleEquipement/BitmapEquip").FirstOrDefault();

            this._libelleEquipement = new SelecteurLabel("LEquip");
            if (X != null)
            {
                this._libelleEquipement.Label = X.Attribute(XML_ATTRIBUTE.VALUE).Value;
                if (this._libelleEquipement.Label == Constantes.DIRECT_TO_BMP)
                {
                    this._libelleEquipement.NomFichierBitmap = PegaseData.Instance.XMLFile.GetValue("XmlMetier/HorsMode/LibelleEquipement/FichierBitmap", "", "", XML_ATTRIBUTE.VALUE);
                }
            }
            else
            {
                this._libelleEquipement.Label = "";
            }

            X = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/LibelleEquipement/PoliceGras").FirstOrDefault();

            if (X != null)
            {
                if (X.Attribute(XML_ATTRIBUTE.VALUE).Value == "1")
                {
                    this.LibelleEquipement.PoliceGrasSelecteur = true;
                }
                else
	            {
                    this.LibelleEquipement.PoliceGrasSelecteur = false;
	            }
            }
            else
            {
                this.LibelleEquipement.PoliceGrasSelecteur = false;
            }

            this.Formules.Clear();
            //2 charger le contenu du flowdocuement si il existe.
            if (PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/CollecFormuleTxt") != null)
            {
                XElement equtiontxt = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/CollecFormuleTxt/FormuleTxt").First();
                if (equtiontxt != null)
                {
                    //XMLCore.XMLProcessing equation = new XMLCore.XMLProcessing();
                    //equation.OpenXML(equtiontxt);
                    //XElement XModeConfig = new XElement(equtiontxt);
                    //XNode tequations = XModeConfig.DescendantNodes().First();
                    //string data = tequations.ToString();
                    //this._equationtxt = toFlowDocument(data);
                    string data = equtiontxt.Attribute(XML_ATTRIBUTE.VALUE).Value;
                    this.EquationTxt = toFlowDocument(data);

                    //this._equationtxt = XamlReader.Load(new XmlTextReader(new StringReader(data))) as FlowDocument;

                }
            }
            else
            {
                // 2 - Charger les équations

                ObservableCollection<XElement> XFormules = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/CollecFormuleUniversel/Formule/DescripteurFormule");
                ObservableCollection<XElement> XTableEquation = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/CollecFormuleUniversel/Formule/TableEquation");

                if (XFormules != null)
                {
                    for (Int32 i = 0; i < XFormules.Count; i++)
                    {
                        Formule formule = new Formule(XFormules[i], XTableEquation[i]);
                        this.Formules.Add(formule);
                    }
                }
            }
        
            // 3 - Charger les alarmes
            this.Alarmes.Clear();
            ObservableCollection<XElement> XAlarmes = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/CollecConfigAlarme/Alarmes");

            if (XAlarmes != null)
            {
                foreach (var xalarme in XAlarmes)
                {
                    Alarme A = new Alarme(xalarme);
                    this.Alarmes.Add(A);
                }
            }

            // 4 - Charger les fonctions valider changement et comportement MT
            String Value = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigDesModesDExploitation/ParamGenMode/ValiderChangement", "", "", XML_ATTRIBUTE.VALUE);

            if (Value != null)
            {
                if (Value == "1")
                {
                    this._validerChangement = true;
                }
                else
                {
                    this._validerChangement = false;
                }
            }

            Value = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigDesModesDExploitation/ParamGenMode/ComportementMT", "", "", XML_ATTRIBUTE.VALUE);

            if (Value != null)
            {
                if (Value == "0")
                {
                    this._sortieSecu = true;
                }
                else
                {
                    this._sortieSecu = false;
                }
            }
        } // endMethod: InitHorsMode

        /// <summary>
        /// Sérialiser les données indépendantes du mode
        /// </summary>
        public void SerialiseHorsMode()
        {
            // 1 - Le libellé de l'équipement
            XElement X = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/LibelleEquipement/BitmapEquip").First();

            if (X != null)
            {
                X.Attribute(XML_ATTRIBUTE.VALUE).Value = this.LibelleEquipement.Label;
                if (this.LibelleEquipement.Label == Constantes.DIRECT_TO_BMP)
                {
                    PegaseData.Instance.XMLFile.SetValue("XmlMetier/HorsMode/LibelleEquipement/FichierBitmap", "", "", XML_ATTRIBUTE.VALUE, this.LibelleEquipement.NomFichierBitmap);
                }
            }
            X = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/LibelleEquipement/PoliceGras").First();

            if (X != null)
            {
                if (this.LibelleEquipement.PoliceGrasSelecteur)
                {
                    X.Attribute(XML_ATTRIBUTE.VALUE).Value = "1";
                }
                else
                {
                    X.Attribute(XML_ATTRIBUTE.VALUE).Value = "0";
                }
            }

            if (this.EquationTxt != null)
            {
                ObservableCollection<XElement> equationtxt = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/CollecFormuleTxt");
               
            }
            //else
            //{ 
                // 2 - Enregistrer les équations
                ObservableCollection<XElement> TableFormules = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/CollecFormuleUniversel/Formule");

                if ((TableFormules != null) && (TableFormules.Count()>0))
                {
                    ObservableCollection<XElement> XFormules = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/CollecFormuleUniversel/Formule/DescripteurFormule");
                    ObservableCollection<XElement> XTableEquation = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/CollecFormuleUniversel/Formule/TableEquation");
                    if (XFormules != null)
                    {
                        if (XFormules.Count == XTableEquation.Count)
                        {
                            for (int i = XFormules.Count - 1; i >= 0; i--)
                            {
                                XTableEquation[i].Remove();
                                XFormules[i].Remove();
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Erreur! Formules universelles incohérentes...", "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                        }
                    }
                }
           // }
            // 2.0 - Ouvrir le package permettant de charger une formule de référence

            // 2.1 - Contruire la partie correspondante
            // si le nouveau mode de gestion des equations est opérationnnelles on ne suave garde pas les anciennnes equations.
            //if (this.EquationTxt == null)
            if (true)
            {
                TableFormules = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/CollecFormuleUniversel/Formule");
                foreach (var formule in this.Formules)
                {
                    if (formule.CommentaireFormule.Equals("Build by Easy Config"))
                    {
                        XElement XFormuleHeader = XElement.Parse(JAY.XMLCore.DefaultXMLTemplate.Instance.TemplateFormuleModeUniversel);
                        XMLCore.XMLProcessing XPF = new XMLCore.XMLProcessing();
                        XPF.OpenXML(XFormuleHeader);

                        XPF.GetNodeByPath("Commandes").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = formule.Commandes;
                        XPF.SetValue("CommentaireFormule", "", "", XML_ATTRIBUTE.PLAGE_VALEUR, formule.FormuleType.ToString());
                        XPF.GetNodeByPath("CommentaireFormule").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = formule.CommentaireFormule;
                        XPF.GetNodeByPath("Fonction").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = formule.Fonction;
                        XPF.GetNodeByPath("Fonctionnement").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = formule.Fonctionnement;
                        XPF.GetNodeByPath("MnemoLogiquePhy").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = formule.MnemoLogiquePhy;
                        XPF.GetNodeByPath("NbEquation").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = formule.NbEquation.ToString();
                        //XPF.GetNodeByPath("ModeExploit").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = formule.NumModeExploit.ToString();
                        XPF.GetNodeByPath("ModeExploit").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "32";

                        XElement TableEquation = XElement.Parse(JAY.XMLCore.DefaultXMLTemplate.Instance.TemplateTableEquationModeUniversel);

                        // 7.4.1 - Recopier les bonnes données dans la formule
                        foreach (var equation in formule.Equations)
                        {
                            // 7.4.2 - Ajouter les équations
                            XElement XMnemo = XElement.Parse(JAY.XMLCore.DefaultXMLTemplate.Instance.TemplateMnemologique);
                            XMnemo.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = equation.MnemoLogique;
                            XElement XEquation = XElement.Parse(JAY.XMLCore.DefaultXMLTemplate.Instance.TemplateEquation);
                            XEquation.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = equation.TextEquation;
                            // 7.4.3 - Ajouter les deux noeuds à la table
                            TableEquation.Add(XMnemo);
                            TableEquation.Add(XEquation);
                        }
                        // 7.4.4 - Ajouter le noeud en tant qu'enfant du noeud Formules
                        TableFormules[0].Add(XPF.RootNode);
                        TableFormules[0].Add(TableEquation);
                    }
                }
            }
            if (this.EquationTxt != null) //else
            {
                // sauvegarde du flow document  correspondant a la nouvelle gestion de saisies des equations
                XElement CollecFormuleTxt;
                XElement FormuleTxt;
                XAttribute attribCode;
                XAttribute attribVariable;
                XAttribute attribValue;
                XElement CollecFormuleTxtRoot;
                XElement Horsmoderoot = null;
                String value = XamlWriter.Save(this.EquationTxt);

                var stringReader = new StringReader(value);
                var xmlReader = System.Xml.XmlReader.Create(stringReader);
                FlowDocument tmp = XamlReader.Load(xmlReader) as FlowDocument;
                value = XamlWriter.Save(tmp);

                CollecFormuleTxt = new XElement("SousBloc");
                FormuleTxt = new XElement("Variable");

                attribCode = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "CollecFormuleTxt");
                attribVariable = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "FormuleTxt");
                attribValue = new XAttribute(XMLCore.XML_ATTRIBUTE.VALUE, value );

                CollecFormuleTxt.Add(attribCode);
                FormuleTxt.Add(attribVariable);
                FormuleTxt.Add(attribValue);
                CollecFormuleTxt.Add(FormuleTxt);

                // CollecFormuleTxt.Add(toto);
                //XElement xmlTree = XElement.Parse(value);
                //CollecFormuleTxt.Add(xmlTree);

                Horsmoderoot = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode").First();
                if (Horsmoderoot != null)
                {
                    if (PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/CollecFormuleTxt") == null)
                    {
                        Horsmoderoot.Add(CollecFormuleTxt);
                    }
                    else
                    {
                        CollecFormuleTxtRoot = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/CollecFormuleTxt").First();
                        if (!CollecFormuleTxtRoot.ToString().Equals(CollecFormuleTxt.ToString()))
                        {
                            CollecFormuleTxtRoot.Remove();
                            Horsmoderoot.Add(CollecFormuleTxt);
                        }
                    }
                }
                // on essay de rajouter les equations dans l'ancien mode pour maintenir la compatibilité
                // on analyse les modes
                
                JAY.PegaseCore.AnalyseEquation analyse = new AnalyseEquation();
                string result;
                analyse.RechercheCommentaire(this.EquationTxt, out result);
                List<AnalyseEquation.EquationBymode> list = analyse.AnalyseEquationByModeByFonction(ref result);
                TableFormules = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/CollecFormuleUniversel/Formule");
               
                foreach (var formule in list)
                {
                    if (formule.mode.Equals(32))
                    {
                        XElement XFormuleHeader = XElement.Parse(JAY.XMLCore.DefaultXMLTemplate.Instance.TemplateFormuleModeUniversel);
                        XMLCore.XMLProcessing XPF = new XMLCore.XMLProcessing();
                        XPF.OpenXML(XFormuleHeader);


                        XPF.GetNodeByPath("Commandes").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "Commands";
                        XPF.SetValue("CommentaireFormule", "", "", XML_ATTRIBUTE.PLAGE_VALEUR, "Equation from flowdocument");
                        XPF.GetNodeByPath("CommentaireFormule").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "None";
                        XPF.GetNodeByPath("Fonction").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "FunctionfromFlowdocument";
                        XPF.GetNodeByPath("Fonctionnement").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "none";
                        XPF.GetNodeByPath("MnemoLogiquePhy").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "Mode"+formule.mode.ToString()+"Flowdocument";
                        XPF.GetNodeByPath("NbEquation").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "1";
                        //XPF.GetNodeByPath("ModeExploit").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = formule.NumModeExploit.ToString();
                        XPF.GetNodeByPath("ModeExploit").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "32";

                        XElement TableEquation = XElement.Parse(JAY.XMLCore.DefaultXMLTemplate.Instance.TemplateTableEquationModeUniversel);

                        // 7.4.1 - Recopier les bonnes données dans la formule

                        // 7.4.2 - Ajouter les équations
                        XElement XMnemo = XElement.Parse(JAY.XMLCore.DefaultXMLTemplate.Instance.TemplateMnemologique);
                        XMnemo.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "Flowdocument";
                        XElement XEquation = XElement.Parse(JAY.XMLCore.DefaultXMLTemplate.Instance.TemplateEquation);
                        XEquation.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = formule.equation;
                        // 7.4.3 - Ajouter les deux noeuds à la table
                        TableEquation.Add(XMnemo);
                        TableEquation.Add(XEquation);

                        // 7.4.4 - Ajouter le noeud en tant qu'enfant du noeud Formules
                        TableFormules[0].Add(XPF.RootNode);
                        TableFormules[0].Add(TableEquation);
                    }
                }
               

                // sauvegarde des ligne de config PLD
                //ObservableCollection <XElement> DicoConfigPLD = PegaseData.Instance.XMLFile.GetNodeByPath("XmlTechnique/ParametresApplicatifs2/ParametresFixesIHM2/PerformanceLevel/DicoConfigPLD/");
                //string offsetAbsolu = "";
                //string offsetRelatif = "";
                //    if (DicoConfigPLD!= null)
                //    {
                //        if (LutteAntiGFI.Instance.FicheExtension == TypeFiche.TYPE0)
                //        {
                //            LutteAntiGFI.Instance.FicheExtension = TypeFiche.TYPE1;
                //        }
                //    }
                //    if (LutteAntiGFI.Instance.FicheExtension != TypeFiche.TYPE0)
                //    {
                //        if (DicoConfigPLD != null && DicoConfigPLD.Count > 0)
                //        {
                //            foreach (var elem in DicoConfigPLD)
                //            {
                //                if (elem.Attribute(XMLCore.XML_ATTRIBUTE.CODE).Value == "DicoConfigPLD")
                //                {
                //                    offsetAbsolu = elem.Attribute("offsetAbsolu").Value;
                //                    offsetRelatif = elem.Attribute("offsetRelatif").Value;
                //                    break;
                //                }
                //            }

                //            UInt32 offsetAbsoluint = Convert.ToUInt32(offsetAbsolu);
                //            UInt32 offsetRelatifint = Convert.ToUInt32(offsetRelatif);
                //            ObservableCollection<XElement> lignePLD = PegaseData.Instance.XMLFile.GetNodeByPath("XmlTechnique/ParametresApplicatifs2/ParametresFixesIHM2/PerformanceLevel/DicoConfigPLD/");
                //            lignePLD.First().Elements().ToList().Remove();
                //            int cpt_pld = 0;
                //            foreach (var lignepld in analyse.PLDConfig)
                //            {
                //                XElement XCmdPLD = XElement.Parse(JAY.XMLCore.DefaultXMLTemplate.Instance.TemplateConfigCmdPLD);
                //                XMLCore.XMLProcessing XPLD = new XMLCore.XMLProcessing();
                //                XPLD.OpenXML(XCmdPLD);
                //                XPLD.GetNodeByPath("MasqueOrganesPLD").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = lignepld.Buttons.ToString();
                //                XPLD.GetNodeByPath("MasqueModes").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = lignepld.Modes.ToString();
                //                XPLD.GetNodeByPath("MasqueOrganesPLD").First().Attribute(XMLCore.XML_ATTRIBUTE.OFFSET_ABSOLU).Value = offsetAbsoluint.ToString();
                //                XPLD.GetNodeByPath("MasqueOrganesPLD").First().Attribute(XMLCore.XML_ATTRIBUTE.OFFSET_RELATIF).Value = offsetRelatifint.ToString();
                //                XPLD.GetNodeByPath("MasqueModes").First().Attribute(XMLCore.XML_ATTRIBUTE.OFFSET_ABSOLU).Value = (offsetAbsoluint + 4).ToString();
                //                XPLD.GetNodeByPath("MasqueModes").First().Attribute(XMLCore.XML_ATTRIBUTE.OFFSET_RELATIF).Value = (offsetRelatifint + 4).ToString();
                //                XPLD.RootNode.Attribute(XMLCore.XML_ATTRIBUTE.OFFSET_ABSOLU).Value = offsetAbsoluint.ToString();
                //                XPLD.RootNode.Attribute(XMLCore.XML_ATTRIBUTE.OFFSET_RELATIF).Value = offsetRelatifint.ToString();
                //                lignePLD[0].Add(XPLD.RootNode);
                //                offsetAbsoluint += 8;
                //                offsetRelatifint += 8;
                //                cpt_pld++;
                //            }
                //            for (;cpt_pld<48;cpt_pld++)
                //            {
                //                XElement XCmdPLD = XElement.Parse(JAY.XMLCore.DefaultXMLTemplate.Instance.TemplateConfigCmdPLD);
                //                XMLCore.XMLProcessing XPLD = new XMLCore.XMLProcessing();
                //                XPLD.OpenXML(XCmdPLD);
                //                XPLD.GetNodeByPath("MasqueOrganesPLD").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "0xFFFFFFFF";
                //                XPLD.GetNodeByPath("MasqueModes").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "0xFFFFFFFF";
                //                XPLD.GetNodeByPath("MasqueOrganesPLD").First().Attribute(XMLCore.XML_ATTRIBUTE.OFFSET_ABSOLU).Value = offsetAbsoluint.ToString();
                //                XPLD.GetNodeByPath("MasqueOrganesPLD").First().Attribute(XMLCore.XML_ATTRIBUTE.OFFSET_RELATIF).Value = offsetRelatifint.ToString();
                //                XPLD.GetNodeByPath("MasqueModes").First().Attribute(XMLCore.XML_ATTRIBUTE.OFFSET_ABSOLU).Value = (offsetAbsoluint + 4).ToString();
                //                XPLD.GetNodeByPath("MasqueModes").First().Attribute(XMLCore.XML_ATTRIBUTE.OFFSET_RELATIF).Value = (offsetRelatifint + 4).ToString();
                //                XPLD.RootNode.Attribute(XMLCore.XML_ATTRIBUTE.OFFSET_ABSOLU).Value = offsetAbsoluint.ToString();
                //                XPLD.RootNode.Attribute(XMLCore.XML_ATTRIBUTE.OFFSET_RELATIF).Value = offsetRelatifint.ToString();
                //                lignePLD[0].Add(XPLD.RootNode);
                //                offsetAbsoluint += 8;
                //                offsetRelatifint += 8;
                //            }
                //        }
                //        try
                //        {
                //            XElement ligneNivSecuPLDPLD = PegaseData.Instance.XMLFile.GetNodeByPath("XmlTechnique/ParametresApplicatifs2/ParametresFixesIHM2/PerformanceLevel/TypeDeSecurite/NivSecu/NivSecuPLD/").First();
                //            var query = from organ in PegaseData.Instance.MOperateur.OrganesCommandes where organ.NomOrganeMO == "A14" select organ;

                //            // pas d'equations pld
                //            if (analyse.PLDConfig.Count == 0)
                //            {
                //                ligneNivSecuPLDPLD.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "0";
                //            }
                //            // equation pld sur produit avec boutons secu
                //            else if (PegaseData.Instance.MOperateur.PresenceSafetyButton > 0)
                //            {
                //                if (query != null && query.Count() > 0)
                //                {
                //                    OrganCommand BPSecu = query.First();
                //                    if (BPSecu.ReferenceOrgan == "D31061A")
                //                    {
                //                        ligneNivSecuPLDPLD.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "3";
                //                    }
                //                    else if (BPSecu.ReferenceOrgan == "D31062A")
                //                    {
                //                        ligneNivSecuPLDPLD.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "2";
                //                    }
                //                    else
                //                    {
                //                        ligneNivSecuPLDPLD.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "0";
                //                    }
                //                    XElement variableXml = PegaseData.Instance.XMLFile.GetNodeByPath("XmlTechnique/ParametresApplicatifs/ParametresModifiables/OptionsDispo/GestionTypeFiche/TypeFiche/").First();


                //                    if (variableXml != null)
                //                    {
                //                        if (LutteAntiGFI.Instance.EtatTypeFiche == TypeFiche.TYPE0)
                //                        {
                //                            LutteAntiGFI.Instance.EtatTypeFiche = TypeFiche.TYPE1;
                //                        }
                //                        if ((variableXml.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value == "0") && ligneNivSecuPLDPLD.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value != "0")
                //                        {
                //                            variableXml.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "1";

                //                        }

                //                    }
                //                }
                //            }
                //            // equation pld sur produit sans boutons
                //            else
                //            {
                //                ligneNivSecuPLDPLD.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "1";
                //            }
                //        }
                //        catch { }
                //    }
                //
            }
            // 8 - sauvegarder les alarmes
            this.SerializeAlarmes();

            // 4 - Charger les fonctions valider changement et comportement MT
            String Value;

            if (this._validerChangement)
            {
                Value = "1";
            }
            else
	        {
                Value = "0";
	        }
            PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigDesModesDExploitation/ParamGenMode/ValiderChangement", "", "", XML_ATTRIBUTE.VALUE, Value);

            if (this._sortieSecu)
            {
                Value = "0";
            }
            else
            {
                Value = "-1";
            }
            PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigDesModesDExploitation/ParamGenMode/ComportementMT", "", "", XML_ATTRIBUTE.VALUE, Value);

        } // endMethod: SerialiseHorsMode

        /// <summary>
        /// Sérialiser les alarmes
        /// </summary>
        public void SerializeAlarmes ( )
        {
            ObservableCollection<XElement> alarmes = PegaseData.Instance.XMLFile.GetNodeByPath("XmlMetier/HorsMode/CollecConfigAlarme/Alarmes");
            if (alarmes != null)
            {
                for (int i = 0; i < alarmes.Count; i++)
                {
                    this.Alarmes[i].SerialiseAlarme(alarmes[i]);
                    //        //this.MAJAlarmes(alarmes[i], "Fonction", this.Alarmes[i].Fonction);
                    //        //this.MAJAlarmes(alarmes[i], "MnemoLogique", this.Alarmes[i].Mnemologique);
                    //        //this.MAJAlarmes(alarmes[i], "Tempo", this.Alarmes[i].Tempo.ToString());
                    //        //this.MAJAlarmes(alarmes[i], "Afficher", XMLCore.Tools.ConvertFromBoolean2String(this.Alarmes[i].Afficher));
                    //        //this.MAJAlarmes(alarmes[i], "Vibrer", XMLCore.Tools.ConvertFromBoolean2String(this.Alarmes[i].Vibrer));
                    //        //this.MAJAlarmes(alarmes[i], "AutoAcquit", XMLCore.Tools.ConvertFromBoolean2String(this.Alarmes[i].AutoAcquit));
                    //        //this.MAJAlarmes(alarmes[i], "IdentLibelInformation", this.Alarmes[i].IdentLibelInformation);
                }
                //    //XMLTools.ImportNewBitmaps();
            }
        } // endMethod: SerializeAlarmes

        /// <summary>
        /// Mettre à jour la valeur d'un élément d'une alarme en fonction du code variable fourni
        /// </summary>
        //private void MAJAlarmes ( XElement XEl, String Code, String Value )
        //{
        //    var Query = from variable in XEl.Descendants("Variable")
        //                where variable.Attribute(XML_ATTRIBUTE.CODE).Value == Code
        //                select variable;

        //    Query.First().Attribute(XML_ATTRIBUTE.VALUE).Value = Value;
        //} // endMethod: MAJAlarmes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: HorsMode
}