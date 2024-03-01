using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using System.Windows.Input;
using Mvvm = GalaSoft.MvvmLight;
using System.Globalization;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Class définissant l'ensemble des libellés d'informations utilisés pour 
    /// construire une ligne de retour d'info pour un mode
    /// </summary>
    public class InfoByMode : Mvvm.ViewModelBase
    {
        private const String DIRECT_TO_BMP = "Direct_To_.BMP";
        private const Int32 NB_DECIMAL_MAX = 3;

        // Variables
        private XMLCore.XMLProcessing _navRetoursInfo;
        private Information _information;
        private Boolean _isExpanded;
        private Boolean _calibrationTerrain;
        private Boolean _calibrationTheorique;
        private Single _TPtA_X;
        private Single _TptB_X;
        private Double _valMin;
        private Double _valMax;
        private InformationLabel _labelRetour;
        private InformationLabel _labelUnite;
        private InformationLabel _labelMax;
        private InformationLabel _labelMin;
        private InformationLabel[] _labelsVarNumerique;

        // Valeurs sauvegardées
        private float _olda;
        private float _oldb;
        private ParamsGainOffset _oldCurrentEANA;
        private float _oldPointA_Y;
        private float _oldPointA_X;
        private float _oldPointB_Y;
        private float _oldPointB_X;
        private float _oldPtA_X;
        private float _oldPtB_X;
        private String _oldUnit;
        private Int32 _oldNbDecimal;

        #region Langues

        /// <summary>
        /// traduction pour la fenêtre d'edition de la mise à l'échelle
        /// </summary>
        public String MO_EME_CALTER
        {
            get
            {
                return "/LIBEL_MO/MO_EME_CALTER";
            }
        } // endProperty: MO_EME_CALTER

        /// <summary>
        /// traduction pour la fenêtre d'edition de la mise à l'échelle
        /// </summary>
        public String MO_EME_CALTHEO
        {
            get
            {
                return "/LIBEL_MO/MO_EME_CALTHEO";
            }
        } // endProperty: MO_EME_CALTHEO


        /// <summary>
        /// traduction pour la fenêtre d'edition de la mise à l'échelle
        /// </summary>
        public String MO_EME_VLUE
        {
            get
            {
                return "/LIBEL_MO/MO_EME_VLUE";
            }
        } // endProperty: MO_EME_VLUE

        /// <summary>
        /// traduction pour la fenêtre d'edition de la mise à l'échelle
        /// </summary>
        public String MO_EME_VAT
        {
            get
            {
                return "/LIBEL_MO/MO_EME_VAT";
            }
        } // endProperty: MO_EME_VAT

        /// <summary>
        /// traduction pour la fenêtre d'edition de la mise à l'échelle
        /// </summary>
        public String MO_EME_POINTA
        {
            get
            {
                return "/LIBEL_MO/MO_EME_POINTA";
            }
        } // endProperty: MO_EME_POINTA

        /// <summary>
        /// traduction pour la fenêtre d'edition de la mise à l'échelle
        /// </summary>
        public String MO_EME_POINTB
        {
            get
            {
                return "/LIBEL_MO/MO_EME_POINTB";
            }
        } // endProperty: MO_EME_POINTB

        /// <summary>
        /// traduction pour la fenêtre d'edition de la mise à l'échelle
        /// </summary>
        public String MO_EME_VTHEO
        {
            get
            {
                return "/LIBEL_MO/MO_EME_VTHEO";
            }
        } // endProperty: MO_EME_VTHEO

        /// <summary>
        /// traduction pour la fenêtre d'edition de la mise à l'échelle
        /// </summary>
        public String MO_EME_VALID
        {
            get
            {
                return "/LIBEL_MO/MO_EME_VALID";
            }
        } // endProperty: MO_EME_VALID

        /// <summary>
        /// traduction pour la fenêtre d'edition de la mise à l'échelle
        /// </summary>
        public String MO_EME_SMIN
        {
            get
            {
                return "/LIBEL_MO/MO_EME_SMIN";
            }
        } // endProperty: MO_EME_SMIN

        /// <summary>
        /// traduction pour la fenêtre d'edition de la mise à l'échelle
        /// </summary>
        public String MO_EME_SMAX
        {
            get
            {
                return "/LIBEL_MO/MO_EME_SMAX";
            }
        } // endProperty: MO_EME_SMAX

        /// <summary>
        /// traduction pour la fenêtre d'edition de la mise à l'échelle
        /// </summary>
        public String MO_EME_LMIN
        {
            get
            {
                return "/LIBEL_MO/MO_EME_LMIN";
            }
        } // endProperty: MO_EME_LMIN

        /// <summary>
        /// traduction pour la fenêtre d'edition de la mise à l'échelle
        /// </summary>
        public String MO_EME_LMAX
        {
            get
            {
                return "/LIBEL_MO/MO_EME_LMAX";
            }
        } // endProperty: MO_EME_LMAX

        /// <summary>
        /// traduction pour la fenêtre d'edition de la mise à l'échelle
        /// </summary>
        public String MO_EME_UNIT
        {
            get
            {
                return "/LIBEL_MO/MO_EME_UNIT";
            }
        } // endProperty: MO_EME_UNIT

        /// <summary>
        /// traduction pour la fenêtre d'edition de la mise à l'échelle
        /// </summary>
        public String MO_EME_NBDEC
        {
            get
            {
                return "/LIBEL_MO/MO_EME_NBDEC";
            }
        } // endProperty: MO_EME_NBDEC

        /// <summary>
        /// traduction pour la fenêtre d'edition de la mise à l'échelle
        /// </summary>
        public String MO_EME_EXPERT
        {
            get
            {
                return "/LIBEL_MO/MO_EME_EXPERT";
            }
        } // endProperty: MO_EME_EXPERT

        /// <summary>
        /// traduction pour la fenêtre d'edition de la mise à l'échelle
        /// </summary>
        public String MO_EME_GAIN
        {
            get
            {
                return "/LIBEL_MO/MO_EME_GAIN";
            }
        } // endProperty: MO_EME_GAIN

        /// <summary>
        /// traduction pour la fenêtre d'edition de la mise à l'échelle
        /// </summary>
        public String MO_EME_OFFSET
        {
            get
            {
                return "/LIBEL_MO/MO_EME_OFFSET";
            }
        } // endProperty: MO_EME_OFFSET

        /// <summary>
        /// traduction pour la fenêtre d'edition de la mise à l'échelle
        /// </summary>
        public String MO_EME_LEGTHEO
        {
            get
            {
                return LanguageSupport.Get().GetText("/LIBEL_MO/MO_EME_LEGTHEO");
            }
        } // endProperty: MO_EME_LEGTHEO

        #endregion

        // Propriétés

        #region Propriétés

        /// <summary>
        /// Le commentaire lié au retour d'information
        /// </summary>
        public String CommentaireRI
        {
            get
            {
                return this.Information.CommentaireRI;
            }
            set
            {
                this.Information.CommentaireRI = value;
                RaisePropertyChanged("CommentaireRI");
            }
        } // endProperty: CommentaireRI

        /// <summary>
        /// Les libellés utilisé pour un retour d'information 'Variable Numérique'
        /// </summary>
        public InformationLabel[] LibelsVarNumerique
        {
            get
            {
                if (this._labelsVarNumerique == null)
                {
                    this._labelsVarNumerique = new InformationLabel[64];
                }

                return this._labelsVarNumerique;
            }
            private set
            {
                this._labelsVarNumerique = value;
            }
        } // endProperty: LibelsVarNumerique

        /// <summary>
        /// Le libellé Max est-il éditable ?
        /// </summary>
        public Boolean IsInfoLibelMaxEditable
        {
            get
            {
                Boolean Result = true;

                if (this.InfoLibelMax != null)
                {
                    if (this.InfoLibelMax.LibelInformation == DIRECT_TO_BMP)
                    {
                        Result = false;
                    }
                }
                return Result;
            }
        } // endProperty: IsEditable

        /// <summary>
        /// Le libellé Min est-il éditable
        /// </summary>
        public Boolean IsInfoLibelMinEditable
        {
            get
            {
                Boolean Result = true;

                if (this.InfoLibelMin != null)
                {
                    if (this.InfoLibelMin.LibelInformation == DIRECT_TO_BMP)
                    {
                        Result = false;
                    }
                }

                return Result;
            }
        } // endProperty: IsInfoLibelMinEditable

        /// <summary>
        /// Le libellé InfoLibelRetour est-il éditable?
        /// </summary>
        public Boolean IsInfoLibelRetourEditable
        {
            get
            {
                Boolean Result = true;

                if (this.InfoLibelRetour != null)
                {
                    if (this.InfoLibelRetour.LibelInformation == DIRECT_TO_BMP)
                    {
                        Result = false;
                    }
                }

                return Result;
            }
        } // endProperty: IsInfoLibelRetourEditable

        /// <summary>
        /// Le libellé InfoLibelUnit est-il éditable?
        /// </summary>
        public Boolean IsInfoLibelUnitEditable
        {
            get
            {
                Boolean Result = true;

                if (this.InfoLibelMin != null)
                {
                    if (this.InfoLibelMin.LibelInformation == DIRECT_TO_BMP)
                    {
                        Result = false;
                    }
                }

                return Result;
            }
        } // endProperty: IsInfoLibelUnitEditable

        /// <summary>
        /// Traduction du bouton Annuler
        /// </summary>
        public String Cancel
        {
            get
            {
                return "/LIBEL_MO/MO_EME_CANCEL";
            }
        } // endProperty: Cancel

        /// <summary>
        /// Traduction du bouton Valider
        /// </summary>
        public String Validate
        {
            get
            {
                return "/LIBEL_MO/MO_EME_VALID";
            }
        } // endProperty: Validate

        /// <summary>
        /// L'abscisse du point de mesure A
        /// </summary>
        public Single PointA_X
        {
            get
            {
                Single Result = 0;
                if (this.Information != null)
                {
                    Result = (Single)Math.Round(this.Information.PointA_X, NB_DECIMAL_MAX);
                    //Cmd = (Single)Math.Round(Cmd, this.NbDecimales);
                }

                return Result;
            }
            set
            {
                if (this.Information != null)
                {
                    this.Information.PointA_X = (Single)Math.Round(value, NB_DECIMAL_MAX);
                    RaisePropertyChanged("PointA_X");
                }
            }
        } // endProperty: PointA_X

        /// <summary>
        /// L'ordonnée du point A
        /// </summary>
        public Single PointA_Y
        {
            get
            {
                Single Result = 0;
                if (this.Information != null)
                {
                    Result = (Single)Math.Round(this.Information.PointA_Y, NB_DECIMAL_MAX);
                    //Cmd = (Single)Math.Round(Cmd, this.NbDecimales);
                }
                return Result;
            }
            set
            {
                if (this.Information != null)
                {
                    this.Information.PointA_Y = (Single)Math.Round(value, NB_DECIMAL_MAX);
                    RaisePropertyChanged("PointA_Y");
                }
            }
        } // endProperty: PointA_Y

        /// <summary>
        /// L'abscisse du point de mesure B
        /// </summary>
        public Single PointB_X
        {
            get
            {
                Single Result = 0;
                if (this.Information != null)
                {
                    Result = (Single)Math.Round(this.Information.PointB_X, NB_DECIMAL_MAX);
                    //Cmd = (Single)Math.Round(Cmd, this.NbDecimales);
                }

                return Result;
            }
            set
            {
                if (this.Information != null)
                {
                    this.Information.PointB_X = (Single)Math.Round(value, NB_DECIMAL_MAX);
                    RaisePropertyChanged("PointB_X");
                }
            }
        } // endProperty: PointB_X

        /// <summary>
        /// L'ordonnée du point B
        /// </summary>
        public Single PointB_Y
        {
            get
            {
                Single Result = 0;
                if (this.Information != null)
                {
                    Result = (Single)Math.Round(this.Information.PointB_Y, NB_DECIMAL_MAX);
                    //Cmd = (Single)Math.Round(Cmd, this.NbDecimales);
                }

                return Result;
            }
            set
            {
                if (this.Information != null)
                {
                    this.Information.PointB_Y = (Single)Math.Round(value, NB_DECIMAL_MAX);
                    RaisePropertyChanged("PointB_Y");
                }
            }
        } // endProperty: PointB_Y

        /// <summary>
        /// Le lien vers la définition du retour d'information
        /// </summary>
        public Information Information
        {
            get
            {
                return this._information;
            }
        } // endProperty: Information

        /// <summary>
        /// L'identifiant de l'information
        /// </summary>
        public String IdentInformation
        {
            get
            {
                String Result;
                if (this._information == null)
                {
                    Result = "";
                }
                else
                {
                    Result = this._information.IdentRetour;
                }
                return Result;
            }
        } // endProperty: IdentInformation

        /// <summary>
        /// Le libellé placé avant le retour d'information
        /// </summary>
        public InformationLabel InfoLibelRetour
        {
            get
            {
                return this._labelRetour;
            }
            private set
            {
                this._labelRetour = value;
            }
        } // endProperty: InfoLibelRetour

        /// <summary>
        /// Le libellé de l'unité
        /// </summary>
        public InformationLabel InfoLibelUnit
        {
            get
            {
                return this._labelUnite;
            }
            set
            {
                this._labelUnite = value;
            }
        } // endProperty: InfoLibelUnit

        /// <summary>
        /// L'unité de la grandeur électrique utilisée pour la calibration
        /// </summary>
        public String InfoElectricUnit
        {
            get
            {
                return this.Information.LienAnaUnite;
            }
        } // endProperty: InfoElectricUnit

        /// <summary>
        /// Le libellé à afficher si la valeur est inférieure au seuil mini
        /// </summary>
        public InformationLabel InfoLibelMin
        {
            get
            {
                return this._labelMin;
            }
            set
            {
                this._labelMin = value;
            }
        } // endProperty: InfoLibelMin

        /// <summary>
        /// Le libellé à afficher si la valeur est supérieure au seuil maxi
        /// </summary>
        public InformationLabel InfoLibelMax
        {
            get
            {
                return this._labelMax;
            }
            set
            {
                this._labelMax = value;
            }
        } // endProperty: InfoLibelMax

        /// <summary>
        /// Une valeur de démo défini à partir de la configuration 
        /// </summary>
        public String ValueDemo
        {
            get
            {
                String Result = "";
                String Format;
                Single Value;


                if (this._information != null && this._information.TypeVariable == RIType.RIANA)
                {
                    if (!this.Information.IsLienAna)
                    {
                        Value = (Single)(this.ValMax - this.ValMin) / 2; 
                    }
                    else
                    {
                        Value = (Single)(this.PointB_Y - this.PointA_Y) / 2;
                    }

                    Format = "{0:0." + this.ZeroFill(this._information.NbDecimales) + "}";

                    Result = String.Format(Format, Value);
                }

                return Result;
            }
        } // endProperty: ValueDemo

        /// <summary>
        /// Une valeur de démo, sans mise à l'échelle
        /// </summary>
        public String ValueNonMiseAEchelle
        {
            get
            {
                String Result = "";
                String Format;
                Single Value;


                if (this._information != null)
                {
                    Value = (this._information.ValMax - this._information.ValMin) / 2;
                    Format = "{0:0." + this.ZeroFill(this._information.NbDecimales) + "}";

                    Result = String.Format(Format, Value);
                }

                return Result;
            }
        } // endProperty: ValueNonMiseAEchelle

        /// <summary>
        /// Le retour d'information est-il un booléen ?
        /// </summary>
        public Boolean IsBoolean
        {
            get;
            set;
        } // endProperty: IsBoolean

        /// <summary>
        /// Remplir la chaine avec des zéros
        /// </summary>
        private String ZeroFill(Int32 NbZero)
        {
            String Result = "";

            for (int i = 0; i < NbZero; i++)
            {
                Result += "0";
            }
            return Result;

        } // endMethod: ZeroFill 

        /// <summary>
        /// Retourne le libellé numérique courant
        /// </summary>
        public InformationLabel CurrentLibelVarNum
        {
            get
            {
                InformationLabel Result = null;

                if (this.LibelsVarNumerique.Length > 0)
                {
                    for (int i = 0; i < this.LibelsVarNumerique.Length; i++)
                    {
                        if (this.LibelsVarNumerique[i] != null)
                        {
                            Result = this.LibelsVarNumerique[i];
                            break;
                        } 
                    }
                }

                return Result;
            }
        } // endProperty: CurrentLibelVarNum

        /// <summary>
        /// la pente pour la mise à l'échelle
        /// </summary>
        public Single a
        {
            get
            {
                Single Result = 0;
                if (this._information != null)
                {
                    Result = this._information.Scaling_a;
                }
                return Result;
            }
            set
            {
                this._information.Scaling_a = value;
            }
        } // endProperty: a

        /// <summary>
        /// L'offset pour la mise à l'échelle
        /// </summary>
        public Single b
        {
            get
            {
                Single Result = 0;
                if (this._information != null)
                {
                    Result = this._information.Scaling_b;
                }
                return Result;
            }
            set
            {
                this._information.Scaling_b = value;
            }
        } // endProperty: b

        /// <summary>
        /// Valeur minimum exprimé en unité physique
        /// </summary>
        public Double ValMin
        {
            get
            {
                Double Result = this._valMin; // *this.a + this.b;
                //Cmd = Math.Round(Cmd, this.Information.NbDecimales);
                return Result;
            }
            set
            {
                Double valMin = value;
                // Si le retour d'information est lié à une entrée analogique, calculer la valeur système
                //if (this.Information != null && this.Information.TypeVariable == RIType.RIANA && this.Information.IsLienAna)
                //{
                    valMin = (value - this.b) / this.a; 
                //}
                if ((Int32)((Int16)valMin) == (Int32)valMin)
                {
                    this._valMin = value;
                }
                else
                {
                    throw new Exception("Bad mood!");
                }
            }
        } // endProperty: ValMin

        /// <summary>
        /// Valeur maximum exprimée en unité physique
        /// </summary>
        public Double ValMax
        {
            get
            {
                Double Result = this._valMax; // = this._valMax * this.a + this.b;
                //Cmd = Math.Round(Cmd, this.Information.NbDecimales);
                return Result;
            }
            set
            {
                // Vérifier que la valeur est valide
                Double valMax = value;
                // Si le retour d'information est lié à une entrée analogique, calculer la valeur système
                //if (this.Information != null && this.Information.TypeVariable == RIType.RIANA && this.Information.IsLienAna)
                //{
                    valMax = (value - this.b) / this.a; 
                //}
                if ((Int32)((Int16)valMax) == (Int32)valMax)
                {
                    this._valMax = value;
                }
                else
                {
                    throw new Exception("Bad mood!");
                }
            }
        } // endProperty: ValMax

        /// <summary>
        /// Numéro de l'information
        /// </summary>
        public Int32 NumInfo
        {
            get;
            set;
        } // endProperty: NumInfo

        /// <summary>
        /// Le nombre de décimales pour le retour d'information
        /// </summary>
        public Int32 NbDecimales
        {
            get
            {
                return this.Information.NbDecimales;
            }
            set
            {
                this.Information.NbDecimales = value;

                RaisePropertyChanged("PointA_X");
                RaisePropertyChanged("PointA_Y");
                RaisePropertyChanged("PointB_X");
                RaisePropertyChanged("PointB_Y");
                RaisePropertyChanged("TPtA_X");
                RaisePropertyChanged("TPtB_X");
                RaisePropertyChanged("NbDecimales");
            }
        } // endProperty: NbDecimales

        /// <summary>
        /// Le titre de la fenêtre
        /// </summary>
        public String Title
        {
            get
            {
                String Result = String.Format("{0} - {1} {2} {3}", this.NumInfo, this.InfoLibelRetour.LibelInformation, this.ValueDemo, this.InfoLibelUnit.LibelInformation);

                return Result;
            }
        } // endProperty: Title

        /// <summary>
        /// Le gain et l'offset sont dépliés
        /// </summary>
        public Boolean IsExpanded
        {
            get
            {
                return this._isExpanded;
            }
            set
            {
                this._isExpanded = value;
                RaisePropertyChanged("IsExpanded");
                RaisePropertyChanged("IsEnabled");
            }
        } // endProperty: IsExpanded

        /// <summary>
        /// Le tableau de calibration n'est pas disponible
        /// </summary>
        public Boolean IsEnabled
        {
            get
            {
                return !this.IsExpanded;
            }
        } // endProperty: IsEnabled

        /// <summary>
        /// L'unité de la variable utilisée dans les équations
        /// </summary>
        public String UnitInfo
        {
            get
            {
                String Result = "";

                if (this._information != null)
                {
                    Result = ((Int32)this._information.TypeVariable).ToString();
                }

                return Result;
            }
            set
            {
                this._information.TypeVariable = (RIType)(Convert.ToInt32(value));
            }
        } // endProperty: UnitInfo

        /// <summary>
        /// La calibration théorique
        /// </summary>
        public Boolean CalibrationTheorique
        {
            get
            {
                return this._calibrationTheorique;
            }
            set
            {
                //this.CalculGO();
                this._calibrationTheorique = value;
                // La mise à jour de l'affichage est gérée dans la calibration de terrain
            }
        } // endProperty: CalibrationTheorique

        /// <summary>
        /// La calibration terrain
        /// </summary>
        public Boolean CalibrationTerrain
        {
            get
            {
                return this._calibrationTerrain;
            }
            set
            {
                //this.CalculGO();
                this._calibrationTerrain = value;
                if (this._calibrationTerrain)
                {
                    // calculer les valeurs observées à partir des valeurs théoriques
                    this.Information.PointA_X = this.CalculFromTheoricToObserve(this.TPtA_X);
                    this.Information.PointB_X = this.CalculFromTheoricToObserve(this.TPtB_X);
                    RaisePropertyChanged("PointA_X");
                    RaisePropertyChanged("PointB_X");
                }
                else
                {
                    // calculer les valeurs théoriques à partir des valeurs observées
                    this.TPtA_X = this.CalculFromReadToTheoric(this.PointA_X);
                    this.TPtB_X = this.CalculFromReadToTheoric(this.PointB_X);
                    RaisePropertyChanged("TPtA_X");
                    RaisePropertyChanged("TPtB_X");
                }
                this.MAJTypeCalibration();
            }
        } // endProperty: CalibrationTerrain

        /// <summary>
        /// La visibilité du paramétrage de l'entrée Ana
        /// </summary>
        public Visibility EAnaVisibility
        {
            get
            {
                Visibility Result = System.Windows.Visibility.Hidden;
                if (!this.CalibrationTerrain)
                {
                    if (this.Information.EAna != null)
                    {
                        Result = System.Windows.Visibility.Visible;
                    }
                }
                return Result;
            }
        } // endProperty: EAnaVisibility

        /// <summary>
        /// Le nom de l'entrée EAna
        /// </summary>
        public String EAnaName
        {
            get
            {
                String Result = "";

                if (this.Information != null)
                {
                    if (this.Information.EAna != null)
                    {
                        Result = this.Information.EAna.MnemoHardware + " (" + this.Information.EAna.MnemoBornier + ")"; 
                    }
                }

                return Result;
            }
        } // endProperty: EAnaName

        /// <summary>
        /// Le mode EAna en cours de sélection
        /// </summary>
        public ParamsGainOffset CurrentEAnaModes
        {
            get
            {
                ParamsGainOffset Result = null;
                if (this.Information != null)
                {
                    if (this.Information.EAna != null)
                    {
                        Result = this.Information.EAna.CurrentEAnaModes; 
                    }
                }
                return Result;
            }
            set
            {
                if (this.Information != null)
                {
                    this.Information.EAna.CurrentEAnaModes = value;
                }
                this.TPtA_X = value.Min;
                this.TPtB_X = value.Max; 

                RaisePropertyChanged("CurrentEAnaModes");
                RaisePropertyChanged("LegendCalibrTheorique");
                RaisePropertyChanged("InfoElectricUnit");
            }
        } // endProperty: CurrentEAnaModes
        
        /// <summary>
        /// La liste des modes EAna disponible
        /// </summary>
        public ObservableCollection<ParamsGainOffset> EAnaModes
        {
            get
            {
                ObservableCollection<ParamsGainOffset> Result = new ObservableCollection<ParamsGainOffset>();
                if (this.Information != null)
                {
                    if (this.Information.EAna != null)
                    {
                        Result = this.Information.EAna.EAnaModes; 
                    }
                }

                return Result;
            }
        } // endProperty: EAnaModes

        /// <summary>
        /// La visibilité des boutons radios permettant de choisir le type de calibration
        /// </summary>
        public Visibility RadioVisibility
        {
            get
            {
                Visibility Result = System.Windows.Visibility.Collapsed;

                if (this.Information.IsLienAna)
                {
                    Result = System.Windows.Visibility.Visible;
                }

                return Result;
            }
        } // endProperty: RadioVisibility

        /// <summary>
        /// La visibilité du tableau théorique
        /// </summary>
        public Visibility TabTheoriqueVisibility
        {
            get
            {
                Visibility Result;

                if (this.Information.IsLienAna)
                {
                    if (this.CalibrationTerrain)
                    {
                        Result = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        Result = Visibility.Visible;
                    }
                }
                else
                {
                    Result = Visibility.Collapsed;
                }

                return Result;
            }
        } // endProperty: TabTheoriqueVisibility

        /// <summary>
        /// La visibilité du tableau terrain
        /// </summary>
        public Visibility TabTerrainVisibility
        {
            get
            {
                Visibility Result;

                if (this.Information.IsLienAna)
                {
                    if (!this.CalibrationTerrain)
                    {
                        Result = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        Result = Visibility.Visible;
                    }
                }
                else
                {
                    Result = Visibility.Collapsed;
                }

                return Result;
            }
        } // endProperty: TabTerrainVisibility

        /// <summary>
        /// Légende de la calibration théorique
        /// </summary>
        public String LegendCalibrTheorique
        {
            get
            {
                String Result = String.Format(this.MO_EME_LEGTHEO, this.Information.LienAnaType, this.Information.LienAnaValMin, this.Information.LienAnaValMax, this.Information.LienAnaUnite);

                return Result;
            }
        } // endProperty: LegendCalibrTheorique

        /// <summary>
        /// Point théorique entré par l'utilisateur
        /// </summary>
        public Single TPtA_X
        {
            get
            {
                Single V = this._TPtA_X;
                //V = (Single)Math.Round(V, this.NbDecimales);
                return V;
            }
            set
            {
                this._TPtA_X = (Single)Math.Round(value, NB_DECIMAL_MAX);
                //this.PointA_X = this.CalculValObserve(this._TPtA_X);
                RaisePropertyChanged("TPtA_X");
                //RaisePropertyChanged("PointA_X");
            }
        } // endProperty: TPtA_X

        /// <summary>
        /// Point théorique entré par l'utilisateur
        /// </summary>
        public Single TPtB_X
        {
            get
            {
                Single V = this._TptB_X;
                //V = (Single)Math.Round(V, this.NbDecimales);
                return V;
            }
            set
            {
                this._TptB_X = (Single)Math.Round(value, NB_DECIMAL_MAX);
                //this.PointB_X = this.CalculValObserve(this._TptB_X);
                RaisePropertyChanged("TPtB_X");
                //RaisePropertyChanged("PointB_X");
            }
        } // endProperty: TPtB_X

        #endregion

        // Constructeurs
        public InfoByMode(Information Info, InformationLabel Label, InformationLabel Unite, InformationLabel Max, InformationLabel Min)
        {
            this._information = Info;
            this.InfoLibelRetour = Label;
            this.InfoLibelUnit = Unite;
            this.InfoLibelMax = Max;
            this.InfoLibelMin = Min;
            this.PointA_Y = 0;
            this.PointB_Y = 10;

            // Calcul de ValMax et ValMin
            //if (this.Information != null && this.Information.TypeVariable == RIType.RIANA && this.Information.IsLienAna)
            //{
                this._valMax = this._information.ValMax * this.a + this.b;
                this._valMax = Math.Round(this._valMax, this.NbDecimales);

                this._valMin = this._information.ValMin * this.a + this.b;
                this._valMin = Math.Round(this._valMin, this.NbDecimales); 
            //}
            //else if (this.Information != null)
            //{
            //    this._valMax = this._information.ValMax;
            //    this._valMin = this._information.ValMin;
            //}

            // Créer les commandes
            this.CreateCommandCalculGO();
            this.CreateCommandValidExpertGO();
            this.CreateCommandCancel();

            // Initialisation du mode par défaut
            if (this.Information != null)
            {
                if (this.Information.EAna != null)
                {
                    String MT = PegaseCore.PegaseData.Instance.ModuleT.TypeMT.ToUpper();
                    String ModeName = "";
                    if (this.Information.LienAnaName != null)
                    {
                        ModeName = this.Information.LienAnaName;
                    }

                    //ParamsGainOffset PGO = PegaseCore.RefGainOffset.Get().GetParamByMaterialAndName(MT, ModeName);
                    //if (PGO != null)
                    //{
                    //    this.CurrentEAnaModes = PGO;
                    //}
                }
            }

            // Initialiser la valeur des Points
            this.PointA_X = this.PointA_Y;
            this.PointB_X = this.PointB_Y;

            // recalculer les points théoriques
            this.CalculTheoricFromValue();

            this.SaveInitialsValues();

            this.CalibrationTheorique = true;
        }
        
        public InfoByMode(XElement Info, Information information)
        {
            // Initialisation
            this._navRetoursInfo = new XMLCore.XMLProcessing();

            this._navRetoursInfo.OpenXML(Info);
            this._information = information;
            this._calibrationTerrain = false;
            this._calibrationTheorique = true;
            // Calcul de ValMax et ValMin
            //if (information != null && information.IsLienAna)
            //{
                this._valMax = this._information.ValMax * this.a + this.b;
                this._valMax = Math.Round(this._valMax, this.NbDecimales);

                this._valMin = this._information.ValMin * this.a + this.b;
                this._valMin = Math.Round(this._valMin, this.NbDecimales); 
            //}
            //else
            //{
            //    this._valMax = this._information.ValMax;
            //    this._valMin = this._information.ValMin;
            //}

            // Créer les commandes
            this.CreateCommandCalculGO();
            this.CreateCommandValidExpertGO();
            this.CreateCommandCancel();

            // Initialisation du mode par défaut
            if (this.Information != null)
            {
                if (this.Information.EAna != null)
                {
                    String MT = PegaseCore.PegaseData.Instance.ModuleT.TypeMT.ToUpper();
                    String ModeName = "";
                    if (this.Information.LienAnaName != null)
                    {
                        ModeName = this.Information.LienAnaName;
                    }
                    
                    //ParamsGainOffset PGO = PegaseCore.RefGainOffset.Get().GetParamByMaterialAndName(MT, ModeName);
                    //if (PGO != null)
                    //{
                    //    this.CurrentEAnaModes = PGO; 
                    //}
                }
            }

            // Initialiser la valeur des Points
            this.PointA_X = this.PointA_Y;
            this.PointB_X = this.PointB_Y;

            // recalculer les points théoriques
            this.CalculTheoricFromValue( );

            // Initialiser le LabelInfo
            String IdentLabelInfo = this._navRetoursInfo.GetNodeByPath("LibelRetour").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            var Query = from row in PegaseCore.PegaseData.Instance.OLogiciels.LibelRI
                        where row.IdentLibelInformation == IdentLabelInfo
                        select row;

            this.InfoLibelRetour = Query.FirstOrDefault();

            // Initialiser le LibelUnite
            if (this._navRetoursInfo.GetNodeByPath("LibelUnité") != null)
            {
                IdentLabelInfo = this._navRetoursInfo.GetNodeByPath("LibelUnité").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value; 
            }
            else if (this._navRetoursInfo.GetNodeByPath("LibelUnite") != null)
            {
                IdentLabelInfo = this._navRetoursInfo.GetNodeByPath("LibelUnite").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            }
            else if (this._navRetoursInfo.GetNodeByPath("LibelUnit?") != null)
            {
                IdentLabelInfo = this._navRetoursInfo.GetNodeByPath("LibelUnit?").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            }
            else
            {
                throw new Exception("Fiche corrompue, LibelUnite non présent");
            }

            var QueryU = from row in PegaseCore.PegaseData.Instance.OLogiciels.LibelRI
                        where row.IdentLibelInformation == IdentLabelInfo
                        select row;

            this.InfoLibelUnit = QueryU.FirstOrDefault();

            // Initialiser le LibelMin
            IdentLabelInfo = this._navRetoursInfo.GetNodeByPath("LibelMin").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            var QueryMin = from row in PegaseCore.PegaseData.Instance.OLogiciels.LibelRI
                        where row.IdentLibelInformation == IdentLabelInfo && row.IdentLibelInformation != ""
                        select row;

            this.InfoLibelMin = QueryMin.FirstOrDefault();

            // Initialiser le LibelMax
            IdentLabelInfo = this._navRetoursInfo.GetNodeByPath("LibelMax").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            var QueryMax = from row in PegaseCore.PegaseData.Instance.OLogiciels.LibelRI
                        where row.IdentLibelInformation == IdentLabelInfo && row.IdentLibelInformation != ""
                        select row;

            this.InfoLibelMax = QueryMax.FirstOrDefault();

            // Initialiser les LibelVarNumerique
            ObservableCollection<XElement> libelVarNum = this._navRetoursInfo.GetNodeByPath("LibelVarNumerique");
            Int32 i = 0;
            if (libelVarNum != null)
            {
                foreach (var item in libelVarNum)
                {
                    String value = item.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                    if (value != "")
                    {
                        var QueryLibel = from row in PegaseCore.PegaseData.Instance.OLogiciels.LibelRI
                                         where row.IdentLibelInformation == value
                                         select row;

                        this.LibelsVarNumerique[i] = QueryLibel.FirstOrDefault();
                    }
                    else
                    {
                        this.LibelsVarNumerique[i] = null;
                    }
                    i++;
                } 
            }
        }

        // Méthodes
        
        /// <summary>
        /// Sauver les valeurs initiales
        /// </summary>
        public void SaveInitialsValues ( )
        {
            // Sauvegarder les valeurs des paramètres pour l'action cancel
            this._oldPointA_Y = this.PointA_Y;
            this._oldPointB_Y = this.PointB_Y;
            this._oldPointA_X = this.PointA_X;
            this._oldPointB_X = this.PointB_X;
            this._oldPtA_X = this.TPtA_X;
            this._oldPtB_X = this.TPtB_X;
            this._olda = this.a;
            this._oldb = this.b;
            this._oldCurrentEANA = this.CurrentEAnaModes;
            this._oldUnit = this.UnitInfo;
            this._oldNbDecimal = this.NbDecimales;
        } // endMethod: SaveInitialsValues

        /// <summary>
        /// Mettre à jour l'affichage présentant le type de calibration
        /// </summary>
        private void MAJTypeCalibration ( )
        {
            RaisePropertyChanged("CalibrationTerrain");
            RaisePropertyChanged("CalibrationTheorique");
            RaisePropertyChanged("TabTerrainVisibility");
            RaisePropertyChanged("TabTheoriqueVisibility");
            RaisePropertyChanged("LegendCalibrTheorique");
            RaisePropertyChanged("EAnaVisibility");
        } // endMethod: MAJTypeCalibration
        
        /// <summary>
        /// Mettre à jour les valeurs théoriques si besoin
        /// </summary>
        public void MajValUITheorique ( )
        {
            RaisePropertyChanged("TPtA_X");
            RaisePropertyChanged("TPtB_X");
        } // endMethod: MajValUITheorique

        /// <summary>
        /// Calculer la valeur théorique en fonction de la valeur souhaitée
        /// </summary>
        public void CalculTheoricFromValue ( )
        {
            Single G, O;
            Single X1, X2;

            X1 = this.CalculValFromObserve(this.PointA_Y);
            X2 = this.CalculValFromObserve(this.PointB_Y);

            this.CalculDroite(out G, out O, 0, this.Information.LienAnaValMin, 4095, this.Information.LienAnaValMax);

            this.TPtA_X = X1 * G + O;
            this.TPtB_X = X2 * G + O;
        } // endMethod: CalculTheoricFromValue

        /// <summary>
        /// Calculé la valeur utilisée pour calculer le Gain et Offset à partir d'une valeur théorique
        /// </summary>
        private Single CalculValFromTheoric ( Single ValTheorique )
        {
            Single Result, a, b;

            this.CalculDroite(out a, out b, this.Information.LienAnaValMin, 0, this.Information.LienAnaValMax, 4095);

            Result = ValTheorique * a + b;
            return Result;
        } // endMethod: CalculValObserve
        
        /// <summary>
        /// Calculé la valeur utilisée pour calculer le Gain et l'Offset à partir d'une valeur observée
        /// </summary>
        private Single CalculValFromObserve ( Single ValObserve )
        {
            Single Result;

            Result = (ValObserve - b) / a;
            return Result;
        } // endMethod: CalculValFromObserve
        
        /// <summary>
        /// Calculé la valeur lue sur la télécommande à partir de la valeur théorique
        /// </summary>
        private Single CalculFromTheoricToObserve ( Single ValTheo )
        {
            Single Result = this.CalculValFromTheoric(ValTheo);
            Result = Result * this.a + this.b;
            
            return Result;
        } // endMethod: CalculFromTheoricToObserve

        /// <summary>
        /// Calculé la valeur théorique (courant / tension) à partir de la valeur lue
        /// </summary>
        private Single CalculFromReadToTheoric ( Single ValRead )
        {
            Single Result = this.CalculValFromObserve(ValRead);
            Single a, b;

            this.CalculDroite(out a, out b, 0, this.Information.LienAnaValMin, 4095, this.Information.LienAnaValMax);

            Result = Result * a + b;
            return Result;
        } // endMethod: CalculFromReadToObserve

        /// <summary>
        /// Calculer les coefficient d'une droite à partir des données points
        /// </summary>
        private void CalculDroite ( out Single a, out Single b, Single PtX1, Single PtY1, Single PtX2, Single PtY2 )
        {
            // Calculer le gain et l'offset
            Single G, O;

            G = (PtY1 - PtY2) / (PtX1 - PtX2);
            O = PtY1 - (G * PtX1);

            a = G;
            b = O;
        } // endMethod: CalculDroite

        // Command

        #region CommandCalculGO
        /// <summary>
        /// La commande CalculGO
        /// </summary>
        public ICommand CommandCalculGO
        {
            get;
            internal set;
        } // endProperty: CommandCalculGO

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandCalculGO()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandCalculGO = new Mvvm.Command.RelayCommand(ExecuteCommandCalculGO, CanExecuteCommandCalculGO);
        } // endMethod: CreateCommandCalculGO

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandCalculGO()
        {
            // Si toutes les données sont bonnes ->  effectuer les calculs et fermer la fenêtre
            if (this.TabTheoriqueVisibility == Visibility.Visible || this.TabTerrainVisibility == Visibility.Visible)
            {
                // Si un lien EAna est effectif
                this.CalculGO();
            }
            this.ExecuteCommandValidExpertGO();
        } // endMethod: ExecuteCommandCalculGO
        
        /// <summary>
        /// Calculer G & O
        /// </summary>
        private void CalculGO ( )
        {
            Single G, O;
            Single AX = 0, BX = 0;
            //Single SMin, SMax;

            // Calculer les valeurs réels des seuils
            // SMin = (Single)this.ValMin;
            // SMax = (Single)this.ValMax;

            // Calculer les valeur dans la dynamique 0 - 4095
            if (this.CalibrationTerrain)
            {
                AX = this.CalculValFromObserve(this.PointA_X);
                BX = this.CalculValFromObserve(this.PointB_X);
            }
            else if (this.CalibrationTheorique)
            {
                AX = this.CalculValFromTheoric(this._TPtA_X);
                BX = this.CalculValFromTheoric(this._TptB_X);
            }
            else
            {
                G = 0;
                O = 0;
            }

            // Calculer les nouveaux gains et les nouveaux seuils
            this.CalculDroite(out G, out O, AX, this.PointA_Y, BX, this.PointB_Y);
            this.a = G;
            this.b = O;

            // Recalculer les Valeurs des seuils avec les nouveaux Gains et Offsets
            //this.ValMin = SMin;
            //this.ValMax = SMax;

            RaisePropertyChanged("a");
            RaisePropertyChanged("b");
        } // endMethod: CalculGO

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandCalculGO()
        {
            Boolean Result = false;

            if (!this.IsExpanded)
            {
                //if ((Int32)((Int16)this._valMax) == (Int32)this._valMax)
                //{
                //    if ((Int32)((Int16)this._valMin) == (Int32)this._valMin)
                //    {
                Result = true;
                //    }
                //}
            }

            return Result;
        } // endMethod: CanExecuteCommandCalculGO 
        #endregion

        #region CommandCancel
        /// <summary>
        /// La commande Cancel
        /// </summary>
        public ICommand CommandCancel
        {
            get;
            internal set;
        } // endProperty: CommandCancel

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandCancel()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandCancel = new Mvvm.Command.RelayCommand(ExecuteCommandCancel, CanExecuteCommandCancel);
        } // endMethod: CreateCommandCancel

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandCancel()
        {
            // Restaurer les valeurs
            this.a = this._olda;
            this.b = this._oldb;
            if (this._oldCurrentEANA != null)
            {
                this.CurrentEAnaModes = this._oldCurrentEANA; 
            }
            this.PointA_Y = this._oldPointA_Y;
            this.PointB_Y = this._oldPointB_Y;
            this.UnitInfo = this._oldUnit;
            this.NbDecimales = this._oldNbDecimal;
            this.TPtA_X = this._oldPtA_X;
            this.TPtB_X = this._oldPtB_X;
            this.PointA_X = this._oldPointA_X;
            this.PointB_X = this._oldPointB_X;

            // Envoyer un message pour annoncer à la fenêtre qu'elle doit être fermée
            Mvvm.Messaging.Messenger.Default.Send<CommandMessage>(new CommandMessage(this, PegaseCore.Commands.CMD_CLOSE_WINDOW));
        } // endMethod: ExecuteCommandCancel

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandCancel()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandCancel 
        #endregion

        #region CommandValidExpertGO
        /// <summary>
        /// La commande ValidExpertGO
        /// </summary>
        public ICommand CommandValidExpertGO
        {
            get;
            internal set;
        } // endProperty: CommandValidExpertGO

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandValidExpertGO()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandValidExpertGO = new Mvvm.Command.RelayCommand(ExecuteCommandValidExpertGO, CanExecuteCommandValidExpertGO);
        } // endMethod: CreateCommandValidExpertGO

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandValidExpertGO()
        {
            this.PointA_X = this.PointA_Y;
            this.PointB_X = this.PointB_Y;
            // Fermer la fenêtre
            Mvvm.Messaging.Messenger.Default.Send<CommandMessage>(new CommandMessage(this, PegaseCore.Commands.CMD_CLOSE_WINDOW));
            // Calculer les valeurs Min / Max
            Double valMax;

            //if (this.Information != null && this.Information.TypeVariable == RIType.RIANA && this.Information.IsLienAna)
            //{
                valMax = (this._valMax - this.b) / this.a;
            //}
            //else
            //{
            //    valMax = this._valMax;
            //}
            this.Information.ValMax = (Int16)valMax;

            Double valMin;

            //if (this.Information != null && this.Information.TypeVariable == RIType.RIANA && this.Information.IsLienAna)
            //{
                valMin = (this._valMin - this.b) / this.a; 
            //}
            //else
            //{
            //    valMin = this._valMin;
            //}
            this.Information.ValMin = (Int16)valMin;
        } // endMethod: ExecuteCommandValidExpertGO

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandValidExpertGO()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandValidExpertGO 
        #endregion
    }
}
