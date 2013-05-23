using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Data
{
    /// <summary>
    /// An interface of query factory.
    /// </summary>
    public interface IQueryFactory
    {
        /// <summary>
        /// Creates a domain query instance
        /// </summary>
        /// <typeparam name="T">Type of domain query</typeparam>
        /// <returns></returns>
        T Create<T>() where T : IQuery;

        /// <summary>
        /// Releases the specified domain query.
        /// </summary>
        /// <param name="query">The query.</param>
        void Release(IQuery query);
    }
}
