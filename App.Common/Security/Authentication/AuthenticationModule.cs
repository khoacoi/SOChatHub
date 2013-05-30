using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace App.Common.Security.Authentication
{
    public class AuthenticationModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += OnAuthenticateRequest;
        }

        private void OnAuthenticateRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;

            if (context.User != null)
                return;

            var ticket = Authentication.GetTicketFromCookie();
            if (ticket != null)
            {
                if (ticket.Expired || !ticket.Verify())
                {
                    Authentication.SignOut();
                }
                else
                {
                    var principal = new Principal(new UserIdentity(ticket.UserData), ticket.UserData.Role);
                    context.User = principal;

                    Thread.CurrentPrincipal = principal;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
