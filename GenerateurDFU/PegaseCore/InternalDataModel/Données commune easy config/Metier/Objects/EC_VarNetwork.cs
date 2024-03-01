using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Mvvm = GalaSoft.MvvmLight;
using JAY.PegaseCore;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Reflection;

namespace JAY.PegaseCore
{
    public enum typeVarNetwork
    {
        IBoolNetwork,
        OBoolNetwork,
        INumNetwork,
        ONumNetwork,
        DataSystem
    }

    /// <summary>
    /// Description d'une variable réseau
    /// </summary>
    public class EC_VarNetwork : Mvvm.ViewModelBase
    {
        // Variables
        #region Variables

        private typeVarNetwork _type;
        private String _adresse;
        private String _variableNetwork;
        private ObservableCollection<VariableE> _listChoixVariable;
        private VariableE _currentVariable;
        private String _adrMODBus;
        private String _adrCANSDO;
        private String _adrCANRDPO;
        private String _adrMODBus_offset;
        private String _adrCANSDO_offset;
        private String _adrCANRDPO_offset;
        private String _adrProfibus;
        private String _adrEthercat;
        private String _adrDevicenet;
        private String _adrEthernet;
        private String _adrPowerlink;
        private String _typevariable;
        private String _name;
        private String _mnemoHardFamilleMO;
        private ListChoixVariableImage _currentVariableImage;


        #endregion

        #region Propriétés

        public String Colonne1R
        {
            get
            {
                return "Type : ";
            }
        }
        public String Colonne2R
        {
            get
            {
                return "@rel : ";
            }
        }

        public String Colonne3R
        {
            get
            {
                return "@abs : ";
            }
        }
        public String Colonne1W
        {
            get
            {
                return "Type : ";
            }
        }
        public String Colonne2W
        {
            get
            {
                return "@rel : ";
            }
        }
        public String Colonne3W
        {
            get
            {
                return "@abs : ";
            }
        }
        public String AdrReseau1R
        {
            get
            {
                        return TypeVariable;

            }
        }
        public String AdrReseau2R
        {
            get
            {
                switch (EasyConfigData.Get().TypeBusEasyConfig)
                {
                    case TypeBusEasyConfig.MODBUS:
                        return AdrMODBus;
                        break;
                    case TypeBusEasyConfig.CANOPEN:
                        return AdrCANRPDO + "\n" + AdrCANSDO;
                        break;
                    case TypeBusEasyConfig.ETHERCAT:
                    case TypeBusEasyConfig.POWERLINK:
                        return AdrEthercat;
                        break;
                    case TypeBusEasyConfig.PROFIBUSV1:
                    case TypeBusEasyConfig.PROFINET:
                        return AdrProfibus;
                        break;
                    default:
                        return "Not Defined";
                        break;
                }
            }
        }
        // utilisé pour le rapport
        public String AdrReseau2R_bis
        {
            get
            {
                switch (EasyConfigData.Get().TypeBusEasyConfig)
                {
                    case TypeBusEasyConfig.MODBUS:
                        return AdrMODBus_bis;
                        break;
                    case TypeBusEasyConfig.CANOPEN:
                        return AdrCANRPDO_bis + "\n" + AdrCANSDO_bis;
                        break;
                    case TypeBusEasyConfig.ETHERCAT:
                    case TypeBusEasyConfig.POWERLINK:
                        return AdrEthercat;
                        break;
                    case TypeBusEasyConfig.PROFIBUSV1:
                    case TypeBusEasyConfig.PROFINET:
                        return AdrProfibus;
                        break;
                    default:
                        return "Not Defined";
                        break;
                }
            }
        }
        public String AdrReseau3R
        {
            get
            { //DEVICENET ETHERNET_IP CCLINK
                switch (EasyConfigData.Get().TypeBusEasyConfig)
                {
                    case TypeBusEasyConfig.MODBUS:
                        return AdrMODBusOffset;
                        break;
                    case TypeBusEasyConfig.CANOPEN:
                        return AdrCANRPDOOffset + "\n" + AdrCANSDOOffset;
                        break;
                    case TypeBusEasyConfig.ETHERCAT:
                    case TypeBusEasyConfig.POWERLINK:
                        return AdrEthercat;
                        break;
                    case TypeBusEasyConfig.PROFIBUSV1:
                    case TypeBusEasyConfig.PROFINET:
                        return AdrProfibus;
                        break;
                    default:
                        return "Not Defined";
                        break;
                }
            }
        }
        public String AdrReseau3R_bis
        {
            get
            { //DEVICENET ETHERNET_IP CCLINK
                switch (EasyConfigData.Get().TypeBusEasyConfig)
                {
                    case TypeBusEasyConfig.MODBUS:
                        return AdrMODBusOffset;
                        break;
                    case TypeBusEasyConfig.CANOPEN:
                        return AdrCANRPDOOffset_bis + "\n" + AdrCANSDOOffset_bis;
                        break;
                    case TypeBusEasyConfig.ETHERCAT:
                    case TypeBusEasyConfig.POWERLINK:
                        return AdrEthercat;
                        break;
                    case TypeBusEasyConfig.PROFIBUSV1:
                    case TypeBusEasyConfig.PROFINET:
                        return AdrProfibus;
                        break;
                    default:
                        return "Not Defined";
                        break;
                }
            }
        }
        public String AdrReseau1W
        {
            get
            {
                        return TypeVariable;
            }
        }
        public String AdrReseau2W
        {
            get
            {
                switch (EasyConfigData.Get().TypeBusEasyConfig)
                {
                    case TypeBusEasyConfig.MODBUS:
                        return AdrMODBus;
                        break;
                    case TypeBusEasyConfig.CANOPEN:
                        return AdrCANRPDO + "\n" + AdrCANSDO;
                        break;
                    case TypeBusEasyConfig.ETHERCAT:
                    case TypeBusEasyConfig.POWERLINK:
                        return AdrEthercat;
                        break;
                    case TypeBusEasyConfig.PROFIBUSV1:
                    case TypeBusEasyConfig.PROFINET:
                        return AdrProfibus;
                        break;
                    default:
                        return "Not Defined";
                        break;
                }
            }
        }
        // utiliser pour le rapport
        public String AdrReseau2W_bis
        {
            get
            {
                switch (EasyConfigData.Get().TypeBusEasyConfig)
                {
                    case TypeBusEasyConfig.MODBUS:
                        return AdrMODBus_bis;
                        break;
                    case TypeBusEasyConfig.CANOPEN:
                        return AdrCANRPDO_bis + "\n" + AdrCANSDO_bis;
                        break;
                    case TypeBusEasyConfig.ETHERCAT:
                    case TypeBusEasyConfig.POWERLINK:
                        return AdrEthercat;
                        break;
                    case TypeBusEasyConfig.PROFIBUSV1:
                    case TypeBusEasyConfig.PROFINET:
                        return AdrProfibus;
                        break;
                    default:
                        return "Not Defined";
                        break;
                }
            }
        }
        public String AdrReseau3W
        {
            get
            {
                switch (EasyConfigData.Get().TypeBusEasyConfig)
                {
                    case TypeBusEasyConfig.MODBUS:
                        return AdrMODBusOffset;
                        break;
                    case TypeBusEasyConfig.CANOPEN:
                        return AdrCANRPDOOffset + "\n" + AdrCANSDOOffset;
                        break;
                    case TypeBusEasyConfig.ETHERCAT:
                    case TypeBusEasyConfig.POWERLINK:
                        return AdrEthercat;
                    case TypeBusEasyConfig.PROFIBUSV1:
                    case TypeBusEasyConfig.PROFINET:
                        return AdrProfibus;
                        break;
                        break;
                    default:
                        return "Not Defined";
                        break;
                }
            }
        }
        public String AdrReseau3W_bis
        {
            get
            {
                switch (EasyConfigData.Get().TypeBusEasyConfig)
                {
                    case TypeBusEasyConfig.MODBUS:
                        return AdrMODBusOffset;
                        break;
                    case TypeBusEasyConfig.CANOPEN:
                        return AdrCANRPDOOffset_bis + "\n" + AdrCANSDOOffset_bis;
                        break;
                    case TypeBusEasyConfig.ETHERCAT:
                    case TypeBusEasyConfig.POWERLINK:
                        return AdrEthercat;
                    case TypeBusEasyConfig.PROFIBUSV1:
                    case TypeBusEasyConfig.PROFINET:
                        return AdrProfibus;
                        break;
                        break;
                    default:
                        return "Not Defined";
                        break;
                }
            }
        }
        public String TypeVariable
        {
            get
            {
                return this._typevariable;
            }
            set
            {
                this._typevariable = value;
                RaisePropertyChanged("AdrCANRPDO");
            }
        }

        /// <summary>
        /// L'adresse de la variable sur le _adrEthercat
        /// </summary>
        public String AdrPowerlink
        {
            get
            {
                return this._adrPowerlink;
            }
            set
            {
                this._adrPowerlink = value;
                RaisePropertyChanged("AdrCANRPDO");
            }
        } // endProperty: _adrEthercat

        /// <summary>
        /// L'adresse de la variable sur le _adrEthercat
        /// </summary>
        public String AdrEthernet
        {
            get
            {
                return this._adrEthernet;
            }
            set
            {
                this._adrEthernet = value;
                RaisePropertyChanged("AdrCANRPDO");
            }
        } // endProperty: _adrEthercat
        
        /// <summary>
        /// L'adresse de la variable sur le _adrEthercat
        /// </summary>
        public String AdrDevicenet
        {
            get
            {
                return this._adrDevicenet;
            }
            set
            {
                this._adrDevicenet = value;
                RaisePropertyChanged("AdrCANRPDO");
            }
        } // endProperty: _adrEthercat

        /// <summary>
        /// L'adresse de la variable sur le _adrEthercat
        /// </summary>
        public String AdrEthercat
        {
            get
            {
                return this._adrEthercat;
            }
            set
            {
                this._adrEthercat = value;
                RaisePropertyChanged("AdrCANRPDO");
            }
        } // endProperty: _adrEthercat

        /// <summary>
        /// L'adresse de la variable sur le _adrProfibus
        /// </summary>
        public String AdrProfibus
        {
            get
            {
                return this._adrProfibus;
            }
            set
            {
                this._adrProfibus = value;
                RaisePropertyChanged("AdrCANRPDO");
            }
        } // endProperty: _adrProfibus

        /// <summary>
        /// L'adresse de la variable sur le CAN RDPO
        /// </summary>
        public String AdrCANRPDO
        {
            get
            {
                return this._adrCANRDPO;
            }
            set
            {
                this._adrCANRDPO = value;
                RaisePropertyChanged("AdrCANRPDO");
            }
        } // endProperty: AdrCANRPDO
        public String AdrCANRPDO_bis
        { get; set;
        }

        /// <summary>
        /// L'adresse de la variable sur le CAN SDO
        /// </summary>
        public String AdrCANSDO
        {
            get
            {
                return this._adrCANSDO;
            }
            set
            {
                this._adrCANSDO = value;
                RaisePropertyChanged("AdrCANSDO");
            }
        } // endProperty: AdrCANSDO
        public String AdrCANSDO_bis
        {
            get;set;
        } // endProperty: AdrCANSDO
        /// <summary>
        /// L'adresse de la variable sur le modBus
        /// </summary>
        public String AdrMODBus
        {
            get
            {
                return this._adrMODBus;
            }
            set
            {
                this._adrMODBus = value;
                RaisePropertyChanged("AdrMODBus");
            }
        } // endProperty: AdrMODBus

        public String AdrMODBus_bis
        {
            get;set;
        } // endProperty: AdrMODBus

        /// <summary>
        /// L'adresse de la variable sur le CAN RDPO
        /// </summary>
        public String AdrCANRPDOOffset
        {
            get
            {
                return this._adrCANRDPO_offset;
            }
            set
            {
                this._adrCANRDPO_offset = value;
                RaisePropertyChanged("AdrCANRPDO");
            }
        } // endProperty: AdrCANRPDO
        public String AdrCANRPDOOffset_bis
        {
            get;set;
        } // endProperty: AdrCANRPDO

        /// <summary>
        /// L'adresse de la variable sur le CAN SDO
        /// </summary>
        public String AdrCANSDOOffset
        {
            get
            {
                return this._adrCANSDO_offset;
            }
            set
            {
                this._adrCANSDO_offset = value;
                RaisePropertyChanged("AdrCANSDO");
            }
        } // endProperty: AdrCANSDO
        public String AdrCANSDOOffset_bis
        {
            get;set;
        } // endProperty: AdrCANSDO
        /// <summary>
        /// L'adresse de la variable sur le modBus
        /// </summary>
        public String AdrMODBusOffset
        {
            get
            {
                return this._adrMODBus_offset;
            }
            set
            {
                this._adrMODBus_offset = value;
                RaisePropertyChanged("AdrMODBus");
            }
        } // endProperty: AdrMODBus
        // Propriétés
        
        /// <summary>
        /// La variable en cours de liaison
        /// </summary>
        public VariableE CurrentVariable
        {
            get
            {
                return this._currentVariable;
            }
            set
            {
                this._currentVariable = value;
                RaisePropertyChanged("CurrentVariable");
            }
        } // endProperty: CurrentVariable

        /// <summary>
        /// List de variables de choix pour lié le la variable reseau et la variable interne
        /// </summary>
        public ObservableCollection<VariableE> ListChoixVariable
        {
            get
            {
                return this._listChoixVariable;
            }
            private set
            {
                this._listChoixVariable = value;
                RaisePropertyChanged("ListChoixVariable");
               
            }
        } // endProperty: ListChoixVariable

        /// <summary>
        /// Le mnemologique de la variable réseau
        /// </summary>
        public String VariableNetwork
        {
            get
            {
                return this._variableNetwork;
            }
            set
            {
                this._variableNetwork = value;
                RaisePropertyChanged("VarDataSys");
            }
        } // endProperty: VariableNetwork

        /// <summary>
        /// La variable interne liée à la variable réseau
        /// </summary>
        public String VariableInternal
        {
            get
            {
                String Result = "";

                if (this.CurrentVariable.Name != "----")
                {
                    Result = this.CurrentVariable.Name;
                }

                return Result;
            }
            set
            {
                var Query = from variable in this.ListChoixVariable
                            where variable.Name == value
                            select variable;

                if (Query.Count() > 0)
                {
                    this.CurrentVariable = Query.First();
                }

                RaisePropertyChanged("CurrentVariable");
            }
        } // endProperty: VariableInternal

        /// <summary>
        /// Le type de variable réseau
        /// </summary>
        public typeVarNetwork Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
                RaisePropertyChanged("Type");
            }
        } // endProperty: Type

        public String  TypeHard
        {
            get
            {
                return this._mnemoHardFamilleMO;
            }
            set
            {
                _mnemoHardFamilleMO = value;
            }
        }
        /// <summary>
        /// l'adresse côté réseau
        /// </summary>
        public String Adresse
        {
            get
            {
                return this._adresse;
            }
            set
            {
                this._adresse = value;
                RaisePropertyChanged("Adresse");
            }
        } // endProperty: Adresse
        /// <summary>

        public String Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name= value;
            }
        }

        #endregion

        // Constructeur
        #region Constructeur

        public EC_VarNetwork(ObservableCollection<VariableE> listChoixVariable)
        {
            foreach (VariableE item in listChoixVariable)
            {
                if (item.TypeHard.Equals("AXE"))
                {
                    Image ana = new Image();

                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri("JAY.Affichage.View.index.png", UriKind.Relative);
                    bi3.EndInit();
                    ana.Stretch = Stretch.Fill;
                    ana.Source = bi3;
                    item.Image = 1;


                }
                else if (item.TypeHard.Equals("AC"))
                {
                    Image cran = new Image();
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri("JAY.Affichage.View.index2.png", UriKind.Relative);
                    bi3.EndInit();
                    cran.Stretch = Stretch.Fill;
                    cran.Source = bi3;
                    item.Image = 2;
                }
                else
                {
                    item.Image = 0;
                }
            }
            this.ListChoixVariable = listChoixVariable;
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: EC_VarNetwork
}
