using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mvvm = GalaSoft.MvvmLight;
using System.Windows.Input;
using System.Windows;
using System.Net.Mail;
using System.Net;
using JAY.DAL;

namespace JAY.DAL.BDDDistante
{
    /// <summary>
    /// Le viewmodel de la fenêtre de connexion
    /// </summary>
    public class ViewModelConnect : Mvvm.ViewModelBase
    {
        // Variables
        #region Variables

        private String _userID;
        private String _userPassword;
        private Visibility _recupPasswordVisibility;
        private String _currentRequestPassword = null;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le mot de passe en cours 
        /// </summary>
        public String CurrentRequestPassword
        {
            get
            {
                return this._currentRequestPassword;
            }
            private set
            {
                this._currentRequestPassword = value;
            }
        } // endProperty: CurrentRequestPassword

        /// <summary>
        /// La visibilité du bouton permettant la récupération du mot de passe
        /// </summary>
        public Visibility RecupPasswordVisibility
        {
            get
            {
                return this._recupPasswordVisibility;
            }
            set
            {
                this._recupPasswordVisibility = value;
                RaisePropertyChanged("RecupPasswordVisibility");
            }
        } // endProperty: RecupPasswordVisibility

        /// <summary>
        /// Garder les informations de connexion
        /// </summary>
        public Boolean KeepConnectionData
        {
            get
            {
                return DefaultValues.Get().KeepConnectionData;
            }
            set
            {
                DefaultValues.Get().KeepConnectionData = value;
                RaisePropertyChanged("KeepConnectionData");
            }
        } // endProperty: KeepConnectionData

        /// <summary>
        /// L'ID de l'utilisateur
        /// </summary>
        public String UserID
        {
            get
            {
                return this._userID;
            }
            set
            {
                this._userID = value;
                RaisePropertyChanged("UserID");
            }
        } // endProperty: UserID

        /// <summary>
        /// Le mot de passe de l'utilisateur
        /// </summary>
        public String UserPassword
        {
            get
            {
                return this._userPassword;
            }
            set
            {
                this._userPassword = value;
                RaisePropertyChanged("UserPassword");
            }
        } // endProperty: UserPassword

        #endregion

        // Constructeur
        #region Constructeur

        public ViewModelConnect()
        {
            // Créer les commandes
            this.CreateCommandCreateNewAccount();
            this.CreateCommandLostPassword();

            if (this.IsInDesignMode)
            {
                this.RecupPasswordVisibility = Visibility.Visible;
            }
            else
            {
                this.RecupPasswordVisibility = Visibility.Hidden;
            }
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

        #region Commandes

        #region CommandLostPassword
        /// <summary>
        /// La commande LostPassword
        /// </summary>
        public ICommand CommandLostPassword
        {
            get;
            internal set;
        } // endProperty: CommandLostPassword

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandLostPassword()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandLostPassword = new Mvvm.Command.RelayCommand(ExecuteCommandLostPassword, CanExecuteCommandLostPassword);
        } // endMethod: CreateCommandLostPassword

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandLostPassword()
        {
            // 1 - Send email
            ServiceMethod.ServerMethodClient client = new ServiceMethod.ServerMethodClient();
            this.CurrentRequestPassword = client.RequestChangePassword(this.UserID);
            client.Close();

            ChangePasswordWindow CPW = new ChangePasswordWindow(this.CurrentRequestPassword);
            
            if (CPW.ShowDialog().Value == true)
            {
                // Enregistrer le nouveau mot de passe
                BDDServeur.Get().ChangePassword(this.UserID, CPW.passwordBox1.Password);

                // Valider la connexion
                this.UserPassword = CPW.passwordBox1.Password;
            }
        } // endMethod: ExecuteCommandLostPassword

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandLostPassword()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandLostPassword

        #endregion

        #region CommandCreateNewAccount
        /// <summary>
        /// La commande CreateNewAccount
        /// </summary>
        public ICommand CommandCreateNewAccount
        {
            get;
            internal set;
        } // endProperty: CommandCreateNewAccount

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandCreateNewAccount()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandCreateNewAccount = new Mvvm.Command.RelayCommand(ExecuteCommandCreateNewAccount, CanExecuteCommandCreateNewAccount);
        } // endMethod: CreateCommandCreateNewAccount

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandCreateNewAccount()
        {
            CreateAccountWindow CAW = new CreateAccountWindow();
            if (CAW.ShowDialog().Value == true)
            {
                // Créer l'utilisateur
            }
        } // endMethod: ExecuteCommandCreateNewAccount

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandCreateNewAccount()
        {
            Boolean Result = false;

            if (true)
            {
                Result = true;
            }

            return Result;
        } // endMethod: CanExecuteCommandCreateNewAccount
        #endregion

        #endregion
    } // endClass: ViewModelConnect
}