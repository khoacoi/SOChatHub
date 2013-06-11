using App.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.ViewModels.Chat;

namespace Web.Controllers.Chat.Queries
{
    public interface IChatViewModelQuery : IQuery
    {
        ChatViewModel GetChatViewModel();
    }
}