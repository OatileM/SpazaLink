using System;
using System.Collections.Generic;
using System.Text;

namespace SpazaLink.Shared.Enums
{
    /// <summary>
    /// Defines the different types of suppliers in the SpazaLink network
    /// Used to categorize suppliers based on their business model and scale
    /// </summary>
    public enum SupplierType
    {
        /// <summary>
        /// Large wholesale retailers (e.g., Makro, Metro)
        /// Typically offer bulk pricing and wide product ranges
        /// </summary>
        Wholesaler,
        
        /// <summary>
        /// Brand-specific distributors
        /// Authorized distributors for specific brands or product lines
        /// </summary>
        Distributor,
        
        /// <summary>
        /// Direct from factory suppliers
        /// Manufacturers selling directly to retailers
        /// </summary>
        Manufacturer,
        
        /// <summary>
        /// Small local producers
        /// Local businesses producing goods for the community
        /// </summary>
        LocalProducer
    }
}
