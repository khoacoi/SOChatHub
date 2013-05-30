using SharpLite.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.User
{
    public class OAuthMembership : EntityWithTypedId<Guid>
    {
        public virtual UserProfile UserProfile { get; set; }
        public virtual string Provider { get; set; }
        public virtual string ProviderUserID { get; set; }
    }
}
