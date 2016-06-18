using System.Collections.Generic;
using System.Web.Http;
using Stock.Entities.Common;
using Stock.Services;

namespace Stock.Api.Controllers
{
    /// <summary>
    /// Base API controller
    /// </summary>
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// Prepares error result if any
        /// </summary>
        /// <param name="result">Response from BLL layer to parse</param>
        /// <returns></returns>
        protected IHttpActionResult GetErrorResult(ServiceResult result)
        {
            if (result == null) return InternalServerError();
            if (result.Succeeded) return null;
            string errorMessage = result.Errors != null ? string.Join(",", result.Errors) : "Service error";
            return BadRequest(errorMessage);
        }
    }

    /// <summary>
    /// Generic Base API Contrller
    /// </summary>
    /// <typeparam name="T">Type to work with</typeparam>
    /// <typeparam name="TIdentity">Identity Type of T</typeparam>
    public class BaseApiController<T, TIdentity> : BaseApiController where T : class, IIdentifier<TIdentity>
    {
        protected readonly IService<T, TIdentity> EntityService;
        protected BaseApiController(IService<T, TIdentity> entityService)
        {
            EntityService = entityService;
        }

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns></returns>
        public virtual IHttpActionResult Get()
        {
            ServiceResult<IEnumerable<T>> serviceResult = EntityService.GetAll();
            return GetErrorResult(serviceResult) ?? Ok(serviceResult.Result);
        }

        /// <summary>
        /// Get entity by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public virtual IHttpActionResult Get(TIdentity id)
        {
            ServiceResult<T> serviceResult = EntityService.FindByKey(id);
            return GetErrorResult(serviceResult) ?? Ok(serviceResult.Result);
        }

        /// <summary>
        /// Create entity
        /// </summary>
        /// <param name="entity">Entity to create</param>
        /// <returns></returns>
        public virtual IHttpActionResult Post(T entity)
        {
            ServiceResult<T> serviceResult = EntityService.Create(entity);
            return GetErrorResult(serviceResult) ?? Ok(serviceResult.Result);
        }
    }
}
