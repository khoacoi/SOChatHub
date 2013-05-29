using App.Common.Web;
using App.Common.Web.Mvc;
using Castle.MicroKernel.Registration;
using FluentValidation;
using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Startup.Tasks
{
    public class FluentValidationRegistrationTask : IBootstrapperTask
    {
        public void Execute(Castle.Windsor.IWindsorContainer container)
        {
            FluentValidationModelValidatorProvider.Configure(p =>
            {
                p.ValidatorFactory = new WindsorFluentValidatorFactory(container);
            });

            //ValidatorOptions.ResourceProviderType = typeof(CommonErrors);

            container.Register(
                Classes.FromAssemblyInDirectory(new AssemblyFilter(HttpRuntime.BinDirectory, "Web.*"))
                    .BasedOn(typeof(IValidator<>))
                    .LifestylePerWebRequest()
                    .WithService.FromInterface()
                );
        }
    }
}