using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using JAY.PegaseCore;
using Mvvm = GalaSoft.MvvmLight;
using System.Windows.Media;

namespace JAY.PegaseCore
{
    public enum TypeEffecteur
    {
        physic = 0,
        selecteur = 1
    }
    /// <summary>
    /// Effecteur : class définissant le lien entre un organe de la télécommande / ou un sélecteur, les positions accessibles et les sorties (sorties / relais...).
    /// </summary>
    public class EC_Effecteur : Mvvm.ViewModelBase
    {
        // Variables
        #region Variables

        private ObservableCollection<EC_Position> _positions;
        private String _name;
        private String _idEffecteur;
        private TypeEffecteur _effecteurType;
        private OrganCommand _organ;
        private Selecteur _selecteur;
        private Boolean _isChecked;
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
        /// Au moins une sortie est mise en action grâce à cet effecteur
        /// </summary>
        public Boolean IsChecked
        {
            get
            {
                return this._isChecked;
            }
            private set
            {
                this._isChecked = value;
                RaisePropertyChanged("IsChecked");
                RaisePropertyChanged("ContextualColor");
            }
        } // endProperty: IsChecked

        /// <summary>
        /// Couleur contextuel montrant si l'effecteur contient des sorties utilisées
        /// </summary>
        public SolidColorBrush ContextualColor
        {
            get
            {
                SolidColorBrush Result;

                if (this.IsChecked)
                {
                    Result = new SolidColorBrush(Color.FromArgb(0x6C, 0x6A, 0x85, 0x44));
                }
                else
                {
                    Result = new SolidColorBrush(Color.FromArgb(0, 200, 255, 150));
                }

                return Result;
            }
        } // endProperty: ContextualColor

        /// <summary>
        /// L'organe de commande lié à cet effecteur
        /// La donnée est en lecture seule
        /// </summary>
        public OrganCommand Organ
        {
            get
            {
                return this._organ;
            }
        } // endProperty: Organ

        /// <summary>
        /// L'Id de l'effecteur permettant de l'identifier précisement
        /// </summary>
        public String IdEffecteur
        {
            get
            {
                return this._idEffecteur;
            }
            set
            {
                this._idEffecteur = value;
                RaisePropertyChanged("IdEffecteur");
            }
        } // endProperty: IdEffecteur

        /// <summary>
        /// La liste des positions disponibles pour cet effecteur
        /// </summary>
        public ObservableCollection<EC_Position> Positions
        {
            get
            {
                return this._positions;
            }
            set
            {
                this._positions = value;
                RaisePropertyChanged("Positions");
            }
        } // endProperty: Positions

        /// <summary>
        /// Le nom de l'effecteur
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
        /// Le type de l'effecteur (physic = bouton / combo...) ou selecteur
        /// </summary>
        public TypeEffecteur EffecteurType
        {
            get
            {
                return this._effecteurType;
            }
            set
            {
                this._effecteurType = value;
                RaisePropertyChanged("EffecteurType");
            }
        } // endProperty: EffecteurType

        #endregion

        // Constructeur
        #region Constructeur

        // Initialiser la classe à partir d'un selecteur
        public EC_Effecteur(Selecteur selecteur, String name, ObservableCollection<String> libelPosition, EC_Mode mode)
        {
            this.ParentMode = mode;
            this._selecteur = selecteur;
            this.Name = name;
            this.IdEffecteur = this._selecteur.IdentSelecteur;
            this.EffecteurType = TypeEffecteur.selecteur;
            this.InitPositions(this._selecteur.NbPosition, libelPosition);
        }

        // Initialiser la classe à partir d'un organe
        public EC_Effecteur(OrganCommand organ, ObservableCollection<String> libelPosition, EC_Mode mode)
        {
            this.ParentMode = mode;
            this._organ = organ;
            if (this._organ.NomOrganeMO != this._organ.MnemoClient)
            {
                this.Name = String.Format("{0} : {1}", this._organ.NomOrganeMO, this._organ.MnemoClient); 
            }
            else
            {
                this.Name = String.Format("{0}", this._organ.NomOrganeMO);
            }
            this.IdEffecteur = this._organ.NomOrganeMO;
            this.EffecteurType = TypeEffecteur.physic;
            this.InitPositions(this._organ.NbPosOrgane, libelPosition);
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Evaluer si au moins une sortie est pilotée par cet effecteur
        /// </summary>
        public void EvalIsChecked ( )
        {
            Boolean Result = false;

            foreach (var position in this.Positions)
            {
                if (position.IsCheked)
                {
                    Result = true;
                    break;
                }
            }

            this.IsChecked = Result;
        } // endMethod: EvalIsChecked

        /// <summary>
        /// Sauvegarder l'ensemble des données significatives du noeud
        /// </summary>
        public XElement Save ( XElement node )
        {
            XElement Result = node;
            Int32 i = 0;

            foreach (var position in this.Positions)
            {
                // Créer la balise position
                XElement XPosition = new XElement("Position");
                XAttribute XAttrib = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "Position");
                XPosition.Add(XAttrib);

                if (this.EffecteurType == TypeEffecteur.physic)
                {
                    XAttrib = new XAttribute("Name", position.Name);
                }
                else
                {
                    XAttrib = new XAttribute("Name", String.Format("Pos{0:00}", i));
                    i++;
                }
                XPosition.Add(XAttrib);

                // Sauver le résultat de l'état des sorties
                XPosition = position.Save(XPosition);

                // Ajouter la nouvelle balise au résultat
                Result.Add(XPosition);
            }

            return Result;
        } // endMethod: Save

        /// <summary>
        /// Charger les données du noeud
        /// </summary>
        public void Load ( XElement node )
        {
            Int32 i = 0;

            foreach (XElement item in node.Descendants("Position"))
            {
                String Name = item.Attribute("Name").Value;

                if (this.Organ != null && this.Organ.MnemoHardFamilleMO == "AC")
                {
                    var Query = from position in this.Positions
                                where position.Name == Name
                                select position;

                    if (Query.Count() > 0)
                    {
                        Query.First().Load(item);
                    } 
                }
                else
                {
                    if (this.Positions != null)
                    {
                        if (i < this.Positions.Count)
                        {
                            this.Positions[i].Load(item);
                            i++;
                        }
                    }
                }
            }
        } // endMethod: Load

        /// <summary>
        /// Initialiser le nombre de positions possible pour l'organe / le sélecteur
        /// </summary>
        public void InitPositions ( Int32 nbPositions, ObservableCollection<String> libelPosition )
        {
            ObservableCollection<EC_Position> positions = new ObservableCollection<EC_Position>();

            for (int i = 0; i < nbPositions; i++)
            {
                String libel = "";

                //if (libelPosition != null)
                //{
                //    if (i < libelPosition.Count)
                //    {
                //        if (libelPosition[i] != null && libelPosition[i] != Constantes.DIRECT_TO_BMP)
                //        {
                //            libel = libelPosition[i];
                //        }
                //    }
                //}

                if (libel == "")
                {
                    if (this.EffecteurType == TypeEffecteur.physic && this.Organ.MnemoHardFamilleMO == "CO" && nbPositions == 2)
                    {
                        libel = String.Format("Pos{0:00}", i + 1);
                    }
                    else if (this.EffecteurType == TypeEffecteur.physic && this.Organ.MnemoHardFamilleMO == "AC" )
                    {
                        if (i > 1 && i < nbPositions - 2)
                        {
                            if (i - (nbPositions / 2) < 0)
                            {
                                libel = String.Format("Step {0:00}", i - (nbPositions / 2));
                            }
                            else if (i - (nbPositions / 2) > 0)
                            {
                                libel = String.Format("Step +{0:00}", i - (nbPositions / 2));
                            }
                            else
                            {
                                libel = String.Format("Step   0");
                            } 
                        }
                    }
                    else
                    {
                        libel = String.Format("Pos{0:00}", i );
                    }
                }

                if (libel != "")
                {
                    EC_Position position = new EC_Position(libel, this.ParentMode);
                    position.ParentEffecteur = this;
                    positions.Add(position);
                }
            }

            this.Positions = positions;
        } // endMethod: InitPositions

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: EC_Effecteur
}
