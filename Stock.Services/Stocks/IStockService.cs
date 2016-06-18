using System;
using Stock.Entities.Stocks;

namespace Stock.Services.Stocks
{
    /// <summary>
    /// Interface for stock data business logic
    /// </summary>
    public interface IStockService : IService<StockData, Guid>
    {
    }
}
