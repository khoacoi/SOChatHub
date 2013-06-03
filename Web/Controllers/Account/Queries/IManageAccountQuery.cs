using App.Common.Data;
using App.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.ViewModels.Account;

namespace Web.Controllers.Account.Queries
{
    public interface IManageAccountQuery : IQuery
    {
        ManageAccountViewModel GetManageAccountViewModel();
    }
}