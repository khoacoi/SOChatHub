using SharpLite.Domain;
using SharpLite.Domain.DataInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Data
{
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Creates an instant of <c>IRepository</c>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRepository<T> Create<T>() where T : Entity;

        /// <summary>
        /// Creates an instant of <c>IRepositoryWithTypedId</c> for entity with its <c>Guid</c> id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRepositoryWithTypedId<T, Guid> CreateWithGuid<T>() where T : EntityWithTypedId<Guid>;

        /// <summary>
        /// Creates an instance of <c>IRepositoryWithTypedId</c>
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TId">The type of the entity id.</typeparam>
        /// <returns></returns>
        IRepositoryWithTypedId<T, TId> Create<T, TId>() where T : EntityWithTypedId<TId>;

        /// <summary>
        /// Releases the specified repository
        /// </summary>
        /// <param name="anObject">An object.</param>
        void Release(object anObject);
    }
}
