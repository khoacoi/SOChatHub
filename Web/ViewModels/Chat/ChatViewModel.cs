using App.Core.Mvc;
using App.Core.Mvc.Knockout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.ViewModels.Chat
{
    [ClientViewModelName("ChatViewModel")]
    public class ChatViewModel : PageViewModel
    {
        public List<UserProfileViewModel> AvailableUser { get; set; }

        public ChatViewModel()
        {
            this.AvailableUser = new List<UserProfileViewModel>();
        }
    }
}