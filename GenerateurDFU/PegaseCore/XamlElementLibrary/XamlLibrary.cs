using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using JAY.FileCore;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Une bibliothèque d'objets Xaml chargés dynamiquement et affichable / positionnable
    /// </summary>

    public class XamlLibrary
    {
        // Variables singleton
        private static XamlLibrary _instance;
        static readonly object instanceLock = new object();

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
                    if (PegaseData.Instance != null && PegaseData.Instance.CurrentPackage != null)
                    {
                        this._collectionXamlElement = new ObservableCollection<XamlElement>();
                        this.Load(DefaultValues.Get().PlastronGraphicFolder, PegaseData.Instance.CurrentPackage);
                    }
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

        // Retourne une instance unique de la classe
        public static XamlLibrary Get()
        {
            lock (instanceLock)
            {
                if (_instance == null)
                    _instance = new XamlLibrary();


                return _instance;
            }
        }
        
        /// <summary>
        /// Supprimer un élément sélectionné
        /// </summary>
        public void DeleteSelectedElement ( )
        {
            for(Int32 i = this.CollectionXamlElement.Count - 1; i >= 0; i--)
            {
                XamlElement item = this.CollectionXamlElement[i];
                if (item.IsSelected)
                {
                    // supprimer uniquement les fichiers de la bibliothèque local, pas ceux de la fiche
                    String fileName = DefaultValues.Get().PlastronGraphicFolder + item.Name + ".xaml";
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                        this.CollectionXamlElement.Remove(item);
                    }
                }
            }
        } // endMethod: DeleteSelectedElement

        /// <summary>
        /// Ajouter un element Xaml
        /// </summary>
        public void AddXamlElement ( String XamlSource, String name, String UserName )
        {
            XamlElement element;

            element = XamlElement.Parse(XamlSource, name, UserName);
            if (element != null)
            {
                this._collectionXamlElement.Add(element);
            }
        } // endMethod: AddXamlElement

        /// <summary>
        /// Lire la bibliothèque de ficher Xaml au chemin spécifié
        /// </summary>
        private void Load(String Path)
        {
            String[] Files;

            // 1 - récupérer la liste des fichiers
            XamlLibrary.Get()._collectionXamlElement.Clear();
            Files = Directory.GetFiles(Path, SEARCH_STRING);

            if (Files != null)
            {
                if (Files.Length > 0)
                {
                    Int32 i = 0;
                    foreach (String FileName in Files)
                    {
                        String name = String.Format("Element{0:0000}", i);
                        i++;
                        XamlElement element = XamlElement.Load(FileName, name);
                        if (element != null)
                        {
                            XamlLibrary.Get()._collectionXamlElement.Add(element);
                        }
                    }
                }
            }
        } // endMethod: Load

        /// <summary>
        /// Charger une bibliothèque localiser à un emplacement disque, 
        /// cumulée à la bibliothèque présente dans le package idialog
        /// </summary>
        private void Load( String Path, iDialogPackage idialogFile)
        {
            this.Load(Path);

            // 1 - récupérer la liste des fichiers .xaml
            ObservableCollection<String> xamlFiles = idialogFile.GetXamlImages();

            // 2 - charger les fichiers et les incorporer
            if (xamlFiles.Count > 0)
            {
                foreach (String xamlFile in xamlFiles)
                {
                    String fileContent;
                    Uri partUri = new Uri(xamlFile, UriKind.Relative);
                    Stream stream = idialogFile.GetPartStream(partUri);
                    using (StreamReader SR = new StreamReader(stream))
                    {
                        fileContent = SR.ReadToEnd();
                    }

                    // Récupérer le CRC32 du fichier
                    String strCRC32 = System.IO.Path.GetFileNameWithoutExtension(xamlFile);
                    UInt32 CRC32 = Convert.ToUInt32(strCRC32);

                    // Si le CRC32 n'existe pas dans la bibliothèque -> insérer le fichier dans la bibliothèque
                    var QueryCRC32 = from xfile in XamlLibrary.Get()._collectionXamlElement
                                     where xfile.CRC32 == CRC32
                                     select xfile;

                    if (QueryCRC32.Count() == 0)
                    {
                        // Enregistrer le fichier dans la bibliothèque
                        String fileDestName = DefaultValues.Get().PlastronGraphicFolder + strCRC32 + ".xaml";
                        Stream fileDestStream = File.Create(fileDestName);

                        using (StreamWriter SW = new StreamWriter(fileDestStream))
                        {
                            SW.Write(fileContent);
                            SW.Flush();
                        }

                        // Charger le fichier depuis la bibliothèque
                        XamlElement element = XamlElement.Parse(fileContent, fileDestName, strCRC32);
                        if (element != null)
                        {
                            XamlLibrary.Get().CollectionXamlElement.Add(element);
                        }
                    }
                }
            }
            idialogFile.ClosePackage();
        } // endMethod: Load

        /// <summary>
        /// Retrouver un élément suivant son nom
        /// </summary>
        public UIElement GetElement ( String Name )
        {
            UIElement Result = null;

            var Query = from element in CollectionXamlElement
                        where element.Name == Name
                        select element;

            if (Query.Count() > 0)
            {
                Result = Query.First().XamlGraphic;
            }

            return Result;
        } // endMethod: GetElement

        /// <summary>
        /// Retrouver un élément suivant son nom
        /// </summary>
        public UIElement GetElement(UInt32 CRC32)
        {
            UIElement Result = null;

            var Query = from element in CollectionXamlElement
                        where element.CRC32 == CRC32
                        select element;

            if (Query.Count() > 0)
            {
                Result = Query.First().XamlGraphic;
            }

            return Result;
        } // endMethod: GetElement

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: XamlLibrary
}