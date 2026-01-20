using System;
using System.Collections.Generic;
using System.Text;

namespace SpazaLink.Shared.Models
{
    /// <summary>
    /// Represents a product in the SpazaLink inventory system
    /// Contains all product information including pricing, stock, and supplier details
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Unique identifier for the product
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Display name of the product
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Detailed description of the product
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Primary category classification (e.g., "Groceries", "Beverages")
        /// Used for filtering and organization
        /// </summary>
        public string Category { get; set; } = string.Empty;
        
        /// <summary>
        /// Secondary category for more specific classification (e.g., "Soft Drinks", "Canned Food")
        /// </summary>
        public string SubCategory { get; set; } = string.Empty;
        
        /// <summary>
        /// Base price per unit in South African Rand (ZAR)
        /// </summary>
        public decimal BasePrice { get; set; }
        
        /// <summary>
        /// Unit of measurement for the product (e.g., "each", "kg", "litre")
        /// </summary>
        public string Unit { get; set; } = string.Empty;
        
        /// <summary>
        /// Number of individual units per case/bulk package
        /// Default is 1 for individual items
        /// </summary>
        public int UnitsPerCase { get; set; } = 1;
        
        /// <summary>
        /// Unique identifier of the supplier providing this product
        /// </summary>
        public Guid SupplierId { get; set; }
        
        /// <summary>
        /// Display name of the supplier for quick reference
        /// </summary>
        public string SupplierName { get; set; } = string.Empty;
        
        /// <summary>
        /// Current stock level available for this product
        /// </summary>
        public int StockLevel { get; set; }
        
        /// <summary>
        /// Minimum quantity required for orders
        /// Default is 1 unit
        /// </summary>
        public int MinimumOrderQuantity { get; set; } = 1;
        
        /// <summary>
        /// Indicates if the product is currently available for ordering
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// List of image URLs for product photos
        /// Used for display in mobile apps and web interfaces
        /// </summary>
        public List<string> ImageUrls { get; set; } = new();
        
        /// <summary>
        /// Timestamp of the last update to this product record
        /// Automatically set to current UTC time
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
