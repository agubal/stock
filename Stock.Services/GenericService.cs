using System;
using System.Collections.Generic;
using Stock.DataAccess;
using Stock.Entities.Common;

namespace Stock.Services
{
    /// <summary>
    /// Generic service for App Business logic
    /// </summary>
    /// <typeparam name="T">Type of domain entity</typeparam>
    /// <typeparam name="TIdentity">Type of identity of domain entity</typeparam>
    public class GenericService<T, TIdentity> : IService<T, TIdentity> where T : class, IIdentifier<TIdentity>
    {
        private readonly IRepository<T, TIdentity> _entityRepository;

        public GenericService(IRepository<T, TIdentity> entityRepository)
        {
            _entityRepository = entityRepository;
        }

        /// <summary>
        /// Find entity by Id
        /// </summary>
        /// <param name="key">Id</param>
        /// <returns></returns>
        public virtual ServiceResult<T> FindByKey(TIdentity key)
        {
            T entity = _entityRepository.FindByKey(key);
            return new ServiceResult<T>(entity);
        }

        /// <summary>
        /// Filter entities by conition
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns></returns>
        public virtual ServiceResult<IEnumerable<T>> Filter(Func<T, bool> predicate)
        {
            IEnumerable<T> entities = _entityRepository.Filter(predicate);
            return new ServiceResult<IEnumerable<T>>(entities);

        }

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns></returns>
        public virtual ServiceResult<IEnumerable<T>> GetAll()
        {
            IEnumerable<T> entities = _entityRepository.All();
            return new ServiceResult<IEnumerable<T>>(entities);

        }

        /// <summary>
        /// Create new entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual ServiceResult<T> Create(T entity)
        {
            bool isCreated = _entityRepository.Create(entity);
            return isCreated ? new ServiceResult<T>(entity) : new ServiceResult<T>("Failed to create entity");
        }

        /// <summary>
        /// Bulk create entities
        /// </summary>
        /// <param name="entities">Entities to create</param>
        /// <returns></returns>
        public ServiceResult<IEnumerable<T>> Create(params T[] entities)
        {
            _entityRepository.Create(entities);
            return new ServiceResult<IEnumerable<T>>(entities);
        }

        /// <summary>
        /// Delete entity by Id
        /// </summary>
        /// <param name="key">Id</param>
        /// <returns></returns>
        public ServiceResult Delete(TIdentity key)
        {
            _entityRepository.Delete(key);
            return new ServiceResult();
        }
    }
}
