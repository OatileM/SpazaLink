using SpazaLink.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpazaLink.Shared.Models
{
    public class Trader
    {

        // Primary Identifier
        public Guid ID { get; set; } = Guid.NewGuid();

        // Basic Information required for registration
        public string BusinessName { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // Location Information
        public string Area { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Landmark { get; set; } = string.Empty;

        // Business Details
        public TraderType Type { get; set; } = TraderType.SpazaShop;
        public List<string> ProductCategories { get; set; } = new();

        // Status and Verification
        public TraderStatus Status { get; set; } = TraderStatus.PendingVerification;
        public TraderTier Tier { get; set; } = TraderTier.Bronze;
        public bool isVerified { get; set; } = false;
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        // Simplified KYC
        public string? IDNumber { get; set; }

        // Payment Preferences 
        public List<PaymentMethod> AcceptedPaymentMethods { get; set; } = new()
        {
            PaymentMethod.Cash
        };

        // Contact Methods for notification
        public string? WhatsappNumber{ get; set; }
        public string? Email { get; set; }



    }
}
