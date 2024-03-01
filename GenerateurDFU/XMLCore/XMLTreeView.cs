using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace JAY.XMLCore
{
    /// <summary>
    /// Un TreeView permettant de renvoyer des éléments XMLLeaf de XMLCore
    /// </summary>
    public class XMLTreeView : TreeView
    {
        #region Propriétés de dépendances

        /// <summary>
        /// The <see cref="CurrentItem" /> dependency property's name.
        /// </summary>
        public const string CurrentItemPropertyName = "CurrentItem";

        /// <summary>
        /// Gets or sets the value of the <see cref="CurrentItem" />
        /// property. This is a dependency property.
        /// </summary>
        public XMLLeaf CurrentItem
        {
            get
            {
                return (XMLLeaf)GetValue(CurrentItemProperty);
            }
            set
            {
                SetValue(CurrentItemProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="CurrentItem" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty CurrentItemProperty = DependencyProperty.Register(
            CurrentItemPropertyName,
            typeof(XMLLeaf),
            typeof(XMLTreeView),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(CurrentItemChanged)));
        
        /// <summary>
        /// Mettre à jour la propriété de dépendance du control
        /// </summary>
        public static void CurrentItemChanged ( object sender, DependencyPropertyChangedEventArgs e )
        {
            XMLTreeView TV = sender as XMLTreeView;

            if (TV.SelectedItem is XMLLeaf)
            {
                TV.CurrentItem = (XMLLeaf)TV.SelectedItem;
            }
        } // endMethod: CurrentItemChanged

        #endregion

        #region Constructeur
        // Constructeur
        // Abonnement à l'évenement SelectedItemChanged
        public XMLTreeView()
        {
            this.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(XMLTreeView_SelectedItemChanged);
        }

        // Méthode répondant à l'évènement SelectedItemChanged
        void XMLTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            CurrentItemChanged(this, new DependencyPropertyChangedEventArgs());
        }

        #endregion
    }
}
