using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Menu
{
    public class Menu : SharpLite.Domain.EntityWithTypedId<Guid>
    {
        public virtual string Controller { get; set; }
        public virtual string Action { get; set; }
        public virtual bool IsAdministration { get; set; }

        public virtual string Name { get; set; }
    }
}
