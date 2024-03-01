using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using Mvvm = GalaSoft.MvvmLight;
using JAY.PegaseCore;
using System.Xml;

using System.IO;


using JAY.XMLCore;
using JAY.FileCore;

using JAY.PegaseCore.Helper;


namespace JAY.PegaseCore
{
    public class EC_MaskMode : Mvvm.ViewModelBase
    {
        private UInt32 _togglesarttmask;
        private Byte _verifierjs1;
        private Byte _verifierjs2;
        private Byte _verifierjs3;
        private Byte _verifierjs1ana;
        private Byte _verifierjs2ana;
        private Byte _verifierjs3ana;
        private String _name;


        private ObservableCollection<OutputTemplateII> _outputs;
                // Constructeur
        

        public EC_MaskMode()
        {
            String XValue;
            // Initialiser

            // Arrêt passif
            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/Configorgane/VerifierBpTogg", "", "", XML_ATTRIBUTE.VALUE);
            if ((XValue != null) && (XValue != ""))
            {
                try
                {
                    if (!XValue.Contains("0x"))
                    {
                        this.ToggleStartMask = Convert.ToUInt32(XValue);
                    }
                    else
                    {
                        this.ToggleStartMask = Convert.ToUInt32(XValue.Substring(2), 16);
                    }
                }
                catch
                {
                    this.ToggleStartMask = 0xFFFFFFFF;
                }
            }
            else
            {
                this.ToggleStartMask = 0xFFFFFFFF;
            }


            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsJoystick/JoyStick_1/VerifierJS1", "", "", XML_ATTRIBUTE.VALUE);
            if (XValue != null && XValue != "")
            {
                this.VerifierJs1 = (byte)Tools.ConvertASCIIToInt32(XValue);
            }
            else
            {
                this.VerifierJs1 = 0;
            }


            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsJoystick/JoyStick_2/VerifierJS2", "", "", XML_ATTRIBUTE.VALUE);
            if (XValue != null && XValue != "")
            {
                this.VerifierJs2 = (byte)Tools.ConvertASCIIToInt32(XValue);
            }
            else
            {
                this.VerifierJs2 = 0;
            }
            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsJoystick/JoyStick_3/VerifierJS3", "", "", XML_ATTRIBUTE.VALUE);
            if (XValue != null && XValue != "")
            {
                this.VerifierJs3 = (byte)Tools.ConvertASCIIToInt32(XValue);
            }
            else
            {
                this.VerifierJs3 = 0;
            }
            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsJoystick/JoyStick_1/VerifierJS1Ana", "", "", XML_ATTRIBUTE.VALUE);
            if (XValue != null && XValue != "")
            {
                this.VerifierJs1Ana = (byte)Tools.ConvertASCIIToInt32(XValue);
            }
            else
            {
                this.VerifierJs1Ana = 0;
            }
            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsJoystick/JoyStick_2/VerifierJS2Ana", "", "", XML_ATTRIBUTE.VALUE);
            if (XValue != null && XValue != "")
            {
                this.VerifierJs2Ana = (byte)Tools.ConvertASCIIToInt32(XValue);
            }
            else
            {
                this.VerifierJs2Ana = 0;
            }
            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsJoystick/JoyStick_3/VerifierJS3Ana", "", "", XML_ATTRIBUTE.VALUE);
            if (XValue != null && XValue != "")
            {
                this.VerifierJs3Ana = (byte)Tools.ConvertASCIIToInt32(XValue);
            }
            else
            {
                this.VerifierJs3Ana = 0;
            }
            this.InitMask();
        }

        private void UpdateOuputs()
        {

        }

        public int LineNumber
        {
            get
            {
                int Result = 0;
                return Result;
            }
        } // endProperty: ModeName


                
        /// <summary>
        /// Le nom du produit
        /// </summary>
        public String NameII
        {
            get
            {
                this._name = LanguageSupport.Get().GetText("EASYCONF/MASKMODE");
                return this._name;
            }
            set
            {
                this._name = value;
                RaisePropertyChanged("Name");
            }
        }

        private Boolean FindStateOrgan(string organ, ref int index)
        {
           
            UInt32 masque = this.ToggleStartMask ;
            if (organ.Equals("BOUTON_13")) { index = 1; return ((Boolean)((masque >> 1) & 0x01).Equals(0)); }
            else if (organ.Equals("BOUTON_11")) { index = 2; return ((Boolean)((masque >> 2) & 0x01).Equals(0)); }
            else if (organ.Equals("BOUTON_12")) { index = 3; return ((Boolean)((masque >> 3) & 0x01).Equals(0)); }
            else if (organ.Equals("BOUTON_01")) { index = 4; return ((Boolean)((masque >> 4) & 0x01).Equals(0)); }
            else if (organ.Equals("BOUTON_02")) { index = 5; return ((Boolean)((masque >> 5) & 0x01).Equals(0)); }
            else if (organ.Equals("BOUTON_03")) { index = 6; return ((Boolean)((masque >> 6) & 0x01).Equals(0)); }
            else if (organ.Equals("BOUTON_04")) { index = 7; return ((Boolean)((masque >> 7) & 0x01).Equals(0)); }
            else if (organ.Equals("BOUTON_05")) { index = 8; return ((Boolean)((masque >> 8) & 0x01).Equals(0)); }
            else if (organ.Equals("BOUTON_06")) { index = 9; return ((Boolean)((masque >> 9) & 0x01).Equals(0)); }
            else if (organ.Equals("BOUTON_07")) { index = 10; return ((Boolean)((masque >> 10) & 0x01).Equals(0)); }
            else if (organ.Equals("BOUTON_08")) { index = 11; return ((Boolean)((masque >> 11) & 0x01).Equals(0)); }
            else if (organ.Equals("BOUTON_09")) { index = 12; return ((Boolean)((masque >> 12) & 0x01).Equals(0)); }
            else if (organ.Equals("BOUTON_10")) { index = 13; return ((Boolean)((masque >> 13) & 0x01).Equals(0)); }


            else if (organ.Equals("BOUTON_14")) { index = 17; return ((Boolean)((masque >> 17) & 0x01).Equals(0)); }
            else if (organ.Equals("BOUTON_18")) { index = 18; return ((Boolean)((masque >> 18) & 0x01).Equals(0)); }
            else if (organ.Equals("BOUTON_19")) { index = 19; return ((Boolean)((masque >> 19) & 0x01).Equals(0)); }

            else if (organ.Equals("COMMUTATEUR_01")) { index = 20; return ((Boolean)((masque >> 20) & 0x01).Equals(0)); }
            else if (organ.Equals("COMMUTATEUR_02")) { index = 21; return ((Boolean)((masque >> 21) & 0x01).Equals(0)); }
            else if (organ.Equals("COMMUTATEUR_03")) { index = 22; return ((Boolean)((masque >> 22) & 0x01).Equals(0)); }
            else if (organ.Equals("COMMUTATEUR_04")) { index = 23; return ((Boolean)((masque >> 23) & 0x01).Equals(0)); }
            else if (organ.Equals("COMMUTATEUR_05")) { index = 24; return ((Boolean)((masque >> 24) & 0x01).Equals(0)); }
            else if (organ.Equals("COMMUTATEUR_06")) { index = 25; return ((Boolean)((masque >> 25) & 0x01).Equals(0)); }
            else if (organ.Equals("COMMUTATEUR_07")) { index = 26; return ((Boolean)((masque >> 26) & 0x01).Equals(0)); }
            else if (organ.Equals("COMMUTATEUR_08")) { index = 27; return ((Boolean)((masque >> 27) & 0x01).Equals(0)); }
            else if (organ.Equals("COMMUTATEUR_09")) { index = 28; return ((Boolean)((masque >> 28) & 0x01).Equals(0)); }
            else if (organ.Equals("COMMUTATEUR_10")) { index = 29; return ((Boolean)((masque >> 29) & 0x01).Equals(0)); }
            else if (organ.Equals("COMMUTATEUR_11")) { index = 30; return ((Boolean)((masque >> 30) & 0x01).Equals(0)); }
            else if (organ.Equals("COMMUTATEUR_12")) { index = 31; return ((Boolean)((masque >> 31) & 0x01).Equals(0)); }
            else if (organ.Equals("BOUTON_15")) { index = -10; return !(this._verifierjs1.Equals(0)); }
            else if (organ.Equals("BOUTON_16")) { index = -11; return !(this._verifierjs2.Equals(0)); }
            else if (organ.Equals("BOUTON_17")) { index = -12; return !(this._verifierjs3.Equals(0)); }
            else { index = -1; return (true); }
        }
        

        /// <summary>
        /// Initialiser les effecteurs
        /// </summary>
        private void InitMask()
        {
            this.OutputsII = new ObservableCollection<OutputTemplateII>();
            
            foreach (var organ in JAY.PegaseCore.EasyConfigData.Get().ColonnesBtn)
            {
                //if (organ.IsEnable.Equals(true))
                //{
                    OutputTemplateII item = new OutputTemplateII();
                    int index=0;
                    if (FindStateOrgan(organ.Mmnemologique, ref index))
                    {
                        item.IsUsed = true;
                    }
                    else
                    {
                        item.IsUsed = false;
                    }
                    item.IsEnable = true;
                    this.OutputsII.Add(item);
                //}
            }
        } // endMethod: InitEffecteurs




      

        // Propriétés
        #region Propriétés
        /// <summary>
        /// Les relais / sorties disponiblent pouvant être affectés par cette sortie
        /// </summary>
        public ObservableCollection<OutputTemplateII> OutputsII
        {
            get
            {
                return this._outputs;
            }
            set
            {
                this._outputs = value;
                RaisePropertyChanged("Outputs");
            }
        } // endProperty: Outputs


        public void SaveMask()
        {
            int cpt = 0;
            uint maskcalc = 0xFFFFFFFF;
            byte bp1 = 0;
            byte bp2 = 0;
            byte bp3 = 0;

            foreach (var organ in JAY.PegaseCore.EasyConfigData.Get().ColonnesBtn)
            {
                OutputTemplateII item = new OutputTemplateII();
                int index = -1;

                FindStateOrgan(organ.Mmnemologique, ref index);
                if (index > 0)
                {
                    if (this.OutputsII[cpt].IsEnable == true)
                    {
                        if (this.OutputsII[cpt].IsUsed == true)
                        {
                            maskcalc = maskcalc & (~((uint)1 << index));
                        }
                    }
                }
                else
                {
                    if (this.OutputsII[cpt].IsEnable == true)
                    {
                        if (this.OutputsII[cpt].IsUsed == true)
                        {
                            if (index == -10)
                            {
                                bp1 = 1;
                            }
                            else if (index == -11)
                            {
                                bp2 = 1;
                            }
                            else if (index == -12)
                            {
                                bp3 = 1;
                            }
                        }
                    }
                }
                cpt++;
            }
            this.ToggleStartMask = maskcalc;
            this.VerifierJs1 = bp1;
            this.VerifierJs2 = bp2;
            this.VerifierJs3 = bp3;


            String XValue;
            //XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/Configorgane/VerifierBpTogg", "", "", XML_ATTRIBUTE.VALUE);

            XValue = maskcalc.ToString();// Tools.UInt32ToBitfield32(maskcalc);
            PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/Configorgane/VerifierBpTogg", "", "", XML_ATTRIBUTE.VALUE,XValue);

            XValue = this.VerifierJs1.ToString();
            PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsJoystick/JoyStick_1/VerifierJS1", "", "", XML_ATTRIBUTE.VALUE, XValue);
            XValue = this.VerifierJs2.ToString();
            PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsJoystick/JoyStick_2/VerifierJS2", "", "", XML_ATTRIBUTE.VALUE, XValue);
            XValue = this.VerifierJs3.ToString();
            PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ConfigBoutonsJoystick/JoyStick_3/VerifierJS3", "", "", XML_ATTRIBUTE.VALUE, XValue);


        }
        /// <summary>
        /// 
        /// </summary>
        public UInt32 ToggleStartMask
        {
            get
            {
                return this._togglesarttmask;
            }
            set
            {
                this._togglesarttmask = value;
            }
        } // endProperty
        /// <summary>
        /// 
        /// </summary>
        public Byte VerifierJs1
        {
            get
            {
                return this._verifierjs1;
            }
            set
            {
                this._verifierjs1 = value;
            }
        } // endProperty
        /// <summary>
        /// 
        /// </summary>
        public Byte VerifierJs2
        {
            get
            {
                return this._verifierjs2;
            }
            set
            {
                this._verifierjs2 = value;
            }
        } // endProperty
        /// <summary>
        /// 
        /// </summary>
        public Byte VerifierJs3
        {
            get
            {
                return this._verifierjs3;
            }
            set
            {
                this._verifierjs3 = value;
            }
        } // endProperty
        /// <summary>
        /// 
        /// </summary>
        public Byte VerifierJs1Ana
        {
            get
            {
                return this._verifierjs1ana;
            }
            set
            {
                this._verifierjs1ana = value;
            }
        } // endProperty
        /// <summary>
        /// 
        /// </summary>
        public Byte VerifierJs2Ana
        {
            get
            {
                return this._verifierjs2ana;
            }
            set
            {
                this._verifierjs2ana = value;
            }
        } // endProperty
        /// <summary>
        /// 
        /// </summary>
        public Byte VerifierJs3Ana
        {
            get
            {
                return this._verifierjs3ana;
            }
            set
            {
                this._verifierjs3ana = value;
            }
        } // endProperty
        #endregion
    }
}
