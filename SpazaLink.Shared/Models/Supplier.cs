using System;
using System.Collections.Generic;
using System.Text;
using SpazaLink.Shared.Enums;

namespace SpazaLink.Shared.Models
{
    public class Supplier
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string AreaServed { get; set; } = string.Empty;
        public decimal MinimumOrderValue { get; set; }
        public List<string> DeliveryAreas { get; set; } = new();
        public SupplierType Type { get; set; }
    }
}
