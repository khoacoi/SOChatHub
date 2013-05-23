using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Security.Authentication
{
    public class UserIdentity : IIdentity
    {
        private readonly User _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserIdentity" /> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public UserIdentity(User user)
        {
            _user = user;
        }

        /// <summary>
        /// Gets the type of authentication used.
        /// </summary>
        /// <returns>The type of authentication used to identify the user.</returns>
        public string AuthenticationType
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets a value that indicates whether the user has been authenticated.
        /// </summary>
        /// <returns>It always return true</returns>
        public bool IsAuthenticated
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the name of the current user.
        /// </summary>
        /// <returns>The name of the user on whose behalf the code is running.</returns>
        public string Name
        {
            get { return _user.UserName; }
        }

        /// <summary>
        /// Gets the Choice context user.
        /// </summary>
        /// <value>
        /// The context user.
        /// </value>
        public User ContextUser
        {
            get { return _user; }
        }
    }
}
