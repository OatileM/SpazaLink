using System;
using System.Collections.Generic;
using System.Text;
using SpazaLink.Shared.Enums;

namespace SpazaLink.Shared.Models
{
    /// <summary>
    /// Represents a supplier in the SpazaLink network
    /// Contains supplier information, delivery areas, and business terms
    /// </summary>
    public class Supplier
    {
        /// <summary>
        /// Unique identifier for the supplier
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Business name of the supplier
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Primary contact phone number for the supplier
        /// </summary>
        public string ContactNumber { get; set; } = string.Empty;
        
        /// <summary>
        /// Primary area or region served by this supplier
        /// </summary>
        public string AreaServed { get; set; } = string.Empty;
        
        /// <summary>
        /// Minimum order value required for purchases (in ZAR)
        /// Used to enforce supplier business terms
        /// </summary>
        public decimal MinimumOrderValue { get; set; }
        
        /// <summary>
        /// List of specific areas where this supplier provides delivery
        /// Used for logistics and order routing
        /// </summary>
        public List<string> DeliveryAreas { get; set; } = new();
        
        /// <summary>
        /// Classification of supplier type (Wholesaler, Distributor, Manufacturer, LocalProducer)
        /// Determines business relationship and pricing structure
        /// </summary>
        public SupplierType Type { get; set; }
    }
}
