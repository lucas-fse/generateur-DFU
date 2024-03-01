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
	/// Logique d'interaction pour ImportWindow.xaml
	/// </summary>
	public partial class ImportWindow : Window
	{
		public ImportWindow()
		{
			this.InitializeComponent();
			
			// Insérez le code requis pour la création d’objet sous ce point.
		}

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BDDLocal.ViewModelImportWindow vmIW = this.DataContext as BDDLocal.ViewModelImportWindow;

            if (vmIW.Progress < 100)
            {
                e.Cancel = true; 
            }
        }
	}
}