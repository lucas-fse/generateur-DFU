using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Xml;
using System.Windows.Media;
using Mvvm = GalaSoft.MvvmLight;

namespace JAY.WpfCore
{
    /// <summary>
    /// Un élement Xaml chargé dynamiquement et prêt à être affiché
    /// </summary>
    public class XamlElement : Mvvm.ViewModelBase
    {
        // Constantes
        #region Constantes

        public const String TYPE_ELEMENT = "XamlElement";

        #endregion

        // Variables
        #region Variables

        private UIElement _xamlGraphic;
        private String _name;
        private Boolean _isALibraryElement;

        private static Boolean _isInMove;
        private static Point _oldPosition;
        private static Point _currentPosition;
        private static UIElement _currentDragElement;
        private static Canvas _rootCanvas;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// L'objet a-t-il été cloné? Ou fait-il parti de la bibliothèque
        /// </summary>
        public Boolean IsALibraryElement
        {
            get
            {
                return this._isALibraryElement;
            }
            private set
            {
                this._isALibraryElement = value;
                RaisePropertyChanged("IsALibraryElement");
            }
        } // endProperty: IsALibraryElement

        /// <summary>
        /// Le nom de la resource Xaml
        /// </summary>
        public String Name
        {
            get
            {
                return this._name;
            }
            private set
            {
                this._name = value;
                RaisePropertyChanged("Name");
            }
        } // endProperty: Name

        /// <summary>
        /// Le graphic Xaml chargé
        /// </summary>
        public UIElement XamlGraphic
        {
            get
            {
                return this._xamlGraphic;
            }
            private set
            {
                this._xamlGraphic = value;
                RaisePropertyChanged("XamlGraphic");
            }
        } // endProperty: XamlGraphic

        #endregion

        // Constructeur
        #region Constructeur

        public XamlElement()
        {
        }

        static XamlElement()
        {
            _isInMove = false;
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Lire un fichier Xaml
        /// </summary>
        public static XamlElement Load ( String FileName )
        {
            XamlElement Result = null;

            if (File.Exists(FileName))
            {
                String name = Path.GetFileNameWithoutExtension(FileName);
                Result = XamlElement.Load(File.OpenRead(FileName), name);
                
            }

            return Result;
        } // endMethod: Load

        /// <summary>
        /// Lire un fichier Xaml
        /// </summary>
        public static XamlElement Load( Stream stream, String name )
        {
            XamlElement Result = new XamlElement();

            UIElement element = (UIElement)XamlReader.Load(stream);
            Result.Name = name;
            Result.XamlGraphic = element;

            // Initialiser la possibilité de déplacement de l'objet
            Result.IsALibraryElement = true;
            Result.XamlGraphic.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(XamlGraphic_MouseLeftButtonDown);

            _rootCanvas = XamlElement.GetRootCanvas();

            return Result;
        }

        static void XamlGraphic_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Cloner l'élément et le mettre en déplacement
            _currentDragElement = XamlElement.Clone((UIElement)sender);
            DragDrop.DoDragDrop(_currentDragElement, new DataObject(XamlElement.TYPE_ELEMENT, _currentDragElement), DragDropEffects.Copy);
        }

        /// <summary>
        /// Acquérir la grille racine
        /// </summary>
        private static Canvas GetRootCanvas( )
        {
            Canvas Result = null;
            Grid Root;

            Application ap = Application.Current;
            Window mainWindow = ap.MainWindow;
            Root = mainWindow.Content as Grid;

            foreach (var item in Root.Children)
            {
                if (item is Canvas)
                {
                    Result = item as Canvas;
                }
            }
            
            return Result;
        } // endMethod: GetRootGrid

        /// <summary>
        /// Cloner l'élément en cours de séléction
        /// </summary>
        public UIElement Clone()
        {
            return XamlElement.Clone(this.XamlGraphic);
        } // endMethod: Clone

        /// <summary>
        /// Cloner un élément
        /// </summary>
        public static UIElement Clone( UIElement source )
        {
            UIElement Result = null;

            if(source != null)
            {
                String s = XamlWriter.Save(source);
                StringReader stringReader = new StringReader(s);

                XmlReader xmlReader = XmlTextReader.Create(stringReader, new XmlReaderSettings());

                Result = (UIElement)XamlReader.Load(xmlReader);
            }

            return Result;
        } // endMethod: Clone

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: XamlElement
}
