using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace App.Common.Security.Authentication
{
    public class User
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>
        /// The user ID.
        /// </value>
        public Guid UserID { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the role of user
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public UserRole Role { get; set; }


        /// <summary>
        /// Gets the current logged user.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static User Current
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    var identity = HttpContext.Current.User.Identity as UserIdentity;
                    return identity != null ? identity.ContextUser : null;
                }
                return null;
            }
        }
    }
}
