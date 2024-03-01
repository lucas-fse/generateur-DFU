using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using JAY.WpfCore;
using JAY.XMLCore;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.IO.Packaging;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Les données du plastron
    /// </summary>
    public class PlastronData
    {
        // Variables
        #region Variables

        private ObservableCollection<XamlElement> _labelPlastrons;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La collection des données libellées du plastron
        /// </summary>
        public ObservableCollection<XamlElement> LabelPlastrons
        {
            get
            {
                if (this._labelPlastrons == null)
                {
                    this._labelPlastrons = new ObservableCollection<XamlElement>();
                }
                return this._labelPlastrons;
            }
        } // endProperty: LabelPlastrons

        #endregion

        // Constructeur
        #region Constructeur

        public PlastronData()
        {
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Sauvegarder les données du plastron
        /// </summary>
        public void Save ( )
        {
            if (this.LabelPlastrons.Count > 0)
            {
                // 1 - supprimer la section plastron du XML si elle existe déjà
                ObservableCollection<XElement> PlastronData = PegaseData.Instance.XMLFile.GetNodeByPath("Plastron");
                if (PlastronData != null && PlastronData.Count > 0)
                {
                    foreach (var plastron in PlastronData)
                    {
                        plastron.Remove();
                    }
                }

                // 2 - créer la section plastron en entier
                XElement Plastron = new XElement("Plastron");
                XAttribute codePlastron = new XAttribute(XML_ATTRIBUTE.CODE, "Plastron");
                Plastron.Add(codePlastron);

                if (PegaseData.Instance.CurrentPackage != null)
                {
                    ZipPackagePart ZPP;
                    if (!PegaseData.Instance.CurrentPackage.IsOpen)
                    {
                        PegaseData.Instance.CurrentPackage.OpenPackage();
                    }
                    // 2.5 - pour tous les XamlElements, sauver leur état
                    foreach (XamlElement label in this.LabelPlastrons)
                    {
                        String fileName = String.Format("{0}{1}.xaml", DefaultValues.Get().PlastronGraphicFolder, label.Name);
                        if (File.Exists(fileName))
                        {
                            XElement LabelData = new XElement("Label");

                            XAttribute codeLabel = new XAttribute(XML_ATTRIBUTE.CODE, "Label");
                            LabelData.Add(codeLabel);

                            XAttribute file = new XAttribute(XML_ATTRIBUTE.File, label.Name);
                            LabelData.Add(file);

                            XAttribute locationX = new XAttribute(XML_ATTRIBUTE.LOCATIONX, label.Position.X.ToString());
                            LabelData.Add(locationX);

                            XAttribute locationY = new XAttribute(XML_ATTRIBUTE.LOCATIONY, label.Position.Y.ToString());
                            LabelData.Add(locationY);

                            XAttribute CRC32 = new XAttribute(XML_ATTRIBUTE.CRC32, label.CRC32.ToString());
                            LabelData.Add(CRC32);

                            Plastron.Add(LabelData);

                            // Enregistrer les labels 'Xaml' dans le fichier
                            String UriStr = String.Format("/Xaml/{0}.xaml", label.CRC32);
                            Uri uri = new Uri(UriStr, UriKind.Relative);

                            // Vérifier si l'uri existe déjà
                            try
                            {
                                ZPP = (ZipPackagePart)PegaseData.Instance.CurrentPackage.CurrentPackage.GetPart(uri);
                                if (ZPP != null)
                                {
                                    PegaseData.Instance.CurrentPackage.CurrentPackage.DeletePart(uri);
                                }
                            }
                            catch
                            {
                            }

                            ZPP = (ZipPackagePart)PegaseData.Instance.CurrentPackage.CurrentPackage.CreatePart(uri, "application/xaml", System.IO.Packaging.CompressionOption.Maximum);
                            Stream partStream = ZPP.GetStream();
                            String FileContenu;
                            try
                            {
                                using (StreamReader SR = new StreamReader(File.OpenRead(fileName)))
                                {
                                    FileContenu = SR.ReadToEnd();
                                }
                                using (StreamWriter SW = new StreamWriter(partStream))
                                {
                                    SW.Write(FileContenu);
                                    SW.Flush();
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    if (PegaseData.Instance.CurrentPackage.IsOpen)
                    {
                        PegaseData.Instance.CurrentPackage.ClosePackage();
                    }
                }

                // 3 - insérer la section plastron dans le XML
                PegaseData.Instance.XMLRoot.AddFirst(Plastron);
            }
        } // endMethod: Save
        
        /// <summary>
        /// Charger les données du plastron
        /// </summary>
        public void Load ( )
        {
            ObservableCollection<XElement> PlastronData = PegaseData.Instance.XMLFile.GetNodeByPath("Plastron");
            this.LabelPlastrons.Clear();

            if (PlastronData != null)
            {
                XElement Plastron = PlastronData.First();

                IEnumerable<XElement> Labels = Plastron.Descendants("Label");

                foreach (XElement label in Labels)
                {
                    XamlElement element;
                    String name = label.Attribute(XML_ATTRIBUTE.File).Value;

                    String StrX = label.Attribute(XML_ATTRIBUTE.LOCATIONX).Value;

                    String StrY = label.Attribute(XML_ATTRIBUTE.LOCATIONY).Value;

                    StrX = XMLCore.Tools.FixFloatStringSeparator(StrX);
                    StrY = XMLCore.Tools.FixFloatStringSeparator(StrY);

                    Double x = Convert.ToDouble(StrX);
                    Double y = Convert.ToDouble(StrY);

                    String StrCRC32 = label.Attribute(XML_ATTRIBUTE.CRC32).Value;
                    UInt32 CRC32 = Convert.ToUInt32(StrCRC32);

                    element = new XamlElement(name, x, y, CRC32);
                    this.LabelPlastrons.Add(element);
                }
            }
        } // endMethod: Load

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: PlastronData
}
