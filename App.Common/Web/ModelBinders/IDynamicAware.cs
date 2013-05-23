using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Web.ModelBinders
{
    /// <summary>
    /// Use this marker interface to indicates an object is able bound as a dynamic object
    /// from JSON. The <see cref="JsonDotNetValueProviderFactory"/> uses this marker to cast an object
    /// returned in JSON to dynamic object.
    /// </summary>
    public interface IDynamicAware
    {
        /// <summary>
        /// Gets a value indicating whether this instance is dynamic object.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dynamic object; otherwise, <c>false</c>.
        /// </value>
        bool IsDynamicObject { get; }
    }
}
