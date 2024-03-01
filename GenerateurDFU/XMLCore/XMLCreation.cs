using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace JAY.XMLCore
{
    /// <summary>
    /// Classe de manipulation des fichiers XML, fusion / délession...
    /// </summary>
    public class XMLCreation
    {
        // Constantes
        #region Constantes

        private const String MODE_OVERRIDE = "Override";
        private const String MODE_ADD = "Add";
        private const String ROOT = "Root";
        private const String XMLPATH = "XMLPath";
        private const String FICHEPACK = "FichePack";

        #endregion

        // Variables
        #region Variables

        private XDocument _doc;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le document Xml en cours de création
        /// </summary>
        public XDocument DocumentXml
        {
            get
            {
                return this._doc;
            }
            private set
            {
                this._doc = value;
            }
        } // endProperty: DocumentXml

        #endregion

        // Constructeur
        #region Constructeur

        public XMLCreation(XDocument Base)
        {
            // Récupérer le FichePack du document Base
            XDeclaration declaration;

            try
            {
                declaration = new XDeclaration(Base.Declaration);
            }
            catch (Exception e)
            {
                declaration = new XDeclaration("1.0", Constantes.XML_ENCODING, "yes");
            }

            this.DocumentXml = new XDocument(declaration, this.GetFichePack(Base)); 
            //this.DocumentXml = Base;
        }

        #endregion

        // Méthodes
        #region Méthodes

        /// <summary>
        /// Sélectionner la FichePack du fichier
        /// </summary>
        public XElement GetFichePack(XDocument Source)
        {
            XElement Result = null;

            IEnumerable<XElement> SNodes = from element in Source.Elements(ROOT).Elements(XMLPATH)
                                           select element;
            XElement SNode = SNodes.FirstOrDefault();

            if (SNode != null)
            {
                Result = SNode.Element(FICHEPACK);
            }
            else
            {
                Result = Source.Element(FICHEPACK);
            }

            return Result;
        } // endMethod: GetFichePack

        /// <summary>
        /// Sélectionner la FichePack du fichier
        /// </summary>
        public XElement GetFichePack ( XElement Source )
        {
            XElement Result = null;

            IEnumerable<XElement> SNodes = from element in Source.Elements(XMLPATH)
                                           select element;
            XElement SNode = SNodes.FirstOrDefault();

            if (SNode != null)
            {
                Result = SNode.Element(FICHEPACK); 
            }
            else
	        {
                Result = Source.Elements(FICHEPACK).First();
	        }
            
            return Result;
        } // endMethod: GetFichePack

        /// <summary>
        /// Insérer le premier élément et le retourner après ajout dans la collection
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public ObservableCollection<XElement> AddFirstElement(XDocument Source)
        {
            ObservableCollection<XElement> Result = new ObservableCollection<XElement>();
            IEnumerable<XElement> Paths = from element in Source.Elements(ROOT).Elements(XMLPATH)
                                          select element;

            // Pour tous les chemins, les insérer dans le document total
            XElement SFichePack;
            XElement path = Paths.First();

            // 1 - Sélectionner le noeud à insérer dans le document Source
            // 1.1 - Nettoyer la chaîne du chemin
            SFichePack = this.GetFichePack(path);
            String SPath;
            SPath = path.Attribute(XML_ATTRIBUTE.Value).Value.Replace("\n", "");
            SPath = SPath.Replace("\t", "");
            if (SPath.Substring(0, 1) == "/")
            {
                SPath = SPath.Substring(1);
            }
            SPath = SPath.Replace(FICHEPACK, "");
            // 1.2 - Sélectionner le Noeud correspondant
            if (SFichePack != null)
            {
                XMLProcessing findSourceNode = new XMLProcessing();
                findSourceNode.OpenXML(SFichePack);
                IEnumerable<XElement> SElements = findSourceNode.GetNodeByPath(SPath);
                // 2 - Sélectionner le noeud parent du noeud à insérer
                //     dans le document total
                XMLProcessing findDestNode = new XMLProcessing();
                findDestNode.OpenXML(this.DocumentXml.Element(FICHEPACK));
                IEnumerable<XElement> DElements = findDestNode.GetNodeByPath(SPath);
                // Le chemin n'existe pas dans le document de destination
                // Le créer et insérer les noeuds
                if (DElements == null)
                {
                    String[] PathElements = SPath.Split(new Char[] { '/' });
                    String TestPath = "";
                    IEnumerable<XElement> OldDElements = DElements;

                    foreach (String pathElement in PathElements)
                    {
                        // tester toutes les étapes du chemin jusqu'à trouver l'étape qui
                        // n'existe pas
                        if (pathElement != "")
                        {
                            TestPath += "/" + pathElement;
                        }
                        DElements = findDestNode.GetNodeByPath(TestPath);
                        if (DElements == null)
                        {
                            // le chemin n'existe pas à partir de ce point
                            // le copier
                            XElement node = findSourceNode.GetNodeByPath(TestPath).First();
                            XElement parentNode = OldDElements.First();
                            Result.Add(parentNode);
                            parentNode.Add(node);
                            // Stocker l'ancien élément
                            OldDElements = findDestNode.GetNodeByPath(TestPath);
                        }
                        else
                        {
                            OldDElements = DElements;
                        }
                    }
                }
                else
                {
                    if (path.Attribute(XML_ATTRIBUTE.Mode).Value == MODE_ADD)
                    {
                        // le chemin existe, insérer les noeuds
                        XElement DestNode = DElements.FirstOrDefault();

                        if (DestNode != null && SElements != null)
                        {
                            // 3 - Inserer les noeuds provenant de la source dans le document total
                            foreach (var element in SElements)
                            {
                                foreach (var node in element.Elements())
                                {
                                    DestNode.Add(node);
                                    //if (Result == null)
                                    //{
                                    Result.Add((XElement)DestNode.LastNode);
                                    //} 
                                }
                            }
                        }
                    }
                    else if (path.Attribute(XML_ATTRIBUTE.Mode).Value == MODE_OVERRIDE)
                    {
                        // le chemin existe, insérer les noeuds
                        XElement DestNode = DElements.FirstOrDefault();

                        DestNode.ReplaceWith(SElements.First());
                    }
                }
            }
            return Result;
        }

        /// <summary>
        /// La fusion d'un XDocument à la base en cours
        /// </summary>
        public void Add ( XDocument Source )
        {
            IEnumerable<XElement> Paths = from element in Source.Elements(ROOT).Elements(XMLPATH)
                                             select element;

            // Pour tous les chemins, les insérer dans le document total
            XElement SFichePack;

            foreach (XElement path in Paths)
	        {
                // 1 - Sélectionner le noeud à insérer dans le document Source
                // 1.1 - Nettoyer la chaîne du chemin
                SFichePack = this.GetFichePack(path);
                String SPath;
                SPath = path.Attribute(XML_ATTRIBUTE.Value).Value.Replace("\n", "");
                SPath = SPath.Replace("\t", "");
                if (SPath.Substring(0,1) == "/")
                {
                    SPath = SPath.Substring(1);
                }
                SPath = SPath.Replace(FICHEPACK, "");
                // 1.2 - Sélectionner le Noeud correspondant
                if (SFichePack != null)
                {
                    XMLProcessing findSourceNode = new XMLProcessing();
                    findSourceNode.OpenXML(SFichePack);
                    IEnumerable<XElement> SElements = findSourceNode.GetNodeByPath(SPath);
                    // 2 - Sélectionner le noeud parent du noeud à insérer
                    //     dans le document total
                    XMLProcessing findDestNode = new XMLProcessing();
                    findDestNode.OpenXML(this.DocumentXml.Element(FICHEPACK));
                    IEnumerable<XElement> DElements = findDestNode.GetNodeByPath(SPath);
                    // Le chemin n'existe pas dans le document de destination
                    // Le créer et insérer les noeuds
                    if (DElements == null)
                    {
                        String[] PathElements = SPath.Split(new Char[] { '/' });
                        String TestPath = "";
                        IEnumerable<XElement> OldDElements = DElements;

                        foreach (String pathElement in PathElements)
                        {
                            // tester toutes les étapes du chemin jusqu'à trouver l'étape qui
                            // n'existe pas
                            if (pathElement != "")
                            {
                                TestPath += "/" + pathElement;
                            }
                            DElements = findDestNode.GetNodeByPath(TestPath);
                            if (DElements == null)
                            {
                                // le chemin n'existe pas à partir de ce point
                                // le copier
                                if (findSourceNode.GetNodeByPath(TestPath) != null)
                                {
                                    XElement node = findSourceNode.GetNodeByPath(TestPath).First();
                                    XElement parentNode = OldDElements.First();
                                    parentNode.Add(node);
                                    // Stocker l'ancien élément
                                    OldDElements = findDestNode.GetNodeByPath(TestPath);
                                }
                            }
                            else
                            {
                                OldDElements = DElements;
                            }
                        }
                    }
                    else
                    {
                        if (path.Attribute(XML_ATTRIBUTE.Mode).Value == MODE_ADD)
                        {
                            // le chemin existe, insérer les noeuds
                            XElement DestNode = DElements.FirstOrDefault();

                            if (DestNode != null && SElements != null)
                            {
                                // 3 - Inserer les noeuds provenant de la source dans le document total
                                foreach (var element in SElements)
                                {
                                    DestNode.Add(element.Elements());
                                }
                            }
                        }
                        else if (path.Attribute(XML_ATTRIBUTE.Mode).Value == MODE_OVERRIDE)
                        {
                            // le chemin existe, insérer les noeuds
                            XElement DestNode = DElements.FirstOrDefault();

                            if (SElements != null)
                            {
                                DestNode.ReplaceWith(SElements.First()); 
                            }
                        }
                    }
                }
	        }
        } // endMethod: Fusion

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: XMLCreation
}
