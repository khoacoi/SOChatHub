using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Mvc
{
    public class NameIDViewModel<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
    }
}
