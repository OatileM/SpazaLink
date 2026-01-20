using System;
using System.Collections.Generic;
using System.Text;

namespace SpazaLink.Shared.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // "Groceries", "Drinks"
        public string SubCategory { get; set; } = string.Empty; // "Soft Drinks", "Canned Food"
        public decimal BasePrice { get; set; }
        public string Unit { get; set; } = string.Empty; // "each", "kg", "case"
        public int UnitsPerCase { get; set; } = 1;
        public Guid SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public int StockLevel { get; set; }
        public int MinimumOrderQuantity { get; set; } = 1;
        public bool IsActive { get; set; } = true;
        public List<string> ImageUrls { get; set; } = new();
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    }
}
