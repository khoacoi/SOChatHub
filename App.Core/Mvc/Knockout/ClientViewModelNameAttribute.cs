using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Mvc.Knockout
{
    public class ClientViewModelNameAttribute : Attribute
    {
        public string Name { get; private set; }

        public ClientViewModelNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
