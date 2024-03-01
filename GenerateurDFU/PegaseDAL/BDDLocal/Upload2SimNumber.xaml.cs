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

namespace JAY
{
	/// <summary>
	/// Logique d'interaction pour Upload2SimNumber.xaml
	/// </summary>
	public partial class Upload2SimNumber : Window
	{
		public Upload2SimNumber()
		{
			this.InitializeComponent();
			
			// Insérez le code requis pour la création d’objet sous ce point.
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (!this.DialogResult.HasValue)
            {
                this.DialogResult = false;
            }
        }
	}
}