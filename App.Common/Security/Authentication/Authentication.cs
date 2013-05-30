using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Common.Extensions;
using System.Web;
using System.Web.Security;
using App.Common.Security.Crypto;
using System.Threading;

namespace App.Common.Security.Authentication
{
    public static class Authentication
    {
        private const string CookieName = "vn012_Cookie";
        private const string EmptyCookieValue = "NoCookie";

        //public static void SignIn(User user, DateTime expiredDate)
        //{
        //    var data = user.Serialize();

        //    HttpCookie cookie = new HttpCookie(CookieName);

        //    cookie.Value = EncryptionUtils.Encrypt(data);
        //    cookie.Expires = expiredDate;

        //    System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
        //}
        public static void SignIn(User user, int timeout)
        {
            var cookie = GetAuthCookie(user.UserName, timeout, user);

            HttpContext.Current.Response.Cookies.Remove(CookieName);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Signs out.
        /// </summary>
        public static void SignOut()
        {
            ClearCookie();

            Thread.CurrentPrincipal = null;
            HttpContext.Current.User = null;
        }

        ///// <summary>
        ///// Spoofs the specified user.
        ///// </summary>
        ///// <param name="spoofingUser">The spoofing user.</param>
        ///// <param name="expiredDate">The expired date.</param>
        //public static void SwitchUser(User spoofingUser, DateTime expiredDate)
        //{
        //    SignOut();
        //    SignIn(spoofingUser, expiredDate);
        //}

        /// <summary>
        /// Clears the cookie.
        /// </summary>
        private static void ClearCookie()
        {
            string emptyCookieValue = string.Empty;
            var current = HttpContext.Current;
            if (current.Request.Browser["supportsEmptyStringInCookieValue"] == "false")
                emptyCookieValue = EmptyCookieValue;

            var cookiePath = HttpContext.Current.Request.ApplicationPath;
            if (!cookiePath.EndsWith("/"))
                cookiePath = cookiePath + "/";

            HttpCookie cookie = new HttpCookie(CookieName, emptyCookieValue);
            cookie.HttpOnly = true;
            cookie.Expires = new DateTime(1900, 10, 12);
            cookie.Path = cookiePath;
            current.Response.Cookies.Remove(CookieName);
            current.Response.Cookies.Add(cookie);
        }

        private static HttpCookie GetAuthCookie(string userName, int timeout, User user)
        {
            if (userName == null)
                userName = string.Empty;

            var cookiePath = HttpContext.Current.Request.ApplicationPath;
            if (!cookiePath.EndsWith("/"))
                cookiePath = cookiePath + "/";

            var ticket = new AuthenticationTicket(userName, timeout, user);

            string encrypted = EncryptionUtils.Encrypt(Convert.ToBase64String(AppAuthenticationTicketSerializer.Serialize(ticket)));

            HttpCookie httpCookie = new HttpCookie(CookieName, encrypted);
            httpCookie.HttpOnly = true;
            httpCookie.Path = cookiePath;
            httpCookie.Secure = HttpContext.Current.Request.IsSecureConnection;
            httpCookie.Expires = ticket.ExpirationUtc.ToLocalTime();
            return httpCookie;
        }

        internal static AuthenticationTicket GetTicketFromCookie()
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(CookieName);
            if (cookie != null && (!string.IsNullOrEmpty(cookie.Value) || !string.Equals(cookie.Value, EmptyCookieValue, StringComparison.InvariantCultureIgnoreCase)))
            {
                try
                {
                    var decrypted = EncryptionUtils.Decrypt(cookie.Value);
                    byte[] ticketBin = Convert.FromBase64String(decrypted);
                    return AppAuthenticationTicketSerializer.Deserialize(ticketBin, ticketBin.Length);
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }
    }
}
