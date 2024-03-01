using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace JAY.WpfCore
{
    /// <summary>
    /// Une bibliothèque d'objets Xaml chargés dynamiquement et affichable / positionnable
    /// </summary>
    public class XamlLibrary
    {
        // Variables
        #region Variables

        private const String SEARCH_STRING = "*.xaml";

        private ObservableCollection<XamlElement> _collectionXamlElement;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La collection des elements Xaml chargés
        /// </summary>
        public ObservableCollection<XamlElement> CollectionXamlElement
        {
            get
            {
                if (this._collectionXamlElement == null)
                {
                    this._collectionXamlElement = new ObservableCollection<XamlElement>();
                }
                return this._collectionXamlElement;
            }
        } // endProperty: CollectionXamlElement

        #endregion

        // Constructeur
        #region Constructeur

        private XamlLibrary()
        {
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Lire la bibliothèque de ficher Xaml au chemin spécifié
        /// </summary>
        public static XamlLibrary Load ( String Path )
        {
            XamlLibrary Result = new XamlLibrary();
            String[] Files;

            // 1 - récupérer la liste des fichiers
            Files = Directory.GetFiles(Path, SEARCH_STRING);

            if (Files != null)
            {
                if (Files.Length > 0)
                {
                    foreach (String FileName in Files)
                    {
                        XamlElement element = XamlElement.Load(FileName);
                        Result.CollectionXamlElement.Add(element);
                    }
                }
            }

            return Result;
        } // endMethod: Load

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: XamlLibrary
}