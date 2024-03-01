using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JAY.DAL
{
	/// <summary>
	/// Logique d'interaction pour ConnectWindow.xaml
	/// </summary>
	public partial class ChangePasswordWindow : Window
    {
        #region Variables

        private String _reallySendPassword;

        #endregion

        #region Propriétés

        /// <summary>
        /// Le mot de passe réellement envoyé à l'utilisateur
        /// </summary>
        public String ReallySendPassword
        {
            get
            {
                return this._reallySendPassword;
            }
            private set
            {
                this._reallySendPassword = value;
            }
        } // endProperty: ReallySendPassword

        #endregion

        public ChangePasswordWindow(String reallySendPassword)
		{
			this.InitializeComponent();
			// stocker le mot de passe réellement envoyé 
            this.ReallySendPassword = reallySendPassword;
            this.ValidateButton.IsEnabled = false;

            ExplainText.Text = LanguageSupport.Get().GetText("Plg_ConnexionT/CP_EXPLAIN");
            SendPasswordText.Text = LanguageSupport.Get().GetText("Plg_ConnexionT/CP_SENDPASSWORD");
            NewPasswordText.Text = LanguageSupport.Get().GetText("Plg_ConnexionT/CP_NEWPASSWORD");
            ConfirmPassword.Text = LanguageSupport.Get().GetText("Plg_ConnexionT/CP_CONFIRMPASSWORD");
            ValidateButton.Content = LanguageSupport.Get().GetText("Plg_ConnexionT/CP_VALIDATE");
            this.Title = LanguageSupport.Get().GetText("Plg_ConnexionT/CP_WINDOW_NAME");
		}

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.VerifyPassword();
        }
        
        /// <summary>
        /// Vérifier le mot de passe
        /// </summary>
        public void VerifyPassword ( )
        {
            Int32 Result = 0;

            // Vérifier si le mot de passe envoyé par le serveur pour permettre la réinitialisation correspond au mot de passe entré
            if (this.ReallySendPassword == SendPassword.Text)
            {
                Result += 1;
            }

            // Vérifier si la longueur du mot de passe est satisfaisante (>6)
            if (passwordBox1.Password.Length > 6)
            {
                Result += 1;
            }

            // Vérifier si le mot de passe et sa vérification sont identique
            if (passwordBox1.Password == passwordBox2.Password)
            {
                Result += 1;
            }

            // Si toutes les conditions sont vérifiées, alimer le bouton de validation
            if (Result == 3)
            {
                this.ValidateButton.IsEnabled = true;
            }
        } // endMethod: VerifyPassword

        private void ValidateButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = false;
        }

        private void passwordBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.VerifyPassword();
        }
	}
}