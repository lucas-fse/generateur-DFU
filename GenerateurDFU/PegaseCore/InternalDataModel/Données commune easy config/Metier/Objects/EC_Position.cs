using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using Mvvm = GalaSoft.MvvmLight;
using JAY.PegaseCore;

namespace JAY.PegaseCore
{
    /// <summary>
    /// 
    /// </summary>
    public class EC_Position : Mvvm.ViewModelBase
    {
        // Variables
        #region Variables

        private ObservableCollection<EC_Output> _outputs;
        private String _name;
        private Boolean _isChecked;
        private EC_Effecteur _parentEffecteur = null;
        private EC_Mode _parentMode;

        #endregion

        // Propriétés
        #region Propriétés
        
        /// <summary>
        /// Le mode parent de l'effecteur
        /// </summary>
        public EC_Mode ParentMode
        {
            get
            {
                return this._parentMode;
            }
            set
            {
                this._parentMode = value;
                RaisePropertyChanged("ParentMode");
            }
        } // endProperty: ParentMode

        /// <summary>
        /// L'effecteur parent de la position
        /// </summary>
        public EC_Effecteur ParentEffecteur
        {
            get
            {
                return this._parentEffecteur;
            }
            set
            {
                this._parentEffecteur = value;
                RaisePropertyChanged("ParentEffecteur");
            }
        } // endProperty: ParentEffecteur

        /// <summary>
        /// au moin une sortie est sur ON pour la position
        /// </summary>
        public Boolean IsCheked
        {
            get
            {
                return this._isChecked;
            }
            private set
            {
                this._isChecked = value;
                if (this.ParentEffecteur != null)
                {
                    this.ParentEffecteur.EvalIsChecked();
                }
                RaisePropertyChanged("IsCheked");
            }
        } // endProperty: IsCheked

        /// <summary>
        /// Le nom de la position
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

        #endregion

        // Constructeur
        #region Constructeur

        public EC_Position(String name, EC_Mode mode)
        {
            this.Name = name;
            if (!IsInDesignMode)
            {
                this.ParentMode = mode;
                this.InitOutput();
            }
            else
            {
                this.InitOutputDesign();
            }
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Evaluer si la position contient au moins une sortie activée
        /// </summary>
        public void EvalIsChecked ( )
        {
            Boolean Result = false;

            foreach (var output in this.Outputs)
            {
                if (output.IsUsed)
                {
                    Result = true;
                }
            }

            this.IsCheked = Result;
        } // endMethod: EvalIsChecked

        /// <summary>
        /// Sauver le résultat des sorties pour chacune des positions
        /// </summary>
        public XElement Save ( XElement node )
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
                    XAttrib = new XAttribute("MnemoHardware", output.STOR.MnemoLogique);
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
        /// Chargement des données du noeud
        /// </summary>
        public void Load ( XElement node )
        {
            foreach (var item in node.Descendants("Output"))
            {
                String MnemoLogique = item.Attribute("MnemoHardware").Value;
                String IsCheck = item.Attribute("IsCheck").Value;

                var Query = from output in this.Outputs
                            where output.STOR != null && output.STOR.MnemoLogique == MnemoLogique
                            select output;

                // Pour compatibilité avec les anciennes fiches, si pas de Mnemologique correspondant, vérifier les MnemoHardwares
                if (Query.Count() == 0)
                {
                    Query = from output in this.Outputs
                            where output.STOR != null && output.STOR.MnemoHardware == MnemoLogique
                            select output;
                }

                // La sortie est-elle utilisée ?
                if ( Query.Count() > 0)
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
        } // endMethod: Load

        /// <summary>
        /// Initialiser les sorties accessibles
        /// </summary>
        public void InitOutput( )
        {
            ObservableCollection<EC_Output> outputs = new ObservableCollection<EC_Output>();

            foreach (var output in PegaseData.Instance.ModuleT.STORS)
            {
                EC_Output ec_output = new EC_Output(output);
                ec_output.ParentPosition = this;
                ec_output.ParentMode = this.ParentMode;
                outputs.Add(ec_output);
            }

            this.Outputs = outputs;
        } // endMethod: InitOutput
        
        /// <summary>
        /// Initialiser les sorties accessible dans le cadre du design de l'interface
        /// </summary>
        public void InitOutputDesign ( )
        {
            
        } // endMethod: InitOutputDesign

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: EC_Position
}
