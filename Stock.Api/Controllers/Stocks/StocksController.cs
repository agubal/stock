using System;
using System.Collections.Generic;
using System.Web.Http;
using Stock.Entities.Calculations;
using Stock.Entities.Common;
using Stock.Entities.Stocks;
using Stock.Services;
using Stock.Services.Stocks;

namespace Stock.Api.Controllers.Stocks
{
    /// <summary>
    /// Api Controller to work with Stock entity
    /// </summary>
    [RoutePrefix("api/stocks")]
    public class StocksController : BaseApiController<StockData, Guid>
    {
        private readonly IService<Calculation, Guid> _calculationService;

        public StocksController(IStockService entityService, IService<Calculation, Guid> calculationService)
            : base(entityService)
        {
            _calculationService = calculationService;
        }

        /// <summary>
        /// Returns Stock's Calculations
        /// </summary>
        /// <param name="stockId">StockData Id</param>
        /// <returns></returns>
        [Route("{stockId}/calculations")]
        public virtual IHttpActionResult GetCalculations(Guid stockId)
        {
            ServiceResult<IEnumerable<Calculation>> serviceResult = _calculationService.Filter(c => c.StockId == stockId);
            return GetErrorResult(serviceResult) ?? Ok(serviceResult.Result);
        }
    }
}
