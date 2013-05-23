using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace App.Common.Security.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly UserRole? _role;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChoiceAuthorizeAttribute" /> class.
        /// </summary>
        public AuthorizeAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChoiceAuthorizeAttribute" /> class.
        /// </summary>
        /// <param name="role">The role.</param>
        public AuthorizeAttribute(UserRole role)
        {
            _role = role;
        }

        /// <summary>
        /// Called when authorization is required.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var granted = AuthorizeCore(filterContext.HttpContext);
            if (granted)
            {
                HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
                cache.SetProxyMaxAge(new TimeSpan(0L));
                cache.AddValidationCallback(new HttpCacheValidateHandler(this.CacheValidateHandler), (object)null);
            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        private bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.User.Identity as UserIdentity;
            var granted = user != null &&
                user.IsAuthenticated &&
                (!_role.HasValue || (_role.HasValue && ((user.ContextUser.Role & _role) == user.ContextUser.Role)));
            return granted;
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = this.OnCacheAuthorization((HttpContextBase)new HttpContextWrapper(context));
        }

        private HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");
            return !this.AuthorizeCore(httpContext) ? HttpValidationStatus.IgnoreThisRequest : HttpValidationStatus.Valid;
        }
    }
}
