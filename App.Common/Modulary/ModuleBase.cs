using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Modulary
{
    public class ModuleBase : IModule
    {
        /// <summary>
        /// Gets the domain assemblies that containts the definition of domain entity and services.
        /// </summary>
        /// <value>
        /// The domain assemblies.
        /// </value>
        public virtual System.Reflection.Assembly[] DomainAssemblies
        {
            get { return new System.Reflection.Assembly[] { }; }
        }

        /// <summary>
        /// Gets the data assemblies that containts the specific implementation of domain services in infrastructure.
        /// </summary>
        /// <value>
        /// The data assemblies.
        /// </value>
        public virtual System.Reflection.Assembly[] DataAssemblies
        {
            get { return new System.Reflection.Assembly[] { }; }
        }

        /// <summary>
        /// Loads the features.
        /// </summary>
        /// <returns></returns>
        public virtual void Initialize(Castle.Windsor.IWindsorContainer container)
        {
            
        }
    }
}
