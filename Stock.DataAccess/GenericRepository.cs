using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Stock.Entities.Common;

namespace Stock.DataAccess
{
    /// <summary>
    /// Generic IRepository interface
    /// </summary>
    /// <typeparam name="T">Type of repository</typeparam>
    /// <typeparam name="TIdentity">Type of identity property of</typeparam>
    public class GenericRepository<T, TIdentity> : IRepository<T, TIdentity> where T : class, IIdentifier<TIdentity>
    {
        private string _tableName;   

        private string _entityTablePath;
        
        /// <summary>
        /// Table to store the entity. In current implementation - JSON File name
        /// </summary>
        private string TableName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_tableName))
                    _tableName = typeof(T).Name;
                return _tableName;
            }
        }
        
        /// <summary>
        /// Path to JSON file which stores entity data
        /// </summary>
        private string EntityTablePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_entityTablePath))
                {
                    _entityTablePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        ConfigurationManager.AppSettings["Database"], string.Format("{0}.txt", TableName));
                    if (!File.Exists(_entityTablePath)) File.WriteAllText(_entityTablePath, string.Empty);
                }
                return _entityTablePath;
            }
        }

        /// <summary>
        /// Get all T
        /// </summary>
        /// <returns>Collection of T</returns>
        public IEnumerable<T> All()
        {
            string json = File.ReadAllText(EntityTablePath);
            List<T> entities = JsonConvert.DeserializeObject<List<T>>(json);
            return entities ?? new List<T>();
        }

        /// <summary>
        /// Find by Key
        /// </summary>
        /// <param name="key">Id</param>
        /// <returns></returns>
        public T FindByKey(TIdentity key)
        {
            IEnumerable<T> entities = All();
            if (entities == null) return null;
            return entities.FirstOrDefault(e => e.Id.Equals(key));
        }

        /// <summary>
        /// Return collection by condition
        /// </summary>
        /// <param name="predicate">The condition</param>
        /// <returns>Collection of T</returns>
        public IEnumerable<T> Filter(Func<T, bool> predicate)
        {
            return All().Where(predicate);            
        }

        /// <summary>
        /// Create new Entity
        /// </summary>
        /// <param name="entities">Entities to create</param>
        /// <returns></returns>
        public bool Create(params T[] entities)
        {
            List<T> allEntities = All().ToList();
            allEntities.AddRange(entities);
            string json = JsonConvert.SerializeObject(allEntities);
            File.WriteAllText(EntityTablePath, json);
            return true;
        }

        /// <summary>
        /// Delete entity by Id
        /// </summary>
        /// <param name="key">Id</param>
        public void Delete(TIdentity key)
        {
            List<T> allEntities = All().ToList();
            allEntities = allEntities.Where(e => !e.Id.Equals(key)).ToList();
            string json = JsonConvert.SerializeObject(allEntities);
            File.WriteAllText(EntityTablePath, json);
        }
    }
}
