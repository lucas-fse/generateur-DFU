using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JAY.PegaseCore.Helper
{
    public class TimeHelper
    {
        
        /// <summary>
        /// Attendre le nombre de millisecondes spécifiées
        /// </summary>
        public static void Wait ( Int32 milliseconds )
        {
            TimeSpan ts = new TimeSpan();
            DateTime start = DateTime.Now;

            while (ts.TotalMilliseconds < milliseconds)
            {
                ts = DateTime.Now - start;
            }
        } // endMethod: Wait

    }
}
