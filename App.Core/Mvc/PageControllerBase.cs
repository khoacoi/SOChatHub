using PerpetuumSoft.Knockout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using App.Common.Commands;
using App.Common.Data;
using App.Core.Mvc.Navigation;

namespace App.Core.Mvc
{
    public abstract class PageControllerBase : Controller
    {
        /// <summary>
        /// Gets or sets the repository factory.
        /// </summary>
        /// <value>
        /// The repository factory.
        /// </value>
        public IRepositoryFactory RepositoryFactory { get; set; }

        /// <summary>
        /// Gets or sets the query factory.
        /// </summary>
        /// <value>
        /// The query factory.
        /// </value>
        public IQueryFactory QueryFactory { get; set; }

        /// <summary>
        /// Gets or sets the command processor.
        /// </summary>
        /// <value>
        /// The command processor.
        /// </value>
        public ICommandProcessor CommandProcessor { get; set; }

        public JsonResult JsonValidation(object model = null)
        {
            if (ModelState.IsValid)
            {
                return new JsonResult
                {
                    Data = new
                    {
                        IsSuccess = true,
                        Result = model
                    }
                };
            }

            return new JsonResult
            {
                Data = new
                {
                    Result = model,
                    IsSuccess = false,
                    Tag = "ValidationError",
                    State = from e in ModelState
                            where e.Value.Errors.Count > 0
                            select new
                            {
                                Name = e.Key,
                                Errors = e.Value.Errors.Select(x => x.ErrorMessage)
                                   .Concat(e.Value.Errors.Where(x => x.Exception != null).Select(x => x.Exception.Message))
                            }
                }
            };
        }

        /// <summary>
        /// Called before the action method is invoked.
        /// Before actually executing an action, it populates RunAsTenantID if it is existed.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    _runingAsOtherTenantID = filterContext.RouteData.Tenant();

        //    //Those two properties in ViewBag is used in _LoginPartial for showing the running as company name.
        //    ViewBag.IsRunningAsOtherTenant = IsRunningAsOtherTenant;

        //    if (IsRunningAsOtherTenant)
        //    {
        //        var runningTenant = GetGrantRunningAsTenant();
        //        if (runningTenant == null)
        //        {
        //            filterContext.Result = new HttpNotFoundResult();
        //        }
        //        else
        //        {
        //            //Those two properties in ViewBag is used in _LoginPartial for showing the running as company name.
        //            ViewBag.RunningAsCompanyName = runningTenant.CompanyName;
        //        }
        //    }

        //    base.OnActionExecuting(filterContext);
        //}

        /// <summary>
        /// Called after the action method is invoked.
        /// Before actually executed action, it populates the menu items into view model for showing the menu in view.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult != null && !filterContext.IsChildAction)
            {
                var vm = viewResult.Model as PageViewModel;
                if (vm != null && vm.MenuItems == null)
                {
                    vm.MenuItems = MenuHelper.LoadMenuItems(this.Url);
                }
            }

            base.OnActionExecuted(filterContext);
        }


        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding)
        {
            KnockoutUtilities.ConvertData(data);
            return base.Json(data, contentType, contentEncoding);
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            KnockoutUtilities.ConvertData(data);
            return base.Json(data, contentType, contentEncoding, behavior);
        }

        #region Helpers
        //private Tenant GetGrantRunningAsTenant()
        //{
        //    return RepositoryFactory.CreateWithGuid<Tenant>().GetAll()
        //        .Where(t => t.SiteAdmins.Any(sa => sa.Id == ChoiceUser.Current.UserID) && t.Id == _runingAsOtherTenantID)
        //        .SingleOrDefault();
        //}
        #endregion
    }
}
