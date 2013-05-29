using App.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Startup.Tasks
{
    public class FilterRegistrationTask : IBootstrapperTask
    {
        public void Execute(Castle.Windsor.IWindsorContainer container)
        {
            //TODO: add global filter handling works with ELMAH.
            //GlobalFilters.Filters.Add(new ImpersonatingValidationFilter());
            //GlobalFilters.Filters.Add(new SSOSessionValidationFilter());
        }
    }
}