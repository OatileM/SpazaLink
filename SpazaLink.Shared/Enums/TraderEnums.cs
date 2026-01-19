using System;
using System.Collections.Generic;
using System.Text;

namespace SpazaLink.Shared.Enums
{
    // What type of trader?
    public enum TraderType
    {
        SpazaShop,      // Small convenience store
        StreetVendor,   // Mobile/market stall
        TuckShop,       // School/office based
        HomeBusiness,   // Operating from home
        Other
    }

    // Current status
    public enum TraderStatus
    {
        PendingVerification,    // Just registered
        Active,                 // Verified and active
        Suspended,              // Temporary suspension
        Inactive,               // Not operating
        Banned                  // Permanently removed
    }

    // Tier based on usage/volume (for future features)
    public enum TraderTier
    {
        Bronze,     // New, < R5k monthly turnover
        Silver,     // R5k - R20k monthly
        Gold,       // R20k - R50k monthly
        Platinum    // > R50k monthly or top 10%
    }

    // Payment methods (expand later)
    public enum PaymentMethod
    {
        Cash,
        QRCode,         // SnapScan, Zapper, etc.
        MobileMoney,    // M-Pesa, etc.
        Card,           // Card machine
        CashOnDelivery,
        EFT             // Electronic transfer
    }

    // For notifications
    public enum NotificationChannel
    {
        SMS,
        WhatsApp,
        Email,
        Push            // Mobile app later
    }
}
