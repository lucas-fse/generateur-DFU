using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Mvvm = GalaSoft.MvvmLight;
using System.Xml;
using System.Xml.Linq;

namespace JAY.PegaseCore
{
    public class CommentairesEasy : Mvvm.ViewModelBase
	{
        private Boolean _isImplemented;
        private String _commentaire;
        private ES _linkedES;

        #region Propriétés

        /// <summary>
        /// Le nom de l'entrée / sortie
        /// </summary>
        public String ESName
        {
            get
            {
                String Result = "";

                if (this.LinkedES != null)
                {
                    Result = String.Format("{0} / {1}", LinkedES.MnemoHardware, LinkedES.MnemoClient);
                }

                return Result;
            }
        } // endProperty: ESName

        /// <summary>
        /// L'entrée / sortie liées
        /// </summary>
        public ES LinkedES
        {
            get
            {
                return this._linkedES;
            }
            set
            {
                this._linkedES = value;
                RaisePropertyChanged("LinkedES");
            }
        } // endProperty: LinkedES

		/// <summary>
        /// La fonction EasyConfig liée au commentaire est-elle implémentée?
        /// </summary>
        public Boolean IsImplemented
        {
            get
            {
                return this._isImplemented;
            }
            set
            {
                this._isImplemented = value;
                RaisePropertyChanged("IsImplemented");
            }
        } // endProperty: IsImplemented
 
        /// <summary>
        /// Le commantaire lié à une sortie 
        /// </summary>
        public String Commentaire
        {
            get
            {
                return this._commentaire;
            }
            set
            {
                this._commentaire = value;
                RaisePropertyChanged("Commentaire");
            }
        } // endProperty: Commentaire


	    #endregion

        public CommentairesEasy()
        {
            this.IsImplemented = false;
            this.Commentaire = "";
            this.LinkedES = null;
        }

        #region Méthodes
        
        /// <summary>
        /// construire un commentaire Easy
        /// </summary>
        public static CommentairesEasy BuildCommentaireEasy ( ES sortie )
        {
            CommentairesEasy Result = new CommentairesEasy();
            Result.LinkedES = sortie;
            
            return Result;
        } // endMethod: BuildCommentaireEasy

        #endregion
    }

    public class ModeEasyCommentaire : Mvvm.ViewModelBase
    {
        private ModeExploitation _linkedMode;
        private ObservableCollection<CommentairesEasy> _easyCommentaires;

        #region Propriétés

        /// <summary>
        /// La liste des commentaires pour la configuration des sorties pour un mode
        /// </summary>
        public ObservableCollection<CommentairesEasy> EasyCommentaires
        {
            get
            {
                if (this._easyCommentaires == null)
                {
                    this._easyCommentaires = new ObservableCollection<CommentairesEasy>();
                }
                return this._easyCommentaires;
            }
            set
            {
                this._easyCommentaires = value;
                RaisePropertyChanged("EasyCommentaires");
            }
        } // endProperty: EasyCommentaires

        /// <summary>
        /// Le nom du mode
        /// </summary>
        public String ModeName
        {
            get
            {
                String Result = "";

                if (this.LinkedMode != null)
                {
                    if (this.LinkedMode.Position < 32)
                    {
                        Result = String.Format("Mode {0} : {1}", this.LinkedMode.Position + 1, this.LinkedMode.ModeLabel.Label); 
                    }
                    else
                    {
                        Result = LanguageSupport.Get().GetText("EASYCONF/ALL_MODE");
                    }
                }

                return Result;
            }
        } // endProperty: ModeName

        /// <summary>
        /// Le mode lié
        /// </summary>
        public ModeExploitation LinkedMode
        {
            get
            {
                return this._linkedMode;
            }
            set
            {
                this._linkedMode = value;
                RaisePropertyChanged("LinkedMode");
            }
        } // endProperty: LinkedMode

        #endregion

        public ModeEasyCommentaire()
        {
        }

        #region Méthodes

        /// <summary>
        /// Construire le mode
        /// </summary>
        public static ModeEasyCommentaire BuildMode ( ModeExploitation mode )
        {
            ModeEasyCommentaire Result;

            Result = new ModeEasyCommentaire();
            Result.LinkedMode = mode;

            // Construire la liste des commentaires pour chacune des sorties
            foreach (ESTOR tor in PegaseData.Instance.ModuleT.STORS)
            {
                CommentairesEasy CE = CommentairesEasy.BuildCommentaireEasy(tor);
                Result.EasyCommentaires.Add(CE);
            }

            foreach (ESAna ana in PegaseData.Instance.ModuleT.SAnas)
            {
                CommentairesEasy CE = CommentairesEasy.BuildCommentaireEasy(ana);
                Result.EasyCommentaires.Add(CE);
            }

            return Result;
        } // endMethod: BuildMode

        #endregion
    }

    /// <summary>
    /// Classe de gestion des commentaires de la fiche de programmation
    /// </summary>
    public class Commentaires : Mvvm.ViewModelBase
    {
        // Variables
        #region Variables
        
        private String _descriptifGeneral;
        private ObservableCollection<ModeEasyCommentaire> _commentairesByModes;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// La liste des commentaires par mode
        /// </summary>
        public ObservableCollection<ModeEasyCommentaire> CommentaireByModes
        {
            get
            {
                if (this._commentairesByModes == null)
                {
                    this._commentairesByModes = new ObservableCollection<ModeEasyCommentaire>();
                }
                return this._commentairesByModes;
            }
            set
            {
                this._commentairesByModes = value;
                RaisePropertyChanged("CommentaireByModes");
            }
        } // endProperty: CommentaireByModes

        /// <summary>
        /// Le descriptif général de l'application
        /// </summary>
        public String DescriptifGeneral
        {
            get
            {
                if (this._descriptifGeneral == null)
                {
                    this._descriptifGeneral = "Ref Pack : \n";
                 }
                return this._descriptifGeneral;
            }
            set
            {
                this._descriptifGeneral = value;
                RaisePropertyChanged("DescriptifGeneral");
            }
        } // endProperty: DescriptifGeneral

        #endregion

        // Constructeur
        #region Constructeur

        public Commentaires()
        {
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Enregistrement des commentaires de la fiche
        /// </summary>
        public void Save ( )
        {
            // Fabriquer la section commentaire
            // 1 - créer la section commentaire
            XElement CommentaireData = new XElement("CommentaireData");
            XAttribute code = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "CommentaireData");
            CommentaireData.Add(code);


            //commentaire des références des descriptifs
            // 2 - créer la section descriptif de l'application
            XElement SectionReference = new XElement("SectionReference");
            code = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "SectionReference");
            SectionReference.Add(code);
            CommentaireData.Add(SectionReference);

            XElement RefCustomer = new XElement("reference");
            code = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "RefCustomer");
            RefCustomer.Add(code);
            RefCustomer.Value = PegaseData.Instance.RefCustomer;
            SectionReference.Add(RefCustomer);

            XElement RefCompany = new XElement("reference");
            code = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "RefCompany");
            RefCompany.Add(code);
            RefCompany.Value = PegaseData.Instance.RefCompany;
            SectionReference.Add(RefCompany);

            XElement RefCustomerCode = new XElement("reference");
            code = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "RefCustomerCode");
            RefCustomerCode.Add(code);
            RefCustomerCode.Value = PegaseData.Instance.RefCustomerCode;
            SectionReference.Add(RefCustomerCode);

            XElement RefJay = new XElement("reference");
            code = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "RefJay");
            RefJay.Add(code);
            RefJay.Value = PegaseData.Instance.RefJAY;
            SectionReference.Add(RefJay);

            XElement RefDate = new XElement("reference");
            code = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "RefDate");
            RefDate.Add(code);
            RefDate.Value = PegaseData.Instance.RefDate.Ticks.ToString();
            SectionReference.Add(RefDate);

            XElement RefIndice = new XElement("reference");
            code = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "RefIndice");
            RefIndice.Add(code);
            RefIndice.Value = PegaseData.Instance.RefIndice;
            SectionReference.Add(RefIndice);

            XElement Refprojet = new XElement("reference");
            code = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "RefProjet");
            Refprojet.Add(code);
            Refprojet.Value = PegaseData.Instance.RefProjet;
            SectionReference.Add(Refprojet);

            // 2 - créer la section descriptif de l'application
            XElement DescriptifGeneral = new XElement("DescriptifGeneral");
            code = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "DescriptifGeneral");
            DescriptifGeneral.Add(code);
            DescriptifGeneral.Value = this.DescriptifGeneral;
            CommentaireData.Add(DescriptifGeneral);
            
            // 3 - créer la section commentaire alarme
            if (PegaseData.Instance.ParamHorsMode != null)
            {
                if (PegaseData.Instance.ParamHorsMode.Alarmes != null)
                {
                    XElement SectionAlarme = new XElement("SectionAlarme");
                    code = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "SectionAlarme");
                    SectionAlarme.Add(code);
                    Int32 i = 0;

                    foreach (Alarme alarme in PegaseData.Instance.ParamHorsMode.Alarmes)
                    {
                        XElement CommentaireAlarme = new XElement("CommentaireAlarme");
                        code = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "CommentaireAlarme");
                        CommentaireAlarme.Add(code);
                        XAttribute AlarmeNumber = new XAttribute("AlarmeNumber", i++);
                        CommentaireAlarme.Add(AlarmeNumber);
                        CommentaireAlarme.Value = alarme.CommentaireAlarme;
                        SectionAlarme.Add(CommentaireAlarme);
                    }

                    CommentaireData.Add(SectionAlarme);
                }
            }

            // 4 - créer la section commentaire RI
            if (PegaseData.Instance.OLogiciels != null)
            {
                if (PegaseData.Instance.OLogiciels.Informations != null)
                {
                    Int32 i = 0;
                    XElement SectionRI = new XElement("SectionRI");
                    code = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "SectionRI");
                    SectionRI.Add(code);
                    foreach (var RI in PegaseData.Instance.OLogiciels.Informations)
	                {
                        XElement CommentaireRI = new XElement("CommentaireRI");
                        code = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "CommentaireRI");
                        CommentaireRI.Add(code);
		                XAttribute RINumber = new XAttribute("RINumber", i++);
                        CommentaireRI.Add(RINumber);
                        CommentaireRI.Value = RI.CommentaireRI;
                        SectionRI.Add(CommentaireRI);
	                }

                    CommentaireData.Add(SectionRI);
                }
            }

            // 5 - créer la section commentaire Easy Config
            if (this.CommentaireByModes != null)
            {
                if (this.CommentaireByModes.Count > 0)
                {
                    XElement SectionCommentaireEasyConfig = new XElement("SectionCommentaireEasyConfig");
                    code = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "SectionCommentaireEasyConfig");
                    SectionCommentaireEasyConfig.Add(code);

                    foreach (ModeEasyCommentaire mode in this.CommentaireByModes)
                    {
                        XElement SectionMode = new XElement("SectionMode");
                        code = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "SectionMode");
                        SectionMode.Add(code);
                        XAttribute ModeNumber = new XAttribute("ModeNumber", mode.LinkedMode.Position.ToString());
                        SectionMode.Add(ModeNumber);

                        foreach (CommentairesEasy commentaire in mode.EasyCommentaires)
                        {
                            XElement XCommentaire = new XElement("Commentaire");
                            code = new XAttribute(XMLCore.XML_ATTRIBUTE.CODE, "Commentaire");
                            XCommentaire.Add(code);
                            XAttribute SortieMnemologique = new XAttribute("SortieMnemologique", commentaire.LinkedES.MnemoLogique);
                            XCommentaire.Add(SortieMnemologique);
                            XAttribute IsImplemented = new XAttribute("IsImplemented", commentaire.IsImplemented.ToString());
                            XCommentaire.Add(IsImplemented);
                            XCommentaire.Value = commentaire.Commentaire;

                            SectionMode.Add(XCommentaire);
                        }

                        SectionCommentaireEasyConfig.Add(SectionMode);
                    }

                    CommentaireData.Add(SectionCommentaireEasyConfig);
                }
            }

            // Supprimer la (les?) sections commentaires précédentes
            IEnumerable<XElement> sectionsCommentaire = PegaseData.Instance.XMLRoot.Descendants("CommentaireData");
            if (sectionsCommentaire != null && sectionsCommentaire.Count() > 0)
            {
                for (int i = sectionsCommentaire.Count() - 1; i >= 0; i--)
			    {
                    sectionsCommentaire.ElementAt(i).Remove();
			    }
            }
            // Inclure la section commentaire dans la racine du fichier iDialog en cours d'utilisation
            PegaseData.Instance.XMLRoot.AddFirst(CommentaireData);
        } // endMethod: Save
        
        /// <summary>
        /// Chargement des commentaires de la fiche
        /// </summary>
        public void Load ( )
        {
            this.BuilCommentairesByMode();
            // 1 - trouver la section commentaire
            IEnumerable<XElement> SectionsCommentaire = PegaseData.Instance.XMLRoot.Descendants("CommentaireData");
            if (SectionsCommentaire != null && SectionsCommentaire.Count() > 0)
            {
                XElement CommentaireData = SectionsCommentaire.First();
                //
                IEnumerable<XElement> SectionReference = CommentaireData.Descendants("SectionReference");
                if ((SectionReference != null) && (SectionReference.Count()>0))
                {
                    
                    XElement References = SectionReference.First();
                    IEnumerable<XElement> referencess = References.Descendants("reference");
                    foreach (XElement reference in referencess)
                    {
                        if (reference.Attribute("code").Value.Equals("RefCustomer"))
                        {
                            PegaseData.Instance.RefCustomer = reference.Value;
                        }
                        if (reference.Attribute("code").Value.Equals("RefCompany"))
                        {
                            PegaseData.Instance.RefCompany = reference.Value;
                        }
                        if (reference.Attribute("code").Value.Equals("RefCustomerCode"))
                        {
                            PegaseData.Instance.RefCustomerCode = reference.Value;
                        }
                        if (reference.Attribute("code").Value.Equals("RefJay"))
                        {
                            PegaseData.Instance.RefJAY = reference.Value;
                        }
                        if (reference.Attribute("code").Value.Equals("RefDate"))
                        {
                            try { PegaseData.Instance.RefDate = new DateTime(Convert.ToInt64(reference.Value)); }
                            catch { PegaseData.Instance.RefDate = DateTime.Now; }
                        }
                        if (reference.Attribute("code").Value.Equals("RefIndice"))
                        {
                            PegaseData.Instance.RefIndice = reference.Value;
                        }
                        if (reference.Attribute("code").Value.Equals("RefProjet"))
                        {
                            PegaseData.Instance.RefProjet = reference.Value;
                        }
                    }
                }

                // 2 - charger les données descriptif application
                IEnumerable<XElement> Descriptifs = CommentaireData.Descendants("DescriptifGeneral");
                if (Descriptifs != null && Descriptifs.Count() > 0)
                {
                    XElement DescriptGeneral = Descriptifs.First();
                    this.DescriptifGeneral = DescriptGeneral.Value;
                }

                // 3 - charger les données commentaire alarme
                IEnumerable<XElement> Alarmes = CommentaireData.Descendants("SectionAlarme");
                if (Alarmes != null && Alarmes.Count() > 0)
                {
                    XElement SectionAlarme = Alarmes.First();

                    IEnumerable<XElement> CommentairesAlarme = SectionAlarme.Descendants("CommentaireAlarme");
                    if (PegaseData.Instance.ParamHorsMode.Alarmes.Count == CommentairesAlarme.Count())
                    {
                        for (int i = 0; i < PegaseData.Instance.ParamHorsMode.Alarmes.Count; i++)
                        {
                            PegaseData.Instance.ParamHorsMode.Alarmes[i].CommentaireAlarme = CommentairesAlarme.ElementAt(i).Value;
                        }
                    }
                }
                // 4 - charger les données commentaire RI
                IEnumerable<XElement> RIs = CommentaireData.Descendants("SectionRI");
                if (RIs != null && RIs.Count() > 0)
                {
                    XElement SectionRI = RIs.First();

                    IEnumerable<XElement> CommentaireRI = SectionRI.Descendants("CommentaireRI");
                    if (PegaseData.Instance.OLogiciels.Informations.Count == CommentaireRI.Count())
                    {
                        for (int i = 0; i < PegaseData.Instance.OLogiciels.Informations.Count; i++)
                        {
                            PegaseData.Instance.OLogiciels.Informations[i].CommentaireRI = CommentaireRI.ElementAt(i).Value;
                        }
                    }
                }
                // 5 - charger les données commentaire Easy Config
                IEnumerable<XElement> CSectionCommentaireEasyConfig = CommentaireData.Descendants("SectionCommentaireEasyConfig");
                if (CSectionCommentaireEasyConfig != null && CSectionCommentaireEasyConfig.Count() > 0)
                {
                    XElement SectionCommentaireEasyConfig = CSectionCommentaireEasyConfig.First();
                    IEnumerable<XElement> SectionModes = SectionCommentaireEasyConfig.Descendants("SectionMode");
                    if (SectionModes != null && SectionModes.Count() > 0)
                    {
                        foreach (XElement XMode in SectionModes)
                        {
                            String Value = XMode.Attribute("ModeNumber").Value;
                            Int32 NumMode = Convert.ToInt32(Value);
                            // Chercher le CommentairesByMode correspondant
                            var Query = from cbm in this.CommentaireByModes
                                        where cbm.LinkedMode.Position == NumMode
                                        select cbm;

                            if (Query.Count() > 0)
                            {
                                ModeEasyCommentaire MEC = Query.First();

                                // Lire l'ensemble des commentaires et les affecter aux sorties
                                IEnumerable<XElement> XCommentaire = XMode.Descendants("Commentaire");
                                if (XCommentaire != null && XCommentaire.Count() > 0)
                                {
                                    foreach (XElement commentaire in XCommentaire)
                                    {
                                        Value = commentaire.Attribute("SortieMnemologique").Value;
                                        var QueryCom = from com in MEC.EasyCommentaires
                                                    where com.LinkedES.MnemoLogique == Value
                                                    select com;

                                        if (QueryCom.Count() > 0)
                                        {
                                            QueryCom.First().Commentaire = commentaire.Value;
                                            String StrIsImplemented = commentaire.Attribute("IsImplemented").Value;
                                            if (StrIsImplemented == "False")
	                                        {
                                                QueryCom.First().IsImplemented = false;
	                                        }
                                            else
                                            {
                                                QueryCom.First().IsImplemented = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        } // endMethod: Load

        /// <summary>
        /// La ligne corrspondant au commentaire a été implémentée
        /// </summary>
        public void CheckCommentaireIsImplemented()
        {
            ModeEasyCommentaire MECUniv = null;

            // Remmettre à 0 l'évaluation
            for (int i = 0; i < this.CommentaireByModes.Count; i++)
            {
                // Recupérer le mode universel
                if (this.CommentaireByModes[i].LinkedMode.Position == 32)
                {
                    MECUniv = this.CommentaireByModes[i];
                }
                for (int j = 0; j < this.CommentaireByModes[i].EasyCommentaires.Count; j++)
                {
                    this.CommentaireByModes[i].EasyCommentaires[j].IsImplemented = false;
                }
            }

            if (MECUniv != null)
            {
                // Analyser les sorties des équations implémentées manuellement ou automatiquement
                // Si les sorties sont implémentées dans le mode universel, les checker de partout
                foreach (var formule in PegaseData.Instance.ParamHorsMode.Formules)
                {
                    foreach (var equation in formule.Equations)
                    {
                        var QuerySortie = from sortie in MECUniv.EasyCommentaires
                                          where sortie.LinkedES.MnemoLogique == equation.MnemoLogique
                                          select sortie;
                        if (QuerySortie.Count() > 0)
                        {
                            QuerySortie.First().IsImplemented = true;
                        }
                    }
                } 
            }

            // pour chacun des modes, trouver les sorties déjà implémentées.
            foreach (ModeEasyCommentaire mode in this.CommentaireByModes)
            {
                if (mode.LinkedMode.Position != 32)
                {
                    foreach (var formule in mode.LinkedMode.Formules)
                    {
                        foreach (var equation in formule.Equations)
                        {
                            var QuerySortie = from sortie in mode.EasyCommentaires
                                              where sortie.LinkedES.MnemoLogique == equation.MnemoLogique
                                              select sortie;
                            if (QuerySortie.Count() > 0)
                            {
                                QuerySortie.First().IsImplemented = true;
                            }
                        }
                    }
                    for (int i = 0; i < mode.EasyCommentaires.Count; i++)
                    {
                        mode.EasyCommentaires[i].IsImplemented = mode.EasyCommentaires[i].IsImplemented | MECUniv.EasyCommentaires[i].IsImplemented;
                    }
                }
            }
        } // endMethod: CheckCommentaireIsImplemented

        /// <summary>
        /// Ajouter un mode à la collection des CommentaireByModes
        /// </summary>
        public void AddMode ( ModeExploitation mode )
        {
            ModeEasyCommentaire MEC = ModeEasyCommentaire.BuildMode(mode);
            this.CommentaireByModes.Add(MEC);
        } // endMethod: AddMode
        
        /// <summary>
        /// Supprimer un mode à la collection des CommentaireByModes
        /// </summary>
        public void RemoveMode ( ModeExploitation mode )
        {
            var Query = from easyMode in this.CommentaireByModes
                        where easyMode.LinkedMode == mode
                        select easyMode;

            if (Query.Count() > 0)
            {
                for (int i = Query.Count() - 1 ; i >= 0; i--)
                {
                    this.CommentaireByModes.Remove(Query.ElementAt(i));
                }
            }
        } // endMethod: RemoveMode

        /// <summary>
        /// Construire la collection complète des CommentaireByModes
        /// Attention : cette collection est vide par défaut. Utiliser la méthode Load pour construire une collection remplie avec les commentaires édités lors de la session précédente...
        /// </summary>
        public void BuilCommentairesByMode ( )
        {
            // 1 - vider la collection
            this.CommentaireByModes.Clear();

            // 2 - parcourir tous les modes et créer les ModeEasy correspondant
            if (PegaseData.Instance.OLogiciels != null)
            {
                // Mode universel
                if (PegaseData.Instance.ParamHorsMode != null)
                {
                    ModeExploitation ModeUniversel = new ModeExploitation(32);
                    this.AddMode(ModeUniversel);
                }
                // Modes d'exploitations
                if (PegaseData.Instance.OLogiciels.ModesExploitation != null)
                {
                    foreach (ModeExploitation mode in PegaseData.Instance.OLogiciels.ModesExploitation)
                    {
                        this.AddMode(mode);
                    }  
                }
            }
        } // endMethod: BuilCommentairesByMode

        /// <summary>
        /// Acquérire le commentaire correspondant au numéro de mode et au Mnémohardware
        /// </summary>
        public CommentairesEasy GetEasyCommentaire ( Int32 NumMode, String MnemoHardware )
        {
            CommentairesEasy Result = null;

            var queryMode = from easyCommentaire in this.CommentaireByModes
                            where easyCommentaire.LinkedMode.Position == NumMode
                            select easyCommentaire;

            if (queryMode.Count() > 0)
            {
                var queryES = from easyCommentaire in queryMode.First().EasyCommentaires
                              where easyCommentaire.LinkedES.MnemoHardware == MnemoHardware
                              select easyCommentaire;

                if (queryES.Count() > 0 && queryES.First().Commentaire != "")
                {
                    Result = queryES.First();
                }
            }

            return Result;
        } // endMethod: GetEasyCommentaire

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: Commentaires
}