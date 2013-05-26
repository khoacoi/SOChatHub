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
            var id = GetEntityId(entity);
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

        /// <summary>
        /// Current user logged in to system
        /// </summary>
        private User CurrentUser
        {
            get { return User.Current; }
        }

        /// <summary>
        /// Get Id of Model
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Guid</returns>
        private static Guid GetEntityId(T entity)
        {
            var propertyInfo = entity.GetType().GetProperty("Id");
            if (propertyInfo != null)
            {
                var propertyValue = propertyInfo.GetValue(entity);
                return propertyValue != null ? (Guid)propertyValue : Guid.Empty;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// Get value of propery name is [Entity + Code]
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string GetEntityCode(T entity)
        {
            var type = entity.GetType();
            var propertyInfo = type.GetProperty(type.Name + "Code");            

            if (propertyInfo != null)
            {
                var propertyValue = propertyInfo.GetValue(entity);                
                return propertyValue != null ? propertyValue.ToString() : string.Empty;
            }
            //In case there is no [Entiy + code] column
            var propertyCode = type.GetProperties().FirstOrDefault(e => e.Name.ToLower().Contains("code"));
            if (propertyCode != null)
            {
                var propertyValue = propertyCode.GetValue(entity);
                return propertyValue != null ? propertyValue.ToString() : string.Empty;
            }
            return string.Empty;
        }

        #endregion End Private Methods
    }
}
