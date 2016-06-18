using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Stock.DataAccess;
using Stock.Entities.Calculations;
using Stock.Entities.Common;
using Stock.Entities.Stocks;

namespace Stock.Services.Stocks
{
    /// <summary>
    /// Service to extend stock data business logic
    /// </summary>
    public class StockService : GenericService<StockData, Guid>, IStockService
    {
        private readonly IService<Calculation, Guid> _calculationService; 

        public StockService(IRepository<StockData, Guid> entityRepository, IService<Calculation, Guid> calculationService) 
            : base(entityRepository)
        {
            _calculationService = calculationService;
        }

        /// <summary>
        /// Create new Stock data, calculate stock productivity result
        /// </summary>
        /// <param name="entity">Stock data to create</param>
        /// <returns></returns>
        public override ServiceResult<StockData> Create(StockData entity)
        {
            //Validate input:
            ServiceResult<StockData> validationResult = ValidateInput(entity);
            if (!validationResult.Succeeded) return validationResult;

            entity.Id = Guid.NewGuid();
            ServiceResult<StockData> createResult = base.Create(entity);
            if (!createResult.Succeeded) return createResult;

            //Total current value
            decimal currentPrincipal = entity.Price * entity.Quantity;

            //Collection with Calculation objects with 0 year data
            var calculations = new List<Calculation>
            {
                new Calculation
                {
                    Id = Guid.NewGuid(),
                    StockId = entity.Id,
                    Value = currentPrincipal,
                    Year = 0
                }
            };

            //Calculate per year:
            for (byte i = 1; i <= entity.Years; i++)
            {
                decimal interestIncome = Math.Round(currentPrincipal*entity.Percentage/100, 2);
                currentPrincipal += interestIncome;
                var calculation = new Calculation
                {
                    StockId = entity.Id,
                    Id = Guid.NewGuid(),
                    Year = i,
                    Value = currentPrincipal
                };
                calculations.Add(calculation);
            }

            ServiceResult<IEnumerable<Calculation>> calcSaveResult = _calculationService.Create(calculations.ToArray());
            if (!calcSaveResult.Succeeded)
            {
                //Remove created Stock:
                Delete(entity.Id);

                //Retern error:
                return new ServiceResult<StockData>(calcSaveResult.Errors);
            }

            entity.Calculations = calculations.ToArray();
            return new ServiceResult<StockData>(entity);
        }

        /// <summary>
        /// Validate client input
        /// </summary>
        /// <param name="entity">Stock data to validate</param>
        /// <returns></returns>
        private ServiceResult<StockData> ValidateInput(StockData entity)
        {
            if(entity == null) return new ServiceResult<StockData>("Stock data was not provided");
            var errors = new List<string>();
            if(string.IsNullOrWhiteSpace(entity.Name)) errors.Add("Stock Name was not provided");
            if(entity.Price == default (decimal)) errors.Add("Price is invalid or was not provided");
            if (entity.Price < 0) errors.Add("Price should have positive value");
            if (entity.Percentage == default(decimal)) errors.Add("Percentage is invalid or was not provided");
            if (entity.Percentage < 0) errors.Add("Percentage should have positive value");
            if (entity.Quantity == default(int)) errors.Add("Quantity is invalid or was not provided");
            if (entity.Quantity < 0) errors.Add("Quantity should have positive value");
            if (entity.Years == default(int)) errors.Add("Years is invalid or was not provided");
            if (entity.Years < 0) errors.Add("Years should have positive value");
            int yearsMaxValue;
            if (int.TryParse(ConfigurationManager.AppSettings["Years.MaxValue"], out yearsMaxValue))
            {
                if (entity.Years > yearsMaxValue) 
                    errors.Add(string.Format("Maximum vakue for Years is {0}", yearsMaxValue));
            }
            return errors.Any() ? new ServiceResult<StockData>(errors) : new ServiceResult<StockData>(entity);
        }
    }
}
