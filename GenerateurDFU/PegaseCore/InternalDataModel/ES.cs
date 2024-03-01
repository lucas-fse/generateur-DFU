using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using JAY.XMLCore;
using Mvvm = GalaSoft.MvvmLight;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Le type définissant s'il s'agit d'une entrée ou d'une sortie
    /// </summary>
    public enum TypeES 
    {
        EAna,
        SAna,
        ETor,
        STor
    }

    /// <summary>
    /// Classe de base des entrées / sorties
    /// </summary>
    public class ES : Mvvm.ViewModelBase
    {
        // Variables
        #region Constantes

        private String OLD_RELAIS_MARCHE = "RELAIS_MARCHE";
        private String NEW_RELAIS_MARCHE = "RELAIS_13";

        #endregion

        #region Variables

        private Int32 _id;
        private Int32 _idCarte;
        private String _mnemoBornier;
        private String _mnemoHardware;
        private String _mnemoClient;
        private String _mnemoLogique;
        private Int32 _indiceFamille;
        private Int32 _indiceES;
        private Int32 _carte;
        private Int32 _voie;
        private TypeES _type;
        private String _typename;

        // en sécurité
        private String _valeurInitiale;
        private String _valeurEnSecurite;
        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Valeur initiale (mise en sécurité)
        /// </summary>
        public String ValeurInitiale
        {
            get
            {
                return this._valeurInitiale;
            }
            set
            {
                this._valeurInitiale = value;
            }
        } // endProperty: ValeurInitiale

        /// <summary>
        /// Valeur en sécurité
        /// </summary>
        public String ValeurEnSecurite
        {
            get
            {
                return this._valeurEnSecurite;
            }
            set
            {
                this._valeurEnSecurite = value;
            }
        } // endProperty: ValeurEnSecurite

        /// <summary>
        /// Type de l'entrée
        /// </summary>
        public TypeES Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        } // endProperty: Type
                /// <summary>
        /// Type de l'entrée
        /// </summary>
        public String TypeName
        {
            get
            {
                return this._typename;
            }
            set
            {
                this._typename = value;
            }
        } // endProperty: Typename
       
        /// <summary>
        /// L'identifiant de l'entrée sortie
        /// </summary>
        public Int32 ID
        {
            get
            {
                return this._id;
            }
            protected set
            {
                this._id = value;
            }
        } // endProperty: ID

        /// <summary>
        /// L'Identifiant de la carte portant l'entrée / sortie
        /// </summary>
        public Int32 IDCarte
        {
            get
            {
                return this._idCarte;
            }
            protected set
            {
                this._idCarte = value;
            }
        } // endProperty: IDCarte

        /// <summary>
        /// La mnémonique du bornier
        /// </summary>
        public String MnemoBornier
        {
            get
            {
                return this._mnemoBornier;
            }
            protected set
            {
                this._mnemoBornier = value;
            }
        } // endProperty: MnemoBornier

        /// <summary>
        /// Mnemonique hardware
        /// </summary>
        public String MnemoHardware
        {
            get
            {
                return this._mnemoHardware;
            }
            set
            {
                this._mnemoHardware = value;
            }
        } // endProperty: MnemoHardware

        /// <summary>
        /// Le mnémonique spécifique défini par le client
        /// </summary>
        public String MnemoClient
        {
            get
            {
                return this._mnemoClient;
            }
            set
            {
                this._mnemoClient = value;
            }
        } // endProperty: MnemoClient

        /// <summary>
        /// Mnemonique logique
        /// </summary>
        public String MnemoLogique
        {
            get
            {
                return this._mnemoLogique;
            }
            protected set
            {
                this._mnemoLogique = value;
            }
        } // endProperty: MnemoLogique

        /// <summary>
        /// Indice de la famille
        /// </summary>
        public Int32 IndiceFamille
        {
            get
            {
                return this._indiceFamille;
            }
            protected set
            {
                this._indiceFamille = value;
            }
        } // endProperty: IndiceFamille

        /// <summary>
        /// L'indice des entrées / sorties
        /// </summary>
        public Int32 IndiceES
        {
            get
            {
                return this._indiceES;
            }
            protected set
            {
                this._indiceES = value;
            }
        } // endProperty: IndiceES

        /// <summary>
        /// Carte
        /// </summary>
        public Int32 Carte
        {
            get
            {
                return this._carte;
            }
            protected set
            {
                this._carte = value;
            }
        } // endProperty: Carte

        /// <summary>
        /// numéro de la voie sur la carte
        /// </summary>
        public Int32 Voie
        {
            get
            {
                return this._voie;
            }
            protected set
            {
                this._voie = value;
            }
        } // endProperty: Voie

        #endregion

        // Constructeur
        #region Constructeur

        /// <summary>
        /// Initialisation des entrées / sorties à partir du XML
        /// </summary>
        /// <param name="Configuration"></param>
        /// <param name="Pilotage"></param>
        public ES(XElement Configuration, XElement Pilotage, XElement ReglageSecurite)
        {
            // Initialiser les données de la classe à partir des elements XML
            String Value;
            XMLProcessing XProcess = new XMLProcessing();

            // -------------  Section Configuration  --------------

            if (Configuration != null)
            {
                XProcess.OpenXML(Configuration);

                // ID
                Value = XProcess.GetValue("Id", "", "", XML_ATTRIBUTE.VALUE);
                if (Value != "")
                {
                    this.ID = Convert.ToInt32(Value);
                }
                else
                {
                    this.ID = -1;
                }

                // IDCarte
                Value = XProcess.GetValue("IdCarte", "", "", XML_ATTRIBUTE.VALUE);
                if (Value != "")
                {
                    this.IDCarte = Convert.ToInt32(Value);
                }

                // MnemoBornier
                Value = XProcess.GetValue("MnemoBornier", "", "", XML_ATTRIBUTE.VALUE);
                this.MnemoBornier = Value;

                // MnemoHardware
                Value = XProcess.GetValue("MnemoHardware", "", "", XML_ATTRIBUTE.VALUE);
                this.MnemoHardware = Value;

                // MnemoClient
                Value = XProcess.GetValue("MnemoHardware", "", "", XML_ATTRIBUTE.PLAGE_VALEUR);
                if (Value == "Plage_MnemoHardware")
                {
                    Value = this.MnemoHardware;
                }
                this.MnemoClient = Value;

                // MnemoLogique
                Value = XProcess.GetValue("MnemoLogique", "", "", XML_ATTRIBUTE.VALUE);
                if (Value == OLD_RELAIS_MARCHE)
                {
                    Value = NEW_RELAIS_MARCHE;
                }
                this.MnemoLogique = Value; 
            }

            // -------------  Section Pilotage  ---------------

            if (Pilotage != null)
            {
                XProcess.OpenXML(Pilotage);
                // IndiceES
                Value = XProcess.GetValue("IndiceES", "", "", XML_ATTRIBUTE.VALUE);
                if (Value != "")
                {
                    this.IndiceES = Convert.ToInt32(Value);
                }

                // IndiceFamille
                Value = XProcess.GetValue("IndiceFamille", "", "", XML_ATTRIBUTE.VALUE);
                if (Value != "")
                {
                    this.IndiceFamille = Convert.ToInt32(Value);
                }

                // Carte
                Value = XProcess.GetValue("Carte", "", "", XML_ATTRIBUTE.VALUE);
                if (Value != "")
                {
                    this.Carte = Convert.ToInt32(Value);
                }

                // Voie
                Value = XProcess.GetValue("Voie", "", "", XML_ATTRIBUTE.VALUE);
                if (Value != "")
                {
                    this.Voie = Convert.ToInt32(Value);
                } 
            }

            // -------------  Section en sécurité  ---------------
            if (ReglageSecurite != null)
            {
                XProcess.OpenXML(ReglageSecurite);

                // Valeur initiale
                this.ValeurInitiale = XProcess.GetValue("ValeurInitiale", "", "", XML_ATTRIBUTE.VALUE);

                // Valeur en sécurité
                Value = XProcess.GetValue("ModifierEnSecurite", "", "", XML_ATTRIBUTE.VALUE);
                if (Value != null && Value == "1")
                {
                    this.ValeurEnSecurite = XProcess.GetValue("ValeurEnSecurite", "", "", XML_ATTRIBUTE.VALUE);
                }
                else
                {
                    this.ValeurEnSecurite = "";
                }
            }
            else
            {
                this.ValeurInitiale = "0";
                this.ValeurEnSecurite = "0";
            }
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: ES
}
