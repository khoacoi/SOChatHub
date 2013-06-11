using App.Core.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Controllers.Chat.Queries;
using Web.ViewModels.Chat;

namespace Web.Controllers.Chat
{
    [Authorize]
    public class ChatController : PageControllerBase
    {
        //
        // GET: /Chat/

        public ActionResult Index()
        {
            var viewModel = this.QueryFactory.Create<IChatViewModelQuery>().GetChatViewModel();
            return View(viewModel);
        }

    }
}
