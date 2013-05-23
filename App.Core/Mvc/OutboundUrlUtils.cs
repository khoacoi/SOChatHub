using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace App.Core.Mvc
{
    /// <summary>
    /// This utility class supports generates url by route data in code where the controller context is not available.
    /// Caution: this utility should only be used where controller context or urlhelper is not available. In otherwise,
    /// please use UrlHelper to generate the url.
    /// </summary>
    public class OutboundUrlUtils
    {
        /// <summary>
        /// Generates the url by specified route data.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">The operation isn't running in http context</exception>
        public static string Generate(object routeData)
        {
            if (HttpContext.Current == null)
                throw new InvalidOperationException("The operation isn't running in http context");

            return RouteTable.Routes.GetVirtualPath(((MvcHandler)HttpContext.Current.CurrentHandler).RequestContext, new RouteValueDictionary(routeData)).VirtualPath;
        }

        public static string ToAbsoluteUrl(string relativeUrl)
        {
            return VirtualPathUtility.ToAbsolute(relativeUrl, HttpContext.Current.Request.ApplicationPath);
        }
    }
}
