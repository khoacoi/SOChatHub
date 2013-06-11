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
        public virtual Guid SenderID { get; set; }
        public virtual string Message { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual eSendStatus SendStatus { get; set; }
    }

    public enum eSendStatus
    {
        Sending,
        Sent,
        Received
    }

    public enum eSendType
    {
        ToAll,
        Private,
        Group
    }
}
