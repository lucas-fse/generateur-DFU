using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mvvm = GalaSoft.MvvmLight;

namespace JAY.PegaseCore
{
    public enum TypeFiche
    {
           TYPE0,
           TYPE1,
           TYPE2,
           TYPE3 } 
            ;

    public class LutteAntiGFI : Mvvm.ViewModelBase
    {
        // Variables singleton
        private static LutteAntiGFI _instance;
        static readonly object instanceLock = new object();



        // Variables
        #region Variables
        private Int32 _currentCompileEtape;
        private Int32 _nbrCompileEtape;
        private Int32 _nbrFlashEtape;
        private Int32 _currentFlashPos;
        private Boolean _flashIsOK;

        private Boolean _verifyWriteProduct;
        private Int32 _nbWriteError;
        private String _errorMessage;
        private ListErreurGeneration _erreurGenerationList;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// List des erreurs de générations
        /// </summary>
        public ListErreurGeneration ErreurGenerationList
        {
            get
            {
                if (this._erreurGenerationList == null)
                {
                    this._erreurGenerationList = new ListErreurGeneration();
                }
                
                return this._erreurGenerationList;
            }
            set
            {
                this._erreurGenerationList = value;
                RaisePropertyChanged("ErreurGenerationList");
            }
        } // endProperty: ErreurGenerationList

        /// <summary>
        /// Doit-on vérifier l'écriture dans les produits?
        /// </summary>
        public Boolean VerifyWriteProduct
        {
            get
            {
                return this._verifyWriteProduct;
            }
            set
            {
                this._verifyWriteProduct = value;
                RaisePropertyChanged("VerifyWriteProduct");
            }
        } // endProperty: VerifyWriteProduct

        /// <summary>
        /// Le nombre d'erreur en écriture
        /// </summary>
        public Int32 NbWriteError
        {
            get
            {
                return this._nbWriteError;
            }
            set
            {
                this._nbWriteError = value;
                RaisePropertyChanged("NbWriteError");
            }
        } // endProperty: NbWriteError

        /// <summary>
        /// Retourne un message d'erreur
        /// </summary>
        public String ErrorMessage
        {
            get
            {
                return this._errorMessage;
            }
            set
            {
                this._errorMessage = value;
                RaisePropertyChanged("ErrorMessage");
            }
        } // endProperty: ErrorMessage

        /// <summary>
        /// L'instance unique de la classe
        /// </summary>
        public static LutteAntiGFI Instance
        {
            get
            {
                return LutteAntiGFI.Get();
            }
        } // endProperty: Instance

        /// <summary>
        /// Le flashage est effectué avec succès (true) ou avec erreurs (false)
        /// </summary>
        public Boolean FlashIsOK
        {
            get
            {
                return this._flashIsOK;
            }
            set
            {
                this._flashIsOK = value;
                RaisePropertyChanged("FlashIsOK");
            }
        } // endProperty: FlashIsOK

        /// <summary>
        /// Le type de la fiche (false = 32 ko / true = 128 ko)
        /// </summary>
        public TypeFiche FicheExtension
        {
            get;
            set;
        } // endProperty: FicheExtension

        /// <summary>
        /// L'extention est utilisée
        /// </summary>
        public TypeFiche EtatTypeFiche
        {
            get;
            set;
        } // endProperty: EtatTypeFiche

        /// <summary>
        /// L'étape de compilation en cours
        /// </summary>
        public Int32 CurrentCompileEtape
        {
            get
            {
                return this._currentCompileEtape;
            }
            set
            {
                this._currentCompileEtape = value;
                RaisePropertyChanged("CurrentCompileEtape");
            }
        } // endProperty: CurrentCompileEtape

        /// <summary>
        /// Le nombre d'étape de compilation
        /// </summary>
        public Int32 NbrCompileEtape
        {
            get
            {
                return this._nbrCompileEtape;
            }
            set
            {
                this._nbrCompileEtape = value;
                RaisePropertyChanged("NbrCompileEtape");
            }
        } // endProperty: MaxCompileEtape

        /// <summary>
        /// Position de flashage en cours
        /// </summary>
        public Int32 CurrentFlashPos
        {
            get
            {
                return this._currentFlashPos;
            }
            set
            {
                this._currentFlashPos = value;
                RaisePropertyChanged("CurrentFlashPos");
            }
        } // endProperty: CurrentFlashPos

        /// <summary>
        /// Nombre d'étape de flashage
        /// </summary>
        public Int32 NbrFlashEtape
        {
            get
            {
                return this._nbrFlashEtape;
            }
            set
            {
                this._nbrFlashEtape = value;
                RaisePropertyChanged("NbrFlashEtape");
            }
        } // endProperty: NbrFlashEtape

        #endregion

        // Constructeur
        #region Constructeur

        private LutteAntiGFI()
        {
        }

        #endregion

        // Méthodes
        #region Méthodes

        // Retourne une instance unique de la classe
        private static LutteAntiGFI Get()
        {
            lock (instanceLock)
            {
                if (_instance == null)
                    _instance = new LutteAntiGFI();


                return _instance;
            }
        }

        #endregion

        // Messages
        #region Messages

        public String GetFormatMessage(String message,params object[] Parametres)
        {
            String message_lang="";
            message_lang = JAY.LanguageSupport.Get().GetText(message);
            return message_lang;
        }
        #endregion

    } // endClass: LutteAntiGFI
}
