using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Common.Web.Components
{
    public interface ITypeIdentifier
    {
        Guid ClassID { get; }
    }
}
