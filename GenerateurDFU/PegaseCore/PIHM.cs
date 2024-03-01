using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Mvvm = GalaSoft.MvvmLight;

namespace JAY.PegaseCore
{
    /// <summary>
    /// La classe de base pour toutes les IHM
    /// de pégase. Elle sert également de type de
    /// base pour l'utilisation des plugins
    /// </summary>
    public class PIHM : Mvvm.ViewModelBase
    {
        // Variables
        #region Variables

        protected UIElement _menu;
        protected Int32 _menuOrder;
        protected Visibility _isMenuVisible;
        protected String _CommandValue;
        private String _menuGroup;
        protected Boolean _isMenuEnable;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le graissage des charactères
        /// </summary>
        public Boolean IsSelected
        {
            get
            {
                Boolean Result = false;
                if (this.Menu != null)
                {
                    System.Windows.Controls.Control CView = this.Menu as System.Windows.Controls.Control;
                    JAY.PegaseCore.PluginsMenu MVModel = CView.DataContext as JAY.PegaseCore.PluginsMenu;
                    Result = MVModel.IsSelected;
                }
                return Result;
            }
            set
            {
                if (this.Menu != null)
                {
                    System.Windows.Controls.Control CView = this.Menu as System.Windows.Controls.Control;
                    JAY.PegaseCore.PluginsMenu MVModel = CView.DataContext as JAY.PegaseCore.PluginsMenu;
                    MVModel.IsSelected = value;
                }
            }
        } // endProperty: fontWeight

        /// <summary>
        /// Le menu est-il disponible?
        /// </summary>
        public Boolean IsMenuEnable
        {
            get
            {
                Boolean Result = false;
                if (this.Menu != null)
                {
                    System.Windows.Controls.Control CView = this.Menu as System.Windows.Controls.Control;
                    JAY.PegaseCore.PluginsMenu MVModel = CView.DataContext as JAY.PegaseCore.PluginsMenu;
                    Result = MVModel.IsActivated;
                }
                return Result;
            }
            set
            {
                if (this.Menu != null)
                {
                    System.Windows.Controls.Control CView = this.Menu as System.Windows.Controls.Control;
                    JAY.PegaseCore.PluginsMenu MVModel = CView.DataContext as JAY.PegaseCore.PluginsMenu;
                    MVModel.IsActivated = value;
                    RaisePropertyChanged("IsMenuEnable");
                }
            }
        } // endProperty: IsMenuEnable

        /// <summary>
        /// La vue à afficher dans le menu
        /// </summary>
        public UIElement Menu
        {
            get
            {
                return this._menu;
            }
            set
            {
                this._menu = value;
            }
        } // endProperty: Menu

        /// <summary>
        /// Le groupe auquel appartient le menu
        /// </summary>
        public String MenuGroup
        {
            get
            {
                return this._menuGroup;
            }
            set
            {
                this._menuGroup = value;
            }
        } // endProperty: MenuGroup

        /// <summary>
        /// L'ordre de la commande dans le menu
        /// </summary>
        public Int32 MenuOrder
        {
            get
            {
                return this._menuOrder;
            }
            protected set
            {
                this._menuOrder = value;
            }
        } // endProperty: MenuOrder

        /// <summary>
        /// Le menu est-il visible lorsque cette IHM est visible?
        /// </summary>
        public Visibility IsMenuVisible
        {
            get
            {
                return this._isMenuVisible;
            }
            protected set
            {
                this._isMenuVisible = value;
            }
        } // endProperty: IsMenuVisible

        /// <summary>
        /// La valeur de la commande lorsque le bouton du menu est cliqué
        /// </summary>
        public String CommandValue
        {
            get
            {
                return this._CommandValue;
            }
            protected set
            {
                this._CommandValue = value;
            }
        } // endProperty: CommandValue

        /// <summary>
        /// Le nom du plugins
        /// </summary>
        public virtual String PluginName
        {
            get
            {
                return Constantes.PluginName_Default;
            }
        } // endProperty: PluginName

        #endregion

        // Constructeur
        #region Constructeur

        public PIHM()
        {
            this._menuOrder = 0;
            this._menu = null;
            this._isMenuVisible = Visibility.Visible;
            this.MenuGroup = "General";
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Mettre à jour les données générées par le plugin
        /// </summary>
        public virtual void Update()
        {

        } // endMethod: Update

        /// <summary>
        /// Sauvegarder les paramètres du plugins s'il y a lieux
        /// </summary>
        public virtual void Save()
        {
            
        } // endMethod: Save

        /// <summary>
        /// Executer la commande principale.
        /// Généralement il s'agit de l'envoie d'un message au MainWindowViewModel
        /// afin d'afficher l'interface
        /// </summary>
        public virtual Boolean Run()
        {
            return true;
        }// endMethod: Run
        
        /// <summary>
        /// Acquérir la vue
        /// </summary>
        public virtual UIElement GetView()
        {
            UIElement Result = null;

            return Result;
        }
        
        /// <summary>
        /// A executer lorsque l'IHM est fermée
        /// </summary>
        public virtual Boolean Close()
        {
            Boolean Result = true;
            
            return Result;
        } // endMethod: Close

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: PIHM
}
