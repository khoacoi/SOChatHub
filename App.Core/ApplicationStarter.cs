using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Facilities.TypedFactory;
using SharpLite.Domain.DataInterfaces;
using SharpLite.NHibernateProvider;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Common.Commands;
using App.Common.Data;
using System.Web;
using Castle.MicroKernel.Handlers;
using Castle.MicroKernel.Context;
using Castle.MicroKernel;
using System.Collections;
using NHibernate;

namespace App.Core
{
    public static class ApplicationStarter
    {
        /// <summary>
        /// Executes the start-up and register necessary component to the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="registerMvc">The MVC component registrations.</param>
        public static void Run(IWindsorContainer container, Action<IWindsorContainer> registerMvc)
        {
            RegisterCore(container);

            registerMvc(container);

            DataPersistentCreator.Startup(container);
            //TenantHost.Startup(container);
        }

        /// <summary>
        /// Registers core components to container
        /// </summary>
        /// <param name="container">The container.</param>
        private static void RegisterCore(IWindsorContainer container)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            container.Register(
                //Component.For<ITenantContextResolver>().ImplementedBy<ChoiceTenantContextResolver>().LifestyleSingleton(),
                //Component.For<ITenantContext>().UsingFactoryMethod((k, ctx) =>
                //{
                //    var resolver = k.Resolve<ITenantContextResolver>();
                //    return resolver.GetTenantContext();
                //}).LifestyleTransient(),

                //Component.For<ITenantInfoProvider>().ImplementedBy<ChoiceTenantInfoProvider>().LifestyleTransient(),

                //Component.For<IThemeProvider>().ImplementedBy<ChoiceThemeProvider>().LifestyleSingleton(),
                //Component.For<WorkingContext>().LifestylePerWebRequest(),
                Component.For<IConnectionString>().Instance(new ConnectionString(connectionString)).LifestyleSingleton(),
                Component.For(typeof(IRepository<>)).ImplementedBy(typeof(Repository<>)).DynamicParameters(BindSessionFactoryParameter).LifestyleTransient(),
                Component.For(typeof(IRepositoryWithTypedId<,>)).ImplementedBy(typeof(RepositoryWithTypedId<,>)).DynamicParameters(BindSessionFactoryParameter).LifestyleTransient(),
                Component.For<ICommandProcessor>().ImplementedBy<CommandProcessor>().LifestylePerWebRequest(),
                Component.For<IQueryFactory>().AsFactory().LifestylePerWebRequest(),
                Component.For<IRepositoryFactory>().AsFactory().LifestylePerWebRequest()
                //Component.For<ICacheManager>().ImplementedBy<CacheManager>().LifestylePerWebRequest(),
                //Component.For<ILocalizer>().ImplementedBy<ResxLocalizer>().LifestyleSingleton(),
                );


            container.Register(
                Component.For<ICommandHandlerFactory>().AsFactory().LifestylePerWebRequest(),
                //Register all IQuery instants.
                Classes.FromAssemblyInDirectory(new AssemblyFilter(HttpRuntime.BinDirectory))
                    .BasedOn<App.Common.Data.IQuery>()
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient(),
                //Register all ICommandHandler<,>
                Classes.FromAssemblyInDirectory(new AssemblyFilter(HttpRuntime.BinDirectory, "App.Domain.*"))
                    .BasedOn(typeof(ICommandHandler<,>))
                    .LifestylePerWebRequest()
                    .WithService.FirstInterface(),
                //Register all ICommandHandler<>
                Classes.FromAssemblyInDirectory(new AssemblyFilter(HttpRuntime.BinDirectory, "App.Domain.*"))
                    .BasedOn(typeof(ICommandHandler<>))
                    .LifestylePerWebRequest()
                    .WithService.FirstInterface()
                );
        }

        private static ComponentReleasingDelegate BindSessionFactoryParameter(IKernel kernel, CreationContext creationContext, IDictionary parameters)
        {
            parameters["sessionFactory"] = kernel.Resolve<ISessionFactory>();
            return r => { };
        }
    }
}
