using App.Common.Data;
using App.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Controllers.Account.Queries
{
    public interface IWebMemberLoginQuery : IQuery
    {
        WebMemberShip LoginWebMemberShip(string username, string password);
    }
}