using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Data
{
    /// <summary>
    /// Represent a connection string inteface. That is used for persisting connection string in IoC container.
    /// </summary>
    public interface IConnectionString
    {
        /// <summary>
        /// Gets the connection string value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        string Value { get; }
    }
}
