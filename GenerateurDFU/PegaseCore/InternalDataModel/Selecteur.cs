using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JAY.PegaseCore
{
    public enum TypeSelecteur
    {
        NonDefini = 0,
        Etiquette = 1,
        SelecteurMecanique = 2,
        SelecteurElectronique = 3
    }

    public enum ComportementBorne
    {
        Passage_borne_oppose_sans_restauration = 0,
        Rester_sur_borne_sans_restauration = 1,
        Passage_borne_oppose_avec_restauration = 4,
        Rester_sur_borne_avec_restauration = 5
    }
    /// <summary>
    /// La classe selecteur
    /// </summary>
    public class Selecteur
    {
        // Variables
        #region Variables

        private XMLCore.XMLProcessing _xmlSelecteur;
        private Int32 _nbPosition;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// L'identifiant du selecteur
        /// </summary>
        public String IdentSelecteur
        {
            get;
            set;
        } // endProperty: IdentSelecteur

        /// <summary>
        /// Le type de sélecteur
        /// </summary>
        public TypeSelecteur SelecteurType
        {
            get;
            set;
        } // endProperty: SelecteurType

        /// <summary>
        /// Le nombre de position
        /// </summary>
        public Int32 NbPosition
        {
            get
            {
                return this._nbPosition;
            }
            set
            {
                this._nbPosition = value;
            }
        } // endProperty: NbPosition

        /// <summary>
        /// Le comportement aux bornes
        /// </summary>
        public ComportementBorne ComportementAuxBornes
        {
            get;
            set;
        } // endProperty: ComportementAuxBornes

        /// <summary>
        /// Nom de l'organe à recopier
        /// </summary>
        public String OrganeARecopier
        {
            get;
            set;
        } // endProperty: OrganeARecopier

        /// <summary>
        /// Le nom du bouton incrémenter
        /// </summary>
        public String BtIncrementer
        {
            get;
            set;
        } // endProperty: BtIncrementer

        /// <summary>
        /// Le nom du bouton décrémenter
        /// </summary>
        public String BtDecrementer
        {
            get;
            set;
        } // endProperty: BtDecrementer

        /// <summary>
        /// Le nom du bouton permettant d'aller à la position max
        /// </summary>
        public String BtPositionnerAValMax
        {
            get;
            set;
        } // endProperty: BtPositionnerAValMax

        /// <summary>
        /// Le nom du bouton permettant d'aller à la position min
        /// </summary>
        public String BtPositionnerAValMin
        {
            get;
            set;
        } // endProperty: BtPositionnerAValMin

        #endregion

        // Constructeur
        #region Constructeur

        // construire à partir du xml
        public Selecteur(XElement selecteur)
        {
            this.InitFromXml(selecteur);
        }

        // construire à partir d'un identifiant de sélecteur
        public Selecteur(String IDSelecteur)
        {
            this.IdentSelecteur = IDSelecteur;
            this.NbPosition = 2;
            this.SelecteurType = TypeSelecteur.SelecteurElectronique;
            this.ComportementAuxBornes = ComportementBorne.Passage_borne_oppose_avec_restauration;
            this.OrganeARecopier = "";
            this.BtDecrementer = "";
            this.BtIncrementer = "";
            this.BtPositionnerAValMax = "";
            this.BtPositionnerAValMin = "";
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Initialiser les données du sélecteur depuis le XML
        /// </summary>
        public void InitFromXml ( XElement selecteur )
        {
            // 0 - Initialisation
            this._xmlSelecteur = new XMLCore.XMLProcessing();
            this._xmlSelecteur.OpenXML(selecteur);
            // 1 - Identifiant du sélecteur
            if (this._xmlSelecteur.GetNodeByPath("IdentSélecteur") != null)
            {
                this.IdentSelecteur = this._xmlSelecteur.GetNodeByPath("IdentSélecteur").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            }
            else if (this._xmlSelecteur.GetNodeByPath("IdentSelecteur") != null)
            {
                this.IdentSelecteur = this._xmlSelecteur.GetNodeByPath("IdentSelecteur").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            }
            else if (this._xmlSelecteur.GetNodeByPath("IdentS?lecteur") != null)
            {
                this.IdentSelecteur = this._xmlSelecteur.GetNodeByPath("IdentS?lecteur").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            }
            else
            {
                throw new Exception("Identifiant du sélecteur introuvable");
            }

            // 2 - Type du selecteur
            String SV = this._xmlSelecteur.GetNodeByPath("TypeSelecteur").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            TypeSelecteur TS = TypeSelecteur.NonDefini;
            switch (SV)
            {
                case "Etiquette":
                    TS = TypeSelecteur.Etiquette;
                    break;
                case "IndicateurPosition":
                    TS = TypeSelecteur.SelecteurMecanique;
                    break;
                case "Selecteur":
                    TS = TypeSelecteur.SelecteurElectronique;
                    break;
                default:
                    TS = TypeSelecteur.NonDefini;
                    break;
            }

            this.SelecteurType = TS;

            // 3 - Comportement aux bornes

            SV = this._xmlSelecteur.GetNodeByPath("ComportementAuxBornes").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            ComportementBorne CB = ComportementBorne.Passage_borne_oppose_avec_restauration;

            if (SV.Length > 0)
            {
                Int32 Val = Convert.ToInt32(SV);
                CB = (ComportementBorne)Val;
            }
            this.ComportementAuxBornes = CB;

            // 4 - Nb Position
            SV = this._xmlSelecteur.GetNodeByPath("NbPosition").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            Int32 NbPos = 0;
            if (SV.Length > 0)
            {
                NbPos = Convert.ToInt32(SV);
            }

            this.NbPosition = NbPos;

            // 5 - Organes à recopier
            this.OrganeARecopier = this._xmlSelecteur.GetNodeByPath("OrganeARecopier").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            // 6 - Bouton incrémenter
            this.BtIncrementer = this._xmlSelecteur.GetNodeByPath("BtIncrementer").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            // 7 - Bouton décrémenter
            this.BtDecrementer = this._xmlSelecteur.GetNodeByPath("BtDecrementer").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            // 8 - Bouton positionner Aval Max
            this.BtPositionnerAValMax = this._xmlSelecteur.GetNodeByPath("BtPositionnerAValMax").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            // 9 - Bouton positionner Aval Min
            this.BtPositionnerAValMin = this._xmlSelecteur.GetNodeByPath("BtPositionnerAValMin").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
        } // endMethod: InitFromXml

        /// <summary>
        /// Supprimer le selecteur
        /// </summary>
        public void Remove ( )
        {
            this._xmlSelecteur.RootNode.Remove();
        } // endMethod: Remove

        /// <summary>
        /// retourne une section XML contenant la description de ce sélecteur en vue de la sérialisation
        /// </summary>
        public void SerialiseSelecteur ( XElement selecteur )
        {
            String SV;

            // Initialiser Cmd avec la structure vide d'un Selecteur
            XMLCore.XMLProcessing XProcess = new XMLCore.XMLProcessing();
            XProcess.OpenXML(selecteur);

            // 1 - Identifiant du sélecteur
            XProcess.GetNodeByPath("IdentSélecteur").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.IdentSelecteur;

            // 2 - Type du selecteur
            switch (this.SelecteurType)
            {
                case TypeSelecteur.NonDefini:
                    SV = "NonDefini";
                    break;
                case TypeSelecteur.SelecteurMecanique:
                    SV = "IndicateurPosition";
                    break;
                case TypeSelecteur.SelecteurElectronique:
                    SV = "Selecteur";
                    break;
                case TypeSelecteur.Etiquette:
                    SV = "Etiquette";
                    break;
                default:
                    SV = "NonDefini";
                    break;
            }
            XProcess.GetNodeByPath("TypeSelecteur").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = SV;

            // 3 - Comportement aux bornes
            XProcess.GetNodeByPath("ComportementAuxBornes").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = ((Int32)this.ComportementAuxBornes).ToString();

            // 4 - Nb Position
            XProcess.GetNodeByPath("NbPosition").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.NbPosition.ToString();

            // 5 - Organes à recopier
            XProcess.GetNodeByPath("OrganeARecopier").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.OrganeARecopier;

            // 6- Bouton incrémenter
            XProcess.GetNodeByPath("BtIncrementer").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.BtIncrementer;

            // 7 - Bouton décrémenter
            XProcess.GetNodeByPath("BtDecrementer").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.BtDecrementer;

            // 8 - Bouton positionner Aval Max
            XProcess.GetNodeByPath("BtPositionnerAValMax").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.BtPositionnerAValMax;

            // 9 - Bouton positionner Aval Min
            XProcess.GetNodeByPath("BtPositionnerAValMin").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.BtPositionnerAValMin;

        } // endMethod: SaveSelecteur

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: Selecteur
}
