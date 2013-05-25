using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.User
{
    public class User : SharpLite.Domain.EntityWithTypedId<Guid>
    {
        public virtual string Email { get; set; }
        public virtual string UserID { get; set; }
        public virtual string Password { get; set; }
        public virtual UserRole UserRole { get; set; }
        public virtual string Name { get; set; }
    }

    public enum UserRole
    {
        RegularUser,
        Vip,
        Administrator,
    }
}
