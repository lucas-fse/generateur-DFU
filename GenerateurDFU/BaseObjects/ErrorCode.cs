using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Les codes d'erreurs
    /// </summary>
    public static class ErrorCode
    {
        public const UInt32 NO_ERROR                = 0x00000000;
        public const UInt32 UNKNOWN_ERROR           = 0xFFFFFFFF;
        public const UInt32 IDIALOG_INTEGRITY_ERROR = 0x00000001;
        public const UInt32 IDIALOG_XML_ERROR       = 0x00000002;
    }
}
