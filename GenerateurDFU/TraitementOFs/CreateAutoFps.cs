using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Net;
using System.Windows.Input;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using JAY.FileCore;
using JAY.XMLCore;
using JAY.PegaseCore;
using JAY;

namespace TraitementOFs
{
    public class VarianteElement
    {
        /// <summary>
        /// Le nom de la variante
        /// </summary>
        public String Name
        {
            get;
            set;
        } // endProperty: Name

        /// <summary>
        /// Le nom du fichier à inclure
        /// </summary>
        public String FileName
        {
            get;
            set;
        } // endProperty: FileName
    }

    public enum ModeExecution
    {
        CreateMode,
        ModifyMode
    }

    public class CreateAutoFPS 
    {
        #region Constante

        private const String ROOT_PATH = "/Parts";

        #endregion

        #region Variables

        private ObservableCollection<String> _langFiles;
        private ObservableCollection<String> _pictureFiles;

        private String _moLabel;
        private String _mtLabel;
        private String _optionLogicielLabel;
        private String _refMO;
        private String _refMT;
        private String _refSIM;
        private String _resumeOptions;

        private String _fpaName;
        private String _fpsName;
        private String _projectName;

        private ObservableCollection<String> _listBaseFiles;
        private ObservableCollection<VarianteElement> _listMOFiles;
        private ObservableCollection<VarianteElement> _listMTFiles;
        private ObservableCollection<VarianteElement> _listSIMFiles;

        #endregion

        #region Propriétés

        /// <summary>
        /// Le libellé du configurateur
        /// </summary>
        public String LibelConfigurateur
        {
            get
            {
                return "WPF_CON/BTN_CONFIGURATEUR";
            }
        } // endProperty: Libel

        /// <summary>
        /// Le libellé pour le bouton coller
        /// </summary>
        public String LibelPaste
        {
            get
            {
                return "WPF_CON/BTN_PASTE";
            }
        } // endProperty: LibelPaste

        /// <summary>
        /// Le libellé pour le bouton créer
        /// </summary>
        public String LibelCreate
        {
            get
            {
                return "WPF_CON/BTN_CREATE";
            }
        } // endProperty: LibelCreate

        /// <summary>
        /// Le libellé pour la référence FPA
        /// </summary>
        public String LibelRefFPA
        {
            get
            {
                return "WPF_CON/LBL_REFFPA";
            }
        } // endProperty: LibelRefFPA

        /// <summary>
        /// Le libellé pour le nom du projet
        /// </summary>
        public String LibelProjectName
        {
            get
            {
                return "WPF_CON/LBL_PROJECTNAME";
            }
        } // endProperty: LibelProjectName

        /// <summary>
        /// Le libellé pour les fichiers de langues
        /// </summary>
        public String LibelLangueFiles
        {
            get
            {
                return "WPF_CON/LBL_LANGUEFILES";
            }
        } // endProperty: LibelLangueFiles

        /// <summary>
        /// Le libellé des fichiers d'images
        /// </summary>
        public String LibelPicturesFiles
        {
            get
            {
                return "WPF_CON/LBL_PICTUREFILES";
            }
        } // endProperty: LibelPicturesFiles

        /// <summary>
        /// Le libellé pour le MO
        /// </summary>
        public String LibelMO
        {
            get
            {
                return "WPF_CON/LBL_MO";
            }
        } // endProperty: LibelMO

        /// <summary>
        /// Le libellé du MT
        /// </summary>
        public String LibelMT
        {
            get
            {
                return "WPF_CON/LBL_MT";
            }
        } // endProperty: LibelMT

        /// <summary>
        /// Le libellé de la SIM
        /// </summary>
        public String LibelSIM
        {
            get
            {
                return "WPF_CON/LBL_SIM";
            }
        } // endProperty: LibelSIM

        /// <summary>
        /// Libellé pour le bouton modifier
        /// </summary>
        public String LibelModify
        {
            get
            {
                return "WPF_CON/LBL_MODIFY";
            }
        } // endProperty: LibelModify

        /// <summary>
        /// Le libellé appliquer
        /// </summary>
        public String LibelApply
        {
            get
            {
                return "WPF_CON/LBL_APPLY";
            }
        } // endProperty: LibelApply

        /// <summary>
        /// Le nom de la FPA jointe
        /// </summary>
        public String FPAName
        {
            get
            {
                return this._fpaName;
            }
            set
            {
                this._fpaName = value;
               
            }
        } // endProperty: FPAName

        /// <summary>
        /// Le nom du projet (pour la sauvegarde)
        /// </summary>
        public String ProjectName
        {
            get
            {
                return this._projectName;
            }
            set
            {
                this._projectName = value;
             
            }
        } // endProperty: ProjectName

        /// <summary>
        /// Le nom de la FPS jointe
        /// </summary>
        public String FPSName
        {
            get
            {
                return this._fpsName;
            }
            set
            {
                this._fpsName = value;
               
            }
        } // endProperty: FPSName

        /// <summary>
        /// La liste des fichiers de base
        /// </summary>
        public ObservableCollection<String> ListBaseFiles
        {
            get
            {
                return this._listBaseFiles;
            }
            private set
            {
                this._listBaseFiles = value;
               
            }
        } // endProperty: ListBaseFiles

        /// <summary>
        /// La liste des fichiers MO
        /// </summary>
        public ObservableCollection<VarianteElement> ListMOFiles
        {
            get
            {
                return this._listMOFiles;
            }
            private set
            {
                this._listMOFiles = value;
               
            }
        } // endProperty: ListMOFiles

        /// <summary>
        /// La liste des fichiers MT
        /// </summary>
        public ObservableCollection<VarianteElement> ListMTFiles
        {
            get
            {
                return this._listMTFiles;
            }
            private set
            {
                this._listMTFiles = value;
            
            }
        } // endProperty: ListMTFiles

        /// <summary>
        /// La liste des fichiers 'Options logiciels' portés par la SIM
        /// </summary>
        public ObservableCollection<VarianteElement> ListSIMFiles
        {
            get
            {
                return this._listSIMFiles;
            }
            set
            {
                this._listSIMFiles = value;
             
            }
        } // endProperty: ListSIMFiles

        /// <summary>
        /// Le libellé du MO
        /// </summary>
        public String MOLabel
        {
            get
            {
                return this._moLabel;
            }
            set
            {
                this._moLabel = value;
              
            }
        } // endProperty: MOLabel

        /// <summary>
        /// Le libellé du MT
        /// </summary>
        public String MTLabel
        {
            get
            {
                return this._mtLabel;
            }
            set
            {
                this._mtLabel = value;
               
            }
        } // endProperty: MTLabel

        /// <summary>
        /// Libellé des options logiciels
        /// </summary>
        public String OptionLogicielLabel
        {
            get
            {
                return this._optionLogicielLabel;
            }
            set
            {
                this._optionLogicielLabel = value;
             
            }
        } // endProperty: OptionLogicielLabel

        /// <summary>
        /// La référence du MO
        /// </summary>
        public String RefMO
        {
            get
            {
                return this._refMO;
            }
            set
            {
                this._refMO = value;
           
            }
        } // endProperty: RefMO

        /// <summary>
        /// La référence du MT
        /// </summary>
        public String RefMT
        {
            get
            {
                return this._refMT;
            }
            set
            {
                this._refMT = value;
            
            }
        } // endProperty: RefMT

        /// <summary>
        /// La rférence SIM
        /// </summary>
        public String RefSIM
        {
            get
            {
                return this._refSIM;
            }
            set
            {
                this._refSIM = value;
           
            }
        } // endProperty: RefSIM

        /// <summary>
        /// Le résumé des options et des variantes
        /// </summary>
        public String ResumeOptions
        {
            get
            {
                return this._resumeOptions;
            }
            set
            {
                this._resumeOptions = value;
            }
        } // endProperty: ResumeOptions

        /// <summary>
        /// La liste des fichiers de langues
        /// </summary>
        public ObservableCollection<String> LangFiles
        {
            get
            {
                ObservableCollection<String> langFiles = new ObservableCollection<String>();

                if (this._langFiles != null)
                {
                    foreach (String lfile in this._langFiles)
                    {
                        String filename;
                        filename = Path.GetFileNameWithoutExtension(lfile);
                        langFiles.Add(filename);
                    }
                }

                return langFiles;
            }
            private set
            {
                this._langFiles = value;
            }
        } // endProperty: LangFiles

        /// <summary>
        /// La liste des fichiers images
        /// </summary>
        public ObservableCollection<String> PictureFiles
        {
            get
            {
                ObservableCollection<String> pictureFiles = new ObservableCollection<String>();

                if (this._pictureFiles != null)
                {
                    foreach (String lfile in this._pictureFiles)
                    {
                        String filename;
                        filename = Path.GetFileNameWithoutExtension(lfile);
                        pictureFiles.Add(filename);
                    }
                }

                return pictureFiles;
            }
            private set
            {
                this._pictureFiles = value;
            }
        } // endProperty: PictureFiles

        #endregion

        public CreateAutoFPS()
        {
            // Initialiser les valeurs
            this.MOLabel = "MO";
            this.MTLabel = "MT";
            this.OptionLogicielLabel = "Options logicielles";

            this.InitiDialogLangFiles();
            this.InitiDialogPictureFiles();
        }

        #region Méthodes

        /// <summary>
        /// Construire un XML contenant la configuration matériel sélectionnée
        /// </summary>
        public XMLCreation BuildiDialogXML()
        {
            XMLCreation Result = null;

            // Concatener la liste des fichiers à assembler
            ObservableCollection<String> selectedRefFiles = new ObservableCollection<String>();

            // 1 - Fichiers de base
            foreach (var item in this.ListBaseFiles)
            {
                selectedRefFiles.Add(ROOT_PATH + item);
            }

            // 2 - Fichiers MO
            foreach (var item in this.ListMOFiles)
            {
                selectedRefFiles.Add(ROOT_PATH + item.FileName);
            }

            // 3 - Fichiers MT
            foreach (var item in this.ListMTFiles)
            {
                selectedRefFiles.Add(ROOT_PATH + item.FileName);
            }

            // 4 - Fusionner tous les fichiers sélectionnés
            XDocument FileXML;
            FilePackage package;

            FileXML = new XDocument();
            package = FilePackage.GetMaterielData();
            Uri BaseFileUri = new Uri(selectedRefFiles[0], UriKind.Relative);
            Stream refFileStream = package.GetPartStream(BaseFileUri);

            if (refFileStream != null)
            {
                FileXML = XDocument.Load(refFileStream);
                Result = new XMLCreation(FileXML);

                for (int i = 1; i < selectedRefFiles.Count; i++)
                {
                    XDocument XmlPart;

                    BaseFileUri = new Uri(selectedRefFiles[i], UriKind.Relative);
                    refFileStream = package.GetPartStream(BaseFileUri);
                    if (refFileStream != null)
                    {
                        XmlPart = XDocument.Load(refFileStream);
                        Result.Add(XmlPart);
                    }
                    else
                    {
                      //  MessageBox.Show(String.Format("Impossible to create config. The file {0} is unkown", selectedRefFiles[i]), "Warning", MessageBoxButton.OK);
                        Result = null;
                        break;
                    }
                }
            }

            package.ClosePackage();

            return Result;
        } // endMethod: BuildiDialogXML

        /// <summary>
        /// Initialiser la liste des fichiers de langues
        /// </summary>
        public void InitiDialogLangFiles()
        {
            NameValueCollection NVC = (NameValueCollection)ConfigurationManager.GetSection("PEGASE");
            String LangPath = "";

            if (NVC != null)
            {
                LangPath = NVC["FileiDialogLangPath"].ToString();
                if (!Directory.Exists(LangPath))
                {
                    LangPath = JAY.DefaultValues.Get().FileLanguageFolder;
                }
                if (Directory.Exists(LangPath))
                {
                    this._langFiles = new ObservableCollection<String>();
                    String[] Files = Directory.GetFiles(LangPath, "*.xml");
                    foreach (String file in Files)
                    {
                        this._langFiles.Add(file);
                    }
                }
            }
        } // endMethod: InitiDialogLangFiles

        /// <summary>
        /// Initialiser la liste des images à inclure par défaut dans le fichier iDialog
        /// </summary>
        public void InitiDialogPictureFiles()
        {
            NameValueCollection NVC = (NameValueCollection)ConfigurationManager.GetSection("PEGASE");
            String PicturePath = "";

            if (NVC != null)
            {
                PicturePath = NVC["FilePicturePath"].ToString();
                if (!Directory.Exists(PicturePath))
                {
                    // Chercher la copy de AppData
                    PicturePath = DefaultValues.Get().DefaultIconsFolder;
                }

                this._pictureFiles = new ObservableCollection<String>();

                // fichiers jpg
                if (Directory.Exists(PicturePath))
                {
                    String[] Files = Directory.GetFiles(PicturePath, "*.jpg");

                    foreach (String file in Files)
                    {
                        this._pictureFiles.Add(file);
                    }

                    // fichiers png
                    Files = Directory.GetFiles(PicturePath, "*.png");
                    foreach (String file in Files)
                    {
                        this._pictureFiles.Add(file);
                    }

                    // fichiers bmp
                    Files = Directory.GetFiles(PicturePath, "*.bmp");
                    foreach (String file in Files)
                    {
                        this._pictureFiles.Add(file);
                    }
                }
            }
        } // endMethod: InitiDialogPictureFiles

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void CreateFPSFromRef(String refMO, String refMT, String refSIM, String fileName)
        {
            this.RefMO = refMO;
            this.RefMT = refMT;
            this.RefSIM = refSIM;

            // Créer la liste des fichiers à importer
            XElement Root = this.RootConfigRefIndus();
            this.ListBaseFiles = this.BuildBaseFiles(Root);
            this.ListMOFiles = this.BuildListFiles(this.RefMO, Root);
            this.ListMTFiles = this.BuildListFiles(this.RefMT, Root);
            this.ListSIMFiles = this.BuildListFiles(this.RefSIM, Root);

            // construire le fichier .idialog
            this.BuildFile(fileName);

        } // endMethod: CreateFPSFromRef

        /// <summary>
        /// Obtenir l'élément racine du fichier ConfigRefIndus
        /// </summary>
        public XElement RootConfigRefIndus()
        {
            XElement Result = null;
            FilePackage Params = FilePackage.GetParamData();
            if (Params != null)
            {
                Stream refFileStream = Params.GetPartStream(DefaultValues.Get().ConfigRefIndusAppl);

                if (refFileStream != null)
                {
                    XDocument document = XDocument.Load(refFileStream);
                    Result = document.Root;
                }
                refFileStream.Close();
                Params.ClosePackage();
            }

            return Result;
        } // endMethod: RootConfigRefIndus

        /// <summary>
        /// Construire la liste des fichiers de base
        /// </summary>
        public ObservableCollection<String> BuildBaseFiles(XElement Root)
        {
            ObservableCollection<String> Result = new ObservableCollection<String>();

            if (Root != null)
            {
                foreach (var baseFile in Root.Descendants("Refbase"))
                {
                    Result.Add(baseFile.Attribute(XML_ATTRIBUTE.File).Value);
                }
            }

            return Result;
        } // endMethod: BuildBaseFiles

        /// <summary>
        /// Construire la liste des fichiers à incorporer en fonction de la référence 
        /// </summary>
        public ObservableCollection<VarianteElement> BuildListFiles(String Ref, XElement Root)
        {
            ObservableCollection<VarianteElement> Result = new ObservableCollection<VarianteElement>();

            if (Root != null)
            {
                foreach (var refProduit in Root.Descendants("Refproduit"))
                {
                    Int32 StartField, LengthField;
                    String FieldValue;

                    StartField = Convert.ToInt32(refProduit.Attribute(XML_ATTRIBUTE.OptionMaskDebut).Value);
                    LengthField = Convert.ToInt32(refProduit.Attribute(XML_ATTRIBUTE.OptionMaskTaille).Value);

                    FieldValue = Ref.Substring(StartField, LengthField);
                    if (refProduit.Attribute(XML_ATTRIBUTE.VALUE).Value == FieldValue)
                    {
                        // Sélectionner le produit et poursuivre le décodage
                        foreach (var produit in refProduit.Descendants("Produit"))
                        {
                            StartField = Convert.ToInt32(produit.Attribute(XML_ATTRIBUTE.OptionMaskDebut).Value);
                            LengthField = Convert.ToInt32(produit.Attribute(XML_ATTRIBUTE.OptionMaskTaille).Value);
                            if (Ref.Length >= StartField + LengthField)
                            {
                                FieldValue = Ref.Substring(StartField, LengthField);
                                // Vérifier quelle variante correspond à la valeur
                                foreach (var variante in produit.Descendants("Variante"))
                                {
                                    if (FieldValue == variante.Attribute(XML_ATTRIBUTE.VALUE).Value)
                                    {
                                        foreach (var filename in variante.Descendants("Fichier"))
                                        {
                                            VarianteElement VE = new VarianteElement();
                                            VE.Name = variante.Attribute(XML_ATTRIBUTE.Libelle).Value;
                                            VE.FileName = filename.Attribute(XML_ATTRIBUTE.Nom).Value;
                                            if (VE.FileName != "")
                                            {
                                                Result.Add(VE);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return Result;
        } // endMethod: BuildListFiles

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void BuildFile(String TempFileName)
        {
            // Assembler la fiche, la sauvegarder en base de donnée et l'ouvrir dans iDialog

            XMLCreation XFusion = this.BuildiDialogXML();
            if (XFusion != null)
            {
                // 1 - Créer un fichier iDialog
                iDialogPackage iDialogFile = new iDialogPackage();
                iDialogFile.CreateIDialogPackage(TempFileName, DefaultValues.Get().RefFileName, ProjectName, XFusion.DocumentXml.ToString(SaveOptions.None), this._langFiles);
                JAY.PegaseCore.XMLTools.ImportNewBitmaps(this._pictureFiles, iDialogFile);
                iDialogFile.ClosePackage();
            }
        } // endMethod: ExecuteCommandBuildFile

        #endregion
    }
}