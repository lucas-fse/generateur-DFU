using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Mvvm = GalaSoft.MvvmLight;
using Messenger = GalaSoft.MvvmLight.Messaging.Messenger;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Classe de base d'un groupe de variables
    /// </summary>
    public class GroupVariable : Mvvm.ViewModelBase
    {
        private const String GROUP_PATH = "/GroupTranslation/{0}";

        // Variables
        #region Variables

        private ObservableCollection<VariableEditable> _variables;
        private String _name;
        private Boolean _isSelected;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La liste des variables appartenant à ce groupe
        /// </summary>
        public ObservableCollection<VariableEditable> Variables
        {
            get
            {
                return this._variables;
            }
            private set
            {
                this._variables = value;
            }
        } // endProperty: Variables

        /// <summary>
        /// La variable est-elle sélectionnée?
        /// </summary>
        public Boolean IsSelected
        {
            get
            {
                return this._isSelected;
            }
            set
            {
                this._isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        } // endProperty: IsSelected

        /// <summary>
        /// Le nom du groupe
        /// </summary>
        public String Name
        {
            get
            {
                String Result;
                String Path = String.Format(GROUP_PATH, this._name);
                Result = LanguageSupport.Get().GetText(Path);
                if (Result == null || Result == "")
                {
                    Result = this._name;
                }

                return Result;
            }
            set
            {
                this._name = value;
            }
        } // endProperty: Name

        #endregion

        // Constructeur
        #region Constructeur

        public GroupVariable()
        {
            this.Variables = new ObservableCollection<VariableEditable>();

            // Recevoir les messages
            Messenger.Default.Register<CommandMessage>(this, ReceiveMessage);
        }

        #endregion

        // Méthodes
        #region Méthodes

        /// <summary>
        /// Le message reçu
        /// </summary>
        public void ReceiveMessage(CommandMessage message)
        {
            // Faut-il mettre à jour la langue ?
            if (message.Command == PegaseCore.Commands.CMD_MAJ_LANGUAGE)
            {
                RaisePropertyChanged("Name");
            }
        } // endMethod: ReceiveMessage

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: GroupVariable
}
