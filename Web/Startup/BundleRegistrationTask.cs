using App.Common.Web;
using App.Core.Mvc.Optimization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Web.Startup
{
    public class BundleRegistrationTask : IBootstrapperTask
    {
        public void Execute(Castle.Windsor.IWindsorContainer container)
        {
            var bundles = BundleTable.Bundles;
            //BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundles/json2").Include(
                        "~/Scripts/plugins/json2/json2.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/plugins/jquery/jquery-{version}.js",
                        "~/Scripts/plugins/jquery/jquery.metadata.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/plugins/jquery/jquery-ui-{version}.js",
                        "~/Scripts/plugins/jquery/jquery.jscrollpane.js",
                        "~/Scripts/plugins/jquery/jquery.form.js",
                        "~/Scripts/plugins/jquery/jquery.tree.js",
                        "~/Scripts/plugins/jquery/jquery.uniform.js"));

            bundles.Add(new ScriptBundle("~/bundles/extension")
                .IncludeDirectory("~/Scripts/Extensions", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryvalidation").Include(
                        "~/Scripts/plugins/jquery/jquery.unobtrusive*",
                        "~/Scripts/plugins/jquery/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jquerycontrol").Include(
                            "~/Scripts/plugins/jquery/jquery.placeholder.js",
                            "~/Scripts/plugins/jquery/jquery.maskedinput-1.3.js",
                            "~/Scripts/plugins/jquery/autoNumeric-{version}.js",
                            "~/Scripts/plugins/jquery/jquery-ui-timepicker-addon.js",
                            "~/Scripts/plugins/jquery/jquery.multiselect.js",
                            "~/Scripts/plugins/jquery/jquery.autoellipsis.js",
                            "~/Scripts/plugins/jquery/jquery.numberformatter-1.2.3.js"
                            ));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/plugins/modernizr/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                       "~/Scripts/plugins/knockout/infuser.js",
                       "~/Scripts/plugins/knockout/TrafficCop.js",
                       "~/Scripts/plugins/knockout/knockout-2.2.1.js",
                       "~/Scripts/plugins/knockout/knockout.mapping-latest.js",
                       "~/Scripts/plugins/knockout/koExternalTemplateEngine.js",
                       "~/Scripts/plugins/knockout/knockout-postbox.js"));

            bundles.Add(new ScriptBundle("~/bundles/require").IncludeDirectory(
                       "~/Scripts/plugins/require", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/headjs").IncludeDirectory(
                        "~/Scripts/plugins/headjs", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                        "~/Scripts/Controllers/Common/messages.js",
                        "~/Scripts/Controllers/Common/controls.js",
                        "~/Scripts/Controllers/Common/table.js",
                        "~/Scripts/Controllers/Common/dialog.js",
                        "~/Scripts/Controllers/Common/init.js",
                        "~/Scripts/Controllers/Common/helpers.js"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                        "~/Content/plugins/bootstrap/css/bootstrap.grid.css"
                ));

            bundles.Add(new StyleBundle("~/Content/system/bundle/jqueryui").Include(
                        "~/Content/system/css/jquery-ui-1.9.2.custom.css",
                        "~/Content/system/css/jquery-ui-timepicker-addon.css",
                        "~/Content/system/css/jquery.multiselect.uniform.css",
                        "~/Content/system/css/jqtree.css",
                        "~/Content/system/css/jquery.jscrollpane.css",
                        "~/Content/system/css/uniform.default.css"));

            var styleBundle = new StyleBundle("~/Content/system/bundle/site").Include(
                        "~/Content/system/css/common.css",
                        "~/Content/system/css/layout.css");
            styleBundle.Transforms.Insert(0, new CssFixPieHtcPathTransform());

            bundles.Add(styleBundle);
        }
    }
}