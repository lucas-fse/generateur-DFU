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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JAY.PegaseCore.Controls
{
    /// <summary>
    /// Logique d'interaction pour EditLabelCtrl.xaml
    /// </summary>
    public partial class EditLabelCtrl : UserControl
    {
        #region Propriétés de dépendances

        #region Label

        public LabelBase Label
        {
            get { return base.GetValue(LabelProperty) as LabelBase; }
            set { base.SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
          DependencyProperty.Register("Label", typeof(LabelBase), typeof(EditLabelCtrl)); 
        #endregion

        #region NbCharactereMax

        public Int32 NbCharacterMax
        {
            get { return (Int32)base.GetValue(NbCharacterMaxProperty); }
            set { base.SetValue(NbCharacterMaxProperty, value); }
        }
        public static readonly DependencyProperty NbCharacterMaxProperty =
          DependencyProperty.Register("NbCharacterMax", typeof(Int32), typeof(EditLabelCtrl));
	
        #endregion

        public Boolean IsEditable
        {
            get { return (Boolean)base.GetValue(IsEditableProperty); }
            set { base.SetValue(IsEditableProperty, value); }
        }

        public static readonly DependencyProperty IsEditableProperty =
          DependencyProperty.Register("IsEditable", typeof(Boolean), typeof(EditLabelCtrl));

        #endregion

        #region Constructeur

        public EditLabelCtrl()
        {
            InitializeComponent();
        }

        #endregion
    }
}
