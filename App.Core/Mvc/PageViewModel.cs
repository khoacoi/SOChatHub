using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Mvc.Navigation;

namespace App.Core.Mvc
{
    /// <summary>
    /// Base viewmodel object for the Choice page view model. 
    /// Page view model is a view model represents data for one page of view, not a partial view model or json view model.
    /// </summary>
    public abstract class PageViewModel
    {
        /// <summary>
        /// Gets or sets the menu items. 
        /// Its value will be populated in Choice controller. And _Layout will use this data for rendering menu in web page.
        /// </summary>
        /// <value>
        /// The menu items.
        /// </value>
        [JsonIgnore]
        public IList<MenuItem> MenuItems { get; set; }
    }
}
