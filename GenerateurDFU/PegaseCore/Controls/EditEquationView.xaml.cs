using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Messenger = GalaSoft.MvvmLight.Messaging.Messenger;
using JAY.PegaseCore;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Logique d'interaction pour EditEquationView.xaml
    /// </summary>
    public partial class EditEquationView : UserControl
    {
        #region Propriétés de dépendances

        #region Formules
        /// <summary>
        /// The <see cref="Formules" /> dependency property's name.
        /// </summary>
        public const string FormulesPropertyName = "Formules";

        /// <summary>
        /// Gets or sets the value of the <see cref="Formules" />
        /// property. This is a dependency property.
        /// </summary>
        public ObservableCollection<JAY.PegaseCore.Formule> Formules
        {
            get
            {
                return (ObservableCollection<JAY.PegaseCore.Formule>)GetValue(FormulesProperty);
            }
            set
            {
                SetValue(FormulesProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="Formules" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty FormulesProperty = DependencyProperty.Register(
            FormulesPropertyName,
            typeof(ObservableCollection<JAY.PegaseCore.Formule>),
            typeof(EditEquationView), new PropertyMetadata(null, new PropertyChangedCallback(FormulesCallBack)));

        public static void FormulesCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs pc)
        {
            if (sender is EditEquationView)
            {
                EditEquationView EEV = sender as EditEquationView;
                EEV.Refresh();
            }
        } 
        #endregion

        #region CurrentModeNumber
        /// <summary>
        /// The <see cref="CurrentModeNumber" /> dependency property's name.
        /// </summary>
        public const string CurrentModeNumberPropertyName = "CurrentModeNumber";

        /// <summary>
        /// Gets or sets the value of the <see cref="CurrentModeNumber" />
        /// property. This is a dependency property.
        /// </summary>
        public Int32 CurrentModeNumber
        {
            get
            {
                return (Int32)GetValue(CurrentModeNumberProperty);
            }
            set
            {
                SetValue(CurrentModeNumberProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="CurrentModeNumber" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty CurrentModeNumberProperty = DependencyProperty.Register(
            CurrentModeNumberPropertyName,
            typeof(Int32),
            typeof(EditEquationView),
            new UIPropertyMetadata(0));

        public static void CurrentModeNumberCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs pc)
        {
            if (sender is EditEquationView)
            {
                EditEquationView EEV = sender as EditEquationView;
                EEV.Refresh();
            }
        }

        #endregion

        #endregion

        public EditEquationView()
        {
            InitializeComponent();
        }

        private void Equations_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView TV = sender as TreeView;

            Object SelectedItem = TV.SelectedItem;
            Messenger.Default.Send<CommandMessage>(new CommandMessage(SelectedItem, PegaseCore.Commands.CMD_MAJ_FORMULE_SELECTED));
        }

        /// <summary>
        /// Rafraichir la vue
        /// </summary>
        public void Refresh ( )
        {
            this.Equations.ItemsSource = this.Formules;
        } // endMethod: Refresh

        /// <summary>
        /// Le bouton ajouter est cliqué (menu contextuel)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            Object selectedItem = this.Equations.SelectedItem;

            if (selectedItem is Formule || selectedItem == null)
            {
                Formule F = selectedItem as Formule;
                Int32 Pos = this.Formules.IndexOf(F) + 1;
                // Vérifier que la position est au dessus de la première équation générée automatiquement
                Int32 PosAuto;
                for (PosAuto = 0; PosAuto < this.Formules.Count; PosAuto++)
                {
                    if (this.Formules[PosAuto].FormuleType == TypeFormule.AUTO)
                    {
                        break;
                    }
                }

                if (PosAuto < this.Formules.Count)
                {
                    if (Pos > PosAuto)
                    {
                        Pos = PosAuto;
                    }
                }

                this.Formules.Insert(Pos, new Formule(this.CurrentModeNumber));
            }
            else if (selectedItem is Equation)
            {
                Equation E = selectedItem as Equation;
                Formule F = null;
                foreach (Formule Fo in this.Formules)
                {
                    foreach (Equation Eq in Fo.Equations)
                    {
                        if (Eq == E)
                        {
                            F = Fo;
                            break;
                        }
                    }
                    if (F != null)
                    {
                        break;
                    }
                }
                if (F.FormuleType != TypeFormule.AUTO)
                {
                    Int32 Pos = F.Equations.IndexOf(E) + 1;
                    F.AddEquation(Pos); 
                }
            }

            Messenger.Default.Send<CommandMessage>(new CommandMessage(this, PegaseCore.Commands.CMD_MAJVIEW));
        }

        private void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            Object selectedItem = this.Equations.SelectedItem;

            if (selectedItem is Formule)
            {
                Formule F = selectedItem as Formule;

                if (F.FormuleType != TypeFormule.AUTO)
                {
                    this.Formules.Remove(F); 
                }
            }
            else if (selectedItem is Equation)
            {
                Equation E = selectedItem as Equation;
                Formule F = null;
                foreach (Formule Fo in this.Formules)
                {
                    foreach (Equation Eq in Fo.Equations)
                    {
                        if (Eq == E)
                        {
                            F = Fo;
                            break;
                        }
                    }
                    if (F != null)
                    {
                        break;
                    }
                }
                if (F.FormuleType != TypeFormule.AUTO)
                {
                    F.DeleteEquation(E);
                }
            }

            Messenger.Default.Send<CommandMessage>(new CommandMessage(this, PegaseCore.Commands.CMD_MAJVIEW));
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            Object selectedItem = this.Equations.SelectedItem;

            if (selectedItem is Formule)
            {
                Formule F = selectedItem as Formule;
                if (F.FormuleType != TypeFormule.AUTO)
                {
                    Int32 i = this.Formules.IndexOf(F);

                    if (i > 0)
                    {
                        this.Formules.Move(i, i - 1);
                    } 
                }
            }
            else if (selectedItem is Equation)
            {
                Equation E = selectedItem as Equation;
                Formule F = null;
                foreach (Formule Fo in this.Formules)
                {
                    foreach (Equation Eq in Fo.Equations)
                    {
                        if (Eq == E)
                        {
                            F = Fo;
                            break;
                        }
                    }
                    if (F != null)
                    {
                        break;
                    }
                }
                if (F.FormuleType != TypeFormule.AUTO)
                {
                    F.MoveEquationUp(E);
                }
            }
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            Object selectedItem = this.Equations.SelectedItem;

            if (selectedItem is Formule)
            {
                Formule F = selectedItem as Formule;
                if (F.FormuleType != TypeFormule.AUTO)
                {
                    Int32 i = this.Formules.IndexOf(F);

                    if (i < this.Formules.Count - 1)
                    {
                        this.Formules.Move(i, i + 1);
                    } 
                }
            }
            else if (selectedItem is Equation)
            {
                Equation E = selectedItem as Equation;
                Formule F = null;
                foreach (Formule Fo in this.Formules)
                {
                    foreach (Equation Eq in Fo.Equations)
                    {
                        if (Eq == E)
                        {
                            F = Fo;
                            break;
                        }
                    }
                    if (F != null)
                    {
                        break;
                    }
                }
                if (F.FormuleType != TypeFormule.AUTO)
                {
                    F.MoveEquationDown(E);
                }
            }
        }

        private static DependencyObject SearchTreeView<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
            {
                source = VisualTreeHelper.GetParent(source);
            }
            return source;
        }

        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseEventArgs e)
        {
            TreeViewItem treeViewItem = (TreeViewItem)SearchTreeView<TreeViewItem>((DependencyObject)e.OriginalSource);
            if (treeViewItem != null)
            {
                treeViewItem.IsSelected = true;
                e.Handled = true;
            }
        }
    }
}
