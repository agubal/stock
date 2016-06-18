using System;
using System.Collections.Generic;
using Stock.Entities.Common;

namespace Stock.Services
{
    /// <summary>
    /// Generic interface for App Business logic
    /// </summary>
    /// <typeparam name="T">Type of domain entity</typeparam>
    /// <typeparam name="TIdentity">Type of identity of domain entity</typeparam>
    public interface IService<T, in TIdentity> where T : class, IIdentifier<TIdentity>
    {
        /// <summary>
        /// Find entity by Id
        /// </summary>
        /// <param name="key">Id</param>
        /// <returns></returns>
        ServiceResult<T> FindByKey(TIdentity key);
        
        /// <summary>
        /// Filter entities by conition
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns></returns>
        ServiceResult<IEnumerable<T>> Filter(Func<T, bool> predicate);
        
        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns></returns>
        ServiceResult<IEnumerable<T>> GetAll();
        
        /// <summary>
        /// Create new entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        ServiceResult<T> Create(T entity);    
        
        /// <summary>
        /// Bulk create entities
        /// </summary>
        /// <param name="entities">Entities to create</param>
        /// <returns></returns>
        ServiceResult<IEnumerable<T>> Create(params T[] entities);

        /// <summary>
        /// delete entity by Id
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ServiceResult Delete(TIdentity key);
    }
}
