using System;
using Stock.Entities.Calculations;
using Stock.Entities.Common;

namespace Stock.Entities.Stocks
{
    /// <summary>
    /// Stock entity
    /// </summary>
    public class StockData : IIdentifier<Guid>
    {
        /// <summary>
        /// Identity
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Stock name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Price per share
        /// </summary>
        public decimal Price { get; set; }
        
        /// <summary>
        /// Quntity of shares
        /// </summary>
        public int Quantity { get; set; }
        
        /// <summary>
        /// Interest rate
        /// </summary>
        public decimal Percentage { get; set; }
        
        /// <summary>
        /// Years to hold
        /// </summary>
        public int Years { get; set; }
        
        /// <summary>
        /// Calculated stock productivity results
        /// </summary>
        public Calculation[] Calculations { get; set; }
    }
}
