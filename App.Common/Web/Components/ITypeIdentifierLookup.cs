using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Web.Components
{
    public interface ITypeIdentifierLookup
    {
        void Scan(Assembly assembly);
        Type Find(Guid typeGUID);
    }
}
