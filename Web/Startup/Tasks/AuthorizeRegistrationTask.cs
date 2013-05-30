using App.Common.Web;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Startup.Tasks
{
    // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
    // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166
    // detail guide here: http://blogs.msdn.com/b/webdev/archive/2012/08/15/oauth-openid-support-for-webforms-mvc-and-webpages.aspx
    public class AuthorizeRegistrationTask : IBootstrapperTask
    {
        public void Execute(Castle.Windsor.IWindsorContainer container)
        {
            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "319649478165197",
                appSecret: "6e1cd15283a6598c89f7f7eb56f81735");

            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}