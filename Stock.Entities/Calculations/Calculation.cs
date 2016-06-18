using System;
using Stock.Entities.Common;

namespace Stock.Entities.Calculations
{
    /// <summary>
    /// Calculation entity
    /// </summary>
    public class Calculation : IIdentifier<Guid>
    {
        /// <summary>
        /// Foreign key to Stock entity
        /// </summary>
        public Guid StockId { get; set; }
        
        /// <summary>
        /// Year of calculation
        /// </summary>
        public byte Year { get; set; }
        
        /// <summary>
        /// Calculated value
        /// </summary>
        public decimal Value { get; set; }
        
        /// <summary>
        /// Identity
        /// </summary>
        public Guid Id { get; set; }
    }
}
