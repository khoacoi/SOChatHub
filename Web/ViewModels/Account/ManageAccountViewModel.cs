using App.Core.Mvc;
using App.Core.Mvc.Knockout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.ViewModels.Account
{
    [ClientViewModelName("ManageAccountViewModel")]
    public class ManageAccountViewModel : PageViewModel
    {
        public string UserName { get; set; }
        public bool IsStoredWebmembership { get; set; }

        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        //public IList<AccountMembershipViewModel> MemberShips { get; set; }

        //public ManageAccountViewModel()
        //{
        //    MemberShips = new List<AccountMembershipViewModel>();
        //}
    }

    [ClientViewModelName("AccountMembershipViewModel")]
    public class AccountMembershipViewModel
    {
        public Guid ID { get; set; }
        public AccountMembershipType AccountMembershipType { get; set; }

        public string ProviderDisplayName { get; set; }
        public string OAuthProvider { get; set; }
        public string OAuthProviderUserID { get; set; }
    }

    public enum AccountMembershipType
    {
        WebMembership,
        Google,
        Facebook,
        Microsoft
    }
}