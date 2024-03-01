using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Mvvm = GalaSoft.MvvmLight;
using JAY.PegaseCore;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Classe permettant de présenter les données d'inter-verrouillage
    /// </summary>
    public class EC_Verrou : Mvvm.ViewModelBase
    {
        // Variables
        #region Variables

        private Boolean _isUsed;
        private String _name;
        private ObservableCollection<EC_Output> _outputs;
        private Int32 _lineNumber;
        private String _mnemologique;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le numéro de la ligne -> utilisé pour alterner les couleurs
        /// </summary>
        public Int32 LineNumber
        {
            get
            {
                return this._lineNumber;
            }
            set
            {
                this._lineNumber = value;
                RaisePropertyChanged("LineNumber");
            }
        } // endProperty: LineNumber

        /// <summary>
        /// Si la ligne est utilisée afficher les contrôles liés, sinon les cacher
        /// </summary>
        public Visibility OnUsedVisibility
        {
            get
            {
                Visibility Result = Visibility.Hidden;

                if (this.IsUsed)
                {
                    Result = Visibility.Visible;
                }

                return Result;
            }
        } // endProperty: OnUsedVisibility

        /// <summary>
        /// Si la ligne est non utilisée afficher les contrôles liés, sinon les cacher
        /// </summary>
        public Visibility OnNotUsedVisibility
        {
            get
            {
                Visibility Result = Visibility.Hidden;

                if (!this.IsUsed)
                {
                    Result = Visibility.Visible;
                }

                return Result;
            }
        } // endProperty: OnNotUsedVisibility

        /// <summary>
        /// Le nom du produit
        /// </summary>
        public String Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
                RaisePropertyChanged("Name");
            }
        } // endProperty: Name
        /// <summary>
        /// Le nom du produit
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
                RaisePropertyChanged("Mnemologique");
            }
        } // endProperty: Name

        /// <summary>
        /// La donnée est-elle utilisée
        /// </summary>
        public Boolean IsUsed
        {
            get
            {
                return this._isUsed;
            }
            set
            {
                this._isUsed = value;
                RaisePropertyChanged("IsUsed");
                RaisePropertyChanged("OnUsedVisibility");
                RaisePropertyChanged("OnNotUsedVisibility");
            }
        } // endProperty: IsUsed

        #endregion

        // Constructeur
        #region Constructeur

        public EC_Verrou(Boolean IsSTOR)
        {
            if (!this.IsInDesignMode)
            {
                this.CreateCommandNewLine();
                this.CreateCommandDeleteVerrou();

                if (IsSTOR)
                {
                    this.InitOutput();  
                }
                else
                {
                    this.InitOutputBtn();
                }
            }
        }

        #endregion

        // Méthodes
        #region Méthodes

        /// <summary>
        /// Sauver le résultat des sorties pour chacune des positions
        /// </summary>
        public XElement Save(XElement node)
        {
            XElement Result = node;

            foreach (var output in this.Outputs)
            {
                XElement XOutput = new XElement("Output");
                // identification de la sortie
                XAttribute XAttrib;

                XAttrib = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "Output");
                XOutput.Add(XAttrib);
                if (output.STOR != null)
                {
                    XAttrib = new XAttribute("MnemoHardware", output.STOR.MnemoHardware);
                    XOutput.Add(XAttrib);
                }
                // La sortie est-t-elle sur on pour cette position?
                XAttrib = new XAttribute("IsCheck", output.IsUsed);
                XOutput.Add(XAttrib);
                // Sauve le noeud
                Result.Add(XOutput);
            }

            return Result;
        } // endMethod: Save

        /// <summary>
        /// Sauver le résultat des sorties pour chacune des positions
        /// </summary>
        public XElement SaveBtn(XElement node)
        {
            XElement Result = node;

            foreach (var output in this.Outputs)
            {
                XElement XOutput = new XElement("Output");
                // identification de la sortie
                XAttribute XAttrib;

                XAttrib = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "Output");
                XOutput.Add(XAttrib);
                if (output.Organ != null)
                {
                    XAttrib = new XAttribute("MnemoHardware", output.Organ.Mnemologique);
                    XOutput.Add(XAttrib);
                }
                // La sortie est-t-elle sur on pour cette position?
                XAttrib = new XAttribute("IsCheck", output.IsUsed);
                XOutput.Add(XAttrib);
                // Sauve le noeud
                Result.Add(XOutput);
            }

            return Result;
        } // endMethod: SaveBtn

        /// <summary>
        /// Initialiser les sorties accessibles
        /// </summary>
        public void InitOutput()
        {
            ObservableCollection<EC_Output> outputs = new ObservableCollection<EC_Output>();

            foreach (var output in PegaseData.Instance.ModuleT.STORS)
            {
                EC_Output ec_output = new EC_Output(output);
                outputs.Add(ec_output);
            }

            this.Outputs = outputs;
        } // endMethod: InitOutput

        /// <summary>
        /// Initialiser les sorties accessibles
        /// </summary>
        public void InitOutputBtn()
        {
            ObservableCollection<EC_Output> outputs = new ObservableCollection<EC_Output>();

            foreach (var organ in PegaseData.Instance.MOperateur.OrganesCommandes)
            {
                EC_Output ec_output = new EC_Output(organ);
                outputs.Add(ec_output);
            }

            this.Outputs = outputs;
        } // endMethod: InitOutput

        /// <summary>
        /// Les relais / sorties disponiblent pouvant être affectés par cette sortie
        /// </summary>
        public ObservableCollection<EC_Output> Outputs
        {
            get
            {
                return this._outputs;
            }
            set
            {
                this._outputs = value;
                RaisePropertyChanged("Outputs");
            }
        } // endProperty: Outputs

        /// <summary>
        /// Chargement des données du noeud
        /// </summary>
        public void Load(XElement node)
        {
            foreach (var item in node.Descendants("Output"))
            {
                String IsCheck = item.Attribute("IsCheck").Value;

                if (item.Attribute("MnemoHardware") != null)
                {
                    String MnemoHardware = item.Attribute("MnemoHardware").Value;

                    var Query = from output in this.Outputs
                                where output.STOR != null && output.STOR.MnemoHardware == MnemoHardware
                                select output;

                    if (Query.Count() > 0)
                    {
                        switch (IsCheck)
                        {
                            case "true":
                                Query.First().SetIsUsed(true);
                                break;
                            case "false":
                                Query.First().SetIsUsed(false);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        } // endMethod: Load

        /// <summary>
        /// Chargement des données du noeud
        /// </summary>
        public void LoadBtn(XElement node)
        {
            foreach (var item in node.Descendants("Output"))
            {
                String IsCheck = item.Attribute("IsCheck").Value;

                if (item.Attribute("MnemoHardware") != null)
                {
                    String MnemoHardware = item.Attribute("MnemoHardware").Value;

                    var Query = from output in this.Outputs
                                where output.Organ != null && output.Organ.Mnemologique == MnemoHardware
                                select output;

                    if (Query.Count() > 0)
                    {
                        switch (IsCheck)
                        {
                            case "true":
                                Query.First().SetIsUsed(true);
                                break;
                            case "false":
                                Query.First().SetIsUsed(false);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        } // endMethod: LoadBtn

        /// <summary>
        /// Au moins une sortie est-elle verrouillée? (True) 
        /// </summary>
        public Boolean IsOneOutputChecked ( )
        {
            Boolean Result = false;

            for (int i = 0; i < this.Outputs.Count; i++)
            {
                if (this.Outputs[i].IsUsed)
                {
                    Result = true;
                    break;
                }
            }
            
            return Result;
        } // endMethod: IsOneOutputChecked

        #endregion

        // Messages
        #region Messages

        #endregion

        // Commandes
        #region Commandes

        #region CommandNewLine
        /// <summary>
        /// La commande NewLine
        /// </summary>
        public ICommand CommandNewLine
        {
            get;
            internal set;
        } // endProperty: CommandNewLine

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandNewLine()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandNewLine = new Mvvm.Command.RelayCommand(ExecuteCommandNewLine, CanExecuteCommandNewLine);
        } // endMethod: CreateCommandNewLine

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandNewLine()
        {
            // Utiliser la ligne actuelle et envoyer un message pour la création d'une ligne supplémentaire
            this.IsUsed = true;
            Mvvm.Messaging.Messenger.Default.Send<CommandMessage>(new CommandMessage(this, Commands.CMD_ADD_NEW_LINE));
        } // endMethod: ExecuteCommandNewLine

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandNewLine()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandNewLine 
        #endregion


        #region CommandDeleteVerrou
        /// <summary>
        /// La commande DeleteVerrou
        /// </summary>
        public ICommand CommandDeleteVerrou
        {
            get;
            internal set;
        } // endProperty: CommandDeleteVerrou

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandDeleteVerrou()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandDeleteVerrou = new Mvvm.Command.RelayCommand(ExecuteCommandDeleteVerrou, CanExecuteCommandDeleteVerrou);
        } // endMethod: CreateCommandDeleteVerrou

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandDeleteVerrou()
        {
            // supprimer la ligne
            Mvvm.Messaging.Messenger.Default.Send<CommandMessage>(new CommandMessage(this, Commands.CMD_DEL_LINE));
        } // endMethod: ExecuteCommandDeleteVerrou

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandDeleteVerrou()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandDeleteVerrou 
        #endregion


        #endregion

    } // endClass: EC_Verrou
}
