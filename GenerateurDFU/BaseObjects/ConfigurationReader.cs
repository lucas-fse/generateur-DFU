using System.Configuration;
using System.Collections.Specialized;
using System;
using JAY;

namespace JAY.PegaseCore.Helper
{
    /// <summary>
    /// ConfigurationReader class
    /// </summary>
    public class ConfigurationReader
    {
        ///// <summary>
        ///// list of applicative parameters
        ///// </summary>
        //private IList<Parametre> parametres;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationReader"/> class.
        /// </summary>
        private ConfigurationReader()
        {
            ////YVA pas utilisé à priori le fait de stocker certains param en base
            ////DaoParametre daoParametre = new DaoParametre();
            ////parametres = daoParametre.rechercheListeObjets();

            // 
            String ConfigFileName = DefaultValues.Get().ConfigFile;
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", ConfigFileName);
        }

        /// <summary>
        /// the threadLock
        /// </summary>
        private static object threadLock = new object();
        private static object threadLock2 = new object();

        /// <summary>
        /// the instance
        /// </summary>
        private static ConfigurationReader instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ConfigurationReader Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (threadLock)
                    {
                        instance = new ConfigurationReader();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <returns>value of parameters or raise an exception</returns>
        public string GetValue(string keyName)
        {
            String Result = keyName;
            lock (threadLock2)
            {
                using (JAY.AppConfig.Change(DefaultValues.Get().ConfigFile))
                {

                    NameValueCollection nvc = (NameValueCollection)ConfigurationManager.GetSection("PEGASE");

                    if (nvc != null)
                    {
                        if (!string.IsNullOrEmpty(nvc[keyName]))
                        {
                            Result = nvc[keyName].ToString();
                        }
                    }
                }
            }
            return Result;
        }
    }
}
