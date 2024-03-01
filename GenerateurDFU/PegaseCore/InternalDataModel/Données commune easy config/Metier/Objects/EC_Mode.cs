using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using Mvvm = GalaSoft.MvvmLight;
using JAY.PegaseCore;
using System.Xml;

namespace JAY.PegaseCore
{
    /// <summary>
    /// EC_Mode : classe gérant les données pour les différents modes dans l'outil Easy Config
    /// </summary>
    public class EC_Mode : Mvvm.ViewModelBase
    {
        // Variables
        #region Variables

        private ObservableCollection<EC_Effecteur> _effecteurs;
        private PegaseCore.ModeExploitation _refMode;
        private ObservableCollection<EC_SortieAna> _sortiesAna;
        private ObservableCollection<EC_OrganAna> _organAnaUsed;
        private ObservableCollection<EC_Verrou> _verrouillages;
        private ObservableCollection<EC_Verrou> _verrouillagesBtn;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La liste des verrouillages boutons pour le mode
        /// </summary>
        public ObservableCollection<EC_Verrou> VerrouillageBtn
        {
            get
            {
                if (this._verrouillagesBtn == null)
                {
                    this._verrouillagesBtn = new ObservableCollection<EC_Verrou>();
                    this.InitVerrouBtn();
                }
                return this._verrouillagesBtn;
            }
            set
            {
                this._verrouillagesBtn = value;
                RaisePropertyChanged("VerrouillageBtn");
            }
        } // endProperty: VerrouillageBtn

        /// <summary>
        /// La liste des verrouillages
        /// </summary>
        public ObservableCollection<EC_Verrou> Verrouillages
        {
            get
            {
                if (this._verrouillages == null)
                {
                    this._verrouillages = new ObservableCollection<EC_Verrou>();
                    // ajouter une ligne vide
                    EC_Verrou verrou = new EC_Verrou(true);
                    verrou.IsUsed = false;
                    verrou.Name = String.Format("{0:00}", this.Verrouillages.Count);
                    this._verrouillages.Add(verrou);
                    this.MAJNumVerroux();
                }
                return this._verrouillages;
            }
            set
            {
                this._verrouillages = value;
                RaisePropertyChanged("Verrouillages");
            }
        } // endProperty: Verrouillage

        /// <summary>
        /// La collection des sorties analogiques par mode
        /// </summary>
        public ObservableCollection<EC_SortieAna> SortiesAna
        {
            get
            {
                if (this._sortiesAna == null)
                {
                    this.InitSortiesAna();
                }
                return this._sortiesAna;
            }
            set
            {
                this._sortiesAna = value;
                RaisePropertyChanged("SortiesAna");
            }
        } // endProperty: SortiesAna

        /// <summary>
        /// La liste des organes analogiques utilisés
        /// </summary>
        public ObservableCollection<EC_OrganAna> OrganAnaUsed
        {
            get
            {
                if (this._organAnaUsed == null)
                {
                   
                }
                return this._organAnaUsed;
            }
            set
            {
                this._organAnaUsed = value;
                RaisePropertyChanged("OrganAnaUsed");
            }
        } // endProperty: OrganAnaUsed

        /// <summary>
        /// Le mode de référence 
        /// </summary>
        public PegaseCore.ModeExploitation RefMode
        {
            get
            {
                return this._refMode;
            }
            set
            {
                this._refMode = value;
                RaisePropertyChanged("RefMode");
            }
        } // endProperty: RefMode

        /// <summary>
        /// La liste des effecteurs associés à ce mode
        /// </summary>
        public ObservableCollection<EC_Effecteur> Effecteurs
        {
            get
            {
                return this._effecteurs;
            }
            set
            {
                this._effecteurs = value;
                RaisePropertyChanged("Effecteurs");
            }
        } // endProperty: Effecteurs

        /// <summary>
        /// Le nom du mode
        /// </summary>
        public String ModeName
        {
            get
            {
                String Result = "Mode ?";

                if (this.RefMode.ModeLabel != null)
                {
                    if (this.RefMode.Position < 32)
                    {
                        Result = String.Format("Mode {0} : {1}", this.RefMode.Position + 1, this.RefMode.ModeLabel.LibelSelecteur); 
                    }
                    else
                    {
                        Result = this.RefMode.ModeLabel.LibelSelecteur;
                    }
                }

                return Result;
            }
        } // endProperty: ModeName

        #endregion

        // Constructeur
        #region Constructeur

        public EC_Mode(ModeExploitation mode)
        {
            this.RefMode = mode;

            // Initialiser les collections
            if (!IsInDesignMode)
            {
                this.InitEffecteurs();
                this.InitSortiesAna();
            }
            else
            {
                this.InitEffecteursDesign();
                this.InitSortiesAnaDesign();
            }

            Mvvm.Messaging.Messenger.Default.Register<CommandMessage>(this, ReceiveMessage);
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Fermer proprement le mode
        /// </summary>
        public new void Dispose ( )
        {
            Mvvm.Messaging.Messenger.Default.Unregister(this);
            base.Dispose();
        } // endMethod: Dispose

        /// <summary>
        /// Modifier le nom d'un mode
        /// </summary>
        public void MajModeName (String Name)
        {
            this.RefMode.ModeLabel.Label = Name;
            RaisePropertyChanged("ModeName");
        } // endMethod: MajModeName (String Name)

        /// <summary>
        /// Action à mener lors de la réception d'un message
        /// </summary>
        /// <param name="msg">
        /// Le message reçu par l'application
        /// </param>
        private void ReceiveMessage(CommandMessage msg)
        {
            if (msg.Sender is EC_Verrou)
            {
                // Vérifier que le verrou est un verrou de ce mode
                EC_Verrou v = msg.Sender as EC_Verrou;

                if (this.Verrouillages.Contains(v))
                {
                    switch (msg.Command)
                    {
                        case Commands.CMD_ADD_NEW_LINE:
                            EC_Verrou verrou = new EC_Verrou(true);
                            verrou.IsUsed = false;
                            verrou.Name = String.Format("IVerrou");
                            this.Verrouillages.Add(verrou);
                            this.MAJNumVerroux();
                            break;
                        case Commands.CMD_DEL_LINE:
                            EC_Verrou verrouDel = msg.Sender as EC_Verrou;
                            this.Verrouillages.Remove(verrouDel);
                            this.MAJNumVerroux();
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        
        /// <summary>
        /// Initialiser la liste des verroux pour les boutons
        /// </summary>
        public void InitVerrouBtn ( )
        {
            Int32 Diagonal = 0;
            // modif CF
            var query = from organ in PegaseData.Instance.MOperateur.OrganesCommandes
                        where ((organ.MnemoHardFamilleMO == "BT") || (organ.MnemoHardFamilleMO == "CO"))
                        select organ;

            if (query.Count() > 0)
            {
                foreach (var organ in query)
                {
                    // modif CF 
                    if ((organ.MnemoHardFamilleMO == "BT") || ((organ.MnemoHardFamilleMO == "CO") && (organ.ReferenceOrgan != "VIDE")) )
                    {
                        EC_Verrou verrou = new EC_Verrou(false);
                        verrou.Outputs.Clear();
                        List<OrganCommand> organs = query.ToList<OrganCommand>();
                        for (int i = 0; i < query.Count(); i++)
                        {
                            EC_Output output = new EC_Output( organs[i] );
                            if (Diagonal == i)
                            {
                                output.SetIsEnable(false);
                            }
                            else if (organs[i].MnemoHardFamilleMO != "BT")
                            {
                                output.SetIsEnable(false);
                            }
                            else
                            {
                                output.SetIsEnable(true);
                            }
                            //modif CF
                            
                            verrou.Outputs.Add(output);
                        }
                        verrou.Name = organ.MnemoClient;
                        verrou.Mnemologique = organ.Mnemologique;
                        Diagonal++;
                        if (organ.MnemoHardFamilleMO == "BT")
                        {
                            this.VerrouillageBtn.Add(verrou);
                        }
                    }
                }
            }
        } // endMethod: InitVerrouBtn

        /// <summary>
        /// Initialiser les sorties analogiques
        /// </summary>
        private void InitSortiesAna()
        {
            // construire une liste d'organes analogiques liée à la sortie
            this._sortiesAna = new ObservableCollection<EC_SortieAna>();
            this._organAnaUsed = new ObservableCollection<EC_OrganAna>();
            if (PegaseData.Instance.ModuleT != null && PegaseData.Instance.ModuleT.SAnas != null && PegaseData.Instance.ModuleT.SAnas.Count > 0)
            {
                foreach (var sortieAna in PegaseData.Instance.ModuleT.SAnas)
                {
                    EC_SortieAna ECSA = new EC_SortieAna(sortieAna);
                    EC_OrganAna ECOA = new EC_OrganAna();
                    ECSA.LinkedOrganAna = ECOA;
                    this._sortiesAna.Add(ECSA);
                    
                    this._organAnaUsed.Add(ECOA);
                }
            }

            // Dans le cas du Timo, ajouter les sorties PWM
            if (PegaseData.Instance.ModuleT != null && PegaseData.Instance.ModuleT.TypeMT == MT.TIMO)
            {
                foreach (var sortiePWM in PegaseData.Instance.ModuleT.STORS)
                {
                    if (sortiePWM.IsPWM)
                    {
                        EC_SortieAna ECSA = new EC_SortieAna(sortiePWM);
                        EC_OrganAna ECOA = new EC_OrganAna();
                        ECSA.LinkedOrganAna = ECOA;
                        this._sortiesAna.Add(ECSA);

                        this._organAnaUsed.Add(ECOA);
                    }
                }
            }

            this.MAJNumOrganAna();
            this.MAJNumSortiesAna();
        } // endMethod: InitSortiesAna
        
        /// <summary>
        /// Initialiser les sorties analogiques pour le mode design
        /// </summary>
        private void InitSortiesAnaDesign()
        {
            EC_SortieAna SA = new EC_SortieAna();
            EC_OrganAna ECOA = new EC_OrganAna();
            SA.LinkedOrganAna = ECOA;
            this._sortiesAna = new ObservableCollection<EC_SortieAna>();
            this._organAnaUsed = new ObservableCollection<EC_OrganAna>();
            this._sortiesAna.Add(SA);
            this._organAnaUsed.Add(ECOA);
            SA = new EC_SortieAna();
            this._sortiesAna.Add(SA);
            this._organAnaUsed.Add(ECOA);
        } // endMethod: InitSortiesAnaDesign

        /// <summary>
        /// Initialiser les effecteurs
        /// </summary>
        private void InitEffecteurs ( )
        {
            // Rafraichir les organes non disponibles
            this.RefMode.MajOrgansUsedInSelecteur();

            this.Effecteurs = new ObservableCollection<EC_Effecteur>();
            // Pour tous les organes
            if (PegaseData.Instance.MOperateur != null && PegaseData.Instance.MOperateur.OrganesCommandes != null)
            {
                foreach (var organ in PegaseData.Instance.MOperateur.OrganesCommandes)
                {
                    if ((organ.ReferenceOrgan != null && organ.ReferenceOrgan != "VIDE") || (organ.NomOrganeMO.Substring(0,1).ToUpper() == "F" || organ.NomOrganeMO.Substring(0,1).ToUpper() == "M") || organ.NomOrganeMO.Substring(0,1).ToUpper() == "N")
                    {
                        // filtrer les organes analogiques
                        if (organ.MnemoHardFamilleMO != "AXE")
                        {
                            // utiliser uniquement les organes disponibles (non utilisé dans les selecteurs)
                            if (!this.RefMode.VerifyIfOrganExistInCollection(this.RefMode.OrganUsedInSelecteur, organ))
                            {
                                EC_Effecteur effecteur = new EC_Effecteur(organ, null, this);
                                effecteur.ParentMode = this;
                                this.Effecteurs.Add(effecteur);
                            }
                        }
                    }
                }
            }

            // Pour tous les selecteurs dans le mode
            if (this.RefMode.Selecteurs != null)
            {
                for (int i = 0; i < this.RefMode.Selecteurs.Count; i++)
                {
                    if (this.RefMode.Selecteurs[i] != null)
                    {
                        if (this.RefMode.Selecteurs[i].SelecteurType == TypeSelecteur.SelecteurElectronique)
                        {
                            ObservableCollection<String> libels = new ObservableCollection<String>();

                            for (int j = 0; j < this.RefMode.LabelsSelecteurByPos[i].Count; j++)
                            {
                                if (this.RefMode.LabelsSelecteurByPos[i][j] != null)
                                {
                                    libels.Add(this.RefMode.LabelsSelecteurByPos[i][j].LibelSelecteur); 
                                }
                            }

                            EC_Effecteur effecteur = new EC_Effecteur(this.RefMode.Selecteurs[i], String.Format("Selector {0:00}", i + 1), libels, this);
                            this.Effecteurs.Add(effecteur);
                        }
                    }
                }
            }
        } // endMethod: InitEffecteurs

        /// <summary>
        /// Initialiser les effecteurs dans le cadre du design de l'interface
        /// </summary>
        private void InitEffecteursDesign ( )
        {
            this.Effecteurs = new ObservableCollection<EC_Effecteur>();
            // Initialiser un organe
            OrganCommand organ = new OrganCommand();
            organ.IndiceOrganeMO = 0;
            organ.MnemoClient = "Haut";
            organ.MnemoHardFamilleMO = "BT";
            organ.MnemoHardOrganeMO = "F1";
            organ.Mnemologique = "F1";
            organ.NbPosOrgane = 3;
            organ.NomOrganeMO = "F1";
            EC_Effecteur effecteur = new EC_Effecteur(organ, null, this);

            this.Effecteurs.Add(effecteur);

            // Initialiser un effecteur
            Selecteur selecteur = new Selecteur("DesignSelecteur");
            selecteur.BtDecrementer = "F3";
            selecteur.BtIncrementer = "F4";
            selecteur.ComportementAuxBornes = ComportementBorne.Passage_borne_oppose_avec_restauration;
            selecteur.NbPosition = 4;
            selecteur.SelecteurType = TypeSelecteur.SelecteurElectronique;
            effecteur = new EC_Effecteur(selecteur, "Selecteur01", null, this);

            this.Effecteurs.Add(effecteur);
        } // endMethod: InitEffecteursDesign
        
        /// <summary>
        /// Charger le noeud
        /// </summary>
        public void LoadConfig ( XElement node )
        {
            foreach (XElement item in node.Descendants("Effecteur"))
            {
                String IDEffecteur = item.Attribute("IDEffecteur").Value;

                var Query = from effecteur in this.Effecteurs
                            where effecteur.IdEffecteur == IDEffecteur
                            select effecteur;

                if (Query.Count() > 0)
                {
                    Query.First().Load(item);
                }
            }
        } // endMethod: LoadConfig
        
        /// <summary>
        /// Charger la configuration des axes
        /// </summary>
        public void LoadAxe ( XElement node )
        {
            foreach (XElement item in node.Descendants("SortieAna"))
            {
                // 1 - identifier la sortie ana
                Int32 Pos = -1;
                String MnHa = "";

                if (item.Attribute("IDSortieAna") != null)
                {
                    MnHa = item.Attribute("IDSortieAna").Value;

                    for (int i = 0; i < this.SortiesAna.Count; i++)
                    {
                        if (this.SortiesAna[i].SAna != null && this.SortiesAna[i].SAna.MnemoHardware == MnHa)
                        {
                            Pos = i;
                            break;
                        }
                    } 
                }
                else if (item.Attribute("IDSortiePWM") != null)
                {
                    MnHa = item.Attribute("IDSortiePWM").Value;
                    for (int i = 0; i < this.SortiesAna.Count; i++)
                    {
                        if (this.SortiesAna[i].SPWM != null && this.SortiesAna[i].SPWM.MnemoHardware == MnHa)
                        {
                            Pos = i;
                            break;
                        }
                    } 
                }

                if (Pos > -1)
                {
                    // 2 - remplir les propriétés de la sortie ana
                    String IsAssociated = item.Attribute("IsAssociated").Value;

                    if (IsAssociated.ToLower() == "true")
                    {
                        this.SortiesAna[Pos].IsAssociated = true;
                    }
                    else
                    {
                        this.SortiesAna[Pos].IsAssociated = false;
                    }

                    if (item.Attribute("LastTypeUI") != null)
                    {
                        this.SortiesAna[Pos].LastTypeUI = item.Attribute("LastTypeUI").Value;
                    }
                    else if(this.SortiesAna[Pos].SAna != null)
                    {
                        this.SortiesAna[Pos].LastTypeUI = this.SortiesAna[Pos].SAna.UIType.ToString();
                    }

                    String ControlType = item.Attribute("ControlType").Value;
                    Int32 CurrentJoystick = -1;

                    if (ControlType == "Joystick")
                    {
                        CurrentJoystick = 0;
                    }
                    else
                    {
                        CurrentJoystick = 1;
                    }

                    this.SortiesAna[Pos].LinkedOrganAna.CurrentType = CurrentJoystick;

                    // 3 - charger l'organe lié à la sortie ana
                    XElement XJoystick, EmulJoystick;

                    XJoystick = item.Descendants("Joystick").First();
                    EmulJoystick = item.Descendants("EmulJoystick").First();

                    // 3.1 - Initialiser les données du joystick
                    String MnemoJoystick = XJoystick.Attribute("Mnemologique").Value;
                    var QueryJoystick = from joystick in PegaseData.Instance.MOperateur.OrganesCommandes
                                        where joystick.MnemoHardOrganeMO == MnemoJoystick
                                        select joystick;
                    //CF 14/10/2016               var QueryJoystick = from joystick in PegaseData.Instance.MOperateur.OrganesCommandes
                    //                                  where joystick.NomOrganeMO == MnemoJoystick
                    //                                  select joystick;
                    
                    if (QueryJoystick.Count() > 0)
                    {
                        OrganCommand recherche = QueryJoystick.First();
                        int i = 0;
                        foreach ( var organ in this.SortiesAna[Pos].LinkedOrganAna.ListJoysticks)
                        {
                            if(organ.MnemoHardOrganeMO.Equals(recherche.MnemoHardOrganeMO))
                            {
                                this.SortiesAna[Pos].LinkedOrganAna.CurrentJoystick = i;
                            }
                            i++;
                        }
                        //this.SortiesAna[Pos].LinkedOrganAna.CurrentJoystick = this.SortiesAna[Pos].LinkedOrganAna.ListJoysticks.IndexOf(QueryJoystick.First());
                    }

                    if (XJoystick.Attribute("BorneMax") != null)
                    {
                        String BMax = XJoystick.Attribute("BorneMax").Value;
                        this.SortiesAna[Pos].BorneMax = Convert.ToDouble(XMLCore.Tools.FixFloatStringSeparator(BMax));
                    }
                    else if(this.SortiesAna[Pos].SAna != null)
                    {
                        this.SortiesAna[Pos].BorneMax = (Int32)this.SortiesAna[Pos].SAna.ValUIMax;
                    }

                    if (XJoystick.Attribute("BorneMin") != null)
                    {
                        String BMin = XJoystick.Attribute("BorneMin").Value;
                        this.SortiesAna[Pos].BorneMin = Convert.ToDouble(XMLCore.Tools.FixFloatStringSeparator(BMin));
                    }
                    else if (this.SortiesAna[Pos].SAna != null)
                    {
                        this.SortiesAna[Pos].BorneMin = (Int32)this.SortiesAna[Pos].SAna.ValUIMin;
                    }

                    if (this.SortiesAna[Pos].LastTypeUI != null && this.SortiesAna[Pos].SAna != null)
                    {
                        if (this.SortiesAna[Pos].SAna.UIType.ToString() != this.SortiesAna[Pos].LastTypeUI)
                        {
                            // Réassigner les bornes min et max
                            this.SortiesAna[Pos].BorneMax = (Int32)this.SortiesAna[Pos].SAna.ValUIMax;
                            this.SortiesAna[Pos].BorneMin = (Int32)this.SortiesAna[Pos].SAna.ValUIMin;
                            this.SortiesAna[Pos].LastTypeUI = this.SortiesAna[Pos].SAna.UIType.ToString();
                        }
                    }

                    if (XJoystick.Attribute("ValZero") != null)
                    {
                        String VZero = XJoystick.Attribute("ValZero").Value;
                        this.SortiesAna[Pos].ValZero = Convert.ToDouble(XMLCore.Tools.FixFloatStringSeparator(VZero));
                    }
                    else
                    {
                        this.SortiesAna[Pos].ValZero = (this.SortiesAna[Pos].BorneMax - this.SortiesAna[Pos].BorneMin) / 2;
                    }

                    if (XJoystick.Attribute("IsCourbePositive") != null)
                    {
                        String ICP = XJoystick.Attribute("IsCourbePositive").Value;
                        if (ICP.ToLower() == "true")
	                    {
                            this.SortiesAna[Pos].IsCourbePositive = true;
	                    }
                        else
                        {
                            this.SortiesAna[Pos].IsCourbePositive = false;
                        }
                    }
                    else
                    {
                        this.SortiesAna[Pos].IsCourbePositive = true;
                    }

                    // 3.2 - Initialiser les données de l'émulation du joystick
                    String MnemoBtnInc = EmulJoystick.Attribute("BtnIncrementMnemologique").Value;
                    String MnemoBtnDec = EmulJoystick.Attribute("BtnDecrementMnemologique").Value;
                    String Increment = EmulJoystick.Attribute("Increment").Value;

                    String Mnemo, Position;
                    this.OrganAnaUsed[Pos].CurrentBtnIncrement = this.OrganAnaUsed[Pos].GetListTextButtonPos(MnemoBtnInc, out Mnemo, out Position);
                    this.OrganAnaUsed[Pos].CurrentBtnDecrement = this.OrganAnaUsed[Pos].GetListTextButtonPos(MnemoBtnDec, out Mnemo, out Position);
                    //var QueryBtnInc = from button in PegaseData.Instance.MOperateur.OrganesCommandes
                    //                  where button.NomOrganeMO == MnemoBtnInc
                    //                  select button;

                    //if (QueryBtnInc.Count() > 0)
                    //{
                    //    this.SortiesAna[Pos].LinkedOrganAna.BtnIncrement = QueryBtnInc.First();
                    //}

                    //var QueryBtnDec = from button in PegaseData.Instance.MOperateur.OrganesCommandes
                    //                  where button.NomOrganeMO == MnemoBtnDec
                    //                  select button;

                    //if (QueryBtnDec.Count() > 0)
                    //{
                    //    this.SortiesAna[Pos].LinkedOrganAna.BtnDecrement = QueryBtnDec.First();
                    //}

                    try
                    {
                        this.SortiesAna[Pos].LinkedOrganAna.IncrementJoystick = Convert.ToInt32(Increment);
                        this.SortiesAna[Pos].LinkedOrganAna.IncrementPercent = ((Double)this.SortiesAna[Pos].LinkedOrganAna.IncrementJoystick * 100.0) / 1023.0;
                    }
                    catch
                    {
                        this.SortiesAna[Pos].LinkedOrganAna.IncrementJoystick = 10;
                    }
                } 
            }

            this.MAJNumOrganAna();
            this.MAJNumSortiesAna();
        } // endMethod: LoadAxe
        
        /// <summary>
        /// Charger la configuration des interverrouillages
        /// </summary>
        public void LoadVerrou ( XElement node )
        {
            this.Verrouillages.Clear();

            foreach (XElement verrou in node.Descendants("Verrouillage"))
            {
                String Name = verrou.Attribute("Name").Value;
                EC_Verrou v = new EC_Verrou(true);
                String XIsUsed = verrou.Attribute("IsUsed").Value;

                if (XIsUsed.ToLower() == "true")
                {
                    v.IsUsed = true;
                }
                else
                {
                    v.IsUsed = false;
                }

                v.Name = Name;
                v.Load(verrou);

                this.Verrouillages.Add(v);
            }

            // Si aucun verrou enregistré
            if (this.Verrouillages.Count == 0)
            {
                // Ajouter une ligne vide
                EC_Verrou verrou = new EC_Verrou(true);
                verrou.IsUsed = false;
                verrou.Name = String.Format("{0:00}", this.Verrouillages.Count);
                this.Verrouillages.Add(verrou);
            }

            this.MAJNumVerroux();
        } // endMethod: LoadVerrou

        /// <summary>
        /// Charger la configuration des interverrouillages
        /// </summary>
        public void LoadVerrouBtn(XElement node)
        {
            foreach (XElement verrou in node.Descendants("VerrouillageBtn"))
            {
                String Name = verrou.Attribute("Name").Value;
                EC_Verrou v = this.GetCurrentVerrou(Name);
                if (v != null)
                {
                    String XIsUsed = verrou.Attribute("IsUsed").Value;

                    if (XIsUsed.ToLower() == "true")
                    {
                        v.IsUsed = true;
                    }
                    else
                    {
                        v.IsUsed = false;
                    }

                    if (this.GetCurrentOrgan(Name) != null)
                    {
                        v.Name = this.GetCurrentOrgan(Name).MnemoClient;
                        v.LoadBtn(verrou);
                    }
                }
            }

            // Si pas de verrouillage bouton sauvegarder, créer un tableau vierge
            if (this.VerrouillageBtn.Count == 0)
            {
                this.InitVerrouBtn();
            }
            else
            {
                // Verrouiller la diagonal
                for (int i = 0; i < this.VerrouillageBtn.Count; i++)
                {
                    this.VerrouillageBtn[i].Outputs[i].SetIsEnable(false);

                     //modif CF
                    // verrouile si l'organe n'est pas un bouton
                    if (this.VerrouillageBtn[i].Outputs[i].Organ.MnemoHardFamilleMO != "BT")
                    {
                        this.VerrouillageBtn[i].Outputs[i].SetIsEnable(false);
                    }
                }
            }

        } // endMethod: LoadVerrouBtn
        
        /// <summary>
        /// Acquerir un EC_Verrou d'après le nom mnémologique
        /// </summary>
        public EC_Verrou GetCurrentVerrou ( String Mnemologique )
        {
            EC_Verrou Result = null;
            String OrganName;
            
            OrganCommand OC = this.GetCurrentOrgan(Mnemologique);

            if (OC != null)
            {
                OrganName = OC.MnemoClient;

                //var query = from verrou in this.VerrouillageBtn
                //            where verrou.Name == OrganName
                //            select verrou;
                var query = from verrou in this.VerrouillageBtn
                            where verrou.Mnemologique == Mnemologique
                            select verrou;

                if (query.Count() > 0)
                {
                    Result = query.First();
                }
            }

            return Result;
        } // endMethod: GetCurrentVerrou

        /// <summary>
        /// Récupérer un organe en fonction de sa mnémologique
        /// </summary>
        public OrganCommand GetCurrentOrgan ( String Mnemologique )
        {
            OrganCommand Result = null;

            var Query = from organ in PegaseData.Instance.MOperateur.OrganesCommandes
                        where organ.Mnemologique == Mnemologique
                        select organ;

            if (Query.Count() > 0)
            {
                Result = Query.First();
            }
            else
            {
                if (Mnemologique == "B14" || Mnemologique == "BOUTON_14")
                {
                    Query = from organ in PegaseData.Instance.MOperateur.OrganesCommandes
                                where organ.Mnemologique == "BOUTON_13"
                                select organ;

                    if (Query.Count() > 0)
                    {
                        Result = Query.First();
                    }
                }
            }

            return Result;
        } // endMethod: GetCurrentOrgan

        /// <summary>
        /// Numerote les lignes de verrous
        /// </summary>
        private void MAJNumVerroux ( )
        {
            if (this.Verrouillages != null)
            {
                for (int i = 0; i < this.Verrouillages.Count; i++)
                {
                    this.Verrouillages[i].LineNumber = i;
                }
            }
        } // endMethod: MAJNumVerroux

        /// <summary>
        /// Numerote les lignes des organes analogiques
        /// </summary>
        private void MAJNumOrganAna()
        {
            if (this.OrganAnaUsed != null)
            {
                for (int i = 0; i < this.OrganAnaUsed.Count; i++)
                {
                    this.OrganAnaUsed[i].LineNumber = i;
                } 
            }
        } // endMethod: MAJNumOrganAna

        /// <summary>
        /// Numerote les lignes de sorties analogiques
        /// </summary>
        private void MAJNumSortiesAna()
        {
            if (this.SortiesAna != null)
            {
                for (int i = 0; i < this.SortiesAna.Count; i++)
                {
                    this.SortiesAna[i].LineNumber = i;
                }
            }
        } // endMethod: MAJNumSortiesAna

        /// <summary>
        /// Ajouter tous les effecteurs au noeud
        /// </summary>
        public XElement SaveConfig ( XElement node )
        {
            XElement Result = node;

            foreach (var effecteur in this.Effecteurs)
            {
                XElement XEffecteur = new XElement("Effecteur");
                // ID
                XAttribute Xattrib;

                Xattrib = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "Effecteur");
                XEffecteur.Add(Xattrib);
                Xattrib = new XAttribute("IDEffecteur", effecteur.IdEffecteur );
                XEffecteur.Add(Xattrib);

                // Nom
                Xattrib = new XAttribute("Name", effecteur.Name);
                XEffecteur.Add(Xattrib);

                // Sauver les positions
                XEffecteur = effecteur.Save(XEffecteur);

                // Stocker le noeud
                Result.Add(XEffecteur);
            }

            return Result;
        } // endMethod: SaveConfig

        /// <summary>
        /// Ajouter tous les effecteurs au noeud
        /// </summary>
        public XElement SaveAxe(XElement node)
        {
            XElement Result = node;

            for (int i = 0; i < this.SortiesAna.Count; i++)
            {
                XElement SAnas = new XElement("SortieAna");
                // ID
                XAttribute Xattrib;

                Xattrib = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "SortieAna");
                SAnas.Add(Xattrib);

                if (this.SortiesAna[i].SAna != null)
                {
                    Xattrib = new XAttribute("IDSortieAna", this.SortiesAna[i].SAna.MnemoHardware);
                    SAnas.Add(Xattrib); 
                }
                else if(this.SortiesAna[i].SPWM != null)
                {
                    Xattrib = new XAttribute("IDSortiePWM", this.SortiesAna[i].SPWM.MnemoHardware);
                    SAnas.Add(Xattrib); 
                }

                // LastTypeUI
                if (this.SortiesAna[i].LastTypeUI != null)
                {
                    Xattrib = new XAttribute("LastTypeUI", this.SortiesAna[i].LastTypeUI);
                    SAnas.Add(Xattrib);
                }

                // IsUsed
                Xattrib = new XAttribute("IsAssociated", this.OrganAnaUsed[i].IsUsed);
                SAnas.Add(Xattrib); 

                // Joystick or Emulation
                Xattrib = new XAttribute("ControlType", this.OrganAnaUsed[i].Type);
                SAnas.Add(Xattrib);

                // Définition des noeuds enfants
                // Définition des joysticks
                XElement Joystick = new XElement("Joystick");
                Xattrib = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "Joystick");
                Joystick.Add(Xattrib);

                String Mn;
                if(this.OrganAnaUsed[i].CurrentJoystick > -1 && this.OrganAnaUsed[i].ListJoysticks.Count > 0)
                {
                    Mn = this.OrganAnaUsed[i].ListJoysticks[this.OrganAnaUsed[i].CurrentJoystick].NomOrganeMO;
                }
                else
	            {
                    Mn = "";
	            }

                Xattrib = new XAttribute("Mnemologique", Mn);
                Joystick.Add(Xattrib);

                Xattrib = new XAttribute("BorneMax", this.SortiesAna[i].BorneMax);
                Joystick.Add(Xattrib);

                Xattrib = new XAttribute("BorneMin", this.SortiesAna[i].BorneMin);
                Joystick.Add(Xattrib);

                Xattrib = new XAttribute("ValZero", this.SortiesAna[i].ValZero);
                Joystick.Add(Xattrib);

                Xattrib = new XAttribute("IsCourbePositive", this.SortiesAna[i].IsCourbePositive);
                Joystick.Add(Xattrib);

                // Définition de l'émulation joystick
                XElement EmulJoystick = new XElement("EmulJoystick");
                Xattrib = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "EmulJoystick");
                EmulJoystick.Add(Xattrib);

                if (this.OrganAnaUsed[i].CurrentBtnIncrement > -1)
                {
                    if (this.OrganAnaUsed[i].CurrentBtnIncrement < this.OrganAnaUsed[i].ListTextButton.Count )
                    {
                        Mn = this.OrganAnaUsed[i].ListTextButton[this.OrganAnaUsed[i].CurrentBtnIncrement]; 
                    }
                    else
                    {
                        Mn = "";
                    }
                }

                Xattrib = new XAttribute("BtnIncrementMnemologique", Mn);
                EmulJoystick.Add(Xattrib);

                if (this.OrganAnaUsed[i].CurrentBtnDecrement > -1)
                {
                    if (this.OrganAnaUsed[i].CurrentBtnDecrement < this.OrganAnaUsed[i].ListTextButton.Count)
                    {
                        Mn = this.OrganAnaUsed[i].ListTextButton[this.OrganAnaUsed[i].CurrentBtnDecrement]; 
                    }
                    else
                    {
                        Mn = "";
                    }
                }

                Xattrib = new XAttribute("BtnDecrementMnemologique", Mn);
                EmulJoystick.Add(Xattrib);

                Xattrib = new XAttribute("Increment", this.OrganAnaUsed[i].IncrementJoystick.ToString());
                EmulJoystick.Add(Xattrib);

                // Ajout des noeuds
                SAnas.Add(Joystick);
                SAnas.Add(EmulJoystick);
                Result.Add(SAnas);
            }

            return Result;
        } // endMethod: SaveAxe

        /// <summary>
        /// Ajouter tous les effecteurs au noeud
        /// </summary>
        public XElement SaveVerrou(XElement node)
        {
            XElement Result = node;

            foreach (var verrou in this.Verrouillages)
            {
                XElement XVerrou = new XElement("Verrouillage");
                XAttribute xAttrib = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "Verrouillage");
                XVerrou.Add(xAttrib);
                XAttribute Name = new XAttribute("Name", verrou.Name);
                XVerrou.Add(Name);

                XAttribute IsUsed = new XAttribute("IsUsed", verrou.IsUsed);
                XVerrou.Add(IsUsed);

                XVerrou = verrou.Save(XVerrou);

                Result.Add(XVerrou);
            }

            return Result;
        } // endMethod: SaveVerrou

        /// <summary>
        /// Ajouter tous les effecteurs au noeud
        /// </summary>
        public XElement SaveVerrouBtn(XElement node)
        {
            XElement Result = node;
            Int32 NumVerrou = 0;

            foreach (var verrou in this.VerrouillageBtn)
            {
                XElement XVerrou = new XElement("VerrouillageBtn");
                XAttribute xAttrib = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "VerrouillageBtn");
                XVerrou.Add(xAttrib);
                XAttribute Name = new XAttribute("Name", verrou.Outputs[NumVerrou].Organ.Mnemologique);
                XVerrou.Add(Name);

                XAttribute IsUsed = new XAttribute("IsUsed", verrou.IsUsed);
                XVerrou.Add(IsUsed);

                XVerrou = verrou.SaveBtn(XVerrou);

                Result.Add(XVerrou);
                NumVerrou++;
            }

            return Result;
        } // endMethod: SaveVerrouBtn

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: EC_Mode
}
