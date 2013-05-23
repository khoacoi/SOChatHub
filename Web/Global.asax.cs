using App.Common.Web;
using App.Core;
using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.Startup;
using App.Common.Extensions;

namespace Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private IWindsorContainer _container;

        protected void Application_Start()
        {
            _container = new WindsorContainer();
            _container.AddFacility<TypedFactoryFacility>();

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(_container));

            ApplicationStarter.Run(_container, InitializeAction);
            //AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            //AuthConfig.RegisterAuth();
        }

        private void InitializeAction(IWindsorContainer container)
        {
            var tasks = new IBootstrapperTask[] {
                //new Log4NetRegistrationTask(),
                //new RouteRegistrationTask(),
                new FilterRegistrationTask(),
                //new BundleRegistrationTask(),
                //new ControllerRegistrationTask(),
                new MvcOverridesRegistrationTask()
                //new FluentValidationRegistrationTask()
            };

            tasks.Each(t => t.Execute(container));
        }
    }
}