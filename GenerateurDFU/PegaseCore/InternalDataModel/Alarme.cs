using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace JAY.PegaseCore
{

    public class Alarme
    {
        // Variables
        #region Variables

        private InformationLabel _informationL;
        private InformationLabel _informationT;
        private Int16 _tempo;
        private Byte _CmdSBCAlarme;
        private String _commentaireAlarme;

        #endregion

        // Propriétés
        #region Propriétés

        /// <summary>
        /// Commentaire pour l'alarme
        /// </summary>
        public String CommentaireAlarme
        {
            get
            {
                if (this._commentaireAlarme == null)
                {
                    this._commentaireAlarme = "";
                }
                return this._commentaireAlarme;
            }
            set
            {
                this._commentaireAlarme = value;
            }
        } // endProperty: CommentaireAlarme

        /// <summary>
        /// L'octet permettant de paramétrer le comportement sur support de charge
        /// </summary>
        public Byte CmdSBCAlarme
        {
            get
            {
                return this._CmdSBCAlarme;

            }
            set
            {
                this._CmdSBCAlarme = value;
            }
        } // endProperty:  CmdSBCRetour

        /// <summary>
        /// La commande du relais 1 du support de charge
        /// </summary>
        public Boolean CmdRelai1SBC
        {
            get
            {
                return JAY.PegaseCore.Helper.BitHelper.ReadBit(this.CmdSBCAlarme, 0);
            }
            set
            {
                this.CmdSBCAlarme = JAY.PegaseCore.Helper.BitHelper.SetBit(this.CmdSBCAlarme, 0, value);
            }
        } // endProperty: CmdRelai1SBC

        /// <summary>
        /// La commande du relais 2 du support de charge
        /// </summary>
        public Boolean CmdRelai2SBC
        {
            get
            {
                return JAY.PegaseCore.Helper.BitHelper.ReadBit(this.CmdSBCAlarme, 1);
            }
            set
            {
                this.CmdSBCAlarme = JAY.PegaseCore.Helper.BitHelper.SetBit(this.CmdSBCAlarme, 1, value);
            }
        } // endProperty: CmdRelai2SBC

        /// <summary>
        /// La commande du buzzer du support de charge
        /// </summary>
        public Boolean CmdBuzSBC
        {
            get
            {
                return JAY.PegaseCore.Helper.BitHelper.ReadBit(this.CmdSBCAlarme, 2);
            }
            set
            {
                this.CmdSBCAlarme = JAY.PegaseCore.Helper.BitHelper.SetBit(this.CmdSBCAlarme, 2, value);
            }
        } // endProperty: CmdBuzSBC

        /// <summary>
        /// Le descriptif de la fonction de l'alarme
        /// </summary>
        public String Fonction
        {
            get;
            set;
        } // endProperty: Fonction

        /// <summary>
        /// Le Mnemologique de l'alarme
        /// </summary>
        public String Mnemologique
        {
            get;
            set;
        } // endProperty: Mnemologique

        /// <summary>
        /// Est ce que le MO doit vibrer lors de l'affichage de l'alarme
        /// </summary>
        public Boolean Vibrer
        {
            get;
            set;
        } // endProperty: Vibrer

        
                    /// <summary>
        /// Le retour est-il distant (pont couplé)
        /// </summary>
        public Boolean Distant
        {
            get;
            set;
        } // endProperty: Vibrer
        /// <summary>
        /// Est-ce que l'alarme doit être affichée
        /// </summary>
        public Boolean Afficher
        {
            get;
            set;
        } // endProperty: Afficher

        /// <summary>
        /// Evenement
        /// </summary>
        public Int16 Event
        {
            get;
            set;
        } // endProperty: Event

        /// <summary>
        /// Y a-t-il un auto acquittement?
        /// </summary>
        public Boolean AutoAcquit
        {
            get;
            set;
        } // endProperty: AutoAcquit

        /// <summary>
        /// Ne pas utiliser le bouton d'acquittement
        /// </summary>
        public Int16 NonUtiliseBtAcquit
        {
            get;
            set;
        } // endProperty: NonUtiliseBtAcquit

        /// <summary>
        /// La temporisation de l'alarme
        /// </summary>
        public Int16 Tempo
        {
            get
            {
                return this._tempo;
            }
            set
            {
                this._tempo = value;
                if (this._tempo < 1)
                {
                    this._tempo = 1;
                }
                else if (this._tempo > 3600)
                {
                    this._tempo = 3600;
                }
            }
        } // endProperty: Tempo

        /// <summary>
        /// Destination vers le journal de bord
        /// </summary>
        public Int16 DestinationJdB
        {
            get;
            set;
        } // endProperty: DestinationJdB

        /// <summary>
        /// Code de l'alarme pour le journal de bord
        /// </summary>
        public Int16 CodeAlarmeJdB
        {
            get;
            set;
        } // endProperty: CodeAlarmeJdB

        /// <summary>
        /// Identifiant vers le libellé situé dans la table Libellé Retour d'Info
        /// </summary>
        public String IdentLibelInformation
        {
            get;
            set;
        } // endProperty: IdentLibelInformation

        /// <summary>
        /// Identifiant vers le libellé situé dans la table Libellé Retour d'Info
        /// </summary>
        public String IdentLibelTitre
        {
            get;
            set;
        } // endProperty: IdentLibelInformation

        /// <summary>
        /// Retourner le libellé information
        /// </summary>
        public InformationLabel InformationL
        {
            get
            {
                InformationLabel Result = null;
                if (this._informationL == null)
                {
                    if (this.IdentLibelInformation != null && this.IdentLibelInformation != "" && PegaseData.Instance.OLogiciels != null)
                    {
                        var Query = from label in PegaseData.Instance.OLogiciels.LibelRI
                                    where label.IdentLibelInformation == this.IdentLibelInformation
                                    select label;

                        this._informationL = Query.FirstOrDefault();

                        if (this._informationL == null)
                        {
                            // L'ID du libellé ne fait référence à un libellé du dictionnaire
                            InformationLabel IL = PegaseData.Instance.OLogiciels.AddLibelRetourInfo();
                            this._informationL = IL;
                            this.IdentLibelInformation = IL.IdentLabel;
                        }

                        Result = this._informationL;
                    }
                    else if(PegaseData.Instance.OLogiciels != null)
                    {
                        // Créer un libellé
                        this._informationL = PegaseData.Instance.OLogiciels.AddLibelRetourInfo();
                        this.IdentLibelInformation = this._informationL.IdentLibelInformation;
                        Result = this._informationL;
                    }
                }
                else
                {
                    Result = this._informationL;
                }

                return Result;
            }
            set
            {
                this._informationL = value;
            }
        } // endProperty: InformationL

        /// <summary>
        /// Retourner le libellé information
        /// </summary>
        public InformationLabel InformationT
        {
            get
            {
                InformationLabel Result = null;
                if (this._informationT == null)
                {
                    if (this.IdentLibelTitre != null && this.IdentLibelTitre != "" && PegaseData.Instance.OLogiciels != null)
                    {
                        var Query = from label in PegaseData.Instance.OLogiciels.LibelRI
                                    where label.IdentLibelInformation == this.IdentLibelTitre
                                    select label;

                        this._informationT = Query.FirstOrDefault();

                        if (this._informationT == null)
                        {
                            // L'ID du libellé ne fait référence à un libellé du dictionnaire
                            InformationLabel IL = PegaseData.Instance.OLogiciels.AddLibelRetourInfo();
                            this._informationT = IL;
                            this._informationT.Label = "";
                            this.IdentLibelTitre = IL.IdentLabel;
                        }

                        Result = this._informationT;
                    }
                    else if (PegaseData.Instance.OLogiciels != null)
                    {
                        // Créer un libellé
                        this._informationT = PegaseData.Instance.OLogiciels.AddLibelRetourInfo();
                        this._informationT.LibelInformation = "";
                       
                        this.IdentLibelTitre = this._informationT.IdentLibelInformation;
                       
                        Result = this._informationT;
                    }
                }
                else
                {
                    Result = this._informationT;
                }

                return Result;
            }
            set
            {
                this._informationT = value;
            }
        } // endProperty: InformationL


        /// <summary>
        /// Le libellé information de l'alarme
        /// </summary>
        public String LibelTitre
        {
            get
            {
                String Result = "";

                if (this.InformationT != null)
                {
                    Result = this.InformationT.LibelInformation;
                }

                return Result;
            }
            set
            {
                if (this.InformationT == null)
                {
                    this.InformationT = PegaseData.Instance.OLogiciels.AddLibelRetourInfo();
                    this.IdentLibelTitre = this.InformationT.IdentLibelInformation;
                }
                this.InformationT.LibelInformation = value;
            }
        } // endProperty: LibelInformation

        /// <summary>
        /// Le libellé information de l'alarme
        /// </summary>
        public String LibelInformation
        {
            get
            {
                String Result = "";

                if (this.InformationL != null)
                {
                    Result = this.InformationL.LibelInformation;
                }

                return Result;
            }
            set
            {
                if (this.InformationL == null)
                {
                    this.InformationL = PegaseData.Instance.OLogiciels.AddLibelRetourInfo();
                    this.IdentLibelInformation = this.InformationL.IdentLibelInformation;
                }
                this.InformationL.LibelInformation = value;
            }
        } // endProperty: LibelInformation

        /// <summary>
        /// Le libellé est-il en gras?
        /// </summary>
        //public Boolean IsBold
        //{
        //    get
        //    {
        //        Boolean Result = false;

        //        if (this.InformationL != null)
        //        {
        //            Result = this.InformationL.PoliceGrasInformation;
        //        }

        //        return Result;
        //    }
        //    set
        //    {
        //        if (this.InformationL != null)
        //        {
        //            this.InformationL.PoliceGrasInformation = value;
        //        }
        //    }
        //} // endProperty: IsBold

        #endregion

        // Constructeur
        #region Constructeur

        public Alarme(XElement alarme)
        {
            this.InitAlarme(alarme);
        }

        #endregion

        // Méthodes
        #region Méthodes

        /// <summary>
        /// Initialiser l'alarme à partir d'un noeud XML
        /// </summary>
        public void InitAlarme ( XElement alarme )
        {
            XMLCore.XMLProcessing XProcess = new XMLCore.XMLProcessing();
            XProcess.OpenXML(alarme);

            // Fonction
            String Value = XProcess.GetNodeByPath("Fonction").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            this.Fonction = Value;

            // Mnemologique
            Value = XProcess.GetNodeByPath("MnemoLogique").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            this.Mnemologique = Value;

            // Vibrer
            Value = XProcess.GetNodeByPath("Vibrer").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            this.Vibrer = this.DetermineBooleanValue(Value);

            // Afficher
            Value = XProcess.GetNodeByPath("Afficher").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            this.Afficher = this.DetermineBooleanValue(Value);

            // Event
            Value = XProcess.GetNodeByPath("Event").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            this.Event = this.DetermineInt16Value(Value);

            // AutoAcquit
            Value = XProcess.GetNodeByPath("AutoAcquit").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            this.AutoAcquit = this.DetermineBooleanValue(Value);

            // NonUtiliseBtAcquit
            Value = XProcess.GetNodeByPath("NonUiliseBtAcquit").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            this.NonUtiliseBtAcquit = this.DetermineInt16Value(Value);

            // Tempo
            Value = XProcess.GetNodeByPath("Tempo").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            this.Tempo = this.DetermineInt16Value(Value);

            // DestinationJdB
            Value = XProcess.GetNodeByPath("DestinationJdB").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            this.DestinationJdB = this.DetermineInt16Value(Value);

            // CodeAlarmeJdB
            Value = XProcess.GetNodeByPath("CodeAlarmeJdB").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            this.CodeAlarmeJdB = this.DetermineInt16Value(Value);

            // IdentLibelInformation
            Value = XProcess.GetNodeByPath("IdentLibelInformation").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            this.IdentLibelInformation = Value;

            // Comportement sur chargeur
            Value = XProcess.GetValue("CmdSBCAlarme", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
            try
            {
                this.CmdSBCAlarme = Convert.ToByte(Value);
            }
            catch
            {
                this.CmdSBCAlarme = 0;
            }
            // Comportement sur chargeur
            Value = XProcess.GetValue("AlarmeDistante", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
            try
            {
                this.Distant = Convert.ToBoolean(Value);
            }
            catch
            {
                this.Distant = false;
            }
            Value = XProcess.GetValue("IndiceLibelleTitre", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
            try
            {
                if (Value != null)
                {
                    if (Value == this.IdentLibelInformation)
                    {
                        this.IdentLibelTitre = "";
                    }
                    else
                    {
                        this.IdentLibelTitre = Value;
                    }
                }
                else
                {
                    this.IdentLibelTitre = "";
                }
            }
            catch
            {
                this.IdentLibelTitre = "";
            }
        } // endMethod: InitAlarme

        /// <summary>
        /// Stocke les données de l'alarme dans le noeud XML transmis
        /// </summary>
        public void SerialiseAlarme ( XElement alarme )
        {
            XMLCore.XMLProcessing XProcess = new XMLCore.XMLProcessing();
            XProcess.OpenXML(alarme);

            // Fonction
            XProcess.GetNodeByPath("Fonction").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.Fonction;

            // Mnemologique
            XProcess.GetNodeByPath("Mnemologique").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.Mnemologique;

            // Vibrer
            XProcess.GetNodeByPath("Vibrer").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.DetermineStringFromBoolean(this.Vibrer);

            // Afficher
            XProcess.GetNodeByPath("Afficher").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.DetermineStringFromBoolean(this.Afficher);

            // Event
            XProcess.GetNodeByPath("Event").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.Event.ToString();

            // AutoAcquit
            XProcess.GetNodeByPath("AutoAcquit").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.DetermineStringFromBoolean(this.AutoAcquit);

            // NonUtiliseBtAcquit
            XProcess.GetNodeByPath("NonUiliseBtAcquit").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.NonUtiliseBtAcquit.ToString();

            // Tempo
            XProcess.GetNodeByPath("Tempo").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.Tempo.ToString();

            // DestinationJdB
            XProcess.GetNodeByPath("DestinationJdB").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.DestinationJdB.ToString();

            // CodeAlarmeJdB
            XProcess.GetNodeByPath("CodeAlarmeJdB").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.CodeAlarmeJdB.ToString();

            if (this.InformationL != null)
            {
                if (this.InformationL.LibelInformation != "")
                {
                    // IdentLibelInformation
                    XProcess.GetNodeByPath("IdentLibelInformation").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.IdentLibelInformation;
                }
                else
                {
                    XProcess.GetNodeByPath("IdentLibelInformation").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "";
                    PegaseData.Instance.OLogiciels.DeleteLibelRetourInfo(this.InformationL);
                } 
            }

            // Comportement sur chargeur

            if (XProcess.GetNodeByPath("CmdSBCAlarme") == null)
            {
                // Construire la variable XML
                XElement element = new XElement(XProcess.GetNodeByPath("IdentLibelInformation").First());
                element.Attribute(XMLCore.XML_ATTRIBUTE.CODE).Value = "CmdSBCAlarme";
                XProcess.InsertXElement(XProcess.GetNodeByPath("IdentLibelInformation").First(), element);
                
            }
            XProcess.SetValue("CmdSBCAlarme", "", "", XMLCore.XML_ATTRIBUTE.VALUE, this.CmdSBCAlarme.ToString());
            if (XProcess.GetNodeByPath("AlarmeDistante") == null)
            {
                // Construire la variable XML
                XElement element = new XElement(XProcess.GetNodeByPath("IdentLibelInformation").First());
                element.Attribute(XMLCore.XML_ATTRIBUTE.CODE).Value = "AlarmeDistante";
                XProcess.InsertXElement(XProcess.GetNodeByPath("IdentLibelInformation").First(), element);

            }
            XProcess.SetValue("AlarmeDistante", "", "", XMLCore.XML_ATTRIBUTE.VALUE, this.Distant.ToString());
            if (XProcess.GetNodeByPath("IndiceLibelleTitre") == null)
            {
                // Construire la variable XML
                XElement element = new XElement(XProcess.GetNodeByPath("IdentLibelInformation").First());
                element.Attribute(XMLCore.XML_ATTRIBUTE.CODE).Value = "IndiceLibelleTitre";
                element.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "";
                XProcess.InsertXElement(XProcess.GetNodeByPath("IdentLibelInformation").First(), element);

            }
            if (this._informationT != null)
            {
                //if (this.InformationT.LibelInformation != "")
                if (true)
                {
                   // IdentLibelInformation
                    XProcess.GetNodeByPath("IndiceLibelleTitre").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.IdentLibelTitre;
                }
                else
                {
                    XProcess.GetNodeByPath("IndiceLibelleTitre").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = "";
                    
                    PegaseData.Instance.OLogiciels.DeleteLibelRetourInfo(this.InformationT);
                    this.InformationT = null;
                }
            }
           } // endMethod: SerialiseAlarme
        
        /// <summary>
        /// Déterminer la valeur Int16 à partir d'une valeur texte
        /// </summary>
        public Int16 DetermineInt16Value ( String Value )
        {
            Int16 Result = 0;

            try
            {
                Result = Convert.ToInt16(Value.Trim());
            }
            catch
            {
                Result = 0;
            }

            return Result;
        } // endMethod: DetermineInt16Value
        
        /// <summary>
        /// Determine la valeur string depuis un boolean
        /// </summary>
        public String DetermineStringFromBoolean ( Boolean Value )
        {
            String Result;

            if (Value)
            {
                Result = "1";
            }
            else
            {
                Result = "0";
            }
            
            return Result;
        } // endMethod: DetermineStringFromBoolean

        /// <summary>
        /// Determiner la valeur Booleene à partir d'une valeur texte
        /// </summary>
        public Boolean DetermineBooleanValue ( String Value )
        {
            Boolean Result = false;

            if (Value.Trim() == "1")
            {
                Result = true;
            }

            return Result;
        } // endMethod: DetermineBooleanValue

        #endregion

        // Messages
        #region Messages

        #endregion

    } // endClass: Alarme
}
