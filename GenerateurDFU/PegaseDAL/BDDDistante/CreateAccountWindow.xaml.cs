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
	/// Logique d'interaction pour CreateAccountWindow.xaml
	/// </summary>
	public partial class CreateAccountWindow : Window
	{
		public CreateAccountWindow()
		{
			this.InitializeComponent();
			
			// Insérez le code requis pour la création d’objet sous ce point.
		}

        private void Window_Closed(object sender, EventArgs e)
        {
            if (!this.DialogResult.HasValue)
            {
                this.DialogResult = false;
            }
        }

        private void PasswordTB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.P1.Text = this.PasswordTB.Password;
            if (this.PasswordTB.Password.Length < 6 || this.PasswordTB.Password != this.PasswordVerifTB.Password)
            {
                this.PasswordVerifTB.Background = new SolidColorBrush(Color.FromRgb(255, 150, 100));
                this.PasswordTB.Background = new SolidColorBrush(Color.FromRgb(255, 150, 100));
            }
            else
            {
                this.PasswordTB.Background = Brushes.White;
                this.PasswordVerifTB.Background = Brushes.White;
            }
        }

        private void PasswordVerifTB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.P1b.Text = this.PasswordVerifTB.Password;
            if (this.PasswordVerifTB.Password.Length < 6 || this.PasswordTB.Password != this.PasswordVerifTB.Password)
            {
                this.PasswordVerifTB.Background = new SolidColorBrush(Color.FromRgb(255, 150, 100));
                this.PasswordTB.Background = new SolidColorBrush(Color.FromRgb(255, 150, 100));
            }
            else
            {
                this.PasswordTB.Background = Brushes.White;
                this.PasswordVerifTB.Background = Brushes.White;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialiser les valeurs
            //BDDDistante.ViewModelCreateAccountWindow VmCAW = this.DataContext as BDDDistante.ViewModelCreateAccountWindow;

            //if (VmCAW != null)
            //{
            //    VmCAW.LastName = "";
            //    VmCAW.FirstName = "";
            //    VmCAW.EMail = "";
            //    this.PasswordTB.Password = "";
            //    this.PasswordVerifTB.Password = "";
            //    VmCAW.PhoneNumber = "";
            //    VmCAW.Society = "";
            //    VmCAW.Street = "";
            //    VmCAW.Town = "";
            //    VmCAW.ZipCode = "";
            //}
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}