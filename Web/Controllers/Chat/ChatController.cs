using App.Core.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            var viewModel = new ChatViewModel();
            return View(viewModel);
        }

    }
}
