using App.Common.Data;
using App.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Controllers.Account.Queries
{
    public class WebMemberLoginQuery : QueryBase, IWebMemberLoginQuery
    {
        public WebMemberShip LoginWebMemberShip(string username, string password)
        {
            var userprofile = this.Session.QueryOver<UserProfile>().Where(x => x.UserName == username).SingleOrDefault();
            if (userprofile != null)
            {
                var webMembership = this.Session.QueryOver<WebMemberShip>().Where(x => x.UserProfile.Id == userprofile.Id).SingleOrDefault();
                if (webMembership != null)
                {
                    var decriptedPassword = App.Utilities.Security.Crypto.DecryptStringAES(webMembership.Password, username);
                    if (decriptedPassword == password)
                        return webMembership;
                }
            }
            return null;

        }
    }
}