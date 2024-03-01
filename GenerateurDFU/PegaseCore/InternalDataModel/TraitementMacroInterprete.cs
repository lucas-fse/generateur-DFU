using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAY.PegaseCore.InternalDataModel
{
    public class ligneDeConfigPLD : IEquatable<ligneDeConfigPLD>
    {
        public UInt32 Modes;
        public UInt32 Buttons;
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            ligneDeConfigPLD objAsPart = obj as ligneDeConfigPLD;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }
        
        public bool Equals(ligneDeConfigPLD other)
        {
            if (other == null) return false;
            return (this.Modes.Equals(other.Modes) && this.Buttons.Equals(other.Buttons));
        }
    }
    public  class TraitementMacroInterprete
    {
        public  List<ligneDeConfigPLD> pldConfig = null;

        public  bool   TraitementMacro(MacroFonction proto, Dictionary<int, string> parametreMacro)
        {
            bool correct = false;
           if (proto.Name.Equals("PLDInputActivated"))
            {
                if (pldConfig== null)
                {
                    pldConfig = new List<ligneDeConfigPLD>();
                }
                ligneDeConfigPLD tmp = PLDInputActivated(proto, parametreMacro);
                if (tmp!=null)
                {
                    if ((pldConfig.Count < 48) && (!pldConfig.Contains(tmp)))
                    {
                        correct = AnalyseMaskPldByMode(tmp);
                        if(correct)
                        {
                            pldConfig.Add(tmp);
                        }
                    }
                    else
                    {
                        correct = false;
                    }
                }
                else
                {
                    correct = false;
                }
            }
            return correct;
        }
        public  void  TraitementMacroClear()
        {
            pldConfig = new List<ligneDeConfigPLD>();
        }
        /// <summary>
        /// met en forme les masque avant analyse 
        /// </summary>
        /// <param name="proto"></param>
        /// <param name="parametreMacro"></param>
        /// <returns></returns>
        private  ligneDeConfigPLD PLDInputActivated(MacroFonction proto, Dictionary<int, string> parametreMacro)
        {
            // 
            string tmp;
            parametreMacro.TryGetValue(0, out tmp);
            if (tmp == null) { tmp = ""; }
            string[] boutons = tmp.Split(',');
            parametreMacro.TryGetValue(1, out tmp);
            if (tmp == null) { tmp = ""; }
            string[] modes = tmp.Split(',');
            bool erreur = false;
            int organDoubleSet = 0;
            Dictionary<OrganCommand, int> DicoMasque = new Dictionary<OrganCommand, int>();
            UInt32 mask = 0;
            //
            for (int i = 0; i < boutons.Count(); i++)
            {
                if (i + 1 < boutons.Count())
                {
                    for (int y = i + 1; y < boutons.Count(); y++)
                    {
                        if (boutons[i].Trim() == boutons[y].Trim())
                        {
                            erreur = true;
                        }
                    }
                }
            }
            //PegaseData.Instance.MOperateur.OrganesCommandes.
            // calcul du masque boutons
            foreach (var bp in boutons)                
            {
                string bptrim = bp.Trim(' ');
                int result = 0;
                if (bptrim.Equals("BOOLEEN_SYSTEME_14"))
                {
                    result = 0;
                    mask = mask | (UInt32)(Math.Pow(2, result));
                }
                else 
                {
                    var query = from organe in PegaseData.Instance.MOperateur.OrganesCommandes
                                where organe.Mnemologique == bptrim && (organe.TypePld.Equals(OrganCommand.TYPE_PLD.DoubleSafety) || organe.TypePld.Equals(OrganCommand.TYPE_PLD.SingleSafety))
                                select organe;
                    if (query != null && query.Count() > 0)
                    {
                        OrganCommand organ = query.First();
                        if (organ.Mnemologique.Equals("AXE_A_CRAN_1") || organ.Mnemologique.Equals("AXE_ANA_1"))
                        {
                            result = 20;
                        }   
                        else if (organ.Mnemologique.Equals("AXE_A_CRAN_2") || organ.Mnemologique.Equals("AXE_ANA_2"))
                        {
                            result = 21;
                        }
                        else if (organ.Mnemologique.Equals("AXE_A_CRAN_3") || organ.Mnemologique.Equals("AXE_ANA_3"))
                        {
                            result = 22;
                        }
                        else if (organ.Mnemologique.Equals("AXE_A_CRAN_4") || organ.Mnemologique.Equals("AXE_ANA_4"))
                        {
                            result = 23;
                        }
                        else if (organ.Mnemologique.Equals("AXE_A_CRAN_5") || organ.Mnemologique.Equals("AXE_ANA_5"))
                        {
                            result = 24;
                        }
                        else if (organ.Mnemologique.Equals("AXE_A_CRAN_6") || organ.Mnemologique.Equals("AXE_ANA_6"))
                        {
                            result = 25;
                        }
                        else if (organ.Mnemologique.Equals("COMMUTATEUR_11"))
                        {
                            result = 26;
                        }
                        else if (organ.Mnemologique.Equals("COMMUTATEUR_12"))
                        {
                            result = 27;
                        }
                        else
                        {                        
                            result = organ.IndiceOrganeMO;
                        }
                        if (PegaseData.Instance.MOperateur.PresenceSafetyButton != 0 && result == 17)
                        {
                            mask += 0x80000;// aux3 couple a l'aux1 pour le bouton safety
                            organDoubleSet = 2;
                        }
                        if (organ.TypePld == OrganCommand.TYPE_PLD.DoubleSafety)
                        {
                            organDoubleSet++; // il faut au mini 2 organs 
                        }
                        else if (organ.TypePld == OrganCommand.TYPE_PLD.SingleSafety)
                        {
                            organDoubleSet = 2; // sauf si organs peut etre seul
                        }
                        mask = mask | (UInt32)(Math.Pow(2, result));
                    }
                    else
                    {
                        erreur = true;
                    }
                }

            }
            if (organDoubleSet < 2) { erreur = true; }
            // calcul du masque de modes
            UInt32 maskModes = 0;
            foreach (var mode in modes)
            {
                int mode_int = 0;
                try
                {
                    mode_int = Convert.ToInt32(mode)-1;
                }
                catch 
                {
                    erreur = true;
                }
                maskModes = maskModes | (UInt32)(Math.Pow(2, mode_int));
            }
            ligneDeConfigPLD ligne = new ligneDeConfigPLD();
            ligne.Buttons = mask;
            ligne.Modes = maskModes;
            if (erreur)
            {
                return null;
            }
            else
            {
                return ligne;
            }
            //M32[0] = LS;
            //M32[1] = this.EtatBtnMarche;
            //M32[2] = this.EtatBtnNav1;
            //M32[3] = this.EtatBtnNav2;

            //M32[4] = this.F1;
            //M32[5] = this.F2;
            //M32[6] = this.EtatBtn03;
            //M32[7] = this.EtatBtn04;

            //M32[8] = this.EtatBtn05;
            //M32[9] = this.EtatBtn06;
            //M32[10] = this.EtatBtn07;
            //M32[11] = this.EtatBtn08;

            //M32[12] = this.EtatBtn09;
            //M32[13] = this.EtatBtn10;
            //M32[14] = this.EtatBtnJoystick01;
            //M32[15] = this.EtatBtnJoystick02;

            //M32[16] = this.EtatBtnJoystick03;
            //M32[17] = this.EtatBtnAuxiliaire01;
            //M32[18] = this.EtatBtnAuxiliaire02;
            //M32[19] = this.EtatBtnAuxiliaire03;
            //M32[20] = JS1
            //M32[21] = JS1
            //M32[22] = JS2
            //M32[23] = JS2
            //M32[24] = JS3
            //M32[25] = JS3
            //M32[26] = A11
            //M32[27] = A12
        }
        private bool AnalyseMaskPldByMode(ligneDeConfigPLD newpld)
        {
            bool result = true;
           
            UInt32 MaskModeEnCours = 1;
            for (int i=1;i<32;i++)
            {
                // pour chaque mode
                bool AnticripationPresent = false;
                
                foreach (var config in pldConfig)
                {
                    // on verifie que la ligne est utilisé dans ce mode
                    if ((config.Modes & MaskModeEnCours)!=0)
                    {
                        if ((newpld.Modes & MaskModeEnCours) != 0)
                        {
                            if ((((config.Buttons & newpld.Buttons) == config.Buttons) ||
                                ((config.Buttons & newpld.Buttons) == newpld.Buttons)))
                            {
                                result &= false;
                            }
                        }
                    }
                }

                MaskModeEnCours *= 2;
            }
            return result;
        }
    }


  
}
