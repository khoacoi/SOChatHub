using Microsoft.Practices.ServiceLocation;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Data
{
    public abstract class QueryBase : IQuery
    {
        /// <summary>
        /// Gets current NHibernate session.
        /// </summary>
        /// <value>
        /// The session.
        /// </value>
        protected virtual ISession Session
        {
            get
            {
                return SessionFactory.GetCurrentSession();
            }
        }

        private ISessionFactory _sessionFactory;
        /// <summary>
        /// Gets the session factory.
        /// </summary>
        /// <value>
        /// The session factory.
        /// </value>
        protected virtual ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = ServiceLocator.Current.GetInstance<ISessionFactory>();
                }
                return _sessionFactory;
            }
        }
    }
}
