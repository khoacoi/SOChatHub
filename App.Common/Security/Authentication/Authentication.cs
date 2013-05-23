using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Common.Extensions;
using System.Web;
using System.Web.Security;
using App.Common.Security.Crypto;

namespace App.Common.Security.Authentication
{
    public static class Authentication
    {
        private const string CookieName = "vn012_Cookie";
        private const string EmptyCookieValue = "NoCookie";

        public static void SignIn(User user, DateTime expiredDate)
        {
            var data = user.Serialize();

            HttpCookie cookie = new HttpCookie(CookieName);

            cookie.Value = EncryptionUtils.Encrypt(data);
            cookie.Expires = expiredDate;

            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Signs out.
        /// </summary>
        public static void SignOut()
        {
            ClearCookie();
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
            HttpCookie cookie = new HttpCookie(CookieName, emptyCookieValue);
            cookie.HttpOnly = true;
            cookie.Expires = new DateTime(1900, 10, 12);
            current.Response.Cookies.Remove(CookieName);
            current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Gets the user in cookie.
        /// </summary>
        /// <returns></returns>
        internal static User GetUserInCookie()
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(CookieName);
            if (cookie != null && (!string.IsNullOrEmpty(cookie.Value) || !string.Equals(cookie.Value, EmptyCookieValue, StringComparison.InvariantCultureIgnoreCase)))
            {
                string data = string.Empty;
                try
                {
                    data = EncryptionUtils.Decrypt(cookie.Value);
                    return data.Deserialize<User>();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }
    }
}
