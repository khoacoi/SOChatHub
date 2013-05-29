using App.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Web.Startup.Tasks
{
    public class SignalRRegistrationTask : IBootstrapperTask
    {
        public void Execute(Castle.Windsor.IWindsorContainer container)
        {
            RouteTable.Routes.MapHubs();
        }
    }
}