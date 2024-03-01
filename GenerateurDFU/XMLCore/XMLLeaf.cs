using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JAY.XMLCore
{
    public enum DataType
    {
        notype,
        String,
        int8_t,
        int16_t,
        int32_t,
        uint8_t,
        uint16_t,
        uint32_t,
        BOOL,
        ASCII,
        bitfield8_t,
        bitfield16_t,
        bitfield32_t,
        octet,
        FLOAT,
        FLOAT32
    }

    // Une feuille de l'arbre XML
    public class XMLLeaf
    {
        // Constantes
        #region Constantes

        public const String STRING = "String";
        public const String INT8_T = "int8_t";
        public const String INT16_T = "int16_t";
        public const String INT32_T = "int32_t";
        public const String UINT8_T = "uint8_t";
        public const String UINT16_T = "uint16_t";
        public const String UINT32_T = "uint32_t";
        public const String BOOL = "Boolean";
        public const String ASCII = "ASCII";
        public const String BITFIELD8_T = "bitfield8_t";
        public const String BITFIELD16_T = "bitfield16_t";
        public const String BITFIELD32_T = "bitfield32_t";
        public const String OCTET = "octet";
        public const String FLOAT = "float";
        public const String FLOAT32 = "float32 IEEE754";

        #endregion

        // Variables
        #region Variables

        private XElement _element;
        private ObservableCollection<XMLLeaf> _childNodes;
        private String _nodeName;
        private DataType _type;
        private ObservableCollection<String> _listValeur;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Défini si le noeud actuel est en cours de sélection
        /// </summary>
        public Boolean IsSelected
        {
            get;
            set;
        } // endProperty: IsSelected

        /// <summary>
        /// La liste des attributs du noeud
        /// </summary>
        public ObservableCollection<XAttribute> Attributes
        {
            get
            {
                ObservableCollection<XAttribute> Attribs = new ObservableCollection<XAttribute>();

                foreach (XAttribute item in this._element.Attributes())
                {
                    Attribs.Add(item);
                }
                return Attribs;
            }
        } // endProperty: Attributes

        /// <summary>
        /// Le type de la donnée
        /// </summary>
        public DataType DType
        {
            get
            {
                this._type = this.InitType();
                return this._type;
            }
        } // endProperty: Type

        /// <summary>
        /// Les noeuds enfants du noeud
        /// </summary>
        public ObservableCollection<XMLLeaf> ChildNodes
        {
            get
            {
                return this._childNodes;
            }
        } // endProperty: ChildNodes

        /// <summary>
        /// Le nom du noeud en cours d'édition
        /// </summary>
        public String NodeName
        {
            get
            {
                return this._nodeName;
            }
            private set
            {
                this._nodeName = value;
            }
        } // endProperty: NodeName

        /// <summary>
        /// La description du noeud
        /// </summary>
        public String Description
        {
            get
            {
                String D = "";

                if (this._element.Attribute(XML_ATTRIBUTE.DESCRIPTION) != null)
                {
                    D = this._element.Attribute(XML_ATTRIBUTE.DESCRIPTION).Value;
                }

                return D;
            }
        } // endProperty: Description

        /// <summary>
        /// La liste des valeurs proposées
        /// </summary>
        public ObservableCollection<String> ListValeur
        {
            get
            {
                if (this._listValeur == null)
                {
                    this._listValeur = this.InitListValeur();
                }
                return this._listValeur;
            }
        } // endProperty: ListValeur

        /// <summary>
        /// La valeur Minimum
        /// </summary>
        public String Minimum
        {
            get
            {
                String M = "";
                if (this._element.Attribute(XML_ATTRIBUTE.MIN)!= null)
	            {
                    M = this._element.Attribute(XML_ATTRIBUTE.MIN).Value;
	            }
                return M;
            }
        } // endProperty: Min

        /// <summary>
        /// La valeur maximum
        /// </summary>
        public String Maximum
        {
            get
            {
                String M = "";
                if (this._element.Attribute(XML_ATTRIBUTE.MAX) != null)
	            {
                    M = this._element.Attribute(XML_ATTRIBUTE.MAX).Value;
	            }
                
                return M;
            }
        } // endProperty: Maximum

        /// <summary>
        /// La valeur
        /// </summary>
        public String Value
        {
            get
            {
                String V = "";
                if (this._element.Attribute(XML_ATTRIBUTE.VALUE)!=null)
	            {
                    V = this._element.Attribute(XML_ATTRIBUTE.VALUE).Value;
	            }
                return V;
            }
            set
            {
                if (this._element.Attribute(XML_ATTRIBUTE.VALUE)!=null)
                {
                    this._element.Attribute(XML_ATTRIBUTE.VALUE).Value = value;
                }
            }
        } // endProperty: Value

        /// <summary>
        /// Niveau hiérarchique du noeud
        /// </summary>
        public Int32 Level
        {
            get;
            set;
        } // endProperty: Level

        #endregion

        // Constructeur
        #region Constructeur

        public XMLLeaf(XElement Element, Int32 ParentLevel)
        {
            // Conserver l'élément
            this._element = Element;
            this.Level = ParentLevel + 1;
            // Initialiser la liste des enfants
            this._childNodes = new ObservableCollection<XMLLeaf>();

            foreach (var xElement in Element.DescendantNodes())
            {
                if (xElement is XElement)
                {
                    if (xElement.Parent == this._element)
                    {
                        XMLLeaf Leaf = new XMLLeaf((XElement)xElement, this.Level);
                        this._childNodes.Add(Leaf);
                    }
                }
            }

            // Initialiser le nom du noeud
            if (Element.Attribute(XML_ATTRIBUTE.CODE) != null)
            {
                this.NodeName = Element.Attribute(XML_ATTRIBUTE.CODE).Value;
                // Si le champ count existe, l'ajouter à NodeName
                if (Element.Attribute(XML_ATTRIBUTE.COUNTMAX) != null)
                {
                    this.NodeName = this.NodeName + " " + Element.Attribute(XML_ATTRIBUTE.COUNTMAX).Value;
                }
                else if (Element.Attribute(XML_ATTRIBUTE.NUM) != null)
                {
                    this.NodeName = Element.Attribute(XML_ATTRIBUTE.NUM).Value + " - " + this.NodeName;
                } 
            }
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Initialiser la liste des valeurs
        /// </summary>
        public ObservableCollection<String> InitListValeur ( )
        {
            ObservableCollection<String> Result;

            Result = new ObservableCollection<String>();

            if (this._element.Attribute(XML_ATTRIBUTE.PLAGE_VALEUR) != null)
            {
                String Source = this._element.Attribute(XML_ATTRIBUTE.PLAGE_VALEUR).Value;
                Char[] SeparatorS = new Char[] { '/' };
                String[] List = Source.Split(SeparatorS);

                foreach (String item in List)
                {
                    // si la description de la plage de valeur comporte /:
                    // il s'agit d'une plage de valeur, pas d'une liste
                    if (!item.Contains(":") && item != "")
                    {
                        Result.Add(item);
                    }
                }
            }

            return Result;
        } // endMethod: InitListValeur
        
        /// <summary>
        /// Initialiser le type de la variable
        /// </summary>
        public DataType InitType ( )
        {
            DataType Result = DataType.notype;

            if (this._element.Attribute(XML_ATTRIBUTE.TYPE) != null)
            {
                String t = this._element.Attribute(XML_ATTRIBUTE.TYPE).Value;

                t = t.Trim();

                switch (t)
                {
                    case STRING:
                        Result = DataType.String;
                        break;
                    case INT8_T:
                        Result = DataType.int8_t;
                        break;
                    case INT16_T:
                        Result = DataType.int16_t;
                        break;
                    case INT32_T:
                        Result = DataType.int32_t;
                        break;
                    case UINT8_T:
                        Result = DataType.uint8_t;
                        break;
                    case UINT16_T:
                        Result = DataType.uint16_t;
                        break;
                    case UINT32_T:
                        Result = DataType.uint32_t;
                        break;
                    case BOOL:
                        Result = DataType.BOOL;
                        break;
                    case ASCII:
                        Result = DataType.ASCII;
                        break;
                    case BITFIELD8_T:
                        Result = DataType.bitfield8_t;
                        break;
                    case BITFIELD16_T:
                        Result = DataType.bitfield16_t;
                        break;
                    case BITFIELD32_T:
                        Result = DataType.bitfield32_t;
                        break;
                    case OCTET:
                        Result = DataType.octet;
                        break;
                    case FLOAT:
                        Result = DataType.FLOAT;
                        break;
                    case FLOAT32:
                        Result = DataType.FLOAT32;
                        break;
                    default:
                        Result = DataType.notype;
                        break;
                }
            }
            return Result;
        } // endMethod: InitType

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: XMLLeaf
}
