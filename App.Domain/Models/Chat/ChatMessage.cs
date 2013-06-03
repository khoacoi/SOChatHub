using SharpLite.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Chat
{
    public class ChatMessage : EntityWithTypedId<Guid>
    {
        public virtual string UserName { get; set; }
        public virtual Guid UserProfileID { get; set; }
        public virtual string Message { get; set; }
        public virtual DateTime Date { get; set; }
    }
}
