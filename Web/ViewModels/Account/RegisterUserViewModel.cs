using App.Core.Mvc;
using App.Core.Mvc.Knockout;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.ViewModels.Account.Validators;

namespace Web.ViewModels.Account
{
    [ClientViewModelName("RegisterUserViewModel")]
    [Validator(typeof(RegisterUserValidator))]
    public class RegisterUserViewModel : PageViewModel
    {
        public string UserID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}