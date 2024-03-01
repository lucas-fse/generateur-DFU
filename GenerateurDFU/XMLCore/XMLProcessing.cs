using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace JAY.XMLCore
{
    /// <summary>
    /// Classe d'entrée pour la gestion des données XML de IDialog
    /// </summary>
    public class XMLProcessing
    {
        // Variables
        #region Variables

        private String _fileName;           // Le nom et le chemin d'accès du fichier
        private XDocument _xmlDoc;          // Le document XML initial
        private XElement _xmlElement;       // Le XML Element initial
        private ObservableCollection<XMLLeaf> _xmlTree;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// L'arbre XML du fichier
        /// </summary>
        public ObservableCollection<XMLLeaf> XMLTree
        {
            get
            {
                return this._xmlTree;
            }
        } // endProperty: XMLTree

        /// <summary>
        /// Le nom du fichier
        /// </summary>
        public String FileName
        {
            get
            {
                return this._fileName;
            }
            private set
            {
                this._fileName = value;
            }
        } // endProperty: FileName

        /// <summary>
        /// retourner le noeud racine de l'arbre XML
        /// </summary>
        public XElement RootNode
        {
            get
            {
                XElement XE = null;
                if (this._xmlDoc != null)
                {
                    XE = this._xmlDoc.Element(XML_NODE.XML_ROOT_NAME);
                }
                else
                {
                    XE = this._xmlElement;
                }

                return XE;
            }
        } // endProperty: RootNode

        /// <summary>
        /// Le document XML lié à cet XMLProcessing
        /// </summary>
        public XDocument Document
        {
            get
            {
                return this._xmlDoc;
            }
            set
            {
                this._xmlDoc = value;
                this._xmlElement = this.RootNode;
                //this._xmlTree = this.BuildXMLTree();
            }
        } // endProperty: xDocument

        #endregion

        // Constructeur
        #region Constructeur

        /// <summary>
        /// Contructeur. Le chargement du XML est réalisé par ailleur.
        /// </summary>
        public XMLProcessing()
        {
            this._fileName = "";
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Fermer le fichier en cours d'utilisation
        /// </summary>
        public void Close ( )
        {
            this._xmlElement = null;
        } // endMethod: Close

        /// <summary>
        /// Construire l'arbre XML à partir du fichier en cours d'édition
        /// </summary>
        public ObservableCollection<XMLLeaf> BuildXMLTree ( )
        {
            ObservableCollection<XMLLeaf> Result;
            ObservableCollection<XElement> Elements = this.ExtractFirstLevelNode();
            Result = new ObservableCollection<XMLCore.XMLLeaf>();

            foreach (XElement element in Elements)
            {
                XMLCore.XMLLeaf leaf = new XMLCore.XMLLeaf(element, 0);
                Result.Add(leaf);
            }
            return Result;
        } // endMethod: BuildXMLTree

        /// <summary>
        /// Ouvrir un fichier XML
        /// </summary>
        /// <param name="FileName">
        /// Le nom du fichier à charger
        /// </param>
        /// <returns>
        /// Un code erreur est retourné. 0x0000 = pas d'erreur
        /// </returns>
        public Int32 OpenXML ( String FileName )
        {
            Int32 Result = XML_ERROR.ERROR_FILE_NOT_FOUND;
            Stream stream = null;
            if (File.Exists(FileName))
            {
                stream = File.OpenRead(FileName);
            }

            if (stream != null)
            {
                Result = this.OpenXML(stream);

                stream.Close();
                Result = XML_ERROR.NO_ERROR;
            }
            return Result;
        } // endMethod: OpenXML        
        
        /// <summary>
        /// Ouvrir un fichier XML
        /// </summary>
        /// <param name="stream">
        /// Le flux à partir duquel charger le fichier
        /// </param>
        public Int32 OpenXML ( Stream stream )
        {
            Int32 Result = XML_ERROR.ERROR_UNDESCRIBED;
            try
            {
                this._xmlDoc = XDocument.Load(stream);
                this._fileName = FileName;
            }
            catch
            {
                Result = XML_ERROR.ERROR_XML_INTEGRITY;
            }

            // Vérifier qu'il s'agit bien d'un document XML IDialog
            if (this.RootNode != null)
            {
                Result = XML_ERROR.NO_ERROR;
                // Tree n'est plus utilisé, ne plus l'initialiser
                // this._xmlTree = this.BuildXMLTree();
            }
            
            return Result;
        } // endMethod: OpenXML
        
        /// <summary>
        /// Initialiser la classe à partir d'un noeud XML
        /// </summary>
        public Int32 OpenXML ( XElement XMLNode )
        {
            Int32 Result = XML_ERROR.NO_ERROR;

            this._xmlElement = XMLNode;
            // Tree n'est plus utilisé, ne plus l'initialiser
            //this._xmlTree = this.BuildXMLTree();

            return Result;
        } // endMethod: OpenXML ( XElement XMLNode )

        /// <summary>
        /// Sauve le fichier XML
        /// </summary>
        public Int32 SaveXMLAs ( String FileName )
        {
            Int32 Result = XML_ERROR.ERROR_UNDESCRIBED;

            this._xmlDoc.Save(FileName);
            this.FileName = FileName;

            Result = XML_ERROR.NO_ERROR;
            return Result;
        } // endMethod: SaveXMLAs

        /// <summary>
        /// Sauve le fichier XML
        /// </summary>
        public Int32 SaveXML( )
        {
            Int32 Result = XML_ERROR.ERROR_UNDESCRIBED;

            if (File.Exists(this._fileName))
            {
                this._xmlDoc.Save(this._fileName);
                Result = XML_ERROR.NO_ERROR;
            }

            return Result;
        } // endMethod: SaveXML

        /// <summary>
        /// Sauver le fichier XML sur le flux
        /// </summary>
        public Int32 SaveXML ( Stream flux )
        {
            Int32 Result = XML_ERROR.ERROR_UNDESCRIBED; ;

            this._xmlDoc.Save(flux);
            Result = XML_ERROR.NO_ERROR;

            return Result;
        } // endMethod: SaveXML

        /// <summary>
        /// La liste des noeuds de premiers niveaux
        /// </summary>
        public ObservableCollection<XElement> ExtractFirstLevelNode ( )
        {
            ObservableCollection<XElement> Result;

            if (this._xmlDoc != null)
            {
                Result = this.ExtractNode(this._xmlDoc.Element(XML_NODE.XML_ROOT_NAME));
            }
            else
            {
                Result = this.ExtractNode(this._xmlElement);
            }

            return Result;
        } // endMethod: ExtractNode

        /// <summary>
        /// Extrait tous les noeuds d'un element donné
        /// </summary>
        /// <param name="XE">
        /// L'élément à partir du quel les données doivent être modifiée
        /// </param>
        /// <returns>
        /// La collection des noeuds
        /// </returns>
        public ObservableCollection<XElement> ExtractNode(XElement XE)
        {
            ObservableCollection<XElement> Result;
            Result = new ObservableCollection<XElement>();

            if (XE == null)
            {
                return null;
            }

            foreach (XElement node in XE.Elements())
            {
                Result.Add(node);
            }
            
            return Result;
        } // endMethod: ExtractNode
        
        /// <summary>
        /// Rechercher le noeud spécifié à partir de la racine en suivant le chemin
        /// </summary>
        /// <param name="Path">
        /// Le chemin du noeud doit être présenté de la façon suivante : Code1/Code2/Code3 ...
        /// Code fait référence à l'attribut code de chacune des balises
        /// </param>
        /// <returns>
        /// retourne le noeud trouvé, ou bien null si aucun noeud n'a été identifié
        /// </returns>
        public ObservableCollection<XElement> GetNodeByPath ( String Path )
        {
            ObservableCollection<XElement> Result = null;
            String[] Codes;
            Char[] Separators = new Char[] { '/' };
            Int32 i;
            ObservableCollection<XElement> XESource, XEDest;
            XESource = new ObservableCollection<XElement>();
            XEDest = new ObservableCollection<XElement>();

            // si la chaine fini par un '/', l'enlever
            if (Path.Count() > 0)
            {
                if (Path[Path.Count() - 1] == '/')
                {
                    Path = Path.Remove(Path.Count() - 1);
                }
            }
            
            // si la chaine commence par un '/', l'enlever
            if (Path.Count() > 0)
            {
                if (Path[0] == '/')
                {
                    Path = Path.Substring(1);
                }
            }

            // si le chemin est vide, retourner ROOT
            if (Path == "")
            {
                Result = new ObservableCollection<XElement>();
                Result.Add(this.RootNode);
                return Result;
            }

            Codes = Path.Split(Separators);

            XESource.Add(this.RootNode);
            for (i = 0; i < Codes.Length; i++)
            {
                for (int j = 0; j < XESource.Count; j++)
                {
                    ObservableCollection<XElement> XE;
                    XE = this.GetNodesByCode(Codes[i], XESource[j]);

                    if (XE != null)
                    {
                        foreach (XElement node in XE)
                        {
                            XEDest.Add(node);
                        }
                    }
                }
                XESource = XEDest;
                XEDest = new ObservableCollection<XElement>();
                if (XESource.Count == 0)
                {
                    XESource = null;
                    break;
                }
            }

            Result = XESource;
            return Result;
        } // endMethod: GetNodeByPath
        
        /// <summary>
        /// Mettre à jour une valeur d'attribut. Le noeud à modifié est repéré par un chemin d'accès
        /// délimité de la façon suivante : 'Code1/Code2' ...
        /// d'un attribut de référence ARef spécifiant l'unicité de la ligne
        /// et d'une valeur
        /// </summary>
        /// <param name="Path">
        /// Le chemin d'accès du noeud de la forme 'Code1/Code2' ...
        /// </param>
        /// <param name="ARef">
        /// Le nom d'un attribut de référence permettant d'avoir une unicité dans la ligne
        /// </param>
        /// <param name="AValue">
        /// La valeur de l'attribut de référence
        /// </param>
        /// <param name="Attribute">
        /// Le nom de l'attribut à mettre à jour
        /// </param>
        /// <param name="Value">
        /// La valeur de l'attribut à mettre à jour
        /// </param>
        /// <returns>
        /// true si la valeur a été mise à jour. false sinon.
        /// </returns>
        public Boolean SetValue ( String Path, String ARef, String AValue, String Attribute, String Value )
        {
            Boolean Result = false;
            XElement XE = null;

            // trouver les noeuds possibles
            ObservableCollection<XElement> XES = this.GetNodeByPath(Path);
            if (XES == null)
            {
                return Result;
            }
            // trouver le noeud exact s'il y a plusieurs noeuds
            if (XES.Count > 1)
            {
                foreach (XElement element in XES)
                {
                    if (element.Attributes(ARef) != null)
                    {
                        if (element.Attribute(ARef).Value == AValue)
                        {
                            XE = element;
                        }
                    }
                }
            }
            else
            {
                XE = XES[0];
            }

            // remplacer la valeur
            if (XE.Attribute(Attribute) != null && Value != null)
            {
                XE.Attribute(Attribute).Value = Value;
                Result = true;
            }

            return Result;
        } // endMethod: SetValue
        
        /// <summary>
        /// La méthode ToString du XMLProcessing
        /// </summary>
        public new String ToString ( )
        {
            String Result = "";

            if (this._xmlDoc != null)
            {
                Result = this._xmlDoc.ToString(); 
            }
            
            return Result;
        } // endMethod: ToString

        /// <summary>
        /// Lire la valeur de l'attribut spécifié
        /// </summary>
        /// <param name="Path">
        /// Le chemin d'accès du noeud de la forme 'Code1/Code2' ...
        /// </param>
        /// <param name="ARef">
        /// Le nom d'un attribut de référence permettant d'avoir une unicité dans la ligne
        /// </param>
        /// <param name="AValue">
        /// La valeur de l'attribut de référence
        /// </param>
        /// <param name="Attribute">
        /// Le nom de l'attribut à mettre à jour
        /// </param>
        /// <returns>
        /// retourne la valeur de l'attribut ou null si l'attribut n'a pas été trouvé
        /// </returns>
        public String GetValue ( String Path, String ARef, String AValue, String Attribute )
        {
            String Result = null;
            XElement XE = null;

            // trouver les noeuds possibles
            ObservableCollection<XElement> XES = this.GetNodeByPath(Path);
            if (XES == null)
            {
                return Result;
            }
            // trouver le noeud exact s'il y a plusieurs noeuds
            if (XES.Count > 1)
            {
                foreach (XElement element in XES)
                {
                    if (ARef != "" && element.Attributes(ARef) != null)
                    {
                        if (element.Attribute(ARef).Value == AValue)
                        {
                            XE = element;
                        }
                    }
                }
                // Par défaut, prendre le premier élément
                if (XE == null)
                {
                    XE = XES.First();
                }
            }
            else
            {
                XE = XES[0];
            }

            // lire la valeur
            if (XE.Attribute(Attribute) != null)
            {
                Result = XE.Attribute(Attribute).Value;
            }

            return Result;
        } // endMethod: ReadValue
        
        /// <summary>
        /// Insérer un bloc XML à la suite du noeud spécifié
        /// </summary>
        public void InsertXElement ( XElement previousNode, XElement node )
        {
            previousNode.AddAfterSelf(node);
        } // endMethod: InsertXElement

        /// <summary>
        /// Recherche les noeuds enfants de ROOT contenant ce code
        /// </summary>
        /// <param name="Code">
        /// Le Code désignant les noeuds
        /// </param>
        /// <returns>
        /// Une collection de XElement si des noeuds enfants sont trouvés, null sinon
        /// </returns>
        public ObservableCollection<XElement> GetNodesByCode(String Code)
        {
            ObservableCollection<XElement> Result = this.GetNodesByCode(Code, RootNode);

            return Result;
        } // endMethod: GetNodeByCode

        /// <summary>
        /// Rechercher les noeuds enfants Xml en connaissant la valeur de l'attribut "code"
        /// </summary>
        /// <param name="Code">
        /// Le Code désignant les noeuds
        /// </param>
        /// <param name="Source">
        /// L'Element XML Parent
        /// </param>
        /// <returns>
        /// Une collection de XElement si des noeuds enfants sont trouvés, null sinon
        /// </returns>
        public ObservableCollection<XElement> GetNodesByCode(String Code, XElement Source)
        {
            ObservableCollection<XElement> Result = null;

            if (Source == null)
            {
                return null;
            }

            IEnumerable<XElement> query = from node in Source.Elements()
                                          select node;

            Result = new ObservableCollection<XElement>();
            foreach (XElement node in query)
            {
                XAttribute attribute = node.Attribute(XML_ATTRIBUTE.CODE);
                if (attribute != null)
	            {
		            if (attribute.Value.Trim().ToUpper() == Code.ToUpper())
                    {
                        Result.Add(node);
                    } 
	            }
            }

            //// S'il n'y a pas de résultats sur les noeuds des descendants directs, rechercher chez les descendants de niveau 2
            //if (Result.Count == 0)
            //{
            //    IEnumerable<XElement> Nodes = from node in Source.Descendants()
            //                                    select node;
            //    foreach (var item in Nodes)
            //    {
            //        ObservableCollection<XElement> XE = this.GetNodesByCode(Code, item);
            //        if (XE != null)
            //        {
            //            Result = XE;
            //            break;
            //        }
            //    } 
            //}
            //else
            //{
            //    Result = null;
            //}
            return Result;
        } // endMethod: GetNodeByCode

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: XMLProcessing
}
