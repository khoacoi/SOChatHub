using App.Common.Web;
using App.Core.Mvc.Optimization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Web.Startup.Tasks
{
    public class BundleRegistrationTask : IBootstrapperTask
    {
        public void Execute(Castle.Windsor.IWindsorContainer container)
        {
            var bundles = BundleTable.Bundles;

            bundles.Add(new ScriptBundle("~/bundles/json2").Include(
                        "~/Scripts/plugins/json2/json2.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/plugins/jquery/jquery-{version}.js",
                        "~/Scripts/plugins/jquery/jquery.metadata.js",
                        "~/Scripts/plugins/jquery/jquery.jscrollpane.js",
                        "~/Scripts/plugins/jquery/jquery.mousewheel.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));
            //Signal-R
            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                "~/Scripts/plugins/signalR/jquery.signalR-{version}.js"
                ));
            //Required
            bundles.Add(new ScriptBundle("~/bundles/require").IncludeDirectory(
                       "~/Scripts/plugins/require", "*.js"));

            //KnockoutJS
            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                       "~/Scripts/plugins/knockout/infuser.js",
                       "~/Scripts/plugins/knockout/TrafficCop.js",
                       "~/Scripts/plugins/knockout/knockout-{version}.js",
                       "~/Scripts/plugins/knockout/knockout.mapping-latest.js",
                       "~/Scripts/plugins/knockout/koExternalTemplateEngine.js",
                       "~/Scripts/plugins/knockout/knockout-postbox.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/plugins/modernizr/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/system/bundle/jqueryui").Include(
                "~/Content/system/css/jquery.jscrollpane.css"
                ));

        }
    }
}