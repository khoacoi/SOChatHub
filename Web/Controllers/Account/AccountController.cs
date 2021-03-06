﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using Web.ViewModels.Account;
using App.Core.Mvc;
using App.Domain.Models.User;
using App.Common.Security;
using App.Common.Security.Crypto;
using Web.Controllers.Account.Queries;

namespace Web.Controllers.Account
{
    [Authorize]
    //[InitializeSimpleMembership]
    public class AccountController : PageControllerBase
    {
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Web.ViewModels.Account.LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var query = this.QueryFactory.Create<IWebMemberLoginQuery>();
                var webMemberShip = query.LoginWebMemberShip(model.UserName, model.Password);
                if (webMemberShip != null)
                {
                    App.Common.Security.Authentication.Authentication.SignIn(
                        new App.Common.Security.Authentication.User()
                        {
                            Email = webMemberShip.Email,
                            UserID = webMemberShip.UserProfile.Id,
                            UserName = webMemberShip.UserProfile.UserName,
                            Role = App.Common.Security.Authentication.UserRole.User
                        }, App.Core.ApplicationSettings.Instance.CookieTimeout);

                    //return RedirectToAction("Index", "Home");
                    return RedirectToLocal(returnUrl);
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        #region OAuthLogin
        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
                return RedirectToAction("ExternalLoginFailure");

            var oAuthRepo = this.RepositoryFactory.CreateWithGuid<OAuthMembership>();
            var oAuthMemberShip = oAuthRepo.GetAll().Where(x => x.ProviderUserID == result.ProviderUserId && x.Provider == result.Provider).SingleOrDefault();

            // do not exists oAuthMemberShip yet. Create new record and save to DB
            if (oAuthMemberShip == null)
            {
                var userProfile = new UserProfile() { UserName = null};
                oAuthMemberShip = new OAuthMembership() { Provider = result.Provider, ProviderUserID = result.ProviderUserId, UserProfile = userProfile };
                oAuthRepo.SaveOrUpdate(oAuthMemberShip);
            }

            if (oAuthMemberShip.UserProfile.UserName == null)
            {
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new Web.ViewModels.Account.RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData, CurrentUserProfileID = oAuthMemberShip.UserProfile.Id});
            }
            else
            {
                App.Common.Security.Authentication.Authentication.SignIn(
                            new App.Common.Security.Authentication.User()
                            {
                                Email = null,
                                UserID = oAuthMemberShip.UserProfile.Id,
                                UserName = oAuthMemberShip.UserProfile.UserName,
                                Role = App.Common.Security.Authentication.UserRole.OAuth
                            }, App.Core.ApplicationSettings.Instance.CookieTimeout);
                return RedirectToLocal(returnUrl);
            }
        }

        //[AllowAnonymous]
        //public ActionResult ExternalLoginCallback(string returnUrl)
        //{
        //    AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        //    if (!result.IsSuccessful)
        //    {
        //        return RedirectToAction("ExternalLoginFailure");
        //    }

        //    if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
        //    {
        //        return RedirectToLocal(returnUrl);
        //    }

        //    if (User.Identity.IsAuthenticated)
        //    {
        //        // If the current user is logged in add the new account
        //        OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
        //        return RedirectToLocal(returnUrl);
        //    }
        //    else
        //    {
        //        // User is new, ask for their desired membership name
        //        string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
        //        ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
        //        ViewBag.ReturnUrl = returnUrl;
        //        return View("ExternalLoginConfirmation", new Web.Models.RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
        //    }
        //}

        
         //POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(Web.ViewModels.Account.RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                var repo = this.RepositoryFactory.CreateWithGuid<UserProfile>();
                if (!repo.GetAll().Any(x => x.UserName.Trim().ToLower() == model.UserName.Trim().ToLower()))
                {
                    var userProfile = repo.Get(model.CurrentUserProfileID);
                    if (userProfile == null)
                        throw new Exception("Cannot found User Profile");

                    userProfile.UserName = model.UserName;
                    repo.SaveOrUpdate(userProfile);
                    App.Common.Security.Authentication.Authentication.SignIn(
                                new App.Common.Security.Authentication.User()
                                {
                                    Email = null,
                                    UserID = userProfile.Id,
                                    UserName = model.UserName,
                                    Role = App.Common.Security.Authentication.UserRole.OAuth
                                }, App.Core.ApplicationSettings.Instance.CookieTimeout);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        #endregion OAthLogin

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            App.Common.Security.Authentication.Authentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            var viewModel = new RegisterUserViewModel();
            return View(viewModel);
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterUserViewModel model)
        {
            if (IsRegisterUserModelValid(model))
            {
                var encryptedPassword = App.Utilities.Security.Crypto.EncryptStringAES(model.Password, model.UserID);
                var userProfile = new UserProfile() { UserName = model.UserID };

                var webMemberShip = new WebMemberShip() { UserProfile = userProfile, CreatedDate = DateTime.Now, Email = model.Email.Trim().ToLower(), Password = encryptedPassword };

                var repo = this.RepositoryFactory.CreateWithGuid<WebMemberShip>();
                repo.SaveOrUpdate(webMemberShip);

                return Json(new
                {
                    IsSuccess = true
                });
            }

            return this.JsonValidation();
        }

        private bool IsRegisterUserModelValid(RegisterUserViewModel model)
        {
            if (this.RepositoryFactory.CreateWithGuid<UserProfile>().GetAll().Any(x => x.UserName.ToLower().Trim() == model.UserID.ToLower().Trim()))
                ModelState.AddModelError("UserID", "User name already exists. Please enter a different user name.");
            if (this.RepositoryFactory.CreateWithGuid<WebMemberShip>().GetAll().Any(x => x.Email == model.Email.ToLower()))
                ModelState.AddModelError("Email", "Email already registered in system. Please recover your password or enter a different email.");
            return ModelState.IsValid;
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {

            //ViewBag.StatusMessage =
            //    message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
            //    : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
            //    : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
            //    : "";
            //ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            //ViewBag.ReturnUrl = Url.Action("Manage");

            var query = this.QueryFactory.Create<IManageAccountQuery>();
            var manageAccountViewModel = query.GetManageAccountViewModel();

            return View("Manage2", manageAccountViewModel);
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(Web.ViewModels.Account.LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            var externalLogins = this.QueryFactory.Create<IRemoveableExternalMembershipQuery>().GetAllRelatedAccountMembershipViewModels();

            return PartialView("_RemoveExternalLoginsPartial", externalLogins);

            //ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            //List<Web.ViewModels.Account.ExternalLogin> externalLogins = new List<Web.ViewModels.Account.ExternalLogin>();
            //foreach (OAuthAccount account in accounts)
            //{
            //    AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

            //    externalLogins.Add(new Web.ViewModels.Account.ExternalLogin
            //    {
            //        Provider = account.Provider,
            //        ProviderDisplayName = clientData.DisplayName,
            //        ProviderUserId = account.ProviderUserId,
            //    });
            //}

            //ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            //return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
