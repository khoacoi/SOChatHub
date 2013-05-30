using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Security.Authentication
{
    public class Principal : IPrincipal
    {
        private readonly UserRole _role;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChoicePrincipal" /> class.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="role">The role.</param>
        public Principal(IIdentity identity, UserRole role)
        {
            Identity = identity;
            _role = role;
        }

        /// <summary>
        /// Gets the identity of the current principal.
        /// </summary>
        /// <returns>The <see cref="T:System.Security.Principal.IIdentity" /> object associated with the current principal.</returns>
        public IIdentity Identity { get; private set; }

        /// <summary>
        /// Determines whether the current principal belongs to the specified role.
        /// </summary>
        /// <param name="role">The name of the role for which to check membership.</param>
        /// <returns>
        /// true if the current principal is a member of the specified role; otherwise, false.
        /// </returns>
        public bool IsInRole(string role)
        {
            UserRole checkingRole;
            if (Enum.TryParse<UserRole>(role, out checkingRole))
            {
                switch (checkingRole)
                {
                    case UserRole.Adminstrator:
                        return _role == UserRole.Adminstrator;
                    case UserRole.User:
                        return _role == UserRole.User;
                    case UserRole.OAuth:
                        return _role == UserRole.OAuth;
                    case UserRole.Guest:
                        return _role == UserRole.Guest;
                }
            }
            return false;
        }
    }
}
