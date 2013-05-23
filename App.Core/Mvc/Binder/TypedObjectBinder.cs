using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using App.Common.Web.Components;

namespace App.Core.Mvc.Binder
{
    public class TypedObjectBinder : DefaultModelBinder
    {
        private ITypeIdentifierLookup _lookup;
        private ITypeIdentifierLookup Lookup
        {
            get
            {
                if (_lookup == null)
                {
                    _lookup = ServiceLocator.Current.GetInstance<ITypeIdentifierLookup>();
                }
                return _lookup;
            }
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName))
            {
                var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".ClassID");
                var classID = Guid.Parse((string)valueResult.RawValue);

                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, Lookup.Find(classID));
            }

            var model = base.BindModel(controllerContext, bindingContext);
            return model;
        }
    }
}
