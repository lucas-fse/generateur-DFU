using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JAY.PegaseCore.Helper
{
    /// <summary>
    /// Le classe de référence des produits
    /// </summary>
    public class RefProduit
    {
        // Variables
        #region Variables

        private ObservableCollection<Produit> _produits;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La liste de produits pour cette référence produit
        /// </summary>
        public ObservableCollection<Produit> Produits
        {
            get
            {
                if (this._produits == null)
                {
                    this._produits = new ObservableCollection<Produit>();
                }

                return this._produits;
            }
            private set
            {
                this._produits = value;
            }
        } // endProperty: Produits

        /// <summary>
        /// Le libellé
        /// </summary>
        public String Libelle
        {
            get;
            private set;
        } // endProperty: Libelle

        /// <summary>
        /// La valeur
        /// </summary>
        public String Valeur
        {
            get;
            private set;
        } // endProperty: Valeur

        /// <summary>
        /// l'Option du champ
        /// </summary>
        public String OptionChamp
        {
            get;
            private set;
        } // endProperty: OptionChamp

        /// <summary>
        /// le debut du masque sur le champ option
        /// </summary>
        public Int32 OptionMaskDebut
        {
            get;
            private set;
        } // endProperty: OptionMaskDebut

        /// <summary>
        /// la longueur du masque sur le champ option
        /// </summary>
        public Int32 OptionMaskTaille
        {
            get;
            private set;
        } // endProperty: OptionMaskTaille

        /// <summary>
        /// Le nom de l'image MO (1)
        /// </summary>
        public String ImgMo1
        {
            get;
            private set;
        } // endProperty: ImgMo1

        /// <summary>
        /// Le nom de alias de l'image MO (1)
        /// </summary>
        public String Alias
        {
            get;
            private set;
        } // endProperty: Alias


        /// <summary>
        /// Le nom de l'image MO (2)
        /// </summary>
        public String ImgMo2
        {
            get;
            private set;
        } // endProperty: ImgMo2

        public String SupRef
        {
            get;
            private set;
        }

        #endregion

        // Constructeur
        #region Constructeur

        public RefProduit(XElement refProduit)
        {
            if (refProduit != null)
            {
                foreach (var produit in refProduit.Descendants("Produit"))
                {
                    Produit p = new Produit(produit);

                    this.Produits.Add(p);
                }

                XAttribute attrib;

                // Libelle
                attrib = refProduit.Attribute("Libelle");
                if (attrib != null)
                {
                    this.Libelle = attrib.Value;
                }
                else
                {
                    this.Libelle = "";
                }

                // Valeur
                attrib = refProduit.Attribute("valeur");
                if (attrib != null)
                {
                    this.Valeur = attrib.Value;
                }
                else
                {
                    this.Valeur = "";
                }

                // OptionChamp
                attrib = refProduit.Attribute("optionchamp");
                if (attrib != null)
                {
                    this.OptionChamp = attrib.Value;
                }
                else
                {
                    this.OptionChamp = "";
                }

                // OptionMaskDebut
                attrib = refProduit.Attribute("optionMaskDebut");
                if (attrib != null)
                {
                    this.OptionMaskDebut = Convert.ToInt32(attrib.Value);
                }
                else
                {
                    this.OptionMaskDebut = 0;
                }

                // OptionMaskTaille
                attrib = refProduit.Attribute("optionMaskTaille");
                if (attrib != null)
                {
                    this.OptionMaskTaille = Convert.ToInt32(attrib.Value);
                }
                else
                {
                    this.OptionMaskTaille = 0;
                }

                // ImgMo1
                attrib = refProduit.Attribute("imgmo1");
                if (attrib != null)
                {
                    this.ImgMo1 = attrib.Value;
                }
                else
                {
                    this.ImgMo1 = "";
                }
                // ImgMo1
                attrib = refProduit.Attribute("alias");
                if (attrib != null)
                {
                    this.Alias = attrib.Value;
                }
                else
                {
                    this.Alias = "";
                }
                // ImgMo2
                attrib = refProduit.Attribute("imgmo2");
                if (attrib != null)
                {
                    this.ImgMo2 = attrib.Value;
                }
                else
                {
                    this.ImgMo2 = "";
                }
                attrib = refProduit.Attribute("refsup");
                if (attrib != null)
                {
                    this.SupRef = attrib.Value;
                }
                else
                {
                    this.SupRef = "";
                }
            }
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: RefProduit

    /// <summary>
    /// Le classe des produits
    /// </summary>
    public class Produit
    {
        // Variables
        #region Variables

        private ObservableCollection<Variante> _variantes;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La liste des variantes du produit
        /// </summary>
        public ObservableCollection<Variante> Variantes
        {
            get
            {
                if (this._variantes == null)
                {
                    this._variantes = new ObservableCollection<Variante>();
                }

                return this._variantes;
            }
            private set
            {
                this._variantes = value;
            }
        } // endProperty: Variantes

        /// <summary>
        /// Le libellé du produit
        /// </summary>
        public String Libelle
        {
            get;
            private set;
        } // endProperty: Libelle

        /// <summary>
        /// le début du mask de l'option
        /// </summary>
        public Int32 optionMaskDebut
        {
            get;
            private set;
        } // endProperty: optionMaskDebut

        /// <summary>
        /// la taille du masque option
        /// </summary>
        public Int32 optionMaskTaille
        {
            get;
            private set;
        } // endProperty: optionMaskTaille

        #endregion

        // Constructeur
        #region Constructeur

        public Produit(XElement produit)
        {
            foreach (var variante in produit.Descendants("Variante"))
            {
                Variante V = new Variante(variante);
                this.Variantes.Add(V);
            }

            XAttribute attrib;

            // attribut libellé
            attrib = produit.Attribute("Libelle");

            if (attrib != null)
            {
                this.Libelle = attrib.Value;
            }
            else
            {
                this.Libelle = "";
            }

            // attribut optionMaskDebut
            attrib = produit.Attribute("optionMaskDebut");

            if (attrib != null)
            {
                this.optionMaskDebut = Convert.ToInt32(attrib.Value);
            }
            else
            {
                this.optionMaskDebut = 0;
            }

            // attribut optionMaskTaille
            attrib = produit.Attribute("optionMaskTaille");

            if (attrib != null)
            {
                this.optionMaskTaille = Convert.ToInt32(attrib.Value);
            }
            else
            {
                this.optionMaskTaille = 0;
            }
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: Produit

    /// <summary>
    /// Les variantes des produits
    /// </summary>
    public class Variante
    {
        // Variables
        #region Variables
        private ObservableCollection<Fichier> _fichiers;
        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Les fichiers utilisés pour décrire cette variante
        /// </summary>
        public ObservableCollection<Fichier> Fichiers
        {
            get
            {
                if (this._fichiers == null)
                {
                    this._fichiers = new ObservableCollection<Fichier>();
                }

                return this._fichiers;
            }
            set
            {
                this._fichiers = value;
            }
        } // endProperty: Fichiers

        /// <summary>
        /// Le libellé de la variante
        /// </summary>
        public String Libelle
        {
            get;
            private set;
        } // endProperty: Libelle

        /// <summary>
        /// La valeur 'code' de la variante
        /// </summary>
        public String Valeur
        {
            get;
            set;
        } // endProperty: Valeur

        /// <summary>
        /// l'image mo 1 si besoin
        /// </summary>
        public String imgmo1
        {
            get;
            set;
        } // endProperty: imgmo1

        /// <summary>
        /// Le nom de l'image mo numero 2
        /// </summary>
        public String imgmo2
        {
            get;
            set;
        } // endProperty: imgmo2

        public String supref
        {
            get;
            set;
        }

        /// <summary>
        /// Le nom de l'image mo numero 2
        /// </summary>
        public String alias
        {
            get;
            set;
        } // endProperty: imgmo2

        #endregion

        // Constructeur
        #region Constructeur

        public Variante(XElement variante)
        {
            XAttribute Attrib;

            foreach (var fichier in variante.Descendants("Fichier"))
            {
                Fichier F = new Fichier(fichier);
                this.Fichiers.Add(F);    
            }

            // Libelle
            Attrib = variante.Attribute("Libelle");
            if (Attrib != null)
            {
                this.Libelle = Attrib.Value;
            }
            else
            {
                this.Libelle = "";
            }

            // Valeur
            Attrib = variante.Attribute("valeur");
            if (Attrib != null)
            {
                this.Valeur = Attrib.Value;
            }
            else
            {
                this.Valeur = "";
            }

            // imgmo1
            Attrib = variante.Attribute("imgmo1");
            if (Attrib != null)
            {
                this.imgmo1 = Attrib.Value;
            }
            else
            {
                this.imgmo1 = "";
            }

            // imgmo2
            Attrib = variante.Attribute("imgmo2");
            if (Attrib != null)
            {
                this.imgmo2 = Attrib.Value;
            }
            else
            {
                this.imgmo2 = "";
            }
            Attrib = variante.Attribute("alias");
            if (Attrib != null)
            {
                this.alias = Attrib.Value;
            }
            else
            {
                this.alias = "";
            }
            Attrib = variante.Attribute("refsup");
            if (Attrib != null)
            {
                this.supref = Attrib.Value;
            }
            else
            {
                this.supref = "";
            }
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: Variante

    /// <summary>
    /// La classe Fichier décrivant les fichiers utilisés pour ces produits
    /// </summary>
    public class Fichier
    {
        // Variables
        #region Variables

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le nom du fichier XML
        /// </summary>
        public String xmlFileName
        {
            get;
            private set;
        } // endProperty: xmlFileName

        #endregion

        // Constructeur
        #region Constructeur

        public Fichier(XElement fichier)
        {
            XAttribute attrib = fichier.Attribute("nom");

            if (attrib != null)
            {
                this.xmlFileName = attrib.Value;
            }
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: Fichier
    
    /// <summary>
    /// Décodage des références industrielles
    /// </summary>
    public class DecodConfigRefIndusAppl
    {
        // Variables singleton
        private static DecodConfigRefIndusAppl _instance;
        static readonly object instanceLock = new object();

        // Variables
        #region Variables

        private ObservableCollection<RefProduit> _refProduits;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La référence des produits
        /// </summary>
        public ObservableCollection<RefProduit> RefProduits
        {
            get
            {
                if (this._refProduits == null)
                {
                    this._refProduits = new ObservableCollection<RefProduit>();
                }

                return this._refProduits;
            }
            set
            {
                this._refProduits = value;
            }
        } // endProperty: RefProduits

        #endregion


        // Constructeur
        #region Constructeur

        private DecodConfigRefIndusAppl()
        {
            this.BuildRefProduits();
        }

        #endregion

        // Méthodes
        #region Méthodes

        // Retourne une instance unique de la classe
        public static DecodConfigRefIndusAppl Get()
        {
            lock (instanceLock)
            {
                if (_instance == null)
                    _instance = new DecodConfigRefIndusAppl();

                return _instance;
            }
        }

        /// <summary>
        /// Construire la référence des produits
        /// </summary>
        private void BuildRefProduits ( )
        {
            // 1 - Ouvrir le fichier ConfigRefIndus
            FileCore.FilePackage fp = FileCore.FilePackage.GetParamData();
            if (fp != null)
            {
                Stream stream = fp.GetPartStream(DefaultValues.Get().ConfigRefIndusAppl);

                XDocument doc = XDocument.Load(stream);
                stream.Close();

                XElement root = doc.Root;

                foreach (var refProduit in root.Descendants("Refproduit"))
                {
                    RefProduit rp = new RefProduit(refProduit);

                    this.RefProduits.Add(rp);
                }

                fp.ClosePackage(); 
            }

        } // endMethod: BuildRefProduits

        /// <summary>
        /// Acquérir les images du rapport correspondant à ce type (B2, B6, G6...)
        /// </summary>
        public void GetRapportImg ( String Type, String reference, out ImgDefinition Img1, out ObservableCollection<ImgDefinition> ImgSecondaire )
        {
            ImgSecondaire = new ObservableCollection<ImgDefinition>();

            var Query = from refProduit in this.RefProduits
                        where refProduit.Valeur == Type
                        select refProduit;

            if (Query.Count() > 0)
            {
                RefProduit RP = Query.First();
                Img1 = new ImgDefinition(Query.First().ImgMo1, Query.First().ImgMo2, Query.First().Alias);


                // Chercher une image complémentaire Img2
                if (reference != null && reference != "")
                {
                    foreach (var product in RP.Produits)
                    {
                        if (reference.Length >= product.optionMaskDebut + product.optionMaskTaille)
                        {
                            String code = reference.Substring(product.optionMaskDebut, product.optionMaskTaille);
                            foreach (var variante in product.Variantes)
                            {
                                if (variante.Valeur == code)
                                {
                                    if (!variante.imgmo1.Equals("") || !variante.imgmo2.Equals("") || !variante.alias.Equals("") || !variante.supref.Equals(""))
                                        ImgSecondaire.Add(new ImgDefinition(variante.imgmo1, variante.imgmo2, variante.alias, variante.supref));
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Img1 = new ImgDefinition("","", "","");
            }
        } // endMethod: GetRapportImg

        public bool GetPresenceKlaxon(String Type, String reference, out bool klaxonpossible)
        {
          
            var Query = from refProduit in this.RefProduits
                        where refProduit.Valeur == Type
                        select refProduit;
            klaxonpossible = false;
            if (Query.Count() > 0)
            {
                RefProduit RP = Query.First();
                if (reference != null && reference != "")
                {
                    foreach (var product in RP.Produits)
                    {
                        if (reference.Length >= product.optionMaskDebut + product.optionMaskTaille)
                        {
                            String code = reference.Substring(product.optionMaskDebut, product.optionMaskTaille);
                            foreach (var variante in product.Variantes)
                            {
                                if (variante.Libelle.Equals("KLAXON"))
                                {
                                    klaxonpossible = true;
                                    if ((variante.Valeur == code))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: DecodConfigRefIndusAppl

    public class ImgDefinition
    {

        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Alias { get; set; }

        public string Refsup { get; set; }

        public ImgDefinition(string img1, string img2, string alias, string refsup)
        {
            Image1 = img1;
            Image2 = img2;
            Alias = alias;
            Refsup = refsup;
        }
        public ImgDefinition(string img1, string img2, string alias)
        {
            Image1 = img1;
            Image2 = img2;
            Alias = alias;
        }
    }
}
