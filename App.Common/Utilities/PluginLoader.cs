using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace App.Common.Utilities
{
    public static class PluginLoader
    {
        /// <summary>
        /// Loads pluggable objects from its assemblies that deployed in Bin directory.
        /// </summary>
        /// <param name="holder">The holder.</param>
        public static void LoadInBinDirectory(object holder)
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(HttpRuntime.BinDirectory));

            var container = new CompositionContainer(catalog);
            container.ComposeParts(holder);
        }
    }
}
