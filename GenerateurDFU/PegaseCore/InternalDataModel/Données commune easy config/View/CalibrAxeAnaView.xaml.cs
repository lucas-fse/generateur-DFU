using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JAY.Affichage.View
{
    /// <summary>
    /// Logique d'interaction pour CalibrAxeAnaView.xaml
    /// </summary>
    public partial class CalibrAxeAnaView : Window
    {
        public CalibrAxeAnaView()
        {
            InitializeComponent();
        }

        private void Btn_OK_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// Le bouton OK est cliqué, fixer le DialogResult
			this.DialogResult = true;
			this.Close();
        }
    }
}
