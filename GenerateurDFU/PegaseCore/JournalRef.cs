using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Le singleton permettant d'interpréter le journal des produits
    /// </summary>
    public class JournalRef
    {
        // Variables singleton
        private static JournalRef _instance;
        static readonly object instanceLock = new object();

        // Variables
        #region Variables
        private ObservableCollection<JournalRefLine> _journalRefs;
        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La liste des erreurs référencées
        /// </summary>
        public ObservableCollection<JournalRefLine> JournalRefs
        {
            get
            {
                if (this._journalRefs == null)
                {
                    this._journalRefs = new ObservableCollection<JournalRefLine>();
                }
                return this._journalRefs;
            }
            private set
            {
                this._journalRefs = value;
            }
        } // endProperty: JournalRefs

        /// <summary>
        /// L'instance unique de la classe
        /// </summary>
        public static JournalRef Instance
        {
            get
            {
                return JournalRef.Get();
            }
        } // endProperty: Instance

        #endregion

        // Constructeur
        #region Constructeur

        private JournalRef()
        {
            // Charger le fichier de référence
            FileCore.FilePackage FP = FileCore.FilePackage.GetParamData();
            if (FP != null)
            {
                Uri paramUri = new Uri(DefaultValues.ERREUR_EMBARQUE_FILE, UriKind.Relative);
                Stream fs = FP.GetPartStream(paramUri);
                XDocument XD = XDocument.Load(fs);
                XElement Root = XD.Elements("Erreurs").First();

                if (Root != null)
                {
                    List<XElement> erreurs = Root.Elements("Erreur").ToList();

                    foreach (var erreur in erreurs)
                    {
                        JournalRefLine JRL = new JournalRefLine(erreur);
                        this.JournalRefs.Add(JRL);
                    }
                }

                FP.ClosePackage();
            }
        }

        #endregion

        // Méthodes
        #region Méthodes

        // Retourne une instance unique de la classe
        private static JournalRef Get()
        {
            lock (instanceLock)
            {
                if (_instance == null)
                    _instance = new JournalRef();


                return _instance;
            }
        }

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: JournalRef
}
