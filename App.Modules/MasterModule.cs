using App.Data.AutomappingOverrides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Common.Modulary;
using App.Domain.Models.User;

namespace App.Modules
{
    public class MasterModule : ModuleBase
    {
        /// Gets the domain assemblies that containts the definition of domain entity and services.
        /// </summary>
        /// <value>
        /// The domain assemblies.
        /// </value>
        public override System.Reflection.Assembly[] DomainAssemblies
        {
            get
            {
                return new System.Reflection.Assembly[] { typeof(User).Assembly };
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
                return new System.Reflection.Assembly[] { typeof(UserMappingOverride).Assembly };
            }
        }
    }
}
