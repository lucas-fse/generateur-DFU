using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Pegase.CompilEquation;
using Mvvm = GalaSoft.MvvmLight;

namespace JAY.PegaseCore
{
    // Enumération définissant les différents états d'une équation
    public enum EquationState
    {
        NON_EVALUEE,
        ERREUR,
        OK
    }

    /// <summary>
    /// Classe contenant toutes les données et fonctions pour éditer une équation
    /// </summary>
    public class Equation : Mvvm.ViewModelBase
    {
        // Variables
        #region Variables

        private String _textEquation;
        private ResultatCompilEquation _resultatCompil;
        private EquationState _etatEquation;
        private Formule _formuleParent;
        private String _textErreur;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La formule parent
        /// </summary>
        public Formule FormuleParent
        {
            get
            {
                return this._formuleParent;
            }
            set
            {
                this._formuleParent = value;
            }
        } // endProperty: FormuleParent

        /// <summary>
        /// La mnémonique logique lié à l'équation
        /// ex: VARIABLE_BOOLEENNE_01
        /// </summary>
        public String MnemoLogique
        {
            get;
            private set;
        } // endProperty: MnemoLogique
        
        /// <summary>
        /// Le texte de l'équation
        /// ex : VARIABLE_BOOLEENNE_01 := (BOUTON_11 != PAS_D_APPUI)
        /// </summary>
        public String TextEquation 
        {
            get
            {
                return this._textEquation;
            }
            set
            {
                this._textEquation = value;
                // Récupérer la mnemologique
                if (this._textEquation.Contains(":="))
                {
                    String[] parts = this._textEquation.Split(new String[] { ":=" }, StringSplitOptions.RemoveEmptyEntries);
                    this.MnemoLogique = parts[0].Trim();
                }
                // Marquer l'équation comme non evaluée
                this.EtatEquation = EquationState.NON_EVALUEE;
                this.TextErreur = "";
            }
        } // endProperty: Equation

        /// <summary>
        /// L'état de l'équation en cours
        /// </summary>
        public EquationState EtatEquation
        {
            get
            {
                return this._etatEquation;
            }
            private set
            {
                this._etatEquation = value;
                if (value == EquationState.NON_EVALUEE)
                {
                    this.FormuleParent.EtatFormule = FormuleState.NON_EVALUEE;
                }
                RaisePropertyChanged("EtatEquation");
            }
        } // endProperty: EtatEquation

        /// <summary>
        /// Le texte de l'erreur et la position
        /// </summary>
        public String TextErreur
        {
            get
            {
                return this._textErreur;
            }
            set
            {
                this._textErreur = value;
                RaisePropertyChanged("TextErreur");
            }
        } // endProperty: TextErreur

        /// <summary>
        /// Le résultat de la compilation de l'équation
        /// </summary>
        public ResultatCompilEquation ResultatCompil
        {
            get
            {
                return this._resultatCompil;
            }
            private set
            {
                this._resultatCompil = value;

                if (this._resultatCompil == null)
                {
                    this.EtatEquation = EquationState.NON_EVALUEE;
                    this.TextErreur = "";
                }
                else
                {
                    switch (this._resultatCompil.Diagnostique)
                    {
                        case Pegase.CompilEquation.DiagnosticCompilEquation_e.EXPRESSION_CORRECTE:
                            this.EtatEquation = EquationState.OK;
                            this.TextErreur = "";
                            break;
                        case Pegase.CompilEquation.DiagnosticCompilEquation_e.PAS_DE_DIAGNOSTIC:
                            this.TextErreur = String.Format("Err (pos {0}) : Pas de diagnostic", value.Position);
                            this.EtatEquation = EquationState.ERREUR;
                            break;
                        case Pegase.CompilEquation.DiagnosticCompilEquation_e.NOM_INCONNU:
                            this.TextErreur = String.Format("Err (pos {0}) : Nom inconnu", value.Position);
                            this.EtatEquation = EquationState.ERREUR;
                            break;
                        case Pegase.CompilEquation.DiagnosticCompilEquation_e.NOM_INCORRECT:
                            this.TextErreur = String.Format("Err (pos {0}) : Nom incorrect", value.Position);
                            this.EtatEquation = EquationState.ERREUR;
                            break;
                        case Pegase.CompilEquation.DiagnosticCompilEquation_e.EQUATION_VIDE:
                            this.TextErreur = String.Format("Err (pos {0}) : Equation vide", value.Position);
                            this.EtatEquation = EquationState.ERREUR;
                            break;
                        case Pegase.CompilEquation.DiagnosticCompilEquation_e.PARENTHESE_FERMANTE_MANQUANTE:
                            this.TextErreur = String.Format("Err (pos {0}) : Parenthèse fermante manquante", value.Position);
                            this.EtatEquation = EquationState.ERREUR;
                            break;
                        case Pegase.CompilEquation.DiagnosticCompilEquation_e.MANQUE_ESPACE_APRES_PARENTHESE_FERMANTE:
                            this.TextErreur = String.Format("Err (pos {0}) : Manque espace après parenthèse fermante", value.Position);
                            this.EtatEquation = EquationState.ERREUR;
                            break;
                        case Pegase.CompilEquation.DiagnosticCompilEquation_e.EXTRA_CHAR_AFTER_EXPR:
                            this.TextErreur = String.Format("Err (pos {0}) : Charactère en plus après l'expression", value.Position);
                            this.EtatEquation = EquationState.ERREUR;
                            break;
                        case Pegase.CompilEquation.DiagnosticCompilEquation_e.OPERATEUR_CONDITIONNEL_INCOMPLET:
                            this.TextErreur = String.Format("Err (pos {0}) : Operateur conditionnel incomplet", value.Position);
                            this.EtatEquation = EquationState.ERREUR;
                            break;
                        case Pegase.CompilEquation.DiagnosticCompilEquation_e.NOMBRE_DECIMAL_INCORRECT:
                            this.TextErreur = String.Format("Err (pos {0}) : Nombre décimal incorrect", value.Position);
                            this.EtatEquation = EquationState.ERREUR;
                            break;
                        case Pegase.CompilEquation.DiagnosticCompilEquation_e.AGE_INDISPONIBLE:
                            this.TextErreur = String.Format("Err (pos {0}) : Age indisponible", value.Position);
                            this.EtatEquation = EquationState.ERREUR;
                            break;
                        case Pegase.CompilEquation.DiagnosticCompilEquation_e.OPERANDE_NON_BINAIRE:
                            this.TextErreur = String.Format("Err (pos {0}) : Operande non binaire", value.Position);
                            this.EtatEquation = EquationState.ERREUR;
                            break;
                        case Pegase.CompilEquation.DiagnosticCompilEquation_e.OPERANDE_NON_NUMERIQUE:
                            this.TextErreur = String.Format("Err (pos {0}) : Opérande non numérique", value.Position);
                            this.EtatEquation = EquationState.ERREUR;
                            break;
                        case Pegase.CompilEquation.DiagnosticCompilEquation_e.OPERANDES_DE_TYPES_INCOMPATIBLES:
                            this.TextErreur = String.Format("Err (pos {0}) : Opérandes de types incompatibles", value.Position);
                            this.EtatEquation = EquationState.ERREUR;
                            break;
                        case Pegase.CompilEquation.DiagnosticCompilEquation_e.OPERANDES_DE_FAMILLES_INCOMPATIBLES:
                            this.TextErreur = String.Format("Err (pos {0}) : Opérandes de familles incompatibles", value.Position);
                            this.EtatEquation = EquationState.ERREUR;
                            break;
                        case Pegase.CompilEquation.DiagnosticCompilEquation_e.OPERANDES_DE_TYPES_DIFFERENTS:
                            this.TextErreur = String.Format("Err (pos {0}) : Opérandes de types différents", value.Position);
                            this.EtatEquation = EquationState.ERREUR;
                            break;
                        case Pegase.CompilEquation.DiagnosticCompilEquation_e.SORTIE_ET_EXPRESSION_DE_TYPES_INCOMPATIBLES:
                            this.TextErreur = String.Format("Err (pos {0}) : Sortie et expression de types incompatibles", value.Position);
                            this.EtatEquation = EquationState.ERREUR;
                            break;
                        case Pegase.CompilEquation.DiagnosticCompilEquation_e.OPERANDE_MANQUANT_OU_INCORRECT:
                            this.TextErreur = String.Format("Err (pos {0}) : Operande manquant ou incorrect", value.Position);
                            this.EtatEquation = EquationState.ERREUR;
                            break;
                        case Pegase.CompilEquation.DiagnosticCompilEquation_e.DEF_OP_ABSENT:
                            this.TextErreur = String.Format("Err (pos {0}) : Définition de l'opérateur absent", value.Position);
                            this.EtatEquation = EquationState.ERREUR;
                            break;
                        default:
                            this.TextErreur = "";
                            this.EtatEquation = EquationState.NON_EVALUEE;
                            break;
                    }
                }
            }
        } // endProperty: ResultatCompil

        #endregion

        // Constructeur
        #region Constructeur

        // Initialiser une équation à partir d'un element XML
        public Equation(XElement mnemologique, XElement textequation, Formule Parent)
        {
            this.FormuleParent = Parent;
            this.MnemoLogique = mnemologique.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            this.TextEquation = textequation.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            this.EtatEquation = EquationState.NON_EVALUEE;
        }

        public Equation(string mnemologique, string textequation)
        {
            this.MnemoLogique = mnemologique;
            this.TextEquation = textequation;
            this.EtatEquation = EquationState.NON_EVALUEE;
        }
        // Initialiser une équation vide
        public Equation(Formule Parent)
        {
            this.FormuleParent = Parent;
            this.MnemoLogique = "";
            this.TextEquation = "";
            this.EtatEquation = EquationState.NON_EVALUEE;
        }
        public Equation(string mnemologique, string textequation, Formule Parent)
        {
            this.MnemoLogique = mnemologique;
            this.FormuleParent = Parent;
            this.TextEquation = textequation;
            
            this.EtatEquation = EquationState.NON_EVALUEE;
        }
        #endregion

        // Méthodes
        #region Méthodes

        /// <summary>
        /// Vérifier l'équation en cours
        /// En cas d'erreur, garde le type d'erreur et la position
        /// </summary>
        public ResultatCompilEquation VerifEquation ( )
        {
            Int32 LongueurProgramme;
            ushort[] Programme;
            Int32 Indice = 0;
            Int32 Famille = 0;

            if (this.TextEquation.Length > 2)
            {
                String IsComment = this.TextEquation.Trim().Substring(0, 2);

                if (IsComment == "//")
                {
                    ResultatCompilEquation RCE = new ResultatCompilEquation();
                    RCE.Diagnostique = Pegase.CompilEquation.DiagnosticCompilEquation_e.EXPRESSION_CORRECTE;
                    RCE.Position = this.TextEquation.Length;
                    this.ResultatCompil = RCE;
                }
                else
                {
                    this.ResultatCompil = DLL_Equation.TesterEquation(this.TextEquation, ref Famille, ref Indice, out LongueurProgramme, out Programme);
                } 
            }
            return this.ResultatCompil;
        } // endMethod: VerifEquation

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: Equation

	
}
