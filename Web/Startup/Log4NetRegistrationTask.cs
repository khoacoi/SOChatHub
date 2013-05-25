using App.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Startup
{
    public class Log4NetRegistrationTask : IBootstrapperTask
    {
        public void Execute(Castle.Windsor.IWindsorContainer container)
        {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}