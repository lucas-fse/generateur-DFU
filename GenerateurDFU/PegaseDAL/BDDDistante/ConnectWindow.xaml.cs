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
	public partial class ConnectWindow : Window
    {
		public ConnectWindow()
		{
			this.InitializeComponent();
            // Traduire les champs
            this.Title = LanguageSupport.Get().GetText("Plg_ConnexionT/CW_WINDOW_NAME");
            this.TextID.Text = LanguageSupport.Get().GetText("Plg_ConnexionT/CW_ID");
            this.TextPassword.Text = LanguageSupport.Get().GetText("Plg_ConnexionT/CW_PASSWORD");
            this.SaveID_Pass.Content = LanguageSupport.Get().GetText("Plg_ConnexionT/CW_KEEP_INFO");
            this.RecupPassword.Content = LanguageSupport.Get().GetText("Plg_ConnexionT/CW_FORGOT_PASSWORD");
            this.CreateAccountButton.Content = LanguageSupport.Get().GetText("Plg_ConnexionT/CW_CREATE_ACCOUNT");
            this.TryToConnectButton.Content = LanguageSupport.Get().GetText("Plg_ConnexionT/CW_CONNECT");
		}

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordCopy.Text = passwordBox.Password;
        }

        private void TryToConnectButton_Click(object sender, RoutedEventArgs e)
        {
            BDDDistante.ConnectResult connectResult = BDDDistante.BDDServeur.Get().VerifyAccessRights(UserID.Text, passwordBox.Password);
            if (connectResult == BDDDistante.ConnectResult.ConnectOK)
            {
                if (this.SaveID_Pass.IsChecked.Value == true)
                {
                    // Sauvegarder les données
                    DefaultValues.Get().UserID = this.UserID.Text;
                    DefaultValues.Get().Password = this.passwordBox.Password;
                }

                DialogResult = true;
                this.Close();
            }
            else if (connectResult == BDDDistante.ConnectResult.PasswordFalse)
            {
                ErrorText.Visibility = System.Windows.Visibility.Visible;
                this.RecupPassword.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                ErrorText.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (!DialogResult.HasValue)
            {
                DialogResult = false;
            }
        }

        private void RecupPassword_Click(object sender, RoutedEventArgs e)
        {
            this.RecupPassword.Visibility = System.Windows.Visibility.Hidden;
        }
	}
}