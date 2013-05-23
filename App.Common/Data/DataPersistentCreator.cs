using Castle.MicroKernel.Registration;
using Castle.Windsor;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Bytecode;
using NHibernate.Tool.hbm2ddl;
using SharpLite.Domain;
using SharpLite.NHibernateProvider;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using App.Common.Data.Conventions;
using App.Common.Data.Drivers;
using App.Common.Extensions;
using App.Common.Modulary;
using App.Common.Utilities;

namespace App.Common.Data
{
    public static class DataPersistentCreator
    {
        private static ConcurrentBag<IModule> _loadedModules = new ConcurrentBag<IModule>();

        private static IWindsorContainer _container { get; set; }

        public static void Startup(IWindsorContainer container)
        {
            LoadModules();

            InitializeModule(container, _loadedModules.ToArray());

            RegisterModuleData(container, _loadedModules.ToArray());
        }

        private static void InitializeModule(IWindsorContainer container, IModule[] module)
        {
            _loadedModules.Each(m => m.Initialize(_container));
        }

        private static void LoadModules()
        {
            var moduleHolder = new ModuleHolder();
            PluginLoader.LoadInBinDirectory(moduleHolder);

            foreach (var module in moduleHolder.Modules)
            {
                _loadedModules.Add(module);
            }
        }


        private static void RegisterModuleData(IWindsorContainer container, IModule[] modules)
        {
            var connectionString = container.Resolve<IConnectionString>();

            var allDomainAssemblies = new List<Assembly>();
            //allDomainAssemblies.Add(typeof(Tenant).Assembly);
            modules.Each(m => allDomainAssemblies.AddRange(m.DomainAssemblies));

            var allDataAssemblies = new List<Assembly>();
            //allDataAssemblies.Add(typeof(TenantUserMappingOverride).Assembly);
            modules.Each(m => allDataAssemblies.AddRange(m.DataAssemblies));

            container.Register(
                Component.For<ISessionFactory>().UsingFactoryMethod(() =>
                    BuildMasterSessionFactory(
                        connectionString.Value,
                        allDomainAssemblies.ToArray(),
                        allDataAssemblies.ToArray())));

            //var tenantInfoProvider = container.Resolve<ITenantInfoProvider>();
            //var tenants = tenantInfoProvider.GetTenants();
            //tenants.Each(t =>
            //{
            //    container.Register(
            //        Component.For<ISessionFactory>().Named(t.TenantID.ToString()).UsingFactoryMethod(() =>
            //            BuildTenantSessionFactory(
            //                connectionString.Value,
            //                allDomainAssemblies.ToArray(),
            //                allDataAssemblies.ToArray()))
            //        );
            //});
        }

        private static ISessionFactory BuildMasterSessionFactory(string connectionString, Assembly[] assemblies, params Assembly[] overrides)
        {
            return InternalBuildSessionFactory(
                connectionString,
                new FluentNHibernate.Conventions.IConvention[] 
                    { 
                        new PrimaryKeyConvention(), 
                        new ForeignKeyConvention(),
                        new HasManyConvention(),
                        new UserTypeConvention()
                    },
                assemblies,
                overrides);
        }

        private static ISessionFactory InternalBuildSessionFactory(string connectionString, FluentNHibernate.Conventions.IConvention[] conventions, Assembly[] assemblies, params Assembly[] overrides)
        {
            var persistenceModel = AutoMap.Assemblies(assemblies)
                        .Conventions.Add(conventions)
                        .IgnoreBase<Entity>()
                        .Where(t => typeof(EntityWithTypedId<Guid>).IsAssignableFrom(t));

            if (overrides != null)
            {
                overrides.Each(a => persistenceModel.UseOverridesFromAssembly(a));
            }

            var config = Fluently.Configure()
                            .ProxyFactoryFactory<DefaultProxyFactoryFactory>()
                            .Database(
                                MsSqlConfiguration.MsSql2008
                                    .ConnectionString(connectionString)
                                    .Provider<ContextDriverConnectionProvider>())
                            .CurrentSessionContext<LazySessionContext>()
                            .Mappings(m => m.AutoMappings.Add(persistenceModel))
                            .BuildConfiguration();
#if DEBUG
            persistenceModel.WriteMappingsTo(AppDomain.CurrentDomain.BaseDirectory + @"Hbm");
            using (ISession session = config.BuildSessionFactory().OpenSession())
            {
                string syntax = AppDomain.CurrentDomain.BaseDirectory + @"Hbm\{0}.sql";
                string filename = string.Format(syntax, session.Connection.Database);

                using (TextWriter stringWriter = new StreamWriter(filename))
                {
                    new SchemaExport(config).Execute(false, false, false, session.Connection, stringWriter);
                }
            }
#endif
            return config.BuildSessionFactory();
        }

    }
}
