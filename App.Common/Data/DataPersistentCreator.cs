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
using NHibernate.Caches.SysCache2;
using FluentNHibernate.Conventions.Helpers;
//using FluentNHibernate.Conventions;

namespace App.Common.Data
{
    public static class DataPersistentCreator
    {
        private static ConcurrentBag<IModule> _loadedModules = new ConcurrentBag<IModule>();

        private static IWindsorContainer _container { get; set; }

        public static void Startup(IWindsorContainer container)
        {
            _container = container;

            LoadModules();

            InitializeModule();

            BuildUpMasterShell();

            BindSessionFactoryToContext();
        }

        private static void BuildUpMasterShell()
        {
            RegisterSessionFactory();
        }

        private static void BindSessionFactoryToContext()
        {
            var sessionFactory = _container.Resolve<ISessionFactory>();
            LazySessionContext.Bind(new Lazy<ISession>(() => BeginSession(sessionFactory)), sessionFactory);
        }

        static ISession BeginSession(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            session.BeginTransaction();
            return session;
        }

        private static void InitializeModule()
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

        private static void RegisterSessionFactory()
        {
            var connectionString = _container.Resolve<IConnectionString>();

            var allDomainAssemblies = new List<Assembly>();
            var allDataAssemblies = new List<Assembly>();
            _loadedModules.Each(m =>
                {
                    allDomainAssemblies.AddRange(m.DomainAssemblies);
                    allDataAssemblies.AddRange(m.DataAssemblies);
                });

            FluentNHibernate.Conventions.IConvention[] conventions = null;
            conventions = new FluentNHibernate.Conventions.IConvention[]
                {
                    new PrimaryKeyConvention(), 
                    new ForeignKeyConvention(),
                    new HasManyConvention(),
                    new UserTypeConvention()
                };


            _container.Register(
                Component.For<ISessionFactory>()
                            .UsingFactoryMethod(() =>
                                BuildSessionFactory(connectionString.Value,conventions,
                                    allDomainAssemblies.ToArray(), allDataAssemblies.ToArray())));

            //_container.Register(Component.For<ISession>()
            //            .LifeStyle.PerWebRequest
            //            .UsingFactoryMethod(kernel => kernel.Resolve<ISessionFactory>().OpenSession()));
        }

        private static ISessionFactory BuildSessionFactory(string connectionString, FluentNHibernate.Conventions.IConvention[] conventions, Assembly[] assemblies, params Assembly[] overrides)
        {
            var persistenceModel = AutoMap.Assemblies(assemblies)
                        .Conventions.Add(conventions)
                        .Conventions.Add(Cache.Is(x => x.NonStrictReadWrite()))
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
                            .Cache(c => c.ProviderClass<SysCacheProvider>().UseSecondLevelCache().UseQueryCache())
                            .Mappings(m =>
                            {
                                m.AutoMappings.Add(persistenceModel);
                                if (overrides != null)
                                    overrides.Each(a => m.HbmMappings.AddFromAssembly(a));
                            })
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
