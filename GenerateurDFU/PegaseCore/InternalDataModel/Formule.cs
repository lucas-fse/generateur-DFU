using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Pegase.CompilEquation;
using Mvvm = GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace JAY.PegaseCore
{
    // Enumération définissant les différents états d'une formule
    public enum FormuleState
    {
        NON_EVALUEE,
        ERREUR,
        OK
    }

    // Enumération définissant le type de formule.
    // User = formule éditée manuellement
    // Auto = formule générée par iDialog
    public enum TypeFormule
    {
        USER = 0,
        AUTO = 1
    }

    /// <summary>
    /// La classe de gestion d'une formule constituée d'équations
    /// </summary>
    public class Formule : Mvvm.ViewModelBase
    {
        #region Constante

        #endregion

        // Variables
        #region Variables

        private ObservableCollection<Equation> _equations;
        private FormuleState _etatFormule;
        private TypeFormule _formuleType;
        
        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Libellé de la fonction
        /// </summary>
        public String LibelFunction
        {
            get
            {
                return "LIBEL_MO/FUNCTION";
            }
        } // endProperty: LibelFunction

        /// <summary>
        /// La collection d'équation liées à la formule
        /// </summary>
        public ObservableCollection<Equation> Equations
        {
            get
            {
                return this._equations;
            }
            set
            {
                this._equations = value;
            }
        } // endProperty: Equations

        /// <summary>
        /// Le numero du mode d'exploitation lié à la formule
        /// </summary>
        public Int32 NumModeExploit
        {
            get;
            set;
        } // endProperty: NumModeExploit

        /// <summary>
        /// Le commentaire de la formule
        /// </summary>
        public String CommentaireFormule
        {
            get;
            set;
        } // endProperty: CommentaireFormule

        /// <summary>
        /// La mnémonique logique physique (à quel composant cette formule est reliée ex: RELAIS_01)
        /// </summary>
        public String MnemoLogiquePhy
        {
            get;
            set;
        } // endProperty: MnemoLogiquePhy

        /// <summary>
        /// La fonction
        /// </summary>
        public String Fonction
        {
            get;
            set;
        } // endProperty: Fonction

        /// <summary>
        /// Les commandes
        /// </summary>
        public String Commandes
        {
            get;
            set;
        } // endProperty: Commandes
        
        /// <summary>
        /// Description du fonctionnement
        /// </summary>
        public String Fonctionnement
        {
            get;
            set;
        } // endProperty: Fonctionnement

        /// <summary>
        /// Le nombre d'équations
        /// </summary>
        public Int32 NbEquation
        {
            get
            {
                return this.Equations.Count;
            }
        } // endProperty: NbEquation

        /// <summary>
        /// Etat de la formule
        /// </summary>
        public FormuleState EtatFormule
        {
            get
            {
                return this._etatFormule;
            }
            set
            {
                this._etatFormule = value;
                RaisePropertyChanged("EtatFormule");
            }
        } // endProperty: EtatFormule

        /// <summary>
        /// Tag définissant si la formule a été générée à la main, ou de façon automatique
        /// </summary>
        public TypeFormule FormuleType
        {
            get
            {
                return this._formuleType;
            }
            set
            {
                this._formuleType = value;
                RaisePropertyChanged("Tag");
            }
        } // endProperty: Tag

        #endregion

        // Constructeur
        #region Constructeur

        /// <summary>
        /// Constructeur pour les formules de mode
        /// </summary>
        /// <param name="formule"></param>
        public Formule(XElement formule)
        {
            this._equations = new ObservableCollection<Equation>();
            XMLCore.XMLProcessing XProcess = new XMLCore.XMLProcessing();
            XProcess.OpenXML(formule);

            ObservableCollection<XElement> tequations = XProcess.GetNodeByPath("TableEquation/Equation");
            ObservableCollection<XElement> mnemologique = XProcess.GetNodeByPath("TableEquation/MnemoLogique");

            // remplir les variables d'en-têtes

            // Le Numero du mode d'exploitation
            String Value;
            if (XProcess.GetNodeByPath("DescripteurFormule/NumModeExploit") != null)
            {
                Value = XProcess.GetNodeByPath("DescripteurFormule/NumModeExploit").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            }
            else
            {
                Value = "32";
            }

            Int32 NME;
            try
            {
                NME = Convert.ToInt32(Value);
            }
            catch
            {
                NME = 0;
            }

            this.NumModeExploit = NME;
            
            // Commentaire de la formule
            this.CommentaireFormule = XProcess.GetNodeByPath("DescripteurFormule/CommentaireFormule").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            // Type de la formule
            String TF = XProcess.GetValue("DescripteurFormule/CommentaireFormule", "", "", XMLCore.XML_ATTRIBUTE.PLAGE_VALEUR);
            switch (TF)
            {
                case "AUTO":
                    this.FormuleType = TypeFormule.AUTO;
                    break;
                case "USER":
                    this.FormuleType = TypeFormule.USER;
                    break;
                default:
                    this.FormuleType = TypeFormule.USER;
                    break;
            }

            // Mnémologique physique
            this.MnemoLogiquePhy = XProcess.GetNodeByPath("DescripteurFormule/MnemoLogiquePhy").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            // Fonction
            this.Fonction = XProcess.GetNodeByPath("DescripteurFormule/Fonction").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            // Commande
            this.Commandes = XProcess.GetNodeByPath("DescripteurFormule/Commandes").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            // Fonctionnement
            this.Fonctionnement = XProcess.GetNodeByPath("DescripteurFormule/Fonctionnement").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            Int32 NbE;
            try
            {
                NbE = Convert.ToInt32(XProcess.GetNodeByPath("DescripteurFormule/NbEquation").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value);
            }
            catch
            {
                NbE = 0;
            }

            // remplir les équations
            if (tequations != null)
            {
                for (Int32 i = 0; this.Equations.Count < NbE; i++)
                {
                    Equation e = new Equation(mnemologique[i], tequations[i], this);
                    if (e.TextEquation != "")
                    {
                        this.Equations.Add(e);
                    }
                    // Si le nombre d'équations déclarées dans le mode est trop important
                    // Le rétablir au nombre réel
                    if (i + 1 == tequations.Count)
                    {
                        break;
                    }
                }
            }
            if (this.Equations.Count == 0)
            {
                this.Equations.Add(new Equation(this));
            }

            this.VerifEquation();

            // Recevoir les messages
            Messenger.Default.Register<CommandMessage>(this, ReceiveMessage);
        }

        /// <summary>
        /// Constructeur pour les formules universelles
        /// </summary>
        /// <param name="DescripteurFormule"></param>
        /// <param name="TableEquation"></param>
        public Formule(XElement DescripteurFormule, XElement TableEquation)
        {
            this._equations = new ObservableCollection<Equation>();
            XMLCore.XMLProcessing XProcessFormule = new XMLCore.XMLProcessing();
            XProcessFormule.OpenXML(DescripteurFormule);

            XMLCore.XMLProcessing XProcessEquation = new XMLCore.XMLProcessing();
            XProcessEquation.OpenXML(TableEquation);

            ObservableCollection<XElement> tequations = XProcessEquation.GetNodeByPath("Equation");
            ObservableCollection<XElement> mnemologique = XProcessEquation.GetNodeByPath("MnemoLogique");

            // remplir les variables d'entêtes

            // Le Numero du mode d'exploitation
            String Value;
            if (XProcessFormule.GetNodeByPath("NumModeExploit") != null)
            {
                Value = XProcessFormule.GetNodeByPath("NumModeExploit").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            }
            else if (XProcessFormule.GetNodeByPath("ModeExploit") != null)
            {
                Value = XProcessFormule.GetNodeByPath("ModeExploit").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            }
            else
            {
                Value = "32";
            }
            Int32 NME;
            try
            {
                NME = Convert.ToInt32(Value);
            }
            catch
            {
                NME = 0;
            }
            this.NumModeExploit = NME;

            // Commentaire de la formule
            this.CommentaireFormule = XProcessFormule.GetNodeByPath("CommentaireFormule").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            // Type de la formule
            String TF = XProcessFormule.GetValue("CommentaireFormule", "", "", XMLCore.XML_ATTRIBUTE.PLAGE_VALEUR);
            switch (TF)
            {
                case "AUTO":
                    this.FormuleType = TypeFormule.AUTO;
                    break;
                case "USER":
                    this.FormuleType = TypeFormule.USER;
                    break;
                default:
                    this.FormuleType = TypeFormule.USER;
                    break;
            }

            // Mnémologique physique
            this.MnemoLogiquePhy = XProcessFormule.GetNodeByPath("MnemoLogiquePhy").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            // Fonction
            this.Fonction = XProcessFormule.GetNodeByPath("Fonction").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            // Commande
            this.Commandes = XProcessFormule.GetNodeByPath("Commandes").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            // Fonctionnement
            this.Fonctionnement = XProcessFormule.GetNodeByPath("Fonctionnement").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

            Int32 NbE;
            try
            {
                NbE = Convert.ToInt32(XProcessFormule.GetNodeByPath("NbEquation").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value);
            }
            catch
            {
                NbE = 0;
            }

            // remplir les équations en fonction du nombre d'équations déclarées dans la formule
            if (tequations != null)
            {
                for (Int32 i = 0; this.Equations.Count < NbE; i++)
                {
                    Equation e = new Equation(mnemologique[i], tequations[i], this);
                    if (e.TextEquation != "")
                    {
                        this.Equations.Add(e);
                    }
                    // Si le nombre d'équations déclarées dans le mode est trop important
                    // Le rétablir au nombre réel
                    if (i + 1 == tequations.Count)
                    {
                        break;
                    }
                }
            }
            if (this.Equations.Count == 0)
            {
                this.Equations.Add(new Equation(this));
            }

            this.VerifEquation();

            // Recevoir les messages
            Messenger.Default.Register<CommandMessage>(this, ReceiveMessage);
        }

        /// <summary>
        /// Constructeur pour une formule non décrite dans le XML
        /// </summary>
        /// <param name="NbMode"></param>
        public Formule(Int32 NbMode)
        {
            // Initialiser une nouvelle formule vide
            this._equations = new ObservableCollection<Equation>();
         //   this._equations.Add(new Equation(this));

            this.NumModeExploit = NbMode;
            this.Commandes = "Commande";
            this.Fonction = "NomFonction";
            this.Fonctionnement = "Fonctionnement";
            this.CommentaireFormule = "Commentaires";
            this.MnemoLogiquePhy = "Variable / organe à affecter";
            this.EtatFormule = FormuleState.NON_EVALUEE;

            // Recevoir les messages
            Messenger.Default.Register<CommandMessage>(this, ReceiveMessage);
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Vérifier l'ensemble des équations de la formule
        /// Si au moins une erreur est présente sur une équation
        /// Le spécifier au niveau de la formule
        /// </summary>
        public void VerifEquation ( )
        {
            this.EtatFormule = FormuleState.NON_EVALUEE;

            foreach (var equation in this.Equations)
            {
                equation.VerifEquation();
                if (equation.EtatEquation == EquationState.ERREUR)
                {
                    this.EtatFormule = FormuleState.ERREUR;
                }
            }

            if (this.EtatFormule != FormuleState.ERREUR)
            {
                this.EtatFormule = FormuleState.OK;
            }
        } // endMethod: VerifEquation

        /// <summary>
        /// Le message reçu
        /// </summary>
        public void ReceiveMessage(CommandMessage message)
        {
            // Faut-il mettre à jour la langue ?
            if (message.Command == PegaseCore.Commands.CMD_MAJ_LANGUAGE)
            {
                RaisePropertyChanged("LibelFunction");
            }
            // Faut-il mettre à jour la visibilité du paramètre en cours d'édition
        } // endMethod: ReceiveMessage

        /// <summary>
        /// Ajouter une équation
        /// </summary>
        public void AddEquation ( Int32 Pos )
        {
            this.Equations.Insert(Pos, new Equation(this));
        } // endMethod: AddEquation

        /// <summary>
        /// Supprimer une equation
        /// </summary>
        public void DeleteEquation ( Equation Eq )
        {
            this.Equations.Remove(Eq);
        } // endMethod: DeleteEquation
        
        /// <summary>
        /// Déplacer une équation vers le haut
        /// </summary>
        public void MoveEquationUp ( Equation Eq )
        {
            Int32 Pos = this.Equations.IndexOf(Eq);
            if (Pos > 0)
            {
                this.Equations.Move(Pos, Pos - 1); 
            }
        } // endMethod: MoveEquationUp
        
        /// <summary>
        /// Deplacer une équation vers le bas
        /// </summary>
        public void MoveEquationDown ( Equation Eq )
        {
            Int32 Pos = this.Equations.IndexOf(Eq);
            if (Pos < this.Equations.Count - 1)
            {
                this.Equations.Move(Pos, Pos + 1);
            }
        } // endMethod: MoveEquationDown

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: Formule
}
