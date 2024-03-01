using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Un paramétrage de gains et offset de référence
    /// </summary>
    public class ParamsGainOffset
    {
        // Variables
        #region Variables

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le nom de ce paramétrage
        /// </summary>
        public String Name
        {
            get;
            private set;
        } // endProperty: Name

        /// <summary>
        /// Le gain associé à cet élément pour les entrées
        /// </summary>
        public Single GainE
        {
            get;
            private set;
        } // endProperty: Gain

        /// <summary>
        /// L'offset associé à cet élément pour les entrées
        /// </summary>
        public Single OffsetE
        {
            get;
            private set;
        } // endProperty: Offset

        /// <summary>
        /// Le gain pour les sorties
        /// </summary>
        public Single GainS
        {
            get;
            private set;
        } // endProperty: GainS

        /// <summary>
        /// L'offset pour les sorties
        /// </summary>
        public Single OffsetS
        {
            get;
            private set;
        } // endProperty: OffsetS

        /// <summary>
        /// La valeur minimal en tension ou en courant
        /// </summary>
        public Single Min
        {
            get;
            private set;
        } // endProperty: Min

        /// <summary>
        /// La valeur max en tension ou en courant
        /// </summary>
        public Single Max
        {
            get;
            private set;
        } // endProperty: Max

        /// <summary>
        /// Le type de données (Tension / Intensité)
        /// </summary>
        public String Type
        {
            get;
            private set;
        } // endProperty: Type

        /// <summary>
        /// L'unité de mesure
        /// </summary>
        public String Unit
        {
            get;
            set;
        } // endProperty: Unit

        /// <summary>
        /// Le typeUI
        /// </summary>
        public Int32 TypeUI
        {
            get;
            private set;
        } // endProperty: TypeUI

        #endregion

        // Constructeur
        #region Constructeur

        public ParamsGainOffset(XElement Params)
        {
            // Parse le XML et charge les bonnes données

            this.Name = Params.Attribute("key").Value;
            
            String G = XMLCore.Tools.FixFloatStringSeparator(Params.Attribute("gain").Value);
            if (G != "")
            {
                this.GainE = Convert.ToSingle(G); 
            }
            else
            {
                this.GainE = 1;
            }
            
            String O = XMLCore.Tools.FixFloatStringSeparator(Params.Attribute("offset").Value);
            if (O != "")
            {
                this.OffsetE = Convert.ToSingle(O); 
            }
            else
            {
                this.OffsetE = 0;
            }

            String GS = XMLCore.Tools.FixFloatStringSeparator(Params.Attribute("gainS").Value);
            if (GS != "")
            {
                this.GainS = Convert.ToSingle(GS);
            }
            else
            {
                this.GainS = 1;
            }

            String OS = XMLCore.Tools.FixFloatStringSeparator(Params.Attribute("offsetS").Value);
            if (OS != "")
            {
                this.OffsetS = Convert.ToSingle(OS);
            }
            else
            {
                this.OffsetS = 0;
            }

            String M = XMLCore.Tools.FixFloatStringSeparator(Params.Attribute("max").Value);
            if (M != "")
            {
                this.Max = Convert.ToSingle(M); 
            }
            
            String m = XMLCore.Tools.FixFloatStringSeparator(Params.Attribute("min").Value);
            if (m != "")
            {
                this.Min = Convert.ToSingle(m); 
            }
            
            this.Type = Params.Attribute("type").Value;
            this.Unit = Params.Attribute("unite").Value;
            String tui = Params.Attribute("TypeUI").Value;
            if (tui != "")
            {
                this.TypeUI = Convert.ToInt32(tui);
            }
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: ParamsGainOffset
	
    /// <summary>
    /// La classe de base contenant les informations de mode (tension / courant)
    /// Et les gains / offsets associés
    /// </summary>
    /// <see cref="RefGainOffset"/>
    public class CollectionGainOffset
	{
        // Variables
        #region Variables

        private Dictionary<String, ParamsGainOffset> _parmsGainOffset;

        #endregion
        
        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le type de l'entrée / sortie (I ou O) utilisé dans la nouvelle stratégie définissant des types IO par matériel
        /// </summary>
        public String TypeIO
        {
            get;
            set;
        } // endProperty: TypeIO

        /// <summary>
        /// Le type du mode actuel (courant ou tension)
        /// </summary>
        public Dictionary<String, ParamsGainOffset> PGainOffset
        {
            get
            {
                return this._parmsGainOffset;
            }
            private set
            {
                this._parmsGainOffset = value;
            }
        } // endProperty: ModeType

        #endregion
        
        // Constructeur
        #region Constructeur
        
        public CollectionGainOffset(XElement Params)
        {
            // parcourir tous les types
            var types = from type in Params.Descendants("Type").Descendants("Couple")
                        select type;
            this._parmsGainOffset = new Dictionary<string, ParamsGainOffset>();

            foreach (var item in types)
            {
                String Key = item.Attribute("key").Value;
                if (Key.Trim() != "")
                {
                    ParamsGainOffset P = new ParamsGainOffset(item);
                    this._parmsGainOffset.Add(Key, P); 
                }
            }
        }
        
        #endregion
        
        // Méthodes
        #region Méthodes
        
        #endregion
        
        // Messages
        #region Messages
        
        #endregion
        
    } // endClass: GainOffset

    public class RefGainOffset
    {
        #region Constantes

        private const String REFGAINOFFSET_URI = "/RefGainOffset.xml";

        #endregion

        // Variables singleton
        private static RefGainOffset _instance;
        static readonly object instanceLock = new object();

        // Variables
        #region Variables

        private Dictionary<String, CollectionGainOffset> _materials;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La collection des matériels disponibles
        /// </summary>
        public Dictionary<String, CollectionGainOffset> Materials
        {
            get
            {
                return this._materials;
            }
            private set
            {
                this._materials = value;
            }
        } // endProperty: Material

        #endregion

        // Constructeur
        #region Constructeur

        /// <summary>
        /// Constructeur
        /// Charge et parse les données du fichier RefGainOffset issus du fichier
        /// params.data
        /// </summary>
        private RefGainOffset()
        {
            // Ouvrir le fichier de données
            FileCore.FilePackage package = FileCore.FilePackage.GetParamData();
            Uri RefGainOffsetUri = new Uri(REFGAINOFFSET_URI, UriKind.Relative);
            Stream stream = package.GetPartStream(RefGainOffsetUri);

            XDocument document = XDocument.Load(stream);
            package.ClosePackage();

            // Parser le document XML et construire les valeurs possibles
            Dictionary<String, CollectionGainOffset> Materials = new Dictionary<string, CollectionGainOffset>();

            // 1 - dans le cas des anciens types UI (même type UI disponible pour toutes les cartes présentes)
            IEnumerable<XElement> Mat = from material in document.Descendants("Root").Descendants("Material")
                                        select material;

            foreach (var mat in Mat)
            {
                String Key = mat.Attribute("key").Value;
                CollectionGainOffset p = new CollectionGainOffset(mat);
                Materials.Add(Key, p);
            }

            // 2 - dans le cas des nouveaux types UI
            IEnumerable<XElement> IOs = from io in document.Descendants("Root").Descendants("Typees").Descendants("Caracteristique")
                  select io;

            foreach (XElement io in IOs)
            {
                String Key = io.Attribute("key").Value;
                CollectionGainOffset p = new CollectionGainOffset(io);
                p.TypeIO = io.Attribute("io").Value;
                Materials.Add(Key, p);
            }

            this._materials = Materials;
        }

        #endregion

        // Méthodes
        #region Méthodes

        // Retourne une instance unique de la classe
        public static RefGainOffset Get()
        {
            lock (instanceLock)
            {
                if (_instance == null)
                    _instance = new RefGainOffset();

                return _instance;
            }
        }

        /// <summary>
        /// Acquérire les paramètres par matériel
        /// </summary>
        public ObservableCollection<ParamsGainOffset> GetParamsByMaterial ( String MaterialName )
        {
            ObservableCollection<ParamsGainOffset> Result;
            Result = new ObservableCollection<ParamsGainOffset>();

            if (MaterialName != "INCONNU" || MaterialName != "UNKNOW")
            {
                try
                {
                    CollectionGainOffset CGO = this.Materials[MaterialName];

                    foreach (var PGO in CGO.PGainOffset)
                    {
                        Result.Add(PGO.Value);
                    }
                }
                catch
                {
                    // Renvoyer une collection vide
                    // Problème connu -> la refIndus du produit est erronnée ou absente dans le xml technique
                }
            }

            return Result;
        } // endMethod: GetParamsByMaterial
        
        /// <summary>
        /// Acquérir le paramètre matériel décrit par le nom du matériel et le nom du mode
        /// </summary>
        public ParamsGainOffset GetParamByMaterialAndName(String MaterialName, String ModeName)
        {
            ParamsGainOffset Result;

            ObservableCollection<ParamsGainOffset> CollectionParam = this.GetParamsByMaterial(MaterialName);
            var Query = from param in CollectionParam
                        where param.Name == ModeName
                        select param;

            Result = Query.FirstOrDefault();

            return Result;
        } // endMethod: GetParamByMaterialAndName
        
        /// <summary>
        /// Acquérir les paramètres par le type UI et le matériel
        /// </summary>
        public ParamsGainOffset GetParamByMaterialAndTypeUI ( String MaterialName, TypeUI UIType )
        {
            ParamsGainOffset Result;

            ObservableCollection<ParamsGainOffset> CollectionParam = this.GetParamsByMaterial(MaterialName);
            var Query = from param in CollectionParam
                        where param.TypeUI == (Int32)UIType
                        select param;

            Result = Query.FirstOrDefault();

            return Result;
        } // endMethod: GetParamByMaterialAndTypeUI


        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: RefGainOffset
}
