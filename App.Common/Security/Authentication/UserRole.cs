using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Security.Authentication
{
    public enum UserRole
    {
        Adminstrator,
        User,
        SSO,
        Guest
    }

    /// <summary>
    /// Declare name of Choice role
    /// </summary>
    public static class UserRoleConstant
    {
        /// <summary>
        /// The site admin role name
        /// </summary>
        public const string Adminstrator = "Adminstrator";

        /// <summary>
        /// The User role name
        /// </summary>
        public const string User = "User";

        /// <summary>
        /// The SSO role name
        /// </summary>
        public const string SSO = "SingleSignOn";
    }
}
