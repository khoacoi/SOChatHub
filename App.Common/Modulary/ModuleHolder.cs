using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

namespace App.Common.Modulary
{
    /// <summary>
    /// It supports holding the application modules.
    /// </summary>
    public class ModuleHolder
    {
        [ImportMany(typeof(IModule))]
        public IModule[] Modules { get; set; }
    }
}
