using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Mvc.Navigation
{
    public class MenuItem
    {
        /// <summary>
        /// Gets or sets a value indicating whether this item belongs to administration feature.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is administration; otherwise, <c>false</c>.
        /// </value>
        public bool IsAdministration { get; set; }

        /// <summary>
        /// Gets or sets the display text of menu item
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the link of menu item.
        /// </summary>
        /// <value>
        /// The link.
        /// </value>
        public string Link { get; set; }
    }
}
