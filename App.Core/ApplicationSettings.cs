using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core
{
    public class ApplicationSettings
    {
         private static object _locker = new object();
        private static ApplicationSettings _instance;

        /// <summary>
        /// Gets the instance of Choice settings.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static ApplicationSettings Instance
        {
            get 
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new ApplicationSettings();
                            _instance.LoadConfigs();
                        }
                    }
                }
                return _instance;
            }
        }

        private ApplicationSettings() { }

        
        /// <summary>
        /// Get The Encrypt Key for emcription
        /// </summary>
        public string EncrypPrivateKey { get; private set; }

        public string EncryptPublicKey { get; private set; }

        public int CookieTimeout { get; private set; }

        /// <summary>
        /// Loads the configs.
        /// </summary>
        private void LoadConfigs()
        {
            EncrypPrivateKey = ConfigurationManager.AppSettings["EncrypPrivateKey"];
            EncryptPublicKey = ConfigurationManager.AppSettings["EncrypPublicKey"];
            CookieTimeout = int.Parse(ConfigurationManager.AppSettings["CookieTimeout"]);
        }

        
    }
}
