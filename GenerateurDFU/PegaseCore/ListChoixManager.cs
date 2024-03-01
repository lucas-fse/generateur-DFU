using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using JAY.XMLCore;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Gestion d'une liste de choix fournie par le XML
    /// sous la forme "/oui=1/non=0"
    /// Cette classe permet d'alimenter un combobox ou une liste en
    /// extrayant les étiquettes d'une part et en fournissant les fonctions
    /// permettant de relier une étiquette et sa valeur
    /// </summary>
    public class ListChoixManager
    {
        // Variables
        #region Variables

        private ObservableCollection<String> _labels;
        private ObservableCollection<String> _values;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La liste des labels autorisées
        /// </summary>
        public ObservableCollection<String> Labels
        {
            get
            {
                return this._labels;
            }
            set
            {
                this._labels = value;
            }
        } // endProperty: Labels

        /// <summary>
        /// La liste des valeurs autorisées
        /// </summary>
        public ObservableCollection<String> Values
        {
            get
            {
                return this._values;
            }
            set
            {
                this._values = value;
            }
        } // endProperty: Values

        #endregion

        // Constructeur
        #region Constructeur

        public ListChoixManager(String Choices)
        {
            if (Choices == null)
            {
                return;
            }
            // Initialisation
            this._labels = new ObservableCollection<String>();
            this._values = new ObservableCollection<String>();

            if (Choices.Length > 0)
            {
                // éclater la liste en ces différents composants
                String[] Parts = Choices.Split(new Char[] { '/' });
                if (Parts.Length < 2)
                {
                    // rechercher la traduction
                    String Path = String.Format(VariableEditable.LANGAGE_PATH, Choices);
                    String Traduction = LanguageSupport.Get().GetToolTip(Path);
                    if (Traduction != null)
                    {
                        Parts = Traduction.Split(new Char[] { '/' });
                    }
                }

                if (Parts.Length > 1)
                {
                    this.InitList(Parts);
                }
            }
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Initialise la liste des choix
        /// </summary>
        private void InitList ( String[] ListOfChoice )
        {
            foreach (String part in ListOfChoice)
            {
                if (part == "")
                {
                    continue;
                }

                String[] Elements = part.Split(new Char[] { '=' });
                if (Elements.Length > 1)
                {
                    this._labels.Add(Elements[0]);
                    this._values.Add(Elements[1]);
                }
            }
        } // endMethod: InitList

        public void  Add(string labels, string value)
        {
            this._labels.Add(labels);
            this._values.Add(value);
        }
        public void remove(int pos)
        {
            this._labels.RemoveAt(pos);
            this._values.RemoveAt(pos);
        }
        /// <summary>
        /// Obtenir la valeur correspondante au label
        /// </summary>
        public String GetValue ( String Label )
        {
            String Result = "";

            if (this._labels != null && this._values != null)
            {
                for (int i = 0; i < this._labels.Count; i++)
                {
                    if (this._labels[i] == Label)
                    {
                        Result = this._values[i];
                        break;
                    }
                }
            }

            return Result;
        } // endMethod: GetValue
        
        /// <summary>
        /// Obtenir le label correspondant à la valeur
        /// </summary>
        public String GetLabel ( String Value )
        {
            String Result = "";

            if (this._values != null && this._labels != null)
            {
                for (int i = 0; i < this._values.Count; i++)
                {
                    if (this._values[i] == Value)
                    {
                        Result = this._labels[i];
                        break;
                    }
                } 
            }

            return Result;
        } // endMethod: GetLabel

        /// <summary>
        /// Acquérir la position du label
        /// </summary>
        public Int32 GetPosByLabel ( String Label )
        {
            Int32 Result = -1;

            if (this.Labels != null)
            {
                for (int i = 0; i < this.Labels.Count; i++)
                {
                    if (this.Labels[i] == Label)
                    {
                        Result = i;
                        break;
                    }
                }
            }

            return Result;
        } // endMethod: GetPos
        
        /// <summary>
        /// Acquérir la position de la valeur
        /// </summary>
        public Int32 GetPosByValue ( String Value )
        {
            Int32 Result = -1;

            if (this.Values != null)
            {
                for (int i = 0; i < this.Values.Count; i++)
                {
                    if (this.Values[i] == Value)
                    {
                        Result = i;
                        break;
                    }
                } 
            }

            return Result;
        } // endMethod: GetPosByValue

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: ListChoixManager
}
