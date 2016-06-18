using System;
using System.Collections.Generic;
using Stock.Entities.Common;

namespace Stock.DataAccess
{
    /// <summary>
    /// Generic IRepository interface
    /// </summary>
    /// <typeparam name="T">Type of repository</typeparam>
    /// <typeparam name="TIdentity">Type of identity property of</typeparam>
    public interface IRepository<T, in TIdentity> where T : IIdentifier<TIdentity>
    {
        /// <summary>
        /// Get all
        /// </summary>
        /// <returns>Collection of T</returns>
        IEnumerable<T> All();

        /// <summary>
        /// Find by Key
        /// </summary>
        /// <param name="key">Id</param>
        /// <returns></returns>
        T FindByKey(TIdentity key);

        /// <summary>
        /// Returns collection by condition
        /// </summary>
        /// <param name="predicate">The condition</param>
        /// <returns>Collection of T</returns>
        IEnumerable<T> Filter(Func<T, bool> predicate);

        /// <summary>
        /// Creates new Entity
        /// </summary>
        /// <param name="entities">Entities to create</param>
        /// <returns></returns>
        bool Create(params T[] entities);

        /// <summary>
        /// Delete entity by Id
        /// </summary>
        /// <param name="key">Id</param>
        void Delete(TIdentity key);
    }
}
