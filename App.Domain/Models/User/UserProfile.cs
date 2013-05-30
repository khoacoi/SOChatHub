using SharpLite.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.User
{
    public class UserProfile : EntityWithTypedId<Guid>
    {
        public virtual string UserName { get; set; }
    }
}
