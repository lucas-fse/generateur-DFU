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


    public class ComplementaryData : Mvvm.ViewModelBase
    {
        #region Variables
        private string _modename;
        private ObservableCollection<EC_MaskMode> _verrouillagesBtn;
        #endregion

        #region Propriétés
        /// <summary>
        /// Le nom du mode
        /// </summary>
        public String ModeNameII
        {
            get
            {
                String Result = LanguageSupport.Get().GetText("EASYCONF/MASKCHGMODE");
                return Result;
            }
            set
            {
                this._modename = value;
                RaisePropertyChanged("ModeName");
            }
        } // endProperty: ModeName

        /// <summary>
        /// La liste des verrouillages boutons pour le mode
        /// </summary>
        public ObservableCollection<EC_MaskMode> VerrouillageBtnII

        {
            get
            {
                return this._verrouillagesBtn;
            }
            set
            {
                this._verrouillagesBtn = value;
                RaisePropertyChanged("VerrouillageBtn");
            }
        } // endProperty: VerrouillageBtn
        public ComplementaryData()
        {
            this.VerrouillageBtnII = new ObservableCollection<EC_MaskMode>();
           
            EC_MaskMode item = new EC_MaskMode();
            if (this.VerrouillageBtnII.Count == 0)
            {
                this.VerrouillageBtnII.Add(item);
            }
        }

        public void SerialiseMaskM()
        {
            // sauvegarde du masque de changement de mode
            if (JAY.PegaseCore.EasyConfigData.Get().MaskModes.Count() != 0)
            {
                JAY.PegaseCore.EasyConfigData.Get().MaskModes[0].VerrouillageBtnII[0].SaveMask();
            }
        }
        #endregion
        


    }
}
