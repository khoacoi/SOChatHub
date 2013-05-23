using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Modulary
{
    /// <summary>
    /// Declases interface of application module supported in the system.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Gets the domain assemblies that containts the definition of domain entity and services.
        /// </summary>
        /// <value>
        /// The domain assemblies.
        /// </value>
        Assembly[] DomainAssemblies { get; }

        /// <summary>
        /// Gets the data assemblies that containts the specific implementation of domain services in infrastructure.
        /// </summary>
        /// <value>
        /// The data assemblies.
        /// </value>
        Assembly[] DataAssemblies { get; }

        /// <summary>
        /// Initializes the application module.
        /// </summary>
        /// <param name="container">The container.</param>
        void Initialize(IWindsorContainer container);
    }
}
