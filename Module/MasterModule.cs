using App.Common.Modulary;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Modules
{
    [Export(typeof(IModule))]
    class MasterModule : ModuleBase
    {
        /// <summary>
        /// Gets the domain assemblies that containts the definition of domain entity and services.
        /// </summary>
        /// <value>
        /// The domain assemblies.
        /// </value>
        public override System.Reflection.Assembly[] DomainAssemblies
        {
            get
            {
                return new System.Reflection.Assembly[] { typeof(App.Domain.Models.User.User).Assembly };
            }
        }

        /// <summary>
        /// Gets the data assemblies that containts the specific implementation of domain services in infrastructure.
        /// </summary>
        /// <value>
        /// The data assemblies.
        /// </value>
        public override System.Reflection.Assembly[] DataAssemblies
        {
            get
            {
                return new System.Reflection.Assembly[] { typeof(App.Data.AutomappingOverrides.UserMappingOverride).Assembly };
            }
        }
    }
}
