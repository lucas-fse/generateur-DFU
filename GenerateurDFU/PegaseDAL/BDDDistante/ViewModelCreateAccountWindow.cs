using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Mvvm = GalaSoft.MvvmLight;
using System.Text.RegularExpressions;

namespace JAY.DAL.BDDDistante
{
    /// <summary>
    /// Le viewModel de la fenêtre CreateAccountWindow
    /// </summary>
    public class ViewModelCreateAccountWindow : Mvvm.ViewModelBase
    {
        #region Constantes

        private const Int32 LastNameIsOK = 0;
        private const Int32 FirstNameIsOK = 1;
        private const Int32 SocietyIsOK = 2;
        private const Int32 StreetIsOK = 3;
        private const Int32 TownIsOK = 4;
        private const Int32 ZipCodeIsOK = 5;
        private const Int32 EMailIsOK = 6;
        private const Int32 Password1IsOK = 7;
        private const Int32 Password1bIsOK = 8;
        private const Int32 PhoneNumberIsOK = 9;

        private const String REGEX_EMAIL = @"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b";

        #endregion

        // Variables
        #region Variables

        private String _lastName;
        private String _firstName;
        private String _society;
        private String _street;
        private String _town;
        private String _zipCode;
        private String _email;
        private String _password1;
        private String _password1b;
        private String _phoneNumber;
        private Boolean[] _checkDataIsOK;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Le libellé du nom
        /// </summary>
        public String LibelName
        {
            get
            {
                return LanguageSupport.Get().GetText("Plg_ConnexionT/CREATE_NAME");
            }
        } // endProperty: LibelName

        /// <summary>
        /// Le libellé pour le prénom
        /// </summary>
        public String LibelFirstName
        {
            get
            {
                return LanguageSupport.Get().GetText("Plg_ConnexionT/CREATE_FIRSTNAME");
            }
        } // endProperty: LibelFirstName

        /// <summary>
        /// Le libellé pour la société
        /// </summary>
        public String LibelSociety
        {
            get
            {
                return LanguageSupport.Get().GetText("Plg_ConnexionT/CREATE_SOCIETY");
            }
        } // endProperty: LibelSociety

        /// <summary>
        /// Le libellé pour le nom de la rue
        /// </summary>
        public String LibelStreet
        {
            get
            {
                return LanguageSupport.Get().GetText("Plg_ConnexionT/CREATE_STREET");
            }
        } // endProperty: LibelStreet

        /// <summary>
        /// Le libellé pour le nom de la ville
        /// </summary>
        public String LibelTown
        {
            get
            {
                return LanguageSupport.Get().GetText("Plg_ConnexionT/CREATE_TOWN");
            }
        } // endProperty: LibelTown

        /// <summary>
        /// Le libellé pour le ZipCode
        /// </summary>
        public String LibelZipCode
        {
            get
            {
                return LanguageSupport.Get().GetText("Plg_ConnexionT/CREATE_ZIPCODE");
            }
        } // endProperty: LibelZipCode

        /// <summary>
        /// Le libellé pour l'EMail
        /// </summary>
        public String LibelEMail
        {
            get
            {
                return LanguageSupport.Get().GetText("Plg_ConnexionT/CREATE_EMAIL");
            }
        } // endProperty: LibelEMail

        /// <summary>
        /// Le libellé pour le mot de passe
        /// </summary>
        public String LibelPassword
        {
            get
            {
                return LanguageSupport.Get().GetText("Plg_ConnexionT/CREATE_PASSWORD");
            }
        } // endProperty: LibelPassword

        /// <summary>
        /// Le libellé pour la vérification du mot de passe
        /// </summary>
        public String LibelPasswordCheck
        {
            get
            {
                return LanguageSupport.Get().GetText("Plg_ConnexionT/CREATE_PASSWORD_VERIF");
            }
        } // endProperty: LibelPasswordCheck

        /// <summary>
        /// Le libellé pour le téléphone
        /// </summary>
        public String LibelTelephone
        {
            get
            {
                return LanguageSupport.Get().GetText("Plg_ConnexionT/CREATE_TEL");
            }
        } // endProperty: LibelTelephone

        /// <summary>
        /// Le libellé pour le bouton création de compte
        /// </summary>
        public String LibelCreateAccount
        {
            get
            {
                return LanguageSupport.Get().GetText("Plg_ConnexionT/CREATE_ACCOUNT");
            }
        } // endProperty: LibelCreateAccount

        /// <summary>
        /// Le nom de la personne, ce champ est requis
        /// </summary>
        public String LastName
        {
            get
            {
                if (this._lastName == null)
                {
                    this._lastName = "";
                }
                return this._lastName;
            }
            set
            {
                this._lastName = value;
                if (this._lastName.Length == 0)
                {
                    this._checkDataIsOK[LastNameIsOK] = false;
                    throw new ApplicationException("LastName is requiered");
                }
                else
                {
                    this._checkDataIsOK[LastNameIsOK] = true;
                }
                RaisePropertyChanged("LastName");
            }
        } // endProperty: LastName

        /// <summary>
        /// Le prénom de la personne, ce champ est requis
        /// </summary>
        public String FirstName
        {
            get
            {
                if (this._firstName == null)
                {
                    this._firstName = "";
                }
                return this._firstName;
            }
            set
            {
                this._firstName = value;
                if (this._firstName.Length == 0)
                {
                    this._checkDataIsOK[FirstNameIsOK] = false;
                    throw new ArgumentException("FirstName is requiered");
                }
                else
                {
                    this._checkDataIsOK[FirstNameIsOK] = true;
                }
                RaisePropertyChanged("FirstName");
            }
        } // endProperty: FirstName

        /// <summary>
        /// Le nom de la société, ce paramètre est requis
        /// </summary>
        public String Society
        {
            get
            {
                if (this._society == null)
                {
                    this._society = "";
                }
                return this._society;
            }
            set
            {
                this._society = value;
                if (this._society.Length == 0)
                {
                    this._checkDataIsOK[SocietyIsOK] = false;
                    throw new ArgumentException("Society is requiered");
                }
                else
                {
                    this._checkDataIsOK[SocietyIsOK] = true;
                }
                RaisePropertyChanged("Society");
            }
        } // endProperty: Society

        /// <summary>
        /// Le nom de la rue, ce paramètre est requis
        /// </summary>
        public String Street
        {
            get
            {
                if (this._street == null)
                {
                    this._street = "";
                }
                return this._street;
            }
            set
            {
                this._street = value;
                if (this._street.Length == 0)
                {
                    this._checkDataIsOK[StreetIsOK] = false;
                    throw new ArgumentException("Street is requiered");
                }
                else
                {
                    this._checkDataIsOK[StreetIsOK] = true;
                }
                RaisePropertyChanged("Street");
            }
        } // endProperty: Street

        /// <summary>
        /// Le nom de la ville
        /// </summary>
        public String Town
        {
            get
            {
                if (this._town == null)
                {
                    this._town = "";
                }

                return this._town;
            }
            set
            {
                this._town = value;
                if (this._town.Length == 0)
                {
                    this._checkDataIsOK[TownIsOK] = false;
                    throw new ArgumentException("Town is requiered");
                }
                else
                {
                    this._checkDataIsOK[TownIsOK] = true;
                }
                RaisePropertyChanged("Town");
            }
        } // endProperty: Town

        /// <summary>
        /// Le ZipCode, c'est un paramètre requis
        /// </summary>
        public String ZipCode
        {
            get
            {
                if (this._zipCode == null)
                {
                    this._zipCode = "";
                }
                return this._zipCode;
            }
            set
            {
                this._zipCode = value;
                if (this._zipCode.Length == 0)
                {
                    this._checkDataIsOK[ZipCodeIsOK] = false;
                    throw new ArgumentException("ZipCode is requiered");
                }
                else
                {
                    this._checkDataIsOK[ZipCodeIsOK] = true;
                }
                RaisePropertyChanged("ZipCode");
            }
        } // endProperty: ZipCode

        /// <summary>
        /// L'EMail sert d'identifiant pour la connexion distante. Ce paramètre est requis.
        /// </summary>
        public String EMail
        {
            get
            {
                if (this._email == null)
                {
                    this._email = "";
                }
                return this._email;
            }
            set
            {
                this._email = value;
                // Vérification de la validité de l'email par REGEX
                Regex verifyEmail = new Regex(REGEX_EMAIL);
                if (verifyEmail.IsMatch(this._email.ToUpper()))
                {
                    // Vérifier que l'e-mail n'existe pas déjà
                    var query = from user in BDDServeur.Get().PegaseSupportEntities.Users
                                where user.emailAdress == value
                                select user;

                    if (query.Count() == 0)
                    {
                        this._checkDataIsOK[EMailIsOK] = true; 
                    }
                    else
                    {
                        throw new ArgumentException("Account already exist...");
                    }
                }
                else
                {
                    this._checkDataIsOK[EMailIsOK] = false;
                    throw new ArgumentException("Please, enter a valid email...");
                }

                RaisePropertyChanged("EMail");
            }
        } // endProperty: EMail

        /// <summary>
        /// Password1, requis
        /// </summary>
        public String Password1
        {
            get
            {
                if (this._password1 == null)
                {
                    this._password1 = "";
                }
                return this._password1;
            }
            set
            {
                this._password1 = value;
                // Vérification
                if (this._password1.Length < 6 || this._password1 != this._password1b)
                {
                    this._checkDataIsOK[Password1IsOK] = false;
                    throw new ArgumentException("Bad password");
                }
                else
                {
                    this._checkDataIsOK[Password1IsOK] = true;
                    this._checkDataIsOK[Password1bIsOK] = true;
                }
                RaisePropertyChanged("Password1");
            }
        } // endProperty: Password1

        /// <summary>
        /// Le password bis, afin de vérifier la conformité des deux
        /// </summary>
        public String Password1b
        {
            get
            {
                if (this._password1b == null)
                {
                    this._password1b = "";
                }
                return this._password1b;
            }
            set
            {
                this._password1b = value;
                if (this._password1b.Length < 6 || this._password1 != this._password1b)
                {
                    this._checkDataIsOK[Password1bIsOK] = false;
                    throw new ArgumentException("Bad password");
                }
                else
                {
                    this._checkDataIsOK[Password1bIsOK] = true;
                    this._checkDataIsOK[Password1IsOK] = true;
                }
                RaisePropertyChanged("Password1b");
            }
        } // endProperty: Password1b

        /// <summary>
        /// Le numéro de téléphone du client 
        /// </summary>
        public String PhoneNumber
        {
            get
            {
                if (this._phoneNumber == null)
                {
                    this._phoneNumber = "";
                }
                return this._phoneNumber;
            }
            set
            {
                this._phoneNumber = value;
                this._checkDataIsOK[PhoneNumberIsOK] = true;
                RaisePropertyChanged("PhoneNumber");
            }
        } // endProperty: PhoneNumber

        #endregion

        // Constructeur
        #region Constructeur

        public ViewModelCreateAccountWindow()
        {
            this._checkDataIsOK = new Boolean[10];

            this.CreateCommandCreateUser();
        }

        #endregion

        // Méthodes
        #region Méthodes

        #endregion

        // Messages
        #region Messages

        #endregion

        // Commandes
        #region Commandes

        #region CommandCreateUser
        /// <summary>
        /// La commande CreateUser
        /// </summary>
        public ICommand CommandCreateUser
        {
            get;
            internal set;
        } // endProperty: CommandCreateUser

        /// <summary>
        /// Créer la Commande
        /// </summary>
        private void CreateCommandCreateUser()
        {
            // utiliser : using Mvvm = GalaSoft.MvvmLight;
            CommandCreateUser = new Mvvm.Command.RelayCommand(ExecuteCommandCreateUser, CanExecuteCommandCreateUser);
        } // endMethod: CreateCommandCreateUser

        /// <summary>
        /// La méthode d'exécution de la commande
        /// </summary>
        public void ExecuteCommandCreateUser()
        {
            ServiceData.Users user;

            user = new ServiceData.Users();
            user.emailAdress = this.EMail;
            user.FirstName = this.FirstName;
            user.LastName = this.LastName;
            user.Password = BDDServeur.Get().CryptageSha1(this.Password1);
            user.Phone = this.PhoneNumber;
            user.RegistryDate = DateTime.Now;
            user.NextPasswordChange = DateTime.Now + new TimeSpan(10000);
            user.PasswordNeedChange = false;
            user.Sociaty = this.Society;
            user.Street = this.Street;
            user.Town = this.Town;
            user.ZipCode = this.ZipCode;
            BDDServeur.Get().PegaseSupportEntities.AddToUsers(user);
            BDDServeur.Get().PegaseSupportEntities.SaveChanges();
        } // endMethod: ExecuteCommandCreateUser

        /// <summary>
        /// Vérifie si la commande peut être exécutée
        /// </summary>
        public Boolean CanExecuteCommandCreateUser()
        {
            Boolean Result = this._checkDataIsOK[LastNameIsOK];

            foreach (Boolean ChekedData in this._checkDataIsOK)
            {
                Result = Result && ChekedData;
            }

            return Result;
        } // endMethod: CanExecuteCommandCreateUser

        #endregion

        #endregion
    } // endClass: ViewModelCreateAccountWindow
}
