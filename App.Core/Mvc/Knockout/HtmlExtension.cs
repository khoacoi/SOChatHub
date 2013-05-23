using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace App.Core.Mvc.Knockout
{
    /// <summary>
    /// Mapping server model to client model for knockoutJS binding.
    /// </summary>
    public static class HtmlExtension
    {
        /// <summary>
        /// Create client script tag to wrap viewmodel client and initialize KO binding.
        /// </summary>
        /// <param name="helper">MVC Htmlhelper.</param>
        /// <param name="viewModel">Server Model.</param>
        /// <param name="bindingElementId">Client Element Id to apply binding.</param>
        /// <returns>Client script for knockoutJS binding.</returns>
        public static MvcHtmlString KOApplyBindings(this HtmlHelper helper, object viewModel, string bindingElementId = "")
        {
            var viewModelName = GetViewModelName(viewModel);
            var sb = new StringBuilder();
            sb.AppendLine(@"<script type=""text/javascript"">");
            sb.AppendLine(string.Format("var {0} = ko.mapping.fromJS({1});", viewModelName, ConvertViewModelToKnockout(viewModel)));
            if (string.IsNullOrWhiteSpace(bindingElementId))
                sb.AppendLine(string.Format("ko.applyBindings({0});", viewModelName));
            else
                sb.AppendLine(string.Format(@"ko.applyBindings({0}, $(""#{1}"")[0]);", viewModelName, bindingElementId));
            sb.AppendLine(@"</script>");
            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// Create viewmodel client.
        /// </summary>
        /// <param name="helper">MVC Htmlhelper</param>
        /// <param name="viewModel">Server Model.</param>
        /// <returns>Client script for client model mapping.</returns>
        public static MvcHtmlString KOViewModelInitialize(this HtmlHelper helper, object viewModel)
        {
            var viewModelName = GetViewModelName(viewModel);
            var sb = new StringBuilder();
            sb.AppendLine(@"<script type=""text/javascript"">");
            sb.AppendLine(string.Format("var {0} = ko.mapping.fromJS({1});", viewModelName, ConvertViewModelToKnockout(viewModel)));
            sb.AppendLine(@"</script>");
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString KOViewModelInstance(this HtmlHelper helper, object viewModel)
        {
            var viewModelName = GetViewModelName(viewModel);
            var sb = new StringBuilder();
            sb.AppendLine(@"<script type=""text/javascript"">");
            sb.AppendLine(string.Format("function {0}() {1} return {2}; {3};", viewModelName, "{", ConvertViewModelToKnockout(viewModel), "}"));
            sb.AppendLine(@"</script>");
            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// Create client script tag to wrap viewmodel client and initialize KO binding.
        /// Default server model is the model in ViewContext.
        /// </summary>
        /// <param name="helper">MVC Htmlhelper.</param>
        /// <param name="bindingElementId">Client Element Id to apply binding.</param>
        /// <returns>Client script for knockoutJS binding.</returns>
        public static MvcHtmlString KOApplyBindings(this HtmlHelper helper, string bindingElementId = "")
        {
            return KOApplyBindings(helper, helper.ViewContext.ViewData.Model, bindingElementId);
        }

        /// <summary>
        /// Create viewmodel client.
        /// Default server model is the model in ViewContext.
        /// </summary>
        /// <param name="helper">MVC Htmlhelper</param>
        /// <param name="viewModel">Server Model.</param>
        /// <returns>Client script for client model mapping.</returns>
        public static MvcHtmlString KOViewModelInitialize(this HtmlHelper helper)
        {
            return KOViewModelInitialize(helper, helper.ViewContext.ViewData.Model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static MvcHtmlString KOGetViewModelName(this HtmlHelper helper, object viewModel)
        {
            return MvcHtmlString.Create(GetViewModelName(viewModel));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString KOGetViewModelName(this HtmlHelper helper)
        {
            return MvcHtmlString.Create(GetViewModelName(helper.ViewContext.ViewData.Model));
        }

        private static string GetViewModelName(object viewModel)
        {
            var attrs = (ClientViewModelNameAttribute[])viewModel.GetType().GetCustomAttributes(typeof(ClientViewModelNameAttribute), false);
            if (attrs.Length > 0)
                return attrs[0].Name;
            else
                return "ViewModel";
        }

        private static string ConvertViewModelToKnockout(object viewModel)
        {
            return FluentJson.Knockout.ToViewModel(viewModel).ToString().Replace("<script>", "< script>").Replace("</script>", "</ script>");
        }

        public static MvcHtmlString ClientViewModel(this HtmlHelper helper)
        {
            var viewModel = helper.ViewContext.ViewData.Model;
            var json = JsonConvert.SerializeObject(viewModel);

            var viewModelName = GetViewModelName(viewModel);
            var sb = new StringBuilder();
            sb.AppendLine(@"<script type=""text/javascript"">");
            sb.AppendLine(string.Format("var {0} = ko.mapping.fromJS({1});", viewModelName, json));
            sb.AppendLine(@"</script>");
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}
