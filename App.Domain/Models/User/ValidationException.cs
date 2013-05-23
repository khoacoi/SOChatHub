using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Domain.Models.User
{
    class ValidationException : Exception
    {
        private string p;

        public ValidationException(string p)
        {
            // TODO: Complete member initialization
            this.p = p;
        }
    }
}
