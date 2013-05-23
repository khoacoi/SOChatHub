using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Data
{
    public class ConnectionString : IConnectionString
    {
        public ConnectionString(string value)
        {
            this.Value = value;
        }
        /// <summary>
        /// Get the connection string value
        /// </summary>
        public string Value{get; private set;}
    }
}
