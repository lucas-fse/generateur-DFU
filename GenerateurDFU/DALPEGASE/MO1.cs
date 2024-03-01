using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALPEGASE
{
    public partial class ORDRE_FABRICATION
    {
        public void copy(ORDRE_FABRICATION of)
        {
            this.NUM_OF = of.NUM_OF;
            this.NUM_COMMANDE_CLIENT = of.NUM_COMMANDE_CLIENT;
            this.REF_INDUSTRIELLE_MO = of.REF_COMMERCIALE_MO;
            this.REF_COMMERCIALE_MO = of.REF_COMMERCIALE_MO;
            this.REF_INDUSTRIELLE_MT = of.REF_INDUSTRIELLE_MT;
            this.REF_COMMERCIALE_MT = of.REF_COMMERCIALE_MT;
            this.REF_COMMERCIALE_SIM = of.REF_COMMERCIALE_SIM;
            this.REF_FIRMWARE_MO = of.REF_FIRMWARE_MO;
            this.REF_FIRMWARE_MT = of.REF_FIRMWARE_MT;
            this.REF_COMMERCIALE_PACK = of.REF_COMMERCIALE_PACK;
            this.OPTIONS_LOGICIELLES = of.OPTIONS_LOGICIELLES;
            this.REF_FICHE_PERSO = of.REF_FICHE_PERSO;
            this.NUM_SERIE_PACK = of.NUM_SERIE_PACK;
            this.NB_PACK = of.NB_PACK;
            this.NB_MO = of.NB_MO;
            this.NB_MT = of.NB_MT;
            this.NB_SIM = of.NB_SIM;
            this.DATE_PROGRAMMATION = of.DATE_PROGRAMMATION;
            this.OPTION_MATERIEL_MO = of.OPTION_MATERIEL_MO;
            this.OPTION_MATERIEL_MT= of.OPTION_MATERIEL_MT;
            this.COMMANDE_SYNCHRO = of.COMMANDE_SYNCHRO;
        }
    }
    public partial class MO
    {
        public void copy(MO cc)
        {
          //  this.ID_PACK_INSTALLE = cc.ID_PACK_INSTALLE;
            this.CODE_ARTICLE_LOGICIEL = cc.CODE_ARTICLE_LOGICIEL;
            this.CODE_IDENTITE_APPRENTISSAGE = cc.CODE_IDENTITE_APPRENTISSAGE;
            this.CODE_IDENTITE_NATIF = cc.CODE_IDENTITE_NATIF;
            this.DATE_DERNIERE_MAJ = DateTime.Now;
            this.DATE_FABRICATION = cc.DATE_FABRICATION;
            this.NUM_ORDRE = cc.NUM_ORDRE;
            this.NUM_SERIE_CARTE_TEST = cc.NUM_SERIE_CARTE_TEST;
            this.NUM_SERIE_MO = cc.NUM_SERIE_MO;
            this.PACK_INSTALLE = cc.PACK_INSTALLE;
            this.REF_INDUSTRIELLE = cc.REF_INDUSTRIELLE;
            this.VERSION_LOGICIELLE = cc.VERSION_LOGICIELLE;
            
        }
    }
    public partial class MT
    {
        public void copy(MT cc)
        {
           // this.ID_PACK_INSTALLE = cc.ID_PACK_INSTALLE;
            this.CODE_ARTICLE_LOGICIEL = cc.CODE_ARTICLE_LOGICIEL;
            this.CODE_IDENTITE_APPRENTISSAGE = cc.CODE_IDENTITE_APPRENTISSAGE;
            this.CODE_IDENTITE_NATIF = cc.CODE_IDENTITE_NATIF;
            this.DATE_DERNIERE_MAJ = DateTime.Now;
            this.DATE_FABRICATION = cc.DATE_FABRICATION;
            this.NUM_ORDRE = cc.NUM_ORDRE;
            this.NUM_SERIE_CARTE_TEST = cc.NUM_SERIE_CARTE_TEST;
            this.NUM_SERIE_MT = cc.NUM_SERIE_MT;
            this.PACK_INSTALLE = cc.PACK_INSTALLE;
            this.REF_INDUSTRIELLE = cc.REF_INDUSTRIELLE;
            this.VERSION_LOGICIELLE = cc.VERSION_LOGICIELLE;

        }
    }
    public partial class SIM
    {
        public void copy(SIM cc)
        {
           // this.ID_PACK_INSTALLE = cc.ID_PACK_INSTALLE;
            this.DATE_FABRICATION = cc.DATE_FABRICATION;
            this.ID_PACK_INSTALLE = cc.ID_PACK_INSTALLE;
            this.NUM_ORDRE = cc.NUM_ORDRE;
            this.NUM_SERIE_CARTE_TEST = cc.NUM_SERIE_CARTE_TEST;
            this.NUM_SERIE_SIM = cc.NUM_SERIE_SIM;
            this.PACK_INSTALLE = cc.PACK_INSTALLE;
            this.REF_INDUSTRIELLE = cc.REF_INDUSTRIELLE;
        }
    }
}
