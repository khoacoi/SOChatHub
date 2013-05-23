using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace App.Common.Web.ModelBinders
{
    /// <summary>
    /// http://www.codeproject.com/Articles/420536/Customizing-property-binding-through-attributes
    /// </summary>
    public class ExtendedDefaultModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext,
            ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.Attributes.OfType<PropertyBindAttribute>().Any())
            {
                var modelBindAttr = propertyDescriptor.Attributes.OfType<PropertyBindAttribute>().FirstOrDefault();

                if (modelBindAttr.BindProperty(controllerContext, bindingContext, propertyDescriptor))
                    return;
            }

            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }
}
