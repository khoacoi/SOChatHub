using App.Core.Mvc;
using App.Core.Mvc.Knockout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.ViewModels.Account
{
    [ClientViewModelName("RegisterUserViewModel")]
    public class RegisterUserViewModel : PageViewModel
    {
        public string UserID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}