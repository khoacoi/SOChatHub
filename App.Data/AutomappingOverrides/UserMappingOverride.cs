using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using App.Domain.Models.User;

namespace App.Data.AutomappingOverrides
{
    public class UserProfileMappingOverride : IAutoMappingOverride<UserProfile>
    {
        public void Override(AutoMapping<UserProfile> mapping)
        {
            //throw new NotImplementedException();
        }
    }
}
