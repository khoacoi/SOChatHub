using App.Common.Web;
using App.Common.Web.Mvc;
using Castle.MicroKernel.Registration;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web.Startup.Tasks
{
    public class SignalRRegistrationTask : IBootstrapperTask
    {
        public void Execute(Castle.Windsor.IWindsorContainer container)
        {
            RouteTable.Routes.MapHubs();

            //container.Register(
            //    Types.FromAssemblyInDirectory(new AssemblyFilter(HttpRuntime.BinDirectory, "Web.*"))
            //    .BasedOn<Hub>()
            //    .WithServiceAllInterfaces()
            //    .WithServiceSelf()
            //    .LifestyleTransient()
            //    .LifestylePerWebRequest());

            //DependencyResolver.SetResolver(new WindsorDependencyResolver(container));
        }
    }
}