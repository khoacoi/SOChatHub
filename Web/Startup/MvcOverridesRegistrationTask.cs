using App.Common.Web;
using App.Common.Web.Components;
using App.Common.Web.ModelBinders;
using App.Core.Mvc.Binder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Startup
{
    /// <summary>
    /// A task registers the custom infrastructure components in asp.net mvc.
    /// </summary>
    public class MvcOverridesRegistrationTask : IBootstrapperTask
    {
        public void Execute(Castle.Windsor.IWindsorContainer container)
        {
            ////Bind default binder to Choice extended default model binder. By this extension, model binder now 
            ////supports property binder extension, instead of in parameter of action of controller.
            ModelBinders.Binders.DefaultBinder = new ExtendedDefaultModelBinder();
            ModelBinders.Binders.Add(typeof(TypedObject), new TypedObjectBinder());
        }
    }
}