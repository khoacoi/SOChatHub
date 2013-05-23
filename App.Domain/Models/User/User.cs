using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.User
{
    public class User : BOBase
    {
        private string email;
        private string password;

        public User(string email, string password)
        {
            // TODO: Complete member initialization
            this.email = email;
            this.password = password;
        }
    }
}
