using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using App.Common.Data;
using App.Domain.Models.Menu;

namespace App.Core.Mvc.Navigation
{
    public static class MenuHelper
    {
        /// <summary>
        /// Loads the menu items.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="runAsTenant">The run as tenant id.</param>
        /// <returns></returns>
        public static IList<MenuItem> LoadMenuItems(UrlHelper urlHelper)
        {
            var repo = ServiceLocator.Current.GetInstance<IRepositoryFactory>();
            var repoMenu = repo.CreateWithGuid<Menu>();

            var menus = repoMenu.GetAll().ToList();

            var items = new List<MenuItem>();

            foreach (var menu in menus)
            {
                var link = string.Empty;
                link = urlHelper.RouteUrl("Default", new { controller = menu.Controller, action = menu.Action });

                var item = new MenuItem()
                {
                    Text = menu.Name,
                    Link = link,
                    IsAdministration = menu.IsAdministration
                };

                items.Add(item);
            }
            //var items = new List<MenuItem>();
            if (items == null)
                items = new List<MenuItem>();

            return items;
        }
    }
}
