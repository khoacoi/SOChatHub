using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Security.Authentication
{
    class AuthenticationTicket
    {
        private const int VersionConst = 1;

        public AuthenticationTicket(string name, int timeout, User userData)
            : this(name, VersionConst, DateTime.UtcNow, DateTime.UtcNow.AddMinutes((double)timeout), userData)
        { 
        }

        public AuthenticationTicket(string name, int version, DateTime issueDateUtc, DateTime expirationUtc, User userData)
        {
            Name = name;
            Version = version;
            IssueDateUtc = issueDateUtc;
            ExpirationUtc = expirationUtc;
            UserData = userData;
        }

        public int Version { get; private set; }

        public string Name { get; private set; }

        public DateTime IssueDateUtc { get; private set; }

        public DateTime ExpirationUtc { get; private set; }

        public User UserData { get; private set; }

        public bool Expired
        {
            get { return ExpirationUtc <= DateTime.UtcNow; }
        }

        public void RenewTicket(int timeout)
        {
            IssueDateUtc = DateTime.UtcNow;
            ExpirationUtc = DateTime.UtcNow.AddMinutes((double)timeout);
        }

        public bool Verify()
        {
            return Version == VersionConst; 
        }
    }
}
