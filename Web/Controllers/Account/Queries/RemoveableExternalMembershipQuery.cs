using App.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.ViewModels.Account;
using App.Common.Extensions;
using App.Domain.Models.User;
using App.Common.Security.Authentication;
using Microsoft.Web.WebPages.OAuth;

namespace Web.Controllers.Account.Queries
{
    public class RemoveableExternalMembershipQuery : QueryBase, IRemoveableExternalMembershipQuery
    {
        public List<AccountMembershipViewModel> GetAllRelatedAccountMembershipViewModels()
        {
            var result = new List<AccountMembershipViewModel>();
            var listOAthMembership = this.Session.QueryOver<OAuthMembership>().Where(x => x.UserProfile.Id == User.Current.UserID).List();
            if (listOAthMembership != null && listOAthMembership.Any())
            {
                listOAthMembership.Each(x =>
                {
                    AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(x.Provider);
                    result.Add(new AccountMembershipViewModel()
                       {
                           ID = x.Id,
                           OAuthProvider = x.Provider,
                           OAuthProviderUserID = x.ProviderUserID,
                           ProviderDisplayName = clientData.DisplayName,
                       });
                });
            }
            return result;
        }
    }
}