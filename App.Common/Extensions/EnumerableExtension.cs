using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Extensions
{
    /// <summary>
    /// Declares extension method of <c>IEnummerable<typeparamref name="T"/></c>
    /// </summary>
    public static class EnumerableExtension
    {
    
        /// <summary>
        /// Executes an action for each element in collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="action">The action.</param>
        public static void Each<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }
    }
}
