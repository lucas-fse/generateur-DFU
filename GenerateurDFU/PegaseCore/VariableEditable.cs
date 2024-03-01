using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Forms = System.Windows.Forms;
using Mvvm = GalaSoft.MvvmLight;
using Messenger = GalaSoft.MvvmLight.Messaging.Messenger;
using JAY.XMLCore;
using System.Windows.Media.Imaging;
using System.IO;

namespace JAY.PegaseCore
{
    /// <summary>
    /// La classe de base de description des variables éditables
    /// </summary>
    public class VariableEditable : Mvvm.ViewModelBase
    {
        public const String LANGAGE_PATH = "/XMLTranslation/{0}";
        public const String FILTER_PATH = "/FilterTranslation/{0}";
        public const String EDIT_BROWSE = "BROWSE";
        public const String EDIT_COUPLAGE = "COUPLAGE";
        public const String EDIT_TEMPO = "TEMPO";
        public const String EDIT_HM = "HOMMEMORT";
        public const String EDIT_MASQUE32 = "MASQUE32";
        public const String EDIT_MASQUE32_16 = "MASQUE32_16";
        public const String EDIT_MASQUE_USER_MODE = "MASQUE_USER_MODE";
        public const String EDIT_PIN_MASQUE_USER = "PIN_MASQUE_USER";
        public const String EDIT_MASQUEVERR = "MASQUE_DEM_VERR";
        public const String EDIT_MODBUS = "MODBUS";
        public const String EDIT_LIAISON_FILAIRE = "LIAISON_FIL";
        public const String EDIT_INFRAROUGE = "INFRAROUGE";

        // Variables
        #region Variables

        private XElement _refElement;
        private ListChoixManager _autorizedValues = null;
        private Int32 _currentAutorizedValue;
        private Boolean _isFirstTime = true;
        private Boolean _isEditable;
        private String _varGroupName;
        private Visibility _listVisibility;
        private Visibility _valueVisibility;
        private Visibility _tbValueVisibility;
        private Visibility _browseVisibility;
        private Visibility _editSpecialVisibility;
        private String _commentaire;
        private String _description;
        private String _listOfChoice;
        private String _filter;
        private String _editMethod;
        private Int32 _longueurMax;
        private String _regex;
        private Int32 _separatorHeight;
        private Thickness _margin;
        private String _editMethodParam;
        private String _refElementValue;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La variable est visible et éditable par les utilisateurs
        /// </summary>
        public Boolean IsUser
        {
            get;
            private set;
        } // endProperty: IsUser

        /// <summary>
        /// La variable est visible et éditable par les experts
        /// </summary>
        public Boolean IsExpert
        {
            get;
            private set;
        } // endProperty: IsExpert

        /// <summary>
        /// Edition de la méthode de paramétrage
        /// </summary>
        public String EditMethodParam
        {
            get
            {
                return this._editMethodParam;
            }
            set
            {
                this._editMethodParam = value;
                RaisePropertyChanged("EditMethodParam");
            }
        } // endProperty: EditMethodParam

        /// <summary>
        /// Permet de controler l'indentation de la ligne
        /// </summary>
        public Thickness Margin
        {
            get
            {
                return this._margin;
            }
            set
            {
                this._margin = value;
                RaisePropertyChanged("Margin");
            }
        } // endProperty: Margin

        /// <summary>
        /// Le libellé du bouton Clear
        /// </summary>
        public String ClearLabel
        {
            get
            {
                return "/EDIT_VARIABLE/LOGO_DEFAULT";
            }
        } // endProperty: ClearLibel

        /// <summary>
        /// La hauteur du séparateur
        /// </summary>
        public Int32 SeparatorHeight
        {
            get
            {
                return this._separatorHeight;
            }
            set
            {
                this._separatorHeight = value;
                RaisePropertyChanged("SeparatorHeight");
            }
        } // endProperty: SeparatorHeight

        /// <summary>
        /// La regex associée à cette variable
        /// </summary>
        public String RegEx
        {
            get
            {
                return this._regex;
            }
            set
            {
                this._regex = value;
                RaisePropertyChanged("RegEx");
            }
        } // endProperty: RegEx

        /// <summary>
        /// La longueur maximum permise pour cette variable (en nombre de charactères)
        /// </summary>
        public Int32 LongueurMax
        {
            get
            {
                if (this._longueurMax == 0)
                {
                    this._longueurMax = 30;
                }
                return this._longueurMax;
            }
            set
            {
                this._longueurMax = value;
                RaisePropertyChanged("LongueurMax");
            }
        } // endProperty: LongueurMax

        /// <summary>
        /// La valeur en cours de sélection dans la liste
        /// </summary>
        public Int32 CurrentAutorizedValue
        {
            get
            {
                return this._currentAutorizedValue;
            }
            set
            {
                this._currentAutorizedValue = value;

                if (!this._isFirstTime)
                {
                    if (value > -1)
                    {
                        this.RefElementValue = this.AutorizedValues.Values[value];  
                    }
                }
            }
        } // endProperty: CurrentAutorizedValue

        /// <summary>
        /// La visibilité de la commande Browse
        /// </summary>
        public System.Windows.Visibility BrowseVisibility
        {
            get
            {
                return this._browseVisibility;
            }
        } // endProperty: BrowseVisibility

        /// <summary>
        /// La visibilité de la commande édition spéciale
        /// </summary>
        public Visibility EditSpecialVisibility
        {
            get
            {
                return this._editSpecialVisibility;
            }
            private set
            {
                this._editSpecialVisibility = value;
            }
        } // endProperty: EditSpecialVisibility

        /// <summary>
        /// La visibilité de la liste de choix
        /// </summary>
        public System.Windows.Visibility ListVisibility
        {
            get
            {
                return this._listVisibility;
            }
        } // endProperty: ListVisibility

        /// <summary>
        /// La visibilité de la valeur
        /// </summary>
        public System.Windows.Visibility ValueVisibility
        {
            get
            {
                return this._valueVisibility;
            }
        } // endProperty: ValueVisibility

        /// <summary>
        /// La visibilité de la valeur non editable
        /// </summary>
        public System.Windows.Visibility TBValueVisibility
        {
            get
            {
                return this._tbValueVisibility;
            }
            private set
            {
                this._tbValueVisibility = value;
                RaisePropertyChanged("TBValueVisibility");
            }
        } // endProperty: TBValueVisibility

        /// <summary>
        /// La liste des valeurs autorisées
        /// </summary>
        public ListChoixManager AutorizedValues
        {
            get
            {
                return this._autorizedValues;
            }
            set
            {
                this._autorizedValues = value;
                RaisePropertyChanged("AutorizedValues");
            }
        } // endProperty: AutorizedValues

        /// <summary>
        /// L'élément XML de référence de la variable éditable
        /// </summary>
        public XElement RefElement
        {
            get
            {
                return this._refElement;
            }
            set
            {
                this._refElement = value;
            }
        } // endProperty: RefElement

        /// <summary>
        /// La valeur de l'élement XML lié
        /// </summary>
        public String RefElementValue
        {
            get
            {
                String Result = this._refElementValue;

                // vérifier si la valeur est en Hexadécimale
                // si oui, la convertir en décimal
                if (Result.Length > 1)
                {
                    switch (VarType.ToLower().Trim())
                    {
                        
                    
                        case "int32_t":
                            if (Result.ToLower().Contains("0x"))
                            {
                                Result = Convert.ToInt32(Result, 16).ToString();
                            }
                        break;
                        case "int16_t":
                            if (Result.ToLower().Contains("0x"))
                            {
                                Result = Convert.ToInt16(Result, 16).ToString();
                            }
                            break;
                        case "int8_t":
                            if (Result.ToLower().Contains("0x"))
                            {
                                Result = Convert.ToSByte(Result, 16).ToString();
                            }
                            break;
                        case "int":
                            if (Result.ToLower().Contains("0x"))
                            {
                                Result = Convert.ToInt32(Result, 16).ToString();
                            }
                            break;
                        case "uint32_t":
                        case "uint16_t":
                        case "uint8_t":
                        case "uint":
                            if (Result.ToLower().Contains("0x"))
                            {
                                Result = Convert.ToUInt32(Result, 16).ToString();
                            }
                            break;
                        default:
                            break;
                    }
                }
                // Si aucune valeur n'est renseignée, générer une valeur par défaut
                if (Result.Length == 0)
                {
                    if (this.RefElementMin != "")
                    {
                        Result = this.RefElementMin;
                    }
                    else
                    {
                        switch (VarType.ToLower().Trim())
                        {
                            case "ascii":
                            case "string":
                                Result = "";
                                break;
                            case "uint32_t":
                                Result = "4294967295";
                                break;
                            case "uint16_t":
                                Result = "65535";
                                break;
                            case "uint8_t":
                                Result = "255";
                                break;
                            case "uint":
                                Result = "255";
                                break;
                            case "int32_t":
                            case "int16_t":
                            case "int8_t":
                            case "int":
                                Result = "-1";
                                break;
                            case "float32IEEE754":
                            case "float":
                                Result = "0xFFFFFFFF";
                                break;
                            default:
                                break;
                        }
                    }
                }
                
                return Result;
            }
            set
            {
                if (RegEx != null)
                {
                    if (RegEx != "")
                    {
                        // Si une RegEx a été définie, traiter la RegEx afin de vérifier le format des données
                        if (value != "")
                        {
                            if (!Regex.IsMatch(value, this.RegEx))
                            {
                                throw new ApplicationException("Bad mood!");
                            }
                        }
                        else
                        {
                            throw new ApplicationException("Bad mood!");
                        }
                    }
                }
                switch (VarType)
                {
                        case "int32_t":
                        case "int16_t":
                        case "int8_t":
                        case "int":
                    Int32 val;
                    try
                    {
                        val = Convert.ToInt32(value);
                    }
                    catch
                    {
                        throw new ApplicationException("Bad mood");
                    }
                    if (RefElementMin != null)
                    {
                        if (RefElementMin != "")
                        {
                            // Si une valeur minimal a été définie
                            Int32 min;
                            try
                            {
                                min = Convert.ToInt32(this.RefElementMin);

                            }
                            catch
                            {
                                throw new ApplicationException("Bad mood!");
                            }

                            if (val < min)
                            {
                                throw new ApplicationException("Bad mood!");
                            }
                        }
                    }
                    if (RefElementMax != null)
                    {
                        if (RefElementMax != "")
                        {
                            // Si une valeur minimal a été définie
                            Int32 max;
                            try
                            {
                                max = Convert.ToInt32(this.RefElementMax);

                            }
                            catch
                            {
                                throw new ApplicationException("Bad mood!");
                            }

                            if (val > max)
                            {
                                throw new ApplicationException("Bad mood!");
                            }
                        }
                    }
                    break;
                    default:
                    break;
                }

                this._refElementValue = value;
                
                RaisePropertyChanged("RefElementValue");
            }
        } // endProperty: RefElementValue

        /// <summary>
        /// Le commentaire lié à la variable
        /// </summary>
        public String VarCommentaire
        {
            get
            {
                String commentaire;
                String Path = String.Format(LANGAGE_PATH, this._commentaire);
                commentaire = LanguageSupport.Get().GetToolTip(Path);
                if (commentaire == null)
                {
                    commentaire = this._commentaire;
                }

                return commentaire;
            }
        } // endProperty: Commentaire

        /// <summary>
        /// La description de la variable
        /// </summary>
        public String VarDescription
        {
            get
            {
                String description;
                String Path = String.Format(LANGAGE_PATH, this._description);
                description = LanguageSupport.Get().GetToolTip(Path);
                if (description == null)
                {
                    description = this._description;
                }
                return description;
            }
            //set;
        } // endProperty: VarDescription
        public String Description
        {
            get
            {

                return  this._description;
               
            }
            //set;
        } // endProperty: VarDescription
        /// <summary>
        /// Le texte pour la valeur minimum 
        /// </summary>
        public String MinText
        {
            get
            {
                String Result = "/EDIT_VARIABLE/VIDE";
                if (this.RefElementMin != "")
                {
                    Result = "/EDIT_VARIABLE/MIN";
                }

                return Result;
            }
        } // endProperty: TraducMin

        /// <summary>
        /// Le texte pour la valeur maximum
        /// </summary>
        public String MaxText
        {
            get
            {
                String Result = "/EDIT_VARIABLE/VIDE";
                if (this.RefElementMin != "")
                {
                    Result = "/EDIT_VARIABLE/MAX";
                }

                return Result;
            }
        } // endProperty: MaxText

        /// <summary>
        /// La valeur Min de l'élément XML lié
        /// </summary>
        public String RefElementMin
        {
            get
            {
                String Result = "";
                if (this._refElement != null)
                {
                    if (this._refElement.Attribute(XML_ATTRIBUTE.MIN) != null)
                    {
                        Result = this._refElement.Attribute(XML_ATTRIBUTE.MIN).Value;
                    }
                    else
                    {
                        Result = "";
                    }
                    
                    if (Result.Trim() != "")
                    {
                        Result =  Result.Trim();
                    }
                }

                return Result;
            }
        } // endProperty: RefElementMin

        /// <summary>
        /// La valeur Max de l'élement XML lié
        /// </summary>
        public String RefElementMax
        {
            get
            {
                String Result = "";
                if (this._refElement != null)
                {
                    if (this._refElement.Attribute(XML_ATTRIBUTE.MAX) != null)
                    {
                        Result = this._refElement.Attribute(XML_ATTRIBUTE.MAX).Value;
                    }
                    else
                    {
                        Result = "";
                    }
                    
                    if (Result.Trim() != "")
                    {
                        Result = Result.Trim();
                    }
                }
                return Result;
            }
        } // endProperty: RefElementMax

        /// <summary>
        /// Nombre de variables
        /// </summary>
        public Int32 NbVariables
        {
            get;
            set;
        } // endProperty: NbVariables

        /// <summary>
        /// Le chemin XML de la variable
        /// </summary>
        public String XmlPath
        {
            get;
            set;
        } // endProperty: XmlPath

        /// <summary>
        /// Le nom de la variable dans le Xml (code)
        /// </summary>
        public String VarName
        {
            get;
            set;
        } // endProperty: VarName

        /// <summary>
        /// Le type de la variable
        /// </summary>
        public String VarType
        {
            get;
            set;
        } // endProperty: VarType

        /// <summary>
        /// L'unité de la variable
        /// </summary>
        public String VarUnite
        {
            get;
            set;
        } // endProperty: VarUnite

        /// <summary>
        /// La variable doit-elle être éditable ?
        /// </summary>
        public Boolean IsEditable
        {
            get
            {
                return this._isEditable;
            }
            private set
            {
                this._isEditable = value;
                this.MAJVisibility();
                RaisePropertyChanged("IsEditable");
            }
        } // endProperty: IsEditable

        /// <summary>
        /// La variable est-elle sélectionnée?
        /// </summary>
        public Boolean IsSelected
        {
            get;
            set;
        } // endProperty: IsSelected

        /// <summary>
        /// Le nom de groupe de la variable
        /// </summary>
        public String VarGroupName
        {
            get
            {
                return this._varGroupName;
            }
            set
            {
                this._varGroupName = value;
                RaisePropertyChanged("VarGroupName");
            }
        } // endProperty: VarGroupName

        #endregion

        // Constructeur
        #region Constructeur

        public VariableEditable()
        {
            this.CreateCommandBrowse();
            this.CreateCommandClearValue();
            this.CreateCommandEditSpecial();
            // Valeur par défaut
            this.IsSelected = false;
            this._description = "";
            this._commentaire = "";
            this.VarGroupName = "";
            this.VarName = "";
            this.VarType = "";
            this.SeparatorHeight = 0;
            this._listVisibility = Visibility.Collapsed;
            this._valueVisibility = Visibility.Visible;
            this._tbValueVisibility = Visibility.Collapsed;

            this._browseVisibility = Visibility.Collapsed;
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
            // Faut-il mettre à jour la langue ?
            if (message.Command == PegaseCore.Commands.CMD_MAJ_LANGUAGE)
            {
                this.AutorizedValues = new ListChoixManager(this._listOfChoice);

                RaisePropertyChanged("MaxText");
                RaisePropertyChanged("MinText");
                RaisePropertyChanged("VarCommentaire");
                RaisePropertyChanged("VarDescription");
                RaisePropertyChanged("ClearLabel");
                RaisePropertyChanged("Labels");
                this.InitListOfChoice();
                RaisePropertyChanged("RefElementValue");
            }
            else if (message.Command == PegaseCore.Commands.CMD_VALUE_CHANGED)
            {
                RaisePropertyChanged("RefElementValue");
            }
        }
        
        /// <summary>
        /// Assigner la valeur de la largeur du séparateur
        /// </summary>
        public void SetSeparatorHeight ( XElement XMLVariable )
        {
            if (XMLVariable.Attribute(XMLCore.XML_ATTRIBUTE.SEPARATOR_HEIGHT) != null)
            {
                String Value = XMLVariable.Attribute(XMLCore.XML_ATTRIBUTE.SEPARATOR_HEIGHT).Value;
                Int32 IntValue;
                try
                {
                    IntValue = Convert.ToInt32(Value);
                }
                catch
                {
                    IntValue = 0;
                }
                this.SeparatorHeight = IntValue;
            }
        } // endMethod: SetSeparatorHeight
        
        /// <summary>
        /// Assigner les valeurs de disponibilité de la variable pour les deux rôles expert et user
        /// </summary>
        public void SetUserExpertDisponibility ( XElement XMLVariable )
        {
            if (XMLVariable.Attribute(XML_ATTRIBUTE.ISUSER) != null)
            {
                if (XMLVariable.Attribute(XML_ATTRIBUTE.ISUSER).Value == "true")
                {
                    this.IsUser = true;
                }
                else
                {
                    this.IsUser = false;
                }
            }

            if (XMLVariable.Attribute(XML_ATTRIBUTE.ISEXPERT) != null)
            {
                if (XMLVariable.Attribute(XML_ATTRIBUTE.ISEXPERT).Value == "true")
                {
                    this.IsExpert = true;
                }
                else
                {
                    this.IsExpert = false;
                }
            }
        } // endMethod: SetUserExpertDisponibility

        /// <summary>
        /// Assigner la valeur des marges
        /// </summary>
        public void SetMargin ( XElement XMLVariable )
        {
            Thickness Result = new Thickness(0, 0, 0, 0);
            if (XMLVariable.Attribute(XML_ATTRIBUTE.MARGIN) != null)
            {
                String Value = XMLVariable.Attribute(XMLCore.XML_ATTRIBUTE.MARGIN).Value;
                Int32 IntValue;
                try
                {
                    IntValue = Convert.ToInt32(Value);
                }
                catch
                {
                    IntValue = 0;
                }
                Result = new Thickness(IntValue, 0, 0, 0);
            }
            this.Margin = Result;
        } // endMethod: SetMargin

        /// <summary>
        /// Mise à jour de la valeur de la variable -> utiliser pour optimisation
        /// </summary>
        public void MAJValue ( )
        {
            RaisePropertyChanged("RefElementValue");
        } // endMethod: MAJValue

        /// <summary>
        /// Assigner la valeur de référence de la variableXML liée
        /// </summary>
        public void SetRefXMLVariable ( XElement XMLVariable )
        {
            this._refElement = XMLVariable;

            // initialiser les valeurs
            String Value;

            if (this._refElement.Attribute(XML_ATTRIBUTE.CODE) != null)
            {
                Value = this._refElement.Attribute(XML_ATTRIBUTE.CODE).Value.Trim();
            }
            else
            {
                Value = "";
            }
            this.VarName = Value;

            if (this._refElement.Attribute(XML_ATTRIBUTE.TYPE) != null)
            {
                Value = this._refElement.Attribute(XML_ATTRIBUTE.TYPE).Value.Trim();
            }
            else
            {
                Value = "";
            }
            this.VarType = Value;

            if (this._refElement.Attribute(XML_ATTRIBUTE.DESCRIPTION) != null)
            {
                Value = this._refElement.Attribute(XML_ATTRIBUTE.DESCRIPTION).Value.Trim();
            }
            else
            {
                Value = "";
            }
            this._description = Value;

            if (this._refElement.Attribute(XML_ATTRIBUTE.COMMENTAIRE) != null)
            {
                Value = this._refElement.Attribute(XML_ATTRIBUTE.COMMENTAIRE).Value.Trim();
            }
            else
            {
                Value = "";
            }
            this._commentaire = Value;

            if (this._refElement.Attribute(XML_ATTRIBUTE.UNIT) != null)
            {
                Value = this._refElement.Attribute(XML_ATTRIBUTE.UNIT).Value.Trim();
            }
            else
            {
                Value = "";
            }
            this.VarUnite = Value;

            if (this._refElement.Attribute(XML_ATTRIBUTE.VALUE) != null)
            {
                this._refElementValue = this._refElement.Attribute(XML_ATTRIBUTE.VALUE).Value.Trim();
            }
            else
            {
                this._refElementValue = "";
            }

            // initialiser la liste des valeurs possibles
            this.InitListOfChoice();
            this.MAJVisibility();

            RaisePropertyChanged("MaxText");
            RaisePropertyChanged("MinText");
        } // endMethod: SetRefXMLVariable
        
        /// <summary>
        /// Initialiser la liste des choix
        /// </summary>
        public void InitListOfChoice ( )
        {
            // initialiser la liste des valeurs possibles
            this._isFirstTime = true;
            this._listOfChoice = this._refElement.Attribute(XML_ATTRIBUTE.PLAGE_VALEUR).Value;
            this.AutorizedValues = new ListChoixManager(this._listOfChoice);

            // Initialiser la valeur en cours de sélection dans la liste de choix
            this._currentAutorizedValue = this.AutorizedValues.GetPosByValue(this.RefElementValue);
            this._isFirstTime = false;
            RaisePropertyChanged("CurrentAutorizedValue");
        } // endMethod: InitListOfChoice

        /// <summary>
        /// Parse une sélection de type non = 0 et retourne 0
        /// </summary>
        public String ParseAutorizedValue ( String SourceValue )
        {
            String Result = this.AutorizedValues.GetValue(SourceValue);
            
            return Result;
        } // endMethod: ParseAutorizedValue
        
        /// <summary>
        /// Résoudre tous les paramètres de visibilité
        /// </summary>
        public void MAJVisibility ( )
        {
            // Résoudre la visibilité de la liste

            if (this.EditSpecialVisibility == Visibility.Collapsed)
            {
                if (this.AutorizedValues.Labels.Count > 0)
                {
                    if (this.IsEditable)
                    {
                        this._listVisibility = Visibility.Visible;
                    }
                    else
                    {
                        this._listVisibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    this._listVisibility = Visibility.Collapsed;
                } 
            }
            else
            {
                this._listVisibility = Visibility.Collapsed;
                this._tbValueVisibility = Visibility.Collapsed;
            }

            // Mettre à jour l'affichage
            RaisePropertyChanged("ValueVisibility");
            RaisePropertyChanged("ListVisibility");
            RaisePropertyChanged("TBValueVisibility");
        } // endMethod: ResolveVisibility
        
        /// <summary>
        /// Définir si la variable est éditable
        /// </summary>
        public void SetIsEditable ( XElement variable )
        {
            String Value = "";

            if (variable.Attribute(XML_ATTRIBUTE.ISEDITABLE) != null)
            {
                Value = variable.Attribute(XML_ATTRIBUTE.ISEDITABLE).Value.ToLower(); 
            }

            switch (Value)
            {
                case "true":
                    this.IsEditable = true;
                    if (this.AutorizedValues.Values.Count == 0)
                    {
                        this._valueVisibility = Visibility.Visible;
                    }
                    else
                    {
                        this._valueVisibility = Visibility.Collapsed;
                    }
                    
                    break;
                case "false":
                    this.IsEditable = false;
                    this._valueVisibility = Visibility.Collapsed;
                    break;
                default:
                    this.IsEditable = true;
                    if (this.AutorizedValues.Values.Count == 0)
                    {
                        this._valueVisibility = Visibility.Visible;
                    }
                    else
                    {
                        this._valueVisibility = Visibility.Collapsed;
                    }
                    break;
            }
        } // endMethod: SetIsEditable
        
        /// <summary>
        /// Définir la longueur maximum de la donnée du champs
        /// </summary>
        public void SetLongueurMax ( XElement variable )
        {
            String LMax = "";
            if (variable.Attribute(XML_ATTRIBUTE.LONGUEURMAX) != null)
	        {
                LMax = variable.Attribute(XML_ATTRIBUTE.LONGUEURMAX).Value;
	        }
            
            try
            {
                this.LongueurMax = Convert.ToInt32(LMax);
            }
            catch
            {
                // si la longueur max est mal renseignée, lui donner une longueur
                this.LongueurMax = 30;
            }
        } // endMethod: SetLongueurMax
        public String EditMethod
        { get
            {
                return this._editMethod;
            }
        }
        /// <summary>
        /// Définir la méthode d'édition de la variable, si cette méthode est spécifique
        /// </summary>
        public void SetEditMethod ( XElement variable )
        {
            this._editMethod = "";
            this._editSpecialVisibility = Visibility.Collapsed;

            // Définir la méthode d'édition
            if (variable.Attribute(XML_ATTRIBUTE.EDITMETHOD) != null)
            {
                this._editMethod = variable.Attribute(XML_ATTRIBUTE.EDITMETHOD).Value; 
            }

            switch (this._editMethod)
            {
                case EDIT_BROWSE:
                    this._browseVisibility = Visibility.Visible;
                    if (variable.Attribute(XML_ATTRIBUTE.FILTER) != null)
                    {
                        this._filter = variable.Attribute(XML_ATTRIBUTE.FILTER).Value;
                    }
                    if (this._filter == "")
                    {
                        this._filter = "*.*|*.*";
                    }
                    RaisePropertyChanged( "BrowseVisibility" );
                    break;
                case EDIT_COUPLAGE:
                case EDIT_MASQUE32:
                case EDIT_MASQUE32_16:
                case EDIT_MASQUE_USER_MODE:
                case EDIT_PIN_MASQUE_USER:
                case EDIT_TEMPO:
                case EDIT_MASQUEVERR:
                case EDIT_HM:
                case EDIT_MODBUS:
                case EDIT_LIAISON_FILAIRE:
                case EDIT_INFRAROUGE:
                    this._editSpecialVisibility = Visibility.Visible;
                    RaisePropertyChanged("EditSpecialVisibility");
                    break;
                default:
                    break;
            }
            // Si des paramètres existent, les stocker
            if (variable.Attribute(XML_ATTRIBUTE.PARAM) != null)
            {
                this.EditMethodParam = variable.Attribute(XML_ATTRIBUTE.PARAM).Value;
            }
            else
            {
                this.EditMethodParam = "";
            }
            
        } // endMethod: SetEditMethod
        
        /// <summary>
        /// Définir la Regex associée à cette variable
        /// </summary>
        public void SetRegEx ( XElement variable )
        {
            String Value = "";

            if (variable.Attribute(XML_ATTRIBUTE.REGEX) != null)
            {
                Value = variable.Attribute(XML_ATTRIBUTE.REGEX).Value;
            }

            this.RegEx = Value;
        } // endMethod: SetRegEx

        /// <summary>
        /// Définir la visibilité de la donnée. Pour certaines méthodes d'éditions,
        /// il n'est pas forcément souhaitable de voir la donnée
        /// </summary>
        public void SetDataVisibility ( XElement variable )
        {
            String Value = "";

            if (variable.Attribute(XML_ATTRIBUTE.DATAISVISIBLE) != null)
            {
                Value = variable.Attribute(XML_ATTRIBUTE.DATAISVISIBLE).Value.ToLower(); 
            }

            switch (Value.Trim())
            {
                case "true":
                    this._tbValueVisibility = Visibility.Visible;
                    break;
                case "false":
                    this._tbValueVisibility = Visibility.Collapsed;
                    break;
                default:
                    this._tbValueVisibility = Visibility.Collapsed;
                    break;
            }
            this.MAJVisibility();
        } // endMethod: SetDataVisibility

        #endregion

        // Messages
        #region Messages

        #endregion

        #region Command

        #region CommandClearValue
        /// <summary>
        /// La commande ClearValue
        /// </summary>
        public ICommand CommandClearValue
        {
            get;
            internal set;
        } // endProperty: CommandClearValue

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandClearValue()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandClearValue = new Mvvm.Command.RelayCommand(ExecuteCommandClearValue, CanExecuteCommandClearValue);
        } // endMethod: CreateCommandClearValue

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandClearValue()
        {
            this.RefElementValue = "";
            PegaseData.Instance.XMLFile.SetValue("XmlMetier/HorsMode/LogoClient/BitmapLogo", "", "", XML_ATTRIBUTE.VALUE, "");
        } // endMethod: ExecuteCommandClearValue

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandClearValue()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandClearValue 
        #endregion

        #region CommandEditSpecial
        /// <summary>
        /// La commande EditSpecial
        /// </summary>
        public ICommand CommandEditSpecial
        {
            get;
            internal set;
        } // endProperty: CommandEditSpecial

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandEditSpecial()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandEditSpecial = new Mvvm.Command.RelayCommand(ExecuteCommandEditSpecial, CanExecuteCommandEditSpecial);
        } // endMethod: CreateCommandEditSpecial

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandEditSpecial()
        {
            // Edition spécial de la valeur
            // La structure permet d'effectuer des actions personnalisées au cas ou...
            switch (this._editMethod)
            {
                default:
                    Messenger.Default.Send<CommandMessage>(new CommandMessage(this, this._editMethod));
                    break;
            }
        } // endMethod: ExecuteCommandEditSpecial

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandEditSpecial()
        {
            Boolean Result = false;

            if(true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandEditSpecial

        #endregion

        #region CommandBrowse
        /// <summary>
        /// La commande Browse
        /// </summary>
        public ICommand CommandBrowse
        {
            get;
            internal set;
        } // endProperty: CommandBrowse

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandBrowse()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandBrowse = new Mvvm.Command.RelayCommand(ExecuteCommandBrowse, CanExecuteCommandBrowse);
        } // endMethod: CreateCommandBrowse

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandBrowse()
        {
            Forms.OpenFileDialog OFD = new Forms.OpenFileDialog();
            OFD = new Forms.OpenFileDialog();
            String Path;
            Path = String.Format(FILTER_PATH, this._filter);
            OFD.Filter = LanguageSupport.Get().GetText(Path);

            if (OFD.ShowDialog() == Forms.DialogResult.OK)
            {
                BitmapSource Result = null;
                try
                {
                    Uri ImageUri = new Uri(OFD.FileName, UriKind.Relative);

                System.IO.Stream BitmapStream = PegaseData.Instance.CurrentPackage.GetPartStream(ImageUri);
                if (BitmapStream == null && File.Exists(OFD.FileName))
                {
                    BitmapStream = File.OpenRead(OFD.FileName);
                }

                String Extension = System.IO.Path.GetExtension(OFD.FileName).ToLower();
                switch (Extension)
                {
                    case ".jpg":
                        Result = FileCore.BitmapTools.OpenBitmapJPG(BitmapStream);
                        break;
                    case ".bmp":
                        Result = FileCore.BitmapTools.OpenBitmapBmp(BitmapStream);
                        break;
                    case ".png":
                        Result = FileCore.BitmapTools.OpenBitmapPng(BitmapStream);
                        break;
                    default:
                        break;
                }
                    BitmapStream.Close();
            }
            catch
            {
                // image invalide
                Result = null;
                MessageBox.Show(LanguageSupport.Get().GetText("FILTER/INVALID_FORMAT"));
            }
            if (Result != null)
            {
                this.RefElementValue = OFD.FileName;
                    PegaseData.Instance.XMLFile.SetValue("XmlMetier/HorsMode/LogoClient/BitmapLogo", "", "", XML_ATTRIBUTE.VALUE, OFD.FileName);
                    // embarquer les images
                    //XMLTools.ImportNewBitmaps();
                    this.MAJValue();
            }
                Result = null;
            }
            
        } // endMethod: ExecuteCommandBrowse

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandBrowse()
        {
            Boolean Result = false;

            if (this.VarType == "String" && this.ListVisibility != System.Windows.Visibility.Visible)
            {
                Result = true;
            }

            RaisePropertyChanged("BrowseVisibility");
            return Result;
        } // endMethod: CanExecuteCommandBrowse 
        #endregion

        #endregion
    } // endClass: VariableEditable
}
