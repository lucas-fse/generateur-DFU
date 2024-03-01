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

namespace JAY.PegaseCore
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
        private Point _position;
        private UInt32 _crc32;
        private Boolean _isSelected;

        private static Boolean _isInMove;
        private static Point _oldPosition;
        private static Point _currentPosition;
        private static UIElement _currentDragElement;
        private static Canvas _rootCanvas;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// L'élément est-il sélectionné?
        /// </summary>
        public Boolean IsSelected
        {
            get
            {
                return this._isSelected;
            }
            set
            {
                this._isSelected = value;
            }
        } // endProperty: IsSelected

        /// <summary>
        /// CRC32 permettant de retrouver avec précision le fichier utilisé initialement
        /// </summary>
        public UInt32 CRC32
        {
            get
            {
                return this._crc32;
            }
            private set
            {
                this._crc32 = value;
            }
        } // endProperty: CRC32

        /// <summary>
        /// La position en cours
        /// </summary>
        public Point Position
        {
            get
            {
                if (this._position == null)
                {
                    this._position = new Point();
                }

                return this._position;
            }
            set
            {
                this._position = value;
                RaisePropertyChanged("Position");
            }
        } // endProperty: CurrentPosition

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

        public XamlElement(String name, Double lX, Double lY, UInt32 CRC32)
        {
            this.Name = name;
            this.Position = new Point(lX, lY);

            // Trouver l'image correspondante dans la bibliothèque
            UIElement graphic = XamlLibrary.Get().GetElement(CRC32);
            this.XamlGraphic = graphic;
            this.CRC32 = CRC32;
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
        public static XamlElement Load( String FileName, String Name )
        {
            XamlElement Result = null;

            if (File.Exists(FileName))
            {
                String UserName = Path.GetFileNameWithoutExtension(FileName);
                Result = XamlElement.Load(File.OpenRead(FileName), Name, UserName);
            }

            return Result;
        } // endMethod: Load

        /// <summary>
        /// Lire un fichier Xaml
        /// </summary>
        public static XamlElement Load( Stream stream, String name, String UserName )
        {
            XamlElement Result;
            String XamlStr;

            // Calculer le CRC du fichier
            using (StreamReader SR = new StreamReader(stream))
            {
                XamlStr = SR.ReadToEnd();
            }

            Result = XamlElement.Parse(XamlStr, name, UserName);

            if (Result == null)
            {
                MessageBox.Show(String.Format("Can't import {0}.xaml. File contains improper geometry (ex: TextBlock)", UserName));
            }

            return Result;
        }

        /// <summary>
        /// Construire un XamlElement à partir d'une chaine contenant les données Xaml
        /// </summary>
        public static XamlElement Parse(String XamlStr, String name, String UserName)
        {
            XamlElement Result = new XamlElement();

            try
            {
                // Si le Xaml contient TextBlock -> import impossible
                if (!XamlStr.Contains("TextBlock"))
                {
                    // Nettoyer le Xaml
                    String WhiteSpaceCollapse = "whiteSpaceCollapse=\"preserve\"";
                    while (XamlStr.Contains(WhiteSpaceCollapse))
                    {
                        Int32 Start = XamlStr.IndexOf(WhiteSpaceCollapse);
                        XamlStr = XamlStr.Remove(Start, WhiteSpaceCollapse.Length);
                    }

                    // Parser
                    UIElement element = (UIElement)XamlReader.Parse(XamlStr);

                    Result.Name = UserName;
                    Result.XamlGraphic = element;
                    Result.CRC32 = PegaseCore.XMLTools.CalculCRCStr(XamlStr);

                    // Initialiser la possibilité de déplacement de l'objet
                    Result.IsALibraryElement = true;
                    _rootCanvas = XamlElement.GetRootCanvas();
                }
                else
                {
                    Result = null;
                }
            }
            catch
            {
                Result = null;
            }

            return Result;
        } // endMethod: Parse


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
