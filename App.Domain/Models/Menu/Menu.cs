using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Menu
{
    public class Menu : BOBase
    {
        public virtual string Controller { get; set; }
        public virtual string Action { get; set; }
        public virtual bool IsAdministration { get; set; }

        public virtual string Name { get; set; }
    }
}
