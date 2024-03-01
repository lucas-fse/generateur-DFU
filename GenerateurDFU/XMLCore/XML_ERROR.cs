using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JAY.XMLCore
{
    /// <summary>
    /// Code d'erreurs pour la partie IDialog_XML
    /// </summary>
    public static class XML_ERROR
    {
        public const Int32 NO_ERROR = 0x0000;
        public const Int32 ERROR_UNDESCRIBED = 0xFFFF;
        public const Int32 ERROR_FILE_NOT_FOUND = 0x0001;
        public const Int32 ERROR_XML_NODE_NOT_FOUND = 0x0002;
        public const Int32 ERROR_XML_INTEGRITY = 0x0003;
        public const Int32 ERROR_XML_TOO_OLD = 0x0004;
        public const Int32 ERROR_FILE_NOT_EXIST = 0x0005;
    }
}
