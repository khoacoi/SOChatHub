using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Optimization;

namespace App.Core.Mvc.Optimization
{
    public class CssFixPieHtcPathTransform : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            var absoluteFilePath = VirtualPathUtility.ToAbsolute("~/Scripts/plugins/pie/PIE.htc", context.HttpContext.Request.ApplicationPath);
            response.Content = response.Content.Replace("/Scripts/plugins/pie/PIE.htc", absoluteFilePath);
            response.ContentType = "text/css";
        }
    }
}
