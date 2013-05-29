using App.Common.Web;
using App.Common.Web.Mvc;
using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Startup.Tasks
{
    public class ControllerRegistrationTask : IBootstrapperTask
    {
        public void Execute(Castle.Windsor.IWindsorContainer container)
        {
            container.Register(
                Types.FromAssemblyInDirectory(new AssemblyFilter(HttpRuntime.BinDirectory, "Web.*"))
                .BasedOn<Controller>()
                .WithServiceAllInterfaces()
                .WithServiceSelf()
                .LifestyleTransient()
                .LifestylePerWebRequest());

            DependencyResolver.SetResolver(new WindsorDependencyResolver(container));
        }
    }
}