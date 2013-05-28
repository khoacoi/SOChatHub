using NHibernate;
using NHibernate.Linq;
using SharpLite.Domain.DataInterfaces;
using SharpLite.NHibernateProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Common.Security.Authentication;

namespace App.Core
{
    public class RepositoryWithTypedId<T, TId> : IRepositoryWithTypedId<T, TId> where T : class
    {
        private readonly ISessionFactory _sessionFactory;

        protected virtual ISession Session
        {
            get
            {
                if (this._sessionFactory.GetCurrentSession() == null)
                    throw new Exception("Session has been null");
                return this._sessionFactory.GetCurrentSession();
            }
        }

        public virtual IDbContext DbContext
        {
            get
            {
                return (IDbContext)new DbContext(this._sessionFactory);
            }
        }

        public RepositoryWithTypedId(ISessionFactory sessionFactory)
        {
            if (sessionFactory == null)
                throw new ArgumentNullException("sessionFactory may not be null");
            this._sessionFactory = sessionFactory;
        }

        public virtual T Get(TId id)
        {
            return this.Session.Get<T>((object)id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return LinqExtensionMethods.Query<T>(this.Session);
        }

        public virtual T SaveOrUpdate(T entity)
        {
            this.Session.SaveOrUpdate((object)entity);

            this.Session.Flush();
            return entity;
        }

        public virtual void Delete(T entity)
        {
            this.Session.Delete((object)entity);
            this.Session.Flush();
        }

        #region Private Methods

        #endregion End Private Methods

    }
}
