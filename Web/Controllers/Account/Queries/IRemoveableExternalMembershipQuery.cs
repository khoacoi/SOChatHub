using App.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.ViewModels.Account;

namespace Web.Controllers.Account.Queries
{
    public interface IRemoveableExternalMembershipQuery : IQuery
    {
        List<AccountMembershipViewModel> GetAllRelatedAccountMembershipViewModels();
    }
}