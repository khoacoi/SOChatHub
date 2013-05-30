using SharpLite.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.User
{
    public class WebMemberShip : EntityWithTypedId<Guid>
    {
        public virtual UserProfile UserProfile { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual DateTime CreatedDate { get; set; }
    }
}
