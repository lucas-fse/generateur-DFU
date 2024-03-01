using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using JAY.PegaseCore;
//using JAY.Affichage;

namespace JAY.PegaseCore
{
    public class VariableSortieTOR
    {
        private EC_Output _sortieTOR;
        private VariableE _variableBoolLinked;
        private Boolean _isUsed;

        /// <summary>
        /// La sortie TOR liée
        /// </summary>
        public EC_Output SortieTOR
        {
            get
            {
                return this._sortieTOR;
            }
            set
            {
                this._sortieTOR = value;
            }
        } // endProperty: SortieTOR

        /// <summary>
        /// La variable booléenne liée
        /// </summary>
        public VariableE VariableBoolLinked
        {
            get
            {
                return this._variableBoolLinked;
            }
            set
            {
                this._variableBoolLinked = value;
            }
        } // endProperty: VariableBoolLinked

        /// <summary>
        /// Le couple variable / sortie TOR est-il utilisé?
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
            }
        } // endProperty: IsUsed

        /// <summary>
        /// Le mnémonique de la variable à utiliser dans les équations
        /// </summary>
        public String VarMnemo
        {
            get
            {
                return this.VariableBoolLinked.Name;
            }
        } // endProperty: VarName

        /// <summary>
        /// Le mnémologique de la sortie TOR utilisé dans les équations
        /// </summary>
        public String OTorMnemo
        {
            get
            {
                return this.SortieTOR.STOR.MnemoLogique;
            }
        } // endProperty: OTorMnemo

        public VariableSortieTOR(EC_Output OTOR, VariableE variable)
        {
            this.SortieTOR = OTOR;
            this.VariableBoolLinked = variable;
            this.IsUsed = false;
        }
    }

    public class VariableInterVerrouBtn
    {
        String _buttonMnemoClient;
        VariableE _variableBool;

        /// <summary>
        /// L'organe pour lequel la variable est utilisée
        /// </summary>
        public String ButtonMnemoClient
        {
            get
            {
                return this._buttonMnemoClient;
            }
            set
            {
                this._buttonMnemoClient = value;
            }
        } // endProperty: ButtonMnemologique

        /// <summary>
        /// La variable Booléenne relié à un organe pour l'interverrouillage
        /// </summary>
        public VariableE VariableBool
        {
            get
            {
                return this._variableBool;
            }
            set
            {
                this._variableBool = value;
            }
        } // endProperty: VariableBool

        public VariableInterVerrouBtn(String BtnMnemoClient, VariableE Variable)
        {
            this.ButtonMnemoClient = BtnMnemoClient;
            this.VariableBool = Variable;
        }

        /// <summary>
        /// Récupérer l'organe lié à la variable
        /// </summary>
        public OrganCommand GetCurrentOrgan ( )
        {
            OrganCommand Result = null;

            var Query = from organ in PegaseData.Instance.MOperateur.OrganesCommandes
                        where organ.MnemoClient == this.ButtonMnemoClient
                        select organ;

            if (Query.Count() > 0)
            {
                Result = Query.First();
            }

            return Result;
        } // endMethod: GetCurrentOrgan

        /// <summary>
        /// Récupérer l'organe lié à la variable
        /// </summary>
        public static OrganCommand GetCurrentOrgan(String ButtonMnemoClient)
        {
            OrganCommand Result = null;

            var Query = from organ in PegaseData.Instance.MOperateur.OrganesCommandes
                        where organ.MnemoClient == ButtonMnemoClient
                        select organ;

            if (Query.Count() > 0)
            {
                Result = Query.First();
            }

            return Result;
        } // endMethod: GetCurrentOrgan
    }

    public static class EquationBuilder
    {
        #region Constantes

        private const Int32 MODE_UNIVERSELLE = 32;

        #endregion

        // Variables
        #region Variables

        private static ObservableCollection<VariableSortieTOR> _dicoVarTor;
        private static ObservableCollection<VariableInterVerrouBtn> _dicoVarIVBtn;
        private static Int32 _numNextVarBool;
        private static Int32 _numNextVarNum;
        private static Int32 _numEndUniversalModeVarNum;
        private static List<VariableE> _variablesEquation;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le dictionnaire des variables utilisées pour l'inter-verrouillage
        /// </summary>
        public static ObservableCollection<VariableInterVerrouBtn> DicoVarIVBtn
        {
            get
            {
                if (EquationBuilder._dicoVarIVBtn == null)
                {
                    EquationBuilder._dicoVarIVBtn = new ObservableCollection<VariableInterVerrouBtn>();
                }
                return EquationBuilder._dicoVarIVBtn;
            }
            set
            {
                EquationBuilder._dicoVarIVBtn = value;
            }
        } // endProperty: DicoVarIVBtn

        /// <summary>
        /// Le dictionnaire des variables booléennes utilisées pour évaluer l'état des relais / sorties TOR
        /// </summary>
        private static ObservableCollection<VariableSortieTOR> DicoVarTOR
        {
            get
            {
                if (_dicoVarTor == null)
                {
                    _dicoVarTor = new ObservableCollection<VariableSortieTOR>();
                }

                return _dicoVarTor;
            }
            set
            {
                _dicoVarTor = value;
            }
        } // endProperty: DicoVariablesSortiesTOR

        #endregion

        // Constructeur
        #region Constructeur

        static EquationBuilder()
        {
            // Initialisation
            PegaseData.Instance.CheckValidVariableForEquation();

            // 1 - Lister les équations utilisables
            var Query = from variable in PegaseData.Instance.Variables
                        where variable.IsUsedByUser == false && variable.Name.Contains("VARIABLE_NUMERIQUE")
                        select variable;

            EquationBuilder._variablesEquation = Query.ToList<VariableE>();
        }

        #endregion

        // Méthodes
        #region Méthodes
        /// <summary>
        /// Ajouter une formule à un mode ou au mode universel
        /// </summary>
        private static void AddFormule(Formule f, EC_Mode mode)
        {
            if (f != null)
            {
                if (mode.RefMode.Position == 32)
                {
                    PegaseData.Instance.ParamHorsMode.Formules.Add(f);
                }
                else
                {
                    if (mode.RefMode.Position < PegaseData.Instance.OLogiciels.ModesExploitation.Count)
                    {
                        PegaseData.Instance.OLogiciels.ModesExploitation[mode.RefMode.Position].Formules.Add(f);
                    }
                }
            }
        } // endMethod: AddFormule

        /// <summary>
        /// Initialiser le dictionnaire associant variables et sorties TOR
        /// </summary>
        public static Boolean InitDicoVariablesSortiesTOR ( EC_Mode mode )
        {
            Boolean Result = false;
            Int32 NextVar = 0;
            // 1 - mettre à jour la liste des variables utilisables
            //PegaseData.Instance.CheckValidVariableForEquation();
            EquationBuilder.DicoVarTOR.Clear();

            // 2 - Définir les variables disponibles
            var QueryVar = from variable in PegaseData.Instance.Variables
                           where variable.IsUsedByUser == false && variable.IsUsedByiDialog == false && variable.VarType == typeof(Boolean) && variable.Name.Contains("VARIABLE_BOOLEENNE")
                           select variable;

            List<VariableE> VariablesDispo = QueryVar.ToList();

            for (int i = 0; (i < mode.Effecteurs[0].Positions[0].Outputs.Count) && (NextVar<VariablesDispo.Count); i++)
            {
                // 3 - définir si la sortie TOR est utilisée
                Boolean IsUsed = EquationBuilder.IsOutputUsed(i, mode);

                // 4 - si la sortie est utilisée, lui associer une variable

                if(IsUsed)
                {
                    EquationBuilder.DicoVarTOR.Add(new VariableSortieTOR(mode.Effecteurs[0].Positions[0].Outputs[i], VariablesDispo[NextVar]));
                    VariablesDispo[NextVar].IsUsedByiDialog = true;
                    if (mode.RefMode.Position == 32)
                    {
                        VariablesDispo[NextVar].IsUsedByUniversalMode = true;
                    }
                    NextVar++;
                    Result = true;
                }
            }

            return Result;
        } // endMethod: InitDicoVariablesSorties

        /// <summary>
        /// Le numéro de sortie
        /// </summary>
        public static Boolean IsOutputUsed ( Int32 NbOutput, EC_Mode mode )
        {
            Boolean Result = false;

            // 1 - définir si la sortie TOR est utilisée

            for (int j = 0; j < mode.Effecteurs.Count; j++)
            {
                for (int k = 0; k < mode.Effecteurs[j].Positions.Count; k++)
                {
                    if (mode.Effecteurs[j].Positions[k].Outputs[NbOutput].IsUsed == true)
                    {
                        Result = true;
                    }
                }
            }

            return Result;
        } // endMethod: IsOutputUsed

        /// <summary>
        /// Initialiser le dictionnaire associant variables et Inter-Verrouillage bouton
        /// </summary>
        public static Boolean InitDicoVariablesIVBtn(EC_Mode mode) //(ObservableCollection<EC_Verrou> VerrouillageBtn)
        {
            Boolean Result = false;
            Int32 NumVar = 0;
            bool mode_universel = false;
            if (mode.RefMode.Position == 32)
            {
                mode_universel = true;
            }
            ObservableCollection<EC_Verrou> VerrouillageBtn = mode.VerrouillageBtn;
            // 1 - mettre à jour la liste des variables utilisables
           // PegaseData.Instance.CheckValidVariableForEquation();
            EquationBuilder.DicoVarIVBtn.Clear();

            // 2 - Définir les variables disponibles
            var QueryVar = from variable in PegaseData.Instance.Variables
                           where variable.IsUsedByUser == false && variable.IsUsedByiDialog == false && variable.VarType == typeof(Boolean) && variable.Name.Contains("VARIABLE_BOOLEENNE")
                           select variable;

            List<VariableE> VariablesDispo = QueryVar.ToList();

            if (VerrouillageBtn.Count <= VariablesDispo.Count)
            {
                for (int i = 0; i < VerrouillageBtn.Count; i++)
                {
                    if (VerrouillageBtn[i].IsOneOutputChecked())
                    {
                        EquationBuilder.DicoVarIVBtn.Add(new VariableInterVerrouBtn(VerrouillageBtn[i].Name, VariablesDispo[NumVar]));
                        VariablesDispo[NumVar].IsUsedByiDialog = true;
                        if (mode_universel== true)
                        {
                            VariablesDispo[NumVar].IsUsedByUniversalMode = true;
                        }
                        NumVar++;
                    }
                }
                Result = true;
            }

            return Result;
        } // endMethod: InitDicoVariablesIVBtn

        /// <summary>
        /// Construire toutes les équations liées à l'utilisation du réseau
        /// </summary>
        public static void BuildAllNetworkEquation ( EC_Mode mode )
        {
            // construire les équations entrées booléennes
            EquationBuilder.BuildReseauInput(EasyConfigData.Get().VarIBool, mode);
            // construire les équations entrées numériques
            EquationBuilder.BuildReseauInput(EasyConfigData.Get().VarINum, mode);
            // construire les équations sorties booléennes
            EquationBuilder.BuildReseauOutput(EasyConfigData.Get().VarOBool, mode);
            // construire les équations sorties numériques
            EquationBuilder.BuildReseauOutput(EasyConfigData.Get().VarONum, mode);
        } // endMethod: BuildAllNetworkEquation

        /// <summary>
        /// Construire la totalité des équations bouton / relais ou selecteur / relais
        /// </summary>
        public static void BuildAllEquations ( ObservableCollection<EC_Mode> Modes )
        {
            EquationBuilder._numNextVarBool = 0;
            EquationBuilder._numEndUniversalModeVarNum = 0;

            // Liberer toutes les variables
        //    foreach (VariableE variable in PegaseData.Instance.Variables)
        //    {
        //        variable.IsUsedByUniversalMode = false;
        //    }

            PegaseData.Instance.CheckValidVariableForEquation();
            // Construire les équations
            foreach (var mode in Modes)
            {
                // Libérer les variables booléennes utilisées précédemment dans les modes pas dans l'universel
                foreach (VariableE variable in PegaseData.Instance.Variables)
                {
                    if (!variable.IsUsedByUniversalMode)
                    {
                       // variable.IsUsedByiDialog = false; 
                    }
                    else
                    {
                        variable.IsUsedByiDialog = true;
                    }
                }

                //EquationBuilder.InitDicoVariablesIVBtn(mode.VerrouillageBtn);
                //PegaseData.Instance.CheckValidVariableForEquation();
                EquationBuilder.InitDicoVariablesIVBtn(mode);
                EquationBuilder.InitDicoVariablesSortiesTOR(mode);

                // Inter-verrouillage Boutons
                Formule f = EquationBuilder.BuildInterVerrouBtn(mode);
                if (f != null)
                {
                    EquationBuilder.AddFormule(f, mode);
                }

                // Boutons / Selecteurs / Relais
                for(int i = 0; i < mode.Effecteurs[0].Positions[0].Outputs.Count; i++)
                {
                    // 1 - construire les formules pour les associations organes / selecteurs / relais
                    f = EquationBuilder.BuildButtonRelayEquation(mode, i);
                    if (f != null)
                    {
                        EquationBuilder.AddFormule(f, mode);
                    }
                }

                // Interverrouillage
                EquationBuilder.BuildVerrouEquation(mode);

                // Sortie ana
                EquationBuilder.BuildSAnaEquation(mode);
            }
        } // endMethod: BuildAllEquations

        /// <summary>
        /// Construire les interverrouillages suivant la méthodes des boutons
        /// </summary>
        public static Formule BuildInterVerrouBtn ( EC_Mode mode )
        {
            Formule Result = null;
            
            // 1 - Initialiser la fonction
            if (EquationBuilder.DicoVarIVBtn.Count > 0)
            {
                Formule f = new Formule(mode.RefMode.Position);
                f.Fonction = "Buttons Interlocking";
                f.Equations.Clear();
                f.MnemoLogiquePhy = EquationBuilder.DicoVarIVBtn[0].VariableBool.Name;
                f.FormuleType = TypeFormule.AUTO;
                f.CommentaireFormule = "Build by Easy Config";
                f.Equations.Clear();

                Int32 NumOrgan = 0;

                foreach (var verrouBtn in mode.VerrouillageBtn)
                {
                    if (verrouBtn.IsOneOutputChecked())
                    {
                        String Eq = "";
                        Boolean IsFirst = true;

                        foreach (var vBtn in verrouBtn.Outputs)
                        {
                            if (vBtn.IsEnable && vBtn.IsUsed)
                            {
                                if (IsFirst)
                                {
                                    VariableInterVerrouBtn variable = EquationBuilder.GetVarByNameBtn(verrouBtn.Name);
                                    // Créer une équation
                                    Eq = String.Format("{0} := {1} != OFF",variable.VariableBool.Name, verrouBtn.Outputs[NumOrgan].Organ.Mnemologique);
                                    Eq += String.Format(" ET {0} == OFF", vBtn.Organ.Mnemologique);
                                    IsFirst = false;
                                }
                                else
                                {
                                    // continuer l'équation
                                    Eq += String.Format(" ET {0} == OFF", vBtn.Organ.Mnemologique);
                                }
                            }
                        }

                        if (Eq != "")
                        {
                            Equation E = new Equation(f);
                            E.TextEquation = Eq;
                            f.Equations.Add(E);
                        }
                    }

                    NumOrgan++;
                }
                if (f.Equations.Count > 0)
                {
                    Result = f;
                }
            }

            return Result;
        } // endMethod: MethodName

        public static VariableSortieTOR GetVarByNumOutput(Int32 NumOutput, EC_Mode mode)
        {
            VariableSortieTOR Result = null;

            EC_Output output = mode.Effecteurs[0].Positions[0].Outputs[NumOutput];
            var Query = from row in DicoVarTOR
                        where row.SortieTOR == output
                        select row;

            if (Query.Count() > 0)
            {
                Result = Query.First();
            }

            return Result;
        }

        /// <summary>
        /// Quelle variable est associé à un interverrouillage bouton?
        /// </summary>
        public static VariableInterVerrouBtn GetVarByNameBtn ( String Name )
        {
            VariableInterVerrouBtn Result = null;

            var Query = from vivBtn in EquationBuilder.DicoVarIVBtn
                        where vivBtn.ButtonMnemoClient == Name
                        select vivBtn;

            if (Query.Count() > 0)
            {
                Result = Query.First();
            }

            return Result;
        } // endMethod: GetVarByNameBtn

        /// <summary>
        /// Construire les équations de type bouton -> relais
        /// </summary>
        private static Formule BuildButtonRelayEquation ( EC_Mode mode, Int32 OutputNumber)
        {
            Formule Result = null;
            
            // 1 - Initialiser la fonction
            VariableSortieTOR variableSortieTor = EquationBuilder.GetVarByNumOutput(OutputNumber, mode);

            if (EquationBuilder.DicoVarTOR.Count > 0 && variableSortieTor != null)
            {
                Boolean IsFirst = true;
                Formule f = new Formule(mode.RefMode.Position);
                f.Fonction = "R_BT_" + variableSortieTor.OTorMnemo;
                f.Equations.Clear();
                f.MnemoLogiquePhy = variableSortieTor.VarMnemo;
                f.FormuleType = TypeFormule.AUTO;
                f.CommentaireFormule = "Build by Easy Config";

                Equation Eq = new Equation(f);
                String variableBool = variableSortieTor.VarMnemo;
                Eq.TextEquation = String.Format("{0} := ", variableBool);

                foreach (var effecteur in mode.Effecteurs)
                {
                    String suiteEq = "";
                    if (effecteur.EffecteurType == TypeEffecteur.physic)
                    {
                        // Organes physiques
                        if (effecteur.Organ != null)
                        {
                            if (effecteur.Organ.MnemoHardFamilleMO.ToUpper() == "BT")
                            {
                                suiteEq = EquationBuilder.BuildButtonEquation(effecteur, OutputNumber);
                            }
                            else if (effecteur.Organ.MnemoHardFamilleMO.ToUpper() == "CO")
                            {
                                if (effecteur.Organ.Mnemologique.Contains("COMMUTATEUR_12POSITIONS"))
                                {
                                    List<String> eqs = EquationBuilder.BuildCommutateur12Equation(effecteur, OutputNumber);
                                    if (eqs.Count == 1)
                                    {
                                        suiteEq = eqs[0];
                                    }
                                    else if (eqs.Count == 2)
                                    {
                                        suiteEq = eqs[1];
                                        Equation EqCom12_p0 = new Equation(f);
                                        EqCom12_p0.TextEquation = eqs[0];
                                        f.Equations.Add(EqCom12_p0);
                                        // Changer le nom de la variable
                                        Eq.TextEquation = Eq.TextEquation.Replace(variableBool, EquationBuilder.GetVarByNumOutput(OutputNumber, mode).VarMnemo);
                                    }
                                    else
                                    {
                                        suiteEq = "";
                                    }
                                }
                                else
                                {
                                    suiteEq = EquationBuilder.BuildCommutateurEquation(effecteur, OutputNumber);
                                }
                            }
                            else if (effecteur.Organ.MnemoHardFamilleMO.ToUpper() == "AC")
                            {
                                suiteEq = EquationBuilder.BuildAxeCranEquation(effecteur, OutputNumber);
                            }
                        }
                    }
                    else
                    {
                        // Selecteur
                        suiteEq = EquationBuilder.BuildSelecteurRelayEquation(effecteur, OutputNumber);
                    }

                    if (suiteEq != "")
                    {
                        if (IsFirst)
                        {
                            Eq.TextEquation += suiteEq;
                            IsFirst = false;
                        }
                        else
                        {
                            Eq.TextEquation += " OU " + suiteEq;
                        }
                    }
                }

                if (IsFirst)
                {
                    Result = null;
                }
                else
                {
                    f.Equations.Add(Eq);
                    Result = f;
                } 
            }
            return Result;
        } // endMethod: BuildButtonRelayEquation
        
        /// <summary>
        /// Construire l'équation d'un bouton s'il y a lieu
        /// </summary>
        private static String BuildButtonEquation ( EC_Effecteur button, Int32 OutputNumber )
        {
            String Result = "";
            String EqSuite = "";
            Boolean IsFirst = true;
            
            // 2 - Pour chacune des positions, déterminer si la sortie est sur On ou Off
            Boolean[] Check = new Boolean[button.Positions.Count];
            Int32 NbCheck = 0;

            for (int i = 0; i < button.Positions.Count; i++)
            {
                if (button.Positions[i].Outputs[OutputNumber].IsUsed)
                {
                    NbCheck++;
                    Check[i] = true;
                }
                else
                {
                    Check[i] = false;
                }
            }

            // 3 -> pour chacune des positions, définir l'équation pour cette sortie
            // compléter la formule
            for (int i = 0; i < Check.Length; i++)
            {
                EqSuite = "";
                if (Check[i])
                {
                    switch (i)
                    {
                        case 0:
                            EqSuite = String.Format("{0} == OFF", button.Organ.Mnemologique);
                            break;
                        case 1:
                            EqSuite = String.Format("{0} == SV", button.Organ.Mnemologique);
                            break;
                        case 2:
                            EqSuite = String.Format("{0} == DV", button.Organ.Mnemologique);
                            break;
                        default:
                            EqSuite = "";
                            break;
                    }
                    // L'organe est-il utilisé dans l'interverrouillage bouton?
                    // Si oui, compléter l'équation

                    foreach (var variable in EquationBuilder.DicoVarIVBtn)
                    {
                        if (variable.ButtonMnemoClient == button.Organ.MnemoClient)
                        {
                            EqSuite = "( " + EqSuite + String.Format(" ET {0}", variable.VariableBool.Name) + " )";
                        }
                    }

                    if (IsFirst)
                    {
                        Result = EqSuite;
                        IsFirst = false;
                    }
                    else
                    {
                        Result += " OU " + EqSuite;
                    }
                }
            }

            return Result;
        } // endMethod: BuildButtonEquation

        /// <summary>
        /// Construire l'équation d'un commutateur s'il y a lieu
        /// </summary>
        private static String BuildCommutateurEquation(EC_Effecteur commutateur, Int32 OutputNumber)
        {
            String Result = "";
            String EqSuite = "";
            Boolean IsFirst = true;

            // 2 - Pour chacune des positions, déterminer si la sortie est sur On ou Off
            Boolean[] Check = new Boolean[commutateur.Positions.Count];
            Int32 NbCheck = 0;

            for (int i = 0; i < commutateur.Positions.Count; i++)
            {
                if (commutateur.Positions[i].Outputs[OutputNumber].IsUsed)
                {
                    NbCheck++;
                    Check[i] = true;
                }
                else
                {
                    Check[i] = false;
                }
            }

            // 3 -> pour chacune des positions, définir l'équation pour cette sortie
            // compléter la formule
            for (int i = 0; i < Check.Length; i++)
            {
                EqSuite = "";
                if (Check[i] && Check.Length == 3)
                {
                    switch (i)
                    {
                        case 0:
                            EqSuite = String.Format("{0} == POSITION_0", commutateur.Organ.Mnemologique);
                            break;
                        case 1:
                            EqSuite = String.Format("{0} == POSITION_1", commutateur.Organ.Mnemologique);
                            break;
                        case 2:
                            EqSuite = String.Format("{0} == POSITION_2", commutateur.Organ.Mnemologique);
                            break;
                        default:
                            EqSuite = "";
                            break;
                    }
                }
                else if(Check[i] && Check.Length == 2)
                {
                    switch (i)
                    {
                        case 0:
                            EqSuite = String.Format("{0} == POSITION_1", commutateur.Organ.Mnemologique);
                            break;
                        case 1:
                            EqSuite = String.Format("{0} == POSITION_2", commutateur.Organ.Mnemologique);
                            break;
                        default:
                            EqSuite = "";
                            break;
                    }
                }
                if (EqSuite != "")
                {
                    if (IsFirst)
                    {
                        Result = EqSuite;
                        IsFirst = false;
                    }
                    else
                    {
                        Result += " OU " + EqSuite;
                    } 
                }
            }

            return Result;
        } // endMethod: BuildCommutateurEquation
        
        /// <summary>
        /// Obtenir la prochaine variable booléenne disponible
        /// </summary>
        private static VariableE GetNextVarBool ()
        {
            VariableE Result = null;

            var QueryVar = from variable in PegaseData.Instance.Variables
                           where variable.IsUsedByUser == false && variable.IsUsedByiDialog == false && variable.VarType == typeof(Boolean) && variable.Name.Contains("VARIABLE_BOOLEENNE")
                           select variable;

            if (QueryVar.Count() > 0)
            {
                Result = QueryVar.First();
                Result.IsUsedByiDialog = true;
            }

            return Result;
        } // endMethod: GetNextVarBool

        /// <summary>
        /// Construire l'équation d'un commutateur s'il y a lieu
        /// </summary>
        private static List<String> BuildCommutateur12Equation(EC_Effecteur commutateur, Int32 OutputNumber)
        {
            List<String> Result = new List<String>();
            String EqPos0 = "", EqCom12 = "";
            String EqSuite = "";
            String variable = "";
            Boolean IsFirst = true;
            String Position;

            // 2 - Pour chacune des positions, déterminer si la sortie est sur On ou Off

            for (int i = 0; i < commutateur.Positions.Count; i++)
            {
                EqSuite = "";
                if (commutateur.Positions[i].Outputs[OutputNumber].IsUsed)
                {
                    switch (i)
                    {
                        case 0:
                            variable = EquationBuilder.GetNextVarBool().Name;
                            EqPos0 = String.Format("{0} := ", variable);
                            
                            for (int j = 0; j < 11; j++)
                            {
                                EqPos0 += String.Format("COM12P_{0} != POS_{1:00} ET ", commutateur.Organ.Mnemologique.Substring(commutateur.Organ.Mnemologique.Length - 1), j);
                            }
                            EqPos0 = EqPos0.Substring(0, EqPos0.Length - 4);

                            if (IsFirst)
                            {
                                EqCom12 = String.Format("{0}", variable);
                                IsFirst = false;
                            }
                            else
                            {
                                EqCom12 += " OU " + String.Format("{0}", variable);
                            }
                            break;
                        default:
                            Position = String.Format("== POSITION_{0:00}", i - 1);
                            EqSuite = String.Format("{0} {1}", commutateur.Organ.Mnemologique, Position);
                            if (IsFirst)
                            {
                                EqCom12 = EqSuite;
                                IsFirst = false;
                            }
                            else
                            {
                                EqCom12 += " OU " + EqSuite;
                            }
                            break;
                    }
                }
            }

            if (!IsFirst)
            {
                if (EqPos0 != "")
                {
                    Result.Add(EqPos0);
                }
                Result.Add(EqCom12);
            }

            return Result;
        } // endMethod: BuildCommutateur12Equation

        /// <summary>
        /// Construire l'équation d'un commutateur s'il y a lieu
        /// </summary>
        private static String BuildAxeCranEquation(EC_Effecteur AxeC, Int32 OutputNumber)
        {
            String Result = "";
            String EqSuite = "";
            Boolean IsFirst = true;

            // 2 - Pour chacune des positions, déterminer si la sortie est sur On ou Off
            Boolean[] Check = new Boolean[AxeC.Positions.Count];
            Int32 NbCheck = 0;

            for (int i = 0; i < AxeC.Positions.Count; i++)
            {
                if (AxeC.Positions[i].Outputs[OutputNumber].IsUsed)
                {
                    NbCheck++;
                    Check[i] = true;
                }
                else
                {
                    Check[i] = false;
                }
            }

            // 3 -> pour chacune des positions, définir l'équation pour cette sortie
            // compléter la formule
            Int32 Offset = (9 - Check.Length) / 2;

            for (int i = 0; i < Check.Length; i++)
            {
                EqSuite = "";
                if (Check[i])
                {
                    switch (i + Offset)
                    {
                        //case 0:
                        //    EqSuite = String.Format("{0} == CRAN_MOINS_SIX", AxeC.Organ.Mnemologique);
                        //    break;
                        //case 1:
                        //    EqSuite = String.Format("{0} == CRAN_MOINS_CINQ", AxeC.Organ.Mnemologique);
                        //    break;
                        case 0:
                            EqSuite = String.Format("{0} == CRAN_MOINS_QUATRE", AxeC.Organ.Mnemologique);
                            break;
                        case 1:
                            EqSuite = String.Format("{0} == CRAN_MOINS_TROIS", AxeC.Organ.Mnemologique);
                            break;
                        case 2:
                            EqSuite = String.Format("{0} == CRAN_MOINS_DEUX", AxeC.Organ.Mnemologique);
                            break;
                        case 3:
                            EqSuite = String.Format("{0} == CRAN_MOINS_UN", AxeC.Organ.Mnemologique);
                            break;
                        case 4:
                            EqSuite = String.Format("{0} == CRAN_REPOS", AxeC.Organ.Mnemologique);
                            break;
                        case 5:
                            EqSuite = String.Format("{0} == CRAN_PLUS_UN", AxeC.Organ.Mnemologique);
                            break;
                        case 6:
                            EqSuite = String.Format("{0} == CRAN_PLUS_DEUX", AxeC.Organ.Mnemologique);
                            break;
                        case 7:
                            EqSuite = String.Format("{0} == CRAN_PLUS_TROIS", AxeC.Organ.Mnemologique);
                            break;
                        case 8:
                            EqSuite = String.Format("{0} == CRAN_PLUS_QUATRE", AxeC.Organ.Mnemologique);
                            break;
                        //case 9:
                        //    EqSuite = String.Format("{0} == CRAN_PLUS_CINQ", AxeC.Organ.Mnemologique);
                        //    break;
                        //case 10:
                        //    EqSuite = String.Format("{0} == CRAN_PLUS_SIX", AxeC.Organ.Mnemologique);
                        //    break;
                        default:
                            EqSuite = "";
                            break;
                    }

                    if (EqSuite != "")
                    {
                        if (IsFirst)
                        {
                            Result = EqSuite;
                            IsFirst = false;
                        }
                        else
                        {
                            Result += " OU " + EqSuite;
                        }
                    } 
                }
            }

            return Result;
        } // endMethod: BuildAxeCranEquation

        /// <summary>
        /// Construire les équations de type selecteurs -> relais
        /// </summary>
        private static String BuildSelecteurRelayEquation(EC_Effecteur selecteur, Int32 OutputNumber)
        {
            String Result = "";

            // 1 - Initialiser la fonction
            Boolean IsFirst = true;
            String suiteEq;

            // 1 - pour chacune des positions déterminer si la sortie est sur On ou Off

            for (int i = 0; i < selecteur.Positions.Count; i++)
            {
                suiteEq = "";
                if (selecteur.Positions[i].Outputs[OutputNumber].IsUsed)
                {
                    String SelecteurNum = selecteur.Name.Substring(selecteur.Name.Length - 2);
                    suiteEq = String.Format("SELECTEUR_{0} == SELECTION_{1:00}", SelecteurNum, i + 1);
                }
                if (suiteEq != "")
                {
                    if (IsFirst)
                    {
                        Result = suiteEq;
                        IsFirst = false;
                    }
                    else
                    {
                        Result += " OU " + suiteEq;
                    }
                }
            }
            
            return Result;
        } // endMethod: BuildSelecteurRelayEquation

        /// <summary>
        /// Construire les équations d'interverrouillage -> relais
        /// </summary>
        public static void BuildVerrouEquation( EC_Mode mode )
        {
            //EquationBuilder._numNextVarBool = 0;

            //var QueryVarOTORUsed = from varOTOR in EquationBuilder.DicoVarTOR
            //                       where varOTOR.IsUsed == true
            //                       select varOTOR;

            if (EquationBuilder.DicoVarTOR.Count() > 0)
            {
                for (int i = 0; i < EquationBuilder.DicoVarTOR.Count; i++)
                {
                    // 1 - Initialiser la fonction
                    Boolean IsFirst = true;
                    Formule f = new Formule(mode.RefMode.Position);
                    f.Fonction = "Lock_" + EquationBuilder.DicoVarTOR[i].OTorMnemo;
                    f.Equations.Clear();
                    f.MnemoLogiquePhy = EquationBuilder.DicoVarTOR[i].OTorMnemo;
                    f.FormuleType = TypeFormule.AUTO;
                    f.CommentaireFormule = "Build by Easy Config";

                    String TextEq = String.Format("{0} := {1}", EquationBuilder.DicoVarTOR[i].OTorMnemo, EquationBuilder.DicoVarTOR[i].VarMnemo);

                    foreach (var verrou in mode.Verrouillages)
                    {
                        // 2 - vérifier si la sortie considérée est utilisée dans cette ligne de verrouillage

                        var QueryIsUsed = from output in verrou.Outputs
                                          where output.STOR.MnemoLogique == EquationBuilder.DicoVarTOR[i].OTorMnemo && output.IsUsed
                                          select output;

                        if (QueryIsUsed.Count() > 0)
                        {
                            // 3 - si elle est utilisé, interverrouiller avec les autres sorties sélectionnées

                            var QueryListOutputCheked = from output in verrou.Outputs
                                                        where output.IsUsed == true
                                                        select output;

                            List<EC_Output> listOutputChecked = QueryListOutputCheked.ToList<EC_Output>();

                            foreach (var output in listOutputChecked)
                            {
                                if (output.STOR.MnemoLogique != EquationBuilder.DicoVarTOR[i].OTorMnemo)
                                {
                                    String VarBool = EquationBuilder.FindVarBool(output.STOR.MnemoLogique, EquationBuilder.DicoVarTOR.ToList<VariableSortieTOR>());
                                    if (VarBool != "")
                                    {
                                        if (IsFirst)
                                        {
                                            TextEq += String.Format(" ET (PAS {0}", VarBool);
                                            IsFirst = false;
                                        }
                                        else
                                        {
                                            TextEq += String.Format(" ET PAS {0}", VarBool);
                                        }
                                    }
                                }
                            }
                        }
                    } // end foreach : mode.Verrouillages
                    if (!IsFirst)
                    {
                        TextEq += ")";
                    }
                    Equation Eq = new Equation(f);
                    Eq.TextEquation = TextEq;
                    f.Equations.Add(Eq);

                    EquationBuilder.AddFormule(f, mode);
                } // end for i
            } // end if : QueryVarOTORUsed
        } // endMethod: BuildVerrouEquation

        /// <summary>
        /// Trouver la correspondance entre un relais et la variable booléenne liée
        /// </summary>
        private static String FindVarBool ( String VarOTOR, List<VariableSortieTOR> listVar )
        {
            String Result = "";

            var Query = from variable in listVar
                        where variable.OTorMnemo == VarOTOR
                        select variable;

            if (Query.Count() > 0)
            {
                Result = Query.First().VarMnemo;
            }

            return Result;
        } // endMethod: FindVarBool
        
        /// <summary>
        /// Trouver la correspondance entre une variable booléenne et la sortie TOR liée
        /// </summary>
        private static String FindOTOR ( String VarBool, List<VariableSortieTOR> listVar )
        {
            String Result = "";

            var Query = from variable in listVar
                        where variable.VarMnemo == VarBool
                        select variable;

            if (Query.Count() > 0)
            {
                Result = Query.First().OTorMnemo;
            }

            return Result;
        } // endMethod: FindOTOR
        
        /// <summary>
        /// Construire les équations pour les sorties ana
        /// </summary>
        public static void BuildSAnaEquation ( EC_Mode mode )
        {
            EquationBuilder._numNextVarNum = EquationBuilder._numEndUniversalModeVarNum;

            if (_variablesEquation != null)
            {
                // 2 - pour toute les sorties anas pour le mode
                foreach (EC_SortieAna sana in mode.SortiesAna)
                {
                    if (sana.IsAssociated)
                    {
                        // si la sortie est associée -> déterminer le type de formule à produire
                        if (sana.LinkedOrganAna.Type == TypeOrgaAna.Joystick )
                        {
                            if (sana.IsCourbePositive)
                            {
                                // Equation courbe positive
                                EquationBuilder.BuildEquationJoystickCPositive(mode, sana, _variablesEquation);
                            }
                            else
                            {
                                // Equation courbe négative
                                EquationBuilder.BuildEquationJoystickCNegative(mode, sana, _variablesEquation);
                            }
                        }
                        else
                        {
                            // Equation émulation joystick
                            EquationBuilder.BuildEquationEmulJoystick(mode, sana, _variablesEquation);
                        }
                    }
                }
            }
        } // endMethod: BuildJoystickEquation

        /// <summary>
        /// Construire une équation Joystick en mode courbe positive
        /// </summary>
        public static void BuildEquationJoystickCNegative ( EC_Mode mode, EC_SortieAna sana, List<VariableE> variablesEquation )
        {
            Formule f = new Formule(mode.RefMode.Position);
            if (sana.SAna != null)
            {
                f.MnemoLogiquePhy = sana.SAna.MnemoLogique; 
            }
            else if (sana.SPWM != null)
            {
                f.MnemoLogiquePhy = sana.SPWM.MnemoLogique;
            }
            f.CommentaireFormule = "Build by Easy Config";
            f.Commandes = "Assign Ana Axis -> Build by Easy Config";
            f.Fonction = "";
            f.Equations.Clear();
            if (sana.SAna != null)
            {
                f.Fonction = sana.SAna.MnemoHardware + " -> " + sana.SAna.MnemoClient; 
            }
            else if(sana.SPWM != null)
            {
                f.Fonction = sana.SPWM.MnemoHardware + " -> " + sana.SPWM.MnemoClient;
            }
            f.FormuleType = TypeFormule.AUTO;

            // 1 - calcul de la pente
            Double BMax = 1, BMin = 0, VZero = 0;

            if (sana.SAna != null)
            {
                BMax = (Int32)((1023f / (sana.SAna.ValUIMax - sana.SAna.ValUIMin)) * ((float)sana.BorneMax - sana.SAna.ValUIMin));
                BMin = (Int32)((1023f / (sana.SAna.ValUIMax - sana.SAna.ValUIMin)) * ((float)sana.BorneMin - sana.SAna.ValUIMin));
                VZero = (Int32)((1023f / (sana.SAna.ValUIMax - sana.SAna.ValUIMin)) * ((float)sana.ValZero - sana.SAna.ValUIMin)); 
            }
            else if (sana.SPWM != null)
            {
                BMax = sana.BorneMax * 10;
                BMin = sana.BorneMin * 10;
                VZero = BMin;
            }

            Double BMmBm = BMax - BMin;
            String EqAna = "";
            Equation Eq;
            Int32 NumSortiePWM;
            String VarNumPWM = "";

            // 2 - équation de reprise après sortie en sécurité
            if (sana.SAna != null)
            {
                if (sana.SAna.ValeurEnSecuritePercent != "")
                {
                    EqAna = String.Format("{0} := BOOLEEN_SYSTEME_01 ? {1} : {0}", sana.SAna.MnemoLogique, sana.SAna.ValeurEnSecurite);
                    Eq = new Equation(f);
                    Eq.TextEquation = EqAna;
                    f.Equations.Add(Eq);
                } 
            }
            else if(sana.SPWM != null)
            {
                NumSortiePWM = Convert.ToInt32(sana.SPWM.MnemoHardware.Substring(1, 1));
                VarNumPWM = String.Format("VARIABLE_NUMERIQUE_{0:00}", 26 + NumSortiePWM);
                EqAna = String.Format("{0} := BOOLEEN_SYSTEME_01 ? {1} : {0}", VarNumPWM, sana.SPWM.PWMCyclique * 10);
                Eq = new Equation(f);
                Eq.TextEquation = EqAna;
                f.Equations.Add(Eq);
            }

            // 3 - détermination de l'équation de calcul
            String EqLimit = "";

            if (sana.SAna != null)
            {
                EqAna = String.Format("{0} := ({1} - {2}) * {3} / 200 + {4}", sana.SAna.MnemoLogique, BMax, BMin, sana.LinkedOrganAna.ListJoysticks[sana.LinkedOrganAna.CurrentJoystick].Mnemologique, VZero);
                EqLimit = String.Format("{0} := {0} > 1023 ? 1023 : {0} < 0 ? 0 : {0}", sana.SAna.MnemoLogique);
            }
            else if(sana.SPWM != null)
            {
                EqAna = String.Format("{0} := ({1} - {2}) * ({3} + 100) / 200 + {4}", VarNumPWM, BMax, BMin, sana.LinkedOrganAna.ListJoysticks[sana.LinkedOrganAna.CurrentJoystick].Mnemologique, VZero);
                EqLimit = String.Format("{0} := {0} > 1023 ? 1023 : {0} < 0 ? 0 : {0}", VarNumPWM);
            }

            // 4 - ajouter toutes les équations
            Eq = new Equation(f);
            Eq.TextEquation = EqAna;
            f.Equations.Add(Eq);

            Eq = new Equation(f);
            Eq.TextEquation = EqLimit;
            f.Equations.Add(Eq);

            // 5 - PWM - TIMO
            if (sana.SPWM != null)
            {
                Eq = new Equation(f);
                Eq.TextEquation = String.Format("{0} := {1} > 0 ? VRAI : FAUX", sana.SPWM.MnemoLogique, VarNumPWM);
                f.Equations.Add(Eq);
            }

            EquationBuilder.AddFormule(f, mode);
        } // endMethod: BuildEquationJoystickCNegative

        /// <summary>
        /// Construire une équation Joystick en mode courbe negative
        /// </summary>
        public static void BuildEquationJoystickCPositive(EC_Mode mode, EC_SortieAna sana, List<VariableE> variablesEquation)
        {
            Formule f = new Formule(mode.RefMode.Position);
            if (sana.LinkedOrganAna.CurrentJoystick > -1)
            {
                if (sana.SAna != null)
                {
                    f.MnemoLogiquePhy = sana.SAna.MnemoLogique;
                }
                else if (sana.SPWM != null)
                {
                    f.MnemoLogiquePhy = sana.SPWM.MnemoLogique;
                }
                f.CommentaireFormule = "Build by Easy Config";
                f.Commandes = "Assign Ana Axis -> Build by Easy Config";
                f.Fonction = "";
                f.Equations.Clear();
                if (sana.SAna != null)
                {
                    f.Fonction = "OAna -> " + sana.SAna.MnemoClient;
                }
                else if (sana.SPWM != null)
                {
                    f.Fonction = sana.SPWM.MnemoHardware + " -> " + sana.SPWM.MnemoClient;
                }
                f.FormuleType = TypeFormule.AUTO;

                // 1 - calcul de la pente
                Double BMax = 1, BMin = 0, VZero = 0;

                if (sana.SAna != null)
                {
                    BMax = (Int32)((1023f / (sana.SAna.ValUIMax - sana.SAna.ValUIMin)) * ((float)sana.BorneMax - sana.SAna.ValUIMin));
                    BMin = (Int32)((1023f / (sana.SAna.ValUIMax - sana.SAna.ValUIMin)) * ((float)sana.BorneMin - sana.SAna.ValUIMin));
                    VZero = (Int32)((1023f / (sana.SAna.ValUIMax - sana.SAna.ValUIMin)) * ((float)sana.ValZero - sana.SAna.ValUIMin));
                }
                else if (sana.SPWM != null)
                {
                    BMax = sana.BorneMax * 10;
                    BMin = sana.BorneMin * 10;
                    VZero = BMin;
                }

                Double BMmBm = BMax - BMin;
                String EqAna;
                Equation Eq;
                Int32 NumSortiePWM;
                String VarNumPWM = "";

                // 2 - équation de reprise après sortie en sécurité
                if (sana.SAna != null)
                {
                    if (sana.SAna.ValeurEnSecuritePercent != "")
                    {
                        if (mode.RefMode.Position != 32)
                        {
                            EqAna = String.Format("{0} := BOOLEEN_SYSTEME_02 ? {1} : {0}", sana.SAna.MnemoLogique, sana.SAna.ValeurEnSecurite); 
                        }
                        else
                        {
                            EqAna = String.Format("{0} := BOOLEEN_SYSTEME_01 ? {1} : {0}", sana.SAna.MnemoLogique, sana.SAna.ValeurEnSecurite); 
                        }

                        Eq = new Equation(f);
                        Eq.TextEquation = EqAna;
                        f.Equations.Add(Eq);
                    }
                }
                else if (sana.SPWM != null)
                {
                    NumSortiePWM = Convert.ToInt32(sana.SPWM.MnemoHardware.Substring(1, 1));
                    VarNumPWM = String.Format("VARIABLE_NUMERIQUE_{0:00}", 26 + NumSortiePWM);
                    EqAna = String.Format("{0} := BOOLEEN_SYSTEME_01 ? {1} : {0}", VarNumPWM, sana.SPWM.PWMCyclique * 10);
                    Eq = new Equation(f);
                    Eq.TextEquation = EqAna;
                    f.Equations.Add(Eq);
                }

                // 3 - détermination de l'équation de calcul
                EqAna = "";
                String EqLimit = "";
                if (sana.SAna != null)
                {
                    EqAna = String.Format("{0} := {1} > 0 ? {1} * ({2} - {3}) / 100 + {4} : -{1} * ({2} - {3}) / 100 + {4}", sana.SAna.MnemoLogique, sana.LinkedOrganAna.ListJoysticks[sana.LinkedOrganAna.CurrentJoystick].Mnemologique, BMax, BMin, VZero);
                    EqLimit = String.Format("{0} := {0} > 1023 ? 1023 : {0} < 0 ? 0 : {0}", sana.SAna.MnemoLogique);
                }
                else if (sana.SPWM != null)
                {
                    EqAna = String.Format("{0} := {1} > 0 ? {1} * ({2} - {3}) / 100 + {4} : -{1} * ({2} - {3}) / 100 + {4}", VarNumPWM, sana.LinkedOrganAna.ListJoysticks[sana.LinkedOrganAna.CurrentJoystick].Mnemologique, BMax, BMin, VZero);
                    EqLimit = String.Format("{0} := {0} > 1023 ? 1023 : {0} < 0 ? 0 : {0}", VarNumPWM);
                }

                // 4 - ajouter toutes les équations
                Eq = new Equation(f);
                Eq.TextEquation = EqAna;
                f.Equations.Add(Eq);

                Eq = new Equation(f);
                Eq.TextEquation = EqLimit;
                f.Equations.Add(Eq);

                // 5 - PWM - TIMO
                if (sana.SPWM != null)
                {
                    Eq = new Equation(f);
                    Eq.TextEquation = String.Format("{0} := {1} > 0 ? VRAI : FAUX", sana.SPWM.MnemoLogique, VarNumPWM);
                    f.Equations.Add(Eq);
                }

                EquationBuilder.AddFormule(f, mode);  
            }
        } // endMethod: BuildEquationJoystickCPositive

        /// <summary>
        /// Construire une équation émulation du joystick
        /// </summary>
        public static void BuildEquationEmulJoystick ( EC_Mode mode, EC_SortieAna sana, List<VariableE> variablesEquation )
        {
            Formule f = new Formule(mode.RefMode.Position);
            if (sana.SAna != null)
            {
                f.MnemoLogiquePhy = sana.SAna.MnemoLogique; 
            }
            else if (sana.SPWM != null)
            {
                f.MnemoLogiquePhy = sana.SPWM.MnemoLogique;
            }
            f.CommentaireFormule = "Build by Easy Config";
            f.Commandes = "Assign Ana Button -> Build by Easy Config";
            f.Fonction = "";
            f.Equations.Clear();

            if (sana.SAna != null)
            {
                f.Fonction = "OAna -> " + sana.SAna.MnemoClient; 
            }
            else if (sana.SPWM != null)
            {
                f.Fonction = "OAna -> " + sana.SPWM.MnemoClient;
            }

            f.FormuleType = TypeFormule.AUTO;

            ObservableCollection<VariableE> FreeBooleean = new ObservableCollection<VariableE>();
            Int32 FreeBooleanNum = 0;

            // 1 - Lister les variables booléennes disponibles
            var Query = from variable in PegaseData.Instance.Variables
                        where variable.IsUsedByUser == false && variable.IsUsedByiDialog == false && variable.Name.Contains("VARIABLE_BOOLEENNE")
                        select variable;

            if (Query.Count() > 0)
            {
                foreach (var variable in Query)
                {
                    FreeBooleean.Add(variable);
                }

                // Supprimer les variables déja associées

                foreach (var variable in EquationBuilder.DicoVarTOR)
                {
                    var QueryV = from v in FreeBooleean
                                 where v.Name == variable.VarMnemo
                                 select v;

                    if (variable.IsUsed)
                    {
                        if (QueryV.Count() > 0)
                        {
                            VariableE v = QueryV.First();
                            FreeBooleean.Remove(v);
                        }
                    }
                }
            }

            // 2 - Ecrire les équations
            String EqAna;
            Equation Eq;

            String MnemoButtonInc = "", VarBoolInc, EtatBoutonInc = "", Pas, VarNumPWM;
            String MnemoButtonDec = "", VarBoolDec, EtatBoutonDec = "";

            Pas = sana.LinkedOrganAna.IncrementJoystick.ToString();
            VarNumPWM = variablesEquation[EquationBuilder._numNextVarNum].Name;

            EquationBuilder._numNextVarNum++;

            if (mode.RefMode.Position == 32)
            {
                EquationBuilder._numEndUniversalModeVarNum = EquationBuilder._numNextVarNum;
            }

            VarBoolInc = FreeBooleean[FreeBooleanNum].Name;
            FreeBooleanNum++;
            VarBoolDec = FreeBooleean[FreeBooleanNum].Name;
            FreeBooleanNum++;

            var QueryVar = from variable in PegaseData.Instance.Variables
                           where variable.VarType == typeof(Boolean) && (variable.Name.Contains(VarBoolInc) || variable.Name.Contains(VarBoolDec))
                           select variable;

            if (QueryVar.Count() > 0)
            {
                foreach (VariableE result in QueryVar)
                {
                    result.IsUsedByiDialog = true;
                    if (mode.RefMode.Position == 32)
                    {
                        result.IsUsedByUniversalMode = true;
                    }
                }
            }

            sana.LinkedOrganAna.GetListTextButtonPos(sana.LinkedOrganAna.ListTextButton[sana.LinkedOrganAna.CurrentBtnIncrement], out MnemoButtonInc, out EtatBoutonInc);
            sana.LinkedOrganAna.GetListTextButtonPos(sana.LinkedOrganAna.ListTextButton[sana.LinkedOrganAna.CurrentBtnDecrement], out MnemoButtonDec, out EtatBoutonDec);

            Double BMax = 1, BMin = 0;

            if (sana.SAna != null)
            {
                BMax = (Int32)((1023f / (sana.SAna.ValUIMax - sana.SAna.ValUIMin)) * ((float)sana.BorneMax - sana.SAna.ValUIMin));
                BMin = (Int32)((1023f / (sana.SAna.ValUIMax - sana.SAna.ValUIMin)) * ((float)sana.BorneMin - sana.SAna.ValUIMin));

                if (sana.SAna.ValeurEnSecuritePercent != "")
                {
                    if (mode.RefMode.Position != 32)
                    {
                        EqAna = String.Format("{0} := BOOLEEN_SYSTEME_02 ? {1} : {0}", VarNumPWM, sana.SAna.ValeurEnSecurite); 
                    }
                    else
                    {
                        EqAna = String.Format("{0} := BOOLEEN_SYSTEME_01 ? {1} : {0}", VarNumPWM, sana.SAna.ValeurEnSecurite);
                    }
                    Eq = new Equation(f);
                    Eq.TextEquation = EqAna;
                    f.Equations.Add(Eq);
                }
            }
            else if (sana.SPWM != null)
            {
                BMax = sana.BorneMax * 10;
                BMin = sana.BorneMin * 10;

                Int32 NumSortiePWM = Convert.ToInt32(sana.SPWM.MnemoHardware.Substring(1, 1));
                VarNumPWM = String.Format("VARIABLE_NUMERIQUE_{0:00}", 26 + NumSortiePWM);

                if (sana.SPWM.PWMCyclique > 0 )
                {
                    if (mode.RefMode.Position != 32)
                    {
                        EqAna = String.Format("{0} := BOOLEEN_SYSTEME_02 ? {1} : {0}", VarNumPWM, sana.SPWM.PWMCyclique * 10); 
                    }
                    else
                    {
                        EqAna = String.Format("{0} := BOOLEEN_SYSTEME_01 ? {1} : {0}", VarNumPWM, sana.SPWM.PWMCyclique * 10);
                    }
                    Eq = new Equation(f);
                    Eq.TextEquation = EqAna;
                    f.Equations.Add(Eq);
                }
            }
            
            String EqAffectEtatButtonInc = String.Format("{0} := {1} == {2} ET {3} ? {0} + {4} : {0}", VarNumPWM, MnemoButtonInc, EtatBoutonInc, VarBoolInc, Pas);
            String EqStopInc = String.Format("{0} := {1} != {2} ? VRAI : FAUX", VarBoolInc, MnemoButtonInc, EtatBoutonInc);

            String EqAffectEtatButtonDec = String.Format("{0} := {1} == {2} ET {3} ? {0} - {4} : {0}", VarNumPWM, MnemoButtonDec, EtatBoutonDec, VarBoolDec, Pas);
            String EqStopDec = String.Format("{0} := {1} != {2} ? VRAI : FAUX", VarBoolDec, MnemoButtonDec, EtatBoutonDec);
            String EqLimitInc = String.Format("{0} := {0} > {1} ? {1} : {0}", VarNumPWM, BMax);
            String EqLimitDec = String.Format("{0} := {1} < {2} ? {2} : {1}", VarNumPWM, VarNumPWM, BMin);
            String EqFinal = "";
            if (sana.SAna != null)
            {
                EqFinal = String.Format("{0} := {1}", sana.SAna.MnemoLogique, VarNumPWM); 
            }
            else if (sana.SPWM != null)
            {
                EqFinal = ""; // String.Format("{0} := {1}", sana.SPWM.MnemoLogique, VarNum); 
            }

            Eq = new Equation(f);
            Eq.TextEquation = EqAffectEtatButtonInc;
            f.Equations.Add(Eq);

            Eq = new Equation(f);
            Eq.TextEquation = EqStopInc;
            f.Equations.Add(Eq);

            Eq = new Equation(f);
            Eq.TextEquation = EqLimitInc;
            f.Equations.Add(Eq);

            Eq = new Equation(f);
            Eq.TextEquation = EqAffectEtatButtonDec;
            f.Equations.Add(Eq);

            Eq = new Equation(f);
            Eq.TextEquation = EqStopDec;
            f.Equations.Add(Eq);

            Eq = new Equation(f);
            Eq.TextEquation = EqLimitDec;
            f.Equations.Add(Eq);

            if (EqFinal != "")
            {
                Eq = new Equation(f);
                Eq.TextEquation = EqFinal;
                f.Equations.Add(Eq); 
            }

            // PWM - TIMO
            if (sana.SPWM != null)
            {
                Eq = new Equation(f);
                Eq.TextEquation = String.Format("{0} := {1} > 0 ? VRAI : FAUX", sana.SPWM.MnemoLogique, VarNumPWM);
                f.Equations.Add(Eq);
            }

            EquationBuilder.AddFormule(f, mode);
        } // endMethod: BuildEquationEmulJoystick
        
        /// <summary>
        /// Construire les équations pour les entrées du réseau
        /// </summary>
        public static void BuildReseauInput(ObservableCollection<EC_VarNetwork> varNetWork, EC_Mode modeUniversel)
        {
            Formule f = new Formule(MODE_UNIVERSELLE);
            f.Commandes = "InputNetwork";
            f.CommentaireFormule = "Build by Easy Config";
            f.Equations.Clear();
            f.Fonction = "Assign Input Network";
            f.FormuleType = TypeFormule.AUTO;
            f.MnemoLogiquePhy = "";

            foreach (var variable in varNetWork)
            {
                if (variable.CurrentVariable != null && variable.CurrentVariable.Name != "")
                {
                    var Query = from vari in PegaseData.Instance.ModuleT.ETORS
                                where vari.MnemoLogique == variable.VariableInternal
                                select vari;

                    if (Query.Count() == 0)
                    {
                        Query = from vari in PegaseData.Instance.ModuleT.STORS
                                where vari.MnemoLogique == variable.VariableInternal
                                select vari;
                    }

                    Equation eq = new Equation(f);

                    if (Query.Count() > 0)
                    {
                        ESTOR esTor = Query.First();

                        if (!esTor.IsPWM)
                        {
                            eq.TextEquation = String.Format("{0} := {1}", variable.VariableInternal, variable.VariableNetwork);
                            foreach (VariableE lockvar in PegaseData.Instance.Variables)
                            {
                                if (lockvar.Name == variable.VariableInternal)
                                {
                                    lockvar.IsUsedByUniversalMode = true;
                                }
                            }
                        }
                        else
                        {
                            Int32 Num = Convert.ToInt32(esTor.MnemoHardware.Substring(1, 1));
                            eq.TextEquation = String.Format("VARIABLE_NUMERIQUE_{0:00} := {1}", 26 + Num, variable.VariableNetwork);
                            f.Equations.Add(eq);
                            eq = new Equation(f);
                            eq.TextEquation = String.Format("{0} := VARIABLE_NUMERIQUE_{1:00} > 0 ? VRAI : FAUX", variable.VariableInternal, 26 + Num);
                            string alocker = String.Format("VARIABLE_NUMERIQUE_{0:00}", 26 + Num);
                            foreach (VariableE lockvar in PegaseData.Instance.Variables)
                            {
                                if ((lockvar.Name == variable.VariableInternal)||(lockvar.Name == alocker))
                                {
                                    lockvar.IsUsedByUniversalMode = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        eq.TextEquation = String.Format("{0} := {1}", variable.VariableInternal, variable.VariableNetwork);
                        foreach (VariableE lockvar in PegaseData.Instance.Variables)
                        {
                            if ((lockvar.Name == variable.VariableInternal))
                            {
                                lockvar.IsUsedByUniversalMode = true;
                            }
                        }
                    }
                    f.Equations.Add(eq);
                }
            }

            if (f.Equations.Count > 0)
            {
                EquationBuilder.AddFormule(f, modeUniversel);
            }
        } // endMethod: BuildReseauInput

        /// <summary>
        /// Construire les équations pour les sorties du réseau
        /// </summary>
        public static void BuildReseauOutput ( ObservableCollection<EC_VarNetwork> varNetWork, EC_Mode modeUniversel )
        {
            Formule f = new Formule(MODE_UNIVERSELLE);
            f.Commandes = "OutputNetwork";
            f.CommentaireFormule = "Build by Easy Config";
            f.Equations.Clear();
            f.Fonction = "Assign Output Network";
            f.FormuleType = TypeFormule.AUTO;
            f.MnemoLogiquePhy = "";

            foreach (var variable in varNetWork)
            {
                if (variable.CurrentVariable != null && variable.CurrentVariable.Name != "")
                {
                    var Query = from vari in PegaseData.Instance.ModuleT.ETORS
                                where vari.MnemoLogique == variable.VariableInternal
                                select vari;

                    if (Query.Count() == 0)
                    {
                        Query = from vari in PegaseData.Instance.ModuleT.STORS
                                    where vari.MnemoLogique == variable.VariableInternal
                                    select vari;
                    }

                    Equation eq = new Equation(f);

                    if (Query.Count() > 0)
	                {
		                ESTOR esTor = Query.First();

                        if(!esTor.IsPWM)
                        {
                            eq.TextEquation = String.Format("{0} := {1}", variable.VariableNetwork, variable.VariableInternal);
                            foreach (VariableE lockvar in PegaseData.Instance.Variables)
                            {
                                if ((lockvar.Name == variable.VariableInternal) )
                                {
                                    lockvar.IsUsedByUniversalMode = true;
                                }
                            }
                        }
                        else
                        {
                            Int32 Num = Convert.ToInt32(esTor.MnemoHardware.Substring(1,1));
                            eq.TextEquation = String.Format("{0} := VARIABLE_NUMERIQUE_{1:00}", variable.VariableNetwork, 26 + Num);
                            string alocker = String.Format("VARIABLE_NUMERIQUE_{1:00}", 26 + Num);
                            foreach (VariableE lockvar in PegaseData.Instance.Variables)
                            {
                                if ((lockvar.Name == variable.VariableInternal) || (lockvar.Name == alocker))
                                {
                                    lockvar.IsUsedByUniversalMode = true;
                                }
                            }
                        } 
	                }
                    else
                    {
                        eq.TextEquation = String.Format("{0} := {1}", variable.VariableNetwork, variable.VariableInternal);
                        foreach (VariableE lockvar in PegaseData.Instance.Variables)
                        {
                            if ((lockvar.Name == variable.VariableInternal))
                            {
                                lockvar.IsUsedByUniversalMode = true;
                            }
                        }
                    }
                    f.Equations.Add(eq);
                }
            }

            if (f.Equations.Count > 0)
            {
                EquationBuilder.AddFormule(f, modeUniversel);
            }
        } // endMethod: BuildReseauOutput

        /// <summary>
        /// Construire les équations du mode sécurité
        /// </summary>
        public static void BuildSecurityEquation(EC_Mode modeSecu)
        {
            //Formule f = new Formule(Constantes.MODE_SECU);
            //f.Commandes = "AutoPWM";
            //f.EtatFormule = FormuleState.NON_EVALUEE;
            //f.Fonction = "Restaurer le PWM à ces valeurs par défaut";
            //f.FormuleType = TypeFormule.AUTO;
            //f.Equations.Clear();

            //// Ecrire les équations PWM
            //foreach (var sana in modeSecu.SortiesAna)
            //{
            //    if (sana.SPWM != null && sana.SPWM.ValeurEnSecurite == "")
            //    {
            //        Int32 Num = Convert.ToInt32(sana.SPWM.MnemoHardware.Substring(1, 1));
            //        Equation eq = new Equation(f);
            //        eq.TextEquation = String.Format("VARIABLE_NUMERIQUE_{0:00} := {1}", 26 + Num, sana.SPWM.PWMCyclique * 10);
            //        f.Equations.Add(eq);
            //        eq = new Equation(f);
            //        eq.TextEquation = String.Format("{0} := VARIABLE_NUMERIQUE_{1:00} > 0 ? VRAI : FAUX", sana.SPWM.MnemoLogique, 26 + Num);
            //        f.Equations.Add(eq);
            //    }
            //}

            //if (f.Equations.Count > 0)
            //{
            //    PegaseData.Instance.OLogiciels.ModeSecurite.Formules.Add(f);
            //}
        } // endMethod: BuildSecurity

        #endregion

        // Messages
        #region Messages

        #endregion
    } // endClass: EquationBuilder
}
