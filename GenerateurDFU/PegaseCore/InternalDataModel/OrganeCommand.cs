using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mvvm = GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Les organes de commandes
    /// </summary>
    public class OrganCommand : Mvvm.ViewModelBase
    {

        public enum TYPE_PLD
        {
            None,
            SingleSafety,
            DoubleSafety
        }
    
    // Variables
    #region Variables

    private String _modeUsed;
        private String _nomOrganeMO;
        private Int32 _IndiceOrganeMO;
        private String _mnemoHardOrganeMO;
        private String _mnemoClient;
        private String _mnemologique;
        private String _mnemoHardFamilleMO;
        private Int32 _nbPosOrgane;
        private String _referenceOrgan;
        private Int32 _orientationOrgane;
        private Boolean _verrouillageEnCroix;
        private String _descriptionOrgan;
        
        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le nombre de cran de l'organe (axe à cran)
        /// </summary>
        public Int32 NbCran
        {
            get
            {
                Int32 Result = 0;

                if (this.MnemoHardFamilleMO == "AC")
                {
                    Result = (this.NbPosOrgane - 4) / 2;
                }

                return Result;
            }
        } // endProperty: NbCran

        public Int32 Position { get; set; }

        /// <summary>
        /// Le verrrouillage en croix pour le joystick
        /// </summary>
        public Boolean VerrouillageEnCroix
        {
            get
            {
                return this._verrouillageEnCroix;
            }
            set
            {
                this._verrouillageEnCroix = value;
                RaisePropertyChanged("VerrouillageEnCroix");
            }
        } // endProperty: VerrouillageEnCroix

        /// <summary>
        /// L'orientation de l'organe
        /// </summary>
        public Int32 OrientationOrgane
        {
            get
            {
                return this._orientationOrgane;
            }
            set
            {
                this._orientationOrgane = value;
                RaisePropertyChanged("OrientationOrgane");
            }
        } // endProperty: OrientationOrgane


        public TYPE_PLD TypePld
        {
            get;set;

        }
        /// <summary>
        /// retourner la description de l'organe
        /// </summary>
        public String DescriptionOrgan
        {
            get
            {
                if (this.ReferenceOrgan != null && this.ReferenceOrgan != "")
                {
                    this._descriptionOrgan = LanguageSupport.Get().GetToolTip("/CHECK_MATERIEL/" + this.ReferenceOrgan);
                }

                return this._descriptionOrgan;
            }
            set
            {
                this._descriptionOrgan = value;
            }
        } // endProperty: DescriptionOrgan

        /// <summary>
        /// La référence de l'organe
        /// </summary>
        public String ReferenceOrgan
        {
            get
            {
                return this._referenceOrgan;
            }
            set
            {
                if (value != null)
                {
                    this._referenceOrgan = value;
                    RaisePropertyChanged("ReferenceOrgan");
                    RaisePropertyChanged("DescriptionOrgan"); 
                }
            }
        } // endProperty: ReferenceOrgan
        

        /// <summary>
        /// Le mode d'utilisation de se contrôle (mode ou all mode)
        /// </summary>
        public String ModeUsed
        {
            get
            {
                return this._modeUsed;
            }
            set
            {
                this._modeUsed = value;
            }
        } // endProperty: ModeUsed

        /// <summary>
        /// Le libellé de l'état
        /// </summary>
        public String LibelEtat
        {
            get
            {
                return "/CHECK_MATERIEL/LIBEL_ETAT";
            }
        } // endProperty: LibelEtat

        /// <summary>
        /// Le nombre de position de l'organe
        /// </summary>
        public Int32 NbPosOrgane
        {
            get
            {
                return this._nbPosOrgane;
            }
            set
            {
                if (this.MnemoHardFamilleMO == "BT")
                {
                    if (value == 2 || value == 3)
                    {
                        this._nbPosOrgane = value;
                    }
                }
                else if(this.MnemoHardFamilleMO == "AC")
                {
                    //this._nbPosOrgane = 13;
                    if (value == 6)
                    {
                        this._nbPosOrgane = value;
                    }
                    else
                    {
                        if (value % 2 == 0)
                        {
                            this._nbPosOrgane = value + 1;
                        }
                        else
                        {
                            this._nbPosOrgane = value;
                        }
                    }
                }
                else if (this._mnemoHardFamilleMO == "CO")
                {
                    if (this.Mnemologique.Contains("COMMUTATEUR_12"))
                    {
                        if (value > 0 && value <= 12)
                        {
                            this._nbPosOrgane = value;
                        }
                    }
                    else
                    {
                        if (value == 2 || value == 3)
                        {
                            this._nbPosOrgane = value;
                        }
                    }
                }
                else
                {
                    this._nbPosOrgane = value;
                }
                RaisePropertyChanged("NbPosOrgane");
                RaisePropertyChanged("LibelPosition");
            }
        } // endProperty: NbPosOrgane

        /// <summary>
        /// Le libellé du nombre de position
        /// </summary>
        public String LibelPosition
        {
            get
            {
                String Result;

                if (this.MnemoHardFamilleMO == "AC")
                {
                    Int32 pos = (this.NbPosOrgane - 4) / 2;
                    if (this.NbPosOrgane > 6)
                    {
                        if (this.NbPosOrgane < 14)
                        {
                            Result = String.Format("+{0}/-{0}", pos);  
                        }
                        else
                        {
                            Result = LanguageSupport.Get().GetText("CHECK_MATERIEL/SANS_CRAN");
                        }
                    }
                    else
	                {
                        Result = LanguageSupport.Get().GetText("CHECK_MATERIEL/AXE_VEROUILLER");
	                }
                }
                else
                {
                    Result = String.Format("{0} pos", this.NbPosOrgane);
                }

                return Result;
            }
        } // endProperty: LibelPosition

        /// <summary>
        /// Le nom de l'organe
        /// </summary>
        public String NomOrganeMO
        {
            get
            {
                return this._nomOrganeMO;
            }
            set
            {
                this._nomOrganeMO = value;
                if (this._nomOrganeMO.Trim() == "M")
                {
                    this.Mnemologique = "BOUTON_13";
                }
            }
        } // endProperty: NomOrganeMO

        /// <summary>
        /// L'indice de l'organe
        /// </summary>
        public Int32 IndiceOrganeMO
        {
            get
            {
                return this._IndiceOrganeMO;
            }
            set
            {
                this._IndiceOrganeMO = value;
            }
        } // endProperty: IndiceOrganeMO

        public String ZoneOrganeMO
        {
            get;
            
            set;
        } // endProperty: IndiceOrganeMO

        /// <summary>
        /// Le mnémonique de l'organe du MO
        /// </summary>
        public String MnemoHardOrganeMO
        {
            get
            {
                return this._mnemoHardOrganeMO;
            }
            set
            {
                this._mnemoHardOrganeMO = value;
            }
        } // endProperty: MnemoHardOrganeMO

        /// <summary>
        /// Mnémonique client pour l'organe
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
        /// La mnemologique de l'organe
        /// </summary>
        public String Mnemologique
        {
            get
            {
                return this._mnemologique;
            }
            set
            {
                this._mnemologique = value;
            }
        } // endProperty: Mnemologique

        /// <summary>
        /// La mnémologique hard de la famille MO
        /// </summary>
        public String MnemoHardFamilleMO
        {
            get
            {
                return this._mnemoHardFamilleMO;
            }
            set
            {
                this._mnemoHardFamilleMO = value;
            }
        } // endProperty: MnemoHardFamilleMO

        #endregion

        // Constructeur
        #region Constructeur

        public OrganCommand()
        {
            // Valeurs par défaut utilisées lors de la mise en forme
            this.MnemoHardFamilleMO = "BT";
            this.MnemoHardOrganeMO = "F1";
            this.MnemoClient = "F1";
            this.Mnemologique = "B1";
            this.NomOrganeMO = "F1";
            this.NbPosOrgane = 3;
            this.IndiceOrganeMO = 1;
            this.Position = 0;
            // Abonnement aux messages
            Messenger.Default.Register<CommandMessage>(this, ReceiveMessage);
        }

        #endregion

        // Méthodes
        #region Méthodes

        public void ReceiveMessage(CommandMessage message)
        {
            if (message.Sender == null)
            {
                switch (message.Command)
                {
                    case PegaseCore.Commands.CMD_MAJ_LANGUAGE:
                        RaisePropertyChanged("LibelEtat");
                        RaisePropertyChanged("DescriptionOrgan");
                        RaisePropertyChanged("LibelPosition");
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: OrganCommand
}

