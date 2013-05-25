using Castle.Windsor;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Web.Mvc
{
    public class WindsorFluentValidatorFactory : ValidatorFactoryBase
    {
        private readonly IWindsorContainer _container;

        public WindsorFluentValidatorFactory(IWindsorContainer container)
        {
            _container = container;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            return _container.Kernel.HasComponent(validatorType) ? _container.Resolve(validatorType) as IValidator : null;
        }
    }
}
