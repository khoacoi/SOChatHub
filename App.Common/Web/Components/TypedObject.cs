using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Web.Components
{
    public abstract class TypedObject : ITypeIdentifier
    {
        public Guid ClassID
        {
            get { return this.GetType().GUID; }
        }
    }
}
