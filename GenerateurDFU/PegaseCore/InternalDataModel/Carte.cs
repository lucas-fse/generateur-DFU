using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using JAY.XMLCore;
using Mvvm = GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace JAY.PegaseCore
{
    public enum typeCarte
    {
        Absent=-1,
        Absent0=0,
        Carte_Mere=1,
        Carte_Relais=2,
        Carte_SanaByPass=3,
        Carte_EtorEana=4,
        Carte_Sstatique_1=5,
        Carte_Statique_2=6,
        Carte_Bus=7
    }
    /// <summary>
    /// Les données des différentes cartes
    /// </summary>
    public class Carte : Mvvm.ViewModelBase
    {
        #region Constantes
        #endregion

        // Variables
        #region Variables

        private Int32 _id;
        private String _nom;
        private String _emplacement;
        private String _description;
        private Boolean _isInstalled;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// L'ID de la carte
        /// </summary>
        public Int32 ID
        {
            get
            {
                return this._id;
            }
            private set
            {
                this._id = value;
            }
        } // endProperty: ID

        /// <summary>
        /// Le nom de la carte
        /// </summary>
        public String Nom
        {
            get
            {
                return this._nom;   
            }
            private set
            {
                this._nom = value;
            }
        } // endProperty: NomCarte

        public String NomTraduit
        {
            get
            {
                return "XML_Config/" + this._nom;   
            }

        } // endProperty: NomCarte
        /// <summary>
        /// Emplacement de la carte
        /// </summary>
        public String Emplacement
        {
            get
            {
                return this._emplacement;
            }
            private set
            {
                this._emplacement = value;
            }
        } // endProperty: Emplacement

        /// <summary>
        /// La description de la carte
        /// </summary>
        public String Description
        {
            get
            {
                return this._description;
            }
            private set
            {
                this._description = value;
            }
        } // endProperty: Description

        /// <summary>
        /// La carte décrite est-elle installée?
        /// </summary>
        public Boolean IsInstalled
        {
            get
            {
                return this._isInstalled;
            }
            private set
            {
                this._isInstalled = value;
            }
        } // endProperty: IsInstalled

        #endregion

        // Constructeur
        #region Constructeur

        public Carte(XElement DescriptionCarte)
        {
            XMLProcessing XProcess = new XMLProcessing();
            XProcess.OpenXML(DescriptionCarte);
            String Value;

            // IdCarte
            Value = XProcess.GetValue("IdCarte", "", "", XML_ATTRIBUTE.VALUE);
            if (Value != "")
            {
                this.ID = Convert.ToInt32(Value);
            }

            // Nom de la carte
            this.Nom = XProcess.GetValue("NomCarte", "", "", XML_ATTRIBUTE.VALUE);
            // recherhce de la traduction des cartes

            // Emplacement de la carte
            this.Emplacement = XProcess.GetValue("EmplacementCarte", "", "", XML_ATTRIBUTE.VALUE);

            // Description de la carte
            this.Description = XProcess.GetValue("DescriptionCarte", "", "", XML_ATTRIBUTE.VALUE);

            // Présence de la carte
            Value = XProcess.GetValue("PresenceCarte", "", "", XML_ATTRIBUTE.VALUE);
            if (Value != null && Value.Trim() == "1")
            {
                this.IsInstalled = true;
            }
            else
            {
                this.IsInstalled = false;
            }
            Messenger.Default.Register<CommandMessage>(this, ReceiveMessage);
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Fermer les données proprement
        /// </summary>
        public void Dispose ( )
        {
            
        } // endMethod: Dispose
        /// <summary>
        /// Le message reçu
        /// </summary>
        public void ReceiveMessage(CommandMessage message)
        {
            // Faut-il mettre à jour la langue ?
            if (message.Command == PegaseCore.Commands.CMD_MAJ_LANGUAGE)
            {
                RaisePropertyChanged("FolderName");
                RaisePropertyChanged("LabelValInitiale");
                RaisePropertyChanged("LabelValSecurite");
                RaisePropertyChanged("LabelCyclicRatio");
                RaisePropertyChanged("LabelFrequence");
                RaisePropertyChanged("NomTraduit");
            }

            // Faut-il mettre à jour la visibilité du paramètre en cours d'édition

        } // endMethod: ReceiveMessage
        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: Carte
}
