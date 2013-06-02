using App.Common.Data;
using App.Common.Security.Authentication;
using App.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.ViewModels.Account;
using App.Common.Extensions;

namespace Web.Controllers.Account.Queries
{
    public class MangeAccountQuery : QueryBase, IManageAccountQuery
    {
        public ManageAccountViewModel GetManageAccountViewModel()
        {
            var currentUserProfileID = User.Current.UserID;
            var userProfile = this.Session.Get<UserProfile>(currentUserProfileID);
            var webMembership = this.Session.QueryOver<WebMemberShip>().Where(x => x.UserProfile.Id == currentUserProfileID).SingleOrDefault();
            var manageAccountViewModel = new ManageAccountViewModel();
            manageAccountViewModel.UserName = userProfile.UserName;
            
            manageAccountViewModel.IsStoredWebmembership = webMembership != null;
            manageAccountViewModel.Password = webMembership != null ? App.Utilities.Security.Crypto.DecryptStringAES(webMembership.Password, userProfile.UserName) : null;

            return manageAccountViewModel;
        }

    }
}