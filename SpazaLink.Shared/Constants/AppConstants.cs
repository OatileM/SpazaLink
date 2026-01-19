using System;
using System.Collections.Generic;
using System.Text;

namespace SpazaLink.Shared.Constants
{
    public static class AppConstants
    {
        // Validation constants
        public const int MaxBusinessNameLength = 200;
        public const int MinBusinessNameLength = 2;
        public const string SouthAfricaPhoneRegex = @"^(\+27|0)[1-9][0-9]{8}$";
        public const string IDNumberRegex = @"^[0-9]{13}$"; // Simple SA ID check

        // Tier thresholds (in Rands)
        public const decimal BronzeMaxTurnover = 5000;
        public const decimal SilverMaxTurnover = 20000;
        public const decimal GoldMaxTurnover = 50000;

        // Default values
        public const int DefaultPageSize = 20;
        public const int MaxProductCategories = 10;

        // Status messages
        public const string VerificationPendingMessage = "Your registration is pending verification";
        public const string VerificationSuccessMessage = "Trader verified successfully";
    }

}
