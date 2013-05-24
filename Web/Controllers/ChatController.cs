using App.Core.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class ChatController : PageControllerBase
    {
        //
        // GET: /Chat/

        public ActionResult Index()
        {
            return View();
        }

    }
}
