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
        /// Gets the choice web service.
        /// </summary>
        /// <value>
        /// The choice web service.
        /// </value>
        public string ChoiceWebService { get; private set; }

        /// <summary>
        /// Gets the file server.
        /// </summary>
        /// <value>
        /// The file server.
        /// </value>
        public string FileServer { get; private set; }

        /// <summary>
        /// Gets the temp file server.
        /// </summary>
        /// <value>
        /// The temp file server.
        /// </value>
        public string TempFileServer { get; private set; }

        /// <summary>
        /// Gets the HTTP file server.
        /// </summary>
        /// <value>
        /// The HTTP file server.
        /// </value>
        public string HttpFileServer { get; private set; }

        /// <summary>
        /// Gets the temp HTML file server.
        /// </summary>
        /// <value>
        /// The temp HTML file server.
        /// </value>
        public string TempHtmlFileServer { get; private set; }

        /// <summary>
        /// Gets the default width of the dialog.
        /// </summary>
        /// <value>
        /// The default width of the dialog.
        /// </value>
        public int DefaultDialogWidth { get; private set; }

        /// <summary>
        /// Gets the default height of the dialog.
        /// </summary>
        /// <value>
        /// The default height of the dialog.
        /// </value>
        public int DefaultDialogHeight { get; private set; }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public string PageSize { get; private set; }

        /// <summary>
        /// Gets the admin email.
        /// </summary>
        /// <value>
        /// The admin email.
        /// </value>
        public string AdminEmail { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance can log audit activity.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can log audit activity; otherwise, <c>false</c>.
        /// </value>
        public bool CanLogAuditActivity { get; private set; }

        /// <summary>
        /// Loads the configs.
        /// </summary>
        private void LoadConfigs()
        {
            ChoiceWebService = ConfigurationManager.AppSettings["ChoiceWS"];
            FileServer = ConfigurationManager.AppSettings["FileServer"];
            TempFileServer = ConfigurationManager.AppSettings["TempFileServer"];
            HttpFileServer = ConfigurationManager.AppSettings["HttpFileServer"];
            TempHtmlFileServer = ConfigurationManager.AppSettings["HttpTempFileServer"];
            DefaultDialogWidth = int.Parse(ConfigurationManager.AppSettings["DialogWidth"]);
            DefaultDialogHeight = int.Parse(ConfigurationManager.AppSettings["DialogHeight"]);
            PageSize = ConfigurationManager.AppSettings["PageSizes"];
            AdminEmail = ConfigurationManager.AppSettings["MailAdmin"];
            CanLogAuditActivity = bool.Parse(ConfigurationManager.AppSettings["CanLogAuditActivity"]);
        }
    }
}
