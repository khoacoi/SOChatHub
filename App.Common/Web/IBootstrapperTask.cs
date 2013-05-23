using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Web
{
    public interface IBootstrapperTask
    {
        void Execute(IWindsorContainer container);
    }
}
