using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using JAY.FileCore;
using JAY.XMLCore;
using System.Configuration;
using System.Collections.Specialized;
using Mvvm = GalaSoft.MvvmLight;

namespace JAY.PegaseCore.InternalDataModel
{
    /// <summary>
    /// Gestion des variables simples
    /// </summary>
    public class GestionVariableSimple : Mvvm.ViewModelBase
    {
        // Variables singleton
        private static GestionVariableSimple _instance;
        static readonly object instanceLock = new object();

        #region Constantes

        private const String XMLVariableAllURI = "/EditableVariablesAll.xml";
        private const String GROUP = "Group";
        private const String VARIABLE = "Variable";

        #endregion

        // Variables
        #region Variables

        private ObservableCollection<VariableEditable> _listVariableEditable;
        private ObservableCollection<VariableEditable> _listVariableUser;
        private ObservableCollection<VariableEditable> _listVariableExpert;
        private ObservableCollection<GroupVariable> _listGroup;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La liste de tous les groupes de variables disponibles
        /// La liste de toutes les variables par groupes
        /// </summary>
        public ObservableCollection<GroupVariable> ListGroup
        {
            get
            {
                if (this._listGroup == null)
                {
                    this.InitList();
                }

                return this._listGroup;
            }
            private set
            {
                this._listGroup = value;
                RaisePropertyChanged("ListGroup");
            }
        } // endProperty: ListGroup

        /// <summary>
        /// L'instance unique du singleton
        /// </summary>
        public static GestionVariableSimple Instance
        {
            get
            {
                return Get();
            }
        } // endProperty: Instance

        /// <summary>
        /// La liste de toutes les variables simples éditables
        /// </summary>
        public ObservableCollection<VariableEditable> ListVariableEditable
        {
            get
            {
                if (this._listVariableEditable == null)
                {
                    this.InitList();
                }
                return this._listVariableEditable;
            }
            set
            {
                this._listVariableEditable = value;
            }
        } // endProperty: ListVariableEditable

        /// <summary>
        /// La liste des variables éditables pour l'utilisateur
        /// </summary>
        public ObservableCollection<VariableEditable> ListVariableEditableUser
        {
            get
            {
                if (this._listVariableUser == null)
                {
                    IEnumerable<VariableEditable> query = from ve in this.ListVariableEditable
                                                          where ve.IsUser
                                                          select ve;

                    if (query.Count() > 0)
                    {
                        this._listVariableUser = new ObservableCollection<VariableEditable>();
                        foreach (var item in query)
                        {
                            this._listVariableUser.Add(item);
                        }
                    }
                }

                return this._listVariableUser;
            }
        } // endProperty: ListVariableEditableUser

        /// <summary>
        /// La liste des variables éditables pour l'expert
        /// </summary>
        public ObservableCollection<VariableEditable> ListVariableEditableExpert
        {
            get
            {
                if (this._listVariableExpert == null)
                {
                    IEnumerable<VariableEditable> query = from ve in this.ListVariableEditable
                                                          where ve.IsExpert
                                                          select ve;

                    if (query.Count() > 0)
                    {
                        this._listVariableExpert = new ObservableCollection<VariableEditable>();
                        foreach (var item in query)
                        {
                            this._listVariableExpert.Add(item);
                        }
                    }
                }

                return this._listVariableExpert;
            }
        } // endProperty: ListVariableEditableExpert

        #endregion

        // Constructeur
        #region Constructeur

        private GestionVariableSimple()
        {
        }

        #endregion

        // Méthodes
        #region Méthodes

        // Retourne une instance unique de la classe
        private static GestionVariableSimple Get()
        {
            lock (instanceLock)
            {
                if (_instance == null)
                    _instance = new GestionVariableSimple();

                return _instance;
            }
        }

        /// <summary>
        /// Initialiser la liste des variables à éditer
        /// </summary>
        public void InitList()
        {
            // 1 - lire le fichier EditableVariable.data et reconstituer tous les groupes
            FilePackage package = FilePackage.GetVariableSimple();
            String FileUri = XMLVariableAllURI;
            ObservableCollection<VariableEditable> listVariable = new ObservableCollection<VariableEditable>();

            //NameValueCollection NVC = (NameValueCollection)ConfigurationManager.GetSection("PEGASE");

            //if (NVC != null)
            //{
            //    // CF 05022014 if (NVC["UserMode"].ToString() == "Expert")
            //    if (DefaultValues.Get().UserMode == DefaultValues.EXPERT)
            //    {
            //        FileUri = XMLExpertURI;
            //    }
            //    else
            //    {
            //        FileUri = XMLUserURI;
            //    }
            //}

            Stream fileStream = package.GetPartStream(new Uri(FileUri, UriKind.Relative));

            XDocument doc = XDocument.Load(fileStream);
            Boolean IsFirstGroup = true;
            ObservableCollection<GroupVariable> groups = new ObservableCollection<GroupVariable>();

            // 2 - initialiser chacune des variables à partir du fichier XML des variables éditables
            foreach (XElement group in doc.Descendants(GROUP))
            {
                GroupVariable GV = new GroupVariable();
                ObservableCollection<VariableEditable> variables = new ObservableCollection<VariableEditable>();
                if (IsFirstGroup)
                {
                    GV.IsSelected = true;
                    IsFirstGroup = false;
                }

                List<XElement> Variables = PegaseCore.PegaseData.Instance.XMLRoot.Descendants("Variable").ToList<XElement>();

                foreach (XElement variable in group.Descendants(VARIABLE))
                {
                    // 2.1 - rechercher la liste des variables correspondantes dans le fichier de paramètres chargés
                    var query = from xmlVar in Variables
                                where xmlVar.Attribute(XML_ATTRIBUTE.CODE) != null && xmlVar.Attribute(XML_ATTRIBUTE.CODE).Value == variable.Value
                                select xmlVar;

                    if (query.Count() > 0)
                    {
                        if (variable.Attribute(XML_ATTRIBUTE.CODE_COMPLEMENT) != null)
                        {
                            query = from row in query
                                    where row.Attribute(XML_ATTRIBUTE.DESCRIPTION).Value == variable.Attribute(XML_ATTRIBUTE.CODE_COMPLEMENT).Value
                                    select row;
                        }

                        Int32 NbVar = 1;

                        GV.Name = group.Attribute("value").Value;
                        if (GV.Name == "")
                        {
                            GV.Name = "Groupe de variable";
                        }

                        foreach (var item in query)
                        {
                            // 3 - initialiser chacune des variables à partir du fichier XML en cours d'édition
                            VariableEditable VE = new VariableEditable();
                            VE.IsSelected = true;
                            VE.VarGroupName = group.Attribute("value").Value;
                            VE.VarName = variable.Value;
                            VE.NbVariables = NbVar;
                            VE.SetRefXMLVariable(item);

                            // 3.0 - La variable est-elle éditable ?
                            VE.SetIsEditable(variable);

                            // 3.1 - Y a-t-il une méthode d'édition spécifique ?
                            VE.SetEditMethod(variable);

                            // 3.2 - La donnée doit-elle être affichée ou non ?
                            VE.SetDataVisibility(variable);

                            // 3.3 - Définir la longueur maximum de la variable
                            VE.SetLongueurMax(variable);

                            // 3.4 - Définir la regex associée à l'édition de cette variable
                            VE.SetRegEx(variable);

                            // 3.5 - Définir la largeur du séparateur de ligne
                            VE.SetSeparatorHeight(variable);

                            // 3.6 - Définir la marge de la ligne
                            VE.SetMargin(variable);

                            // 3.7 - Définir l'appartenance de la variable (Expert / User / Les deux)
                            VE.SetUserExpertDisponibility(variable);
                            listVariable.Add(VE);

                            GV.Variables.Add(VE);
                            NbVar++;
                        }
                    } // endif: Query.count
                } // endforeach : variable
                groups.Add(GV);
            } // endforeach : group

            this.ListGroup = groups;

            // 4 - fermer le fichier
            package.ClosePackage();

            this._listVariableEditable = listVariable;
        } // endMethod: InitList

        /// <summary>
        /// Sauvegarder les valeurs éditées dans le XML
        /// </summary>
        public void SaveList()
        {
            // 1 - lire le fichier EditableVariable.data et reconstituer tous les groupes
            FilePackage package = FilePackage.GetVariableSimple();
            String FileUri = XMLVariableAllURI;
            ObservableCollection<VariableEditable> listVariable = new ObservableCollection<VariableEditable>();

            Stream fileStream = package.GetPartStream(new Uri(FileUri, UriKind.Relative));

            XDocument doc = XDocument.Load(fileStream);
            Boolean IsFirstGroup = true;
            ObservableCollection<GroupVariable> groups = new ObservableCollection<GroupVariable>();

            // 2 - initialiser chacune des variables à partir du fichier XML des variables éditables
            foreach (XElement group in doc.Descendants(GROUP))
            {
                GroupVariable GV = new GroupVariable();
                ObservableCollection<VariableEditable> variables = new ObservableCollection<VariableEditable>();
                if (IsFirstGroup)
                {
                    GV.IsSelected = true;
                    IsFirstGroup = false;
                }

                List<XElement> Variables = PegaseCore.PegaseData.Instance.XMLRoot.Descendants("Variable").ToList<XElement>();

                foreach (XElement variable in group.Descendants(VARIABLE))
                {
                    // 2.1 - rechercher la liste des variables correspondantes dans le fichier de paramètres chargés
                    var query = from xmlVar in Variables
                                where xmlVar.Attribute(XML_ATTRIBUTE.CODE) != null && xmlVar.Attribute(XML_ATTRIBUTE.CODE).Value == variable.Value
                                select xmlVar;

                    if (query.Count() > 0)
                    {
                        if (variable.Attribute(XML_ATTRIBUTE.CODE_COMPLEMENT) != null)
                        {
                            query = from row in query
                                    where row.Attribute(XML_ATTRIBUTE.DESCRIPTION).Value == variable.Attribute(XML_ATTRIBUTE.CODE_COMPLEMENT).Value
                                    select row;
                        }

                        // Pour chacune des variables trouvées dans le XML, mettre en face la liste des variables éditables et mettre à jour les valeurs
                        foreach (var value in query)
                        {
                            String XmlValue = value.ToString();
                            var queryEV = from ev in this.ListVariableEditable
                                          where ev.RefElement.ToString() == XmlValue && string.IsNullOrEmpty(ev.EditMethod)
                                          select ev;

                            if (queryEV.Count() > 0)
                            {
                                if (value.Attribute(XML_ATTRIBUTE.VALUE) != null)
	                            {
                                    value.Attribute(XML_ATTRIBUTE.VALUE).Value = queryEV.First().RefElementValue;
                                }
                            }
                        }
                    } // endif: Query.count
                } // endforeach : variable
            } // endforeach : group

            // 4 - fermer le fichier
            package.ClosePackage();
        } // endMethod: SaveList

        /// <summary>
        /// Retourner les variables du groupe suivant le mode choisi (Expert, User)
        /// </summary>
        public ObservableCollection<VariableEditable> GetVariableByGroup ( GroupVariable GV, String mode )
        {
            ObservableCollection<VariableEditable> Result = new ObservableCollection<VariableEditable>();

            if (GV != null)
            {
                // Filtrer en fonction du mode
                if (mode == DefaultValues.EXPERT)
                {
                    var query = from v in GV.Variables
                                where v.IsExpert
                                select v;

                    if (query.Count() > 0)
                    {
                        foreach (var item in query)
                        {
                            Result.Add(item);
                        }
                    }
                }
                else
                {
                    var query = from v in GV.Variables
                                where v.IsUser
                                select v;
                    if (query.Count() > 0)
                    {
                        foreach (var item in query)
                        {
                            Result.Add(item);
                        }
                    }
                }
            }

            return Result;
        } // endMethod: GetVariableByGroup

        /// <summary>
        /// Retourner la variable désignée par son nom VarName
        /// </summary>
        public VariableEditable GetVariableByName ( String VarName )
        {
            VariableEditable Result = null;

            var query = from v in this.ListVariableEditable
                        where v.VarName == VarName
                        select v;

            if (query.Count() > 0)
            {
                Result = query.First();
            }

            return Result;
        } // endMethod: GetVariableByName
        /// <summary>
        /// Retourner la variable désignée par son nom VarName
        /// </summary>
        public VariableEditable GetVariableByNameCompl(String VarName,String Codecompl)
        {
            VariableEditable Result = null;

            var query = from v in this.ListVariableEditable
                        where v.VarName == VarName && v.Description == Codecompl
                        select v;

            if (query.Count() > 0)
            {
                Result = query.First();
            }

            return Result;
        } // endMethod: GetVariableByName
        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: GestionVariableSimple
}
