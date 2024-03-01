using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JAY
{
    public class Constantes
    {
        // Int32

        // Nombre maximum de modes
        public const Int32 MAX_MODE = 32;
        public const Int32 MAX_SELECTEUR = 6;

        public const Int32 MODE_SECU = 33;
        public const Int32 MODE_UNIVERSELLE = 32;

        // Regex

        // Format numérique sur 4 digites comme les codes Pins par exemple
        public const String FORMAT_PIN = "[0-9]{4}";

        // Rapport
        public const String LOOP_START = "LOOP_";
        public const String LOOP_END = "ENDLOOP_";

        // clé du fichier app.config
        public const String REGEX_REFERP_MO = "RefIndusMoRule";
        public const String REGEX_REFERP_MT = "RefIndusMtRule";
        public const String REGEX_REFERP_SIM = "OptionsLogiciellesRule";

        // Autres constantes
        public const String PREFIX_LIBEL_WHC = "FIXE_WHC";
        public const String DIRECT_TO_BMP = "Direct_To_.BMP";

        public const Int32 RefYear = 1970;
        public const Int32 RefMounth = 1;
        public const Int32 RefDay = 1;

        public const String PluginName_Default = "Plugin";
        public const String PluginName_Mapping = "Mapping";

        public const String XML_ENCODING = "iso-8859-1";

        public const String LANG_FR = "Francais";
        public const String LANG_EN = "Anglais";
        public const String LANG_IT = "Italien";
        public const String LANG_GE = "Allemand";
    }
}
