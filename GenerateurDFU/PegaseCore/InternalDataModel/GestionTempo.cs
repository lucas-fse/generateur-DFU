using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using JAY.XMLCore;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Gestion des variables de temporisation
    /// </summary>
    public class GestionTempo
    {
        #region Constantes
        private Int32 DELAI_MIN_MISE_EN_VEILLE = 60;
        #endregion

        #region Variables

        private Int32   _delaiArretPassif;
        private Int32   _minDelaiArretPassif;
        private Int32   _maxDelaiArretPassif;

        private Boolean _activationMiseEnVeille;
        private Int32   _delaiMiseEnVeille;
        private Int32   _minDelaiMiseEnVeille;
        private Int32   _maxDelaiMiseEnVeille;

        private Boolean _activationHommeMort;
        private Int32   _delaiAlarmeHommeMort;
        private Int32   _minDelaiAlarmeHommeMort;
        private Int32   _maxDelaiAlarmeHommeMort;

        private Boolean _activationAbsRadioMT;
        private Int32   _delaiAbsRadioMT;

        private Int32   _dureeAlarmeHommeMort;
        private Int32   _minDureeAlarmeHommeMort;
        private Int32   _maxDureeAlarmeHommeMort;

        private UInt32  _maskHM32;
        private UInt16  _maskHM16;

        #endregion

        #region Propriétés

        /// <summary>
        /// Activation delai d'absence radio MT
        /// </summary>
        public Boolean ActivationABSRadioMT
        {
            get
            {
                return this._activationAbsRadioMT;
            }
            set
            {
                this._activationAbsRadioMT = value;
            }
        } // endProperty: ActivationABSRadioMT

        /// <summary>
        /// Délai avant mise en sécurité si abscence radio MT
        /// </summary>
        public Int32 DelaiABSRadioMT
        {
            get
            {
                return this._delaiAbsRadioMT;
            }
            set
            {
                this._delaiAbsRadioMT = value;
            }
        } // endProperty: DelaiABSRadioMT

        /// <summary>
        /// Le masque homme mort 32 bits
        /// </summary>
        public UInt32 MaskHM32
        {
            get
            {
                return this._maskHM32;
            }
            set
            {
                this._maskHM32 = value;
            }
        } // endProperty: MaskHM32

        /// <summary>
        /// Le masque homme mort 16 bits
        /// </summary>
        public UInt16 MaskHM16
        {
            get
            {
                return this._maskHM16;
            }
            set
            {
                this._maskHM16 = value;
            }
        } // endProperty: MaskHM16

        /// <summary>
        /// Delai de l'arrêt passif
        /// </summary>
        public Int32 DelaiArretPassif
        {
            get
            {
                return this._delaiArretPassif;
            }
            set
            {
                this._delaiArretPassif = value;
            }
        } // endProperty: DelaiArretPassif

        /// <summary>
        /// Minimum autorisé pour le delai arrêt passif
        /// </summary>
        public Int32 MinDelaiArretPassif
        {
            get
            {
                return this._minDelaiArretPassif;
            }
            private set
            {
                this._minDelaiArretPassif = value;
            }
        } // endProperty: MinDelaiArretPassif

        /// <summary>
        /// Maximum autorisé pour le delai arrêt passif
        /// </summary>
        public Int32 MaxDelaiArretPassif
        {
            get
            {
                return this._maxDelaiArretPassif;
            }
            private set
            {
                this._maxDelaiArretPassif = value;
            }
        } // endProperty: MaxDelaiArretPassif

        /// <summary>
        /// Activation de la mise en veille
        /// </summary>
        public Boolean ActivationMiseEnVeille
        {
            get
            {
                return this._activationMiseEnVeille;
            }
            set
            {
                this._activationMiseEnVeille = value;
            }
        } // endProperty: ActivationMiseEnVeille

        /// <summary>
        /// Delai avant la mise en veille. Delai min = 60 s. Si Delai < 60 -> Désactivation de la mise en veille
        /// </summary>
        public Int32 DelaiMiseEnVeille
        {
            get
            {
                return this._delaiMiseEnVeille;
            }
            set
            {
                this._delaiMiseEnVeille = value;
                if (this._delaiMiseEnVeille < DELAI_MIN_MISE_EN_VEILLE)
                {
                    this.ActivationMiseEnVeille = false;
                }
                else
                {
                    this.ActivationMiseEnVeille = true;
                }
            }
        } // endProperty: DelaiMiseEnVeille

        /// <summary>
        /// Minimum autorisé pour la mise en veille
        /// </summary>
        public Int32 MinDelaiMiseEnVeille
        {
            get
            {
                return this._minDelaiMiseEnVeille;
            }
            private set
            {
                this._minDelaiMiseEnVeille = value;
            }
        } // endProperty: MinDelaiMiseEnVeille

        /// <summary>
        /// Maximum autorisé pour la mise en veille
        /// </summary>
        public Int32 MaxDelaiMiseEnVeille
        {
            get
            {
                return this._maxDelaiMiseEnVeille;
            }
            private set
            {
                this._maxDelaiMiseEnVeille = value;
            }
        } // endProperty: MaxDelaiMiseEnVeille

        /// <summary>
        /// Activation de l'homme mort
        /// </summary>
        public Boolean ActivationHommeMort
        {
            get
            {
                return this._activationHommeMort;
            }
            set
            {
                this._activationHommeMort = value;
            }
        } // endProperty: ActivationHommeMort

        /// <summary>
        /// Delai de l'alarme homme mort
        /// </summary>
        public Int32 DelaiAlarmeHommeMort
        {
            get
            {
                return this._delaiAlarmeHommeMort;
            }
            set
            {
                this._delaiAlarmeHommeMort = value;
            }
        } // endProperty: DelaiAlarmeHommeMort

        /// <summary>
        /// Minimum autorisé pour le delai de l'alarme homme mort
        /// </summary>
        public Int32 MinDelaiAlarmeHommeMort
        {
            get
            {
                return this._minDelaiAlarmeHommeMort;
            }
            private set
            {
                this._minDelaiAlarmeHommeMort = value;
            }
        } // endProperty: MinDelaiAlarmeHommeMort

        /// <summary>
        /// Maximum autorisé pour le delai de l'alarme homme mort
        /// </summary>
        public Int32 MaxDelaiAlarmeHommeMort
        {
            get
            {
                return this._maxDelaiAlarmeHommeMort;
            }
            private set
            {
                this._maxDelaiAlarmeHommeMort = value;
            }
        } // endProperty: MaxDelaiAlarmeHommeMort

        /// <summary>
        /// La durée de l'alarme de l'homme mort
        /// </summary>
        public Int32 DureeAlarmeHommeMort
        {
            get
            {
                return this._dureeAlarmeHommeMort;
            }
            set
            {
                this._dureeAlarmeHommeMort = value;
            }
        } // endProperty: DureeAlarmeHommeMort

        /// <summary>
        /// Minimum autorisé pour la durée de l'alarme homme mort
        /// </summary>
        public Int32 MinDureeAlarmeHommeMort
        {
            get
            {
                return this._minDureeAlarmeHommeMort;
            }
            private set
            {
                this._minDureeAlarmeHommeMort = value;
            }
        } // endProperty: MinDureeAlarmeHommeMort

        /// <summary>
        /// Maximum autorisé pour la durée de l'alarme homme mort
        /// </summary>
        public Int32 MaxDureeAlarmeHommeMort
        {
            get
            {
                return this._maxDureeAlarmeHommeMort;
            }
            private set
            {
                this._maxDureeAlarmeHommeMort = value;
            }
        } // endProperty: MaxDureeAlarmeHommeMort

        public Int32 Sensibiliteacc
        {
            get; set;
        }

        #endregion

        #region Constructeur

        public GestionTempo()
        {
            String XValue;
            // Initialiser

            // Arrêt passif
            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/ActivationMiseEnVeille", "", "", XML_ATTRIBUTE.VALUE);
            if (XValue == "1")
            {
                this.ActivationMiseEnVeille = true;
            }
            else
            {
                this.ActivationMiseEnVeille = false;
            }

            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionArretPassifMT/Delai", "", "", XML_ATTRIBUTE.VALUE);
            if (XValue != null && XValue != "")
            {
                this.DelaiArretPassif = Tools.ConvertASCIIToInt32(XValue);
            }
            else
            {
                this.DelaiArretPassif = 0;
            }

            this.MinDelaiArretPassif = 0;
            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionArretPassifMT/Delai", "", "", XML_ATTRIBUTE.MIN);
            if (XValue != null && XValue != "")
            {
                this.MinDelaiArretPassif = Tools.ConvertASCIIToInt32(XValue);
            }
            if (this.MinDelaiArretPassif < 3)
            {
                this.MinDelaiArretPassif = 3;
            }

            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionArretPassifMT/Delai", "", "", XML_ATTRIBUTE.MAX);
            if (XValue != null && XValue != "")
            {
                this.MaxDelaiArretPassif = Tools.ConvertASCIIToInt32(XValue);
            }
            else
            {
                this.MaxDelaiArretPassif = 20;
            }
            // Mise en veille
            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/Delai", "", "", XML_ATTRIBUTE.VALUE);
            if (XValue != null && XValue != "")
            {
                this.DelaiMiseEnVeille = Tools.ConvertASCIIToInt32(XValue);
            }
            else
            {
                this.DelaiMiseEnVeille = 0;
            }

            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/Delai", "", "", XML_ATTRIBUTE.MIN);
            if (XValue != null && XValue != "")
            {
                this.MinDelaiMiseEnVeille = Tools.ConvertASCIIToInt32(XValue);
            }
            else
            {
                this.MinDelaiMiseEnVeille = 60;
            }

            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/Delai", "", "", XML_ATTRIBUTE.MAX);
            if (XValue != null && XValue != "")
            {
                this.MaxDelaiMiseEnVeille = Tools.ConvertASCIIToInt32(XValue);
            }
            else
            {
                this.MaxDelaiMiseEnVeille = 3600;
            }

            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionHommeMort/Reglage/DelaiAlarme", "", "", XML_ATTRIBUTE.VALUE);
            if (XValue != null && XValue != "")
            {
                this.DelaiAlarmeHommeMort = Tools.ConvertASCIIToInt32(XValue);
            }
            else
            {
                this.DelaiAlarmeHommeMort = 10;
            }

            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionHommeMort/Reglage/DelaiAlarme", "", "", XML_ATTRIBUTE.MIN);
            if (XValue != null && XValue != "")
            {
                this.MinDelaiAlarmeHommeMort = Tools.ConvertASCIIToInt32(XValue);
            }
            else
            {
                this.MinDelaiAlarmeHommeMort = 0;
            }

            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionHommeMort/Reglage/DelaiAlarme", "", "", XML_ATTRIBUTE.MAX);
            if (XValue != null && XValue != "")
            {
                this.MaxDelaiAlarmeHommeMort = Tools.ConvertASCIIToInt32(XValue);
            }
            else
            {
                this.MaxDelaiAlarmeHommeMort = 60;
            }

            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionHommeMort/Reglage/DureeAlarme", "", "", XML_ATTRIBUTE.VALUE);
            if (XValue != null && XValue != "")
            {
                this.DureeAlarmeHommeMort = Tools.ConvertASCIIToInt32(XValue);
            }
            else
            {
                this.DureeAlarmeHommeMort = 10;
            }

            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionHommeMort/Reglage/DureeAlarme", "", "", XML_ATTRIBUTE.MIN);
            if (XValue != null && XValue != "")
            {
                this.MinDureeAlarmeHommeMort = Tools.ConvertASCIIToInt32(XValue);
            }
            else
            {
                this.MinDureeAlarmeHommeMort = 5;
            }

            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionHommeMort/Reglage/DureeAlarme", "", "", XML_ATTRIBUTE.MAX);
            if (XValue != null && XValue != "")
            {
                this.MaxDureeAlarmeHommeMort = Tools.ConvertASCIIToInt32(XValue);
            }
            else
            {
                this.MaxDureeAlarmeHommeMort = 60;
            }

            // Homme mort
            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionHommeMort/Reglage/Actif", "", "", XML_ATTRIBUTE.VALUE);
            if (XValue != null && XValue != "")
            {
                if (XValue.Trim() == "1")
                {
                    this.ActivationHommeMort = true;
                }
                else
                {
                    this.ActivationHommeMort = false;
                    this.DelaiAlarmeHommeMort = 0;
                    this.DureeAlarmeHommeMort = 0;
                }
            }
            else
            {
                this.ActivationHommeMort = false;
            }

            // Masque homme mort
            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionHommeMort/RearmementAcq/MasqueAcquittement", "", "", XML_ATTRIBUTE.VALUE);
            if (XValue != null && XValue != "")
            {
                try
                {
                    if (!XValue.Contains("0x"))
                    {
                        this.MaskHM32 = Convert.ToUInt32(XValue);
                    }
                    else
                    {
                        this.MaskHM32 = Convert.ToUInt32(XValue.Substring(2), 16);
                    }
                }
                catch
                {
                    this.MaskHM32 = 0xFFFFFFFF;
                }
            }
            else
            {
                this.MaskHM32 = 0xFFFFFFFF;
            }

            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionHommeMort/RearmementAcq/MasqueAcquittement2", "", "", XML_ATTRIBUTE.VALUE);
            if (XValue != null && XValue != "")
            {
                try
                {
                    if (!XValue.Contains("0x"))
                    {
                        this.MaskHM16 = Convert.ToUInt16(XValue);
                    }
                    else
                    {
                        this.MaskHM16 = Convert.ToUInt16(XValue.Substring(2), 16);
                    }
                }
                catch
                {
                    this.MaskHM16 = 0xFFFF;
                }
            }
            else
            {
                this.MaskHM16 = 0xFFFF;
            }

            // Activation absence radio MT
            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/ActivationAbsRadioMT", "", "", XML_ATTRIBUTE.VALUE);
            if (XValue != null && XValue != "")
            {
                if (XValue.Trim() == "1")
                {
                    this.ActivationABSRadioMT = true;
                }
                else
                {
                    this.ActivationABSRadioMT = false;
                }
            }
            else
            {
                this.ActivationABSRadioMT = false;
            }

            // Delai absence radio MT
            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/DelaiAbsRadioMT", "", "", XML_ATTRIBUTE.VALUE);
            if (XValue != null && XValue != "")
            {
                try
                {
                    Int32 Value;

                    Value = Convert.ToInt32(XValue);
                    this.DelaiABSRadioMT = Value;
                }
                catch
                {
                    this.DelaiABSRadioMT = 0;
                }
            }
            else
            {
                this.DelaiABSRadioMT = 0;
            }

            // Delahm
            XValue = PegaseData.Instance.XMLFile.GetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ParametresAccelerometre/Seuil", "", "", XML_ATTRIBUTE.VALUE);
            if (XValue != null && XValue != "")
            {
                try
                {
                    Int32 Value;

                    Value = Convert.ToInt32(XValue);
                    this.Sensibiliteacc = Value;
                }
                catch
                {
                    this.Sensibiliteacc = 0;
                }
            }
            else
            {
                this.Sensibiliteacc = 0;
            }

        }

        #endregion

        #region Méthodes
        
        /// <summary>
        /// Sauvegarder les données dans le XML en cours d'utilisation
        /// </summary>
        public void Save ( )
        {
            PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionArretPassifMT/Delai", "", "", XML_ATTRIBUTE.VALUE, this.DelaiArretPassif.ToString());

            // Mise en veille
            PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/Delai", "", "", XML_ATTRIBUTE.VALUE, this.DelaiMiseEnVeille.ToString());
            if (this.ActivationMiseEnVeille)
            {
                PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/ActivationMiseEnVeille", "", "", XML_ATTRIBUTE.VALUE, "1");
            }
            else
            {
                PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/ActivationMiseEnVeille", "", "", XML_ATTRIBUTE.VALUE, "0");
            }

            // Homme mort
            if (this.ActivationHommeMort)
            {
                PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionHommeMort/Reglage/Actif", "", "", XML_ATTRIBUTE.VALUE, "1"); 
            }
            else
            {
                PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionHommeMort/Reglage/Actif", "", "", XML_ATTRIBUTE.VALUE, "0"); 
            }
            
            PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionHommeMort/Reglage/DelaiAlarme", "", "", XML_ATTRIBUTE.VALUE, this.DelaiAlarmeHommeMort.ToString());

            PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionHommeMort/Reglage/DureeAlarme", "", "", XML_ATTRIBUTE.VALUE, this.DureeAlarmeHommeMort.ToString());

            // Masque homme mort
            PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionHommeMort/RearmementAcq/MasqueAcquittement", "", "", XML_ATTRIBUTE.VALUE, this.MaskHM32.ToString());
            PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionHommeMort/RearmementAcq/MasqueAcquittement2", "", "", XML_ATTRIBUTE.VALUE, this.MaskHM16.ToString());

            // Abscence radio
            if (this.ActivationABSRadioMT)
            {
                PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/ActivationAbsRadioMT", "", "", XML_ATTRIBUTE.VALUE, "1");
            }
            else
            {
                PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/ActivationAbsRadioMT", "", "", XML_ATTRIBUTE.VALUE, "0");
            }
            PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/GestionMiseEnVeilleMO/DelaiAbsRadioMT", "", "", XML_ATTRIBUTE.VALUE, this.DelaiABSRadioMT.ToString());
            PegaseData.Instance.XMLFile.SetValue("XmlTechnique/ParametresApplicatifs/ParametresModifiables/ParametresAccelerometre/Seuil", "", "", XML_ATTRIBUTE.VALUE,this.Sensibiliteacc.ToString());
        } // endMethod: Save

        #endregion
    }
}
